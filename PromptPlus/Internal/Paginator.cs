// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

using PromptPlusControls.Drivers;

namespace PromptPlusControls.Internal
{
    internal class Paginator<T> : IDisposable
    {
        private T[] _filteredItems;
        private int _maxpageSize;
        private readonly T[] _items;
        private readonly int _userpageSize;
        private readonly Func<T, string> _textSelector;
        private readonly IConsoleDriver _consoleDriver;

        public Paginator(IEnumerable<T> items, int? pageSize, Optional<T> defaultValue, Func<T, string> textSelector)
        {
            _consoleDriver = new ConsoleDriver();
            _items = items.ToArray();
            _userpageSize = pageSize ?? _items.Length;
            _textSelector = textSelector;
            EnsureTerminalPagesize();
            InitializeDefaults(defaultValue);
        }

        #region IDisposable

        public void Dispose()
        {
            _consoleDriver.Dispose();
        }

        #endregion

        private void EnsureTerminalPagesize()
        {
            T selectedItem = default;
            if (SelectedIndex >= 0 && (_maxpageSize * SelectedPage) + SelectedIndex <= _items.Length - 1)
            {
                selectedItem = _items[(_maxpageSize * SelectedPage) + SelectedIndex];
            }
            if (_consoleDriver.BufferHeight - 1 < _userpageSize + ConsoleDriver.MinBufferHeight)
            {
                _maxpageSize = _consoleDriver.BufferHeight - (ConsoleDriver.MinBufferHeight);
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
                return true;
            }
            return false;
        }

        public bool LastItem()
        {
            if (Count >= 0)
            {
                SelectedIndex = Count - 1;
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
            return true;
        }

        public bool PreviousItem()
        {
            if (Count < 0)
            {
                return false;
            }
            SelectedIndex = SelectedIndex <= 0 ? Count - 1 : SelectedIndex - 1;
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

            PageCount = (_filteredItems.Length - 1) / _maxpageSize + 1;
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
