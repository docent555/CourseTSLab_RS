using System;
using System.Linq;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;
using TSLab.Script.Optimization;

namespace Урок_11._1
{
    public class MultiTrailStop : IExternalScript
    {
        public OptimProperty HighPeriod = new OptimProperty(24, 10, 200, 10);
        public OptimProperty LowPeriod = new OptimProperty(24, 10, 200, 10);
        public OptimProperty Slippage = new OptimProperty(50, 10, 100, 10);
        public OptimProperty StopLoss = new OptimProperty(2, 0.2, 5, 0.5);
        public OptimProperty TrailEnable = new OptimProperty(1.5, 0.2, 5, 0.5);
        public OptimProperty TrailLoss = new OptimProperty(1, 0.2, 5, 0.5);

        public void Execute(IContext ctx, ISecurity sec)
        {
            var highest = ctx.GetData("highest", new string[] { HighPeriod.ToString() }, 
                () => Series.Highest(sec.HighPrices, HighPeriod));
            var lowest = ctx.GetData("lowest", new string[] { LowPeriod.ToString() },
                () => Series.Lowest(sec.LowPrices, LowPeriod));

            var trailHnd = new TrailStop()
            {
                StopLoss = StopLoss,
                TrailEnable = TrailEnable,
                TrailLoss = TrailLoss
            };

            for (var i = 25; i < ctx.BarsCount; i++)
            {
                var le = sec.Positions.GetLastActiveForSignal("LE", i);
                var se = sec.Positions.GetLastActiveForSignal("SE", i);
                var currPos = sec.Positions.GetActiveForBar(i);

                if (le == null) sec.Positions.BuyIfGreater(i + 1, 1, highest[i], Slippage, "LE");
                if (se == null) sec.Positions.SellIfLess(i + 1, 1, lowest[i], Slippage, "SE");

                if (!currPos.Any())
                    continue;

                // выставить стоп
                foreach (var p in currPos)
                {
                    switch (p.EntrySignalName)
                    {
                        case "LE":
                            {
                                //  выставить стоп
                                var x = i - p.EntryBarNum + 1;
                                var y = x * x;
                                var pStop = p.EntryPrice - 500 + y * 1;
                                var tStop = trailHnd.Execute(p, i);
                                if (pStop > tStop)
                                {
                                    p.CloseAtStop(i + 1, pStop, Slippage, "LXPar");
                                }
                                else
                                {
                                    p.CloseAtStop(i + 1, tStop, Slippage, "LXTrail");
                                }
                            }
                            break;

                        case "SE":
                            {
                                //  выставить стоп
                                var x = i - p.EntryBarNum + 1;
                                var y = x * x;
                                var pStop = p.EntryPrice + 500 - y * 1;
                                var tStop = trailHnd.Execute(p, i);
                                if (pStop < tStop)
                                {
                                    p.CloseAtStop(i + 1, pStop, Slippage, "SXPar");
                                }
                                else
                                {
                                    p.CloseAtStop(i + 1, tStop, Slippage, "SXTrail");
                                }
                            }
                            break;
                    }
                }
            }

            if (ctx.IsOptimization) return;

            // вывод на панель
            var pane = ctx.CreatePane(sec.ToString(), 100, false);
            var color = new Color(System.Drawing.Color.Blue.ToArgb());
            var lst = pane.AddList(sec.ToString(), sec, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            color = new Color(System.Drawing.Color.DarkGreen.ToArgb());
            lst = pane.AddList("highest", highest, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
            lst.Thickness = 2;

            color = new Color(System.Drawing.Color.DarkRed.ToArgb());
            lst = pane.AddList("lowest", lowest, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
            lst.Thickness = 2;
        }
    }
}
