using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Samples
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // ================= Вспомогательный пример ================= //
            (total_amount, elepsedMilliseconds) = CaclulateTest();
            (long, long) resultTest = CaclulateTest();
            // ================= Вспомогательный пример ================= //
            
            Console.WriteLine("=== Сравнение CPU-bound и I/O-bound операций ===\n");

            // Демонстрация информации о потоках
            PrintAvialableThreadInfo();

            // ========== CPU-BOUND операции ==========
            Console.WriteLine("\n--- CPU-bound операции (вычисления) ---");
            
            // Синхронная версия
            CaclulateSynchron(out long syncTotal, out long syncTime);
            Console.WriteLine($"Синхронно: {syncTotal} за {syncTime}мс");

            // Через Thread
            CaclulateWithThreads(out long threadTotal, out long threadTime);
            Console.WriteLine($"Через Thread: {threadTotal} за {threadTime}мс");

            // Через Task (правильно для CPU-bound)
            var taskResult = await CaclulateWithTasks();
            Console.WriteLine($"Через Task (CPU-bound): {taskResult.total} за {taskResult.time}мс");

            // ========== I/O-BOUND операции ==========
            Console.WriteLine("\n--- I/O-bound операции (ожидание) ---");
            
            // Имитация I/O операций
            var ioResult = await SimulateIoOperations();
            Console.WriteLine($"I/O операции: {ioResult.total} файлов за {ioResult.time}мс");

            // Сравнение производительности
            Console.WriteLine($"\n--- Сравнение ---");
            Console.WriteLine($"Ускорение Thread vs Синхронно: {(double)syncTime/threadTime:F2}x");
            Console.WriteLine($"Ускорение Task vs Синхронно: {(double)syncTime/taskResult.time:F2}x");

            Console.ReadLine();
        }

        static void PrintAvialableThreadInfo()
        {
            ThreadPool.GetAvailableThreads(out int workerAvailable, out int completionPortAvailable);
            ThreadPool.GetMaxThreads(out int workerMax, out int completionPortMax);
            ThreadPool.GetMinThreads(out int workerMin, out int completionPortMin);

            Console.WriteLine($"Доступно потоков: {workerAvailable}/{workerMax}");
            Console.WriteLine($"Минимум потоков: {workerMin}");
        }

        #region === CPU-bound операции (вычисления) ===

        static void CaclulateSynchron(out long total_amount, out long elepsedMilliseconds)
        {
            var stopwatch = Stopwatch.StartNew();
            total_amount = 0;

            for (long i = 1; i <= 1_000_000; i++)
            {
                total_amount += i * i; // CPU-intensive calculation
            }

            stopwatch.Stop();
            elepsedMilliseconds = stopwatch.ElapsedMilliseconds;
        }

        static void CaclulateWithThreads(out long total_amount, out long elepsedMilliseconds)
        {
            var stopwatch = Stopwatch.StartNew();

            long amount_1 = 0, amount_2 = 0, amount_3 = 0, amount_4 = 0;

            Thread t1 = new Thread(() => amount_1 = FirstDivision());
            Thread t2 = new Thread(() => amount_2 = SecondDivision());
            Thread t3 = new Thread(() => amount_3 = ThirdDivision());
            Thread t4 = new Thread(() => amount_4 = FourthDivision());

            t1.Start(); t2.Start(); t3.Start(); t4.Start();
            t1.Join(); t2.Join(); t3.Join(); t4.Join();

            total_amount = amount_1 + amount_2 + amount_3 + amount_4;
            stopwatch.Stop();
            elepsedMilliseconds = stopwatch.ElapsedMilliseconds;
        }

        static async Task<(long total, long time)> CaclulateWithTasks()
        {
            var stopwatch = Stopwatch.StartNew();

            // ПРАВИЛЬНО для CPU-bound: используем Task.Run для вычислений
            Task<long> t1 = Task.Run(FirstDivision);
            Task<long> t2 = Task.Run(SecondDivision);
            Task<long> t3 = Task.Run(ThirdDivision);
            Task<long> t4 = Task.Run(FourthDivision);

            await Task.WhenAll(t1, t2, t3, t4);

            long total = t1.Result + t2.Result + t3.Result + t4.Result;
            stopwatch.Stop();

            return (total, stopwatch.ElapsedMilliseconds);
        }

        #endregion

        #region === I/O-bound операции (ожидание) ===

        static async Task<(int total, long time)> SimulateIoOperations()
        {
            var stopwatch = Stopwatch.StartNew();

            // ПРАВИЛЬНО для I/O-bound: используем чистый async/await
            // без Task.Run, так как операции сами являются асинхронными
            
            Task<string> fileReadTask = SimulateFileReadAsync();
            Task<string> networkRequestTask = SimulateNetworkRequestAsync();
            Task<string> databaseQueryTask = SimulateDatabaseQueryAsync();

            // Все операции работают параллельно без блокировки потоков
            await Task.WhenAll(fileReadTask, networkRequestTask, databaseQueryTask);

            stopwatch.Stop();
            return (3, stopwatch.ElapsedMilliseconds); // 3 операции завершены
        }

        static async Task<string> SimulateFileReadAsync()
        {
            // Имитация чтения файла - I/O операция
            await Task.Delay(500); // Ожидание диска
            return "file content";
        }

        static async Task<string> SimulateNetworkRequestAsync()
        {
            // Имитация сетевого запроса - I/O операция  
            await Task.Delay(800); // Ожидание сети
            return "network response";
        }

        static async Task<string> SimulateDatabaseQueryAsync()
        {
            // Имитация запроса к БД - I/O операция
            await Task.Delay(600); // Ожидание БД
            return "db result";
        }

        #endregion

        #region === Вспомогательные методы для вычислений ===

        static long FirstDivision() => CalculateRange(1, 250_000);
        static long SecondDivision() => CalculateRange(250_001, 500_000);
        static long ThirdDivision() => CalculateRange(500_001, 750_000);
        static long FourthDivision() => CalculateRange(750_001, 1_000_000);

        static long CalculateRange(long start, long end)
        {
            long amount = 0;
            for (long i = start; i <= end; i++)
            {
                amount += i * i;
            }
            return amount;
        }

        #endregion
    }
}
