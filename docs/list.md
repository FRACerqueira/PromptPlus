# PromptPlus # List
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**List Options**](listoptions) |
[**BaseOptions**](baseoptions)

## Documentation
Control List. Create Generic IEnumerable with auto-paginator, tooptip , input validator, message error by type/format and more.

![](./images/List.gif)

### Options

[**List Options**](listoptions)

### Syntax
[**Top**](#promptplus--list)

```csharp
List<T>(ListOptions<T> options, CancellationToken? cancellationToken = null)
List<T>(Action<ListOptions<T>> configure, CancellationToken? cancellationToken = null)
```

```csharp
List<T>(string message, int minimum = 1, int maximum = -1, int? pageSize = null, bool uppercase = false, bool allowduplicate = true, IList<Func<object, ValidationResult>> validators = null, CancellationToken? cancellationToken = null)
```

**Highlighted parameters**
- pageSize = maximum item per page. Tf the value is null, the value will be calculated according to the screen size 
- minimum = minimum items selected
- maximum = maximum items selected
- allowduplicate = accept duplicate items
- uppercase = input to upercase

### Return
[**Top**](#promptplus--list)

```csharp
ResultPPlus<IEnumerable<T>>
```

### Sample
[**Top**](#promptplus--list)

```csharp
var lst = PPlus.List<string>("Please add item(s)", uppercase: true, cancellationToken: _stopApp);
if (lst.IsAborted)
{
   return;
}
Console.WriteLine($"You picked {string.Join(", ", lst.Value)}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**List Options**](listoptions) |
[**BaseOptions**](baseoptions)
