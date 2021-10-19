# PromptPlus # BaseMethods

[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Hotkeys**](index.md#hotkeys) |
[**Global Settings**](index.md#global-settings)

## Documentation
Base Methods for all controls.

### Methods
[**Top**](#promptplus--basemethods)

- IPromptControls<T> EnabledAbortKey(bool value)
    - Enabled/Disabled [**Hotkey**](index.md#hotkeys) AbortKeyPress.
	- Default Value = [**Global Settings**](index.md#global-settings) EnabledStandardTooltip

- IPromptControls<T> EnabledAbortAllPipes(bool value)
    - Enabled/Disabled [**Hotkey**](index.md#global-settings) AbortAllPipesKeyPress.
	- Default Value = [**Global Settings**](index.md#global-settings) EnabledAbortAllPipes

- IPromptControls<T> EnabledPromptTooltip(bool value)
	- Enabled/Disabled [**Hotkey**](index.md#global-settings) AbortAllPipesKeyPress.

- IPromptControls<T> HideAfterFinish(bool value)
    - Hide result after finish.
	- Default Value = false. Exception to the Keypress control that the value = true

- ResultPromptPlus<T> Run(CancellationToken? value = null)
	- Control execution

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Hotkeys**](index.md#hotkeys) |
[**Global Settings**](index.md#global-settings)



