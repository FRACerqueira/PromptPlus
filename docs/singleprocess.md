# PromptPlus # SingleProcess
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**WaitProcess Control**](waitprocess) |
[**WaitProcess Options**](waitprocessoptions)

## Documentation
Class for [**WaitProcess Control**](waitprocess) and [**WaitProcess Options**](waitprocessoptions).

### Properties
[**Top**](#promptplus--singleprocess)

- ProcessId
	- Type : string
	- Process identification. If null value, ProcessId = Guid.NewGuid().ToString()

- ProcessToRun   
	- Type : Func<CancellationToken, Task<object>>
	- function that will perform the Task

- ProcessTextResult   
	- Type : Func<object, string>
	- function that will perform the textual representation of the result. IF null value, ProcessTextResult = x == null ? "" : x.ToString() 

  
### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**WaitProcess Control**](waitprocess) |
[**WaitProcess Options**](waitprocessoptions)

