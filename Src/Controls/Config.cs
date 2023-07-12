// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Resources;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using PPlus.Controls.Objects;
using System.Collections.Generic;
using System.Linq;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the common config properties for all controls.
    /// </summary>
    public class Config
    {
        internal Dictionary<SymbolType, (string value, string unicode)> _globalSymbols = new();

        internal Config()
        {
            InitSymbols();
            AppCulture = Thread.CurrentThread.CurrentCulture;
            DefaultCulture = null;
        }

        private CultureInfo? _defaultCulture;

        /// <summary>
        /// Get/Set default Culture(<see cref="CultureInfo"/>) for all controls.
        /// </summary>
        public CultureInfo DefaultCulture
        {
            get
            {
                if (_defaultCulture == null)
                { 
                    return Thread.CurrentThread.CurrentCulture;
                }
                return _defaultCulture;
            }
            set
            {
                var localvalue = value;
                var fallback = false;
                if (localvalue == null)
                {
                    fallback = true;
                    _defaultCulture = null;
                    localvalue = Thread.CurrentThread.CurrentCulture;
                }
                IsImplementedResource = ImplementedResource(localvalue);
                if (!IsImplementedResource)
                {
                    if (File.Exists($"PromptPlus.{localvalue.Name}.resources"))
                    {

                        var rm = ResourceManager.CreateFileBasedResourceManager($"PromptPlus", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), null);
                        var innerField = typeof(PromptPlusResources).GetField("resourceMan", BindingFlags.NonPublic | BindingFlags.Static);
                        innerField.SetValue(null, rm);
                    }
                }
                else
                {
                    var innerField = typeof(PromptPlusResources).GetField("resourceCulture", BindingFlags.NonPublic | BindingFlags.Static);
                    innerField.SetValue(null, localvalue);
                }
                PromptPlusResources.Culture = localvalue;
                if (!fallback)
                {
                    _defaultCulture = localvalue;
                }
                Messages.UpdateCulture();
            }
        }

        private int _pageSize = 10;

        /// <summary>
        /// Get/Set Page Size from colletions.
        /// <br>Default value : 10. If value less than 1 internal sette to 1.</br>
        /// </summary>
        public int PageSize 
        {
            get { return _pageSize; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                _pageSize = value;
            } 
        }


        private int _completionMinimumPrefixLength = 3;

        /// <summary>
        /// Get/Set Minimum Prefix Length.
        /// <br>Default value : 3.If value less than 0 internal sette to 0.</br>
        /// </summary>
        public int CompletionMinimumPrefixLength 
        {
            get { return _completionMinimumPrefixLength; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _completionMinimumPrefixLength = value;
            }
        }

        private int _completionWaitToStart = 1000;

        /// <summary>
        /// Get/Set Interval in mileseconds to wait start Completion funcion.
        /// <br>Default value : 1000. If value less than 10 internal sette to 10.</br>
        /// </summary>
        public int CompletionWaitToStart
        {
            get { return _completionWaitToStart; }
            set
            {
                if (value < 10)
                {
                    value = 10;
                }
                _completionWaitToStart = value;
            }
        }
  
        private int _completionMaxCount = 1000;

        /// <summary>
        /// Get/Set Completion Max Items to return.
        /// <br>Default value : 1000. If value  less than 1 internal sette to 1.</br>
        /// </summary>  
        public int CompletionMaxCount 
        {
            get { return _completionMaxCount; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                _completionMaxCount = value;
            }
        }

        /// <summary>
        /// Get/Set History Timeout.
        /// <br>Default value : 365 days</br>
        /// </summary>
        public TimeSpan HistoryTimeout { get; set; } = FileHistory.DefaultHistoryTimeout;

        /// <summary>
        /// Get/Set enabled show Tooltip for all controls.
        /// <br>Default value : true</br>
        /// </summary>
        public bool ShowTooltip { get; set; } = true;

        /// <summary>
        /// Get/Set enabled abortKey(ESC) for all controls.
        /// <br>Default value : true</br>
        /// </summary>
        public bool EnabledAbortKey { get; set; } = true;


        /// <summary>
        /// Get/Set hide controls after finish for all controls.
        /// <br>Default value : false</br>
        /// </summary>
        public bool HideAfterFinish { get; set; } = false;

        /// <summary>
        /// Get/Set hide controls On Abort for all controls.
        /// <br>Default value : false</br>
        /// </summary>
        public bool HideOnAbort { get; set; } = false;

        private char? _secretChar = '#';

        /// <summary>
        /// Get/Set value char for secret input
        /// <br>Default value : '#'.  Fall-back when null : '#'</br>
        /// </summary>
        public char? SecretChar
        {
            get
            {
                if (_secretChar.HasValue)
                {
                    return _secretChar.Value;
                }
                return '#';
            }
            set
            {
                _secretChar = value;
            }
        }

        private char? _yesChar;

        /// <summary>
        /// Get/Set value for YES answer
        /// <br>Default value : YesChar in built-in resources.  Fall-back when null : Y</br>
        /// </summary>
        public char? YesChar 
        {
            get 
            {
                if (_yesChar.HasValue)
                { 
                    return _yesChar.Value;
                }
                return Messages.YesChar[0];
            }
            set
            {
                _yesChar = value;
            }
        }

        private char? _noChar;

        /// <summary>
        /// Get/Set value for NO answer
        /// <br>Default value : NoChar in built-in resources.  Fall-back when null : N</br>
        /// </summary>
        public char? NoChar
        {
            get
            {
                if (_noChar.HasValue)
                {
                    return _noChar.Value;
                }
                return Messages.NoChar[0];
            }
            set
            {
                _noChar = value;
            }
        }


        /// <summary>
        /// Get/Set <see cref="HotKey"/> to show/hide Tooltip.
        /// <br>Default value : '[F1]'</br>
        /// </summary>
        public HotKey TooltipKeyPress => HotKey.TooltipDefault;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to toggle password view.
        /// <br>Default value : '[F2]'</br>
        /// </summary>
        public HotKey PasswordViewPress { get; set; } = HotKey.PasswordViewDefault;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to Select all item.
        /// <br>Default value : '[F2]'</br>
        /// </summary>
        public HotKey SelectAllPress { get; set; } = HotKey.SelectAllDefault;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to Invert Selected item.
        /// <br>Default value : '[F3]'</br>
        /// </summary>
        public HotKey InvertSelectedPress { get; set; } = HotKey.InvertSelectedDefault;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to Edit item.
        /// <br>Default value : '[F2]'</br>
        /// </summary>
        public HotKey EditItemPress { get; set; } = HotKey.EditItemDefault;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> to Remove item.
        /// <br>Default value : '[F3]'</br>
        /// </summary>
        public HotKey RemoveItemPress { get; set; } = HotKey.RemoveItemDefault;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> toggle current path to full path.
        /// <br>Default value : '[F2]'</br>
        /// </summary>
        public HotKey FullPathPress { get; set; } = HotKey.TooltipFullPathDefault;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> Toggle Expand/Collapse node.
        /// <br>Default value : '[F3]'</br>
        /// </summary>
        public HotKey ToggleExpandPress { get; set; } = HotKey.ToggleExpandNodeDefault;

        /// <summary>
        /// Get/Set <see cref="HotKey"/> Toggle Expand /Collapse All node.
        /// <br>Default value : '[F4]'</br>
        /// </summary>
        public HotKey ToggleExpandAllPress { get; set; } = HotKey.ToggleExpandAllNodeDefault;


        /// <summary>
        /// Get/Set the Symbols for all controls.If empty params return current set.
        /// </summary>
        /// <param name="schema"><see cref="SymbolType"/> to set </param>
        /// <param name="value">Text symbol when not unicode support</param>
        /// <param name="unicode">Text symbol when has unicode support</param>
        /// <returns>(string value, string unicode) value</returns>
        public (string value, string unicode) Symbols(SymbolType schema, string? value = null, string? unicode = null)
        {
            if (value == null && unicode == null)
            {
                return _globalSymbols[schema];
            }
            if (value == null && unicode != null)
            {
                value = unicode;
            }
            _globalSymbols[schema] = (value, unicode ?? value);
            return _globalSymbols[schema];
        }

        internal CultureInfo AppCulture { get; set; } = CultureInfo.CurrentCulture;

        internal HotKey AbortKeyPress { get; set; } = new(ConsoleKey.Escape, false, false, false);

        internal bool IsImplementedResource { get; private set; } = true;

        private void InitSymbols()
        {
            _globalSymbols = new();
            var aux = Enum.GetValues(typeof(SymbolType)).Cast<SymbolType>();
            foreach (var item in aux)
            {
                switch (item)
                {
                    case SymbolType.MaskEmpty:
                        _globalSymbols.Add(SymbolType.MaskEmpty, ("_", "■"));
                        break;
                    case SymbolType.Done:
                        _globalSymbols.Add(SymbolType.Done, ("V", "√"));
                        break;
                    case SymbolType.Selector:
                        _globalSymbols.Add(SymbolType.Selector, (">", "›"));
                        break;
                    case SymbolType.Selected:
                        _globalSymbols.Add(SymbolType.Selected, ("*", "♦"));
                        break;
                    case SymbolType.NotSelect:
                        _globalSymbols.Add(SymbolType.NotSelect, ("o", "○"));
                        break;
                    case SymbolType.Expanded:
                        _globalSymbols.Add(SymbolType.Expanded, ("[-]", "[-]"));
                        break;
                    case SymbolType.Collapsed:
                        _globalSymbols.Add(SymbolType.Collapsed, ("[+]", "[+]"));
                        break;
                    case SymbolType.IndentGroup:
                        _globalSymbols.Add(SymbolType.IndentGroup, ("|-", "├─"));
                        break;
                    case SymbolType.IndentEndGroup:
                        _globalSymbols.Add(SymbolType.IndentEndGroup, ("|_", "└─"));
                        break;
                    case SymbolType.TreeLinecross:
                        _globalSymbols.Add(SymbolType.TreeLinecross, (" |-", " ├─"));
                        break;
                    case SymbolType.TreeLinecorner:
                        _globalSymbols.Add(SymbolType.TreeLinecorner, (" |_", " └─"));
                        break;
                    case SymbolType.TreeLinevertical:
                        _globalSymbols.Add(SymbolType.TreeLinevertical, (" | ", " │ "));
                        break;
                    case SymbolType.TreeLinespace:
                        _globalSymbols.Add(SymbolType.TreeLinespace, ("   ", "   "));
                        break;
                    case SymbolType.DoubleBorder:
                        _globalSymbols.Add(SymbolType.DoubleBorder, ("=", "═"));
                        break;
                    case SymbolType.SingleBorder:
                        _globalSymbols.Add(SymbolType.SingleBorder, ("-", "─"));
                        break;
                    case SymbolType.HeavyBorder:
                        _globalSymbols.Add(SymbolType.HeavyBorder, ("*", "━"));
                        break;
                    default:
                        throw new PromptPlusException($"Symbol {item} Not Implemented");

                }
            }
        }

        private static bool ImplementedResource(CultureInfo cultureInfo)
        {

            var code = cultureInfo.Name;
            if (code == "en-US")
            {
                return true;
            }
            else if (code == "pt-BR")
            {
                return true;
            }
            return false;
        }
    }
}
