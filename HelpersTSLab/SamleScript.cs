using TSLab.Script;
using TSLab.Script.Handlers;

namespace HelpersTSLab
{
    public class SampleScript : IExternalScript
    {
        public void Execute(IContext ctx, ISecurity sec)
        {
            // вывод в лог            
            ctx.LogInfo("Скрипт отработал.");

            // вывод на панель
            var pane = ctx.First;
            var color = new Color(System.Drawing.Color.Red.ToArgb());
            var lst = pane.AddList("Instrument", sec, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            // подсветка свечей
            color = new Color(System.Drawing.Color.BurlyWood.ToArgb());
            lst.SetColor(ctx.BarsCount - 5, color);

            // вывод на панель
            pane = ctx.CreatePane("Вторая панель", 25, false);
            color = new Color(System.Drawing.Color.Green.ToArgb());
            lst = pane.AddList("ClosePrices", sec.ClosePrices, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);

            // толщина
            lst.Thickness = 5;

            // вывод тела свечи
            //var bodyList = new double[ctx.BarsCount];
            //var bodyList = new double[sec.Bars.Count];
            var bodyList = sec.OpenPrices.Subtract(sec.ClosePrices);

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
