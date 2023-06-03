using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NanoProxy
{
    public class SomeClass
    {
        private string _property;

        public SomeClass()
        {
        }

        public virtual string Property
        {
            get { return _property; }
            set { _property = value; }
        }
    }

    public class SomeOtherClass : SomeClass
    {
        private readonly Action<object, object, string> setterInterceptor;

        public SomeOtherClass(Action<object, object, string> setterInterceptor)
        {
            this.setterInterceptor = setterInterceptor;
        }

        public void Method(object value)
        {
        }

        public override string Property
        {
            get
            {
                return base.Property;
            }
            set
            {
                setterInterceptor.Invoke(value, base.Property, "name");
                base.Property = value;
            }
        }
    }

    public class Wrapper
    {
        public SomeClass Instance { get; set; }
    }
}
