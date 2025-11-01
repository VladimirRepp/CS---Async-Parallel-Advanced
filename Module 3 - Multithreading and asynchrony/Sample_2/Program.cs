// Задание: 
// Асинхронная загрузка веб-страниц 
// Используя HttpClient, одновременно загрузите главные страницы 
// трех разных сайтов. Выводите сообщение о завершении каждой загрузки.

class Program
{
    static async Task Main()
    {
        string[] urls = {
            "https://httpbin.org/delay/1", // Сайт, который искусственно задерживает ответ на 1 сек
            "https://httpbin.org/delay/2",
            "https://httpbin.org/delay/1",
            "https://metanit.com/sharp/tutorial/"
        };

        try
        {
            Console.WriteLine("Начало параллельной загрузки...");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Запускаем все задачи одновременно
            Task<string>[] downloadTasks = urls.Select(url => DownloadStringAsync(url)).ToArray();

            // Ожидаем завершения ВСЕХ задач
            string[] pages = await Task.WhenAll(downloadTasks);

            stopwatch.Stop();
            Console.WriteLine($"Все страницы загружены за {stopwatch.ElapsedMilliseconds} мс.");
            Console.WriteLine($"Размеры: {string.Join(", ", pages.Select(p => p.Length))} символов.");

        }
        catch (Exception ex)
        {
            // 403-й код HTTP (Forbidden) означает,
            // что доступ к ресурсу запрещён (даже при аутентификации).   

            Console.WriteLine($"(!!!) Исключение: {ex.Message}");
        }
    }

    static async Task<string> DownloadStringAsync(string url)
    {
        using var client = new HttpClient();
        Console.WriteLine($"Начало загрузки: {url}");
        string content = await client.GetStringAsync(url);
        Console.WriteLine($"Завершена загрузка: {url}");
        return content;
    }
}
