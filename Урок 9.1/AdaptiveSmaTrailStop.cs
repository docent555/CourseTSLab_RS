using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;
using TSLab.Script.Optimization;

namespace Урок_11._1
{
    public class AdaptiveSmaTrailStop : IExternalScript
    {
        public OptimProperty HighPeriod = new OptimProperty(24, 10, 200, 10);
        public OptimProperty StopSmaPeriod = new OptimProperty(20, 10, 300, 20);
        public OptimProperty Slippage = new OptimProperty(50, 10, 100, 10);

        public void Execute(IContext ctx, ISecurity sec)
        {
            var highest = ctx.GetData("highest", new string[] { HighPeriod.ToString() }, 
                () => Series.Highest(sec.HighPrices, HighPeriod));
            var stopSma = ctx.GetData("SMA", new string[] { StopSmaPeriod.ToString() }, 
                () => Series.SMA(sec.ClosePrices, StopSmaPeriod));

            for (var i = 0; i < ctx.BarsCount; i++)
            {
                var le = sec.Positions.GetLastActiveForSignal("LE", i);                

                if (le == null)
                {
                    if (sec.ClosePrices[i] > stopSma[i])
                    {
                        sec.Positions.BuyIfGreater(i + 1, 1, highest[i], Slippage, "LE");
                    }                    
                }
                else
                {
                    //  выставить стоп
                    var passed = i - le.EntryBarNum + 1;
                    var smaPeriod = StopSmaPeriod - passed;
                    var adoptSma = ctx.GetData("SMA", new string[] { smaPeriod.ToString() },
                        () => Series.SMA(sec.ClosePrices, smaPeriod));
                    var stop = adoptSma[i];
                    le.CloseAtStop(i + 1, stop, Slippage, "LXS");
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

            color = new Color(System.Drawing.Color.Red.ToArgb());
            lst = pane.AddList("stopSma", stopSma, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
            lst.Thickness = 1;
        }
    }
}
