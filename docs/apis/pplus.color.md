# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:Color 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# Color

Namespace: PPlus

Represents a color.

```csharp
public struct Color
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [Color](./pplus.color.md)<br>
Implements [IEquatable&lt;Color&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1)

## Properties

### **DefaultBackcolor**

Gets the default Backcolor color.

```csharp
public static Color DefaultBackcolor { get; internal set; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DefaultForecolor**

Gets the default Forecolor color.

```csharp
public static Color DefaultForecolor { get; internal set; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **R**

Gets the red component.

```csharp
public byte R { get; }
```

#### Property Value

[Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

### **G**

Gets the green component.

```csharp
public byte G { get; }
```

#### Property Value

[Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

### **B**

Gets the blue component.

```csharp
public byte B { get; }
```

#### Property Value

[Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

### **Black**

Gets the color "Black" (RGB 0,0,0).

```csharp
public static Color Black { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Maroon**

Gets the color "Maroon" (RGB 128,0,0).

```csharp
public static Color Maroon { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Green**

Gets the color "Green" (RGB 0,128,0).

```csharp
public static Color Green { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Olive**

Gets the color "Olive" (RGB 128,128,0).

```csharp
public static Color Olive { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Navy**

Gets the color "Navy" (RGB 0,0,128).

```csharp
public static Color Navy { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Purple**

Gets the color "Purple" (RGB 128,0,128).

```csharp
public static Color Purple { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Teal**

Gets the color "Teal" (RGB 0,128,128).

```csharp
public static Color Teal { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Silver**

Gets the color "Silver" (RGB 192,192,192).

```csharp
public static Color Silver { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey**

Gets the color "Grey" (RGB 128,128,128).

```csharp
public static Color Grey { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Red**

Gets the color "Red" (RGB 255,0,0).

```csharp
public static Color Red { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Lime**

Gets the color "Lime" (RGB 0,255,0).

```csharp
public static Color Lime { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Yellow**

Gets the color "Yellow" (RGB 255,255,0).

```csharp
public static Color Yellow { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Blue**

Gets the color "Blue" (RGB 0,0,255).

```csharp
public static Color Blue { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Fuchsia**

Gets the color "Fuchsia" (RGB 255,0,255).

```csharp
public static Color Fuchsia { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Aqua**

Gets the color "Aqua" (RGB 0,255,255).

```csharp
public static Color Aqua { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **White**

Gets the color "White" (RGB 255,255,255).

```csharp
public static Color White { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey0**

Gets the color "Grey0" (RGB 0,0,0).

```csharp
public static Color Grey0 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **NavyBlue**

Gets the color "NavyBlue" (RGB 0,0,95).

```csharp
public static Color NavyBlue { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkBlue**

Gets the color "DarkBlue" (RGB 0,0,135).

```csharp
public static Color DarkBlue { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Blue3**

Gets the color "Blue3" (RGB 0,0,175).

```csharp
public static Color Blue3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Blue3_1**

Gets the color "Blue3_1" (RGB 0,0,215).

```csharp
public static Color Blue3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Blue1**

Gets the color "Blue1" (RGB 0,0,255).

```csharp
public static Color Blue1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkGreen**

Gets the color "DarkGreen" (RGB 0,95,0).

```csharp
public static Color DarkGreen { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepSkyBlue4**

Gets the color "DeepSkyBlue4" (RGB 0,95,95).

```csharp
public static Color DeepSkyBlue4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepSkyBlue4_1**

Gets the color "DeepSkyBlue4_1" (RGB 0,95,135).

```csharp
public static Color DeepSkyBlue4_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepSkyBlue4_2**

Gets the color "DeepSkyBlue4_2" (RGB 0,95,175).

```csharp
public static Color DeepSkyBlue4_2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DodgerBlue3**

Gets the color "DodgerBlue3" (RGB 0,95,215).

```csharp
public static Color DodgerBlue3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DodgerBlue2**

Gets the color "DodgerBlue2" (RGB 0,95,255).

```csharp
public static Color DodgerBlue2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Green4**

Gets the color "Green4" (RGB 0,135,0).

```csharp
public static Color Green4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SpringGreen4**

Gets the color "SpringGreen4" (RGB 0,135,95).

```csharp
public static Color SpringGreen4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Turquoise4**

Gets the color "Turquoise4" (RGB 0,135,135).

```csharp
public static Color Turquoise4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepSkyBlue3**

Gets the color "DeepSkyBlue3" (RGB 0,135,175).

```csharp
public static Color DeepSkyBlue3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepSkyBlue3_1**

Gets the color "DeepSkyBlue3_1" (RGB 0,135,215).

```csharp
public static Color DeepSkyBlue3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DodgerBlue1**

Gets the color "DodgerBlue1" (RGB 0,135,255).

```csharp
public static Color DodgerBlue1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Green3**

Gets the color "Green3" (RGB 0,175,0).

```csharp
public static Color Green3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SpringGreen3**

Gets the color "SpringGreen3" (RGB 0,175,95).

```csharp
public static Color SpringGreen3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkCyan**

Gets the color "DarkCyan" (RGB 0,175,135).

```csharp
public static Color DarkCyan { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSeaGreen**

Gets the color "LightSeaGreen" (RGB 0,175,175).

```csharp
public static Color LightSeaGreen { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepSkyBlue2**

Gets the color "DeepSkyBlue2" (RGB 0,175,215).

```csharp
public static Color DeepSkyBlue2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepSkyBlue1**

Gets the color "DeepSkyBlue1" (RGB 0,175,255).

```csharp
public static Color DeepSkyBlue1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Green3_1**

Gets the color "Green3_1" (RGB 0,215,0).

```csharp
public static Color Green3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SpringGreen3_1**

Gets the color "SpringGreen3_1" (RGB 0,215,95).

```csharp
public static Color SpringGreen3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SpringGreen2**

Gets the color "SpringGreen2" (RGB 0,215,135).

```csharp
public static Color SpringGreen2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Cyan3**

Gets the color "Cyan3" (RGB 0,215,175).

```csharp
public static Color Cyan3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkTurquoise**

Gets the color "DarkTurquoise" (RGB 0,215,215).

```csharp
public static Color DarkTurquoise { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Turquoise2**

Gets the color "Turquoise2" (RGB 0,215,255).

```csharp
public static Color Turquoise2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Green1**

Gets the color "Green1" (RGB 0,255,0).

```csharp
public static Color Green1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SpringGreen2_1**

Gets the color "SpringGreen2_1" (RGB 0,255,95).

```csharp
public static Color SpringGreen2_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SpringGreen1**

Gets the color "SpringGreen1" (RGB 0,255,135).

```csharp
public static Color SpringGreen1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumSpringGreen**

Gets the color "MediumSpringGreen" (RGB 0,255,175).

```csharp
public static Color MediumSpringGreen { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Cyan2**

Gets the color "Cyan2" (RGB 0,255,215).

```csharp
public static Color Cyan2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Cyan1**

Gets the color "Cyan1" (RGB 0,255,255).

```csharp
public static Color Cyan1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkRed**

Gets the color "DarkRed" (RGB 95,0,0).

```csharp
public static Color DarkRed { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepPink4**

Gets the color "DeepPink4" (RGB 95,0,95).

```csharp
public static Color DeepPink4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Purple4**

Gets the color "Purple4" (RGB 95,0,135).

```csharp
public static Color Purple4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Purple4_1**

Gets the color "Purple4_1" (RGB 95,0,175).

```csharp
public static Color Purple4_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Purple3**

Gets the color "Purple3" (RGB 95,0,215).

```csharp
public static Color Purple3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **BlueViolet**

Gets the color "BlueViolet" (RGB 95,0,255).

```csharp
public static Color BlueViolet { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Orange4**

Gets the color "Orange4" (RGB 95,95,0).

```csharp
public static Color Orange4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey37**

Gets the color "Grey37" (RGB 95,95,95).

```csharp
public static Color Grey37 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumPurple4**

Gets the color "MediumPurple4" (RGB 95,95,135).

```csharp
public static Color MediumPurple4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SlateBlue3**

Gets the color "SlateBlue3" (RGB 95,95,175).

```csharp
public static Color SlateBlue3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SlateBlue3_1**

Gets the color "SlateBlue3_1" (RGB 95,95,215).

```csharp
public static Color SlateBlue3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **RoyalBlue1**

Gets the color "RoyalBlue1" (RGB 95,95,255).

```csharp
public static Color RoyalBlue1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Chartreuse4**

Gets the color "Chartreuse4" (RGB 95,135,0).

```csharp
public static Color Chartreuse4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSeaGreen4**

Gets the color "DarkSeaGreen4" (RGB 95,135,95).

```csharp
public static Color DarkSeaGreen4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **PaleTurquoise4**

Gets the color "PaleTurquoise4" (RGB 95,135,135).

```csharp
public static Color PaleTurquoise4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SteelBlue**

Gets the color "SteelBlue" (RGB 95,135,175).

```csharp
public static Color SteelBlue { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SteelBlue3**

Gets the color "SteelBlue3" (RGB 95,135,215).

```csharp
public static Color SteelBlue3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **CornflowerBlue**

Gets the color "CornflowerBlue" (RGB 95,135,255).

```csharp
public static Color CornflowerBlue { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Chartreuse3**

Gets the color "Chartreuse3" (RGB 95,175,0).

```csharp
public static Color Chartreuse3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSeaGreen4_1**

Gets the color "DarkSeaGreen4_1" (RGB 95,175,95).

```csharp
public static Color DarkSeaGreen4_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **CadetBlue**

Gets the color "CadetBlue" (RGB 95,175,135).

```csharp
public static Color CadetBlue { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **CadetBlue_1**

Gets the color "CadetBlue_1" (RGB 95,175,175).

```csharp
public static Color CadetBlue_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SkyBlue3**

Gets the color "SkyBlue3" (RGB 95,175,215).

```csharp
public static Color SkyBlue3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SteelBlue1**

Gets the color "SteelBlue1" (RGB 95,175,255).

```csharp
public static Color SteelBlue1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Chartreuse3_1**

Gets the color "Chartreuse3_1" (RGB 95,215,0).

```csharp
public static Color Chartreuse3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **PaleGreen3**

Gets the color "PaleGreen3" (RGB 95,215,95).

```csharp
public static Color PaleGreen3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SeaGreen3**

Gets the color "SeaGreen3" (RGB 95,215,135).

```csharp
public static Color SeaGreen3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Aquamarine3**

Gets the color "Aquamarine3" (RGB 95,215,175).

```csharp
public static Color Aquamarine3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumTurquoise**

Gets the color "MediumTurquoise" (RGB 95,215,215).

```csharp
public static Color MediumTurquoise { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SteelBlue1_1**

Gets the color "SteelBlue1_1" (RGB 95,215,255).

```csharp
public static Color SteelBlue1_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Chartreuse2**

Gets the color "Chartreuse2" (RGB 95,255,0).

```csharp
public static Color Chartreuse2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SeaGreen2**

Gets the color "SeaGreen2" (RGB 95,255,95).

```csharp
public static Color SeaGreen2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SeaGreen1**

Gets the color "SeaGreen1" (RGB 95,255,135).

```csharp
public static Color SeaGreen1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SeaGreen1_1**

Gets the color "SeaGreen1_1" (RGB 95,255,175).

```csharp
public static Color SeaGreen1_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Aquamarine1**

Gets the color "Aquamarine1" (RGB 95,255,215).

```csharp
public static Color Aquamarine1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSlateGray2**

Gets the color "DarkSlateGray2" (RGB 95,255,255).

```csharp
public static Color DarkSlateGray2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkRed_1**

Gets the color "DarkRed_1" (RGB 135,0,0).

```csharp
public static Color DarkRed_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepPink4_1**

Gets the color "DeepPink4_1" (RGB 135,0,95).

```csharp
public static Color DeepPink4_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkMagenta**

Gets the color "DarkMagenta" (RGB 135,0,135).

```csharp
public static Color DarkMagenta { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkMagenta_1**

Gets the color "DarkMagenta_1" (RGB 135,0,175).

```csharp
public static Color DarkMagenta_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkViolet**

Gets the color "DarkViolet" (RGB 135,0,215).

```csharp
public static Color DarkViolet { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Purple_1**

Gets the color "Purple_1" (RGB 135,0,255).

```csharp
public static Color Purple_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Orange4_1**

Gets the color "Orange4_1" (RGB 135,95,0).

```csharp
public static Color Orange4_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightPink4**

Gets the color "LightPink4" (RGB 135,95,95).

```csharp
public static Color LightPink4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Plum4**

Gets the color "Plum4" (RGB 135,95,135).

```csharp
public static Color Plum4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumPurple3**

Gets the color "MediumPurple3" (RGB 135,95,175).

```csharp
public static Color MediumPurple3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumPurple3_1**

Gets the color "MediumPurple3_1" (RGB 135,95,215).

```csharp
public static Color MediumPurple3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SlateBlue1**

Gets the color "SlateBlue1" (RGB 135,95,255).

```csharp
public static Color SlateBlue1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Yellow4**

Gets the color "Yellow4" (RGB 135,135,0).

```csharp
public static Color Yellow4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Wheat4**

Gets the color "Wheat4" (RGB 135,135,95).

```csharp
public static Color Wheat4 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey53**

Gets the color "Grey53" (RGB 135,135,135).

```csharp
public static Color Grey53 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSlateGrey**

Gets the color "LightSlateGrey" (RGB 135,135,175).

```csharp
public static Color LightSlateGrey { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumPurple**

Gets the color "MediumPurple" (RGB 135,135,215).

```csharp
public static Color MediumPurple { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSlateBlue**

Gets the color "LightSlateBlue" (RGB 135,135,255).

```csharp
public static Color LightSlateBlue { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Yellow4_1**

Gets the color "Yellow4_1" (RGB 135,175,0).

```csharp
public static Color Yellow4_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkOliveGreen3**

Gets the color "DarkOliveGreen3" (RGB 135,175,95).

```csharp
public static Color DarkOliveGreen3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSeaGreen**

Gets the color "DarkSeaGreen" (RGB 135,175,135).

```csharp
public static Color DarkSeaGreen { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSkyBlue3**

Gets the color "LightSkyBlue3" (RGB 135,175,175).

```csharp
public static Color LightSkyBlue3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSkyBlue3_1**

Gets the color "LightSkyBlue3_1" (RGB 135,175,215).

```csharp
public static Color LightSkyBlue3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SkyBlue2**

Gets the color "SkyBlue2" (RGB 135,175,255).

```csharp
public static Color SkyBlue2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Chartreuse2_1**

Gets the color "Chartreuse2_1" (RGB 135,215,0).

```csharp
public static Color Chartreuse2_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkOliveGreen3_1**

Gets the color "DarkOliveGreen3_1" (RGB 135,215,95).

```csharp
public static Color DarkOliveGreen3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **PaleGreen3_1**

Gets the color "PaleGreen3_1" (RGB 135,215,135).

```csharp
public static Color PaleGreen3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSeaGreen3**

Gets the color "DarkSeaGreen3" (RGB 135,215,175).

```csharp
public static Color DarkSeaGreen3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSlateGray3**

Gets the color "DarkSlateGray3" (RGB 135,215,215).

```csharp
public static Color DarkSlateGray3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SkyBlue1**

Gets the color "SkyBlue1" (RGB 135,215,255).

```csharp
public static Color SkyBlue1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Chartreuse1**

Gets the color "Chartreuse1" (RGB 135,255,0).

```csharp
public static Color Chartreuse1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightGreen**

Gets the color "LightGreen" (RGB 135,255,95).

```csharp
public static Color LightGreen { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightGreen_1**

Gets the color "LightGreen_1" (RGB 135,255,135).

```csharp
public static Color LightGreen_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **PaleGreen1**

Gets the color "PaleGreen1" (RGB 135,255,175).

```csharp
public static Color PaleGreen1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Aquamarine1_1**

Gets the color "Aquamarine1_1" (RGB 135,255,215).

```csharp
public static Color Aquamarine1_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSlateGray1**

Gets the color "DarkSlateGray1" (RGB 135,255,255).

```csharp
public static Color DarkSlateGray1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Red3**

Gets the color "Red3" (RGB 175,0,0).

```csharp
public static Color Red3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepPink4_2**

Gets the color "DeepPink4_2" (RGB 175,0,95).

```csharp
public static Color DeepPink4_2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumVioletRed**

Gets the color "MediumVioletRed" (RGB 175,0,135).

```csharp
public static Color MediumVioletRed { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Magenta3**

Gets the color "Magenta3" (RGB 175,0,175).

```csharp
public static Color Magenta3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkViolet_1**

Gets the color "DarkViolet_1" (RGB 175,0,215).

```csharp
public static Color DarkViolet_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Purple_2**

Gets the color "Purple_2" (RGB 175,0,255).

```csharp
public static Color Purple_2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkOrange3**

Gets the color "DarkOrange3" (RGB 175,95,0).

```csharp
public static Color DarkOrange3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **IndianRed**

Gets the color "IndianRed" (RGB 175,95,95).

```csharp
public static Color IndianRed { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **HotPink3**

Gets the color "HotPink3" (RGB 175,95,135).

```csharp
public static Color HotPink3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumOrchid3**

Gets the color "MediumOrchid3" (RGB 175,95,175).

```csharp
public static Color MediumOrchid3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumOrchid**

Gets the color "MediumOrchid" (RGB 175,95,215).

```csharp
public static Color MediumOrchid { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumPurple2**

Gets the color "MediumPurple2" (RGB 175,95,255).

```csharp
public static Color MediumPurple2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkGoldenrod**

Gets the color "DarkGoldenrod" (RGB 175,135,0).

```csharp
public static Color DarkGoldenrod { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSalmon3**

Gets the color "LightSalmon3" (RGB 175,135,95).

```csharp
public static Color LightSalmon3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **RosyBrown**

Gets the color "RosyBrown" (RGB 175,135,135).

```csharp
public static Color RosyBrown { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey63**

Gets the color "Grey63" (RGB 175,135,175).

```csharp
public static Color Grey63 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumPurple2_1**

Gets the color "MediumPurple2_1" (RGB 175,135,215).

```csharp
public static Color MediumPurple2_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumPurple1**

Gets the color "MediumPurple1" (RGB 175,135,255).

```csharp
public static Color MediumPurple1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Gold3**

Gets the color "Gold3" (RGB 175,175,0).

```csharp
public static Color Gold3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkKhaki**

Gets the color "DarkKhaki" (RGB 175,175,95).

```csharp
public static Color DarkKhaki { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **NavajoWhite3**

Gets the color "NavajoWhite3" (RGB 175,175,135).

```csharp
public static Color NavajoWhite3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey69**

Gets the color "Grey69" (RGB 175,175,175).

```csharp
public static Color Grey69 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSteelBlue3**

Gets the color "LightSteelBlue3" (RGB 175,175,215).

```csharp
public static Color LightSteelBlue3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSteelBlue**

Gets the color "LightSteelBlue" (RGB 175,175,255).

```csharp
public static Color LightSteelBlue { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Yellow3**

Gets the color "Yellow3" (RGB 175,215,0).

```csharp
public static Color Yellow3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkOliveGreen3_2**

Gets the color "DarkOliveGreen3_2" (RGB 175,215,95).

```csharp
public static Color DarkOliveGreen3_2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSeaGreen3_1**

Gets the color "DarkSeaGreen3_1" (RGB 175,215,135).

```csharp
public static Color DarkSeaGreen3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSeaGreen2**

Gets the color "DarkSeaGreen2" (RGB 175,215,175).

```csharp
public static Color DarkSeaGreen2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightCyan3**

Gets the color "LightCyan3" (RGB 175,215,215).

```csharp
public static Color LightCyan3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSkyBlue1**

Gets the color "LightSkyBlue1" (RGB 175,215,255).

```csharp
public static Color LightSkyBlue1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **GreenYellow**

Gets the color "GreenYellow" (RGB 175,255,0).

```csharp
public static Color GreenYellow { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkOliveGreen2**

Gets the color "DarkOliveGreen2" (RGB 175,255,95).

```csharp
public static Color DarkOliveGreen2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **PaleGreen1_1**

Gets the color "PaleGreen1_1" (RGB 175,255,135).

```csharp
public static Color PaleGreen1_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSeaGreen2_1**

Gets the color "DarkSeaGreen2_1" (RGB 175,255,175).

```csharp
public static Color DarkSeaGreen2_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSeaGreen1**

Gets the color "DarkSeaGreen1" (RGB 175,255,215).

```csharp
public static Color DarkSeaGreen1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **PaleTurquoise1**

Gets the color "PaleTurquoise1" (RGB 175,255,255).

```csharp
public static Color PaleTurquoise1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Red3_1**

Gets the color "Red3_1" (RGB 215,0,0).

```csharp
public static Color Red3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepPink3**

Gets the color "DeepPink3" (RGB 215,0,95).

```csharp
public static Color DeepPink3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepPink3_1**

Gets the color "DeepPink3_1" (RGB 215,0,135).

```csharp
public static Color DeepPink3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Magenta3_1**

Gets the color "Magenta3_1" (RGB 215,0,175).

```csharp
public static Color Magenta3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Magenta3_2**

Gets the color "Magenta3_2" (RGB 215,0,215).

```csharp
public static Color Magenta3_2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Magenta2**

Gets the color "Magenta2" (RGB 215,0,255).

```csharp
public static Color Magenta2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkOrange3_1**

Gets the color "DarkOrange3_1" (RGB 215,95,0).

```csharp
public static Color DarkOrange3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **IndianRed_1**

Gets the color "IndianRed_1" (RGB 215,95,95).

```csharp
public static Color IndianRed_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **HotPink3_1**

Gets the color "HotPink3_1" (RGB 215,95,135).

```csharp
public static Color HotPink3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **HotPink2**

Gets the color "HotPink2" (RGB 215,95,175).

```csharp
public static Color HotPink2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Orchid**

Gets the color "Orchid" (RGB 215,95,215).

```csharp
public static Color Orchid { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumOrchid1**

Gets the color "MediumOrchid1" (RGB 215,95,255).

```csharp
public static Color MediumOrchid1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Orange3**

Gets the color "Orange3" (RGB 215,135,0).

```csharp
public static Color Orange3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSalmon3_1**

Gets the color "LightSalmon3_1" (RGB 215,135,95).

```csharp
public static Color LightSalmon3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightPink3**

Gets the color "LightPink3" (RGB 215,135,135).

```csharp
public static Color LightPink3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Pink3**

Gets the color "Pink3" (RGB 215,135,175).

```csharp
public static Color Pink3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Plum3**

Gets the color "Plum3" (RGB 215,135,215).

```csharp
public static Color Plum3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Violet**

Gets the color "Violet" (RGB 215,135,255).

```csharp
public static Color Violet { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Gold3_1**

Gets the color "Gold3_1" (RGB 215,175,0).

```csharp
public static Color Gold3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightGoldenrod3**

Gets the color "LightGoldenrod3" (RGB 215,175,95).

```csharp
public static Color LightGoldenrod3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Tan**

Gets the color "Tan" (RGB 215,175,135).

```csharp
public static Color Tan { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MistyRose3**

Gets the color "MistyRose3" (RGB 215,175,175).

```csharp
public static Color MistyRose3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Thistle3**

Gets the color "Thistle3" (RGB 215,175,215).

```csharp
public static Color Thistle3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Plum2**

Gets the color "Plum2" (RGB 215,175,255).

```csharp
public static Color Plum2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Yellow3_1**

Gets the color "Yellow3_1" (RGB 215,215,0).

```csharp
public static Color Yellow3_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Khaki3**

Gets the color "Khaki3" (RGB 215,215,95).

```csharp
public static Color Khaki3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightGoldenrod2**

Gets the color "LightGoldenrod2" (RGB 215,215,135).

```csharp
public static Color LightGoldenrod2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightYellow3**

Gets the color "LightYellow3" (RGB 215,215,175).

```csharp
public static Color LightYellow3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey84**

Gets the color "Grey84" (RGB 215,215,215).

```csharp
public static Color Grey84 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSteelBlue1**

Gets the color "LightSteelBlue1" (RGB 215,215,255).

```csharp
public static Color LightSteelBlue1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Yellow2**

Gets the color "Yellow2" (RGB 215,255,0).

```csharp
public static Color Yellow2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkOliveGreen1**

Gets the color "DarkOliveGreen1" (RGB 215,255,95).

```csharp
public static Color DarkOliveGreen1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkOliveGreen1_1**

Gets the color "DarkOliveGreen1_1" (RGB 215,255,135).

```csharp
public static Color DarkOliveGreen1_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkSeaGreen1_1**

Gets the color "DarkSeaGreen1_1" (RGB 215,255,175).

```csharp
public static Color DarkSeaGreen1_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Honeydew2**

Gets the color "Honeydew2" (RGB 215,255,215).

```csharp
public static Color Honeydew2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightCyan1**

Gets the color "LightCyan1" (RGB 215,255,255).

```csharp
public static Color LightCyan1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Red1**

Gets the color "Red1" (RGB 255,0,0).

```csharp
public static Color Red1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepPink2**

Gets the color "DeepPink2" (RGB 255,0,95).

```csharp
public static Color DeepPink2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepPink1**

Gets the color "DeepPink1" (RGB 255,0,135).

```csharp
public static Color DeepPink1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DeepPink1_1**

Gets the color "DeepPink1_1" (RGB 255,0,175).

```csharp
public static Color DeepPink1_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Magenta2_1**

Gets the color "Magenta2_1" (RGB 255,0,215).

```csharp
public static Color Magenta2_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Magenta1**

Gets the color "Magenta1" (RGB 255,0,255).

```csharp
public static Color Magenta1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **OrangeRed1**

Gets the color "OrangeRed1" (RGB 255,95,0).

```csharp
public static Color OrangeRed1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **IndianRed1**

Gets the color "IndianRed1" (RGB 255,95,95).

```csharp
public static Color IndianRed1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **IndianRed1_1**

Gets the color "IndianRed1_1" (RGB 255,95,135).

```csharp
public static Color IndianRed1_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **HotPink**

Gets the color "HotPink" (RGB 255,95,175).

```csharp
public static Color HotPink { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **HotPink_1**

Gets the color "HotPink_1" (RGB 255,95,215).

```csharp
public static Color HotPink_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MediumOrchid1_1**

Gets the color "MediumOrchid1_1" (RGB 255,95,255).

```csharp
public static Color MediumOrchid1_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **DarkOrange**

Gets the color "DarkOrange" (RGB 255,135,0).

```csharp
public static Color DarkOrange { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Salmon1**

Gets the color "Salmon1" (RGB 255,135,95).

```csharp
public static Color Salmon1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightCoral**

Gets the color "LightCoral" (RGB 255,135,135).

```csharp
public static Color LightCoral { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **PaleVioletRed1**

Gets the color "PaleVioletRed1" (RGB 255,135,175).

```csharp
public static Color PaleVioletRed1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Orchid2**

Gets the color "Orchid2" (RGB 255,135,215).

```csharp
public static Color Orchid2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Orchid1**

Gets the color "Orchid1" (RGB 255,135,255).

```csharp
public static Color Orchid1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Orange1**

Gets the color "Orange1" (RGB 255,175,0).

```csharp
public static Color Orange1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **SandyBrown**

Gets the color "SandyBrown" (RGB 255,175,95).

```csharp
public static Color SandyBrown { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightSalmon1**

Gets the color "LightSalmon1" (RGB 255,175,135).

```csharp
public static Color LightSalmon1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightPink1**

Gets the color "LightPink1" (RGB 255,175,175).

```csharp
public static Color LightPink1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Pink1**

Gets the color "Pink1" (RGB 255,175,215).

```csharp
public static Color Pink1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Plum1**

Gets the color "Plum1" (RGB 255,175,255).

```csharp
public static Color Plum1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Gold1**

Gets the color "Gold1" (RGB 255,215,0).

```csharp
public static Color Gold1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightGoldenrod2_1**

Gets the color "LightGoldenrod2_1" (RGB 255,215,95).

```csharp
public static Color LightGoldenrod2_1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightGoldenrod2_2**

Gets the color "LightGoldenrod2_2" (RGB 255,215,135).

```csharp
public static Color LightGoldenrod2_2 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **NavajoWhite1**

Gets the color "NavajoWhite1" (RGB 255,215,175).

```csharp
public static Color NavajoWhite1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **MistyRose1**

Gets the color "MistyRose1" (RGB 255,215,215).

```csharp
public static Color MistyRose1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Thistle1**

Gets the color "Thistle1" (RGB 255,215,255).

```csharp
public static Color Thistle1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Yellow1**

Gets the color "Yellow1" (RGB 255,255,0).

```csharp
public static Color Yellow1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **LightGoldenrod1**

Gets the color "LightGoldenrod1" (RGB 255,255,95).

```csharp
public static Color LightGoldenrod1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Khaki1**

Gets the color "Khaki1" (RGB 255,255,135).

```csharp
public static Color Khaki1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Wheat1**

Gets the color "Wheat1" (RGB 255,255,175).

```csharp
public static Color Wheat1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Cornsilk1**

Gets the color "Cornsilk1" (RGB 255,255,215).

```csharp
public static Color Cornsilk1 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey100**

Gets the color "Grey100" (RGB 255,255,255).

```csharp
public static Color Grey100 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey3**

Gets the color "Grey3" (RGB 8,8,8).

```csharp
public static Color Grey3 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey7**

Gets the color "Grey7" (RGB 18,18,18).

```csharp
public static Color Grey7 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey11**

Gets the color "Grey11" (RGB 28,28,28).

```csharp
public static Color Grey11 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey15**

Gets the color "Grey15" (RGB 38,38,38).

```csharp
public static Color Grey15 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey19**

Gets the color "Grey19" (RGB 48,48,48).

```csharp
public static Color Grey19 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey23**

Gets the color "Grey23" (RGB 58,58,58).

```csharp
public static Color Grey23 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey27**

Gets the color "Grey27" (RGB 68,68,68).

```csharp
public static Color Grey27 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey30**

Gets the color "Grey30" (RGB 78,78,78).

```csharp
public static Color Grey30 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey35**

Gets the color "Grey35" (RGB 88,88,88).

```csharp
public static Color Grey35 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey39**

Gets the color "Grey39" (RGB 98,98,98).

```csharp
public static Color Grey39 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey42**

Gets the color "Grey42" (RGB 108,108,108).

```csharp
public static Color Grey42 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey46**

Gets the color "Grey46" (RGB 118,118,118).

```csharp
public static Color Grey46 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey50**

Gets the color "Grey50" (RGB 128,128,128).

```csharp
public static Color Grey50 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey54**

Gets the color "Grey54" (RGB 138,138,138).

```csharp
public static Color Grey54 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey58**

Gets the color "Grey58" (RGB 148,148,148).

```csharp
public static Color Grey58 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey62**

Gets the color "Grey62" (RGB 158,158,158).

```csharp
public static Color Grey62 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey66**

Gets the color "Grey66" (RGB 168,168,168).

```csharp
public static Color Grey66 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey70**

Gets the color "Grey70" (RGB 178,178,178).

```csharp
public static Color Grey70 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey74**

Gets the color "Grey74" (RGB 188,188,188).

```csharp
public static Color Grey74 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey78**

Gets the color "Grey78" (RGB 198,198,198).

```csharp
public static Color Grey78 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey82**

Gets the color "Grey82" (RGB 208,208,208).

```csharp
public static Color Grey82 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey85**

Gets the color "Grey85" (RGB 218,218,218).

```csharp
public static Color Grey85 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey89**

Gets the color "Grey89" (RGB 228,228,228).

```csharp
public static Color Grey89 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Grey93**

Gets the color "Grey93" (RGB 238,238,238).

```csharp
public static Color Grey93 { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

## Constructors

### **Color(Byte, Byte, Byte)**

Initializes a new instance of the [Color](./pplus.color.md) struct.

```csharp
Color(byte red, byte green, byte blue)
```

#### Parameters

`red` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
The red component.

`green` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
The green component.

`blue` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
The blue component.

## Methods

### **Blend(Color, Single)**

Blends two colors.

```csharp
Color Blend(Color other, float factor)
```

#### Parameters

`other` [Color](./pplus.color.md)<br>
The other color.

`factor` [Single](https://docs.microsoft.com/en-us/dotnet/api/system.single)<br>
The blend factor.

#### Returns

The resulting color.

### **FromHex(Color)**

Gets the hexadecimal representation of the color.

```csharp
string FromHex(Color value)
```

#### Parameters

`value` [Color](./pplus.color.md)<br>
The [Color](./pplus.color.md)

#### Returns

The hexadecimal representation of the color.

### **GetHashCode()**

```csharp
int GetHashCode()
```

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)

### **Equals(Object)**

Checks if [Color](./pplus.color.md) are equal the instance.

```csharp
bool Equals(object obj)
```

#### Parameters

`obj` [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>
The object to compare

#### Returns

`true` if the two colors are equal, otherwise `false`.

### **Equals(Color)**

Checks if [Color](./pplus.color.md) are equal the instance.

```csharp
bool Equals(Color other)
```

#### Parameters

`other` [Color](./pplus.color.md)<br>
The [Color](./pplus.color.md)

#### Returns

`true` if the two colors are equal, otherwise `false`.

### **FromHtml(String)**

Converts string color Html format (#RRGGBB) into [Color](./pplus.color.md).

```csharp
Color FromHtml(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The html color to convert.

#### Returns

A [Color](./pplus.color.md).

### **ToConsoleColor(Color)**

Converts a [Color](./pplus.color.md) to a .

```csharp
ConsoleColor ToConsoleColor(Color color)
```

#### Parameters

`color` [Color](./pplus.color.md)<br>
The color to convert.

#### Returns

A  representing the [Color](./pplus.color.md).

### **FromInt32(Int32)**

Converts a color number into a [Color](./pplus.color.md).

```csharp
Color FromInt32(int number)
```

#### Parameters

`number` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The color number.

#### Returns

The color representing the specified color number.

### **FromConsoleColor(ConsoleColor)**

Converts a  to a [Color](./pplus.color.md).

```csharp
Color FromConsoleColor(ConsoleColor color)
```

#### Parameters

`color` ConsoleColor<br>
The color to convert.

#### Returns

A [Color](./pplus.color.md) representing the .

### **ToString()**

Convert to string

```csharp
string ToString()
```

#### Returns

The [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)


- - -
[**Back to List Api**](./apis.md)
