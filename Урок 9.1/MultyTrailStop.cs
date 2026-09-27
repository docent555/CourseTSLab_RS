using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;
using TSLab.Script.Optimization;

namespace Урок_11._1
{
    // Скрипт может входить в лонг и шорт. В обоих случаях используется канал дончиана и входим по пробою верхней и нижней границы.
    // После входа для лонга и шорта выставляется несколько стопов. А именно: простой трейл стоп и параболический стоп.
    // Скрипт может входить в разнонаправленные позиции, то есть делать ЛОКИ.
    public class MultyTrailStop : IExternalScript
    {
        #region Оптимизируемые параметры
        public OptimProperty HighPeriod = new OptimProperty(24, 10, 200, 10);
        public OptimProperty LowPeriod = new OptimProperty(24, 10, 200, 10);
        public OptimProperty Slippage = new OptimProperty(50, 10, 100, 10);
        public OptimProperty StopLoss = new OptimProperty(2, 0.2, 5, 0.5);
        public OptimProperty TrailEnable = new OptimProperty(1.5, 0.2, 5, 0.5);
        public OptimProperty TrailLoss = new OptimProperty(1, 0.2, 5, 0.5); 
        #endregion

        public void Execute(IContext ctx, ISecurity sec)
        {
            //#region Расчет индикаторов
            //var highest = ctx.GetData("highest", new string[] { HighPeriod.ToString() },
            //                  () => Series.Highest(sec.HighPrices, HighPeriod));
            //var lowest = ctx.GetData("lowest", new string[] { LowPeriod.ToString() },
            //                          () => Series.Lowest(sec.LowPrices, LowPeriod));

            //var trailHnd = new TrailStop()
            //{
            //    StopLoss = StopLoss,
            //    TrailEnable = TrailEnable,
            //    TrailLoss = TrailLoss
            //}; 
            //#endregion

            //#region Торговая логика
            //// пропустим разгонную часть индикатора, начав с НЕ НУЛЕВОГО бара.
            //var d = Math.Max(HighPeriod, LowPeriod);
            //for (var i = d + 1; i < ctx.BarsCount; i++)
            //{
            //    var le = sec.Positions.GetLastActiveForSignal("LE");
            //    var se = sec.Positions.GetLastActiveForSignal("SE");
            //    var currPos = sec.Positions.GetActiveForBar(i);         // запросим активные позиции для текущего бара. Применяется ниже

            //    // разрешаем входить в разнонаправленные позиции. Но только одна позиция в одну сторону.
            //    if (se == null)
            //        sec.Positions.SellIfLess(i + 1, 1, lowest[i], Slippage, "SE");

            //    if (le == null)
            //        sec.Positions.BuyIfGreater(i + 1, 1, highest[i], Slippage, "LE");

            //    // если в начале данной итерации позиций активных на текущем баре не было, значит стопы ставить некуда, переходим на след итерацию.
            //    if (!currPos.Any())
            //        continue;

            //    // выставить стопы для всех активных позиции. Как ставить стоп, определяем по Сигналу входа..
            //    foreach (var p in currPos)
            //    {
            //        switch (p.EntrySignalName)
            //        {
            //            case "LE":
            //                {
            //                    // расчитаем цены стопа для тейла и для параболика. Используем хелпер метод, чтобы в будущем не думать.
            //                    var pStop = p.ParabolicStop(i, 500, 1, 1);
            //                    var tStop = trailHnd.Execute(p, i);

            //                    // выставим ближайший к цене стоп. 
            //                    if (pStop > tStop)
            //                        p.CloseAtStop(i + 1, pStop, Slippage, "LXPar");
            //                    else
            //                        p.CloseAtStop(i + 1, tStop, Slippage, "LXTr");
            //                }
            //                break;

            //            case "SE":
            //                {
            //                    // расчитаем цены стопа для тейла и для параболика. Используем хелпер метод, чтобы в будущем не думать.
            //                    var pStop = p.ParabolicStop(i, 500, 1, 1);
            //                    var tStop = trailHnd.Execute(p, i);

            //                    // выставим ближайший к цене стоп. 
            //                    if (pStop < tStop)
            //                        p.CloseAtStop(i + 1, pStop, Slippage, "SXPar");
            //                    else
            //                        p.CloseAtStop(i + 1, tStop, Slippage, "SXTr");
            //                }
            //                break;
            //        }
            //    }
            //} 
            //#endregion

            //#region Визуализация
            //if (ctx.IsOptimization)
            //    return;

            //// отрисовка
            //var pane = ctx.CreatePane(sec.ToString(), 100, false);
            //var color = new Color(System.Drawing.Color.Green.ToArgb());
            //var lst = pane.AddList(sec.ToString(), sec, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            //color = new Color(System.Drawing.Color.Blue.ToArgb());
            //lst = pane.AddList("highest", highest, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
            //lst.Thickness = 2;

            //color = new Color(System.Drawing.Color.Red.ToArgb());
            //lst = pane.AddList("lowest", lowest, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
            //lst.Thickness = 2; 
            //#endregion
        }
    }
}
