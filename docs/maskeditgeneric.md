# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # MaskEditGeneric
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultMasked**](resultmasked) |
[**BaseOptions**](baseoptions)


## Documentation
Control MaskEdit. Generic input with masked input, tooltips and input validator

![](./images/MaskEditGeneric.gif)

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
 MaskEdit([MaskedType.Generic, string prompt = null)
 ```

 ### Methods
[**Top**](#promptplus--maskeditdate)

 ```csharp
  Prompt(string value)
  ``` 
  - set prompt message 
- ```csharp
  MaskType(MaskedType value, string mask = null)
  ``` 
  - set masked type and valid mask input for  MaskedType = Generic.For other type mask will be ignored.
- ```csharp
  PageSize(int value)
    ```
    - Maximum item per page. If the value is ommited, the value will be calculated according to the screen size 
- ```csharp
  AddValidator(Func<object, ValidationResult> validator);
  ``` 
    - item of input validator
- ```csharp
  AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
  ``` 
    - List of input validator
- ```csharp
  ValidateOnDemand()
  ``` 
    - Run the validators on each interaction
- ```csharp
  Culture(CultureInfo cultureinfo)
  ``` 
    - Language/Culture of date/time/number/currency. If null value, culture is DefaultCulture of PromptPlus.
- ```csharp
   FillZeros(bool value)
  ``` 
    - Fill input type with zeros for number types.
- ```csharp
   ShowInputType(bool value)
  ``` 
    - Show tooptip of type input.
 - ```csharp
   ShowDayWeek(FormatWeek value)
  ``` 
    - Show day week of type input date-type.For other type format will be ignored
- ```csharp
   FormatYear(FormatYear value)
  ``` 
    - Formart year(Y4,Y2) to input date-type.Default vaue is Y4 (4 positions).For other type format will be ignored
- ```csharp
   FormatTime(FormatTime value)
  ``` 
    - Formart time(HMS,OnlyHM,OnlyH) to input time-type.Default value is HMS (Hours, minutes, seconds).For other type format will be ignored
- ```csharp
    AmmoutPositions(int intvalue, int decimalvalue)
  ``` 
    - Ammout max. positions of integers and decimals to input number-type/currency-type.For other type ammount will be ignored
- ```csharp
    AcceptSignal(bool signal)
  ``` 
    -  Accept signal to input number-type/currency-type. If not defined, only positive number.For other type signal will be ignored

### Return
[**Top**](#promptplus--maskeditdate)

```csharp
IControlMaskEdit                //for Control Methods
IPromptControls<ResultMasked>   //for others Base Methods
ResultPromptPlus<ResultMasked>  //for Base Method Run, when execution is direct 
IPromptPipe                     //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                   //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--maskeditgeneric)

```csharp
var mask = PromptPlus.MaskEdit(MaskedType.Generic, "Inventory Number")
    .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
    .Run(_stopApp);

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
[**ResultPromptPlus**](resultpromptplus) |
[**ResultMasked**](resultmasked) |
[**BaseOptions**](baseoptions)

