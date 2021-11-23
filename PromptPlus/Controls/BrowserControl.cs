// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;

using PPlus.Internal;

using PPlus.Objects;
using PPlus.Resources;

namespace PPlus.Controls
{
    internal class BrowserControl : ControlBase<ResultBrowser>, IControlBrowser
    {
        private ResultBrowser _defaultopt;
        private string _currentPath;
        private readonly BrowserOptions _options;
        private readonly InputBuffer _filterBuffer = new();
        private Paginator<ResultBrowser> _paginator;
        private readonly Func<ResultBrowser, string> _aliasSelector = x => x.AliasSelected;
        private readonly Func<ResultBrowser, string> _textSelector = x => x.SelectedValue;
        private readonly Func<ResultBrowser, string> _fullPathSelector = x => Path.Combine(x.PathValue, x.SelectedValue);

        public BrowserControl(BrowserOptions options) : base(options, true)
        {
            _options = options;
        }

        public override void InitControl()
        {
            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;

            switch (_options.Filter)
            {
                case BrowserFilter.None:
                    InitFilebrowser();
                    break;
                case BrowserFilter.OnlyFolder:
                    InitFolderbrowser();
                    break;
                default:
                    throw new NotImplementedException(string.Format(Exceptions.Ex_FileBrowserNotImplemented, _options.Filter));
            }

            Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
            Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;

        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ResultBrowser result)
        {
            screenBuffer.WriteDone(_options.Message);
            FinishResult = _fullPathSelector(result);
            screenBuffer.WriteAnswer(FinishResult);
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            var prompt = $"{ _options.Message}";
            if (_options.SearchPattern != "*" && _options.ShowSearchPattern)
            {
                prompt += $" ({_options.SearchPattern})";
            }
            screenBuffer.WritePrompt(prompt);

            if (_filterBuffer.Length > 0 && _paginator.IsUnSelected)
            {
                screenBuffer.WriteAnswer(_filterBuffer.ToBackward());
            }
            else
            {
                if (_paginator.TryGetSelectedItem(out var result))
                {
                    screenBuffer.WriteAnswer(_textSelector(result));
                }
            }

            screenBuffer.PushCursor();

            if (_filterBuffer.Length > 0 && _paginator.IsUnSelected)
            {
                screenBuffer.WriteAnswer(_filterBuffer.ToForward());
            }

            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
            }
            if (_options.ShowNavigationCurrentPath)
            {
                screenBuffer.WriteLineDescription($"{Messages.FolderCurrentPath} {_currentPath}");
            }

            if (EnabledStandardTooltip)
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
                if (_options.EnabledPromptTooltip)
                {
                    screenBuffer.WriteLine();
                    if (_paginator.PageCount > 1)
                    {
                        screenBuffer.WriteHint(Messages.KeyNavPaging);
                    }
                    screenBuffer.WriteHint(Messages.FolderKeyNavigation);
                }
            }

            if (_filterBuffer.Length > 0)
            {
                screenBuffer.WriteLineFilter(Messages.ItemsFiltered);
                screenBuffer.WriteFilter($" ({_filterBuffer})");
            }

            var subset = _paginator.ToSubset();

