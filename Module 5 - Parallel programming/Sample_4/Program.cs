// Задание: 
// Параллельная агрегация с Parallel.For и локальным хранилищем 
// Вычислите сумму квадратов большого массива чисел, используя локальное 
// хранилище потоков для уменьшения конфликтов.

// Локальное хранилище значительно уменьшает конкуренцию (конфликт за ресурсы).

class Program
{
    static void Main()
    {
        var numbers = Enumerable.Range(1, 1_000_000).ToArray();

        long totalSum = 0;
        object lockObject = new object();

        // Простой способ (с конфликтами)
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        Parallel.ForEach(numbers, number =>
        {
            var square = number * number;
            lock (lockObject)
            {
                totalSum += square;
            }
        });
        stopwatch.Stop();
        Console.WriteLine($"С блокировкой: {stopwatch.ElapsedMilliseconds} мс, сумма: {totalSum}");

        // Оптимизированный способ с локальным хранилищем
        stopwatch.Restart();
        totalSum = 0;

        Parallel.ForEach(
            source: numbers,
            localInit: () => 0L, // Локальная сумма для каждого потока
            body: (number, state, localSum) => localSum + (number * number),
            localFinally: (localSum) =>
            {
                // Атомарно добавляем локальную сумму к общей
                Interlocked.Add(ref totalSum, localSum);
            }
        );

        stopwatch.Stop();
        Console.WriteLine($"С локальным хранилищем: {stopwatch.ElapsedMilliseconds} мс, сумма: {totalSum}");
    }
}
