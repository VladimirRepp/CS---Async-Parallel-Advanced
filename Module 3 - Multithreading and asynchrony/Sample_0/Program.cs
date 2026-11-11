// Задание: 
// Запуск отдельного потока 
// Создайте метод, который выводит числа от 1 до 10 с задержкой в 1 секунду. 
// Запустите этот метод в отдельном потоке, одновременно выводя в основном 
// потоке буквы от 'A' до 'E'.

class Program
{
    static void Main()
    {
        // Запуск метода в отдельном потоке
        Thread numbersThread = new Thread(PrintNumbers);
        numbersThread.Start();

        // Работа в основном потоке
        PrintChar();

        // Попробуй закомментировать. Что получится?
        numbersThread.Join(); // Ждем завершения потока с числами
        Console.WriteLine("Все потоки завершили работу.");
         Console.ReadLine();
    }

    static void PrintChar()
    {
        for (char c = 'A'; c <= 'E'; c++)
        {
            Console.WriteLine($"Поток [Main]: {c}");
            Thread.Sleep(800); // Небольшая задержка для наглядности
        }
    }
    
    static void PrintNumbers()
    {
        for (int i = 1; i <= 10; i++)
        {
            Console.WriteLine($"Поток [Numbers]: {i}");
            Thread.Sleep(1000);
        }
    }
}
