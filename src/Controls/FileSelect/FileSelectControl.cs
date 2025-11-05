// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PromptPlusLibrary.Controls.FileSelect
{
    internal sealed class FileSelectControl : BaseControlPrompt<ItemFile>, IFileSelectControl
    {
        private readonly Dictionary<FileStyles, Style> _optStyles = BaseControlOptions.LoadStyle<FileStyles>();
        private readonly List<ItemNodeControl<ItemFile>> _items = [];
        private readonly Func<ItemNodeControl<ItemFile>, bool> IsRoot;
        private readonly ConcurrentQueue<(string, bool, List<ItemNodeControl<ItemFile>>)> _resultTask = [];

        private int _indexTooptip;
        private string _tooltipModeSelect = string.Empty;
        private string _tooltipModeFilter = string.Empty;
        private bool _onlyFolders;
        private bool _showFolderInfoFullPath;
        private bool _hideSize;
        private bool _acceptHiddenAttributes;
        private bool _acceptSystemAttributes;
        private string _searchPattern = "*";
        private string _originalsearchPattern = "*";
        private byte _pageSize;
        private string _root = AppDomain.CurrentDomain.BaseDirectory;
        private Func<ItemFile, (bool, string?)>? _predicatevalidselect;
        private Func<ItemFile, bool>? _predicatevaliddisabled;
        private Paginator<ItemNodeControl<ItemFile>>? _localpaginator;
        private EmacsBuffer _filterBuffer;
        private FilterMode _filterType = FilterMode.Disabled;
        private bool _hideZeroEntries;
        private long _minvalueSize = long.MinValue;
        private long _maxvalueSize = long.MaxValue;

        private enum ModeView
        {
            Select,
            Filter
        }
        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Select,[] },
            { ModeView.Filter,[] }
        };
        private ModeView _modeView = ModeView.Select;


#pragma warning disable IDE0079
#pragma warning disable IDE0290 // Use primary constructor
        public FileSelectControl(IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            IsRoot = (item) => item.UniqueId == (_items.Count == 0 ? "" : _items[0].UniqueId);
            _filterBuffer = new EmacsBuffer(false, CaseOptions.Any, (_) => true, ConfigPlus.MaxLenghtFilterText);
            _pageSize = ConfigPlus.PageSize;

        }
