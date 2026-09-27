using TSLab.Script;
using TSLab.Script.Handlers;

namespace EntryWithStopTakeTest
{
    public class EntryWithStopTakeTest : IExternalScript
    {
        public void Execute(IContext ctx, ISecurity sec)
        {
            var le = sec.Positions.GetLastActiveForSignal("LE");

            if (le == null)
            {
                sec.Positions.BuyAtMarket(ctx.BarsCount, 1, "LE");
            }
            else
            {
                le.CloseAtStop(ctx.BarsCount, le.EntryPrice * 0.995, 10, "LXS");
                le.CloseAtProfit(ctx.BarsCount, le.EntryPrice * 1.005, 10, "LXP");
            }
        }
    }
}
