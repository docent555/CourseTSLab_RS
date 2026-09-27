using System;
using System.Linq;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;
using TSLab.Script.Optimization;

namespace Урок_11._2
{   
    public class SampleScript : IExternalScript
    {
        #region Оптимизируемые параметры ------------------------------------
        public OptimProperty HighPeriod = new OptimProperty(24, 10, 200, 10);

        public OptimProperty Slippage = new OptimProperty(50, 10, 100, 10);

        public OptimProperty Stop = new OptimProperty(2, 0.25, 5, 0.25);
        public OptimProperty Take = new OptimProperty(2, 0.25, 5, 0.25);

        public OptimProperty PosAdd = new OptimProperty(1, 0.25, 5, 0.25);
        public OptimProperty StopMove = new OptimProperty(0.75, 0.25, 5, 0.25);
        #endregion

        public void Execute(IContext ctx, ISecurity sec)
        {
            // Скрипт может входить только в лонг. 
            // По пробою канала дончиана на базовом таймфрейме, входим в лонга.    
            // Дальше если закрепились выше канала по дневному таймфрейму, наращиваем позу двойным размером от текущей активной позиции.    
            // Если мы еще не нарастились по дневному каналу, то ставим отложенник на добавление по достижения нужного профита.    
            // Изначально ставим стоп и тейк. По достижению профита тащим стоп в безубыток.    
            // Если же мы взяли позу по пробою дневного канала, тогда дальше танем общую позицию трейлом. Без тейков.
            #region Расчет индикаторов -------------------------------------------
            var highest = ctx.GetData("highest", new string[] { HighPeriod.ToString() },
                    () => Series.Highest(sec.HighPrices, HighPeriod));

            var lowest = ctx.GetData("lowest", new string[] { HighPeriod.ToString() },
                    () => Series.Lowest(sec.LowPrices, HighPeriod)); // период тот-же что и для highest
            #endregion


            #region Торговая логика --------------------------------------------------------

            var count = ctx.BarsCount;
            if (!ctx.IsLastBarClosed) count--;            

            var stopMoveFlag = false;

            var tradeStart = Math.Max(ctx.TradeFromBar, HighPeriod + 1);
            for (var i = tradeStart; i < count; i++)
            {
                var leb = sec.Positions.GetLastActiveForSignal("LEB", i);
                var lea = sec.Positions.GetLastActiveForSignal("LEA", i);
                var led = sec.Positions.GetLastActiveForSignal("LED", i);

                // отобрали позиции с сигналом вохода начинающимся на LE
                var lePos = sec.Positions.GetActiveForBar(i).Where(p => p.EntrySignalName.StartsWith("LE")).ToList();

                if (leb == null)
                {
                    sec.Positions.BuyIfGreater(i + 1, 1, highest[i], Slippage, "LEB");
                    stopMoveFlag = false;
                }
                else
                {
                    // пробуем наращивать позицию
                    if (lea == null)
                    {
                        var posAddPrice = leb.EntryPrice * (1 + PosAdd / 100.0);
                        sec.Positions.BuyIfGreater(i + 1, 1, posAddPrice, Slippage, "LEA");
                    }

                    var lePos1 = sec.Positions.GetActiveForBar(i).Where(p => p.EntrySignalName.StartsWith("LE")).ToList();

                    
                    var avgEntryPrice = lePos.AvgEntryPrice();

                    // выставить тейк
                    var take = avgEntryPrice * (1 + Take / 100.0);
                    lePos.ForEach(p => p.CloseAtProfit(i + 1, take, Slippage, "LXP")); // из TSLab.Utility


                    // выставить стоп
                    var stopMovePrice = avgEntryPrice * (1 + StopMove / 100.0);
                    if (sec.HighPrices[i] > stopMovePrice || stopMoveFlag)
                    {
                        stopMoveFlag = true;
                        var stop = avgEntryPrice;
                        lePos.ForEach(p => p.CloseAtStop(i + 1, stop, Slippage, "LXS"));
                    }
                    else
                    {
                        var stop = avgEntryPrice * (1 - Stop / 100.0);
                        lePos.ForEach(p => p.CloseAtStop(i + 1, stop, Slippage, "LXS"));
                    }

                    

                   
                }
            }
            #endregion

            #region Визуализация --------------------------------------------------------------
            if (ctx.IsOptimization) return;

            // вывод на панель
            var pane = ctx.CreatePane(sec.ToString(), 100, false);
            var color = new Color(System.Drawing.Color.Blue.ToArgb());
            var lst = pane.AddList(sec.ToString(), sec, CandleStyles.BAR_CANDLE, color, PaneSides.RIGHT);

            color = new Color(System.Drawing.Color.DarkGreen.ToArgb());
            lst = pane.AddList("highest", highest, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
            lst.Thickness = 1;

            color = new Color(System.Drawing.Color.DarkRed.ToArgb());
            lst = pane.AddList("lowest", lowest, ListStyles.LINE, color, LineStyles.SOLID, PaneSides.RIGHT);
            lst.Thickness = 1; 
            #endregion
        }
    }
}
