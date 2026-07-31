<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## FilterTableMode Enum

Filter strategy for filter items in table\.

```csharp
public enum FilterTableMode
```
### Fields

<a name='PromptPlusLibrary.FilterTableMode.Answer'></a>

`Answer` 0

Filter by the answer text \(result of `TextSelector`\)\.

<a name='PromptPlusLibrary.FilterTableMode.ColumnFilters'></a>

`ColumnFilters` 1

Filter by the concatenated text of all filterable columns
\(columns declared with `isFilterable: true`\)\.