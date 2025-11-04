using System.Threading.Tasks;

namespace Samples
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            long total_amount, elepsedMilliseconds;
            
            // ================================================= //
            (total_amount, elepsedMilliseconds) = CaclulateTest();
            (long, long) resultTest = CaclulateTest();
            // ================================================= //

            CaclulateSynchron(out total_amount, out elepsedMilliseconds);
            Console.WriteLine($"CaclulateSynchron: total_amount = {total_amount} | elepsedMilliseconds = {elepsedMilliseconds}");

            CaclulateWithThreads(out total_amount, out elepsedMilliseconds);
            Console.WriteLine($"CaclulateWithThreads: total_amount = {total_amount} | elepsedMilliseconds = {elepsedMilliseconds}");

            Task<(long, long)> taskCaclulate = CaclulateWithTasks();
            await taskCaclulate;
            (total_amount, elepsedMilliseconds) = taskCaclulate.Result;
            Console.WriteLine($"CaclulateWithTasks: total_amount = {total_amount} | elepsedMilliseconds = {elepsedMilliseconds}");
            
            Console.ReadLine();
        }

        static  void PrintAvialableThreadInfo()
        {
            int workerAvailableThreads, completionPortThreads;
            int workerMaxThreads;
            int workerMinThreads;
            ThreadPool.GetAvailableThreads(out workerAvailableThreads, out completionPortThreads);
            ThreadPool.GetMaxThreads(out workerMaxThreads, out completionPortThreads);
            ThreadPool.GetMinThreads(out workerMinThreads, out completionPortThreads);

            Console.WriteLine($"workerAvailableThreads = {workerAvailableThreads}, \n" +
                $"workerMaxThreads = {workerMaxThreads}, \n" +
                $"workerMinThreads = {workerMinThreads}");

            Console.ReadLine();
        }

        #region === Задача - Параллельные вычисления === 

        // Задача:
        // Параллельные вычисления:
        // Создайте метод, вычисляющий сумму квадратов чисел от 1 до 1,000,000.
        // Разбейте диапазон на 4 части и вычислите каждую часть в отдельной задаче
        // (Task), затем суммируйте результаты. Сравните время выполнения с
        // последовательной версией.

        static (long, long) CaclulateTest()
        {
            long total_amount = 0;
            long elepsedMilliseconds = 0;
            return (total_amount, elepsedMilliseconds);
        }

        static void CaclulateSynchron(out long total_amount, out long elepsedMilliseconds)
        {
            // Вместо void можно указать возвращаемый тип (long, long)
            // плохо или хорошо?

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            total_amount = 0;
            // Решение №1 - без потоков 
            for (long i = 1; i <= 1_000_000; i++)
            {
                total_amount += i * i;
            }

            stopwatch.Stop();
            elepsedMilliseconds = stopwatch.ElapsedMilliseconds;
        }

        static void CaclulateWithThreads(out long total_amount, out long elepsedMilliseconds)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            long amount_1 = 0;
            long amount_2 = 0;
            long amount_3 = 0;
            long amount_4 = 0;

            total_amount = 0;
            elepsedMilliseconds = 0;

            // Решение №1 - через потоки 
            Thread firstDivisionThread = new Thread(() =>
            {
                amount_1 = FirstDivision();
            });
            Thread secondDivisionThread = new Thread(() =>
            {
                amount_2 = SecondDivision();
            });
            Thread thirdDivisionThread = new Thread(() =>
            {
                amount_3 = ThirdDivision();
            });
            Thread fourthDivisionThread = new Thread(() =>
            {
                amount_4 = FourthDivision();
            });


            firstDivisionThread.Start();
            secondDivisionThread.Start();
            thirdDivisionThread.Start();
            fourthDivisionThread.Start();

            firstDivisionThread.Join();
            secondDivisionThread.Join();
            thirdDivisionThread.Join();
            fourthDivisionThread.Join();

            total_amount += amount_1 + amount_2 + amount_3 + amount_4;
           
            stopwatch.Stop();
            elepsedMilliseconds = stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Подсчет суммы квадратов с использованием задач (Task)
        /// </summary>
        /// <returns>(total_amount, elepsedMilliseconds)</returns>
        static async Task<(long, long)> CaclulateWithTasks()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            long total_amount = 0;
            long elepsedMilliseconds = 0;

            Task<long> taskFirstDivision = Task.Run(FirstDivision);
            Task<long> taskSecondDivision = Task.Run(SecondDivision);
            Task<long> taskThirdDivision = Task.Run(ThirdDivision);
            Task<long> taskFourthDivision = Task.Run(FourthDivision);

            // Ожидание всех задач:
            // Task.WaitAll(taskFirstDivision, taskSecondDivision, taskThirdDivision, taskFourthDivision); 
            // или
            await Task.WhenAll(taskFirstDivision, taskSecondDivision, taskThirdDivision, taskFourthDivision); // ждем завершения всех задач
           
            // Ожидание самой быстрой задачи:
            // Task.WaitAny(taskFirstDivision, taskSecondDivision, taskThirdDivision, taskFourthDivision);

            // Ожидание конкретной задачи:
            // taskFirstDivision.Wait();
            // или 
            // await taskFirstDivision;

            total_amount = taskFirstDivision.Result + taskSecondDivision.Result +
                           taskThirdDivision.Result + taskFourthDivision.Result;

            stopwatch.Stop();
            elepsedMilliseconds = stopwatch.ElapsedMilliseconds;

            return (total_amount, elepsedMilliseconds);
        }

        static long FirstDivision()
        {
            long amount = 0;
            // 1ый диапазон
            for (long i = 1; i <= 250_000; i++)
            {
                amount += i * i;
            }

            return amount;
        }

        static long SecondDivision()
        {
            long amount = 0;
            // 2ый диапазон
            for (long i = 250_001; i <= 500_000; i++)
            {
                amount += i * i;
            }

            return amount;
        }

        static long ThirdDivision()
        {
            long amount = 0;
            // 3ий диапазон
            for (long i = 500_001; i <= 750_000; i++)
            {
                amount += i * i;
            }

            return amount;
        }

        static long FourthDivision()
        {
            long amount = 0;
            // 4ый диапазон
            for (long i = 750_001; i <= 1_000_000; i++)
            {
                amount += i * i;
            }

            return amount;
        }

        #endregion

    }
}
