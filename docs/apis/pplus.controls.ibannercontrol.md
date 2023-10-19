# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IBannerControl 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IBannerControl

Namespace: PPlus.Controls

Represents the interface with all Methods of the Banner control

```csharp
public interface IBannerControl
```

## Methods

### <a id="methods-loadfont"/>**LoadFont(String)**

Load external font from file

```csharp
IBannerControl LoadFont(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
fullpath of file

#### Returns

[IBannerControl](./pplus.controls.ibannercontrol.md)

### <a id="methods-loadfont"/>**LoadFont(Stream)**

Load external font from [Stream](https://docs.microsoft.com/en-us/dotnet/api/system.io.stream)

```csharp
IBannerControl LoadFont(Stream value)
```

#### Parameters

`value` [Stream](https://docs.microsoft.com/en-us/dotnet/api/system.io.stream)<br>
stream instance

#### Returns

[IBannerControl](./pplus.controls.ibannercontrol.md)

### <a id="methods-run"/>**Run(Nullable&lt;Color&gt;, BannerDashOptions)**

Execute this control and show banner.

```csharp
void Run(Nullable<Color> color, BannerDashOptions bannerDash)
```

#### Parameters

`color` [Nullable&lt;Color&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The foregound [Color](./pplus.color.md) text

`bannerDash` [BannerDashOptions](./pplus.controls.bannerdashoptions.md)<br>
The type of [BannerDashOptions](./pplus.controls.bannerdashoptions.md)


- - -
[**Back to List Api**](./apis.md)
