// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PPlus.FIGlet;

using PPlus.Objects;

namespace PPlus
{
    public interface IPromptConfig
    {
        IPromptConfig EnabledAbortKey(bool value);
        IPromptConfig EnabledAbortAllPipes(bool value);
        IPromptConfig EnabledPromptTooltip(bool value);
        IPromptConfig HideAfterFinish(bool value);
    }

    public interface IPromptControls<T>: IPromptConfig
    {
        ResultPromptPlus<T> Run(CancellationToken? value = null);
    }

    public interface IFIGlet
    {
        IFIGlet LoadFont(string value);
        IFIGlet LoadFont(Stream value);
        IFIGlet FIGletWidth(CharacterWidth value);
        void Run(ConsoleColor? color = null);
    }

    public interface IFormPlusBase : IDisposable
    {
        string PipeId { get; }

        string PipeTitle { get; }

        object ContextState { get; }

        Func<ResultPipe[], object, bool> Condition { get; }
    }

    public interface IPromptPipe
    {
        IPromptPipe PipeCondition(Func<ResultPipe[], object, bool> condition);
        IFormPlusBase ToPipe(string id, string title, object state = null);
    }

    public interface IControlPipeLine
    {
        IControlPipeLine AddPipe(IFormPlusBase value);
        IControlPipeLine AddPipes(IEnumerable<IFormPlusBase> value);
        ResultPromptPlus<IEnumerable<ResultPipe>> Run(CancellationToken? value = null);
    }

    public interface IControlKeyPress : IPromptControls<bool>, IPromptPipe
    {
        IControlKeyPress Prompt(string value);
        IControlKeyPress Config(Action<IPromptConfig> context);
    }

    public interface IControlMaskEdit : IPromptControls<ResultMasked>, IPromptPipe
    {
        IControlMaskEdit Prompt(string value, string description = null);
        IControlMaskEdit ShowInputType(bool value);
        IControlMaskEdit AddValidator(Func<object, ValidationResult> value);
        IControlMaskEdit AddValidators(IEnumerable<Func<object, ValidationResult>> value);
        IControlMaskEdit Mask(string value);
        IControlMaskEdit Default(object value);
        IControlMaskEdit UpperCase(bool value);
        IControlMaskEdit Culture(CultureInfo cultureinfo);
        IControlMaskEdit FillZeros(bool value);
        IControlMaskEdit FormatYear(FormatYear value);
        IControlMaskEdit FormatTime(FormatTime value);
        IControlMaskEdit AmmoutPositions(int intvalue, int decimalvalue);
        IControlMaskEdit AcceptSignal(bool value);
        IControlMaskEdit ShowDayWeek(FormatWeek value);
        IControlMaskEdit ValidateOnDemand();
        IControlMaskEdit DescriptionSelector(Func<ResultMasked, string> value);
        IControlMaskEdit Config(Action<IPromptConfig> context);

    }

    public interface IControlInput : IPromptControls<string>, IPromptPipe
    {
        IControlInput Prompt(string value, string description = null);
        IControlInput Default(string value);
        IControlInput InitialValue(string value);
        IControlInput IsPassword(bool swithVisible);
        IControlInput AddValidator(Func<object, ValidationResult> validator);
        IControlInput AddValidators(IEnumerable<Func<object, ValidationResult>> validators);
        IControlInput ValidateOnDemand();
        IControlInput DescriptionSelector(Func<string, string> value);
        IControlInput Config(Action<IPromptConfig> context);
    }

    public interface IControlSliderNumber : IPromptControls<double>, IPromptPipe
    {
        IControlSliderNumber Prompt(string value, string description = null);
        IControlSliderNumber Default(double value);
        IControlSliderNumber Range(double minvalue, double maxvalue);
        IControlSliderNumber Step(double value);
        IControlSliderNumber LargeStep(double value);
        IControlSliderNumber FracionalDig(int value);
        IControlSliderNumber Config(Action<IPromptConfig> context);

    }

    public interface IControlSliderSwitch : IPromptControls<bool>, IPromptPipe
    {
        IControlSliderSwitch Prompt(string value, string description = null);
        IControlSliderSwitch Default(bool value);
        IControlSliderSwitch OffValue(string value);
        IControlSliderSwitch OnValue(string value);
        IControlSliderSwitch Config(Action<IPromptConfig> context);
    }

    public interface IControlProgressbar : IPromptControls<ProgressBarInfo>, IPromptPipe
    {
        IControlProgressbar Prompt(string value, string description = null);
        IControlProgressbar UpdateHandler(Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> value);
        IControlProgressbar Width(int value);
        IControlProgressbar StartInterationId(object value);
        IControlProgressbar Config(Action<IPromptConfig> context);

    }

    public interface IControlWaitProcess : IPromptControls<IEnumerable<ResultProcess>>, IPromptPipe
    {
        IControlWaitProcess Prompt(string value, string description = null);
        IControlWaitProcess AddProcess(SingleProcess process);
        IControlWaitProcess SpeedAnimation(int value);
        IControlWaitProcess Config(Action<IPromptConfig> context);
    }

    public interface IControlConfirm : IPromptControls<bool>, IPromptPipe
    {
        IControlConfirm Prompt(string value, string description = null);
        IControlConfirm Default(bool value);
        IControlConfirm Config(Action<IPromptConfig> context);

    }

