// Задание: 
// Запуск калькулятора и ожидание завершения 
// Напишите программу, которая запускает calc.exe и ждет, 
// пока пользователь его закроет, после чего выводит 
// сообщение "Калькулятор закрыт".

using System.Diagnostics;

try
{
    ProcessStartInfo startInfo = new ProcessStartInfo("calc.exe");
    using Process process = Process.Start(startInfo);
    Console.WriteLine("Калькулятор запущен...");

    process.WaitForExit(); // Бесконечно ждем завершения

    // Калькулятор не простое приложение в Win 10/11.
    // Оно запускается через другие приложени и 
    // может продолжить работать после закрытия. 
    // По этому его завершение не так просто отследить,
    // пока что оставим это так 
    Console.WriteLine("Калькулятор закрыт. Код выхода: " + process.ExitCode);
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
