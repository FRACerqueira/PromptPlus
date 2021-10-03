# PromptPlus # Step
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**PipeLine Control**](pipeline)

## Documentation

**Extension** to define a pipe for a [**PipeLine Control**](pipeline).

### Syntax

```csharp
Step(string title, Func<ResultPipe[],object,  bool> condition = null, object contextstate = null, string id = null)
```
### Extension From

```csharp
IFormPPlusBase
```

### Parameters
[**Top**](#promptplus--pipelinestep)

- title
	- Type : string
	- ReadOnly
	- Title of pipe

- condition 
	- Type : Func<ResultPipe[],object,  bool>
	- ReadOnly
	- Precondition for running the pipe.

- contextstate 
	- Type : object
	- ReadOnly
	- Generic context state for usage in condition.

- id 
	- Type : string
	- ReadOnly
	- pipe identifier.If null value, id = Guid.NewGuid().ToString()

### Links

[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**PipeLine Control**](pipeline)

