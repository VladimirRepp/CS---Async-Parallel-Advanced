using PluginCommon;

// 1:
// Зависимости -> Добавить ссылку на проект -> Обзор 
// -> Обзор... -> Выбрать PluginCommon.dll -> ОК

// 2:
// UpperCasePlugin -> Собрать = dll

namespace ReversePlugin
{
    public class ReversePlugin : IPlugin
    {
        public string Name => "Reverse Plugin";

        public string Execute(string input)
        {
            return new string(input.Reverse().ToArray());
        }
    }
}
