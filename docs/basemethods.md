# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # BaseMethods

[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Hotkeys**](index.md#hotkeys) |
[**Global Settings**](index.md#global-settings)

## Documentation
Base Methods for all controls.

### Methods
[**Top**](#-promptplus--basemethods)

- ```csharp
  IPromptControls<T> EnabledAbortKey(bool value)
  ``` 
    - Enabled/Disabled [**Hotkey**](index.md#hotkeys) AbortKeyPress.
	- Default Value = [**Global Settings**](index.md#global-settings) EnabledStandardTooltip
- ```csharp
  IPromptControls<T> EnabledAbortAllPipes(bool value)
  ``` 
    - Enabled/Disabled [**Hotkey**](index.md#global-settings) AbortAllPipesKeyPress.
	- Default Value = [**Global Settings**](index.md#global-settings) EnabledAbortAllPipes

- ```csharp
  IPromptControls<T> EnabledPromptTooltip(bool value)
  ``` 
	- Enabled/Disabled [**Hotkey**](index.md#global-settings) AbortAllPipesKeyPress.

- ```csharp
  IPromptControls<T> HideAfterFinish(bool value)
  ``` 
    - Hide result after finish.
	- Default Value = false. Exception to the Keypress-control that the value = true

- ```csharp
  ResultPromptPlus<T> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Hotkeys**](index.md#hotkeys) |
[**Global Settings**](index.md#global-settings)



