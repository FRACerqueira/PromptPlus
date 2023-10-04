// ***************************************************************************************
// MIT LICENCE
// Copyright (c) 2019 shibayan.
// https://github.com/shibayan/Sharprompt
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace PPlus.Controls.Objects
{
    internal class Paginator<T>
    {
        private T[] _filteredItems;
        private readonly T[] _items;
        private readonly int _userpageSize;
        private readonly Func<T, string> _textSelector;
        private readonly Func<T, bool> _validatorAction;
        private readonly FilterMode _filterMode;
        private readonly Func<T, T, bool>? _founddefault;
        private readonly Func<T, bool>? _countvalidator;

        public Paginator(FilterMode filterMode, IEnumerable<T> items, int pageSize, Optional<T> defaultValue, Func<T, T, bool>? founddefault, Func<T, string>? textSelector = null, Func<T, bool> validatorAction = null, Func<T, bool> countvalidator = null)
        {
            _countvalidator = countvalidator;
            _filterMode = filterMode;
            _items = items.ToArray();
            _userpageSize = pageSize;
            _textSelector = textSelector ?? ((x) => x.ToString());
            _filteredItems = Array.Empty<T>();
            _validatorAction = validatorAction;
            if (validatorAction == null)
            {
                _validatorAction = (item) => true;
            }
            EnsurePage();
            _founddefault = founddefault;
            InitializeDefaults(defaultValue, _founddefault);
        }

        private void EnsurePage()
        {
            T selectedItem = default;
            if (SelectedIndex >= 0 && (_userpageSize * SelectedPage) + SelectedIndex <= _items.Length - 1)
            {
                selectedItem = _items[(_userpageSize * SelectedPage) + SelectedIndex];
            }

            if (SelectedIndex >= 0 && _userpageSize > 0)
            {
                if (_filterMode == FilterMode.StartsWith)
                {
                    _filteredItems = _items
                        .Where(x => _textSelector(x).StartsWith(FilterTerm, StringComparison.InvariantCultureIgnoreCase))
                        .ToArray();
                }
                else if (_filterMode == FilterMode.Contains)
                {
                    _filteredItems = _items
                        .Where(x => _textSelector(x).Contains(FilterTerm, StringComparison.InvariantCultureIgnoreCase))
                        .ToArray();
                }
                else // Disabled
                {
                    _filteredItems = _items.ToArray();
                }


                for (var i = (_userpageSize * SelectedPage) + SelectedIndex; i < _filteredItems.Length; i++)
                {
                    if (EqualityComparer<T>.Default.Equals(_filteredItems[i], selectedItem))
                    {
                        SelectedIndex = i % _userpageSize;
                        SelectedPage = i / _userpageSize;
                        PageCount = ((_filteredItems.Length - 1) / _userpageSize) + 1;
                        break;
                    }
                }
            }
        }

        public int PageCount { get; private set; }

        public int SelectedPage { get; private set; }

        public int SelectedIndex { get; private set; } = 0;

        public int CurrentIndex => (_userpageSize * SelectedPage) + SelectedIndex;


        public T SelectedItem
        {
            get
            {
                if (SelectedIndex < 0)
                {
                    return default;
                }
                return _filteredItems[(_userpageSize * SelectedPage) + SelectedIndex];
            }
        }

        public int TotalCountValid
        { 
            get 
            {
                if (_countvalidator == null)
                {
                    return _filteredItems.Length;
                }
                return _filteredItems.Length - _filteredItems.Where(item => !_countvalidator.Invoke(item)).Count();
            }
        }

        public int TotalCount => _filteredItems.Length;

        public int Count => Math.Min(_filteredItems.Length - (_userpageSize * SelectedPage), _userpageSize);

        public string FilterTerm { get; private set; } = "";

        public bool TryGetSelectedItem(out T selectedItem)
        {
            if (SelectedIndex == -1 || _filteredItems.Length == 0)
            {
                selectedItem = default;
                return false;
            }
            selectedItem = _filteredItems[(_userpageSize * SelectedPage) + SelectedIndex];
            if (!_validatorAction.Invoke(selectedItem))
            {
                selectedItem = default;
                return false;
            }
            return true;
        }

        public bool IsFistPageItem => SelectedIndex == 0 && Count > 0;

        public bool IsLastPageItem => SelectedIndex == Count - 1 && Count > 0;

        public bool IsUnSelected => SelectedIndex == -1;

        public void UnSelected()
        {
            SelectedIndex = -1;
        }

        public bool FirstItem()
        {
            if (Count >= 0)
            {
                SelectedIndex = 0;
                if (_filteredItems.Length > 0)
                {
                    var aux = FirstItemValid(SelectedIndex);
                    if (aux.Item1 < 0)
                    {
                        return false;
                    }
                    SelectedPage = aux.Item2;
                    SelectedIndex = aux.Item1;
                }
                return true;
            }
            UnSelected();
            return false;
        }

        private (int, int) FirstItemValid(int initvalue)
        {
            var ok = false;
            var startindex = initvalue;
            var auxpage = SelectedPage;
            var aux = (_userpageSize * auxpage) + startindex;
            var auxSelectedIndex = aux;
            int auxcount() => Math.Min(_filteredItems.Length - (_userpageSize * auxpage), _userpageSize);
            var first = true;
            do
            {
                if (startindex > auxcount() - 1)
                {
                    auxpage = auxpage >= PageCount - 1 ? 0 : auxpage + 1;
                    startindex = 0;
                    aux = (_userpageSize * auxpage) + startindex;
                }
                if (aux == auxSelectedIndex && !first)
                {
                    break;
                }
                first = false;
                var auxselectedItem = _filteredItems[aux];
                if (_validatorAction.Invoke(auxselectedItem))
                {
                    startindex--;
                    ok = true;
                }
                aux++;
                startindex++;
            }
            while (!ok);
            if (!ok)
            {
                startindex = -1;
            }
            return (startindex, auxpage);
        }

        private (int, int) LastItemValid(int initvalue)
        {
            var ok = false;
            var startindex = initvalue;
            var auxpage = SelectedPage;
            var aux = (_userpageSize * auxpage) + startindex;
            var auxSelectedIndex = aux;
            int auxcount() => Math.Min(_filteredItems.Length - (_userpageSize * auxpage), _userpageSize);
            var first = true;
            do
            {
                if (startindex < 0)
                {
                    auxpage = auxpage <= 0 ? PageCount - 1 : auxpage - 1;
                    startindex = auxcount() - 1;
                    aux = (_userpageSize * auxpage) + startindex;
                }
                if (aux == auxSelectedIndex && !first)
                {
                    break;
                }
                first = false;
                var auxselectedItem = _filteredItems[aux];
                if (_validatorAction.Invoke(auxselectedItem))
                {
                    startindex++;
                    ok = true;
                }
                aux--;
                startindex--;
            }
            while (!ok);
            if (!ok)
            {
                startindex = -1;
            }
            return (startindex, auxpage);
        }

        public string PaginationMessage()
        {
            return string.Format(Messages.PaginationTemplate, TotalCountValid, SelectedPage + 1, PageCount);
        }

        public bool LastItem()
        {
            if (Count >= 0)
            {
                SelectedIndex = Count - 1;
                if (_filteredItems.Length > 0)
                {
                    var aux = LastItemValid(SelectedIndex);
                    if (aux.Item1 < 0)
                    {
                        return false;
                    }
                    SelectedPage = aux.Item2;
                    SelectedIndex = aux.Item1;
                }
                return true;
            }
            return false;
        }

        public bool NextItem()
        {
            if (Count < 0)
            {
                return false;
            }
            SelectedIndex = SelectedIndex >= Count - 1 ? 0 : SelectedIndex + 1;
            var aux = FirstItemValid(SelectedIndex);
            if (aux.Item1 < 0)
            {
                return false;
            }
            SelectedPage = aux.Item2;
            SelectedIndex = aux.Item1;
            return true;
        }

        public bool PreviousItem()
        {
            if (Count < 0)
            {
                return false;
            }
            SelectedIndex = SelectedIndex <= 0 ? Count - 1 : SelectedIndex - 1;
            var aux = LastItemValid(SelectedIndex);
            if (aux.Item1 < 0)
            {
                return false;
            }
            SelectedPage = aux.Item2;
            SelectedIndex = aux.Item1;
            return true;
        }
        public void Home(IndexOption selectedIndexOption = IndexOption.FirstItem)
        {
            SelectedPage = 0;
            MoveToSelectIndex(selectedIndexOption);
        }

        public bool NextPage(IndexOption selectedIndexOption = IndexOption.None)
        {
            if (Count < 0)
            {
                return false;
            }
            if (PageCount == 1)
            {
                if (selectedIndexOption == IndexOption.FirstItem || selectedIndexOption == IndexOption.LastItem)
                {
                    MoveToSelectIndex(selectedIndexOption);
                }
                return false;
            }
            SelectedPage = SelectedPage >= PageCount - 1 ? 0 : SelectedPage + 1;
            MoveToSelectIndex(selectedIndexOption);
            return true;
        }

        public bool PreviousPage(IndexOption selectedIndexOption = IndexOption.None)
        {
            if (Count < 0)
            {
                return false;
            }
            if (PageCount == 1)
            {
                if (selectedIndexOption == IndexOption.FirstItem || selectedIndexOption == IndexOption.LastItem)
                {
                    MoveToSelectIndex(selectedIndexOption);
                }
                return false;
            }

            SelectedPage = SelectedPage <= 0 ? PageCount - 1 : SelectedPage - 1;
            MoveToSelectIndex(selectedIndexOption);
            return true;
        }

        public void UpdateFilter(string term, Optional<T>? selected = null)
        {
            FilterTerm = term;

            SelectedIndex = -1;
            SelectedPage = 0;

            InitializeCollection();

            if (selected.HasValue)
            {
                InitializeDefaults(selected.Value, _founddefault);
                return;
            }


            if (Count > 0)
            {
                FirstItem();
            }
        }

        public ArraySegment<T> ToSubset()
        {
            EnsurePage();
            return new ArraySegment<T>(_filteredItems, _userpageSize * SelectedPage, Count);
        }

        public void EnsureVisibleIndex(int index)
        {
            for (var i = 0; i < _items.Length; i++)
            {
                if (i == index)
                {
                    SelectedIndex = i % _userpageSize;
                    SelectedPage = i / _userpageSize;
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
                case IndexOption.FirstItemWhenHasPages:
                case IndexOption.FirstItem:
                    FirstItem();
                    break;
                case IndexOption.LastItemWhenHasPages:
                case IndexOption.LastItem:
                    LastItem();
                    break;
                default:
                    break;
            }
        }

        private void InitializeCollection()
        {
            if (_filterMode == FilterMode.StartsWith)
            {
                _filteredItems = _items.Where(x => _textSelector(x).StartsWith(FilterTerm, StringComparison.InvariantCultureIgnoreCase))
                                       .ToArray();
            }
            else if (_filterMode == FilterMode.Contains)
            {
                _filteredItems = _items.Where(x => _textSelector(x).Contains(FilterTerm, StringComparison.InvariantCultureIgnoreCase))
                                       .ToArray();
            }
            else if (_filterMode == FilterMode.Disabled)
            {
                _filteredItems = _items.ToArray();
            }
            else
            {
                throw new PromptPlusException($"FilterMode: {_filterMode} Not Implemented");
            }

            if (_filteredItems.Length == 0)
            {
                PageCount = 0;
            }
            else
            {
                PageCount = (_filteredItems.Length - 1) / _userpageSize + 1;
            }
        }

        private void InitializeDefaults(Optional<T> defaultValue, Func<T, T, bool>? founddefault)
        {
            InitializeCollection();

            if (!defaultValue.HasValue)
            {
                return;
            }

            for (var i = 0; i < _filteredItems.Length; i++)
            {
                if (founddefault != null)
                {
                    if (founddefault(_filteredItems[i], defaultValue.Value))
                    {
                        SelectedIndex = i % _userpageSize;
                        SelectedPage = i / _userpageSize;
                        break;
                    }
                }
                else
                {
                    if (EqualityComparer<string>.Default.Equals(_textSelector(_filteredItems[i]), _textSelector(defaultValue.Value)))
                    {
                        SelectedIndex = i % _userpageSize;
                        SelectedPage = i / _userpageSize;
                        break;
                    }
                }
            }
        }
    }
}
