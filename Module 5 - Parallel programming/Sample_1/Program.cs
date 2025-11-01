// Задание: 
// Использование PLINQ для фильтрации и проекции 
// Создайте коллекцию из 1000 чисел. С помощью PLINQ найдите 
// все простые числа и преобразуйте их в строки формата "Простое число: X".

class Program
{
    static void Main()
    {
        var numbers = Enumerable.Range(1, 1000).ToArray();

        // Обычный LINQ
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var primesLinq = numbers.Where(IsPrime)
                               .Select(n => $"Простое число: {n}")
                               .ToArray();
        stopwatch.Stop();
        Console.WriteLine($"LINQ: {stopwatch.ElapsedMilliseconds} мс, найдено: {primesLinq.Length}");

        // PLINQ
        stopwatch.Restart();
        var primesPlinq = numbers.AsParallel()
                                .Where(IsPrime)
                                .Select(n => $"Простое число: {n}")
                                .ToArray();
        stopwatch.Stop();
        Console.WriteLine($"PLINQ: {stopwatch.ElapsedMilliseconds} мс, найдено: {primesPlinq.Length}");

        // Проверка
        Console.WriteLine("Результаты совпадают: " +
            primesLinq.SequenceEqual(primesPlinq));
    }

    static bool IsPrime(int number)
    {
        if (number < 2) 
            return false;
        if (number == 2)
            return true;
        if (number % 2 == 0) 
            return false;

        var boundary = (int)Math.Floor(Math.Sqrt(number));

        for (int i = 3; i <= boundary; i += 2)
        {
            if (number % i == 0)
                return false;
        }

        return true;
    }
}
