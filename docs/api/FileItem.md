<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## FileItem Class

Represents a file system entry \(file or directory\) selected by the File control\.

```csharp
public sealed class FileItem
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → FileItem
### Properties

<a name='PromptPlusLibrary.FileItem.FullPath'></a>

## FileItem\.FullPath Property

Gets the full path of the entry\.

```csharp
public string FullPath { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='PromptPlusLibrary.FileItem.IsDirectory'></a>

## FileItem\.IsDirectory Property

Gets whether the entry is a directory\.

```csharp
public bool IsDirectory { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.FileItem.LastWriteTime'></a>

## FileItem\.LastWriteTime Property

Gets the last write time of the entry\.

```csharp
public System.DateTime LastWriteTime { get; }
```

#### Property Value
[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='PromptPlusLibrary.FileItem.Length'></a>

## FileItem\.Length Property

Gets the file length in bytes\. Zero for directories\.

```csharp
public long Length { get; }
```

#### Property Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='PromptPlusLibrary.FileItem.Name'></a>

## FileItem\.Name Property

Gets the display name of the entry\.

```csharp
public string Name { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Methods

<a name='PromptPlusLibrary.FileItem.ToString()'></a>

## FileItem\.ToString\(\) Method

Returns a string that represents the current object\.

```csharp
public override string ToString();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string that represents the current object\.