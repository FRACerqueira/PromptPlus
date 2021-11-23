using System;
using System.Collections.Generic;

using CommandDotNet.Builders;

using PPlus.CommandDotNet.Controls;

namespace PPlus
{
    internal interface IControlWizard: IPromptControls<IArgumentNode>, IPromptPipe
    {
        IControlWizard Config(Action<IPromptConfig> context);
        IControlWizard UpdateTokenArgs(IEnumerable<WizardArgs> args);
    }
}
