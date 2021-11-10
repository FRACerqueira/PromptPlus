// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Threading;

using PromptPlusControls.Resources;

namespace PromptPlusControls.Internal
{
    internal static class Messages
    {
        public static void UpdateCulture()
        {
            PasswordStandardHotkeys = PromptPlusResources.PasswordStandardHotkeys;
            AnyKey = PromptPlusResources.AnyKey;
            CanceledText = PromptPlusResources.CanceledText;
            EmptyTitle = PromptPlusResources.EmptyTitle;
            EnterFininsh = PromptPlusResources.EnterFininsh;
            EscCancel = PromptPlusResources.EscCancel;
            EscCancelWithPipeline = PromptPlusResources.EscCancelWithPipeline;
            EscCancelWithPipeNotAll = PromptPlusResources.EscCancelWithPipeNotAll;
            FileNotSelected = PromptPlusResources.FileNotSelected;
            FolderCurrentPath = PromptPlusResources.FolderCurrentPath;
            FolderKeyNavigation = string.Format(PromptPlusResources.FolderKeyNavigation, PromptPlus.UnSelectFilter);
            Invalid = PromptPlusResources.Invalid;
            ItemsFiltered = PromptPlusResources.ItemsFiltered;
            KeyNavPaging = PromptPlusResources.KeyNavPaging;
            ListItemAlreadyexists = PromptPlusResources.ListItemAlreadyexists;
            ListKeyNavigation = string.Format(PromptPlusResources.ListKeyNavigation, PromptPlus.RemoveAll);
            ListMaxSelection = PromptPlusResources.ListMaxSelection;
            ListMinSelection = PromptPlusResources.ListMinSelection;
            LongNoKey = PromptPlusResources.LongNoKey;
            LongYesKey = PromptPlusResources.LongYesKey;
            MaxLength = PromptPlusResources.MaxLength;
            MinLength = PromptPlusResources.MinLength;
            MultiSelectMinSelection = PromptPlusResources.MultiSelectMinSelection;
            MultiSelectMaxSelection = PromptPlusResources.MultiSelectMaxSelection;
            MultiSelectKeyNavigation = string.Format(PromptPlusResources.MultiSelectKeyNavigation, PromptPlus.UnSelectFilter, PromptPlus.SelectAll, PromptPlus.InvertSelect);
            NoKey = PromptPlusResources.NoKey.ToCharArray()[0];
            NoMatchRegex = PromptPlusResources.NoMatchRegex;
            OffValue = PromptPlusResources.OffValue;
            OnValue = PromptPlusResources.OnValue;
            PaginationTemplate = PromptPlusResources.PaginationTemplate;
            PipelineText = PromptPlusResources.PipeLineText;
            Pressedkey = PromptPlusResources.PressedKey;
            Required = PromptPlusResources.Required;
            ResizedTerminal = PromptPlusResources.ResizedTerminal;
            ResizeTerminal = PromptPlusResources.ResizeTerminal;
            RunningText = PromptPlusResources.RunningText;
            SelectKeyNavigation = PromptPlusResources.SelectKeyNavigation;
            AutoCompleteKeyNavigation = string.Format(PromptPlusResources.AutoCompleteKeyNavigation, PromptPlus.UnSelectFilter);
            AutoCompleteKeyNotfound = PromptPlusResources.AutoCompleteKeyNotfound;
            ShowKeyPressStandardHotKeys = PromptPlusResources.ShowKeyPressStandardHotKeys;
            ShowKeyPressStandardHotKeysWithPipeline = PromptPlusResources.ShowKeyPressStandardHotKeysWithPipeline;
            ShowProcessStandardHotKeysWithPipeline = PromptPlusResources.ShowProcessStandardHotKeysWithPipeline;
            ShowStandardHotKeys = PromptPlusResources.ShowStandardHotKeys;
            ShowStandardHotKeysWithPipeline = PromptPlusResources.ShowStandardHotKeysWithPipeline;
            SkipedText = PromptPlusResources.SkipedText;
            NumberUpDownKeyNavigator = PromptPlusResources.NumberUpDownKeyNavigator;
            SliderNumberKeyNavigator = PromptPlusResources.SliderNumberKeyNavigator;
            SliderSwitcheKeyNavigator = PromptPlusResources.SliderSwitcheKeyNavigator;
            SummaryPipelineReturnText = PromptPlusResources.SummaryPipelineReturnto;
            WaittingProcess = PromptPlusResources.WaittingProcess;
            WaittingText = PromptPlusResources.WaittingText;
            YesKey = PromptPlusResources.YesKey.ToCharArray()[0];
            MaskEditPosAnyChar = PromptPlusResources.MaskEditPosAnyChar;
            MaskEditPosCustom = PromptPlusResources.MaskEditPosCustom;
            MaskEditPosDay = PromptPlusResources.MaskEditPosDay;
            MaskEditPosHour = PromptPlusResources.MaskEditPosHour;
            MaskEditPosLetter = PromptPlusResources.MaskEditPosLetter;
            MaskEditPosMonth = PromptPlusResources.MaskEditPosMonth;
            MaskEditPosMinute = PromptPlusResources.MaskEditPosMinute;
            MaskEditPosNumeric = PromptPlusResources.MaskEditPosNumeric;
            MaskEditPosSecond = PromptPlusResources.MaskEditPosSecond;
            MaskEditPosYear = PromptPlusResources.MaskEditPosYear;
            MaskEditInputType = PromptPlusResources.MaskEditInputType;
            MaskEditErase = PromptPlusResources.MaskEditErase;
            InvalidTypeBoolean = PromptPlusResources.InvalidTypeBoolean;
            InvalidTypeByte = PromptPlusResources.InvalidTypeByte;
            InvalidTypeChar = PromptPlusResources.InvalidTypeChar;
            InvalidTypeNumber = PromptPlusResources.InvalidTypeNumber;
            InvalidTypeDateTime = string.Format(PromptPlusResources.InvalidTypeDateTime, FormatDate(), FormatTime());
            SelectKeyNavigation = string.Format(PromptPlusResources.SelectKeyNavigation, PromptPlus.UnSelectFilter);
            FinishResultList = PromptPlusResources.FinishResultList;
            FinishResultTasks = PromptPlusResources.FinishResultTasks;
        }

