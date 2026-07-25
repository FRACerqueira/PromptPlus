// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Resources;
using System.Runtime.CompilerServices;

// Specifies the neutral culture for the assembly's resources
[assembly: NeutralResourcesLanguage("en-US")]

// Grants the headless VirtualTerminal test driver access to internal types (PromptPlusControls, PromptConfig, BaseControlPrompt<T>)
[assembly: InternalsVisibleTo("PromptPlus.Tests")]
