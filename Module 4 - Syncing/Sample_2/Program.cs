// Задание: 
// ReaderWriterLockSlim для разделения чтения/записи 
// Создайте общий кэш (например, Dictionary). Эмулируйте 5 "читателей", 
// которые часто обращаются к кэшу, и 2 "писателей", которые редко его обновляют. 
// Используйте ReaderWriterLockSlim.

// Обратите внимание, что несколько "читателей" могут работать одновременно.

class Program
{
    private static readonly Dictionary<int, string> _cache = new();
    private static readonly ReaderWriterLockSlim _cacheLock = new();
    private static int _readCount = 0;
    private static int _writeCount = 0;

    static async Task Main()
    {
        // Инициализируем кэш
        _cache[1] = "Value1";

        // Запускаем читателей
        var readerTasks = Enumerable.Range(1, 5)
            .Select(i => Task.Run(() => Reader(i))).ToArray();

        // Запускаем писателей
        var writerTasks = Enumerable.Range(1, 2)
            .Select(i => Task.Run(() => Writer(i))).ToArray();

        await Task.Delay(5000); // Даем поработать 5 секунд

        Console.WriteLine($"\nИтоги: Прочитано {_readCount} раз, Записано {_writeCount} раз");
    }

    static void Reader(int readerId)
    {
        while (true)
        {
            _cacheLock.EnterReadLock();
            try
            {
                // Имитируем чтение
                foreach (var item in _cache)
                {
                    Thread.Sleep(10); // Короткая задержка
                }
                Interlocked.Increment(ref _readCount);
                Console.WriteLine($"Читатель {readerId} прочитал кэш (всего записей: {_cache.Count})");
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
            Thread.Sleep(100); // Пауза между чтениями
        }
    }

    static void Writer(int writerId)
    {
        var random = new Random();
        while (true)
        {
            Thread.Sleep(1000); // Писатели работают реже

            _cacheLock.EnterWriteLock();
            try
            {
                int key = random.Next(1, 10);
                _cache[key] = $"Value{key} от писателя {writerId}";
                Interlocked.Increment(ref _writeCount);
                Console.WriteLine($"Писатель {writerId} обновил запись {key}");
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }
    }
}
