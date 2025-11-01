// Задание: 
// Синхронизация с помощью lock 
// Создайте 10 потоков, каждый из которых увеличивает общий счетчик 1000 раз. 
// Убедитесь, что без lock результат меньше 10000, а с lock — всегда ровно 10000.

class Program
{
    private static int _counterWithoutLock = 0;
    private static int _counterWithLock = 0;
    private static readonly object _lockObject = new object();

    static void Main()
    {
        // Тест без блокировки
        RunTest("БЕЗ блокировки", IncrementUnsafe);
        Console.WriteLine($"Результат: {_counterWithoutLock} (ожидалось 10000)");

        // Тест с блокировкой
        RunTest("С блокировкой", IncrementSafe);
        Console.WriteLine($"Результат: {_counterWithLock} (ожидалось 10000)");
    }

    static void RunTest(string testName, Action incrementAction)
    {
        Console.WriteLine($"\n--- Тест: {testName} ---");
        Thread[] threads = new Thread[10];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() => {
                for (int j = 0; j < 1000; j++)
                {
                    incrementAction();
                }
            });
            threads[i].Start();
        }

        // Ждем завершения всех потоков
        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    static void IncrementUnsafe()
    {
        _counterWithoutLock++;
    }

    static void IncrementSafe()
    {
        lock (_lockObject) // Критическая секция
        {
            _counterWithLock++;
        }
    }
}
