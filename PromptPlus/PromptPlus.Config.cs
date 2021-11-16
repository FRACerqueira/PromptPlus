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

using PromptPlusInternal;

using PromptPlusObjects;

namespace PromptPlusControls
{
    public static partial class PromptPlus
    {
        public static string SaveConfigToFile(string folderfile = "")
        {
            var theme = new Theme
            {
                EnabledBeep = PromptPlus.EnabledBeep,
                EnabledAbortAllPipes = PromptPlus.EnabledAbortAllPipes,
                EnabledAbortKey = PromptPlus.EnabledAbortKey,
                EnabledStandardTooltip = PromptPlus.EnabledStandardTooltip,
                EnabledPromptTooltip = PromptPlus.EnabledPromptTooltip,
                PasswordChar = PromptPlus.PasswordChar,
                Culture = PromptPlus.DefaultCulture.Name,
                Version = Theme.CurrentVersion
            };

            theme.Colors.Description = PromptPlus.ColorSchema.Description;
            theme.Colors.Answer = PromptPlus.ColorSchema.Answer;
            theme.Colors.BackColorSchema = PromptPlus._consoleDriver.BackgroundColor;
            theme.Colors.Disabled = PromptPlus.ColorSchema.Disabled;
            theme.Colors.DoneSymbol = PromptPlus.ColorSchema.DoneSymbol;
            theme.Colors.Error = PromptPlus.ColorSchema.Error;
            theme.Colors.Filter = PromptPlus.ColorSchema.Filter;
            theme.Colors.ForeColorSchema = PromptPlus._consoleDriver.ForegroundColor;
            theme.Colors.Hint = PromptPlus.ColorSchema.Hint;
            theme.Colors.Pagination = PromptPlus.ColorSchema.Pagination;
            theme.Colors.PromptSymbol = PromptPlus.ColorSchema.PromptSymbol;
            theme.Colors.Select = PromptPlus.ColorSchema.Select;
            theme.Colors.SliderBackcolor = PromptPlus.ColorSchema.SliderBackcolor;
            theme.Colors.SliderForecolor = PromptPlus.ColorSchema.SliderForecolor;

            theme.HotKeys.ToggleVisibleDescription = PromptPlus.ToggleVisibleDescription.ToString();
            theme.HotKeys.AbortAllPipesKeyPress = PromptPlus.AbortAllPipesKeyPress.ToString();
            theme.HotKeys.AbortKeyPress = PromptPlus.AbortKeyPress.ToString();
            theme.HotKeys.TooltipKeyPress = PromptPlus.TooltipKeyPress.ToString();
            theme.HotKeys.ResumePipesKeyPress = PromptPlus.ResumePipesKeyPress.ToString();
            theme.HotKeys.UnSelectFilter = PromptPlus.UnSelectFilter.ToString();
            theme.HotKeys.SwitchViewPassword = PromptPlus.SwitchViewPassword.ToString();
            theme.HotKeys.SelectAll = PromptPlus.SelectAll.ToString();
            theme.HotKeys.InvertSelect = PromptPlus.InvertSelect.ToString();
            theme.HotKeys.RemoveAll = PromptPlus.RemoveAll.ToString();

            theme.Symbols.MaskEmpty = PromptPlus.Symbols.MaskEmpty;
            theme.Symbols.Done = PromptPlus.Symbols.Done;
            theme.Symbols.Error = PromptPlus.Symbols.Error;
            theme.Symbols.File = PromptPlus.Symbols.File;
            theme.Symbols.Folder = PromptPlus.Symbols.Folder;
            theme.Symbols.NotSelect = PromptPlus.Symbols.NotSelect;
            theme.Symbols.Prompt = PromptPlus.Symbols.Prompt;
            theme.Symbols.Selected = PromptPlus.Symbols.Selected;
            theme.Symbols.Selector = PromptPlus.Symbols.Selector;
            theme.Symbols.Skiped = PromptPlus.Symbols.Skiped;
            theme.Symbols.TaskRun = PromptPlus.Symbols.TaskRun;

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
                    PromptPlus.DefaultCulture = new CultureInfo(PromptPlus.AppCulture.Name);
                }
                else
                {
                    PromptPlus.DefaultCulture = new CultureInfo(theme.Culture);
                }
                PromptPlus.EnabledBeep = theme.EnabledBeep;
                PromptPlus.EnabledAbortAllPipes = theme.EnabledAbortAllPipes;
                PromptPlus.EnabledAbortKey = theme.EnabledAbortKey;
                PromptPlus.EnabledStandardTooltip = theme.EnabledStandardTooltip;
                PromptPlus.EnabledPromptTooltip = theme.EnabledPromptTooltip;
                PromptPlus.PasswordChar = theme.PasswordChar ?? '#';

