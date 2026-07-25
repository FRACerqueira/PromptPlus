<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## HotKey Struct

Represents a configurable hotkey composed of a base [System\.ConsoleKey](https://learn.microsoft.com/en-us/dotnet/api/system.consolekey 'System\.ConsoleKey') and optional modifier flags\.

```csharp
public readonly struct HotKey : System.IEquatable<PromptPlusLibrary.HotKey>, System.IEquatable<System.ConsoleKeyInfo>
```

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1'), [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

### Remarks
This struct is lightweight and immutable after construction \(properties are init via the primary constructor\)\.
Use the static members for common built\-in hotkeys\.
### Constructors

<a name='PromptPlusLibrary.HotKey.HotKey(System.ConsoleKey,bool,bool,bool)'></a>

## HotKey\(ConsoleKey, bool, bool, bool\) Constructor

Represents a configurable hotkey composed of a base [System\.ConsoleKey](https://learn.microsoft.com/en-us/dotnet/api/system.consolekey 'System\.ConsoleKey') and optional modifier flags\.

```csharp
public HotKey(System.ConsoleKey key, bool alt=false, bool ctrl=false, bool shift=false);
```
#### Parameters

<a name='PromptPlusLibrary.HotKey.HotKey(System.ConsoleKey,bool,bool,bool).key'></a>

`key` [System\.ConsoleKey](https://learn.microsoft.com/en-us/dotnet/api/system.consolekey 'System\.ConsoleKey')

The primary [System\.ConsoleKey](https://learn.microsoft.com/en-us/dotnet/api/system.consolekey 'System\.ConsoleKey')\.

<a name='PromptPlusLibrary.HotKey.HotKey(System.ConsoleKey,bool,bool,bool).alt'></a>

`alt` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether Alt is part of the hotkey\.

<a name='PromptPlusLibrary.HotKey.HotKey(System.ConsoleKey,bool,bool,bool).ctrl'></a>

`ctrl` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether Ctrl is part of the hotkey\.

<a name='PromptPlusLibrary.HotKey.HotKey(System.ConsoleKey,bool,bool,bool).shift'></a>

`shift` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether Shift is part of the hotkey\.

### Remarks
This struct is lightweight and immutable after construction \(properties are init via the primary constructor\)\.
Use the static members for common built\-in hotkeys\.
### Properties

<a name='PromptPlusLibrary.HotKey.Alt'></a>

## HotKey\.Alt Property

Gets a value indicating whether Alt is included\.

```csharp
public bool Alt { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.HotKey.Ctrl'></a>

## HotKey\.Ctrl Property

Gets a value indicating whether Ctrl is included\.

```csharp
public bool Ctrl { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.HotKey.DefaultAbortKeyPress'></a>

## HotKey\.DefaultAbortKeyPress Property

Gets the default abort hotkey \(Esc\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultAbortKeyPress { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultCalendarSwitchNotes'></a>

## HotKey\.DefaultCalendarSwitchNotes Property

Gets the default calendar notes toggle hotkey \(F2\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultCalendarSwitchNotes { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultChartBarSwitchLayout'></a>

## HotKey\.DefaultChartBarSwitchLayout Property

Gets the default chart bar layout switch hotkey \(F2\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultChartBarSwitchLayout { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultChartBarSwitchLegend'></a>

## HotKey\.DefaultChartBarSwitchLegend Property

Gets the default chart bar legend switch hotkey \(F3\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultChartBarSwitchLegend { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultChartBarSwitchOrder'></a>

## HotKey\.DefaultChartBarSwitchOrder Property

Gets the default chart bar order switch hotkey \(F4\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultChartBarSwitchOrder { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultFilterAllSelected'></a>

## HotKey\.DefaultFilterAllSelected Property

Gets the default tooltip Filter all selected items \(F3\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultFilterAllSelected { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultInputHistoryView'></a>

## HotKey\.DefaultInputHistoryView Property

Gets the default history view toggle hotkey \(F3\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultInputHistoryView { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultInputPasswordView'></a>

## HotKey\.DefaultInputPasswordView Property

Gets the default input password visibility toggle hotkey \(F2\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultInputPasswordView { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultToggleAll'></a>

## HotKey\.DefaultToggleAll Property

Gets the default select all toggle hotkey \(F2\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultToggleAll { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultToggleFullPath'></a>

## HotKey\.DefaultToggleFullPath Property

Gets the default full path toggle hotkey \(Shift\+F3\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultToggleFullPath { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultToggleWildcard'></a>

## HotKey\.DefaultToggleWildcard Property

Gets the default select all childs toggle hotkey \(F4\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultToggleWildcard { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultTooltip'></a>

## HotKey\.DefaultTooltip Property

Gets the default tooltip toggle hotkey \(F1\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultTooltip { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.DefaultTooltipShowHide'></a>

## HotKey\.DefaultTooltipShowHide Property

Gets the default show/hide tooltip hotkey \(Ctrl\+F1\)\.

```csharp
public static PromptPlusLibrary.HotKey DefaultTooltipShowHide { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.Key'></a>

## HotKey\.Key Property

Gets the base [System\.ConsoleKey](https://learn.microsoft.com/en-us/dotnet/api/system.consolekey 'System\.ConsoleKey') of this hotkey\.

```csharp
public System.ConsoleKey Key { get; }
```

#### Property Value
[System\.ConsoleKey](https://learn.microsoft.com/en-us/dotnet/api/system.consolekey 'System\.ConsoleKey')

<a name='PromptPlusLibrary.HotKey.KeyInfo'></a>

## HotKey\.KeyInfo Property

Gets the [System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo') representation of this hotkey\.

```csharp
public System.ConsoleKeyInfo KeyInfo { get; }
```

#### Property Value
[System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo')

### Remarks
[System\.ConsoleKeyInfo\.KeyChar](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo.keychar 'System\.ConsoleKeyInfo\.KeyChar') is only set for an explicit allowlist of keys whose
            [System\.ConsoleKey](https://learn.microsoft.com/en-us/dotnet/api/system.consolekey 'System\.ConsoleKey') numeric value coincides with a real character code \(letters,
            digits, space, Backspace/Tab/Enter/Escape\)\. A numeric\-range check is not safe here: many
            non\-printable keys \(arrows, Home/End/PageUp/PageDown, function keys, \.\.\.\) have enum
            values that fall inside the printable ASCII band by coincidence\. Every key outside the
            allowlist reports `'\0'`, matching what a real console reports for those keys,
            instead of casting the enum value into a misleading char\.

<a name='PromptPlusLibrary.HotKey.Shift'></a>

## HotKey\.Shift Property

Gets a value indicating whether Shift is included\.

```csharp
public bool Shift { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')
### Methods

<a name='PromptPlusLibrary.HotKey.Equals(object)'></a>

## HotKey\.Equals\(object\) Method

Indicates whether this instance and a specified object are equal\.

```csharp
public override bool Equals(object? obj);
```
#### Parameters

<a name='PromptPlusLibrary.HotKey.Equals(object).obj'></a>

`obj` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

The object to compare with the current instance\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') if [obj](HotKey.md#PromptPlusLibrary.HotKey.Equals(object).obj 'PromptPlusLibrary\.HotKey\.Equals\(object\)\.obj') and this instance are the same type and represent the same value; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

<a name='PromptPlusLibrary.HotKey.Equals(System.ConsoleKeyInfo)'></a>

## HotKey\.Equals\(ConsoleKeyInfo\) Method

Determines whether this hotkey matches the provided [System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo')\.

```csharp
public bool Equals(System.ConsoleKeyInfo other);
```
#### Parameters

<a name='PromptPlusLibrary.HotKey.Equals(System.ConsoleKeyInfo).other'></a>

`other` [System\.ConsoleKeyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.consolekeyinfo 'System\.ConsoleKeyInfo')

The key info to compare\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if both the key and modifier set are equal; otherwise, `false`\.

<a name='PromptPlusLibrary.HotKey.GetHashCode()'></a>

## HotKey\.GetHashCode\(\) Method

Returns the hash code for this instance\.

```csharp
public override int GetHashCode();
```

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')  
A 32\-bit signed integer that is the hash code for this instance\.

<a name='PromptPlusLibrary.HotKey.ToString()'></a>

## HotKey\.ToString\(\) Method

Returns the fully qualified type name of this instance\.

```csharp
public override string ToString();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The fully qualified type name\.
### Operators

<a name='PromptPlusLibrary.HotKey.op_Equality(PromptPlusLibrary.HotKey,PromptPlusLibrary.HotKey)'></a>

## HotKey\.operator ==\(HotKey, HotKey\) Operator

Returns `true` if two hotkeys are equal\.

```csharp
public static bool operator ==(PromptPlusLibrary.HotKey left, PromptPlusLibrary.HotKey right);
```
#### Parameters

<a name='PromptPlusLibrary.HotKey.op_Equality(PromptPlusLibrary.HotKey,PromptPlusLibrary.HotKey).left'></a>

`left` [HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.op_Equality(PromptPlusLibrary.HotKey,PromptPlusLibrary.HotKey).right'></a>

`right` [HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.HotKey.op_Inequality(PromptPlusLibrary.HotKey,PromptPlusLibrary.HotKey)'></a>

## HotKey\.operator \!=\(HotKey, HotKey\) Operator

Returns `true` if two hotkeys are not equal\.

```csharp
public static bool operator !=(PromptPlusLibrary.HotKey left, PromptPlusLibrary.HotKey right);
```
#### Parameters

<a name='PromptPlusLibrary.HotKey.op_Inequality(PromptPlusLibrary.HotKey,PromptPlusLibrary.HotKey).left'></a>

`left` [HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.HotKey.op_Inequality(PromptPlusLibrary.HotKey,PromptPlusLibrary.HotKey).right'></a>

`right` [HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')