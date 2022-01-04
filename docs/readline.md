# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # Readline
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Input text with GNU Readline Emacs keyboard shortcuts, sugestions and historic.

### Readline-Control

![](./images/Readline.gif)

### Input and List-Control with readline buffer

![](./images/InputHistory.gif)
![](./images/InputSugestion.gif)
![](./images/ListSugestion.gif)


## Emacs keyboard shortcuts
[**Top**](#promptplus--readline)

- Tab/Shift+Tab  : Autocompletes when configured.
- DownArrow/PgDown : History when configured.

- Ctrl+a : Moves the cursor to the line start (equivalent to the key Home).
- Ctrl+b : Moves the cursor back one character (equivalent to the key leftarrow).
- Ctrl+d : Deletes the current character (then equivalent to the key Delete).
- Ctrl+e : Moves the cursor to the line end (equivalent to the key End).
- Ctrl+f : Moves the cursor forward one character (equivalent to the key rightarrow).
- Ctrl+h : Deletes the previous character (same as backspace).
- Ctrl+i : Equivalent to the tab key.
- Ctrl+j : Equivalent to the enter key.
- Ctrl+k : Clears the line content after the cursor.
- Ctrl+l : Clears the screen content (equivalent to the command clear).
- Ctrl+t : Transpose the previous two characters.
- Ctrl+u : Clears the line content before the cursor and copies it into the clipboard.
- Ctrl+w : Clears the word before the cursor and copies it into the clipboard.
- Alt+b : (backward) moves the cursor backward one word.
- Alt+c : Capitalizes the character under the cursor and moves to the end of the word.
- Alt+d : Cuts the word after the cursor.
- Alt+f : (forward) moves the cursor forward one word.
- Alt+l : Lowers the case of every character from the cursor's position to the end of the current word.
- Alt+u : Capitalizes every character from the cursor's position to the end of the current word.


### Syntax
[**Top**](#promptplus--readline)

```csharp
 Readline(string prompt, string description = null)
 ```

### Methods
[**Top**](#promptplus--readline)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
  InitialValue((string value, string error = null)
  ``` 
  -  Initial value for input and initial message error.

- ```csharp
  AddValidator(Func<object, ValidationResult> validator);
  ``` 
    - item of input validator.

- ```csharp
  AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
  ``` 
    - List of input validator

- ```csharp
  FileNameHistory(string value)
  ``` 
    - History filename. Is requeried when EnabledHistory is true. The file name mustbe unique. The history file will be saved in SpecialFolder.UserProfile in the folder 'PromptPlus.History'.

- ```csharp
  SaveHistoryAtFinish(bool value)
  ``` 
    - Auto save History at Finish. Default value 'true'.

- ```csharp
  FinisWhenHistoryEnter(bool value);
  ``` 
    - try Finish when History selected. Default value 'false'.

- ```csharp
  EnabledHistory(bool value)
  ``` 
    - Enabled History. Default value 'false'.

- ```csharp
  MinimumPrefixLength(int value)
  ``` 
    - Minimum prefix length to start history. Default value '3'.

- ```csharp
  TimeoutHistory(TimeSpan value)
  ``` 
    - Timeout history, after timeout the item will be removed. Default value 365 days.

- ```csharp
  MaxHistory(byte value)
  ``` 
    - Maximum number of items in history. when reaching the maximum, the items will be rotated by the oldest. Default value byte.MaxValue

- ```csharp
  SuggestionHandler(Func<SugestionInput, SugestionOutput> value)
  ``` 
    - Function to load and return sugestions when tab/shift+tab is pressed. Default value 'null', no sugestions.

- ```csharp
  PageSize(int value)
    ```
    - Maximum history item per page. If the value is ommited, the value will be calculated according to the screen size

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
  ResultPromptPlus<string> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### SugestionInput

- PromptText    : Current input text. Readonly.
- CursorPrompt  : Current cursor position of input text. Readonly.
- Context       : Object with control context when it exists. Readonly.

### SugestionOutput

- SugestionOutput() : create SugestionOutput with empty sugestions
- SugestionOutput(IList<ItemSugestion> items)  : create SugestionOutput with list of suggestions)
- Add(string value, bool clearrestofline = false, string description = null) : Add sugestion to list
    - value : string with sugestion. Required.
    - clearrestofline : if true, rest of line willbe cleared.Optional.
    - description : Description to sugestion.Optional.
- AddRange(IList<ItemSugestion> items) : Add list sugestion to list
- SetMsgError(string value) : Set message error when sugestion list is empty
- SetCursorPrompt(int value) : Set new CursorPrompt after apply sugestion

### Return
[**Top**](#promptplus--readline)

```csharp
IControlReadline             //for Control Methods
ResultPromptPlus<string>     //After execute Run method
IPromptPipe                  //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--readline)

```csharp
var ctrlreadline = PromptPlus.Readline("Readline>", "Sample Readline control")
    .AddValidator(PromptPlusValidators.Required())
    .PageSize(5)
    .EnabledHistory(true)
    .FileNameHistory("RunReadlineSample")
    .TimeoutHistory(new TimeSpan(24, 0, 0))
    .FinisWhenHistoryEnter(true)
    .SuggestionHandler(mysugestion)
    .Run(_stopApp);

if (ctrlreadline.IsAborted)
{
    return;
}
PromptPlus.WriteLine($"Result : [cyan]{ctrlreadline.Value}[/]!");
```

```csharp
private SugestionOutput mysugestion(SugestionInput arg)
{
    var aux = new SugestionOutput();
    var word = arg.CurrentWord();
    if (word.ToLowerInvariant() == "prompt")
    {
        aux.Add("choose");
        aux.Add("secure");
        aux.Add("help", true);
        return aux;
    }
    var random = new Random();
    for (var i = 0; i < 3; i++)
    {
        var c1 = (char)random.Next(65, 90);
        var c2 = (char)random.Next(97, 122);
        var c3 = (char)random.Next(97, 122);
        aux.Add($"Opc {c1}{c2}{c3}");
    }
    aux.Add("opc Clearline -test a b c", true);
    return aux;
}
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
