# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # SingleProcess
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**WaitProcess Control**](waitprocess) 

## Documentation
struct for definition of single process to [**WaitProcess Control**](waitprocess). 

### Syntax

```csharp
 SingleProcess(Func<CancellationToken, Task<object>> processToRun, string idProcess =null, Func<object, string> processTextResult = null)
````

### Parameters
[**Top**](#-promptplus--singleprocess)

- ProcessToRun   
	- Type : Func<CancellationToken, Task<object>>
	- function that will perform the Task

- ProcessId
	- Type : string
	- Process identification. If null value, ProcessId = Guid.NewGuid().ToString()

- ProcessTextResult   
	- Type : Func<object, string>
	- function that will perform the textual representation of the result. IF null value, ProcessTextResult = x == null ? "" : x.ToString() 

  
### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**WaitProcess Control**](waitprocess) 

