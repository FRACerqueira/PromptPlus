# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ItemBrowser 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ItemBrowser

Namespace: PPlus.Controls

Represents a file or folder item

```csharp
public class ItemBrowser
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ItemBrowser](./pplus.controls.itembrowser.md)

## Properties

### <a id="properties-name"/>**Name**

Get Name

```csharp
public string Name { get; internal set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-isfolder"/>**IsFolder**

Get if item is folder

```csharp
public bool IsFolder { get; internal set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-currentfolder"/>**CurrentFolder**

Get name of parent folder

```csharp
public string CurrentFolder { get; internal set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-fullpath"/>**FullPath**

Get fullpath of item

```csharp
public string FullPath { get; internal set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-length"/>**Length**

Get Length of item. If a folder lenght represents number of item. If file lenght represents the size in bytes

```csharp
public long Length { get; internal set; }
```

#### Property Value

[Int64](https://docs.microsoft.com/en-us/dotnet/api/system.int64)<br>

## Constructors

### <a id="constructors-.ctor"/>**ItemBrowser()**

```csharp
public ItemBrowser()
```


- - -
[**Back to List Api**](./apis.md)
