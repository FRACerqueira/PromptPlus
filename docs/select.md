# PromptPlus # Select
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**Select Options**](selectoptions) |
[**BasePromptOptions**](basepromptoptions)

## Documentation
Control Select. Generic select input IEnumerable/Enum with auto-paginator and tooltips and more.

![](./images/Select.gif)

### Options

[**Select Options**](selectoptions)

### Syntax
[**Top**](#promptplus--select)

```csharp
Select<T>(SelectOptions<T> options, CancellationToken? cancellationToken = null)
Select<T>(Action<SelectOptions<T>> configure, CancellationToken? cancellationToken = null)
```

```csharp
//Note : for enum values , Where T is type from enum
Select<T>(string message, T? defaultValue = null, int? pageSize = null, CancellationToken? cancellationToken = null)
//Note : for  IEnumerable type
Select<T>(string message, IEnumerable<T> items, object defaultValue = null, int? pageSize = null, Func<T, string> textSelector = null, CancellationToken? cancellationToken = null)
```

**Highlighted parameters**
- pageSize = maximum items per page. Tf the value is null, the value will be calculated according to the screen size 
- textSelector (for  IEnumerable type)= Function that returns the string that will be displayed. Tf the value is null,the value will be item => item.ToString()
- textSelector (for  enum values)= Function that returns the string that will be displayed. Tf the value is null,the value will be the \[Display\] attribute if it exists or an enum string.

### Return
[**Top**](#promptplus--select)

```csharp
//for  IEnumerable type
ResultPPlus<T> 
//for  enum values
ResultPPlus<EnumValue<T>>
```

### Sample
[**Top**](#promptplus--select)

```csharp
var city = PPlus.Select("Select your city", 
             new[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" }, 
             pageSize: 3, cancellationToken: _stopApp);
if (city.IsAborted)
{
   return;
}
if (!string.IsNullOrEmpty(city.Value))
{
   Console.WriteLine($"Hello, {city.Value}!");
}
else
{
   Console.WriteLine("You chose nothing!");
}
```

```csharp
var envalue = PPlus.Select<MyEnum>("Select enum value", defaultValue: MyEnum.Foo, cancellationToken: _stopApp);
if (envalue.IsAborted)
{
   return;
}
Console.WriteLine($"You selected {envalue.Value.DisplayName}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**Select Options**](selectoptions) |
[**BasePromptOptions**](basepromptoptions)