        public static string FinishResultTasks { get; private set; } = PromptPlusResources.FinishResultTasks;

        public static string Pressedkey { get; private set; } = PromptPlusResources.PressedKey;

        public static string ResizedTerminal { get; private set; } = PromptPlusResources.ResizedTerminal;

        public static string ResizeTerminal { get; private set; } = PromptPlusResources.ResizeTerminal;

        public static string EmptyTitle { get; private set; } = PromptPlusResources.EmptyTitle;

        private static string s_escCancel;
        public static string EscCancel
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(s_escCancel))
                    {
                        return string.Format(PromptPlusResources.EscCancel, PromptPlus.AbortKeyPress);
                    }
                    else
                    {
                        return string.Format(s_escCancel, PromptPlus.AbortKeyPress);
                    }
                }
                catch (System.Exception)
                {
                    return string.Format(PromptPlusResources.EscCancel, PromptPlus.AbortKeyPress);
                }
            }
            private set
            {
                s_escCancel = value;
            }
        }

        private static string s_escCancelWithPipeline;
        public static string EscCancelWithPipeline
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(s_escCancelWithPipeline))
                    {
                        return string.Format(PromptPlusResources.EscCancelWithPipeline, PromptPlus.AbortKeyPress, PromptPlus.AbortAllPipesKeyPress);
                    }
                    else
                    {
                        return string.Format(s_escCancelWithPipeline, PromptPlus.AbortKeyPress, PromptPlus.AbortAllPipesKeyPress);
                    }
                }
                catch (System.Exception)
                {
                    return string.Format(PromptPlusResources.EscCancelWithPipeline, PromptPlus.AbortKeyPress, PromptPlus.AbortAllPipesKeyPress);
                }

            }
            private set
            {
                s_escCancelWithPipeline = value;
            }
        }

        private static string s_escCancelWithPipeNotAll;
        public static string EscCancelWithPipeNotAll
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(s_escCancelWithPipeNotAll))
                    {
                        return string.Format(PromptPlusResources.EscCancelWithPipeNotAll, PromptPlus.AbortKeyPress);
                    }
                    else
                    {
                        return string.Format(s_escCancelWithPipeNotAll, PromptPlus.AbortKeyPress);
                    }
                }
                catch (System.Exception)
                {
                    return string.Format(PromptPlusResources.EscCancelWithPipeNotAll, PromptPlus.AbortKeyPress);
                }
            }
            private set
            {
                s_escCancelWithPipeNotAll = value;
            }
        }
        public static string FinishResultList { get; private set; } = PromptPlusResources.FinishResultList;

        public static string PasswordStandardHotkeys { get; private set; } = PromptPlusResources.PasswordStandardHotkeys;

        public static char YesKey { get; private set; } = PromptPlusResources.YesKey.ToCharArray()[0];

        public static char NoKey { get; private set; } = PromptPlusResources.NoKey.ToCharArray()[0];

        public static string LongYesKey { get; private set; } = PromptPlusResources.LongYesKey;

        public static string LongNoKey { get; private set; } = PromptPlusResources.LongNoKey;

        public static string AnyKey { get; private set; } = PromptPlusResources.AnyKey;

        public static string Invalid { get; private set; } = PromptPlusResources.Invalid;

        public static string Required { get; private set; } = PromptPlusResources.Required;

        public static string MinLength { get; private set; } = PromptPlusResources.MinLength;

        public static string MaxLength { get; private set; } = PromptPlusResources.MaxLength;

        public static string NoMatchRegex { get; private set; } = PromptPlusResources.NoMatchRegex;

        public static string EnterFininsh { get; private set; } = PromptPlusResources.EnterFininsh;

        public static string MultiSelectMinSelection { get; private set; } = PromptPlusResources.MultiSelectMinSelection;

        public static string MultiSelectMaxSelection { get; private set; } = PromptPlusResources.MultiSelectMaxSelection;

        public static string MultiSelectKeyNavigation { get; private set; } = string.Format(PromptPlusResources.MultiSelectKeyNavigation, PromptPlus.UnSelectFilter, PromptPlus.SelectAll, PromptPlus.InvertSelect);

        public static string ListMinSelection { get; private set; } = PromptPlusResources.ListMinSelection;

        public static string ListMaxSelection { get; private set; } = PromptPlusResources.ListMaxSelection;

        public static string ListKeyNavigation { get; private set; } = string.Format(PromptPlusResources.ListKeyNavigation, PromptPlus.RemoveAll);

        public static string ListItemAlreadyexists { get; private set; } = PromptPlusResources.ListItemAlreadyexists;

        public static string SelectKeyNavigation { get; private set; } = string.Format(PromptPlusResources.SelectKeyNavigation, PromptPlus.UnSelectFilter);

        public static string AutoCompleteKeyNotfound { get; private set; } = PromptPlusResources.AutoCompleteKeyNavigation;

        public static string AutoCompleteKeyNavigation { get; private set; } = string.Format(PromptPlusResources.AutoCompleteKeyNavigation, PromptPlus.UnSelectFilter);

        public static string KeyNavPaging { get; private set; } = PromptPlusResources.KeyNavPaging;

        public static string ItemsFiltered { get; private set; } = PromptPlusResources.ItemsFiltered;

        public static string PaginationTemplate { get; private set; } = PromptPlusResources.PaginationTemplate;

        public static string FolderKeyNavigation { get; private set; } = string.Format(PromptPlusResources.FolderKeyNavigation, PromptPlus.UnSelectFilter);

        public static string FolderCurrentPath { get; private set; } = PromptPlusResources.FolderCurrentPath;

        public static string FileNotSelected { get; private set; } = PromptPlusResources.FileNotSelected;

        public static string NumberUpDownKeyNavigator { get; private set; } = PromptPlusResources.NumberUpDownKeyNavigator;

        public static string SliderNumberKeyNavigator { get; private set; } = PromptPlusResources.SliderNumberKeyNavigator;

        public static string SliderSwitcheKeyNavigator { get; private set; } = PromptPlusResources.SliderSwitcheKeyNavigator;

        public static string OnValue { get; private set; } = PromptPlusResources.OnValue;

        public static string OffValue { get; private set; } = PromptPlusResources.OffValue;

        public static string PipelineText { get; private set; } = PromptPlusResources.PipeLineText;

        public static string RunningText { get; private set; } = PromptPlusResources.RunningText;

        public static string WaittingText { get; private set; } = PromptPlusResources.WaittingText;

        public static string WaittingProcess { get; private set; } = PromptPlusResources.WaittingProcess;

        public static string SkipedText { get; private set; } = PromptPlusResources.SkipedText;

        public static string CanceledText { get; private set; } = PromptPlusResources.CanceledText;

        public static string SummaryPipelineReturnText { get; private set; } = PromptPlusResources.SummaryPipelineReturnto;

        public static string ShowStandardHotKeys { get; private set; } = PromptPlusResources.ShowStandardHotKeys;

        public static string ShowStandardHotKeysWithPipeline { get; private set; } = PromptPlusResources.ShowStandardHotKeysWithPipeline;

        public static string ShowProcessStandardHotKeysWithPipeline { get; private set; } = PromptPlusResources.ShowProcessStandardHotKeysWithPipeline;

        public static string ShowKeyPressStandardHotKeys { get; private set; } = PromptPlusResources.ShowKeyPressStandardHotKeys;

        public static string ShowKeyPressStandardHotKeysWithPipeline { get; private set; } = PromptPlusResources.ShowKeyPressStandardHotKeysWithPipeline;

        public static string MaskEditPosAnyChar { get; private set; } = PromptPlusResources.MaskEditPosAnyChar;

        public static string MaskEditPosCustom { get; private set; } = PromptPlusResources.MaskEditPosCustom;

        public static string MaskEditPosDay { get; private set; } = PromptPlusResources.MaskEditPosDay;

        public static string MaskEditPosHour { get; private set; } = PromptPlusResources.MaskEditPosHour;

        public static string MaskEditPosLetter { get; private set; } = PromptPlusResources.MaskEditPosLetter;

        public static string MaskEditPosMinute { get; private set; } = PromptPlusResources.MaskEditPosMinute;

        public static string MaskEditPosMonth { get; private set; } = PromptPlusResources.MaskEditPosMonth;

        public static string MaskEditPosNumeric { get; private set; } = PromptPlusResources.MaskEditPosNumeric;

        public static string MaskEditPosSecond { get; private set; } = PromptPlusResources.MaskEditPosSecond;

        public static string MaskEditPosYear { get; private set; } = PromptPlusResources.MaskEditPosYear;

        public static string MaskEditErase { get; private set; } = PromptPlusResources.MaskEditErase;

        public static string MaskEditInputType { get; private set; } = PromptPlusResources.MaskEditInputType;

        public static string InvalidTypeBoolean { get; private set; } = PromptPlusResources.InvalidTypeBoolean;

        public static string InvalidTypeByte { get; private set; } = PromptPlusResources.InvalidTypeByte;

        public static string InvalidTypeChar { get; private set; } = PromptPlusResources.InvalidTypeChar;

        public static string InvalidTypeDateTime { get; private set; } = string.Format(PromptPlusResources.InvalidTypeDateTime, FormatDate(), FormatTime());

        private static string FormatDate()
        {
            var dtsep = Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator;
            var stddtfmt = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper().Split(Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator[0]);
            return $"{stddtfmt[0][0]}{dtsep}{stddtfmt[1][0]}{dtsep}{stddtfmt[2][0]}";
        }

        private static string FormatTime()
        {
            var tmsep = Thread.CurrentThread.CurrentCulture.DateTimeFormat.TimeSeparator;
            var stdtmfmt = Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern.ToUpper().Split(Thread.CurrentThread.CurrentCulture.DateTimeFormat.TimeSeparator[0]);
            return $"{stdtmfmt[0][0]}{tmsep}{stdtmfmt[1][0]}{tmsep}{stdtmfmt[2][0]}";
        }


        public static string InvalidTypeNumber { get; private set; } = PromptPlusResources.InvalidTypeNumber;
    }
}
