using System.Linq;
using System.Xml.Schema;
using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Optimization;

namespace Урок_11._1
{
    public class Compression : IExternalScript
    {
        public OptimProperty Mult = new OptimProperty(2, 2, 10, 1);

        public void Execute(IContext ctx, ISecurity sec)
        {
            var iBase = sec.IntervalBase;
            iBase = sec.IntervalInstance.Base;

            var iPeriod = sec.Interval;
            iPeriod = sec.IntervalInstance.Value;

            var iShift = sec.IntervalInstance.Shift;

            ctx.LogInfo("База: {0}\tПериод: {1}\tСмещение: {2}", iBase, iPeriod, iShift);

            // сжатие
            ISecurity compressed;
            //compressed = sec.CompressTo(8);
            //compressed = sec.CompressTo(new Interval(15, DataIntervals.MINUTE));            

            //if (sec.IntervalBase == DataIntervals.MINUTE && sec.Interval == 5)
            if (sec.IntervalInstance == new Interval(5, DataIntervals.MINUTE))
            {
                compressed = sec.CompressTo(new Interval(10, DataIntervals.MINUTE));
                //ctx.LogInfo("База: {0}\tПериод: {1}\tСмещение: {2}", compressed.IntervalBase, compressed.Interval, compressed.IntervalInstance.Shift);
            }

            compressed = sec.CompressTo(new Interval(Mult * sec.Interval, sec.IntervalBase));
            ctx.LogInfo("База: {0}\tПериод: {1}\tСмещение: {2}", compressed.IntervalBase, compressed.Interval, compressed.IntervalInstance.Shift);

            #region Визуализация
            if (ctx.IsOptimization) return;

            // вывод на панель
            var pane = ctx.CreatePane(sec.ToString(), 100, false);
            var color = new Color(System.Drawing.Color.Blue.ToArgb());
            var lst = pane.AddList(sec.ToString(), sec, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            pane = ctx.CreatePane(compressed.ToString(), 50, false);
            color = new Color(System.Drawing.Color.Green.ToArgb());
            lst = pane.AddList(compressed.ToString(), compressed, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            var dec = compressed.Decompress(compressed.ClosePrices);
            color = new Color(System.Drawing.Color.Red.ToArgb());            
            lst = pane.AddList("ClosePrices", dec, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
            lst.Thickness = 2;
            #endregion
        }
    }
}
