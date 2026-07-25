<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## HideTable Enum

Defines which table border elements are hidden when rendering\.
[None](HideTable.md#PromptPlusLibrary.HideTable.None 'PromptPlusLibrary\.HideTable\.None') \(default\) renders all elements\.
Combine flags to hide multiple elements at once\.

```csharp
public enum HideTable
```
### Fields

<a name='PromptPlusLibrary.HideTable.None'></a>

`None` 0

Show all border elements \(default\)\.

<a name='PromptPlusLibrary.HideTable.RowSeparator'></a>

`RowSeparator` 1

Hide horizontal separators between data rows\.

<a name='PromptPlusLibrary.HideTable.Header'></a>

`Header` 2

Hide the entire header row \(column titles\) and the separator line between header and data\.
When set, no header content is rendered; the top border connects directly to the data rows\.

<a name='PromptPlusLibrary.HideTable.ColumnSeparator'></a>

`ColumnSeparator` 4

Hide vertical separators between columns\.

<a name='PromptPlusLibrary.HideTable.OuterBorder'></a>

`OuterBorder` 8

Hide the outer frame border \(top, bottom, left and right edges\)\.