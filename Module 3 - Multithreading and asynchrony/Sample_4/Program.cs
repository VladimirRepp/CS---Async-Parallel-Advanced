// Задание: 
// Отмена асинхронной операции 
// Создайте задачу, которая имитирует длительную операцию 
// (цикл на 10 итераций с задержкой). Реализуйте возможность 
// отменить операцию через 3 секунды с помощью CancellationToken.

// CancellationToken в C# — это токен отмены, который используется 
// для отслеживания запроса на отмену асинхронной операции. 
// Он создаётся на основе объекта CancellationTokenSource и 
// передаётся в асинхронные операции, чтобы они могли проверять, 
// была ли запрошена отмена.

class Program
{
    static async Task Main()
    {
        // Источник токена отмены
        using CancellationTokenSource cts = new CancellationTokenSource();

        // Запускаем длительную операцию с возможностью отмены
        var longRunningTask = LongRunningOperation(cts.Token);

        // Ждем 3 секунды и отменяем
        await Task.Delay(3000);
        cts.Cancel();
        Console.WriteLine("Отмена запрошена...");

        try
        {
            await longRunningTask;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Операция была отменена.");
        }
    }

    static async Task LongRunningOperation(CancellationToken cancellationToken)
    {
        for (int i = 1; i <= 10; i++)
        {
            // Проверяем, не запрошена ли отмена
            cancellationToken.ThrowIfCancellationRequested();

            Console.WriteLine($"Итерация {i}");
            await Task.Delay(1000, cancellationToken); // Задержка тоже может быть отменена
        }
        Console.WriteLine("Операция завершена полностью.");
    }
}
