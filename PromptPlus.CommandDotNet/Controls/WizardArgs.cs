using CommandDotNet.Builders;

namespace PPlus.CommandDotNet.Controls
{
    internal struct WizardArgs
    {
        public WizardArgs(string value, bool isSecret, bool isenabled)
        {
            ArgValue = value;
            ArgNode = null;
            IsSecret = isSecret;
            IsEnabled = isenabled;
        }

        public WizardArgs(string value, IArgumentNode node, bool isSecret, bool isenabled)
        {
            ArgValue = value;
            ArgNode = node;
            IsSecret = isSecret;
            IsEnabled = isenabled;
        }
        public string ArgValue { get; }
        public bool IsSecret { get; }
        public IArgumentNode ArgNode { get; }
        public bool IsEnabled { get; }
    }
}
