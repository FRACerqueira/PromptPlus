using CommandDotNet.Builders;

namespace PPlus.CommandDotNet.Controls
{
    internal struct WizardArgs
    {
        public WizardArgs(string value, IArgumentNode node, bool isSecret)
        {
            ArgValue = value;
            ArgNode = node;
            IsSecret = isSecret;
        }
        public string ArgValue { get; }
        public bool IsSecret { get; }
        public IArgumentNode ArgNode { get; }
    }
}
