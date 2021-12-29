// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PPlus.Objects
{
    public struct SugestionInput
    {
        public SugestionInput()
        {
            PromptText = null;
            CursorPrompt = 0;
            Context = null;
        }

        internal SugestionInput(string input, int cursorPrompt, object context)
        {
            PromptText = input;
            CursorPrompt = cursorPrompt;
            Context = context;
        }
        public string PromptText { get; }
        public int CursorPrompt { get; }
        public object Context { get; }

        public string CurrentWord()
        {
            var pos = CursorPrompt;
            var text = PromptText;

            if (pos <= 0 || text.Length == 0)
            {
                return string.Empty;
            }
            if (pos > text.Length - 1)
            {
                pos = text.Length - 1;
            }
            var word = new StringBuilder();
            if (PromptText[pos] != ' ')
            {
                while (pos > 0)
                {
                    pos--;
                    if (PromptText[pos] == ' ')
                    {
                        pos++;
                        break;
                    }
                }
            }
            while (pos >= 0 && pos < PromptText.Length)
            {
                if (PromptText[pos] == ' ')
                {
                    break;
                }
                else
                {
                    word.Append(PromptText[pos]);
                }
                pos++;
            }
            return word.ToString();
        }
    }

    public struct SugestionOutput
    {
        private readonly List<ItemSugestion> _items = new();
        public SugestionOutput()
        {
            MsgError = null;
            Sugestions = new ReadOnlyCollection<ItemSugestion>(new List<ItemSugestion>());
            CursorPrompt = null;
        }

        public SugestionOutput(IList<ItemSugestion> items, string msgerror = null)
        {
            MsgError = null;
            if (items.Count == 0)
            {
                MsgError = msgerror;
            }
            _items.AddRange(items);
            Sugestions = new ReadOnlyCollection<ItemSugestion>(_items);
            CursorPrompt = null;
        }

        public void Add(string value, bool clearrestofline = false, object objvalue = null, string description = null)
        {
            MsgError = null;
            _items.Add(new ItemSugestion(value, clearrestofline, objvalue ?? value, description));
            Sugestions = new ReadOnlyCollection<ItemSugestion>(_items);
        }

        public void AddRange(IList<ItemSugestion> items)
        {
            MsgError = null;
            _items.AddRange(items);
            Sugestions = new ReadOnlyCollection<ItemSugestion>(_items);
        }

        public void SetMsgError(string value)
        {
            if (_items.Count == 0)
            {
                MsgError = value;
            }
        }
        public void SetCursorPrompt(int value)
        {
            CursorPrompt = value;
        }

        public ReadOnlyCollection<ItemSugestion> Sugestions { get; private set; }

        public string MsgError { get; private set; }

        public int? CursorPrompt { get; private set; }

    }

    public struct ItemSugestion
    {
        public ItemSugestion()
        {
            Sugestion = null;
            ClearRestline = false;
            ObjectValue = null;
            Description = null;
        }

        internal ItemSugestion(string value, bool clearRest, object objvalue, string description)
        {
            Sugestion = value;
            ClearRestline = clearRest;
            ObjectValue = objvalue;
            Description = description;
        }

        public string Sugestion { get; }
        public bool ClearRestline { get; }
        public object ObjectValue { get; }
        public string Description { get; }


    }

}
