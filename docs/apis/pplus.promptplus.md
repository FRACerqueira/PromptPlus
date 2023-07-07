# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:PromptPlus 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# PromptPlus

Namespace: PPlus

Contains all controls, methods, properties and extensions for [PromptPlus](./pplus.promptplus.md).

```csharp
public static class PromptPlus
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [PromptPlus](./pplus.promptplus.md)

## Properties

### **Console**

Gets the current Console drive.

```csharp
public static IConsoleBase Console { get; }
```

#### Property Value

[IConsoleBase](./pplus.iconsolebase.md)<br>

### **Config**

Get global properties for all controls.

```csharp
public static Config Config { get; }
```

#### Property Value

[Config](./pplus.controls.config.md)<br>

### **StyleSchema**

Get global Style-Schema for all controls.

```csharp
public static StyleSchema StyleSchema { get; }
```

#### Property Value

[StyleSchema](./pplus.controls.styleschema.md)<br>

### **Provider**

Get provider mode.

```csharp
public static string Provider { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **IsTerminal**

Get Terminal mode.

```csharp
public static bool IsTerminal { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsUnicodeSupported**

Get Unicode Supported.

```csharp
public static bool IsUnicodeSupported { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **SupportsAnsi**

Get SupportsAnsi mode.

```csharp
public static bool SupportsAnsi { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **ColorDepth**

Get Color capacity.[ColorSystem](./pplus.colorsystem.md)

```csharp
public static ColorSystem ColorDepth { get; }
```

#### Property Value

[ColorSystem](./pplus.colorsystem.md)<br>

### **DefaultStyle**

Get default [Style](./pplus.style.md) console.

```csharp
public static Style DefaultStyle { get; }
```

#### Property Value

[Style](./pplus.style.md)<br>

### **PadLeft**

Get screen margin left

```csharp
public static byte PadLeft { get; }
```

#### Property Value

[Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

### **PadRight**

Get screen margin right

```csharp
public static byte PadRight { get; }
```

#### Property Value

[Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

### **BufferWidth**

Gets the width of the buffer area.

```csharp
public static int BufferWidth { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **BufferHeight**

Gets the height of the buffer area.

```csharp
public static int BufferHeight { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **ForegroundColor**

Get/Set Foreground console with color.

```csharp
public static ConsoleColor ForegroundColor { get; set; }
```

#### Property Value

ConsoleColor<br>

### **BackgroundColor**

Get/set BackgroundColor console with color.

```csharp
public static ConsoleColor BackgroundColor { get; set; }
```

#### Property Value

ConsoleColor<br>

### **OverflowStrategy**

Get write Overflow Strategy.

```csharp
public static Overflow OverflowStrategy { get; }
```

#### Property Value

[Overflow](./pplus.overflow.md)<br>

### **CursorVisible**

Gets or sets a value indicating whether the cursor is visible.

```csharp
public static bool CursorVisible { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **CursorLeft**

Gets or sets a value column position of the cursor within the buffer area.

```csharp
public static int CursorLeft { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **CursorTop**

Gets or set the row position of the cursor within the buffer area.

```csharp
public static int CursorTop { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **KeyAvailable**

Gets a value indicating whether a key press is available in the input stream.

```csharp
public static bool KeyAvailable { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsInputRedirected**

Gets a value that indicates whether input has been redirected from the standard input stream.

```csharp
public static bool IsInputRedirected { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **InputEncoding**

Get/set an encoding for standard input stream.

```csharp
public static Encoding InputEncoding { get; }
```

#### Property Value

[Encoding](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding)<br>

### **In**

Get standard input stream.

```csharp
public static TextReader In { get; }
```

#### Property Value

[TextReader](https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader)<br>

### **CodePage**

Get output CodePage.

```csharp
public static int CodePage { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **IsOutputRedirected**

Gets a value that indicates whether output has been redirected from the standard output stream.

```csharp
public static bool IsOutputRedirected { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsErrorRedirected**

Gets a value that indicates whether error has been redirected from the standard error stream.

```csharp
public static bool IsErrorRedirected { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **OutputEncoding**

Get/set an encoding for standard output stream.

```csharp
public static Encoding OutputEncoding { get; set; }
```

#### Property Value

[Encoding](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding)<br>

### **Out**

Get standard output stream.

```csharp
public static TextWriter Out { get; }
```

#### Property Value

[TextWriter](https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter)<br>

### **Error**

Get standard error stream.

```csharp
public static TextWriter Error { get; }
```

#### Property Value

[TextWriter](https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter)<br>

## Methods

### **AddtoList(String, String)**

Create Add to List Control.

```csharp
public static IControlList AddtoList(string prompt, string description)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

[IControlList](./pplus.controls.icontrollist.md)<br>
[IControlList](./pplus.controls.icontrollist.md)

### **AddtoList(String, String, Action&lt;IPromptConfig&gt;)**

Create Add to List Control.

```csharp
public static IControlList AddtoList(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlList](./pplus.controls.icontrollist.md)<br>
[IControlInput](./pplus.controls.icontrolinput.md)

### **AddtoList(String, Action&lt;IPromptConfig&gt;)**

Create Add to List Control.

```csharp
public static IControlList AddtoList(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlList](./pplus.controls.icontrollist.md)<br>
[IControlInput](./pplus.controls.icontrolinput.md)

### **AddtoMaskEditList(String, String)**

Create Add to MaskEdit List Control.

```csharp
public static IControlMaskEditList AddtoMaskEditList(string prompt, string description)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)<br>
[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### **AddtoMaskEditList(String, String, Action&lt;IPromptConfig&gt;)**

Create Add to MaskEdit List Control.

```csharp
public static IControlMaskEditList AddtoMaskEditList(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)<br>
[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### **AddtoMaskEditList(String, Action&lt;IPromptConfig&gt;)**

Create Add to MaskEdit List Control.

```csharp
public static IControlMaskEditList AddtoMaskEditList(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)<br>
[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### **Banner(String)**

Create Banner Control to Write to console AsciiArt(FIGlet).

```csharp
public static IBannerControl Banner(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The text to write

#### Returns

[IBannerControl](./pplus.controls.ibannercontrol.md)<br>
[IBannerControl](./pplus.controls.ibannercontrol.md)

### **AutoComplete(String, Action&lt;IPromptConfig&gt;)**

Create Auto Complete Control.

```csharp
public static IControlAutoComplete AutoComplete(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)<br>
[IControlInput](./pplus.controls.icontrolinput.md)

### **AutoComplete(String, String, Action&lt;IPromptConfig&gt;)**

Create Auto Complete Control.

```csharp
public static IControlAutoComplete AutoComplete(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)<br>
[IControlInput](./pplus.controls.icontrolinput.md)

### **Input(String, Action&lt;IPromptConfig&gt;)**

Create Input Control.

```csharp
public static IControlInput Input(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)<br>
[IControlInput](./pplus.controls.icontrolinput.md)

### **Input(String, String, Action&lt;IPromptConfig&gt;)**

Create Input Control.

```csharp
public static IControlInput Input(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)<br>
[IControlInput](./pplus.controls.icontrolinput.md)

### **MaskEdit(String, Action&lt;IPromptConfig&gt;)**

Create MaskEdit Control.

```csharp
public static IControlMaskEdit MaskEdit(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)<br>
[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### **MaskEdit(String, String, Action&lt;IPromptConfig&gt;)**

Create MaskEdit Control.

```csharp
public static IControlMaskEdit MaskEdit(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)<br>
[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### **Confirm(String, Action&lt;IPromptConfig&gt;)**

Create Confirm control in yes/no mode.
 <br>Yes/No texts come from resources

```csharp
public static IControlKeyPress Confirm(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlKeyPress](./pplus.controls.icontrolkeypress.md)<br>
[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### **Confirm(String, String, Action&lt;IPromptConfig&gt;)**

Create Confirm control in yes/no mode.
 <br>Yes/No texts come from resources

```csharp
public static IControlKeyPress Confirm(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlKeyPress](./pplus.controls.icontrolkeypress.md)<br>
[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### **Confirm(String, ConsoleKey, ConsoleKey, String, Action&lt;IPromptConfig&gt;)**

Create Confirm control in yes/no mode.

```csharp
public static IControlKeyPress Confirm(string prompt, ConsoleKey opcyes, ConsoleKey opcno, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`opcyes` ConsoleKey<br>
yes key.

`opcno` ConsoleKey<br>
no key

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlKeyPress](./pplus.controls.icontrolkeypress.md)<br>
[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### **KeyPress()**

Create Keypress Control to wait a any key input.

```csharp
public static IControlKeyPress KeyPress()
```

#### Returns

[IControlKeyPress](./pplus.controls.icontrolkeypress.md)<br>
[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### **KeyPress(String, Action&lt;IPromptConfig&gt;)**

Create Keypress Control to wait a any key input.

```csharp
public static IControlKeyPress KeyPress(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlKeyPress](./pplus.controls.icontrolkeypress.md)<br>
[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### **KeyPress(String, String, Action&lt;IPromptConfig&gt;)**

Create Keypress Control to wait a any key input.

```csharp
public static IControlKeyPress KeyPress(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlKeyPress](./pplus.controls.icontrolkeypress.md)<br>
[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### **MultiSelect&lt;T&gt;(String, Action&lt;IPromptConfig&gt;)**

Create MultiSelect Control.

```csharp
public static IControlMultiSelect<T> MultiSelect<T>(string prompt, Action<IPromptConfig> config)
```

#### Type Parameters

`T`<br>
Typeof T

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

IControlMultiSelect&lt;T&gt;<br>
[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### **MultiSelect&lt;T&gt;(String, String)**

Create MultiSelect Control.

```csharp
public static IControlMultiSelect<T> MultiSelect<T>(string prompt, string description)
```

#### Type Parameters

`T`<br>
Typeof T

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

IControlMultiSelect&lt;T&gt;<br>
[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### **MultiSelect&lt;T&gt;(String, String, Action&lt;IPromptConfig&gt;)**

Create MultiSelect Control.

```csharp
public static IControlMultiSelect<T> MultiSelect<T>(string prompt, string description, Action<IPromptConfig> config)
```

#### Type Parameters

`T`<br>
Typeof T

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

IControlMultiSelect&lt;T&gt;<br>
[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### **Select&lt;T&gt;(String, Action&lt;IPromptConfig&gt;)**

Create Select Control.

```csharp
public static IControlSelect<T> Select<T>(string prompt, Action<IPromptConfig> config)
```

#### Type Parameters

`T`<br>
Typeof T

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

IControlSelect&lt;T&gt;<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **Select&lt;T&gt;(String, String)**

Create Select Control.

```csharp
public static IControlSelect<T> Select<T>(string prompt, string description)
```

#### Type Parameters

`T`<br>
Typeof T

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

IControlSelect&lt;T&gt;<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **Select&lt;T&gt;(String, String, Action&lt;IPromptConfig&gt;)**

Create Select Control.

```csharp
public static IControlSelect<T> Select<T>(string prompt, string description, Action<IPromptConfig> config)
```

#### Type Parameters

`T`<br>
Typeof T

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

IControlSelect&lt;T&gt;<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **SliderNumber(String, Action&lt;IPromptConfig&gt;)**

Create Slider Number Control.

```csharp
public static IControlSliderNumber SliderNumber(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The description text to write

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)<br>
[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### **SliderNumber(String, String, Action&lt;IPromptConfig&gt;)**

Create Slider Number Control.

```csharp
public static IControlSliderNumber SliderNumber(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)<br>
[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### **SliderSwitch(String, Action&lt;IPromptConfig&gt;)**

Create Slider Switch on/off Control.

```csharp
public static IControlSliderSwitch SliderSwitch(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)<br>
[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### **SliderSwitch(String, String, Action&lt;IPromptConfig&gt;)**

Create Slider Switch on/off Control.

```csharp
public static IControlSliderSwitch SliderSwitch(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)<br>
[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### **ProgressBar(String, String)**

Create Progress Bar Control

```csharp
public static IControlProgressBar<object> ProgressBar(string prompt, string description)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

[IControlProgressBar&lt;Object&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>

### **ProgressBar&lt;T&gt;(ProgressBarType, String, T, String)**

Create Progress Bar Control

```csharp
public static IControlProgressBar<T> ProgressBar<T>(ProgressBarType barType, string prompt, T defaultresult, string description)
```

#### Type Parameters

`T`<br>
Typeof T

#### Parameters

`barType` [ProgressBarType](./pplus.controls.progressbartype.md)<br>
The type Progress Bar. [ProgressBarType](./pplus.controls.progressbartype.md)

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`defaultresult` T<br>
The instance result

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

IControlProgressBar&lt;T&gt;<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **ProgressBar&lt;T&gt;(ProgressBarType, String, T, String, Action&lt;IPromptConfig&gt;)**

Create instance Progress Bar Control

```csharp
public static IControlProgressBar<T> ProgressBar<T>(ProgressBarType barType, string prompt, T defaultresult, string description, Action<IPromptConfig> config)
```

#### Type Parameters

`T`<br>
Typeof T

#### Parameters

`barType` [ProgressBarType](./pplus.controls.progressbartype.md)<br>
The type Progress Bar. [ProgressBarType](./pplus.controls.progressbartype.md)

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`defaultresult` T<br>
The instance result

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

IControlProgressBar&lt;T&gt;<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **WaitProcess(String, String)**

Create Wait Control

```csharp
public static IControlWait WaitProcess(string prompt, string description)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
IEnumerable [StateProcess](./pplus.controls.stateprocess.md) after Run method. [IControlWait](./pplus.controls.icontrolwait.md)

### **WaitProcess(String, String, Action&lt;IPromptConfig&gt;)**

Create Wait Control

```csharp
public static IControlWait WaitProcess(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
IEnumerable [StateProcess](./pplus.controls.stateprocess.md) after Run method. [IControlWait](./pplus.controls.icontrolwait.md)

### **WaitTimer(String, TimeSpan, SpinnersType, Boolean, Action&lt;IPromptConfig&gt;, Nullable&lt;CancellationToken&gt;)**

Create Wait Control with step [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan) delay and run

```csharp
public static void WaitTimer(string prompt, TimeSpan delay, SpinnersType spinnersType, bool showCountdown, Action<IPromptConfig> config, Nullable<CancellationToken> cancellationToken)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`delay` [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>
The delay process

`spinnersType` [SpinnersType](./pplus.controls.spinnerstype.md)<br>
spinners Type [SpinnersType](./pplus.controls.spinnerstype.md)

`showCountdown` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
True show Countdown, otherwise 'no'

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

`cancellationToken` [Nullable&lt;CancellationToken&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
[CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) for control

### **BrowserMultiSelect(String, String)**

Create MultiSelect Browser Control.

```csharp
public static IControlMultiSelectBrowser BrowserMultiSelect(string prompt, string description)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)<br>
[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### **BrowserMultiSelect(String, String, Action&lt;IPromptConfig&gt;)**

Create MultiSelect Browser Control.

```csharp
public static IControlMultiSelectBrowser BrowserMultiSelect(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)<br>
[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)

### **BrowserMultiSelect(String, Action&lt;IPromptConfig&gt;)**

Create MultiSelect Browser Control.

```csharp
public static IControlMultiSelectBrowser BrowserMultiSelect(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlMultiSelectBrowser](./pplus.controls.icontrolmultiselectbrowser.md)<br>
[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **Browser(String, String)**

Create Browser Control.

```csharp
public static IControlSelectBrowser Browser(string prompt, string description)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)<br>
[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **Browser(String, String, Action&lt;IPromptConfig&gt;)**

Create Browser Control.

```csharp
public static IControlSelectBrowser Browser(string prompt, string description, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)<br>
[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **Browser(String, Action&lt;IPromptConfig&gt;)**

Create Browser Control.

```csharp
public static IControlSelectBrowser Browser(string prompt, Action<IPromptConfig> config)
```

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)<br>
[IControlSelectBrowser](./pplus.controls.icontrolselectbrowser.md)

### **TreeViewMultiSelect&lt;T&gt;(String, String)**

Create TreeView MultiSelect Control.

```csharp
public static IControlTreeViewMultiSelect<T> TreeViewMultiSelect<T>(string prompt, string description)
```

#### Type Parameters

`T`<br>

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

IControlTreeViewMultiSelect&lt;T&gt;<br>
[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **TreeViewMultiSelect&lt;T&gt;(String, String, Action&lt;IPromptConfig&gt;)**

Create TreeView MultiSelect Control.

```csharp
public static IControlTreeViewMultiSelect<T> TreeViewMultiSelect<T>(string prompt, string description, Action<IPromptConfig> config)
```

#### Type Parameters

`T`<br>

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

IControlTreeViewMultiSelect&lt;T&gt;<br>
[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **TreeViewMultiSelect&lt;T&gt;(String, Action&lt;IPromptConfig&gt;)**

Create TreeView MultiSelect Control.

```csharp
public static IControlTreeViewMultiSelect<T> TreeViewMultiSelect<T>(string prompt, Action<IPromptConfig> config)
```

#### Type Parameters

`T`<br>

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

IControlTreeViewMultiSelect&lt;T&gt;<br>
[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **TreeView&lt;T&gt;(String, String)**

Create TreeView Control.

```csharp
public static IControlTreeViewSelect<T> TreeView<T>(string prompt, string description)
```

#### Type Parameters

`T`<br>

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

#### Returns

IControlTreeViewSelect&lt;T&gt;<br>
[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### **TreeView&lt;T&gt;(String, String, Action&lt;IPromptConfig&gt;)**

Create TreeView Control.

```csharp
public static IControlTreeViewSelect<T> TreeView<T>(string prompt, string description, Action<IPromptConfig> config)
```

#### Type Parameters

`T`<br>

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The description text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

IControlTreeViewSelect&lt;T&gt;<br>
[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### **TreeView&lt;T&gt;(String, Action&lt;IPromptConfig&gt;)**

Create TreeView Control.

```csharp
public static IControlTreeViewSelect<T> TreeView<T>(string prompt, Action<IPromptConfig> config)
```

#### Type Parameters

`T`<br>

#### Parameters

`prompt` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The prompt text to write

`config` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The config action [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

IControlTreeViewSelect&lt;T&gt;<br>
[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### **IsPressSpecialKey(ConsoleKeyInfo, ConsoleKey, ConsoleModifiers)**

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

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressEnterKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Enter Key

```csharp
public static bool IsPressEnterKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
If `true`true accept 'CTRL+J'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsLowersCurrentWord(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Lowers Current Word Emacs Key 
 <br>Alt+L = Lowers the case of every character from the cursor's position to the end of the current words

```csharp
public static bool IsLowersCurrentWord(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsClearBeforeCursor(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Clear Before Cursor Emacs Key 
 <br>Ctrl+U = Clears the line content before the cursor

```csharp
public static bool IsClearBeforeCursor(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsClearAfterCursor(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Clear After Cursor Emacs Key 
 <br>Ctrl+K = Clears the line content after the cursor

```csharp
public static bool IsClearAfterCursor(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsClearWordBeforeCursor(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Clear Word Before Cursor Emacs Key 
 <br>Ctrl+W = Clears the word before the cursor

```csharp
public static bool IsClearWordBeforeCursor(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsClearWordAfterCursor(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Clear Word After Cursor Emacs Key 
 <br>Ctrl+D = Clears the word after the cursor

```csharp
public static bool IsClearWordAfterCursor(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsCapitalizeOverCursor(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Capitalize Over Cursor Emacs Key 
 <br>Alt+C = Capitalizes the character under the cursor and moves to the end of the word

```csharp
public static bool IsCapitalizeOverCursor(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsForwardWord(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Forward Word Emacs Key 
 <br>Alt+F = Moves the cursor forward one word.

```csharp
public static bool IsForwardWord(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsBackwardWord(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Backward Word Emacs Key 
 <br>Alt+B = Moves the cursor backward one word.

```csharp
public static bool IsBackwardWord(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsUppersCurrentWord(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Lowers Current Word Emacs Key 
 <br>Alt+U = Upper the case of every character from the cursor's position to the end of the current word

```csharp
public static bool IsUppersCurrentWord(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsTransposePrevious(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Transpose Previous Emacs Key 
 <br>Ctrl+T = Transpose the previous two characters

```csharp
public static bool IsTransposePrevious(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsClearContent(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Clear Emacs Key 
 <br>Ctrl+L = Clears the content

```csharp
public static bool IsClearContent(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressTabKey(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Tab Key

```csharp
public static bool IsPressTabKey(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressShiftTabKey(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Shift + Tab Key

```csharp
public static bool IsPressShiftTabKey(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressEndKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is End Key

```csharp
public static bool IsPressEndKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true`true accept 'CTRL+E'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressHomeKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is End Key

```csharp
public static bool IsPressHomeKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true`true accept 'CTRL+A'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressBackspaceKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is End Key

```csharp
public static bool IsPressBackspaceKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true`true accept 'CTRL+H'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressDeleteKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Delete Key

```csharp
public static bool IsPressDeleteKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true`true accept 'CTRL+D'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressLeftArrowKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Left Arrow Key

```csharp
public static bool IsPressLeftArrowKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true`true accept 'CTRL+B'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressSpaceKey(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Space Key

```csharp
public static bool IsPressSpaceKey(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressRightArrowKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Right Arrow Key

```csharp
public static bool IsPressRightArrowKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true`true accept 'CTRL+F'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressUpArrowKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Up Arrow Key

```csharp
public static bool IsPressUpArrowKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true`true accept 'CTRL+P'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressDownArrowKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is Down Arrow Key

```csharp
public static bool IsPressDownArrowKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true`true accept 'CTRL+N'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressPageUpKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is PageUp Key

```csharp
public static bool IsPressPageUpKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true`true accept 'Alt+P'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressPageDownKey(ConsoleKeyInfo, Boolean)**

Check ConsoleKeyInfo is PageDown Key

```csharp
public static bool IsPressPageDownKey(ConsoleKeyInfo keyinfo, bool emacskeys)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

`emacskeys` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if `true`true accept 'Alt+N'

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **IsPressEscKey(ConsoleKeyInfo)**

Check ConsoleKeyInfo is Esc Key

```csharp
public static bool IsPressEscKey(ConsoleKeyInfo keyinfo)
```

#### Parameters

`keyinfo` ConsoleKeyInfo<br>
to check

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if equal otherwise `false`false.

### **ToCase(ConsoleKeyInfo, CaseOptions)**

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

ConsoleKeyInfo<br>
converted

### **Reset()**

Reset all config and properties to default values

```csharp
public static void Reset()
```

### **Setup(Action&lt;ProfileSetup&gt;)**

Overwrite current console with new console profile.
 <br>After overwrite the new console the screeen is clear<br>and all Style-Schema are updated with backgoundcolor console

```csharp
public static void Setup(Action<ProfileSetup> config)
```

#### Parameters

`config` [Action&lt;ProfileSetup&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
Action with [ProfileSetup](./pplus.profilesetup.md) to configuration

### **Write(Exception, Nullable&lt;Style&gt;, Boolean)**

Write a Exception to output console.

```csharp
public static int Write(Exception value, Nullable<Style> style, bool clearrestofline)
```

#### Parameters

`value` [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Exception to write

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of text

`clearrestofline` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Clear rest of line after write

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of lines write on console

### **Write(String, Nullable&lt;Style&gt;, Boolean)**

Write a text to output console.

```csharp
public static int Write(string value, Nullable<Style> style, bool clearrestofline)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text to write

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of text

`clearrestofline` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Clear rest of line after write

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of lines write on console

### **WriteLine(Exception, Nullable&lt;Style&gt;, Boolean)**

Write a exception to output console with line terminator.

```csharp
public static int WriteLine(Exception value, Nullable<Style> style, bool clearrestofline)
```

#### Parameters

`value` [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Exception to write

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of text

`clearrestofline` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Clear rest of line after write

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of lines write on console

### **WriteLine(String, Nullable&lt;Style&gt;, Boolean)**

Write a text to output console with line terminator.

```csharp
public static int WriteLine(string value, Nullable<Style> style, bool clearrestofline)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text to write

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of text

`clearrestofline` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Clear rest of line after write

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of lines write on console

### **ResetColor()**

Reset colors to default values.

```csharp
public static void ResetColor()
```

### **MoveCursor(CursorDirection, Int32)**

Moves the cursor relative to the current position.

```csharp
public static void MoveCursor(CursorDirection direction, int steps)
```

#### Parameters

`direction` [CursorDirection](./pplus.cursordirection.md)<br>
The direction to move the cursor.

`steps` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The number of steps to move the cursor.

### **SetCursorPosition(Int32, Int32)**

Sets the position of the cursor.

```csharp
public static void SetCursorPosition(int left, int top)
```

#### Parameters

`left` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The column position of the cursor. Columns are numbered from left to right starting at 0.

`top` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The row position of the cursor. Rows are numbered from top to bottom starting at 0.

### **ReadKey(Boolean)**

Obtains the next character or function key pressed by the user.

```csharp
public static ConsoleKeyInfo ReadKey(bool intercept)
```

#### Parameters

`intercept` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.

#### Returns

ConsoleKeyInfo<br>
<br>An oject that describes the System.ConsoleKey constant and Unicode character,<br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo<br>t also describes, in a bitwise combination of System.ConsoleModifiers values,<br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously<br>with the console key.

### **ReadLine()**

<br>Read the line from stream. A line is defined as a sequence of characters followed by<br>a car return ('\r'), a line feed ('\n'), or a carriage return<br>immedy followed by a line feed. The resulting string does not<br>contain the terminating carriage return and/or line feed.

```csharp
public static string ReadLine()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The returned value is null if the end of the input stream has been reached.

### **WaitKeypress(Boolean, Nullable&lt;CancellationToken&gt;)**

Wait Keypress from standard input stream

```csharp
public static Nullable<ConsoleKeyInfo> WaitKeypress(bool intercept, Nullable<CancellationToken> cancellationToken)
```

#### Parameters

`intercept` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.

`cancellationToken` [Nullable&lt;CancellationToken&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The token to monitor for cancellation requests.

#### Returns

[Nullable&lt;ConsoleKeyInfo&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
<br>An oject that describes the System.ConsoleKey constant and Unicode character,<br>if any, that correspond to the pressed console key. The System.ConsoleKeyInfo<br>t also describes, in a bitwise combination of System.ConsoleModifiers values,<br>er one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously<br>with the console key.

### **GetCursorPosition()**

Gets the position of the cursor.

```csharp
public static ValueTuple<int, int> GetCursorPosition()
```

#### Returns

[ValueTuple&lt;Int32, Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.valuetuple-2)<br>
The column and row position of the cursor.

### **SetIn(TextReader)**

set standard input stream.

```csharp
public static void SetIn(TextReader value)
```

#### Parameters

`value` [TextReader](https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader)<br>
A stream that is the new standard input.

### **SetOut(TextWriter)**

set standard output stream.

```csharp
public static void SetOut(TextWriter value)
```

#### Parameters

`value` [TextWriter](https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter)<br>
A stream that is the new standard output.

### **SetError(TextWriter)**

set standard error stream.

```csharp
public static void SetError(TextWriter value)
```

#### Parameters

`value` [TextWriter](https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter)<br>
A stream that is the new standard error.

### **Clear()**

Clears the console buffer and corresponding console window of display information.
 <br>Move cursor fom top console.

```csharp
public static void Clear()
```

### **Clear(Nullable&lt;Color&gt;)**

Clears the console buffer with [Color](./pplus.color.md) and set BackgroundColor with [Color](./pplus.color.md)

```csharp
public static void Clear(Nullable<Color> backcolor)
```

#### Parameters

`backcolor` [Nullable&lt;Color&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [Color](./pplus.color.md) Background

### **Beep()**

Plays the sound of a beep through the console speaker.

```csharp
public static void Beep()
```

### **ClearLine(Nullable&lt;Int32&gt;, Nullable&lt;Style&gt;)**

Clear line

```csharp
public static void ClearLine(Nullable<int> row, Nullable<Style> style)
```

#### Parameters

`row` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The row to clear

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The style color to clear.

### **ClearLine(IConsoleBase, Nullable&lt;Int32&gt;, Nullable&lt;Style&gt;)**

Clear line

```csharp
public static void ClearLine(IConsoleBase consolebase, Nullable<int> row, Nullable<Style> style)
```

#### Parameters

`consolebase` [IConsoleBase](./pplus.iconsolebase.md)<br>
The [IConsoleBase](./pplus.iconsolebase.md)

`row` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The row to clear

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The style color to clear.

### **ClearRestOfLine(Nullable&lt;Style&gt;)**

Clear rest of current line

```csharp
public static void ClearRestOfLine(Nullable<Style> style)
```

#### Parameters

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The style color to clear.

### **ClearRestOfLine(IConsoleBase, Nullable&lt;Style&gt;)**

Clear rest of current line

```csharp
public static void ClearRestOfLine(IConsoleBase consolebase, Nullable<Style> style)
```

#### Parameters

`consolebase` [IConsoleBase](./pplus.iconsolebase.md)<br>
The [IConsoleBase](./pplus.iconsolebase.md)

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The style color to clear.

### **ReadLineWithEmacs(Nullable&lt;UInt32&gt;, CaseOptions, Action&lt;String, Int32&gt;)**

<br>Read the line from stream using Emacs keyboard shortcuts. A line is defined as a sequence of characters followed by<br>a car return ('\r'), a line feed ('\n'), or a carriage return<br>immedy followed by a line feed. The resulting string does not<br>contain the terminating carriage return and/or line feed.

```csharp
public static string ReadLineWithEmacs(Nullable<uint> maxlenght, CaseOptions caseOptions, Action<string, int> afteraccept)
```

#### Parameters

`maxlenght` [Nullable&lt;UInt32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The input Max-lenght

`caseOptions` [CaseOptions](./pplus.controls.caseoptions.md)<br>
The input [CaseOptions](./pplus.controls.caseoptions.md)

`afteraccept` [Action&lt;String, Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-2)<br>
The user action after each accepted keystroke. Firt param is input text, Second param is relative cursor position of text

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The string input value.

### **SingleDash(String, DashOptions, Int32, Nullable&lt;Style&gt;)**

Writes text line representation whie colors and Write single dash after.

```csharp
public static void SingleDash(string value, DashOptions dashOptions, int extralines, Nullable<Style> style)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The value to write.

`dashOptions` [DashOptions](./pplus.dashoptions.md)<br>
[DashOptions](./pplus.dashoptions.md) character

`extralines` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number lines to write after write value

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [Style](./pplus.style.md) to write.

### **SingleDash(IConsoleBase, String, DashOptions, Int32, Nullable&lt;Style&gt;)**

Writes text line representation whie colors and Write single dash after.

```csharp
public static void SingleDash(IConsoleBase consoleBase, string value, DashOptions dashOptions, int extralines, Nullable<Style> style)
```

#### Parameters

`consoleBase` [IConsoleBase](./pplus.iconsolebase.md)<br>

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The value to write.

`dashOptions` [DashOptions](./pplus.dashoptions.md)<br>
[DashOptions](./pplus.dashoptions.md) character

`extralines` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number lines to write after write value

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [Style](./pplus.style.md) to write.

### **WriteLines(Int32)**

Write lines with line terminator

```csharp
public static void WriteLines(int steps)
```

#### Parameters

`steps` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Numbers de lines.

### **WriteLines(IConsoleBase, Int32)**

Write lines with line terminator

```csharp
public static void WriteLines(IConsoleBase consoleBase, int steps)
```

#### Parameters

`consoleBase` [IConsoleBase](./pplus.iconsolebase.md)<br>

`steps` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Numbers de lines.

### **DoubleDash(String, DashOptions, Int32, Nullable&lt;Style&gt;)**

Writes text line representation whie colors in a pair of lines of dashes.

```csharp
public static void DoubleDash(string value, DashOptions dashOptions, int extralines, Nullable<Style> style)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The value to write.

`dashOptions` [DashOptions](./pplus.dashoptions.md)<br>
[DashOptions](./pplus.dashoptions.md) character

`extralines` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number lines to write after write value

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [Style](./pplus.style.md) to write.

### **DoubleDash(IConsoleBase, String, DashOptions, Int32, Nullable&lt;Style&gt;)**

Writes text line representation whie colors in a pair of lines of dashes.

```csharp
public static void DoubleDash(IConsoleBase consoleBase, string value, DashOptions dashOptions, int extralines, Nullable<Style> style)
```

#### Parameters

`consoleBase` [IConsoleBase](./pplus.iconsolebase.md)<br>

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The value to write.

`dashOptions` [DashOptions](./pplus.dashoptions.md)<br>
[DashOptions](./pplus.dashoptions.md) character

`extralines` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number lines to write after write value

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [Style](./pplus.style.md) to write.


- - -
[**Back to List Api**](./apis.md)
