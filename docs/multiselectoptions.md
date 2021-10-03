# PromptPlus # MultiSelectOptions
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**MultiSelect Control**](multiselect) |
[**BaseOptions**](baseoptions) 

## Documentation
Control Options Class for [**MultiSelect**](multiselect). inherits from [**BaseOptions**](baseoptions)

### Properties
[**Top**](#promptplus--multiselectoptions)

- DefaultValue
	- Type : IEnumerable<T>
	- IEnumerable default value for selected items.

- PageSize   
	- Type : int
	- Maximum items per page. Tf the value is null, the value will be calculated according to the screen size
	- Default Value = Null
  
 - Minimum
	- Type : int
	- Minimum items selected.
	- Default Value = 1
	
 - Maximum
	- Type : int
	- Maximum items selected.
	- Default Value = int.MaxValue

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
[**MultiSelect Control**](multiselect) |
[**BaseOptions**](baseoptions) 
