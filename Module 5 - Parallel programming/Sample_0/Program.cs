// Задание: 
// Параллельная обработка коллекции с Parallel.ForEach 
// Создайте коллекцию из 20 чисел. Используя Parallel.ForEach, 
// вычислите квадратный корень каждого числа, замерьте время 
// выполнения и сравните с обычным foreach.

// Для маленькой коллекции разница может быть незначительной из-за
// накладных расходов, но принцип демонстрируется.

class Program
{
    static void Main()
    {
        var numbers = Enumerable.Range(1, 20).ToArray();

        // Последовательная обработка
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var resultsSequential = new double[numbers.Length];

        for (int i = 0; i < numbers.Length; i++)
        {
            resultsSequential[i] = HeavyMathOperation(numbers[i]);
        }

        stopwatch.Stop();
        Console.WriteLine($"Последовательная обработка: {stopwatch.ElapsedMilliseconds} мс");

        // Параллельная обработка
        stopwatch.Restart();
        var resultsParallel = new double[numbers.Length];

        Parallel.ForEach(numbers, (number, state, index) =>
        {
            resultsParallel[index] = HeavyMathOperation(number);
        });

        stopwatch.Stop();
        Console.WriteLine($"Параллельная обработка: {stopwatch.ElapsedMilliseconds} мс");

        // Проверка результатов
        Console.WriteLine("Результаты совпадают: " +
            resultsSequential.SequenceEqual(resultsParallel));
    }

    static double HeavyMathOperation(int number)
    {
        // Имитация тяжелой математической операции
        Thread.Sleep(100);
        return Math.Sqrt(number);
    }
}
