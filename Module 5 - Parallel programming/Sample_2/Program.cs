// Задание: 
// Параллельное выполнение независимых задач с Parallel.Invoke 
// Создайте 5 различных методов, каждый из которых выполняет 
// разную операцию (вывод в консоль, работу с файлом, вычисления). 
// Запустите их параллельно с помощью Parallel.Invoke.

class Program
{
    static void Main()
    {
        Console.WriteLine("Начало параллельного выполнения...");

        Parallel.Invoke(
            () => WriteToConsole("Задача 1"),
            () => CreateFile("test_parallel.txt"),
            () => CalculateSum(1, 1000),
            () => WriteToConsole("Задача 4"),
            () => SimulateNetworkRequest()
        );

        Console.WriteLine("Все задачи завершены.");
    }

    static void WriteToConsole(string message)
    {
        Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] {message}");
        Thread.Sleep(1000);
    }

    static void CreateFile(string filename)
    {
        File.WriteAllText(filename, $"Создано в потоке {Thread.CurrentThread.ManagedThreadId} в {DateTime.Now}");
        Console.WriteLine($"Файл {filename} создан");
        Thread.Sleep(1500);
    }

    static void CalculateSum(int start, int end)
    {
        long sum = 0;
        for (int i = start; i <= end; i++) sum += i;
        Console.WriteLine($"Сумма от {start} до {end} = {sum}");
        Thread.Sleep(800);
    }

    static void SimulateNetworkRequest()
    {
        Console.WriteLine("Имитация сетевого запроса...");
        Thread.Sleep(2000);
        Console.WriteLine("Сетевой запрос завершен");
    }
}
