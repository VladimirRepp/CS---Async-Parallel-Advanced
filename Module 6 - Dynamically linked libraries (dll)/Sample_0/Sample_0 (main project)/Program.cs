using System.Reflection;

class Program
{
    static void Main()
    {
        try
        {
            // Загружаем сборку (предполагается, что DLL в папке с exe)
            Assembly assembly = Assembly.LoadFrom("MathOperations.dll");
            Console.WriteLine($"Загружена сборка: {assembly.FullName}");

            // Получаем тип Calculator
            Type calculatorType = assembly.GetType("MathOperations.Calculator");
            if (calculatorType == null)
            {
                Console.WriteLine("Тип Calculator не найден!");
                return;
            }

            // Создаем экземпляр
            object calculatorInstance = Activator.CreateInstance(calculatorType);

            // Вызываем методы через рефлексию
            MethodInfo addMethod = calculatorType.GetMethod("Add");
            MethodInfo getInfoMethod = calculatorType.GetMethod("GetInfo");

            object result = addMethod.Invoke(calculatorInstance, new object[] { 5, 3 });
            object info = getInfoMethod.Invoke(calculatorInstance, null);

            Console.WriteLine($"5 + 3 = {result}");
            Console.WriteLine($"Info: {info}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
