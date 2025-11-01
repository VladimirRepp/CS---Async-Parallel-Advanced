using System.Reflection;

// Задание: 
// Работа с интерфейсами для плагинной архитектуры 
// Создайте общий интерфейс IPlugin в отдельной сборке. 
// Реализуйте этот интерфейс в двух разных плагинах. 
// В основном приложении динамически загрузите и используйте 
// все доступные плагины.

// 1:
// Зависимости -> Добавить ссылку на проект -> Обзор 
// -> Обзор... -> Выбрать PluginCommon.dll (и все остальные) -> ОК

// 2:
// Запустить 

class Program
{
    static void Main()
    {
        // Ищем все DLL в текущей директории
        string[] pluginFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*Plugin.dll");

        var plugins = new List<IPlugin>();

        foreach (string file in pluginFiles)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(file);

                // Ищем типы, реализующие IPlugin
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface)
                    {
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                        plugins.Add(plugin);
                        Console.WriteLine($"Загружен плагин: {plugin.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки {file}: {ex.Message}");
            }
        }

        // Используем плагины
        string testString = "Hello, Plugins!";

        foreach (var plugin in plugins)
        {
            string result = plugin.Execute(testString);
            Console.WriteLine($"{plugin.Name}: '{testString}' -> '{result}'");
        }
    }
}
