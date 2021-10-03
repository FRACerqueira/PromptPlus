# PromptPlus # MaskEditNumber
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**ResultMasked**](resultmasked)

## Documentation
Control MaskEdit. Numeric/Currency input with language parameter, tooltips and input validator.

![](./images/MaskEditNumber.gif)
![](./images/MaskEditCurrency.gif)

### Options
Not have options

### Shortcuts

- If the Decimal Separator is typed, the cursor advances to part decimal

### Syntax
[**Top**](#promptplus--maskeditnumber)

```csharp
MaskEdit(MaskedNumberType masktype, string message, int ammoutInteger, int ammoutDecimal, double? defaultValue = null, CultureInfo cultureinfo = null, MaskedSignal signal = MaskedSignal.None, bool showtypeinput = true, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
```

**Highlighted parameters**
- masktype = Must be MaskTypeNumber or MaskTypeCurrency
- defaultValue = Initial value
- cultureinfo = Language/Culture of date. If null value, culture is DefaultCulture of PromptPlus
- ammoutInteger = Amount of integers
- ammoutDecimal = Amount of decimals
- signal = Accept signal. MaskedSignal.None = only positive number
- showtypeinput = Show tooptip of type input 
- enabledAbortKey = Enabled/Disabled Hotkey AbortKeyPress
- enabledAbortAllPipes = Enabled/Disabled Hotkey AbortAllPipesKeyPress
- enabledPromptTooltip = Enabled/disabled control´s tootip
- hideAfterFinish = Hide result after finish

### Return
[**Top**](#promptplus--maskeditnumber)

```csharp
ResultPPlus<ResultMasked>
```

### Sample
[**Top**](#promptplus--maskeditnumber)

```csharp
 var mask = PPlus.MaskEdit(PPlus.MaskTypeNumber,"Number", 5, 2, null
                    new CultureInfo("fr-FR"),MaskedSignal.Enabled, cancellationToken: _stopApp);
if (string.IsNullOrEmpty(mask.Value.Input))
{
    Console.WriteLine($"your input was empty!");
}
else
{
    Console.WriteLine($"your input was {mask.Value.ObjectValue}!");
}
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**ResultMasked**](resultmasked)
