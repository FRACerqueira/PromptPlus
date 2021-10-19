# PromptPlus # PipeMethods

[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Hotkeys**](index.md#hotkeys) |
[**Global Settings**](index.md#global-settings)

## Documentation
Base Methods for all controls.

### Methods
[**Top**](#promptplus--pipemethods)

- IPromptPipe Condition(Func<ResultPipe[], object, bool> condition)
    - Function to validate execute pipe. If result is false, the pipe is skipped.

- IFormPlusBase AddPipe(string id, string title, object state = null)
	- id = id identification. If ommited = Guid.NewGuid().ToString()
    - Tile = Title of pipe
	- state = object state of executation

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Hotkeys**](index.md#hotkeys) |
[**Global Settings**](index.md#global-settings)



