using System.ComponentModel.DataAnnotations;

namespace PromptPlusExample
{
    public enum ExampleType
    {
        [Display(Name = "Prompt: Any key")]
        AnyKey,
        [Display(Name = "Prompt: Key Press")]
        KeyPress,
        [Display(Name = "Prompt: Confirm")]
        Confirm,
        [Display(Name = "Prompt: MaskEdit Generic")]
        MaskEditGeneric,
        [Display(Name = "Prompt: MaskEdit Date")]
        MaskEditDate,
        [Display(Name = "Prompt: MaskEdit Time")]
        MaskEditTime,
        [Display(Name = "Prompt: MaskEdit Date and Time")]
        MaskEditDateTime,
        [Display(Name = "Prompt: MaskEdit Number")]
        MaskEditNumber,
        [Display(Name = "Prompt: MaskEdit Currency")]
        MaskEditCurrrency,
        [Display(Name = "Prompt: Input - Generic")]
        Input,
        [Display(Name = "Prompt: Input - Password")]
        Password,
        [Display(Name = "Prompt: Select - IEnumerable")]
        Select,
        [Display(Name = "Prompt: Select - Enum")]
        SelectWithEnum,
        [Display(Name = "Prompt: MultiSelect - IEnumerable")]
        MultiSelect,
        [Display(Name = "Prompt: MultiSelect - Enum")]
        MultiSelectWithEnum,
        [Display(Name = "Prompt: List - Create IEnumerable with Free Input")]
        List,
        [Display(Name = "Prompt: ListMasked - Create IEnumerable with MaskEdit Generic")]
        ListMasked,
        [Display(Name = "Prompt: Browser - Filter Only Folder")]
        FolderBrowser,
        [Display(Name = "Prompt: Browser - Filter None (selected only File)")]
        FileBrowser,
        [Display(Name = "Prompt: Slider Number")]
        SliderNumber,
        [Display(Name = "Prompt: Number Up/Down")]
        NumberUpDown,
        [Display(Name = "Prompt: Slider Switche")]
        SliderSwitche,
        [Display(Name = "Control: Progress Bar")]
        ProgressbarAsync,
        [Display(Name = "Control: WaitProcess - Sinlge Tasks")]
        WaitSingleProcess,
        [Display(Name = "Control: WaitProcess - IEnumerable Tasks parallel")]
        WaitManyProcess,
        [Display(Name = "Control: PipeLine - IEnumerable Prompt sequential with conditions")]
        PipeLine,
        [Display(Name = "Utility: Save and Load Config")]
        SaveLoadConfig,
        [Display(Name = "Utility: Change Language")]
        ChooseLanguage,
        Quit
    }
}
