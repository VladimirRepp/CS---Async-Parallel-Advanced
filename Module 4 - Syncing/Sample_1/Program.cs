// Задание: 
// Использование Mutex для межпроцессной синхронизации 
// Создайте приложение, которое может быть запущено только в одном экземпляре. 
// Используйте Mutex с глобальным именем.

class Program
{
    static void Main()
    {
        // Создаем именованный Mutex
        using var mutex = new Mutex(initiallyOwned: true, name: "MySingleInstanceApp", out bool createdNew);

        if (!createdNew)
        {
            Console.WriteLine("Приложение уже запущено! Завершаем...");
            Thread.Sleep(2000); // Чтобы увидеть сообщение
            return;
        }

        Console.WriteLine("Приложение запущено. Нажмите Enter для выхода...");
        Console.WriteLine("Запустите экземпляра приложения (.exe) для тестов блокировки");
        Console.ReadLine();

        // Mutex будет освобожден автоматически при Dispose (благодаря using)
    }
}
