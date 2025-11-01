// MathOperations -> Собрать = DLL
namespace MathOperations
{
    public class Calculator
    {
        public int Add(int a, int b) => a + b;
        public int Multiply(int a, int b) => a * b;

        public string GetInfo()
        {
            return $"Calculator v1.0, работающий в {Environment.Version}";
        }
    }
}
