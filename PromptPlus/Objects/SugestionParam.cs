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

        internal SugestionInput(string input, int cursorPrompt,object context)
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
            var word = new StringBuilder();
            while (pos > 0)
            {
                pos--;
                if (PromptText[pos] != ' ')
                {
                    break;
                }
            }
            while (pos >=0)
            {
                if (PromptText[pos] == ' ')
                {
                    break;
                }
                else
                {
                    word.Insert(0,PromptText[pos]);
                }
                pos--;
            }
            return word.ToString();
        }
    }

    public struct SugestionOutput
    {
        private readonly List<ItemSugestion> _items = new();
        public SugestionOutput()
        {
            Sugestions = new ReadOnlyCollection<ItemSugestion>(new List<ItemSugestion>());
        }

        public SugestionOutput(IList<ItemSugestion> items)
        {
            _items.AddRange(items);
            Sugestions = new ReadOnlyCollection<ItemSugestion>(_items);
        }

        public void Add(string value, bool clearrestofline = false)
        { 
            _items.Add(new ItemSugestion(value, clearrestofline));
            Sugestions = new ReadOnlyCollection<ItemSugestion>(_items);
        }

        public void AddRange(IList<ItemSugestion> items)
        {
            _items.AddRange(items);
            Sugestions = new ReadOnlyCollection<ItemSugestion>(_items);
        }

        public ReadOnlyCollection<ItemSugestion> Sugestions { get; private set;}
    }

    public struct ItemSugestion
    {
        public ItemSugestion()
        {
            Sugestion = null;
            ClearRestline = false;
        }

        internal ItemSugestion(string value, bool clearRest)
        {
            Sugestion = value;
            ClearRestline = clearRest; 
        }

        public string Sugestion { get; }
        public bool ClearRestline { get; }
    }

}
