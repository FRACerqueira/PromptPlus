# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # Select
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control Select. Generic select IEnumerable/Enum with auto-paginator and tooltips and more.

![](./images/Select.gif)

### Syntax
[**Top**](#promptplus--select)

```csharp
 Select<T>(string prompt, string description = null)
```

### Methods
[**Top**](#promptplus--select)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
 Default(T value)
 ``` 
  - initial item seleted.

- ```csharp
  PageSize(int value)
    ```
    - Maximum item per page. If the value is ommited, the value will be calculated according to the screen size

- ```csharp
  TextSelector(Func<T, string> value)
    ```
    - Function to extract value with type T to string and displayed.
    - For IEnumerable type, if ommited,the value will be item => item.ToString()
    - For enum value, if ommited,the value will be the \[Display\] attribute if it exists or an enum string.

- ```csharp
  DescriptionSelector(Func<T, string> value)
    ```
    - Function to extract the value with type T to string and displayed in the description line.

- ```csharp
  AddItem(T value)
    ```
    - Add item to list.

- ```csharp
  AddItems(IEnumerable<T> value)
    ```
    - Add IEnumerable item to list.

- ```csharp
  HideItem(T value)
    ```
    - Hide item in list.

- ```csharp
  HideItems(IEnumerable<T> value)
    ```
    - Hide IEnumerable items in list.

- ```csharp
  DisableItem(T value)
    ```
    - Tick disabled item in list .

- ```csharp
  DisableItems(IEnumerable<T> value)
    ```
    - Tick disabled IEnumerable items in list.

- ```csharp
  AutoSelectIfOne()
    ```
    - Automatically selects the item when there is only a single filtered item

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
  ResultPromptPlus<T> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Return
[**Top**](#promptplus--select)

```csharp
IControlSelect<T>      //for Control Methods
ResultPromptPlus<T>    //After execute Run method
IPromptPipe            //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase          //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--select)

```csharp
var city = PromptPlus.Select<string>("Select your city")
    .AddItems(new[] { "1 - Seattle", "2 - London", "3 - Tokyo", "4 - New York", "5 - Singapore", "6 - Shanghai" })
    .PageSize(3)
    .AutoSelectIfOne()
    .Run(_stopApp);

if (city.IsAborted)
{
    return;
}
if (!string.IsNullOrEmpty(city.Value))
{
    PromptPlus.WriteLine($"Hello, {city.Value}!");
}
else
{
    PromptPlus.WriteLine("You chose nothing!");
}
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)


