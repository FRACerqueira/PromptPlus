# PromptPlus # ListOptions
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**List Control**](list) |
[**BaseOptions**](baseoptions) 

## Documentation
Control Options Class for [**List**](list). inherits from [**BaseOptions**](baseoptions)

### Properties
[**Top**](#promptplus--listoptions)

- AllowDuplicate
	- Type : bool
	- Accept duplicate items
	- Default Value = true

 - Maximum
	- Type : int
	- Maximum items selected.
	- Default Value = int.MaxValue

- Minimum
	- Type : int
	- Minimum items selected.
	- Default Value = 0

- UpperCase   
	- Type : bool
	- input to upercase
	- Default Value = false

- Validators
	- Type : List<Func<object, ValidationResult>>()
	- List of Validators
  
- TextSelector
  - Type : Func\<T, string\>
	- Default Value = x => x.ToString()
  
### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**List Control**](list) |
[**BaseOptions**](baseoptions) 
