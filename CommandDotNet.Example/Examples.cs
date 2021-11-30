namespace CommandDotNet.Example
{
    [Command()]
    internal class Examples
    {
#if NETCOREAPP3_1
        [SubCommand]
#else
        [Subcommand]
#endif
        public Prompts Prompts { get; set; } = null!;
    }
}
