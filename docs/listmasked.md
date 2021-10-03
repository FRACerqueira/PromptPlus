# PromptPlus # ListMasked
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus)

## Documentation
Control ListMasked. Create generic IEnumerable with masked input, auto-paginator, tooptip , input validator

![](./images/MaskedList.gif)

### Options

Not have options

### Syntax
[**Top**](#promptplus--listmasked)

```csharp
ListMasked<T>(string message, string maskValue, int minimum = 0, int maximum = int.MaxValue, bool uppercase = false, bool showInputType = true, bool fillzeros = false, CultureInfo? cultureInfo = null, int? pageSize = null, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
```

**Highlighted parameters**
- pageSize = maximum item per page. Tf the value is null, the value will be calculated according to the screen size 
- minimum = minimum items selected
- maximum = maximum items selected
- allowduplicate = accept duplicate items
- uppercase = input to upercase
- cultureinfo = Language/Culture of date. If null value, culture is DefaultCulture of PromptPlus.
- fillzeros = fill input numeric type with zeros
- showtypeinput = show tooptip of type input 
- EnabledAbortKey = Enabled/Disabled Hotkey AbortKeyPress
- EnabledAbortAllPipes = Enabled/Disabled Hotkey AbortAllPipesKeyPress
- EnabledPromptTooltip = Enabled/disabled control´s tootip
- HideAfterFinish = Hide result after finish

### Return
[**Top**](#promptplus--listmasked)

```csharp
ResultPPlus<IEnumerable<T>>
```

### Sample
[**Top**](#promptplus--listmasked)


```csharp
var lst = PPlus.ListMasked<string>("Please add item(s)", 
            @"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}", 
            uppercase: true, cancellationToken: _stopApp);
if (lst.IsAborted)
{
   return;
}
Console.WriteLine($"You picked {string.Join(", ", lst.Value)}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus)