#pragma warning restore IDE0290 // Use primary constructor
#pragma warning restore IDE0079

        #region IFileSelectControl

        public IFileSelectControl HideFilesBySize(long minvalue, long maxvalue = long.MaxValue)
        {
            _minvalueSize = minvalue;
            _maxvalueSize = maxvalue;
            return this;
        }
        public IFileSelectControl HideZeroEntries(bool value = true)
        {
            _hideZeroEntries = value;
            return this;
        }

        public IFileSelectControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IFileSelectControl Styles(FileStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public IFileSelectControl OnlyFolders(bool value = true)
        {
            _onlyFolders = value;
            return this;
        }

        public IFileSelectControl EnabledSearchFilter(FilterMode filter = FilterMode.Contains)
        {
            _filterType = filter;
            return this;
        }

        public IFileSelectControl HideSizeInfo(bool value = true)
        {
            _hideSize = value;
            return this;
        }

        public IFileSelectControl AcceptHiddenAttributes(bool value = true)
        {
            _acceptHiddenAttributes = value;
            return this;
        }

        public IFileSelectControl AcceptSystemAttributes(bool value = true)
        {
            _acceptSystemAttributes = value;
            return this;
        }

        public IFileSelectControl SearchPattern(string value)
        {
            _searchPattern = value;
            _originalsearchPattern = value;
            return this;
        }

        public IFileSelectControl PageSize(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be greater or equal than 1");
            }
            _pageSize = value;
            return this;
        }

        public IFileSelectControl Root(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (!Directory.Exists(value))
            {
                throw new ArgumentException("Root folder not exist", nameof(value));
            }
            _root = value;
            return this;
        }

        public IFileSelectControl PredicateSelected(Func<ItemFile, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) =>
            {
                bool fn = validselect(input);
                if (fn)
                {
                    return (true, null);
                }
                return (false, null);
            };
            return this;
        }

        public IFileSelectControl PredicateSelected(Func<ItemFile, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        public IFileSelectControl PredicateDisabled(Func<ItemFile, bool> validdisabled)
        {
            ArgumentNullException.ThrowIfNull(validdisabled);
            _predicatevaliddisabled = validdisabled;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            InitViewFiles();

            _tooltipModeSelect = GetTooltipModeSelect();
            _tooltipModeFilter = GetTooltipModeFilter();
            LoadTooltipToggle();
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            if (_modeView == ModeView.Filter)
            {
                WriteBufferFilter(screenBuffer);
                return;
            }
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            WriteError(screenBuffer);

            WriteDescription(screenBuffer);

            WriteFolderInfo(screenBuffer);

            WriteListSelect(screenBuffer);

            WriteTooltip(screenBuffer);
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsolePlus.CursorVisible;
            ConsolePlus.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    ConsoleKeyInfo keyinfo = WaitKeypressDiscovery(cancellationToken);

                    #region default Press to Finish and tooltip

                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<ItemFile>(new() { Name = "", FullPath = "" }, true);
                        break;
                    }
                    else if (_modeView == ModeView.Select && IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<ItemFile>(new() { Name = "", FullPath = "" }, true);
                        break;
                    }
                    else if (_modeView == ModeView.Filter && IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
                        _searchPattern = _originalsearchPattern;
                        break;
                    }
                    else if (_modeView == ModeView.Filter && keyinfo.IsPressEnterKey())
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
                        if (string.IsNullOrEmpty(_filterBuffer.ToString()))
                        {
                            _searchPattern = _originalsearchPattern;
                            break;
                        }
                        else
                        {
                            _searchPattern = _filterBuffer.ToString();
                        }
                        InitViewFiles();
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey() && _modeView == ModeView.Select && _localpaginator!.SelectedItem != null)
                    {
                        _indexTooptip = 0;
                        if (_localpaginator.SelectedItem.IsDisabled)
                        {
                            SetError(Messages.SelectionDisabled);
                            break;
                        }
                        (bool ok, string? message) = _predicatevalidselect?.Invoke(_localpaginator!.SelectedItem.Value) ?? (true, null);
                        if (!ok)
                        {
                            if (string.IsNullOrEmpty(message))
                            {
                                SetError(Messages.PredicateSelectInvalid);
                            }
                            else
                            {
                                SetError(message);
                            }
                            break;
                        }
                        ResultCtrl = new ResultPrompt<ItemFile>(_localpaginator!.SelectedItem.Value, false);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips[_modeView].Length)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                    #endregion

                    else if (_filterType != FilterMode.Disabled && ConfigPlus.HotKeyFilterMode.Equals(keyinfo))
                    {
                        _indexTooptip = 0;
                        if (_modeView == ModeView.Select)
                        {
                            _modeView = ModeView.Filter;
                            _filterBuffer = new EmacsBuffer(false, CaseOptions.Any, (_) => true, int.MaxValue, ConfigPlus.MaxLenghtFilterText);
                            _filterBuffer.LoadPrintable(_originalsearchPattern);
                        }
                        else
                        {
                            _modeView = ModeView.Select;
                            bool hasfilter = _searchPattern == _originalsearchPattern;
                            _searchPattern = _originalsearchPattern;
                            if (!hasfilter)
                            {
                                InitViewFiles();
                            }
                        }
                        break;
                    }
                    else if (_modeView == ModeView.Filter && _filterBuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_modeView == ModeView.Select && keyinfo.Key == ConsoleKey.None && keyinfo.Modifiers == ConsoleModifiers.Alt)
                    {
                        //has result backgroud task
                        if (_resultTask.TryDequeue(out (string key, bool isload, List<ItemNodeControl<ItemFile>> values) resultitems))
                        {
                            if (resultitems.isload)
                            {
                                LoadTaskResult(resultitems.key, resultitems.values, cancellationToken);
                            }
                            else
                            {
                                UnloadTaskResult(resultitems.key, cancellationToken);
                            }
                            break;
                        }
                    }
                    else if (_modeView == ModeView.Select && keyinfo.IsPressCtrlHomeKey())
                    {
                        _indexTooptip = 0;
                        if (string.IsNullOrEmpty(_localpaginator!.SelectedItem!.ParentUniqueId))
                        {
                            if (!_localpaginator!.Home())
                            {
                                continue;
                            }
                            _indexTooptip = 0;
                            break;
                        }
                        _indexTooptip = 0;
                        int index = _items.FindIndex(x => x.UniqueId == _localpaginator!.SelectedItem!.ParentUniqueId);
                        _localpaginator.EnsureVisibleIndex(index);
                        break;
                    }
                    else if (_modeView == ModeView.Select && keyinfo.IsPressCtrlEndKey())
                    {
                        _indexTooptip = 0;
                        string? parent = _localpaginator!.SelectedItem!.ParentUniqueId;
                        int index = _localpaginator!.SelectedIndex;
                        if (_localpaginator!.SelectedItem.Value.IsFolder && _localpaginator!.SelectedItem.IsExpanded && _localpaginator!.SelectedItem!.Status == NodeStatus.Done)
                        {
                            parent = _localpaginator!.SelectedItem!.UniqueId;
                            index = _items.FindIndex(x => x.UniqueId == parent) + 1;
                        }
                        while (index < _items.Count)
                        {
                            if (_items[index].LastItem && _items[index].ParentUniqueId == parent)
                            {
                                break;
                            }
                            index++;
                        }
                        _localpaginator!.EnsureVisibleIndex(index);
                        break;
                    }
                    else if (_modeView == ModeView.Select && keyinfo.IsPressDownArrowKey())
                    {
                        if (_localpaginator!.IsLastPageItem)
                        {
                            _localpaginator.NextPage(IndexOption.FirstItem);
                        }
                        else
                        {
                            _localpaginator.NextItem();
                        }
                        if (_localpaginator.SelectedItem != null)
                        {
                            if (_localpaginator.SelectedItem.IsDisabled)
                            {
                                SetError(Messages.SelectionDisabled);
                            }
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_modeView == ModeView.Select && keyinfo.IsPressUpArrowKey())
                    {
                        if (_localpaginator!.IsFirstPageItem)
                        {
                            _localpaginator!.PreviousPage(IndexOption.LastItem);
                        }
                        else
                        {
                            _localpaginator!.PreviousItem();
                        }
                        if (_localpaginator.SelectedItem != null)
                        {
                            if (_localpaginator.SelectedItem.IsDisabled)
                            {
                                SetError(Messages.SelectionDisabled);
                            }
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_modeView == ModeView.Select && keyinfo.IsPressPageDownKey())
                    {
                        if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                        {
                            if (_localpaginator.SelectedItem != null)
                            {
                                if (_localpaginator.SelectedItem.IsDisabled)
                                {
                                    SetError(Messages.SelectionDisabled);
                                }
                            }
                            _indexTooptip = 0;
                            break;
                        }
                    }
                    else if (_modeView == ModeView.Select && keyinfo.IsPressPageUpKey())
                    {
                        if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                        {
                            if (_localpaginator.SelectedItem != null)
                            {
                                if (_localpaginator.SelectedItem.IsDisabled)
                                {
                                    SetError(Messages.SelectionDisabled);
                                }
                            }
                            _indexTooptip = 0;
                            break;
                        }
                    }
                    else if (_modeView == ModeView.Select && ConfigPlus.HotKeyToggleFullPath.Equals(keyinfo))
                    {
                        _indexTooptip = 0;
                        _showFolderInfoFullPath = !_showFolderInfoFullPath;
                        break;
                    }
                    else if (_modeView == ModeView.Select && _localpaginator!.SelectedItem != null && !IsRoot(_localpaginator.SelectedItem) && _localpaginator.SelectedItem.Value.IsFolder && "+-".Contains(keyinfo.KeyChar) && keyinfo.Modifiers == ConsoleModifiers.None)
                    {
                        if (keyinfo.KeyChar == '+')
                        {
                            if (_localpaginator.SelectedItem.IsExpanded)
                            {
                                continue;
                            }
                            _indexTooptip = 0;
                            _localpaginator.SelectedItem.IsExpanded = true;
                            _localpaginator.SelectedItem.Status = NodeStatus.Loading;
                            (string, bool, List<ItemNodeControl<ItemFile>>) newitems = EnqueueNewitems(_localpaginator.SelectedItem.UniqueId,
                                _localpaginator.SelectedItem.Level,
                                _localpaginator.SelectedItem.Value.FullPath);
                            _resultTask.Enqueue(newitems);
                            break;
                        }
                        if (keyinfo.KeyChar == '-')
                        {
                            if (!_localpaginator.SelectedItem.IsExpanded)
                            {
                                continue;
                            }
                            _indexTooptip = 0;
                            _localpaginator.SelectedItem.IsExpanded = false;
                            _localpaginator.SelectedItem.Status = NodeStatus.Unloading;
                            _resultTask.Enqueue((_localpaginator.SelectedItem.UniqueId, false, []));
                            break;
                        }
                        continue;
                    }
                }
            }
            finally
            {
                ConsolePlus.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        private void InitViewFiles()
        {
            _items.Clear();
            LoadRoot();

            _localpaginator = new Paginator<ItemNodeControl<ItemFile>>(
                FilterMode.Disabled,
                _items,
                _pageSize,
                Optional<ItemNodeControl<ItemFile>>.Empty(),
                (item1, item2) => item1.UniqueId == item2.UniqueId);

            if (_localpaginator.SelectedItem == null)
            {
                _localpaginator.FirstItem();
            }
            if (_localpaginator.SelectedItem!.IsDisabled)
            {
                SetError(Messages.SelectionDisabled);
            }
        }

        private void UnloadTaskResult(string key, CancellationToken cancellationToken)
        {
            int index = _items.FindIndex(x => x.UniqueId == key);
            if (index == -1)
            {
                return;
            }
            if (!_items[index].Value.IsFolder)
            {
                throw new InvalidOperationException("Internal error");
            }
            int posindex = index + 1;
            while (posindex < _items.Count)
            {
                if (_items[posindex].ParentUniqueId != key || cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                if (_items[posindex].Value.IsFolder && _items[posindex].Status == NodeStatus.Done)
                {
                    UnloadTaskResult(_items[posindex].UniqueId, cancellationToken);
                }
                _items.RemoveAt(posindex);
                continue;
            }
            _indexTooptip = 0;
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            _localpaginator!.UpdatColletion(_items, Optional<ItemNodeControl<ItemFile>>.Set(_items[index]));
            _items[index].Status = NodeStatus.NotLoad;
        }

        private void LoadTaskResult(string key, List<ItemNodeControl<ItemFile>> Items, CancellationToken cancellationToken)
        {
            int index = _items.FindIndex(x => x.UniqueId == key);
            if (index == -1)
            {
                return;
            }
            if (!_items[index].Value.IsFolder)
            {
                throw new InvalidOperationException("Internal error");
            }
            if (_items[index].Status != NodeStatus.Loading)
            {
                return;
            }
            int posindex = index;
            foreach (ItemNodeControl<ItemFile> item in Items)
            {
                if (cancellationToken.IsCancellationRequested || _items[index].Status != NodeStatus.Loading)
                {
                    break;
                }
                posindex++;
                _items.Insert(posindex, item);

            }
            if (cancellationToken.IsCancellationRequested || _items[index].Status != NodeStatus.Loading)
            {
                return;
            }
            _indexTooptip = 0;
            Optional<ItemNodeControl<ItemFile>> defaultvalue = Optional<ItemNodeControl<ItemFile>>.Set(_items[index]);
            _localpaginator!.UpdatColletion(_items, defaultvalue);
            _items[index].Status = NodeStatus.Done;
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = string.Empty;
            if (!ResultCtrl!.Value.IsAborted && _localpaginator!.SelectedItem is not null)
            {
                answer = _localpaginator!.SelectedItem.Value.Name;
            }
            if (ResultCtrl!.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[FileStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[FileStyles.Answer]);

            _showFolderInfoFullPath = true;
            WriteFolderInfo(screenBuffer);

            return true;
        }

        public override void FinalizeControl()
        {
            //none
        }

        private void WriteBufferFilter(BufferScreen screenBuffer)
        {
            WritePromptFilter(screenBuffer);

            WriteAnswerFilter(screenBuffer);

            WriteDescription(screenBuffer);

            WriteTooltip(screenBuffer);
        }

        private void WritePromptFilter(BufferScreen screenBuffer)
        {
            screenBuffer.Write($"{Messages.SearchFilter} ", _optStyles[FileStyles.Prompt]);
        }

        private void WriteAnswerFilter(BufferScreen screenBuffer)
        {
            Style styleAnswer = _optStyles[FileStyles.Answer];

            if (_filterBuffer!.IsVirtualBuffer)
            {
                string str = _filterBuffer!.IsHideLeftBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeftMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft);
                screenBuffer.Write(str, styleAnswer);
            }
            screenBuffer.Write(_filterBuffer!.ToBackward(), styleAnswer);
            screenBuffer.SavePromptCursor();
            if (_filterBuffer!.IsVirtualBuffer)
            {
                screenBuffer.Write(_filterBuffer!.ToForward(), styleAnswer);
                string str = _filterBuffer.IsHideRightBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterRightMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight);
                screenBuffer.WriteLine(str, styleAnswer);
            }
            else
            {
                screenBuffer.WriteLine(_filterBuffer!.ToForward(), styleAnswer);
            }
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[FileStyles.Prompt]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            screenBuffer.WriteLine(GetAnswerText(), _optStyles[FileStyles.Answer]);
            screenBuffer.SavePromptCursor();
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = GeneralOptions.DescriptionValue;
            if (_modeView == ModeView.Filter)
            {
                desc = string.Empty;
            }
            string filter = string.Empty;
            if (_searchPattern != "*" && _searchPattern != "*.*")
            {
                filter = _searchPattern;
            }
            if (!string.IsNullOrEmpty(filter))
            {

                if (string.IsNullOrEmpty(desc))
                {
                    desc = $"{Messages.Filtered} : {filter}";
                }
                else
                {
                    desc = $"{Messages.Filtered} : {filter}, {desc}";
                }
            }
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[FileStyles.Description]);
            }
        }

        private void WriteError(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[FileStyles.Error]);
                ClearError();
                return;
            }
        }

        private void WriteFolderInfo(BufferScreen screenBuffer)
        {

            string info = string.Empty;
            if (!string.IsNullOrEmpty(_localpaginator!.SelectedItem.ParentUniqueId))
            {
                ItemNodeControl<ItemFile> aux = _items.Find(x => x.UniqueId == _localpaginator!.SelectedItem.ParentUniqueId)!;
                if (_showFolderInfoFullPath)
                {
                    info = string.Format(Messages.FileInfoFolder, aux.Value.FullPath);
                }
                else
                {
                    info = string.Format(Messages.FileInfoFolder, aux.Value.Name);
                }
            }
            else
            {
                if (_showFolderInfoFullPath)
                {
                    string aux = _localpaginator!.SelectedItem.Value.FullPath;
                    int index = aux.LastIndexOf(_localpaginator!.SelectedItem.Value.Name, StringComparison.Ordinal);
                    aux = aux[..index];
                    if (aux.Length > 0)
                    {
                        info = $"{Messages.FileInfoRoot}({aux})";
                    }
                    else
                    {
                        info = $"{Messages.FileInfoRoot}";
                    }
                }
                else
                {
                    info = Messages.FileInfoRoot;
                }
            }
            if (!string.IsNullOrEmpty(info))
            {
                screenBuffer.WriteLine(info, _optStyles[FileStyles.Answer]);
            }
        }

        private void WriteListSelect(BufferScreen screenBuffer)
        {
            ArraySegment<ItemNodeControl<ItemFile>> subset = _localpaginator!.GetPageData();
            foreach (ItemNodeControl<ItemFile> item in subset)
            {
                bool checkroot = IsRoot(item);
                ItemFile value = item.Value;
                if (_localpaginator.TryGetSelected(out ItemNodeControl<ItemFile>? selectedItem) && EqualityComparer<ItemNodeControl<ItemFile>>.Default.Equals(item, selectedItem))
                {
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", _optStyles[FileStyles.Selected]);
                    screenBuffer.Write(CreateIndentation(item), _optStyles[FileStyles.Lines]);
                    if (item.Value.IsFolder)
                    {
                        if (!checkroot)
                        {
                            if (item.IsExpanded)
                            {
                                screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Expanded), _optStyles[FileStyles.Selected]);
                            }
                            else
                            {
                                screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Collapsed), _optStyles[FileStyles.Selected]);
                            }
                        }
                    }
                    screenBuffer.Write(value.Name, _optStyles[FileStyles.Selected]);
                    if (!_hideSize && !checkroot)
                    {
                        if (value.IsFolder)
                        {
                            screenBuffer.Write($"({value.Length})", _optStyles[FileStyles.Selected]);
                        }
                        else
                        {
                            screenBuffer.Write($"({BytesToString(value.Length)})", _optStyles[FileStyles.Selected]);
                        }
                    }
                }
                else
                {
                    Style stl = _optStyles[FileStyles.UnSelected];
                    if (item.IsDisabled && !checkroot)
                    {
                        stl = _optStyles[FileStyles.Disabled];
                    }
                    screenBuffer.Write(" ", stl);
                    screenBuffer.Write(CreateIndentation(item), _optStyles[FileStyles.Lines]);
                    if (checkroot)
                    {
                        screenBuffer.Write(value.Name, _optStyles[FileStyles.FileRoot]);
                    }
                    else if (value.IsFolder)
                    {
                        if (item.IsExpanded)
                        {
                            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Expanded), _optStyles[FileStyles.ExpandSymbol]);
                        }
                        else
                        {
                            screenBuffer.Write(ConfigPlus.GetSymbol(SymbolType.Collapsed), _optStyles[FileStyles.ExpandSymbol]);
                        }
                        Style stlf = _optStyles[FileStyles.FileTypeFolder];
                        if (item.IsDisabled && !checkroot)
                        {
                            stlf = _optStyles[FileStyles.Disabled];
                        }
                        screenBuffer.Write(value.Name, stlf);
                    }
                    else
                    {
                        Style stlf = _optStyles[FileStyles.FileTypeFile];
                        if (item.IsDisabled && !checkroot)
                        {
                            stlf = _optStyles[FileStyles.Disabled];
                        }
                        screenBuffer.Write(value.Name, stlf);
                    }
                    if (!_hideSize && !checkroot)
                    {
                        Style stlz = _optStyles[FileStyles.FileSize];
                        if (value.IsFolder)
                        {
                            screenBuffer.Write($"({value.Length})", stlz);
                        }
                        else
                        {
                            screenBuffer.Write($"({BytesToString(value.Length)})", stlz);
                        }
                    }
                }
                screenBuffer.WriteLine("", Style.Default());
            }
            if (_localpaginator.PageCount > 1)
            {
                string template = ConfigPlus.PaginationTemplate.Invoke(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[FileStyles.Pagination]);
            }
        }

        private static string BytesToString(long value)
        {
            string[] suf = ["", " KB", " MB", " GB", " TB", " PB", " EB"]; //Longs run out around EB
            if (value == 0)
            {
                return "0";
            }
            int place = Convert.ToInt32(Math.Floor(Math.Log(value, 1024)));
            double num = Math.Round(value / Math.Pow(1024, place), 0);
            return $"{num}{suf[place]}";
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string tooltip = string.Empty;
            if (_indexTooptip > 0)
            {
                tooltip = GetTooltipToggle();
            }
            else
            {
                if (_modeView == ModeView.Select)
                {
                    tooltip = _tooltipModeSelect;
                }
                else if (_modeView == ModeView.Filter)
                {
                    tooltip = _tooltipModeFilter;
                }
            }
            if (!string.IsNullOrEmpty(tooltip))
            {
                screenBuffer.Write(tooltip, _optStyles[FileStyles.Tooltips]);
            }
        }

        private string GetTooltipToggle()
        {
            return _modeView switch
            {
                ModeView.Select => _toggerTooptips[ModeView.Select][_indexTooptip - 1],
                ModeView.Filter => _toggerTooptips[ModeView.Filter][_indexTooptip - 1],
                _ => throw new NotImplementedException($"ModeView {_modeView} not implemented.")
            };
        }

        private string CreateIndentation(ItemNodeControl<ItemFile> item)
        {
            StringBuilder result = new();
            if (item.Level == 0)
            {
                return string.Empty;
            }
            string? parent = item.ParentUniqueId;
            var aux = new Stack<string>();
            for (int i = 0; i < item.Level - 1; i++)
            {
                string syb = ConfigPlus.GetSymbol(SymbolType.TreeLinevertical);
                if (!string.IsNullOrEmpty(parent))
                {
                    int index = _items.FindIndex(x => x.UniqueId == parent);
                    if (_items[index].LastItem)
                    {
                        syb = new string(' ', ConfigPlus.GetSymbol(SymbolType.TreeLinevertical).Length);
                    }
                    parent = _items[index].ParentUniqueId;
                }
                aux.Push(syb);
            }
            while (aux.TryPop(out string? indentation))
            {
                result.Append(indentation);
            }
            if (item.FirstItem && !item.LastItem)
            {
                result.Append(ConfigPlus.GetSymbol(SymbolType.TreeLinecross));
            }
            else if (item.FirstItem && item.LastItem)
            {
                result.Append(ConfigPlus.GetSymbol(SymbolType.TreeLinecorner));
            }
            else if (!item.FirstItem && !item.LastItem)
            {
                result.Append(ConfigPlus.GetSymbol(SymbolType.TreeLinecross));
            }
            else if (!item.FirstItem && item.LastItem)
            {
                result.Append(ConfigPlus.GetSymbol(SymbolType.TreeLinecorner));
            }
            return result.ToString();
        }

        private ItemFile? CreateItemFile(bool isFile, string name)
        {
            if (isFile)
            {
                FileInfo fi = new(name);
                if (fi.Length < _minvalueSize || fi.Length > _maxvalueSize)
                {
                    return null;
                }
                return new()
                {
                    FullPath = fi.FullName,
                    IsFolder = false,
                    Name = fi.Name,
                    Length = fi.Length
                };
            }
            else
            {
                DirectoryInfo di = new(name);
                string filter = "*";
                if (_onlyFolders)
                {
                    filter = _searchPattern;
                }
                int qtd = di.GetDirectories(filter, GetEnumerationOptions()).Length;
                if (!_onlyFolders)
                {
                    qtd += di.GetFiles(_searchPattern, GetEnumerationOptions()).Length;
                }
                if (_hideZeroEntries && qtd == 0)
                {
                    return null;
                }
                if (_hideSize)
                {
                    qtd = 0;
                }
                return new()
                {
                    FullPath = di.FullName,
                    IsFolder = true,
                    Name = di.Name,
                    Length = qtd
                };
            }
        }

        private EnumerationOptions GetEnumerationOptions()
        {
            return new EnumerationOptions
            {
                IgnoreInaccessible = true,
                MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = false,
                ReturnSpecialDirectories = false,
                AttributesToSkip = GetSkipFiles()
            };
        }

        private void LoadRoot()
        {
            string filter = "*";
            if (_onlyFolders)
            {
                filter = _searchPattern;
            }
            List<(bool IsFile, string Name)> entries = [.. Directory.GetDirectories(_root, filter, GetEnumerationOptions()).Select(x => (IsFile: false, Name: x))];

            if (!_onlyFolders)
            {
                entries.AddRange(Directory.GetFiles(_root, _searchPattern, GetEnumerationOptions())
                        .Select(x => (IsFile: true, Name: x)));
            }

            DirectoryInfo rootdi = new(_root);
            ItemFile rootitem = new()
            {
                FullPath = rootdi.FullName,
                IsFolder = true,
                Name = rootdi.Name,
                Length = entries.Count
            };
            _items.Add(new ItemNodeControl<ItemFile>(Guid.NewGuid().ToString())
            {
                IsExpanded = true,
                Status = NodeStatus.Done,
                IsMarked = false,
                IsDisabled = _predicatevaliddisabled?.Invoke(rootitem) ?? false,
                Level = 0,
                ParentUniqueId = null,
                Value = rootitem
            });
            int pos = -1;
            foreach ((bool IsFile, string? Name) in entries.OrderBy(x => x.Name))
            {
                ItemFile? newitem = CreateItemFile(IsFile, Name);
                if (newitem is null)
                {
                    continue;
                }
                pos++;
                bool first = false;
                bool last = false;
                if (pos == 0)
                {
                    first = true;
                }
                if (pos == entries.Count - 1)
                {
                    last = true;
                }
                _items.Add(new ItemNodeControl<ItemFile>(Guid.NewGuid().ToString())
                {
                    IsExpanded = false,
                    Status = newitem.IsFolder ? NodeStatus.NotLoad : NodeStatus.Done,
                    FirstItem = first,
                    LastItem = last,
                    IsMarked = false,
                    IsDisabled = _predicatevaliddisabled?.Invoke(newitem) ?? false,
                    Level = 1,
                    ParentUniqueId = _items[0].UniqueId,
                    Value = newitem
                });
            }
        }

        private (string, bool, List<ItemNodeControl<ItemFile>>) EnqueueNewitems(string parentid, int level, string fullpath)
        {
            string filter = "*";
            if (_onlyFolders)
            {
                filter = _searchPattern;
            }
            List<(bool IsFile, string Name)> entries = [.. Directory.GetDirectories(fullpath, filter, GetEnumerationOptions()).Select(x => (IsFile: false, Name: x))];
            if (!_onlyFolders)
            {
                entries.AddRange(Directory.GetFiles(fullpath, _searchPattern, GetEnumerationOptions())
                        .Select(x => (IsFile: true, Name: x)));
            }
            int pos = -1;
            List<ItemNodeControl<ItemFile>> newitems = [];
            int newlevel = level + 1;
            foreach ((bool IsFile, string? Name) in entries.OrderBy(x => x.Name))
            {
                ItemFile? newitem = CreateItemFile(IsFile, Name);
                if (newitem == null)
                {
                    continue;
                }
                pos++;
                bool first = false;
                bool last = false;
                if (pos == 0)
                {
                    first = true;
                }
                if (pos == entries.Count - 1)
                {
                    last = true;
                }
                newitems.Add(new ItemNodeControl<ItemFile>(Guid.NewGuid().ToString())
                {
                    IsExpanded = false,
                    Status = newitem.IsFolder ? NodeStatus.NotLoad : NodeStatus.Done,
                    FirstItem = first,
                    LastItem = last,
                    IsMarked = false,
                    IsDisabled = _predicatevaliddisabled?.Invoke(newitem) ?? false,
                    Level = newlevel,
                    ParentUniqueId = parentid,
                    Value = newitem
                });
            }
            return (parentid, true, newitems);
        }

        private FileAttributes GetSkipFiles()
        {
            FileAttributes skipfiles = FileAttributes.Temporary;
            if (!_acceptHiddenAttributes)
            {
                skipfiles |= FileAttributes.Hidden;
            }
            if (!_acceptSystemAttributes)
            {
                skipfiles |= FileAttributes.System;
            }
            return skipfiles;
        }

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips =
                [
                    $"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}"
                ];
                if (GeneralOptions.EnabledAbortKeyValue)
                {
                    lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
                }
                if (_filterType != FilterMode.Disabled)
                {
                    lsttooltips.Add(string.Format(Messages.TooltipFilterMode, ConfigPlus.HotKeyFilterMode));
                }
                if (mode == ModeView.Select)
                {
                    lsttooltips.Add(Messages.TooltipPages);
                }
                if (mode == ModeView.Filter)
                {
                    lsttooltips.AddRange(EmacsBuffer.GetEmacsTooltips());
                }
                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

        private string GetAnswerText()
        {
            if (_localpaginator!.SelectedIndex < 0)
            {
                return string.Empty;
            }
            if (_localpaginator!.SelectedItem.Status == NodeStatus.Loading)
            {
                return $"{_localpaginator!.SelectedItem.Value.Name}({Messages.Loading})";
            }
            if (_localpaginator!.SelectedItem.Status == NodeStatus.Unloading)
            {
                return $"{_localpaginator!.SelectedItem.Value.Name}({Messages.Unloading})";
            }
            return _localpaginator!.SelectedItem.Value.Name;
        }

        private string GetTooltipModeSelect()
        {
            StringBuilder tooltip = new();
            tooltip.Append($"{string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip)}, {Messages.InputFinishEnter}");
            tooltip.Append(", ");
            tooltip.Append(Messages.TooltipToggleExpandPress);
            return tooltip.ToString();
        }

        private string GetTooltipModeFilter()
        {
            StringBuilder tooltip = new();

            tooltip.Append($"{string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip)}, {Messages.InputFinishFilter}");

            return tooltip.ToString();
        }

        private ConsoleKeyInfo WaitKeypressDiscovery(CancellationToken token)
        {
            while (!ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                if (!_resultTask.IsEmpty && _modeView == ModeView.Select)
                {
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, true, false);
                }
                token.WaitHandle.WaitOne(2);
            }
            if (ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                return ConsolePlus.ReadKey(true);
            }
            return new ConsoleKeyInfo();
        }

    }
}
