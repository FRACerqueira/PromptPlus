using System.ComponentModel.DataAnnotations;

namespace PromptPlusExample
{
    public enum ExampleType
    {
        [Display(Name = "Utility: Color text")]
        ColorText,
        [Display(Name = "Utility: Banner ASCII text")]
        Banner,
        [Display(Name = "Control: Any key")]
        AnyKey,
        [Display(Name = "Control: Key Press")]
        KeyPress,
        [Display(Name = "Control: Confirm")]
        Confirm,
        [Display(Name = "Control: MaskEdit Generic")]
        MaskEditGeneric,
        [Display(Name = "Control: MaskEdit Date")]
        MaskEditDate,
        [Display(Name = "Control: MaskEdit Time")]
        MaskEditTime,
        [Display(Name = "Control: MaskEdit Date and Time")]
        MaskEditDateTime,
        [Display(Name = "Control: MaskEdit Number")]
        MaskEditNumber,
        [Display(Name = "Control: MaskEdit Currency")]
        MaskEditCurrrency,
        [Display(Name = "Control: Input - Text")]
        Input,
        [Display(Name = "Control: Input - Password")]
        Password,
        [Display(Name = "Control: Select - IEnumerable")]
        Select,
        [Display(Name = "Control: Select - Enum")]
        SelectWithEnum,
        [Display(Name = "Control: MultiSelect with group")]
        MultiSelectGroup,
        [Display(Name = "Control: MultiSelect - IEnumerable")]
        MultiSelect,
        [Display(Name = "Control: MultiSelect - Enum")]
        MultiSelectWithEnum,
        [Display(Name = "Control: List - Create IEnumerable with Free Input")]
        List,
        [Display(Name = "Control: ListMasked - Create IEnumerable with MaskEdit")]
        ListMasked,
        [Display(Name = "Control: Browser - Filter Only Folder")]
        FolderBrowser,
        [Display(Name = "Control: Browser - Filter None (selected only File)")]
        FileBrowser,
        [Display(Name = "Control: Slider Number")]
        SliderNumber,
        [Display(Name = "Control: Number Up/Down")]
        NumberUpDown,
        [Display(Name = "Control: Slider Switche")]
        SliderSwitche,
        [Display(Name = "Control: Progress Bar")]
        ProgressbarAsync,
        [Display(Name = "Control: WaitProcess - Sinlge Tasks")]
        WaitSingleProcess,
        [Display(Name = "Control: WaitProcess - IEnumerable Tasks parallel")]
        WaitManyProcess,
        [Display(Name = "Control: PipeLine - IEnumerable Prompt sequential with conditions")]
        PipeLine,
        [Display(Name = "Control: StatusBar (frezeen bottom line(s) with template/columns)")]
        StatusBar,
        [Display(Name = "Extension: Import Validators")]
        ImportValidators,
        [Display(Name = "Utility: Save and Load Config")]
        SaveLoadConfig,
        [Display(Name = "Utility: Change Language")]
        ChooseLanguage,
        Quit
    }
}
