// Задание:
// Создание кастомного AssemblyLoadContext 
// Создайте изолированный контекст для загрузки сборок, 
// который позволит впоследствии выгрузить их из памяти.

using System.Reflection;
using System.Runtime.Loader;

class Program
{
    static void Main()
    {
        // Создаем изолированный контекст
        var loadContext = new AssemblyLoadContext("PluginContext", isCollectible: true);

        try
        {
            // Загружаем сборку в изолированном контексте
            Assembly assembly = loadContext.LoadFromAssemblyPath(Path.GetFullPath("MathOperations.dll"));
            Type calculatorType = assembly.GetType("MathOperations.Calculator");
            object calculator = Activator.CreateInstance(calculatorType);

            // Используем сборку
            MethodInfo method = calculatorType.GetMethod("Add");
            object result = method.Invoke(calculator, new object[] { 10, 20 });
            Console.WriteLine($"Результат: {result}");

            // Сборка загружена в изолированном контексте
            Console.WriteLine($"Сборка загружена в контексте: {AssemblyLoadContext.GetLoadContext(assembly)?.Name}");
        }
        finally
        {
            // Выгружаем контекст и все сборки в нем
            loadContext.Unload();
            Console.WriteLine("Контекст выгружен");

            // Принудительный сборщик мусора для очистки
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        Console.WriteLine("Программа завершена. Сборка должна быть выгружена из памяти.");
    }
}

