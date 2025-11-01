// Задание: 
// Мониторинг конкретного процесса 
// Напишите программу, которая каждые 2 секунды проверяет, 
// запущен ли Блокнот (notepad.exe), и выводит сообщение о его состоянии.

using System.Diagnostics;

string processName = "notepad";

while (true)
{
    Process[] notepads = Process.GetProcessesByName(processName);
    if (notepads.Length > 0)
    {
        Console.WriteLine($"{DateTime.Now}: Блокнот запущен. Количество процессов: {notepads.Length}");
    }
    else
    {
        Console.WriteLine($"{DateTime.Now}: Блокнот не запущен.");
    }
    Thread.Sleep(2000); // Пауза 2 секунды
}
