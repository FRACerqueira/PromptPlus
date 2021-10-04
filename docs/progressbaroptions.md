# PromptPlus # ProgressBarOptions
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Progressbar Control**](progressbar) |
[**BaseOptions**](baseoptions) 

## Documentation
Control Options Class for [**Progressbar Control**](progressbar). inherits from [**BaseOptions**](baseoptions)

### Properties
[**Top**](#promptplus--progressbaroptions)

- InterationId 
	- Type : object
	- identification last interaction. If null value, interationId = 0 (int).

- UpdateHandler   
	- Type : Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>>
	- function that will be performed for each interaction
  
- Witdth
	- Type : int
	- Width of Progressbar. If Width < 30, Width = 30.  If Width . 100, Width = 100

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Progressbar Control**](progressbar) |
[**BaseOptions**](baseoptions) 
