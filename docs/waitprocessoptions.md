# PromptPlus # WaitProcessOptions
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**WaitProcess Control**](waitprocess) |
[**SingleProcess**](singleprocess) 

## Documentation
Control Options Class for [**WaitProcess Control**](waitprocess). inherits from [**BaseOptions**](baseoptions)

### Properties
[**Top**](#promptplus--waitprocessoptions)

- SpeedAnimation 
	- Type : int
	- Animation speed in milliseconds. If value < 10 SpeedAnimation = 10.If value > 1000 SpeedAnimation = 1000

- Process   
	- Type : IEnumerable<SingleProcess<T>>
	- IEnumerable of SingleProcess<T> with functions that will be performed
  
- ProcessTextResult
	- Type : Func<T, string>
	- Function that returns the string that will be displayed.Tf the value is null,the value will be item => item.ToString()

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**WaitProcess Control**](waitprocess) |
[**SingleProcess**](singleprocess) 
