# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # Attributes
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**PromptPlus.CommandDotNet**](ppluscmddotnet.md#help)

## Documentation
Prompt Plus created custom attributes for property type and parameter type to automate third-party application integration.

the attributes allow you to identify PromptPlus's native types and controls to execute the controls associated with each kind.

## Attributes

### PromptInitialValue
[**Top**](#attributes)

Binds the property or parameters with initial value to be used in the prompt control when applicable.

```csharp
PromptInitialValue(string initialValue, bool ever = false)
```
- **initialValue**
    - Initial value

- **ever**
    - If true for each interaction of a new value the initial value will be loaded

### PromptValidatorUri
[**Top**](#attributes)

Binds the property or parameters to Uri validator into the prompt control, where applicable.

```csharp
PromptValidatorUri(UriKind uriKind = UriKind.Absolute, string allowedUriScheme = null)
```
- **uriKind**
    - Kind Uri. (RelativeOrAbsolute/Absolute/Relative)

- **allowedUriScheme**
    - List of valid Uri-Scheme separated by semicolon

### PromptPlusTypeNumber
[**Top**](#attributes)

Binds the property or parameters to a kind MaskEdit Numeric control, where applicable

```csharp
PromptPlusTypeNumber(int integerpart, int decimalpart, bool accepSignal = true, CultureInfo? cultureInfo = null)
```
- **integerpart**
    -  Ammout max. positions of integers to input

- **decimalpart**
    -  Ammout max. positions of decimals to input

- **accepSignal**
    -  Accept signal to input

- **CultureInfo**
    -  Language/Culture of number. If null value, culture is current.

### PromptPlusTypeCurrency
[**Top**](#attributes)

Binds the property or parameters to a kind MaskEdit Currency control, where applicable

```csharp
PromptPlusTypeCurrency(int integerpart, int decimalpart, bool accepSignal = true, CultureInfo? cultureInfo = null)
```
- **integerpart**
    -  Ammout max. positions of integers to input

- **decimalpart**
    -  Ammout max. positions of decimals to input

- **accepSignal**
    -  Accept signal to input

- **CultureInfo**
    -  Language/Culture of number. If null value, culture is current.

### PromptPlusTypeDate
[**Top**](#attributes)

Binds the property or parameters to a kind MaskEdit Date-Only control, where applicable

```csharp
PromptPlusTypeDate(FormatYear formatYear = FormatYear.Y4,CultureInfo? cultureInfo = null)
```
- **formatYear**
    -  Formart year(Y4,Y2) to input date-type.Default vaue is Y4 (4 positions)

- **CultureInfo**
    -  Language/Culture of number. If null value, culture is current.

### PromptPlusTypeTime
[**Top**](#attributes)

Binds the property or parameters to a kind MaskEdit Time-Only control, where applicable

```csharp
PromptPlusTypeTime(FormatTime formatTime = FormatTime.HMS, CultureInfo? cultureInfo = null)
```
- **formatTime**
    -  Formart time(HMS,Only-HM,Only-H) to input time-type.Default value is HMS (Hours, minutes, seconds)

- **CultureInfo**
    -  Language/Culture of number. If null value, culture is current.

### PromptPlusTypeBrowser

Binds the property or parameters to a kind Browser control, where applicable

```csharp
PromptPlusTypeBrowser(
            BrowserFilter kind = BrowserFilter.None,
            string prefixExtension = null,
            string searchPattern = null)
```
- **kind**
    -  Fiter result to kind (Folder/File). Default value = BrowserFilter.None (File)

- **prefixExtension**
    -  Prefix to be added to the end of item (only new file/folder)

- **searchPattern**
    -  Specifies what to search for by the browser

### PromptPlusTypeMasked

Binds the property or parameters to a kind MaskEdit Generic control, where applicable

```csharp
PromptPlusTypeMasked(string mask, bool uppercase = false)
```
- **mask**
    -  set masked type and valid mask input

- **uppercase**
    -  Upper case input value.

**Mask Characters and Delimiters**

 - 9 : Only a numeric character
 - L : Only a letter 
 - C : OnlyCustom character 
 - A : Any character
 - N : OnlyCustom character +  Only a numeric character
 - X : OnlyCustom character +  Only a letter

 - \ : Escape character
 - { : Initial delimiter for repetition of masks
 - } : Final delimiter for repetition of masks
 - \[ : Initial delimiter for list of Custom character
 - \] : Final delimiter for list of Custom character

**_Examples:_**

- 9999999 = Seven numeric characters
- 9{7} = Seven numeric characters
- 99\/99 = Four numeric characters separated in the middle by a "/"
- C{2}\[ABC\] = two characters with ony A,B,C valid char.
   
### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**PromptPlus.CommandDotNet**](ppluscmddotnet.md#help)

