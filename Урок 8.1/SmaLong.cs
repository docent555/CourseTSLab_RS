using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace Урок_8._1
{
    public class SmaLong : IExternalScript
    {
        public void Execute(IContext ctx, ISecurity sec)
        {
            // 1 способ расчета SMA
            var smaHendler = new SMA() { Context = ctx, Period = 12 };

            //var fastSma = smaHendler.Execute(sec.ClosePrices);

            //smaHendler.Period = 24;

            //var slowSma = smaHendler.Execute(sec.ClosePrices);

            // 2 способ расчета SMA
            var fastSma = Series.SMA(sec.ClosePrices, 12);

            var slowSma = Series.SMA(sec.ClosePrices, 24);

            // торговля
            for (int i = 25; i < ctx.BarsCount; i++)
            {
                var le = sec.Positions.GetLastActiveForSignal("LE", i);

                if (le == null)
                {
                    // входим
                    if (fastSma[i] > slowSma[i] && fastSma[i-1] <= slowSma[i - 1])
                    {
                        sec.Positions.BuyAtMarket(i + 1, 1, "LE");
                    }
                }
                else
                {
                    // пробуем выйти
                    if (fastSma[i] < slowSma[i] && fastSma[i - 1] >= slowSma[i - 1])
                    {
                        le.CloseAtMarket(i + 1, "LX");
                    }
                }                
            }

            // вывод на панель
            var pane = ctx.CreatePane(sec.ToString(), 100, false);
            var color = new Color(System.Drawing.Color.Blue.ToArgb());
            var lst = pane.AddList(sec.ToString(), sec, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            for (int i = 0; i < ctx.BarsCount; ++i)
            {
                var pos = sec.Positions.GetActiveForBar(i);
                if (pos.Any())
                {
                    lst.SetColor(i, new Color(System.Drawing.Color.Blue.ToArgb()));
                }
                else
                {
                    lst.SetColor(i, new Color(System.Drawing.Color.Gray.ToArgb()));
                }
            }

            color = new Color(System.Drawing.Color.Red.ToArgb());
            lst = pane.AddList("fastSMA", fastSma, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);

            color = new Color(System.Drawing.Color.DarkGreen.ToArgb());
            lst = pane.AddList("slowSMA", slowSma, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
        }
    }
}
