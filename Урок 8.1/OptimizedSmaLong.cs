using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;
using TSLab.Script.Optimization;

namespace Урок_8._1
{
    public class OptimizedSmaLong : IExternalScript
    {
        public OptimProperty fastSmaPeriod = new OptimProperty(12, 6, 100, 6);
        public OptimProperty slowSmaPeriod = new OptimProperty(24, 6, 100, 6);

        public void Execute(IContext ctx, ISecurity sec)
        {
            if (fastSmaPeriod >= slowSmaPeriod) return;

            //var fastSma = Series.SMA(sec.ClosePrices, fastSmaPeriod);

            //var slowSma = Series.SMA(sec.ClosePrices, slowSmaPeriod);

            var fastSma = ctx.GetData("SMA", new string[] { fastSmaPeriod.ToString() }, () => Series.SMA(sec.ClosePrices, fastSmaPeriod));
            var slowSma = ctx.GetData("SMA", new string[] { slowSmaPeriod.ToString() }, () => Series.SMA(sec.ClosePrices, slowSmaPeriod));

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

            // отсечка графики
            if (ctx.IsOptimization) return;

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
