using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using PPlus.Internal;
using PPlus.Objects;

using static System.Environment;

namespace PPlus.Controls
{
    internal class ReadlineControl : ControlBase<string>, IControlReadline
    {
        private const string Folderhistory = "PromptPlus.Readline";
        private const string Filehistory = "{0}.txt";
        private const string Namecontrol = "PromptPlus.Readline";
        private readonly ReadlineOptions _options;
        private Paginator<ItemHistory> _localpaginator;
        private ReadLineBuffer _inputBuffer;
        private IList<ItemHistory> _itemsHistory = new List<ItemHistory>();
        private bool _showingHistory;
        private string _originalText;

        public ReadlineControl(ReadlineOptions options) : base(Namecontrol, options, true)
        {
            _options = options;
        }

        public override void InitControl()
        {
            Thread.CurrentThread.CurrentCulture = PromptPlus.DefaultCulture;
            Thread.CurrentThread.CurrentUICulture = PromptPlus.DefaultCulture;

            if (string.IsNullOrEmpty(_options.FileNameHistory?.Trim()??""))
            {
                _options.FileNameHistory = AppDomain.CurrentDomain.FriendlyName;
            }

            if (PromptPlus.EnabledLogControl)
            {
                AddLog("MinimumPrefixLength", _options.MinimumPrefixLength.ToString(), LogKind.Property);
                AddLog("FinishWhenHistoryEnter", _options.FinishWhenHistoryEnter.ToString(), LogKind.Property);
                AddLog("AcceptInputTab", _options.AcceptInputTab.ToString(), LogKind.Property);
                AddLog("EnabledHistory", _options.EnabledHistory.ToString(), LogKind.Property);
                AddLog("PrefixFileNameHistory", _options.FileNameHistory, LogKind.Property);
                AddLog("SuggestionHandler", (_options.SuggestionHandler != null).ToString(), LogKind.Property);
                AddLog("MaxHistory", _options.MaxHistory.ToString(), LogKind.Property);
                AddLog("PageSize", _options.PageSize.ToString(), LogKind.Property);
                AddLog("TimeoutHistory", _options.TimeoutHistory.ToString(), LogKind.Property);
                AddLog("Validators", _options.Validators.Count.ToString(), LogKind.Property);
            }

            _inputBuffer = new ReadLineBuffer(_options.SuggestionHandler, _options.AcceptInputTab);

            if (_options.EnabledHistory)
            {
                LoadHistory();
            }

            Thread.CurrentThread.CurrentCulture = AppcurrentCulture;
            Thread.CurrentThread.CurrentUICulture = AppcurrentUICulture;

        }

        private void AddHistory(string value)
        {
            var localnewhis = value.Trim();
            var found = _itemsHistory
                .Where(x => x.History.ToLowerInvariant() == localnewhis.ToLowerInvariant())
                .ToArray();
            if (found.Length > 0)
            {
                foreach (var item in found)
                {
                    _itemsHistory.Remove(item);
                }
            }
            if (_itemsHistory.Count >= byte.MaxValue)
            {
                _itemsHistory.RemoveAt(_itemsHistory.Count - 1);
            }
            _itemsHistory.Insert(0,
                ItemHistory.CreateItemHistory(localnewhis, _options.TimeoutHistory));

            var file = string.Format(Filehistory, _options.FileNameHistory);
            var userProfile = GetFolderPath(SpecialFolder.UserProfile);
            if (!Directory.Exists(Path.Combine(userProfile, Folderhistory)))
            {
                Directory.CreateDirectory(Path.Combine(userProfile, Folderhistory));
            }

            File.WriteAllLines(Path.Combine(userProfile, Folderhistory, file),
                _itemsHistory.Where(x => DateTime.Now < new DateTime(x.TimeOutTicks))
                    .Select(x => x.ToString()), Encoding.UTF8);
        }

        private void LoadHistory()
        {
            var file = string.Format(Filehistory, _options.FileNameHistory);
            var userProfile = GetFolderPath(SpecialFolder.UserProfile);
            var result = new List<ItemHistory>();
            if (File.Exists(Path.Combine(userProfile, Folderhistory, file)))
            {
                var aux = File.ReadAllLines(Path.Combine(userProfile, Folderhistory, file));
                foreach (var item in aux)
                {
                    var itemhist = item.Split(ItemHistory.Separator, StringSplitOptions.RemoveEmptyEntries);
                    if (itemhist.Length == 2)
                    {
                        if (long.TryParse(itemhist[1], out var dtTicks))
                        {
                            if (DateTime.Now < new DateTime(dtTicks))
                            {
                                result.Add(new ItemHistory(itemhist[0], dtTicks));
                            }
                        }
                    }
                }
            }
            _itemsHistory = result
                .OrderByDescending(x => x.TimeOutTicks)
                .ToList();
        }

