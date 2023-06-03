using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace NanoProxy
{
    /// <summary>Initilize proxy.</summary>
    public class NanoProxyBuilder
    {
        private const string AssemblyStringName = "DynamicNanoProxy";
        private static IDictionary<Type, Type> _proxyCache = new Dictionary<Type, Type>();
        private readonly static AssemblyName AssemblyName = new AssemblyName(AssemblyStringName);
        private readonly static AssemblyBuilder AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run);
        private readonly static ModuleBuilder ModuleBuilder = AssemblyBuilder.DefineDynamicModule(AssemblyName.Name);
        private readonly static object Lock = new object();

        public NanoProxyBuilder()
        {
        }

        public NanoProxy<T> CreateProxy<T>() where T : class, new()
        {
            Type proxyType;
            lock (Lock)
            {
                if (!_proxyCache.TryGetValue(typeof(T), out proxyType))
                {
                    var typeBuilder = ModuleBuilder.DefineType($"NanoProxyOf_{typeof(T).Name}", TypeAttributes.Class | TypeAttributes.Public, typeof(T));
                    typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

                    var fieldBuilder = typeBuilder.DefineField("_setterInterceptor", typeof(SetInterceptor), FieldAttributes.Private);
                    var ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(InternalSetInterceptor) });
                    var ctorIl = ctorBuilder.GetILGenerator();
                    ctorIl.Emit(OpCodes.Ldarg_0);
                    var superConstructor = typeof(T).GetConstructor(Array.Empty<Type>());
                    ctorIl.Emit(OpCodes.Call, superConstructor);
                    ctorIl.Emit(OpCodes.Ldarg_0);
                    ctorIl.Emit(OpCodes.Ldarg_1);
                    ctorIl.Emit(OpCodes.Stfld, fieldBuilder);
                    ctorIl.Emit(OpCodes.Ret);
                    OverrideProperties<T>(typeBuilder, fieldBuilder);
                    var generatedType = typeBuilder.CreateType();

                    proxyType = _proxyCache[typeof(T)] = generatedType;
                }
            }

            var result = new NanoProxy<T>();
            result.WrapedObject = (T)proxyType.GetConstructor(new Type[] { typeof(InternalSetInterceptor) })
                .Invoke(new object[] { (InternalSetInterceptor)((value, oldValue, property) => result.InternalSetInterceptor(value, oldValue, property)) });
            return result;
        }

        private void OverrideProperties<T>(TypeBuilder typeBuilder, FieldBuilder setterInterceptor) where T : class, new()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(_ => _.GetGetMethod().IsVirtual);
            foreach (var property in properties)
            {
                var returnType = property.PropertyType;
                MethodAttributes getSetAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual;
                MethodBuilder valuePropertyGet = typeBuilder.DefineMethod($"get_{property.Name}", getSetAttributes, returnType, Type.EmptyTypes);
                ILGenerator Generator = valuePropertyGet.GetILGenerator();
                Generator.Emit(OpCodes.Ldarg_0);
                Generator.Emit(OpCodes.Call, property.GetGetMethod());
                Generator.Emit(OpCodes.Ret);
                MethodBuilder ValuePropertySet = typeBuilder.DefineMethod($"set_{property.Name}", getSetAttributes, null, new Type[] { returnType });
                Generator = ValuePropertySet.GetILGenerator();
                Generator.Emit(OpCodes.Nop);
                Generator.Emit(OpCodes.Ldarg_0);
                Generator.Emit(OpCodes.Ldfld, setterInterceptor);
                Generator.Emit(OpCodes.Ldarg_1);
                Generator.Emit(OpCodes.Ldarg_0);
                Generator.Emit(OpCodes.Call, property.GetGetMethod());
                Generator.Emit(OpCodes.Ldstr, property.Name);
                Generator.Emit(OpCodes.Callvirt, typeof(SetInterceptor).GetMethod("Invoke"));
                Generator.Emit(OpCodes.Nop);
                Generator.Emit(OpCodes.Ldarg_0);
                Generator.Emit(OpCodes.Ldarg_1);
                var prop = property.GetSetMethod();
                Generator.Emit(OpCodes.Call, property.GetSetMethod());
                Generator.Emit(OpCodes.Ret);
            }
        }
    }
}
