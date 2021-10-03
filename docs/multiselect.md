# PromptPlus # MultiSelect
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**MultiSelect Options**](multiselectoptions) |
[**BasePromptOptions**](basepromptoptions)

## Documentation
Control MultSelect. Generic multi select input IEnumerable/Enum with auto-paginator , tooltips and more.

![](./images/MultSelect.gif)

### Options

[**MultiSelect Options**](multiselectoptions)

### Syntax
[**Top**](#promptplus--multiselect)

```csharp
MultiSelect<T>(MultiSelectOptions<T> options, CancellationToken? cancellationToken = null)
MultiSelect<T>(Action<MultiSelectOptions<T>> configure, CancellationToken? cancellationToken = null)
```

```csharp
//Note : for enum values , Where T is type from enum
MultiSelect<T>(string message, int minimum = 1, int maximum = -1, IEnumerable<T> defaultValues = null, int? pageSize = null, CancellationToken? cancellationToken = null)
//Note : for  IEnumerable type
MultiSelect<T>(string message, IEnumerable<T> items, int minimum = 1, int maximum = -1, IEnumerable<T> defaultValues = null, int? pageSize = null, Func<T, string> textSelector = null, CancellationToken? cancellationToken = null)
```

**Highlighted parameters**
- pageSize = maximum item per page. Tf the value is null, the value will be calculated according to the screen size 
- minimum = minimum items selected
- maximum = maximum items selected
- defaultValues = initial IEnumerable items seleted
- textSelector (for  IEnumerable type)= Function that returns the string that will be displayed. Tf the value is null,the value will be item => item.ToString()
- textSelector (for  enum values)= Function that returns the string that will be displayed. Tf the value is null,the value will be the \[Display\] attribute if it exists or an enum string.

### Return
[**Top**](#promptplus--multiselect)

```csharp
//for  IEnumerable type
ResultPPlus<IEnumerable<T>> 
//for  enum values
ResultPPlus<IEnumerable<EnumValue<T>>>
```

### Sample
[**Top**](#promptplus--multiselect)

```csharp
var options = PPlus.MultiSelect("Which cities would you like to visit?", 
                new[] { "Seattle", "London", "Tokyo", "New York", "Singapore", "Shanghai" }, 
                pageSize: 3, 
                defaultValues: new[] { "Tokyo" }, 
                minimum: 0, 
                cancellationToken: _stopApp);
if (options.IsAborted)
{
   return;
}
if (options.Value.Any())
{
   Console.WriteLine($"You picked {string.Join(", ", options.Value)}");
}
else
{
   Console.WriteLine("You chose nothing!");
}
```

```csharp
var multvalue = PPlus.MultiSelect("Select enum value", 
                   defaultValues: new[] { MyEnum.Foo,MyEnum.Bar }, 
                   cancellationToken: _stopApp);
if (multvalue.IsAborted)
{
   return;
}
Console.WriteLine($"You picked {string.Join(", ", multvalue.Value.Select(x => x.DisplayName))}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**MultiSelect Options**](multiselectoptions) |
[**BasePromptOptions**](basepromptoptions)
