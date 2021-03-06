// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Objects
{
    public struct ValueDescription<T>
    {
        public ValueDescription()
        {
            Value = default;
            Description = "";

        }
        public ValueDescription(T value, string description)
        {
            Value = value;
            Description = description;
        }

        public T Value { get; }
        public string Description { get; }
    }
}
