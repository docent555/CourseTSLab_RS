using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace HelpersTSLab
{
    public static class TradeHelper
    {
        public static IList<double>? Subtract(this IList<double> list, IList<double> subtrList)
        {
            if (list.Count != subtrList.Count) return null;

            var res = new double[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                res[i] = list[i] - subtrList[i];
            }

            return res;
        }

        public static void LogInfo(this IContext ctx, string msg)
        {
            //var color = new Color(System.Drawing.Color.DarkGreen.ToArgb());
            //ctx.Log("Скрипт отработал.", color);

            ctx.Log(msg);
        }
    }
}
