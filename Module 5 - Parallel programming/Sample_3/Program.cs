// Задание: 
// Обработка исключений в параллельных операциях 
// Создайте параллельный цикл, в котором некоторые итерации будут 
// выбрасывать исключения. Реализуйте корректную обработку всех 
// исключений с помощью AggregateException.

class Program
{
    static void Main()
    {
        var numbers = Enumerable.Range(-5, 15).ToArray(); // Будем делить на некоторые числа

        try
        {
            Parallel.ForEach(numbers, (number, state) =>
            {
                try
                {
                    // Может вызвать DivideByZeroException
                    var result = 100 / number;
                    Console.WriteLine($"100 / {number} = {result}");
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine($"Ошибка: деление на 0 для числа {number}");
                    // Можно записать информацию об ошибке или использовать state.Stop()
                }
            });
        }
        catch (AggregateException ae)
        {
            Console.WriteLine($"\nПоймано AggregateException. Внутренние исключения:");
            foreach (var ex in ae.InnerExceptions)
            {
                Console.WriteLine($" - {ex.GetType().Name}: {ex.Message}");
            }
        }

        Console.WriteLine("Программа завершена.");
    }
}
