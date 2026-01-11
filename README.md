# NanoProxy

[![Nuget](https://img.shields.io/nuget/v/NanoProxy?color=1182c2&logo=nuget)](https://www.nuget.org/packages/NanoProxy/)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)

NanoProxy is a lightweight, high-performance .NET library for generating runtime proxies that intercept property setters. It enables scenarios such as change tracking, validation, logging, and auditing without modifying your original class code.

## Features

- **Dynamic Proxy Generation** - Uses `System.Reflection.Emit` for efficient runtime proxy creation
- **Property Setter Interception** - Captures both old and new values on property changes
- **Type Support** - Works with value types, nullable types, and reference types (strings, objects)
- **Thread-Safe** - Built-in thread-safe proxy type caching for optimal performance
- **Zero Dependencies** - Lightweight library with no external dependencies
- **Virtual Property Override** - Leverages inheritance to intercept virtual property setters
- **.NET 8 Compatible** - Built for modern .NET applications

## Installation

Install NanoProxy via NuGet Package Manager:
```bash
dotnet add package NanoProxy
```

## Usage Example

Suppose you have a class (currently parameterless constructor is required):
```csharp
public class TestClass 
{ 
    public virtual int IntegerProperty { get; set; } 
    public virtual int? NullableIntegerProperty { get; set; } 
    public virtual string StringProperty { get; set; } 
}
```

You can create a proxy and intercept virtual property changes:
```csharp
var builder = new NanoProxy.NanoProxyBuilder();
var proxy = builder.CreateProxy<TestClass>();
proxy.Interceptor = (value, oldValue, propertyInfo) => 
{
    Console.WriteLine($"Property '{propertyInfo}' changed from '{oldValue}' to '{value}'"); 
};
proxy.WrapedObject.NullableIntegerProperty = 42; // Property 'NullableIntegerProperty' changed from '' to '42' 
proxy.WrapedObject.NullableIntegerProperty = 2; /// Property 'NullableIntegerProperty' changed from '42' to '2' 
```

Where interceptor is defined as:
```csharp
public delegate void SetInterceptor(object value, object oldValue, PropertyInfo propertyInfo);
```

## Testing

NanoProxy includes NUnit-based tests. To run them:

1. Open the solution in Visual Studio 2022.
2. Build the solution.
3. Use Test Explorer to run all tests.

## Requirements

- .NET 8.0 or later
- Visual Studio 2022 or compatible IDE

## License

This project is licensed under the [GNU General Public License v2.0](LICENSE).
