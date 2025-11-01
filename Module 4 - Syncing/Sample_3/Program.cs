// Задание: 
// Producer - Consumer с BlockingCollection 
// Реализуйте паттерн "Producer-Consumer": один поток производит 
// данные (числа), а два потока потребляют их. Используйте 
// BlockingCollection<T> для безопасной передачи данных.

using System.Collections.Concurrent;

class Program
{
    static async Task Main()
    {
        // Коллекция с ограничением вместимости (capacity)
        using var queue = new BlockingCollection<int>(boundedCapacity: 5);

        // Задача-производитель
        var producer = Task.Run(() =>
        {
            for (int i = 1; i <= 10; i++)
            {
                queue.Add(i); // Блокируется, если коллекция полна
                Console.WriteLine($"Произведено: {i}");
                Thread.Sleep(200);
            }
            queue.CompleteAdding(); // Сигнал о завершении производства
        });

        // Задачи-потребители
        var consumer1 = Task.Run(() => Consumer(queue, 1));
        var consumer2 = Task.Run(() => Consumer(queue, 2));

        await Task.WhenAll(producer, consumer1, consumer2);
        Console.WriteLine("Все задачи завершены.");
    }

    static void Consumer(BlockingCollection<int> queue, int consumerId)
    {
        // GetConsumingEnumerable() блокируется, пока есть элементы или не вызван CompleteAdding()
        foreach (var item in queue.GetConsumingEnumerable())
        {
            Console.WriteLine($"Потребитель {consumerId} обработал: {item}");
            Thread.Sleep(500); // Имитация обработки
        }
        Console.WriteLine($"Потребитель {consumerId} завершил работу.");
    }
}
