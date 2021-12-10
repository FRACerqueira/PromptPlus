// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using PPlus.Internal;
using PPlus.Objects;

namespace PPlus
{
    public static partial class PromptPlus
    {
        public static string SaveConfigToFile(string folderfile = "")
        {
            var theme = new Theme
            {
                EnabledBeep = EnabledBeep,
                EnabledAbortAllPipes = EnabledAbortAllPipes,
                EnabledAbortKey = EnabledAbortKey,
                EnabledStandardTooltip = EnabledStandardTooltip,
                EnabledPromptTooltip = EnabledPromptTooltip,
                PasswordChar = PasswordChar,
                Culture = DefaultCulture.Name,
                Version = Theme.CurrentVersion
            };

            theme.Colors.Answer = ColorSchema.Answer;
            theme.Colors.BackColorSchema = PPlusConsole.BackgroundColor;
            theme.Colors.Disabled = ColorSchema.Disabled;
            theme.Colors.DoneSymbol = ColorSchema.DoneSymbol;
            theme.Colors.Error = ColorSchema.Error;
            theme.Colors.Filter = ColorSchema.Filter;
            theme.Colors.ForeColorSchema = PPlusConsole.ForegroundColor;
            theme.Colors.Hint = ColorSchema.Hint;
            theme.Colors.Pagination = ColorSchema.Pagination;
            theme.Colors.PromptSymbol = ColorSchema.PromptSymbol;
            theme.Colors.Select = ColorSchema.Select;
            theme.Colors.SliderBackcolor = ColorSchema.SliderBackcolor;
            theme.Colors.SliderForecolor = ColorSchema.SliderForecolor;

            theme.Colors.Description = ColorSchema.Description;

            theme.HotKeys.MarkSelect = MarkSelect.ToString();
            theme.HotKeys.ToggleVisibleDescription = ToggleVisibleDescription.ToString();
            theme.HotKeys.AbortAllPipesKeyPress = AbortAllPipesKeyPress.ToString();
            theme.HotKeys.TooltipKeyPress = TooltipKeyPress.ToString();
            theme.HotKeys.ResumePipesKeyPress = ResumePipesKeyPress.ToString();
            theme.HotKeys.UnSelectFilter = UnSelectFilter.ToString();
            theme.HotKeys.SwitchViewPassword = SwitchViewPassword.ToString();
            theme.HotKeys.SelectAll = SelectAll.ToString();
            theme.HotKeys.InvertSelect = InvertSelect.ToString();
            theme.HotKeys.RemoveAll = RemoveAll.ToString();

            theme.Symbols.MaskEmpty = Symbols.MaskEmpty;
            theme.Symbols.Done = Symbols.Done;
            theme.Symbols.Error = Symbols.Error;
            theme.Symbols.File = Symbols.File;
            theme.Symbols.Folder = Symbols.Folder;
            theme.Symbols.NotSelect = Symbols.NotSelect;
            theme.Symbols.Prompt = Symbols.Prompt;
            theme.Symbols.Selected = Symbols.Selected;
            theme.Symbols.Selector = Symbols.Selector;
            theme.Symbols.Skiped = Symbols.Skiped;
            theme.Symbols.TaskRun = Symbols.TaskRun;

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                IgnoreReadOnlyProperties = true,
            };
            var pathfile = Path.Combine(folderfile, "PromptPlus.config.json");
            File.WriteAllText(pathfile, JsonSerializer.Serialize(theme, options));
            return pathfile;
        }

