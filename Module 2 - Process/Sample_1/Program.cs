// Задание: 
// Запуск PowerShell и выполнение команды 
// Запустите процесс pwsh (PowerShell Core) или powershell,
// выполните команду Get-Date и прочитайте вывод.

using System.Diagnostics;

ProcessStartInfo startInfo = new ProcessStartInfo
{
    FileName = "powershell", // "pwsh" или "powershell"
    Arguments = "-Command \"Get-Date\"",
    UseShellExecute = false, // Важно для перенаправления вывода!
    RedirectStandardOutput = true,
    CreateNoWindow = true
};

using Process process = Process.Start(startInfo);
string output = process.StandardOutput.ReadToEnd();
process.WaitForExit();

Console.WriteLine("Вывод PowerShell:");
Console.WriteLine(output);