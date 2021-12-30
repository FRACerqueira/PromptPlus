# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # BaseMethods

[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Hotkeys**](index.md#hotkeys) |
[**Global Settings**](index.md#global-settings)

## Documentation
Base Methods for all controls.

### Methods
[**Top**](#promptplus--basemethods)

- ```csharp
  IPromptConfig EnabledAbortKey(bool value)
  ``` 
    - Enabled/Disabled [**Hotkey**](index.md#hotkeys) AbortKeyPress.
	- Default Value = [**Global Settings**](index.md#global-settings) EnabledStandardTooltip

- ```csharp
  IPromptConfig EnabledAbortAllPipes(bool value)
  ``` 
    - Enabled/Disabled [**Hotkey**](index.md#global-settings) AbortAllPipesKeyPress.
	- Default Value = [**Global Settings**](index.md#global-settings) EnabledAbortAllPipes

- ```csharp
  IPromptConfig EnabledPromptTooltip(bool value)
  ``` 
	- Enabled/Disabled [**Hotkey**](index.md#global-settings) AbortAllPipesKeyPress.

- ```csharp
  IPromptConfig HideAfterFinish(bool value)
  ``` 
    - Hide result after finish.
	- Default Value = false. Exception to the Keypress-control that the value = true

- ```csharp
  IPromptConfig SetContext(object value)
  ``` 
    - set user context to control.
	- Default Value = null.

- ```csharp
  IPromptConfig AddExtraAction(StageControl stage, Action<object, string> useraction)
  ``` 
    - Execute user action on control stage.
    - StageControl
        - OnStartControl - Occurs only once when control starts.
        - OnInputRender - Occurs every time the control is rendered.
        - OnFinishControl -  Occurs only once when control ends.
    - useraction action: (object)user context of control and (string)Current input text.


### Sample use of ExtraAction
[**Top**](#promptplus--basemethods)

```csharp
//sampe input with sugest using history (1 day)

IList<ItemHistory> _itemsInputSampleHistory;

var name = PromptPlus.Input("What's your name?")
    .Default("Peter Parker")
    .AddValidator(PromptPlusValidators.Required())
    .AddValidator(PromptPlusValidators.MinLength(3))
    .SuggestionHandler(SugestionInputSample, true)
    .Config((ctx) =>
    {
        ctx.AddExtraAction(StageControl.OnStartControl, LoadSampleHistInputSugestion)
           .AddExtraAction(StageControl.OnFinishControl, SaveSampleHistSugestion);
    })
    .Run(_stopApp);
if (name.IsAborted)
{
    return;
}
PromptPlus.WriteLine($"Hello, [cyan]{name.Value}[/]!");
````

```csharp
private SugestionOutput SugestionInputSample(SugestionInput arg)
{
    var result = new SugestionOutput();
    if (_itemsInputSampleHistory.Count > 0)
    {
        foreach (var item in _itemsInputSampleHistory
            .OrderByDescending(x => x.TimeOutTicks))
        {
            result.Add(item.History, true);
        }
    }
    return result;
}
````

```csharp
private void LoadSampleHistInputSugestion(object ctx, string value)
{
    _itemsInputSampleHistory  = FileHistory
        .LoadHistory($"{AppDomain.CurrentDomain.FriendlyName}_SampleHistInputSugestion");
}
````

```csharp
private void SaveSampleHistSugestion(object ctx, string value)
{
    if (value is null)
    {
        return;
    }
    var localnewhis = value.Trim();
    var found = _itemsInputSampleHistory
        .Where(x => x.History.ToLowerInvariant() == localnewhis.ToLowerInvariant())
        .ToArray();
    if (found.Length > 0)
    {
        foreach (var item in found)
        {
            _itemsInputSampleHistory.Remove(item);
        }
    }
    if (_itemsInputSampleHistory.Count >= byte.MaxValue)
    {
        _itemsInputSampleHistory.RemoveAt(_itemsInputSampleHistory.Count - 1);
    }
    _itemsInputSampleHistory.Insert(0,
        ItemHistory.CreateItemHistory(localnewhis, new TimeSpan(1, 0, 0, 0)));

    FileHistory.SaveHistory(
        $"{AppDomain.CurrentDomain.FriendlyName}_SampleHistInputSugestion",
        _itemsInputSampleHistory);
}
````

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Hotkeys**](index.md#hotkeys) |
[**Global Settings**](index.md#global-settings)



