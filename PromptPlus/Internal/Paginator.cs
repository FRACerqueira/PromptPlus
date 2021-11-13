// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

using PromptPlusControls.Drivers;

namespace PromptPlusControls.Internal
{
    internal class Paginator<T>
    {
        private T[] _filteredItems;
        private int _maxpageSize;
        private readonly T[] _items;
        private readonly int _userpageSize;
        private readonly Func<T, string> _textSelector;
        private readonly Func<T, bool> _validatorAction;

        public Paginator(IEnumerable<T> items, int? pageSize, Optional<T> defaultValue, Func<T, string> textSelector, Func<T, bool> validatorAction = null)
        {
            _items = items.ToArray();
            _userpageSize = pageSize ?? _items.Length;
            _textSelector = textSelector;
            EnsureTerminalPagesize();
            _validatorAction = validatorAction;
            if (validatorAction == null)
            {
                _validatorAction = (item) => true;
            }
            InitializeDefaults(defaultValue);
        }

        private void EnsureTerminalPagesize()
        {
            T selectedItem = default;
            if (SelectedIndex >= 0 && (_maxpageSize * SelectedPage) + SelectedIndex <= _items.Length - 1)
            {
                selectedItem = _items[(_maxpageSize * SelectedPage) + SelectedIndex];
            }
            if (PromptPlus._consoleDriver.BufferHeight - 1 < _userpageSize + ConsoleDriver.MinBufferHeight)
            {
                _maxpageSize = PromptPlus._consoleDriver.BufferHeight - (ConsoleDriver.MinBufferHeight);
            }
            else
            {
                _maxpageSize = _userpageSize;
            }
            if (_maxpageSize < 0)
            {
                _maxpageSize *= -1;
            }
            if (SelectedIndex >= 0 && _maxpageSize > 0)
            {
                _filteredItems = _items
                    .Where(x => _textSelector(x).IndexOf(FilterTerm, StringComparison.OrdinalIgnoreCase) != -1)
                    .ToArray();

                var size = Math.Min(_maxpageSize, _filteredItems.Length);

                for (var i = (_maxpageSize * SelectedPage) + SelectedIndex; i < _filteredItems.Length; i++)
                {
                    if (EqualityComparer<T>.Default.Equals(_filteredItems[i], selectedItem))
                    {
                        SelectedIndex = i % _maxpageSize;
                        SelectedPage = i / _maxpageSize;
                        PageCount = ((_filteredItems.Length - 1) / _maxpageSize) + 1;
                        break;
                    }
                }
            }
        }

        public int PageCount { get; private set; }

        public int SelectedPage { get; private set; }

        public int SelectedIndex { get; private set; } = 0;

        public T SelectedItem => _filteredItems[(_maxpageSize * SelectedPage) + SelectedIndex];

        public int TotalCount => _filteredItems.Length;

        public int Count => Math.Min(_filteredItems.Length - (_maxpageSize * SelectedPage), _maxpageSize);

        public string FilterTerm { get; private set; } = "";

        public bool TryGetSelectedItem(out T selectedItem)
        {
            if (SelectedIndex == -1 || _filteredItems.Length == 0)
            {
                selectedItem = default;
                return false;
            }
            selectedItem = _filteredItems[(_maxpageSize * SelectedPage) + SelectedIndex];
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
                if (_filteredItems.Count() > 0)
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
            return false;
        }

        private (int, int) FirstItemValid(int initvalue)
        {
            var ok = false;
            var startindex = initvalue;
            var auxpage = SelectedPage;
            var aux = (_maxpageSize * auxpage) + startindex;
            var auxSelectedIndex = aux;
            Func<int> auxcount = () => Math.Min(_filteredItems.Length - (_maxpageSize * auxpage), _maxpageSize);
            var first = true;
            do
            {
                if (startindex > auxcount.Invoke() - 1)
                {
                    auxpage = auxpage >= PageCount - 1 ? 0 : auxpage + 1;
                    startindex = 0;
                    aux = (_maxpageSize * auxpage) + startindex;
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
            var aux = (_maxpageSize * auxpage) + startindex;
            var auxSelectedIndex = aux;
            Func<int> auxcount = () => Math.Min(_filteredItems.Length - (_maxpageSize * auxpage), _maxpageSize);
            var first = true;
            do
            {
                if (startindex < 0)
                {
                    auxpage = auxpage <= 0 ? PageCount - 1 : auxpage - 1;
                    startindex = auxcount.Invoke() - 1;
                    aux = (_maxpageSize * auxpage) + startindex;
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

        public bool LastItem()
        {
            if (Count >= 0)
            {
                SelectedIndex = Count - 1;
                if (_filteredItems.Count() > 0)
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

        public string PaginationMessage()
        {
            return string.Format(Messages.PaginationTemplate, TotalCount, SelectedPage + 1, PageCount);
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

        public void UpdateFilter(string term)
        {
            FilterTerm = term;

            SelectedIndex = -1;
            SelectedPage = 0;

            EnsureTerminalPagesize();

            InitializeCollection();

            if (Count == 1)
            {
                FirstItem();
            }
        }

        public ArraySegment<T> ToSubset()
        {
            EnsureTerminalPagesize();
            return new ArraySegment<T>(_filteredItems, _maxpageSize * SelectedPage, Count);
        }

        public void EnsureVisibleIndex(int index)
        {
            for (var i = 0; i < _items.Length; i++)
            {
                if (i == index)
                {
                    SelectedIndex = i % _maxpageSize;
                    SelectedPage = i / _maxpageSize;
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
            _filteredItems = _items.Where(x => _textSelector(x).IndexOf(FilterTerm, StringComparison.OrdinalIgnoreCase) != -1)
                                    .ToArray();

            if (_filteredItems.Length == 0)
            {
                PageCount = 0;
            }
            else
            {
                PageCount = (_filteredItems.Length - 1) / _maxpageSize + 1;
            }
        }

        private void InitializeDefaults(Optional<T> defaultValue)
        {
            InitializeCollection();

            if (!defaultValue.HasValue)
            {
                return;
            }

            for (var i = 0; i < _filteredItems.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(_filteredItems[i], defaultValue))
                {
                    SelectedIndex = i % _maxpageSize;
                    SelectedPage = i / _maxpageSize;
                    break;
                }
            }
        }
    }
}
