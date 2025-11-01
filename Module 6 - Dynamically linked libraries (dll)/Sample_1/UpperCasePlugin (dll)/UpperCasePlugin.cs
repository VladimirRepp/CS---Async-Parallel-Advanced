using PluginCommon;

// 1:
// Зависимости -> Добавить ссылку на проект -> Обзор 
// -> Обзор... -> Выбрать PluginCommon.dll -> ОК

// 2:
// UpperCasePlugin -> Собрать = dll


namespace UpperCasePlugin
{
    public class UpperCasePlugin : IPlugin
    {
        public string Name => "UpperCase Plugin";

        public string Execute(string input)
        {
            return input.ToUpper();
        }
    }
}
