namespace Урок_11._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // создание исключения
            var ex = new Exception("Исключение. Очень страшное.");
            throw ex;
        }
    }
}
