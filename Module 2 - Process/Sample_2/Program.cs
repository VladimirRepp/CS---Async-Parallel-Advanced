// Задание: 
// Получение списка всех процессов 
// Выведите в консоль список всех запущенных процессов с их ID и именем. 
// Отсортируйте список по имени процесса

using System.Diagnostics;

var processes = Process.GetProcesses()
                      .OrderBy(p => p.ProcessName);

Console.WriteLine("{0,-10} {1}", "PID", "Name");
Console.WriteLine(new string('-', 50));

foreach (var process in processes)
{
    Console.WriteLine("{0,-10} {1}", process.Id, process.ProcessName);
}
