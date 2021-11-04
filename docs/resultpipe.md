# PromptPlus # ResultPipe
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**PipeLine Control**](pipeline)

## Documentation
Default return struct for [**PipeLine Control**](pipeline).

### Properties
[**Top**](#promptplus--resultmasked)

- PipeId
	- Type : string
	- ReadOnly	
	- Pipe identifier.

- Title
	- Type : string
	- ReadOnly	
	- Title of Pipe.

- Status
	- Type : StatusPipe
	- ReadOnly	
	- Status of pipe.
	    - Waitting = Not run
	    - Skiped = Condition for execution was not accepted
	    - Aborted = Canceled by user
	    - Running = Current pipe is running
	    - Done = Finished

- ValuePipe 
	- Type : Object
	- ReadOnly	
	- pipe result Value.

### Links

[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**PipeLine Control**](pipeline)
