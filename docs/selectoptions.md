# PromptPlus # SelectOptions
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Select Control**](select) |
[**BaseOptions**](baseoptions) 

## Documentation
Control Options Class for [**Select**](select). inherits from [**BaseOptions**](baseoptions)

### Properties
[**Top**](#promptplus--selectoptions)

- DefaultValue
	- Type : Generic
	- Default value for a selected item.

- PageSize   
	- Type : int
	- Maximum items per page. Tf the value is null, the value will be calculated according to the screen size
	- Default Value = Null
	
- Items
  - Type : IEnumerable\<T\>
  - Colletion of items. If the type of items is an **enum** the type must be **EnumValue\<T\>** where T is the type of the enum

- TextSelector
  - Type : Func\<T, string\>
  - Function that returns the string that will be displayed. Tf the value is null,the value will be the \[Display\] attribute if it exists or an enum string.
	- Default Value = x => x.ToString()
  
### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Select Control**](select) |
[**BaseOptions**](baseoptions) 

