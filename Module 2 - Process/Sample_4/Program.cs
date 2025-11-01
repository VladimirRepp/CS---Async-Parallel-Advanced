// Задание: 
// "Охотник за процессами" 
// Создайте консольное приложение, которое принимает имя процесса 
// в качестве аргумента командной строки и "убивает" все его экземпляры.

using System.Diagnostics;

string processName;

Console.WriteLine("Укажите имя процесса. Пример: ProcessHunter notepad");
processName = Console.ReadLine();

Process[] processes = Process.GetProcessesByName(processName);

if (processes.Length == 0)
{
    Console.WriteLine($"Процессы с именем '{processName}' не найдены.");
    return;
}

Console.WriteLine($"Найдено процессов: {processes.Length}. Завершение...");

foreach (Process process in processes)
{
    try
    {
        process.Kill();
        process.WaitForExit(); // Даем процессу время корректно завершиться
        Console.WriteLine($"Процесс {process.ProcessName} (ID: {process.Id}) завершен.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Не удалось завершить процесс {process.Id}: {ex.Message}");
    }
}
