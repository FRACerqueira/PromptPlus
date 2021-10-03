# PromptPlus # MaskEditGeneric
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**ResultMasked**](resultmasked) |
[**BasePromptOptions**](basepromptoptions)


## Documentation
Control MaskEdit. Generic input with masked input , tooltips and input validator

![](./images/MaskEditGeneric.gif)

### Options
Not have options

### Syntax
[**Top**](#promptplus--maskeditgeneric)

**Mask Characters and Delimiters**

 - 9 : Only a numeric character
 - L : Only a letter 
 - C : OnlyCustom character 
 - A : Any character
 - N : OnlyCustom character +  Only a numeric character
 - X : OnlyCustom character +  Only a letter

 - \ : Escape character
 - { : Initial delimiter for repetition of masks
 - } : Final delimiter for repetition of masks
 - \[ : Initial delimiter for list of Custom character
 - \] : Final delimiter for list of Custom character

**_Examples:_**

- 9999999 = Seven numeric characters
- 9{7} = Seven numeric characters
- 99\/99 = Four numeric characters separated in the middle by a "/"
- C{2}\[ABC\] = two characters with ony A,B,C valid char.

```csharp
MaskEdit(MaskedGenericType masktype, string message, string maskedvalue, string defaultValue = null, bool upperCase = true, bool showtypeinput = true, IList<Func<object, ValidationResult>> validators = null, bool enabledPromptTooltip = true, bool enabledAbortKey = true, bool enabledAbortAllPipes = true, bool hideAfterFinish = false, CancellationToken? cancellationToken = null)
```

**Highlighted parameters**
- masktype = Must be MaskedGenericType
- maskedvalue = Input mask
- defaultValue = Initial value witdhout mask
- upperCase = All input to upercase
- showtypeinput = Show tooptip of type input 
- enabledAbortKey = Enabled/Disabled Hotkey AbortKeyPress
- enabledAbortAllPipes = Enabled/Disabled Hotkey AbortAllPipesKeyPress
- enabledPromptTooltip = Default Value = Global Settings EnabledPromptTooltip
- hideAfterFinish = Hide result after finish

### Return
[**Top**](#promptplus--maskeditgeneric)

```csharp
ResultPPlus<ResultMasked>
```

### Sample
[**Top**](#promptplus--maskeditgeneric)

```csharp
var mask = PPlus.MaskEdit(PPlus.MaskTypeGeneric, 
  "Inventory Number", 
  @"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}", 
  cancellationToken: _stopApp);
if (mask.IsAborted)
{
    return;
}
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
[**ResultMasked**](resultmasked) |
[**BasePromptOptions**](basepromptoptions)

