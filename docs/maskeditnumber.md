# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # MaskEditNumber
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultMasked**](resultmasked) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control MaskEdit. Numeric/Currency input with language/culture, tooltips and input validator.

![](./images/MaskEditNumber.gif)
![](./images/MaskEditCurrency.gif)

### Shortcuts

- If the Decimal Separator is typed, the cursor advances to part decimal

### Syntax
[**Top**](#promptplus--maskeditnumber)

```csharp
 MaskEdit([MaskedType.Number, string prompt, string description = null)
 MaskEdit([MaskedType.Currency, string prompt, string description = null)
 ````

### Methods
[**Top**](#promptplus--maskeditnumber)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
  MaskType(MaskedType value, string mask = null)
  ``` 
  - set masked type and valid mask input for  MaskedType = Generic.For other type mask will be ignored.

- ```csharp
   Default(object value)
  ``` 
    - Initial value with mask.

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
  DescriptionSelector(Func<ResultMasked,string> value)
  ``` 
    - Run the fucntion on each interaction and show result in description line.

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

- ```csharp
  Config(Action<IPromptConfig> context)
  ``` 
  - For access [**base methods**](basemethods) common to all controls.

- ```csharp
   PipeCondition(Func<ResultPipe[], object, bool> condition)
  ``` 
  - Set condition to run pipe.

- ```csharp
   ToPipe(string id, string title, object state = null)
  ``` 
  - Transform control to IFormPlusBase.
  - It is mandatory to use with the Pipeline control. See examples in [**PipeLine Control**](pipeline)

- ```csharp
  ResultPromptPlus<ResultMasked> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Return
[**Top**](#promptplus--maskeditnumber)

```csharp
IControlMaskEdit                //for Control Methods
ResultPromptPlus<ResultMasked>  //After execute Run method
IPromptPipe                     //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                   //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--maskeditnumber)

```csharp
var mask = PromptPlus.MaskEdit(MaskedType.Currency)
        .AmmoutPositions(5, 2)
        .Culture(new CultureInfo("en-US"))
        .AcceptSignal(true)
        .Run(_stopApp);

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
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

