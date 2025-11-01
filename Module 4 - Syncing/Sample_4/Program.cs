// Задание: 
// Синхронизация с SemaphoreSlim в async методе 
// Создайте лимит на выполнение не более 3 тяжелых асинхронных 
// операций одновременно. Используйте SemaphoreSlim с async методами.

// Обратите внимание, что одновременно обрабатываются не более 3 элементов.

class Program
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3, 3); // Не более 3 одновременно

    static async Task Main()
    {
        var tasks = Enumerable.Range(1, 10)
            .Select(i => ProcessItemAsync(i))
            .ToArray();

        await Task.WhenAll(tasks);
        Console.WriteLine("Все элементы обработаны.");
    }

    static async Task ProcessItemAsync(int itemId)
    {
        Console.WriteLine($"Элемент {itemId} ждет разрешения...");

        await _semaphore.WaitAsync(); // Асинхронное ожидание
        try
        {
            Console.WriteLine($"Элемент {itemId} начал обработку. Свободных слотов: {_semaphore.CurrentCount}");

            // Имитация асинхронной I/O операции
            await Task.Delay(2000);

            Console.WriteLine($"Элемент {itemId} завершил обработку.");
        }
        finally
        {
            _semaphore.Release(); // Освобождаем слот
        }
    }
}
