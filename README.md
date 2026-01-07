# NanoProxy

NanoProxy is a lightweight .NET library for generating runtime proxies that intercept property setters. It enables scenarios such as change tracking, validation, and logging without modifying the original class code.

## Features

- Dynamic proxy generation using `System.Reflection.Emit`
- Intercepts property setters and exposes old/new values and property name
- Supports both value types, nullable types and strings
- Thread-safe proxy type caching
- Compatible with .NET 8

## Getting Started

### Installation

Add NanoProxy to your solution as a project reference or include the source files in your project.

### Usage Example

Suppose you have a class:
```csharp
public class TestClass { 
public virtual int IntegerProperty { get; set; } 
public virtual int? NullableIntegerProperty { get; set; } 
public virtual string StringProperty { get; set; } }
```

You can create a proxy and intercept property changes:
````csharp
var builder = new NanoProxy.NanoProxyBuilder(); var proxy = builder.CreateProxy();
proxy.SetInterceptor = (value, oldValue, propertyName) => { Console.WriteLine($"Property '{propertyName}' changed from '{oldValue}' to '{value}'"); };
proxy.WrapedObject.IntegerProperty = 42; // Interceptor is called 
proxy.WrapedObject.NullableIntegerProperty = null; // Interceptor is called
````

## Testing

NanoProxy includes NUnit-based tests. To run them:

1. Open the solution in Visual Studio 2022.
2. Build the solution.
3. Use Test Explorer to run all tests.

## Requirements

- .NET 8
- Visual Studio 2022 or compatible IDE

## License

This project is licensed under the [GNU General Public License v2.0](LICENSE).
