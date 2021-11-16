namespace PromptPlusObjects
{
    public struct ValueDescription<T>
    {
        public ValueDescription(T value, string description)
        {
            Value = value;
            Description = description;
        }

        public T Value { get; set; }
        public string Description { get; set; }
    }
}
