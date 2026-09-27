using System;
using System.Collections.Generic;
using System.Linq;
using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace Урок_11._1
{
    /// <summary>
    /// Вспомогательный класс, содержащий хелпер методы. В том числе и методы расширения.
    /// </summary>
    public static class TradeHelper
    {
        /// <summary>
        /// Производит вычитание двух коллекций. 
        /// Из первой вычитает вторую и возвращает коллекцию с элементами равными разности элементов коллекций 1 и 2.
        /// Если колллекции разной длины, то вернет null.
        /// </summary>
        /// <param name="list">Коллекция из которой вычитать</param>
        /// <param name="subtrList">Колллекция которую будет вычитать.</param>
        /// <returns></returns>
        public static IList<double> Subtract(this IList<double> list, IList<double> subtrList)
        {
            // Если длины коллекций различаются то просто вернем null как знак ошибки.
            if (list.Count != subtrList.Count)
                return null;

            // Создаем массив, и забиваем его разностями элементов списков.
            var res = new double[list.Count];
            for (var i = 0; i < list.Count; i++)
                res[i] = list[i] - subtrList[i];


            return res;
        }

        /// <summary>
        /// Производит вычитание двух коллекций. 
        /// Из первой вычитает вторую и возвращает коллекцию с элементами равными разности элементов коллекций 1 и 2.
        /// Если колллекции разной длины, то вернет null.
        /// </summary>
        /// <param name="list">Коллекция из которой вычитать</param>
        /// <param name="subtrList">Колллекция которую будет вычитать.</param>
        /// <returns></returns>
        public static IList<int> Subtract(this IList<int> list, IList<int> subtrList)
        {
            // Если длины коллекций различаются то просто вернем null как знак ошибки.
            if (list.Count != subtrList.Count)
                return null;

            // Создаем массив, и забиваем его разностями элементов списков.
            var res = new int[list.Count];
            for (var i = 0; i < list.Count; i++)
                res[i] = list[i] - subtrList[i];


            return res;
        }

        /// <summary>
        /// Производит сложение двух коллекций. 
        /// К первой коллекции прибавляет вторую и возвращает коллекцию с элементами равными сумме элементов коллекций 1 и 2.
        /// Если колллекции разной длины, то вернет null.
        /// </summary>
        /// <param name="list">Коллекция к которой прибавлять</param>
        /// <param name="subtrList">Колллекция которую будем прибавлять.</param>
        /// <returns></returns>
        public static IList<double> Add(this IList<double> list, IList<double> subtrList)
        {
            // Если длины коллекций различаются то просто вернем null как знак ошибки.
            if (list.Count != subtrList.Count)
                return null;

            // Создаем массив, и забиваем его суммами элементов списков.
            var res = new double[list.Count];
            for (var i = 0; i < list.Count; i++)
                res[i] = list[i] + subtrList[i];


            return res;
        }

        /// <summary>
        /// Производит сложение двух коллекций. 
        /// К первой коллекции прибавляет вторую и возвращает коллекцию с элементами равными сумме элементов коллекций 1 и 2.
        /// Если колллекции разной длины, то вернет null.
        /// </summary>
        /// <param name="list">Коллекция к которой прибавлять</param>
        /// <param name="subtrList">Колллекция которую будем прибавлять.</param>
        /// <returns></returns>
        public static IList<int> Add(this IList<int> list, IList<int> subtrList)
        {
            // Если длины коллекций различаются то просто вернем null как знак ошибки.
            if (list.Count != subtrList.Count)
                return null;

            // Создаем массив, и забиваем его суммами элементов списков.
            var res = new int[list.Count];
            for (var i = 0; i < list.Count; i++)
                res[i] = list[i] + subtrList[i];

            return res;
        }


        /// <summary>
        /// Универсальный метод сложения. Складывает два списка разных объектов. Возвращает список сумм.
        /// </summary>
        /// <typeparam name="TSource">Тип исходного списка.</typeparam>
        /// <param name="list">Список к которому прибавлять</param>
        /// <param name="secList">Список который прибавлять</param>
        /// <param name="selector">Селектор</param>
        /// <returns></returns>
        public static IList<double> Add<TSource>(this IList<TSource> list, IList<TSource> secList, Func<TSource, double> selector)
        {
            // Если длины коллекций различаются то просто вернем null как знак ошибки.
            if (list.Count != secList.Count)
                return null;

            var res = new double[list.Count];
            
            // Заполняем массив суммами, для выбора элемента который суммировать используем селектор.
            // Включаем контроль переполнения типа.
            for (var i = 0; i < list.Count; i++)
                checked
                {
                    res[i] = selector(list[i]) + selector(secList[i]);
                }

            return res;
        }

        /// <summary>
        /// Универсальный метод вычитания. Вычитает два списка разных объектов. Возвращает список разностей.
        /// </summary>
        /// <typeparam name="TSource">Тип исходного списка.</typeparam>
        /// <param name="list">Список из которого вычитать</param>
        /// <param name="secList">Список который вычитать</param>
        /// <param name="selector">Селектор</param>
        /// <returns></returns>
        public static IList<double> Subtract<TSource>(this IList<TSource> list, IList<TSource> secList, Func<TSource, double> selector)
        {
            // Если длины коллекций различаются то просто вернем null как знак ошибки.
            if (list.Count != secList.Count)
                return null;

            var res = new double[list.Count];

            // Заполняем массив разностями, для выбора элемента который вычитать используем селектор.
            // Включаем контроль переполнения типа.
            for (var i = 0; i < list.Count; i++)
                checked
                {
                    res[i] = selector(list[i]) - selector(secList[i]);    
                }
                

            return res;
        }



        /// <summary>
        /// Возвращает истину если свеча растущая, закрытие больше открытия
        /// </summary>
        /// <param name="candle"></param>
        /// <returns></returns>
        public static bool IsWhite(this BaseBar candle)
        {
            return candle.Close > candle.Open;
        }



        /// <summary>
        /// Дает расчет параболического стопа для позиции. Расчет идет по формуле 
        /// stop = pos.EntryPrice - delta + (k * x^2) * step, где x - число бар удержания позы.
        /// </summary>
        /// <param name="pos">Позиция</param>
        /// <param name="bar">Номер бара для которого расчитать параболик</param>
        /// <param name="delta">Смещение стартовой точки стопа вверх или вниз от входа</param>
        /// <param name="k">Коэффициент меняющий вид параболы</param>
        /// <param name="step">Шаг изменения цены стопа</param>
        /// <returns></returns>
        public static double ParabolicStop(this IPosition pos, int bar, double delta, double k, double step)
        {
            var x = bar - pos.EntryBarNum + 1;      // Время удержания позиции
            var y = x * x * k;

            if (pos.IsLong)
                return pos.EntryPrice - delta + y * step;


            return pos.EntryPrice + delta - y * step;
        }

        /// <summary>
        /// Возвращает истину если время удержания позиции равно или больше чем time.
        /// </summary>
        /// <param name="pos">Позиция</param>
        /// <param name="bar">Номер бара для которого считать время удержания</param>
        /// <param name="time">Сколько разрешено держать позици</param>
        /// <returns></returns>
        public static bool TimeStop(this IPosition pos, int bar, TimeSpan time)
        {
            var openDate = pos.EntryBar.Date;
            var currentDate = pos.Security.Bars[bar].Date;
            var diff = currentDate - openDate;

            return (diff >= time);
        }

        public static void LogInfo(this IContext ctx, string str, params object[] args)
        {
            //var color = new Color(System.Drawing.Color.DarkGreen.ToArgb());
            //ctx.Log("Скрипт отработал.", color);

            var msg = string.Format("Info: " + str, args);

            ctx.Log(msg);
        }

        /// <summary>
        /// Возвращает среднюю цену входа для всех позиций из списка.
        /// </summary>
        /// <param name="positions">Спислк позиций для которых расчитать</param>
        /// <returns></returns>
        public static double AvgEntryPrice(this IList<IPosition> positions)
        {
            var totalPrice = positions.Sum(p => p.PositionEntryPrice());
            var totalSize = positions.Sum(p => p.PosSize());

            return totalPrice / totalSize;
        }

        /// <summary>
        /// Полный размер позиции в бумагах. Учитывается размер лота.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static double PosSize(this IPosition pos)
        {
            return pos.Shares * pos.Security.LotSize;
        }

        /// <summary>
        /// Возвращает общую стоимость позиции на момент входа
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static double PositionEntryPrice(this IPosition pos)
        {
            return pos.EntryPrice * pos.PosSize();
        }
    }
}