        public static void LoadConfigFromFile(string folderfile = "")
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                IgnoreReadOnlyProperties = false,
                PropertyNameCaseInsensitive = true,
#pragma warning disable SYSLIB0020 // Type or member is obsolete
                IgnoreNullValues = true,
#pragma warning restore SYSLIB0020 // Type or member is obsolete
            };

            var pathfile = Path.Combine(folderfile, "PromptPlus.config.json");
            if (File.Exists(pathfile))
            {
                var theme = JsonSerializer.Deserialize<Theme>(File.ReadAllText(pathfile), options);

                if (theme.Version < 1)
                {
                    theme.Version = 1;
                }

                if (string.IsNullOrEmpty(theme.Culture))
                {
                    DefaultCulture = new CultureInfo(AppCulture.Name);
                }
                else
                {
                    DefaultCulture = new CultureInfo(theme.Culture);
                }
                EnabledBeep = theme.EnabledBeep;
                EnabledAbortAllPipes = theme.EnabledAbortAllPipes;
                EnabledAbortKey = theme.EnabledAbortKey;
                EnabledStandardTooltip = theme.EnabledStandardTooltip;
                EnabledPromptTooltip = theme.EnabledPromptTooltip;
                PasswordChar = theme.PasswordChar ?? '#';

                Symbols.MaskEmpty = theme.Symbols.MaskEmpty;
                Symbols.Done = theme.Symbols.Done;
                Symbols.Error = theme.Symbols.Error;
                Symbols.File = theme.Symbols.File;
                Symbols.Folder = theme.Symbols.Folder;
                Symbols.NotSelect = theme.Symbols.NotSelect;
                Symbols.Prompt = theme.Symbols.Prompt;
                Symbols.Selected = theme.Symbols.Selected;
                Symbols.Selector = theme.Symbols.Selector;
                Symbols.Skiped = theme.Symbols.Skiped;
                Symbols.TaskRun = theme.Symbols.TaskRun;

                ColorSchema.Answer = theme.Colors.Answer;
                ColorSchema.Disabled = theme.Colors.Disabled;
                ColorSchema.DoneSymbol = theme.Colors.DoneSymbol;
                ColorSchema.Error = theme.Colors.Error;
                ColorSchema.Filter = theme.Colors.Filter;
                ColorSchema.Hint = theme.Colors.Hint;
                ColorSchema.Pagination = theme.Colors.Pagination;
                ColorSchema.PromptSymbol = theme.Colors.PromptSymbol;
                ColorSchema.Select = theme.Colors.Select;
                ColorSchema.SliderBackcolor = theme.Colors.SliderBackcolor;
                ColorSchema.SliderForecolor = theme.Colors.SliderForecolor;

                if (theme.Version >= 2)
                {
                    MarkSelect = ConverteThemeHotkey(theme.HotKeys.MarkSelect);
                    ToggleVisibleDescription = ConverteThemeHotkey(theme.HotKeys.ToggleVisibleDescription);
                    ColorSchema.Description = theme.Colors.Description;
                }
                else
                {
                    MarkSelect = ConverteThemeHotkey(UserHotKey.F8.ToString());
                    ToggleVisibleDescription = ToggleVisibleDescription;
                    ColorSchema.Description = ColorSchema.Answer;
                }
                AbortAllPipesKeyPress = ConverteThemeHotkey(theme.HotKeys.AbortAllPipesKeyPress);
                TooltipKeyPress = ConverteThemeHotkey(theme.HotKeys.TooltipKeyPress);
                ResumePipesKeyPress = ConverteThemeHotkey(theme.HotKeys.ResumePipesKeyPress);
                UnSelectFilter = ConverteThemeHotkey(theme.HotKeys.UnSelectFilter);
                SwitchViewPassword = ConverteThemeHotkey(theme.HotKeys.SwitchViewPassword);
                SelectAll = ConverteThemeHotkey(theme.HotKeys.SelectAll);
                InvertSelect = ConverteThemeHotkey(theme.HotKeys.InvertSelect);
                RemoveAll = ConverteThemeHotkey(theme.HotKeys.RemoveAll);
                ConsoleDefaultColor(theme.Colors.ForeColorSchema, theme.Colors.BackColorSchema);

            }
        }

        private static HotKey ConverteThemeHotkey(string value)
        {
            var elem = value.Split('+');
            var altkey = elem.Any(x => x.ToLower() == "alt");
            var shiftkey = elem.Any(x => x.ToLower() == "shift");
            var ctrlkey = elem.Any(x => x.ToLower() == "crtl");
            var key = elem.Last().ToLower();
            return new HotKey(FindByText(key), altkey, ctrlkey, shiftkey);
        }

        private static UserHotKey FindByText(string key)
        {
            var itens = Enum.GetValues(typeof(UserHotKey));
            foreach (var item in itens)
            {
                if (item.ToString().ToLower() == key.ToLower())
                {
                    return (UserHotKey)item;
                }
            }
            throw new ArgumentException(key);
        }

    }
}
