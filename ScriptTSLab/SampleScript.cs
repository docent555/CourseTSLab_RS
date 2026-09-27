using System.Reflection;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace ScriptTSLab
{
    public class SampleScript : IExternalScript
    {
        public void Execute(IContext ctx, ISecurity sec)
        {
            // вывод в лог
            var color = new Color(System.Drawing.Color.DarkGreen.ToArgb());
            ctx.Log("Скрипт отработал.", color);

            // вывод на панель
            var pane = ctx.First;
            color = new Color(System.Drawing.Color.Red.ToArgb());
            var lst = pane.AddList("Instrument", sec, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            // подсветка свечей
            color = new Color(System.Drawing.Color.BurlyWood.ToArgb());
            lst.SetColor(ctx.BarsCount - 5, color);

            // вывод на панель
            pane = ctx.CreatePane("Вторая панель", 25, false);
            color = new Color(System.Drawing.Color.Green.ToArgb());
            lst = pane.AddList("ClosePrices", sec.ClosePrices, ListStyles.LINE , color, LineStyles.SOLID, PaneSides.RIGHT);

            // толщина
            lst.Thickness = 5;

            // вывод тела свечи
            //var bodyList = new double[ctx.BarsCount];
            //var bodyList = new double[sec.Bars.Count];
            var bodyList = new double[sec.ClosePrices.Count];

            for (int i = 0; i < sec.ClosePrices.Count; i++)
            {
                //bodyList[i] = sec.OpenPrices[i] - sec.ClosePrices[i];
                bodyList[i] = sec.Bars[i].Open - sec.Bars[i].Close;
            }

            // вывод на панель
            pane = ctx.CreatePane("Третья панель", 25, false);
            color = new Color(System.Drawing.Color.BlueViolet.ToArgb());
            lst = pane.AddList("Тела свечей", bodyList, ListStyles.HISTOHRAM, color, LineStyles.SOLID, PaneSides.RIGHT);

            // точность второй панели
            pane = (IGraphPane)ctx.Panes[1];
            pane.UpdatePrecision(PaneSides.RIGHT, 5);

            // подсветка свечей
            color = new Color(System.Drawing.Color.BurlyWood.ToArgb());
            lst.SetColor(ctx.BarsCount - 5, color);            
        }
    }
}
