using HelpersTSLab;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace Урок_8._1
{
    public class Panel3 : IExternalScript
    {
        public void Execute(IContext ctx, ISecurity sec)
        {
            // вывод цены  на панель
            var pane = ctx.CreatePane("Main", 100, false);
            var color = new Color(System.Drawing.Color.Green.ToArgb());
            var lst = pane.AddList(sec.ToString(), sec, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            // открытый интерес
            // 1) вариант
            //var oiHandler = new OpenInterest() { Context = ctx };
            //var oi = oiHandler.Execute(sec);          

            // 2) вариант
            var oi = new double[ctx.BarsCount];

            for (int i = 0; i < ctx.BarsCount; i++) oi[i] = sec.Bars[i].Interest;

            pane = ctx.CreatePane("OI", 25, false);
            color = new Color(System.Drawing.Color.Aqua.ToArgb());
            lst = pane.AddList("ОИ", oi, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
            lst.Thickness = 2;

            // bid/ask
            // 1) вариант
            var bidHandler = new Bid() { Context = ctx };
            var bid = bidHandler.Execute(sec);

            var askHandler = new Ask() { Context = ctx };
            var ask = askHandler.Execute(sec);

            // 2) вариант
            //var bid = new double[ctx.BarsCount];
            //var ask = new double[ctx.BarsCount];
            //for(int i = 0;i < ctx.BarsCount;i++)
            //{
            //    bid[i] = (double)sec.FinInfo.Bid;
            //    ask[i] = (double)sec.FinInfo.Ask;
            //}

            pane = ctx.CreatePane("bid/ask", 25, false);
            color = new Color(System.Drawing.Color.Green.ToArgb());
            lst = pane.AddList("Bid", bid, ListStyles.LINE_WO_ZERO, color, LineStyles.SOLID, PaneSides.RIGHT);

            color = new Color(System.Drawing.Color.Red.ToArgb());
            lst = pane.AddList("Ask", ask, ListStyles.LINE_WO_ZERO, color, LineStyles.SOLID, PaneSides.RIGHT);

            // спред
            var spread = ask.Subtract(bid);

            color = new Color(System.Drawing.Color.Gray.ToArgb());
            lst = pane.AddList("Spread", spread, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.LEFT);
            lst.Thickness = 2;

            // кривая спред больше
            var curve = new int[ctx.BarsCount];

            for (int i = 0; i < ctx.BarsCount; i++)
            { 
                curve[i] = spread[i] > 3 ? 1 : 0;
            }

            pane = (IGraphPane)ctx.Panes[1];
            color = new Color(System.Drawing.Color.RoyalBlue.ToArgb());
            lst = pane.AddList("Spread >", curve, ListStyles.HISTOHRAM, color, LineStyles.SOLID, PaneSides.LEFT);
        }
    }
}
