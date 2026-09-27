using HelpersTSLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace Урок_8._1
{
    public class DoubleSecurity : IExternalScript2
    {
        public void Execute(IContext ctx, ISecurity sec1, ISecurity sec2)
        {
            // вывод цены  на панель
            var pane = ctx.CreatePane(sec1.ToString(), 50, false);
            var color = new Color(System.Drawing.Color.Green.ToArgb());
            var lst = pane.AddList(sec1.ToString(), sec1, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            pane = ctx.CreatePane(sec2.ToString(), 50, false);
            color = new Color(System.Drawing.Color.Red.ToArgb());
            lst = pane.AddList(sec2.ToString(), sec2, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            // разница между ценами закрытия
            var sub = sec1.ClosePrices.Subtract(sec2.ClosePrices);
            pane = ctx.CreatePane(sub.ToString(), 20, false);
            color = new Color(System.Drawing.Color.RosyBrown.ToArgb());
            lst = pane.AddList(sub.ToString(), sub, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
            lst.Thickness = 2;
        }
    }
}
