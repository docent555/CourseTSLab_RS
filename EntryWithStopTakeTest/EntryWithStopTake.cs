using TSLab.Script;
using TSLab.Script.Handlers;

namespace EntryWithStopTake
{
    public class EntryWithStopTake : IExternalScript
    {
        public void Execute(IContext ctx, ISecurity sec)
        {
            for (int i = 0; i < ctx.BarsCount; i++)
            {

                var le = sec.Positions.GetLastActiveForSignal("LE", i);

                if (le == null)
                {
                    sec.Positions.BuyAtMarket(i + 1, 1, "LE");
                }
                else
                {
                    le.CloseAtStop(i + 1, le.EntryPrice * 0.995, 10, "LXS");
                    le.CloseAtProfit(i + 1, le.EntryPrice * 1.005, 10, "LXP");
                }
            }
        }
    }
}
