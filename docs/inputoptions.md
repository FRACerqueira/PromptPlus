# PromptPlus # InputOptions
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Input Control**](input) |
[**Password Control**](password) |
[**BaseOptions**](baseoptions)

## Documentation
Control Options Class for [**Input**](input) and [**Password Control**](password). inherits from [**BaseOptions**](baseoptions)

### Properties
[**Top**](#promptplus--inputoptions)

- SwithVisiblePassword
	- Type : bool
	- Enabled/disabled view input password by [Hotkey](index.md#hotkeys) SwitchViewPassword .
	- Default Value = true
        - SwithVisiblePassword will be ignored if IsPassword = false

- IsPassword   
	- Type : bool
	- Input is password type.
	- Default Value = false
	
- DefaultValue
  - Type : string
  - Default value for input. If the input is empty and there is a DefaultValue and the all condition from Validators is true, the return will be DefaultValue
    - Default value will be ignored if IsPassword = true

- Validators
  - Type : IList<Func<object, ValidationResult>>
  - List of input validator


### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Input Control**](input) |
[**Password Control**](password) |
[**BaseOptions**](baseoptions)



