namespace CommandDotNet.Example
{
    [Command()]
    internal class Examples
    {
        [SubCommand]
        public Prompts Prompts { get; set; } = null!;
    }
}