                PromptPlus.Symbols.MaskEmpty = theme.Symbols.MaskEmpty;
                PromptPlus.Symbols.Done = theme.Symbols.Done;
                PromptPlus.Symbols.Error = theme.Symbols.Error;
                PromptPlus.Symbols.File = theme.Symbols.File;
                PromptPlus.Symbols.Folder = theme.Symbols.Folder;
                PromptPlus.Symbols.NotSelect = theme.Symbols.NotSelect;
                PromptPlus.Symbols.Prompt = theme.Symbols.Prompt;
                PromptPlus.Symbols.Selected = theme.Symbols.Selected;
                PromptPlus.Symbols.Selector = theme.Symbols.Selector;
                PromptPlus.Symbols.Skiped = theme.Symbols.Skiped;
                PromptPlus.Symbols.TaskRun = theme.Symbols.TaskRun;

                PromptPlus.ColorSchema.Description = theme.Colors.Description;
                PromptPlus.ColorSchema.Answer = theme.Colors.Answer;
                PromptPlus.ColorSchema.Disabled = theme.Colors.Disabled;
                PromptPlus.ColorSchema.DoneSymbol = theme.Colors.DoneSymbol;
                PromptPlus.ColorSchema.Error = theme.Colors.Error;
                PromptPlus.ColorSchema.Filter = theme.Colors.Filter;
                PromptPlus.ColorSchema.Hint = theme.Colors.Hint;
                PromptPlus.ColorSchema.Pagination = theme.Colors.Pagination;
                PromptPlus.ColorSchema.PromptSymbol = theme.Colors.PromptSymbol;
                PromptPlus.ColorSchema.Select = theme.Colors.Select;
                PromptPlus.ColorSchema.SliderBackcolor = theme.Colors.SliderBackcolor;
                PromptPlus.ColorSchema.SliderForecolor = theme.Colors.SliderForecolor;

                if (theme.Version >= 2)
                {
                    PromptPlus.ToggleVisibleDescription = ConverteThemeHotkey(theme.HotKeys.ToggleVisibleDescription);
                }
                else
                {
                    PromptPlus.ToggleVisibleDescription = ToggleVisibleDescription;
                    PromptPlus.ColorSchema.Description = ColorSchema.Answer;
                }
                PromptPlus.AbortAllPipesKeyPress = ConverteThemeHotkey(theme.HotKeys.AbortAllPipesKeyPress);
                PromptPlus.AbortKeyPress = ConverteThemeHotkey(theme.HotKeys.AbortKeyPress);
                PromptPlus.TooltipKeyPress = ConverteThemeHotkey(theme.HotKeys.TooltipKeyPress);
                PromptPlus.ResumePipesKeyPress = ConverteThemeHotkey(theme.HotKeys.ResumePipesKeyPress);
                PromptPlus.UnSelectFilter = ConverteThemeHotkey(theme.HotKeys.UnSelectFilter);
                PromptPlus.SwitchViewPassword = ConverteThemeHotkey(theme.HotKeys.SwitchViewPassword);
                PromptPlus.SelectAll = ConverteThemeHotkey(theme.HotKeys.SelectAll);
                PromptPlus.InvertSelect = ConverteThemeHotkey(theme.HotKeys.InvertSelect);
                PromptPlus.RemoveAll = ConverteThemeHotkey(theme.HotKeys.RemoveAll);
                PromptPlus.ConsoleDefaultColor(theme.Colors.ForeColorSchema, theme.Colors.BackColorSchema);

            }
        }

        private static HotKey ConverteThemeHotkey(string value)
        {
            var elem = value.Split('+');
            var altkey = elem.Any(x => x.ToLower() == "alt");
            var shiftkey = elem.Any(x => x.ToLower() == "shift");
            var ctrlkey = elem.Any(x => x.ToLower() == "crtl");
            var key = elem.Last().ToLower();
            if (key == "esc" || key == "escape")
            {
                return new HotKey(ConsoleKey.Escape, altkey, ctrlkey, shiftkey);
            }
            return new HotKey(FindByText(key), altkey, ctrlkey, shiftkey);
        }

        private static ConsoleKey FindByText(string key)
        {
            var itens = Enum.GetValues(typeof(ConsoleKey));
            foreach (var item in itens)
            {
                if (item.ToString().ToLower() == key.ToLower())
                {
                    return (ConsoleKey)item;
                }
            }
            throw new ArgumentException(key);
        }

    }
}
