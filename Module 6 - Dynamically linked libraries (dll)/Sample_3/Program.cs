// Задание:
// Загрузка Native библиотек через P/Invoke
// Продемонстрируйте вызов функции из native DLL (например, из kernel32.dll) с помощью P/Invoke.

using System;
using System.Runtime.InteropServices;

class Program
{
    // Импорт функции из kernel32.dll
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool Beep(uint dwFreq, uint dwDuration);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    static void Main()
    {
        // Пример 1: Получение handle модуля
        IntPtr handle = GetModuleHandle(null);
        Console.WriteLine($"Handle текущего модуля: {handle}");

        // Пример 2: Звуковой сигнал
        Console.WriteLine("Воспроизводим звук...");
        Beep(1000, 500); // 1000 Hz, 500 ms

        // Пример 3: MessageBox
        MessageBox(IntPtr.Zero, "Hello from P/Invoke!", "Native Call", 0);

        Console.WriteLine("Нажмите Enter для выхода...");
        Console.ReadLine();
    }
}
