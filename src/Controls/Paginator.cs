// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromptPlusLibrary.Controls
{
    internal sealed class Paginator<T>
    {
        private T[] _filteredItems = [];
        private T[] _items;
        private readonly int _userPageSize;
        private readonly Func<T, string> _textSelector;
        private readonly Func<T, bool> _validatorAction;
        private readonly FilterMode _filterMode;
        private readonly Func<T, T, bool>? _foundDefault;
        private readonly Func<T, bool>? _countValidator;

        public Paginator(
            FilterMode filterMode,
            IEnumerable<T> items,
            int pageSize,
            Optional<T> defaultValue,
            Func<T, T, bool>? foundDefault,
            Func<T, string>? textSelector = null,
            Func<T, bool>? validatorAction = null,
            Func<T, bool>? countValidator = null)
        {
            _filterMode = filterMode;
            _items = [.. items];
            _userPageSize = pageSize;
            _textSelector = textSelector ?? (x => x?.ToString() ?? string.Empty);
            _validatorAction = validatorAction ?? (_ => true);
            _countValidator = countValidator;
            _foundDefault = foundDefault;

            InitializeCollection();
            Initialize(defaultValue, _foundDefault);
        }

        public int PageCount { get; private set; }
        public int SelectedPage { get; private set; }
        public int SelectedIndex { get; private set; }
        public int CurrentIndex => (_userPageSize * SelectedPage) + SelectedIndex;

        public T SelectedItem => SelectedIndex >= 0 && SelectedIndex < _filteredItems.Length
            ? _filteredItems[(_userPageSize * SelectedPage) + SelectedIndex]
            : default!;

        public int TotalCountValid => _countValidator == null
            ? _filteredItems.Length
            : _filteredItems.Count(item => _countValidator(item));

        public int TotalCount => _filteredItems.Length;
        public int Count => Math.Min(_filteredItems.Length - (_userPageSize * SelectedPage), _userPageSize);
        public string FilterTerm { get; private set; } = string.Empty;

        public bool IsFirstPageItem => SelectedIndex == 0 && Count > 0;
        public bool IsLastPageItem => SelectedIndex == Count - 1 && Count > 0;
        public bool IsUnselected => SelectedIndex == -1;

        public void UnSelect() => SelectedIndex = -1;

        public bool TryGetSelected(out T selectedItem)
        {
            if (SelectedIndex == -1 || _filteredItems.Length == 0)
            {
                selectedItem = default!;
                return false;
            }

            selectedItem = _filteredItems[(_userPageSize * SelectedPage) + SelectedIndex];
            if (!_validatorAction(selectedItem))
            {
                selectedItem = default!;
                return false;
            }
            return true;
        }

        public bool FirstItem()
        {
            if (Count <= 0)
            {
                UnSelect();
                return false;
            }
            var oldindex = SelectedIndex;
            var oldpage = SelectedPage;
            (int index, int page) = FindValidItem(0, forward: true);
            if (index >= 0)
            {
                SelectedPage = page;
                SelectedIndex = index;
                return (oldindex != SelectedIndex || oldpage != SelectedPage);
            }
            return false;
        }

        public bool LastItem()
        {
            if (Count <= 0)
            {
                UnSelect();
                return false;
            }
            var oldindex = SelectedIndex;
            var oldpage = SelectedPage;
            (int index, int page) = FindValidItem(Count - 1, forward: false);
            if (index >= 0)
            {
                SelectedPage = page;
                SelectedIndex = index;
                return (oldindex != SelectedIndex || oldpage != SelectedPage);
            }
            return false;
        }

        public bool NextItem()
        {
            if (Count <= 0)
            {
                UnSelect();
                return false;
            }
            var oldindex = SelectedIndex;
            var oldpage = SelectedPage;
            (int index, int page) = FindValidItem(SelectedIndex + 1, forward: true);
            if (index >= 0)
            {
                SelectedPage = page;
                SelectedIndex = index;
                return (oldindex != SelectedIndex || oldpage != SelectedPage);
            }
            return false;
        }

        public bool PreviousItem()
        {
            if (Count <= 0)
            {
                UnSelect();
                return false;
            }
            var oldindex = SelectedIndex;
            var oldpage = SelectedPage;
            (int index, int page) = FindValidItem(SelectedIndex - 1, forward: false);
            if (index >= 0)
            {
                SelectedPage = page;
                SelectedIndex = index;
                return (oldindex != SelectedIndex || oldpage != SelectedPage);
            }
            return false;
        }

        public bool End(IndexOption selectedIndexOption = IndexOption.LastItem)
        {
            if (Count <= 0)
            {
                UnSelect();
                return false;
            }

            var oldindex = SelectedIndex;
            var oldpage = SelectedPage;
            (int index, int page) = FindValidItem(TotalCount - 1, forward: false);
            if (index >= 0)
            {
                SelectedPage = page;
                SelectedIndex = index;
                MoveToSelectIndex(selectedIndexOption);
                return (oldindex != SelectedIndex || oldpage != SelectedPage);
            }
            return false;

        }

        public bool Home(IndexOption selectedIndexOption = IndexOption.FirstItem)
        {
            if (Count <= 0)
            {
                UnSelect();
                return false;
            }
            var oldindex = SelectedIndex;
            var oldpage = SelectedPage; 
            SelectedPage = 0;
            MoveToSelectIndex(selectedIndexOption);
            return (oldindex != SelectedIndex || oldpage != SelectedPage);
        }

        public bool NextPage(IndexOption selectedIndexOption = IndexOption.None)
        {
            if (Count <= 0)
            {
                UnSelect();
                return false;
            }
            if (PageCount <= 1)
            {
                MoveToSelectIndex(selectedIndexOption);
                return false;
            }
            var oldindex = SelectedIndex;
            var oldpage = SelectedPage;
            SelectedPage = (SelectedPage + 1) % PageCount;
            MoveToSelectIndex(selectedIndexOption);
            return (oldindex != SelectedIndex || oldpage != SelectedPage);
        }

        public bool PreviousPage(IndexOption selectedIndexOption = IndexOption.None)
        {
            if (Count <= 0)
            {
                UnSelect();
                return false;
            }
            if (PageCount <= 1)
            {
                MoveToSelectIndex(selectedIndexOption);
                return false;
            }
            var oldindex = SelectedIndex;
            var oldpage = SelectedPage;
            SelectedPage = (SelectedPage - 1 + PageCount) % PageCount;
            MoveToSelectIndex(selectedIndexOption);
            return (oldindex != SelectedIndex || oldpage != SelectedPage);
        }

        public void UpdatColletion(IEnumerable<T> items, Optional<T>? selected = null)
        {
            _items = [.. items];
            InitializeCollection();
            if (selected.HasValue)
            {
                Initialize(selected.Value, _foundDefault);
            }
            else
            {
                FirstItem();
            }
        }

        public void UpdateFilter(string term, Optional<T>? selected = null)
        {
            FilterTerm = term;
            SelectedIndex = -1;
            SelectedPage = 0;

            InitializeCollection();

            if (selected.HasValue)
            {
                Initialize(selected.Value, _foundDefault);
            }
            else if (Count > 0)
            {
                FirstItem();
            }
        }

        public ArraySegment<T> GetPageData()
        {
            //EnsurePage();
            return new ArraySegment<T>(_filteredItems, _userPageSize * SelectedPage, Count);
        }

        public void EnsureVisibleIndex(int index)
        {
            if (index >= 0 && index < _items.Length)
            {
                SelectedIndex = index % _userPageSize;
                SelectedPage = index / _userPageSize;
            }
        }

        public string PaginationMessage(Func<int, int, int, string>? template)
        {
            return template != null
                ? template(TotalCountValid, SelectedPage + 1, PageCount)
                : string.Format(Messages.PaginationTemplate, TotalCountValid, SelectedPage + 1, PageCount);
        }

        private void InitializeCollection()
        {
            _filteredItems = _filterMode switch
            {
                FilterMode.StartsWith => [.. _items.Where(x => _textSelector(x).StartsWith(FilterTerm, StringComparison.InvariantCultureIgnoreCase))],
                FilterMode.Contains => [.. _items.Where(x => _textSelector(x).Contains(FilterTerm, StringComparison.InvariantCultureIgnoreCase))],
                FilterMode.Disabled => [.. _items],
                _ => throw new NotImplementedException($"FilterMode: {_filterMode} Not Implemented")
            };

            PageCount = _filteredItems.Length == 0 ? 0 : (_filteredItems.Length - 1) / _userPageSize + 1;
        }

        private void Initialize(Optional<T> defaultValue, Func<T, T, bool>? foundDefault)
        {
            if (!defaultValue.HasValue)
            {
                return;
            }

            for (int i = 0; i < _filteredItems.Length; i++)
            {
                if (foundDefault?.Invoke(_filteredItems[i], defaultValue.Value) == true ||
                    EqualityComparer<string>.Default.Equals(_textSelector(_filteredItems[i]), _textSelector(defaultValue.Value)))
                {
                    SelectedIndex = i % _userPageSize;
                    SelectedPage = i / _userPageSize;
                    break;
                }
            }
        }

        private void MoveToSelectIndex(IndexOption selectedIndexOption)
        {
            switch (selectedIndexOption)
            {
                case IndexOption.None:
                    SelectedIndex = -1;
                    break;
                case IndexOption.FirstItem:
                case IndexOption.FirstItemWhenHasPages:
                    FirstItem();
                    break;
                case IndexOption.LastItem:
                case IndexOption.LastItemWhenHasPages:
                    LastItem();
                    break;
            }
        }

        private (int index, int page) FindValidItem(int startIndex, bool forward)
        {
            int page = SelectedPage;
            int index = startIndex;

            while (true)
            {
                if (index < 0 || index >= Count)
                {
                    page = forward ? (page + 1) % PageCount : (page - 1 + PageCount) % PageCount;
                    index = forward ? 0 : Count - 1;
                }

                int globalIndex = (_userPageSize * page) + index;
                if (globalIndex >= _filteredItems.Length || globalIndex < 0)
                {
                    break;
                }

                if (_validatorAction(_filteredItems[globalIndex]))
                {
                    return (index, page);
                }

                index += forward ? 1 : -1;
            }

            return (-1, page);
        }
    }
}
