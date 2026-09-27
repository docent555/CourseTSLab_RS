using System.Diagnostics.CodeAnalysis;

namespace Урок_8._2
{
    internal class Program
    {
        // Описали делегат. Создали новый тип данных.
        delegate double SomeDelegate(double first, int second);
        static event SomeDelegate SomeEvent;

        static void Main(string[] args)
        {
            // Делегаты
            SomeDelegate d0 = new SomeDelegate(Sum);

            d0 += Sub;

            d0(10, 8);

            SomeEvent += Sum;
            SomeEvent += Sub;

            SomeEvent(100, 500);

            // анонимная функция
            d0 = delegate(double f, int s) { return f/s; };

            Console.WriteLine(d0(10,3));

            // лямбда оператор
            d0 = (f,s) => f/s;
            // лямбда выражение
            d0 = (f, s) => { return f / s; };

            // примеры лямбд и анонимных методов
            var x = 10;
            d0 = (f, s) => f * x / s;
        }        

        static double Sum(double f, int s)
        {
            Console.WriteLine(f + s);
            return f + s;            
        }

        static double Sub(double f, int s)
        {
            Console.WriteLine(f - s);
            return f - s;            
        }
    }
}