        public override bool? TryResult(bool IsSummary, CancellationToken stoptoken, out string result)
        {
            bool? isvalidhit = false;
            if (IsSummary)
            {
                result = default;
                return false;
            }
            do
            {
                var keyInfo = WaitKeypress(stoptoken);
                _inputBuffer.TryAcceptedReadlineConsoleKey(keyInfo, out var acceptedkey);
                if (acceptedkey)
                {
                    _showingHistory = false;
                    _originalText = null;
                }
                else if (!_showingHistory && (keyInfo.IsPressDownArrowKey() || keyInfo.IsPressPageDownKey())
                    && _itemsHistory.Count > 0 &&  _inputBuffer.Length >= _options.MinimumPrefixLength)
                {
                    _originalText = _inputBuffer.ToString();
                    if (_inputBuffer.Length > 0)
                    {
                        _localpaginator = new Paginator<ItemHistory>(_itemsHistory
                            .Where(x => x.History.StartsWith(_inputBuffer.ToString(), StringComparison.InvariantCultureIgnoreCase)
                                && DateTime.Now < new DateTime(x.TimeOutTicks)),
                            _options.PageSize, Optional<ItemHistory>.s_empty, (item) => item.History);
                    }
                    else
                    {
                        _localpaginator = new Paginator<ItemHistory>(_itemsHistory
                            .Where(x => DateTime.Now < new DateTime(x.TimeOutTicks)),
                            _options.PageSize, Optional<ItemHistory>.s_empty, (item) => item.History);
                    }
                    if (_localpaginator.Count == 0)
                    {
                        _localpaginator = null;
                    }
                    else
                    {
                        _inputBuffer.Clear().LoadPrintable(_localpaginator.SelectedItem.History);
                        _showingHistory = true;
                    }
                }
                else if (_showingHistory && keyInfo.IsPressEscKey())
                {
                    _inputBuffer.Clear().LoadPrintable(_originalText);
                    _showingHistory = false;
                    _localpaginator = null;
                }
                else if (_showingHistory && IskeyPageNavagator(keyInfo, _localpaginator))
                {
                    _inputBuffer.Clear().LoadPrintable(_localpaginator.SelectedItem.History);
                }
                else if (keyInfo.IsPressEnterKey())
                {
                    if (_showingHistory)
                    {
                        if (_localpaginator.TryGetSelectedItem(out var resulthist))
                        {
                            _inputBuffer.Clear().LoadPrintable(resulthist.History);
                            _localpaginator = null;
                        }
                        _originalText = null;
                    }
                    if (_options.FinishWhenHistoryEnter || !_showingHistory)
                    {
                        result = _inputBuffer.ToString();
                        try
                        {
                            if (!TryValidate(result, _options.Validators, false))
                            {
                                result = default;
                                return false;
                            }
                            AddHistory(result);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            SetError(ex);
                        }
                    }
                    _showingHistory = false;
                }
                else if (CheckDefaultKey(keyInfo))
                {
                    ///none
                }
                else
                {
                    isvalidhit = null;
                }
            } while (KeyAvailable && !stoptoken.IsCancellationRequested);
            result = default;
            return isvalidhit;
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            screenBuffer.Write(_options.Message);
            if (_showingHistory)
            {
                var part1 = _inputBuffer.ToString().Substring(0, _originalText.Length);
                var part2 = _inputBuffer.ToString().Substring(_originalText.Length);
                screenBuffer.WriteFilter(part1);
                screenBuffer.WriteAnswer(part2);
                screenBuffer.PushCursor();
            }
            else
            {
                if (_inputBuffer.InputWithSugestion != null)
                {
                    screenBuffer.WriteAnswer(_inputBuffer.InputWithSugestion[0]);
                    screenBuffer.WriteFilter(_inputBuffer.InputWithSugestion[1]);
                    screenBuffer.PushCursor();
                    screenBuffer.WriteAnswer(_inputBuffer.InputWithSugestion[2]);
                }
                else
                {
                    screenBuffer.PushCursor(_inputBuffer);
                }
            }
            if (HasDescription)
            {
                if (!HideDescription)
                {
                    screenBuffer.WriteLineDescription(_options.Description);
                }
            }

            if (EnabledStandardTooltip)
            {
                ShowStandardHotKeys(screenBuffer);
                CreateMessageHit(screenBuffer);
            }
            if (_showingHistory)
            {
                var subset = _localpaginator.ToSubset();
                foreach (var item in subset)
                {
                    var value = item.History;
                    if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<ItemHistory>.Default.Equals(item, selectedItem))
                    {
                        screenBuffer.WriteLineSelector(value);
                    }
                    else
                    {
                        screenBuffer.WriteLineNotSelector(value);
                    }
                }
                if (_localpaginator.PageCount > 1)
                {
                    screenBuffer.WriteLinePagination(_localpaginator.PaginationMessage());
                }
            }
        }


