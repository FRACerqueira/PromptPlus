# PromptPlus # BrowserOptions
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Browser Control**](browser) |
[**BaseOptions**](baseoptions) 

## Documentation
Control Options Class for [**Browser**](browser). inherits from [**BaseOptions**](baseoptions)

### Properties
[**Top**](#promptplus--browseroptions)

- Filter
  - Type : BrowserFilter
  - Filer discovery. None = Files, OnlyFolder = Folder
  - Default Value = BrowserFilter.None

- PrefixExtension
  - Type : string
  - prefix to be added to the end of the selected item
  - Default Value = null

- AllowNotSelected
  - Type : bool
  - Accept not seleted item
  - Default Value = false

- DefaultValue
  - Type : string
  - Full path of file/folder initial selected
  - Default Value = null

- RootFolder
  - Type : string
  - root for discovery
  - Default Value = null

- SearchPattern
  - Type : string
  - Specifies what to search for by the browser (eg.: *.cs)
  - Default Value = null

- PageSize
  - Type : int
  - maximum items per page. Tf the value is null, the value will be calculated according to the screen size  
  - Default Value = null

- SupressHidden
  - Type : bool
  - Supress file/folder with attribute hidden/system 
  - Default Value = true

- ShowNavigationCurrentPath
  - Type : bool
  - Split fullpath of seleted item and show then
  - Default Value = true

- ShowNavigationCurrentPath
  - Type : bool
  - Split fullpath of seleted item and show then
  - Default Value = true

- ShowSearchPattern
  - Type : bool
  - Show/hide searchPattern in prompt message
  - Default Value = true
  
### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**Browser Control**](browser) |
[**BaseOptions**](baseoptions) 
