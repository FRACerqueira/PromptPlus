# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # ListMasked
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
 ListMasked(string prompt, string description = null)
````

### Methods
[**Top**](#promptplus--listmasked)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

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
  DescriptionSelector(Func<string,string> value)
  ``` 
    - Run the fucntion on each interaction and show result in description line.

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
  AddValidator(Func<object, ValidationResult> validator);
  ``` 
    - item of input validator

- ```csharp
  AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
  ``` 
    - List of input validator

 ```csharp
  InitialValue(T value, bool ever = false)
    ```
    - Set initial input value. Optionally always start with the value when ever is true

- ```csharp
  ValidateOnDemand()
  ``` 
    - Run the validators on each interaction

- ```csharp
  AddItem(string value)
    ```
    - Add item to list.

- ```csharp
  AddItems(IEnumerable<string> value)
    ```
    - Add IEnumerable item to list.

- ```csharp
  TransformItems(Func<string, string> value)
    ```
    - Function that transforms items during startup.

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
  ResultPromptPlus<IEnumerable<ResultMasked>> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Return
[**Top**](#promptplus--listmasked)

```csharp
IControlListMasked                          //for Control Methods
ResultPromptPlus<IEnumerable<ResultMasked>> //After execute Run method
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
