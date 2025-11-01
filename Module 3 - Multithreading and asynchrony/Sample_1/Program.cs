// Задание: 
// Использование Task для CPU-bound операции (требующие CPU вычислений) 
// Создайте метод, который имитирует сложное вычисление 
// (например, через Thread.Sleep). Запустите 3 таких задачи 
// параллельно с помощью Task.Run и дождитесь их завершения.

class Program
{
    static async Task Main() // Main может быть async в C# 7.1+
    {
        Console.WriteLine("Запуск тяжелых задач...");

        // Запускаем задачи (они планируются в пул потоков)
        Task task1 = Task.Run(HeavyCalculation);
        Task task2 = Task.Run(HeavyCalculation);
        Task task3 = Task.Run(HeavyCalculation);

        // Ожидаем завершения всех задач
        await Task.WhenAll(task1, task2, task3);

        Console.WriteLine("Все задачи завершены!");
    }

    static void HeavyCalculation()
    {
        Console.WriteLine($"Задача {Task.CurrentId} начала работу в потоке {Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(2000); // Имитация долгой работы (2 сек)
        Console.WriteLine($"Задача {Task.CurrentId} завершила работу.");
    }
}