        private void ShowStandardHotKeys(ScreenBuffer screenBuffer)
        {
            if (_showingHistory || _inputBuffer.IsInAutoCompleteMode())
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, false, _options.EnabledAbortAllPipes, !HasDescription);
            }
            else
            {
                screenBuffer.WriteLineStandardHotKeys(OverPipeLine, _options.EnabledAbortKey, _options.EnabledAbortAllPipes, !HasDescription);
            }

        }
        private void CreateMessageHit(ScreenBuffer screenBuffer)
        {
            var msg = new StringBuilder();
            if (_showingHistory)
            {
                if (_options.FinishWhenHistoryEnter)
                {
                    msg.Append(Messages.ReadlineFisnishHistoryhit);
                }
                else
                {
                    msg.Append(Messages.ReadlineNotFisnishHistoryhit);
                }
                msg.Append(", ");
                msg.Append(Messages.ReadlineHistoryEsc);
            }
            else
            {
                if (_options.EnabledHistory && !_inputBuffer.IsInAutoCompleteMode())
                {
                    msg.Append(string.Format(Messages.ReadlineHistoryhit, _options.MinimumPrefixLength));
                }
                if (_options.SuggestionHandler != null)
                {
                    if (msg.Length > 0)
                    {
                        msg.Append(", ");
                    }
                    msg.Append(Messages.ReadlineSugestionhit);
                    if (_inputBuffer.IsInAutoCompleteMode())
                    {
                        msg.Append(", ");
                        msg.Append(Messages.ReadlineSugestionMode);
                    }
                }
            }
            screenBuffer.WriteLineHint(msg.ToString());
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, string result)
        {
            if (result != null)
            {
                FinishResult = result;
                screenBuffer.WriteAnswer(FinishResult);
            }
        }

        #region IControlReadline

        public IControlReadline AcceptInputTab(bool value)
        {
            if (_options.SuggestionHandler != null)
            {
                _options.AcceptInputTab = false;
            }
            else
            {
                _options.AcceptInputTab = value;
            }
            return this;
        }

        public IControlReadline AddValidator(Func<object, ValidationResult> validator)
        {
            if (validator == null)
            {
                return this;
            }
            return AddValidators(new List<Func<object, ValidationResult>> { validator });
        }

        public IControlReadline AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
        {
            if (validators == null)
            {
                return this;
            }
            _options.Validators.Merge(validators);
            return this;
        }

        public IControlReadline EnabledHistory(bool value)
        {
            _options.EnabledHistory = value;
            return this;
        }

        public IControlReadline FinisWhenHistoryEnter(bool value)
        {
            _options.FinishWhenHistoryEnter = value;
            return this;
        }

        public IControlReadline MaxHistory(byte value)
        {
            _options.MaxHistory = value;
            return this;
        }

        public IControlReadline PageSize(int value)
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

        public IControlReadline MinimumPrefixLength(int value)
        {
            if (value < 0)
            {
                value = 0;
            }
            _options.MinimumPrefixLength = value;
            return this;
        }

        public IControlReadline FileNameHistory(string value)
        {
            _options.FileNameHistory = value;
            return this;
        }

        public IControlReadline Prompt(string value, string description = null)
        {
            _options.Message = value;
            if (description != null)
            {
                _options.Description = description;
            }
            return this;
        }

        public IControlReadline SuggestionHandler(Func<SugestionInput, SugestionOutput> value)
        {
            _options.SuggestionHandler = value;
            if (value != null)
            {
                _options.AcceptInputTab = false;
            }
            return this;
        }

        public IControlReadline TimeoutHistory(TimeSpan value)
        {
            _options.TimeoutHistory = value;
            return this;
        }

        #endregion
    }
}