    public interface IControlAutoComplete : IPromptControls<string>, IPromptPipe
    {
        IControlAutoComplete Prompt(string value, string description = null);
        IControlAutoComplete PageSize(int value);
        IControlAutoComplete AddValidator(Func<object, ValidationResult> validator);
        IControlAutoComplete AddValidators(IEnumerable<Func<object, ValidationResult>> validators);
        IControlAutoComplete ValidateOnDemand();
        IControlAutoComplete AcceptWithoutMatch();
        IControlAutoComplete SpeedAnimation(int value);
        IControlAutoComplete MinimumPrefixLength(int value);
        IControlAutoComplete CompletionInterval(int value);
        IControlAutoComplete CompletionMaxCount(int value);
        IControlAutoComplete CompletionAsyncService(Func<string, int, CancellationToken, Task<string[]>> value);
        IControlAutoComplete CompletionWithDescriptionAsyncService(Func<string, int, CancellationToken, Task<ValueDescription<string>[]>> value);
        IControlAutoComplete Config(Action<IPromptConfig> context);
    }


    public interface IControlSelect<T> : IPromptControls<T>, IPromptPipe
    {
        IControlSelect<T> Prompt(string value, string description = null);
        IControlSelect<T> Default(T value);
        IControlSelect<T> PageSize(int value);
        IControlSelect<T> TextSelector(Func<T, string> value);
        IControlSelect<T> DescriptionSelector(Func<T, string> value);
        IControlSelect<T> AddItem(T value);
        IControlSelect<T> AddItems(IEnumerable<T> value);
        IControlSelect<T> HideItem(T value);
        IControlSelect<T> HideItems(IEnumerable<T> value);
        IControlSelect<T> DisableItem(T value);
        IControlSelect<T> DisableItems(IEnumerable<T> value);
        IControlSelect<T> AutoSelectIfOne();
        IControlSelect<T> Config(Action<IPromptConfig> context);

    }

    public interface IControlMultiSelect<T> : IPromptControls<IEnumerable<T>>, IPromptPipe
    {
        IControlMultiSelect<T> Prompt(string value, string description = null);
        IControlMultiSelect<T> AddDefault(T value);
        IControlMultiSelect<T> AddDefaults(IEnumerable<T> value);
        IControlMultiSelect<T> PageSize(int value);
        IControlMultiSelect<T> TextSelector(Func<T, string> value);
        IControlMultiSelect<T> DescriptionSelector(Func<T, string> value);
        IControlMultiSelect<T> ShowGroupOnDescription(string noGroupMessage);
        IControlMultiSelect<T> AddItem(T value);
        IControlMultiSelect<T> AddItems(IEnumerable<T> value);
        IControlMultiSelect<T> AddGroup(IEnumerable<T> value, string group);
        IControlMultiSelect<T> HideItem(T value);
        IControlMultiSelect<T> HideItems(IEnumerable<T> value);
        IControlMultiSelect<T> DisableItem(T value);
        IControlMultiSelect<T> DisableItems(IEnumerable<T> value);
        IControlMultiSelect<T> Range(int minvalue, int maxvalue);
        IControlMultiSelect<T> Config(Action<IPromptConfig> context);
    }

    public interface IControlList<T> : IPromptControls<IEnumerable<T>>, IPromptPipe
    {
        IControlList<T> Prompt(string value, string description = null);
        IControlList<T> AddItem(T value);
        IControlList<T> AddItems(IEnumerable<T> value);
        IControlList<T> PageSize(int value);
        IControlList<T> TextSelector(Func<T, string> value);
        IControlList<T> Range(int minvalue, int maxvalue);
        IControlList<T> UpperCase(bool value);
        IControlList<T> AllowDuplicate(bool value);
        IControlList<T> AddValidator(Func<object, ValidationResult> validator);
        IControlList<T> AddValidators(IEnumerable<Func<object, ValidationResult>> validators);
        IControlList<T> ValidateOnDemand();
        IControlList<T> DescriptionSelector(Func<string, string> value);
        IControlList<T> Config(Action<IPromptConfig> context);
    }

    public interface IControlListMasked : IPromptControls<IEnumerable<ResultMasked>>, IPromptPipe
    {
        IControlListMasked Prompt(string value, string description = null);
        IControlListMasked PageSize(int value);
        IControlListMasked Range(int minvalue, int maxvalue);
        IControlListMasked ShowInputType(bool value);
        IControlListMasked AddItem(string value);
        IControlListMasked AddItems(IEnumerable<string> value);
        IControlListMasked AddValidator(Func<object, ValidationResult> validator);
        IControlListMasked AddValidators(IEnumerable<Func<object, ValidationResult>> validators);
        IControlListMasked MaskType(MaskedType value, string mask = null);
        IControlListMasked UpperCase(bool value);
        IControlListMasked Culture(CultureInfo cultureinfo);
        IControlListMasked FillZeros(bool value);
        IControlListMasked FormatYear(FormatYear value);
        IControlListMasked FormatTime(FormatTime value);
        IControlListMasked ShowDayWeek(FormatWeek value);
        IControlListMasked AmmoutPositions(int intvalue, int decimalvalue);
        IControlListMasked AcceptSignal(bool signal);
        IControlListMasked ValidateOnDemand();
        IControlListMasked DescriptionSelector(Func<ResultMasked, string> value);
        IControlListMasked Config(Action<IPromptConfig> context);
    }

    public interface IControlBrowser : IPromptControls<ResultBrowser>, IPromptPipe
    {
        IControlBrowser Prompt(string value, string description = null);
        IControlBrowser Filter(BrowserFilter value);
        IControlBrowser Default(string value);
        IControlBrowser PrefixExtension(string value);
        IControlBrowser AllowNotSelected(bool value);
        IControlBrowser Root(string value);
        IControlBrowser SearchPattern(string value);
        IControlBrowser PageSize(int value);
        IControlBrowser SupressHidden(bool value);
        IControlBrowser PromptCurrentPath(bool value);
        IControlBrowser promptSearchPattern(bool value);
        IControlBrowser Config(Action<IPromptConfig> context);
    }
}
