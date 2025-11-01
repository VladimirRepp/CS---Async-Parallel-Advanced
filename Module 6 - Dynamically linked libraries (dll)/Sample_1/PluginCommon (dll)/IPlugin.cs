// PluginCommon -> Собрать = dll

namespace PluginCommon
{
    public interface IPlugin
    {
        string Name { get; }
        string Execute(string input);
    }
}
