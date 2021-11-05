# PromptPlus # ListMasked
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultMasked**](resultmasked) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control ListMasked. Create IEnumerable with masked-type input, auto-paginator, tooptip , input validator

![](./images/MaskedList.gif)

### Syntax
[**Top**](#promptplus--listmasked)

```csharp
 ListMasked(string prompt = null)
````

### Methods
[**Top**](#promptplus--listmasked)

- ```csharp
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
   TextSelector(Func<T, string> value)
    ```
    - Function to extract value with type T from string and displayed. If not defined, the value will be item => item.ToString()
- ```csharp
  Range(int minvalue, int maxvalue)
    ```
    - Minimum and maximum items added
- ```csharp
  UpperCase(bool value)
    ```
    - UpperCase input
- ```csharp
  AllowDuplicate(bool value)
    ```
    - Allow duplicate input
- ```csharp
  Addvalidator(Func<object, ValidationResult> validator);
  ``` 
    - item of input validator
- ```csharp
  Addvalidators(IEnumerable<Func<object, ValidationResult>> validators)
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
    - Fill input type with zeros for number types.For other type will be ignored.
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
[**Top**](#promptplus--listmasked)

```csharp
IControlListMasked                          //for Control Methods
IPromptControls<IEnumerable<ResultMasked>>  //for others Base Methods
ResultPromptPlus<IEnumerable<ResultMasked>> //for Base Method Run, when execution is direct 
IPromptPipe                                 //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                               //for only definition of pipe to Pipeline Control
```


### Sample
[**Top**](#promptplus--listmasked)


```csharp
var lst = PromptPlus.ListMasked("Please add item(s)")
    .MaskType(MaskedType.Generic, @"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
    .UpperCase(true)
    .Run(_stopApp);

if (lst.IsAborted)
{
    return;
}
foreach (var item in lst.Value)
{
    Console.WriteLine($"You picked {item.ObjectValue}");
}
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultMasked**](resultmasked) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
