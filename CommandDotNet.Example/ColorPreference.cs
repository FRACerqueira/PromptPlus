using System.ComponentModel;

namespace CommandDotNet.Example
{
    //Empty description hidden type in prompt.
    //If filled in, it shows the description in the type.
    //If omitted the attribute is using the class name
    [Description("")]
    public enum ColorPreference
    {
        Blue,
        Green,
        Red,
        Yellow
    }
}
