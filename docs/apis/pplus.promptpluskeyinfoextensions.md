# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:PromptPlusKeyInfoExtensions 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# PromptPlusKeyInfoExtensions

Namespace: PPlus

Represents KeyInfo Extensions

```csharp
public static class PromptPlusKeyInfoExtensions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [PromptPlusKeyInfoExtensions](./pplus.promptpluskeyinfoextensions.md)

## Methods

### <a id="methods-isbackwardword"/>**IsBackwardWord(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Backward Word Emacs Key 
 <br>Alt+B = Moves the cursor backward one word.

```csharp
public static bool IsBackwardWord(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-iscapitalizeovercursor"/>**IsCapitalizeOverCursor(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Capitalize Over Cursor Emacs Key 
 <br>Alt+C = Capitalizes the character under the cursor and moves to the end of the word

```csharp
public static bool IsCapitalizeOverCursor(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-isclearaftercursor"/>**IsClearAfterCursor(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Clear After Cursor Emacs Key 
 <br>Ctrl+K = Clears the line content after the cursor

```csharp
public static bool IsClearAfterCursor(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-isclearbeforecursor"/>**IsClearBeforeCursor(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Clear Before Cursor Emacs Key 
 <br>Ctrl+U = Clears the line content before the cursor

```csharp
public static bool IsClearBeforeCursor(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-isclearcontent"/>**IsClearContent(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Clear Emacs Key 
 <br>Ctrl+L = Clears the content

```csharp
public static bool IsClearContent(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-isclearwordaftercursor"/>**IsClearWordAfterCursor(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Clear Word After Cursor Emacs Key 
 <br>Ctrl+D = Clears the word after the cursor

```csharp
public static bool IsClearWordAfterCursor(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-isclearwordbeforecursor"/>**IsClearWordBeforeCursor(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Clear Word Before Cursor Emacs Key 
 <br>Ctrl+W = Clears the word before the cursor

```csharp
public static bool IsClearWordBeforeCursor(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-isforwardword"/>**IsForwardWord(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Forward Word Emacs Key 
 <br>Alt+F = Moves the cursor forward one word.

```csharp
public static bool IsForwardWord(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-islowerscurrentword"/>**IsLowersCurrentWord(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Lowers Current Word Emacs Key 
 <br>Alt+L = Lowers the case of every character from the cursor's position to the end of the current words

```csharp
public static bool IsLowersCurrentWord(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-isnoresponsekey"/>**IsNoResponseKey(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Yes key

```csharp
public static bool IsNoResponseKey(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressbackspacekey"/>**IsPressBackspaceKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is End Key

```csharp
public static bool IsPressBackspaceKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true` accept 'CTRL+H'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressdeletekey"/>**IsPressDeleteKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Delete Key

```csharp
public static bool IsPressDeleteKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true` accept 'CTRL+D'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressdownarrowkey"/>**IsPressDownArrowKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Down Arrow Key

```csharp
public static bool IsPressDownArrowKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true` accept 'CTRL+N'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressendkey"/>**IsPressEndKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is End Key

```csharp
public static bool IsPressEndKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true` accept 'CTRL+E'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressenterkey"/>**IsPressEnterKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Enter Key

```csharp
public static bool IsPressEnterKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
If `true` accept 'CTRL+J'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressesckey"/>**IsPressEscKey(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Esc Key

```csharp
public static bool IsPressEscKey(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispresshomekey"/>**IsPressHomeKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is End Key

```csharp
public static bool IsPressHomeKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true` accept 'CTRL+A'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressleftarrowkey"/>**IsPressLeftArrowKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Left Arrow Key

```csharp
public static bool IsPressLeftArrowKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true` accept 'CTRL+B'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispresspagedownkey"/>**IsPressPageDownKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is PageDown Key

```csharp
public static bool IsPressPageDownKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true` accept 'Alt+N'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispresspageupkey"/>**IsPressPageUpKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is PageUp Key

```csharp
public static bool IsPressPageUpKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true` accept 'Alt+P'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressrightarrowkey"/>**IsPressRightArrowKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Right Arrow Key

```csharp
public static bool IsPressRightArrowKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true` accept 'CTRL+F'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressshifttabkey"/>**IsPressShiftTabKey(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Shift + Tab Key

```csharp
public static bool IsPressShiftTabKey(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressspacekey"/>**IsPressSpaceKey(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Space Key

```csharp
public static bool IsPressSpaceKey(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressspecialkey"/>**IsPressSpecialKey(ConsoleKeyInfo, ConsoleKey, ConsoleModifiers)**

Check ConsoleKeyInfo is Special Key

```csharp
public static bool IsPressSpecialKey(ConsoleKeyInfo keyinfo, ConsoleKey key, ConsoleModifiers modifier)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`key` ConsoleKey<br>
to compare

`modifier` ConsoleModifiers<br>
to compare

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispresstabkey"/>**IsPressTabKey(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Tab Key

```csharp
public static bool IsPressTabKey(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-ispressuparrowkey"/>**IsPressUpArrowKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Up Arrow Key

```csharp
public static bool IsPressUpArrowKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true` accept 'CTRL+P'

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-istransposeprevious"/>**IsTransposePrevious(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Transpose Previous Emacs Key 
 <br>Ctrl+T = Transpose the previous two characters

```csharp
public static bool IsTransposePrevious(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-isupperscurrentword"/>**IsUppersCurrentWord(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Lowers Current Word Emacs Key 
 <br>Alt+U = Upper the case of every character from the cursor's position to the end of the current word

```csharp
public static bool IsUppersCurrentWord(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-isyesresponsekey"/>**IsYesResponseKey(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Yes key

```csharp
public static bool IsYesResponseKey(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

`true` if equal otherwise `false`.

### <a id="methods-tocase"/>**ToCase(ConsoleKeyInfo, CaseOptions)**

Convert  KeyChar to Uppercase / Lowercase

```csharp
public static ConsoleKeyInfo ToCase(ConsoleKeyInfo keyinfo, CaseOptions value)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to convert

`value` [CaseOptions](./pplus.controls.caseoptions.md)<br>
The [CaseOptions](./pplus.controls.caseoptions.md)

#### Returns

converted


- - -
[**Back to List Api**](./apis.md)
