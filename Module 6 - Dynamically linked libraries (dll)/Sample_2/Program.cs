// Задание: 
// Динамическая компиляция во время выполнения 
// Создайте простой калькулятор, который компилирует 
// и выполняет C# код, введенный пользователем.

// Для этого нужно добавить пакет: Microsoft.CodeAnalysis.CSharp.Scripting

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Интерактивный C# калькулятор (введите 'exit' для выхода)");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            if (input?.ToLower() == "exit")
                break;

            if (string.IsNullOrWhiteSpace(input))
                continue;

            try
            {
                // Используем C# Script для выполнения кода
                var result = await CSharpScript.EvaluateAsync(input,
                    ScriptOptions.Default.WithImports("System", "System.Math"));

                if (result != null)
                    Console.WriteLine($"= {result}");
            }
            catch (CompilationErrorException ex)
            {
                Console.WriteLine($"Ошибка компиляции: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
