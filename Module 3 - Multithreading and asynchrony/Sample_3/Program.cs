// Задание: 
// Обработка задач по мере завершения 
// Измените Задание 3 так, чтобы выводить сообщение и обрабатывать 
// результат каждой задачи сразу по мере ее завершения, а не ждать все сразу.

class Program
{
    static async Task Main()
    {
        string[] urls = {
            "https://httpbin.org/delay/3",
            "https://httpbin.org/delay/1",
            "https://httpbin.org/delay/2"
            // "https://metanit.com/sharp/tutorial/"
        };

        Console.WriteLine("Начало загрузки...");

        try
        {
            // Создаем список задач
            var downloadTasks = urls.Select(url => DownloadStringAsync(url)).ToList();

            // Обрабатываем задачи по мере завершения
            while (downloadTasks.Count > 0)
            {
                Task<string> completedTask = await Task.WhenAny(downloadTasks); // ожидание наиболее быстрого
                downloadTasks.Remove(completedTask);

                string content = await completedTask; // Уже завершена, await мгновенный
                Console.WriteLine($"--> Загружено: {content.Length} символов с {completedTask.AsyncState}");
            }
        }
        catch (Exception ex)
        {
            // 403-й код HTTP (Forbidden) означает,
            // что доступ к ресурсу запрещён (даже при аутентификации).   

            Console.WriteLine($"(!!!) Исключение: {ex.Message}");
        }
        
        Console.WriteLine("Все загрузки обработаны.");
    }

    static async Task<string> DownloadStringAsync(string url)
    {
        using var client = new HttpClient();
        string result = await client.GetStringAsync(url);

        Console.WriteLine($"Обработан URL: {url}");
        return result;
    }
}