            foreach (var item in subset)
            {
                var value = _aliasSelector(item);
                if (_paginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<ResultBrowser>.Default.Equals(item, selectedItem))
                {
                    screenBuffer.WriteLineFileBrowserSelected(value);
                }
                else
                {
                    screenBuffer.WriteLineFileBrowser(value);
                }
            }
            if (_paginator.PageCount > 1)
            {
                screenBuffer.WriteLinePagination(_paginator.PaginationMessage());
            }
        }

        public override bool? TryResult(bool summary, CancellationToken cancellationToken, out ResultBrowser result)
        {
            bool? isvalidhit = false;
            if (summary)
            {
                result = default;
                return false;
            }
            do
            {
                var keyInfo = WaitKeypress(cancellationToken);

                if (CheckDefaultKey(keyInfo))
                {
                    continue;
                }
                else if (IskeyPageNavagator(keyInfo, _paginator))
                {
                    continue;
                }

                if (PromptPlus.UnSelectFilter.Equals(keyInfo))
                {
                    _paginator.UnSelected();
                    result = default;
                    return isvalidhit;
                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0 && _options.Filter == BrowserFilter.None:
                    {
                        if (_paginator.TryGetSelectedItem(out var resultpreview))
                        {
                            if (resultpreview.IsFile)
                            {
                                result = new ResultBrowser(_currentPath, resultpreview.SelectedValue, false, true, !_options.ShowNavigationCurrentPath);
                                return true;
                            }
                            else
                            {
                                SetError(Messages.FileNotSelected);
                            }
                            break;
                        }
                        var newfile = _filterBuffer.ToString();
                        if (string.IsNullOrEmpty(newfile) && !_options.AllowNotSelected)
                        {
                            SetError(Messages.Required);
                            break;
                        }
                        if (!string.IsNullOrEmpty(_options.PrefixExtension))
                        {
                            if (!newfile.ToLower().EndsWith(_options.PrefixExtension))
                            {
                                newfile += _options.PrefixExtension;
                            }
                        }
                        result = new ResultBrowser(_currentPath, newfile, true, true, !_options.ShowNavigationCurrentPath);
                        return true;
                    }
                    case ConsoleKey.Enter when keyInfo.Modifiers == 0 && _options.Filter == BrowserFilter.OnlyFolder:
                    {
                        if (_paginator.TryGetSelectedItem(out var resultpreview))
                        {
                            result = new ResultBrowser(_currentPath, resultpreview.SelectedValue, false, false, !_options.ShowNavigationCurrentPath);
                            return true;
                        }
                        var newfolder = _filterBuffer.ToString();
                        if (string.IsNullOrEmpty(newfolder) && !_options.AllowNotSelected)
                        {
                            SetError(Messages.Required);
                            break;
                        }
                        if (!string.IsNullOrEmpty(_options.PrefixExtension))
                        {
                            if (!newfolder.ToLower().EndsWith(_options.PrefixExtension))
                            {
                                newfolder += _options.PrefixExtension;
                            }
                        }
                        result = new ResultBrowser(_currentPath, newfolder, true, false, !_options.ShowNavigationCurrentPath);
                        return true;
                    }
                    case ConsoleKey.UpArrow when keyInfo.Modifiers == 0 && ((_paginator.Count == 1 && _paginator.IsUnSelected) || _paginator.Count > 1):
                        if (_paginator.IsFistPageItem)
                        {
                            _paginator.PreviousPage(IndexOption.LastItem);
                        }
                        else
                        {
                            _paginator.PreviousItem();
                        }
                        break;
                    case ConsoleKey.DownArrow when keyInfo.Modifiers == 0 && ((_paginator.Count == 1 && _paginator.IsUnSelected) || _paginator.Count > 1):
                        if (_paginator.IsLastPageItem)
                        {
                            _paginator.NextPage(IndexOption.FirstItem);
                        }
                        else
                        {
                            _paginator.NextItem();
                        }
                        break;
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == 0 && !_filterBuffer.IsStart:
                        _paginator.UpdateFilter(_filterBuffer.Backward().ToString());
                        break;
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == 0 && !_filterBuffer.IsEnd:
                        _paginator.UpdateFilter(_filterBuffer.Forward().ToString());
                        break;
                    case ConsoleKey.Backspace when keyInfo.Modifiers == 0 && !_filterBuffer.IsStart:
                        _paginator.UpdateFilter(_filterBuffer.Backspace().ToString());
                        break;
                    case ConsoleKey.Delete when keyInfo.Modifiers == 0 && !_filterBuffer.IsEnd:
                        _paginator.UpdateFilter(_filterBuffer.Delete().ToString());
                        break;
                    case ConsoleKey.LeftArrow when keyInfo.Modifiers == ConsoleModifiers.Control:
                    {
                        if (_currentPath == _options.RootFolder)
                        {
                            isvalidhit = null;
                            break;
                        }
                        var di = new DirectoryInfo(_currentPath);
                        if (di.Parent == null)
                        {
                            isvalidhit = null;
                            break;
                        }
                        _currentPath = di.Parent.FullName;
                        _paginator = new Paginator<ResultBrowser>(ItensFolders(di.Parent.FullName), _options.PageSize, Optional<ResultBrowser>.Create(_defaultopt), _aliasSelector);
                        _paginator.FirstItem();
                        break;
                    }
                    case ConsoleKey.RightArrow when keyInfo.Modifiers == ConsoleModifiers.Control:
                    {
                        if (_paginator.TryGetSelectedItem(out var resultpreview))
                        {
                            var dryfolder = false;
                            if (!resultpreview.IsFile)
                            {
                                dryfolder = IsDirectoryHasChilds(Path.Combine(_currentPath, resultpreview.SelectedValue));
                            }
                            if (_options.Filter == BrowserFilter.None && !resultpreview.IsFile && !dryfolder)
                            {
                                dryfolder = IsDirectoryHasFiles(Path.Combine(_currentPath, resultpreview.SelectedValue));
                            }
                            if (dryfolder)
                            {
                                _currentPath = Path.Combine(_currentPath, resultpreview.SelectedValue);
                                _paginator = new Paginator<ResultBrowser>(ItensFolders(_currentPath), _options.PageSize, Optional<ResultBrowser>.Create(_defaultopt), _aliasSelector);
                                _paginator.FirstItem();
                            }
                            else
                            {
                                isvalidhit = null;
                            }
                        }
                        break;
                    }
                    default:
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            if (!char.IsControl(keyInfo.KeyChar))
                            {
                                _paginator.UpdateFilter(_filterBuffer.Insert(keyInfo.KeyChar).ToString());
                            }
                            else
                            {
                                isvalidhit = null;
                            }
                        }
                        break;
                    }
                }

            } while (KeyAvailable && !cancellationToken.IsCancellationRequested);

            result = default;

            return isvalidhit;
        }

        private void InitFilebrowser()
        {
            var defvalue = _options.DefaultValue;
            if (!string.IsNullOrEmpty(defvalue) && !IsValidFile(defvalue))
            {
                throw new ArgumentException(Exceptions.Ex_FileBrowserDefaultValueArgument);
            }
            if (!string.IsNullOrEmpty(_options.RootFolder) && !IsValidDirectory(_options.RootFolder))
            {
                throw new ArgumentException(Exceptions.Ex_FileBrowserRootValueArgument);
            }
            if (!string.IsNullOrEmpty(defvalue))
            {
                var fi = new FileInfo(defvalue);
                _defaultopt = new ResultBrowser(fi.Directory?.FullName ?? "", fi.Name, false, true, !_options.ShowNavigationCurrentPath);
                _currentPath = fi.Directory.FullName;
            }
            else
            {
                var di = new DirectoryInfo(Directory.GetCurrentDirectory());
                _defaultopt = new ResultBrowser(di.FullName ?? "", "", true, true, !_options.ShowNavigationCurrentPath);
                _currentPath = di.FullName;
            }
            _paginator = new Paginator<ResultBrowser>(ItensFolders(_defaultopt.PathValue), _options.PageSize, Optional<ResultBrowser>.Create(_defaultopt), _aliasSelector);
            if (_paginator.IsUnSelected)
            {
                _paginator.FirstItem();
            }
        }

        private void InitFolderbrowser()
        {
            var defvalue = _options.DefaultValue;
            if (string.IsNullOrEmpty(defvalue))
            {
                defvalue = Directory.GetCurrentDirectory();
            }
            if (!Directory.GetDirectoryRoot(defvalue).Equals(defvalue, StringComparison.InvariantCultureIgnoreCase))
            {
                if (!IsValidDirectory(defvalue))
                {
                    throw new ArgumentException(Exceptions.Ex_FileBrowserDefaultValueArgument);
                }
            }
            if (!string.IsNullOrEmpty(_options.RootFolder) && !IsValidDirectory(_options.RootFolder))
            {
                throw new ArgumentException(Exceptions.Ex_FileBrowserRootValueArgument);
            }
            var di = new DirectoryInfo(defvalue);
            _defaultopt = new ResultBrowser(di.Parent?.FullName ?? "", di.Name, false, false, !_options.ShowNavigationCurrentPath);
            _currentPath = defvalue;
            _paginator = new Paginator<ResultBrowser>(ItensFolders(defvalue), _options.PageSize, Optional<ResultBrowser>.Create(_defaultopt), _aliasSelector);
            if (_paginator.IsUnSelected)
            {
                _paginator.FirstItem();
            }
        }

        private IEnumerable<ResultBrowser> ItensFolders(string folder)
        {
            if (_options.Filter == BrowserFilter.OnlyFolder)
            {
                return LoadFolderItems(folder);
            }
            return LoadFileItems(folder);
        }

        private IEnumerable<ResultBrowser> LoadFileItems(string folder)
        {
            var result = new List<ResultBrowser>();
            foreach (var item in Directory.GetFiles(folder, _options.SearchPattern))
            {
                if (IsValidFile(item))
                {
                    var fi = new FileInfo(item);
                    result.Add(new ResultBrowser(fi.Directory?.FullName ?? "", fi.Name, false, true, !_options.ShowNavigationCurrentPath));
                }
            }
            foreach (var item in Directory.GetDirectories(folder))
            {
                if (IsValidDirectory(item))
                {
                    var di = new DirectoryInfo(item);
                    result.Add(new ResultBrowser(di.Parent?.FullName ?? "", di.Name, false, false, !_options.ShowNavigationCurrentPath));
                }
            }
            return result.OrderBy(x => x.SelectedValue).ToArray();
        }

        private IEnumerable<ResultBrowser> LoadFolderItems(string folder)
        {
            var result = new List<ResultBrowser>();
            foreach (var item in Directory.GetDirectories(folder, _options.SearchPattern))
            {
                if (IsValidDirectory(item))
                {
                    var di = new DirectoryInfo(item);
                    result.Add(new ResultBrowser(di.Parent?.FullName ?? "", di.Name, false, false, !_options.ShowNavigationCurrentPath));
                }
            }
            return result.OrderBy(x => x.SelectedValue).ToArray();
        }

        private bool IsDirectoryHasChilds(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return false;
            }
            try
            {
                return Directory.GetDirectories(folder, _options.SearchPattern).Where(x => IsValidDirectory(x)).Any();
            }
            catch (SecurityException)
            {
                //done - skip
            }
            catch (UnauthorizedAccessException)
            {
                //done - skip
            }
            return false;
        }

        private bool IsDirectoryHasFiles(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return false;
            }
            try
            {
                return Directory.GetFiles(folder, _options.SearchPattern).Where(x => IsValidFile(x)).Any();
            }
            catch (SecurityException)
            {
                //done - skip
            }
            catch (UnauthorizedAccessException)
            {
                //done - skip
            }
            return false;
        }

        private bool IsValidFile(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return false;
            }
            try
            {
                var fi = new FileInfo(file);
                if (fi.Attributes.HasFlag(FileAttributes.System))
                {
                    return false;
                }
                if (!IsValidDirectory(fi.Directory.FullName))
                {
                    return false;
                }
                if (_options.SupressHidden && fi.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    return false;
                }
                return true;
            }
            catch (SecurityException)
            {
                //done - skip
            }
            catch (UnauthorizedAccessException)
            {
                //done - skip
            }
            return false;
        }

        private bool IsValidDirectory(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return false;
            }
            try
            {
                var di = new DirectoryInfo(folder);
                if (di.Name.StartsWith("$"))
                {
                    return false;
                }
                if (di.Attributes.HasFlag(FileAttributes.System))
                {
                    return false;
                }
                if (_options.SupressHidden && di.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    return false;
                }
                return true;
            }
            catch (SecurityException)
            {
                //done - skip
            }
            catch (UnauthorizedAccessException)
            {
                //done - skip
            }
            return false;
        }

        #region IControlBrowser

        public IControlBrowser Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlBrowser Filter(BrowserFilter value)
        {
            _options.Filter = value;
            return this;
        }

        public IControlBrowser Default(string value)
        {
            _options.DefaultValue = value;
            return this;
        }

        public IControlBrowser PrefixExtension(string value)
        {
            _options.PrefixExtension = value;
            return this;
        }

        public IControlBrowser AllowNotSelected(bool value)
        {
            _options.AllowNotSelected = value;
            return this;
        }

        public IControlBrowser Root(string value)
        {
            _options.RootFolder = value;
            return this;
        }

        public IControlBrowser SearchPattern(string value)
        {
            _options.SearchPattern = value;
            return this;
        }

        public IControlBrowser PageSize(int value)
        {
            if (value < 0)
            {
                _options.PageSize = null;
            }
            else
            {
                _options.PageSize = value;
            }
            return this;
        }

        public IControlBrowser SupressHidden(bool value)
        {
            _options.SupressHidden = value;
            return this;
        }

        public IControlBrowser PromptCurrentPath(bool value)
        {
            _options.ShowNavigationCurrentPath = value;
            return this;
        }

        public IControlBrowser promptSearchPattern(bool value)
        {
            _options.ShowSearchPattern = value;
            return this;
        }

        public IControlBrowser Config(Action<IPromptConfig> context)
        {
            context.Invoke(this);
            return this;
        }

        public IPromptConfig EnabledAbortKey(bool value)
        {
            _options.EnabledAbortKey = value;
            return this;
        }

        public IPromptConfig EnabledAbortAllPipes(bool value)
        {
            _options.EnabledAbortAllPipes = value;
            return this;
        }

        public IPromptConfig EnabledPromptTooltip(bool value)
        {
            _options.EnabledPromptTooltip = value;
            return this;
        }

        public IPromptConfig HideAfterFinish(bool value)
        {
            _options.HideAfterFinish = value;
            return this;
        }

        public ResultPromptPlus<ResultBrowser> Run(CancellationToken? value = null)
        {
            InitControl();
            try
            {
                return Start(value ?? CancellationToken.None);
            }
            finally
            {
                Dispose();
            }
        }

        public IPromptPipe PipeCondition(Func<ResultPipe[], object, bool> condition)
        {
            Condition = condition;
            return this;
        }

        public IFormPlusBase ToPipe(string id, string title, object state = null)
        {
            PipeId = id ?? Guid.NewGuid().ToString();
            PipeTitle = title ?? string.Empty;
            ContextState = state;
            return this;
        }

        #endregion

    }
}
