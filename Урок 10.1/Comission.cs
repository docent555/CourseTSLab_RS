using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace Урок_11._1
{
    public class Comission : IExternalScript
    {
        public void Execute(IContext ctx, ISecurity sec)
        {
            // кубик комиссии
            var comisHnd = new AbsolutCommission() { Commission = 0 };
            comisHnd.Execute(sec);

            //комиссия через АПИ
            //sec.Commission = (pos, price, shares, isEntry, isPart) =>
            //{
            //    if (isEntry)
            //        return 7;

            //    return shares * 0;
            //};

            sec.Commission = (pos, price, shares, isEntry, isPart) =>
            {
                if (pos.EntrySignalName == "LE")
                    return 5;

                // КАТЕГОРИЧЕСКИ НЕЛЬЗЯ ВЫЗЫВАТЬ ФУНКЦИИ pos.Profit(), pos.ProfitPct() т.к. они запрашивают вычисление комиссии и будет зацикливание.
                return shares * 10;
            };

            for (int i = 0; i < ctx.BarsCount; i++)
            {
                if (i == 10)
                {
                    sec.Positions.BuyAtMarket(i + 1, 1, "LE");
                }

                if (i == 20)
                {
                    var le = sec.Positions.GetLastActiveForSignal("LE", i);
                    le.CloseAtMarket(i + 1, "LX");
                }

                if (i == 100)
                {
                    sec.Positions.BuyAtMarket(i + 1, 1, "LE");
                }
            }

            #region Визуализация
            if (ctx.IsOptimization) return;

            // вывод на панель
            var pane = ctx.CreatePane(sec.ToString(), 100, false);
            var color = new Color(System.Drawing.Color.Blue.ToArgb());
            var lst = pane.AddList(sec.ToString(), sec, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);
            #endregion
        }
    }
}
