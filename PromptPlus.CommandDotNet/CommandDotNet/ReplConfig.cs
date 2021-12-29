// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

using CommandDotNet;
using CommandDotNet.Builders;

using PPlus.Objects;

namespace PPlus.CommandDotNet
{
    internal class ReplConfig : ICloneable
    {
        public AppRunner AppRunner { get; }
        public bool InSession { get; set; }
        public Option Option { get; private set; }
        public string AppName { get; private set; }
        public bool InSessionParse { get; set; }
        public CommandContext ReplContext { get; set; }
        public Func<SugestionInput, SugestionOutput> SuggestionHandler { get; internal set; }
        public bool SugestionEnterTryFininsh { get; }
        public bool UseSugestionArgumneType { get; }
        public bool EnabledSugestion { get; }
        public bool EnabledHistory { get; }
        public TimeSpan TimeoutHistory { get; }
        public byte PagesizeHistory { get; }

        public string DefaultFileHistory => $"{AppName}_ReplSession";

        public Func<string, ColorToken> ColorizeSessionInitMessage { get; }


        public ReplConfig(AppRunner appRunner,
            CommandContext cmdContext,
            Option option,
            bool enabledsugestion,
            Func<SugestionInput, SugestionOutput> suggestionHandler,
            bool sugestionEnterTryFininsh,
            bool usesugestionArgumnetype,
            bool enabledHistory,
            byte pagesizehistory,
            TimeSpan timeouthistory,
            Func<string, ColorToken> colorizeSessionInitMessage)
        {
            AppRunner = appRunner ?? throw new ArgumentNullException(nameof(appRunner));
            ReplContext = cmdContext;
            Option = option ?? throw new ArgumentNullException(nameof(option));
            AppName = AppRunner.AppSettings.Help.UsageAppName ?? AppInfo.Instance.FileName;
            EnabledSugestion = enabledsugestion;
            SuggestionHandler = suggestionHandler;
            SugestionEnterTryFininsh = sugestionEnterTryFininsh;
            UseSugestionArgumneType = usesugestionArgumnetype;
            EnabledHistory = enabledHistory;
            TimeoutHistory = timeouthistory;
            PagesizeHistory = pagesizehistory;
            ColorizeSessionInitMessage = colorizeSessionInitMessage;
        }

        public string DefaultSessionInit()
        {
            var appInfo = AppInfo.Instance;
            return string.Format(Resources.Messages.Repl_init_session, AppName, appInfo.Version);
        }

        public object Clone()
        {
            return new ReplConfig(
                AppRunner,
                ReplContext,
                Option,
                EnabledSugestion,
                SuggestionHandler,
                SugestionEnterTryFininsh,
                UseSugestionArgumneType,
                EnabledHistory,
                PagesizeHistory,
                TimeoutHistory,
                ColorizeSessionInitMessage)
            {
                InSession = InSession,
                InSessionParse = InSessionParse,
            };
        }
    }
}
