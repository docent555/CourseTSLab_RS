using System.Reflection;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace LimitOrderTST
{
    public class LimitOrderTest : IExternalScript
    {
        public void Execute(IContext ctx, ISecurity sec)
        {
            // Получаем текущую цену. Текущая цена это по факту цена закрытия последнего бара.
            var currentPrice = sec.ClosePrices[^1];

            // При выставлении лимитных заявок нужно отталкиваться от брокера.
            // Некоторые брокеры лимитную заявку выше рынка для лонга считают рыночной и исполняют по рынку.
            // transaq сервер как раз и делает так. Будем ставить заявки ниже рынка для лонга, выше для шорта
            // Обязательно для разных входов, задаем разные сигналы
            sec.Positions.BuyAtPrice(ctx.BarsCount, 1, currentPrice * 0.99, "LE");
            sec.Positions.SellAtPrice(ctx.BarsCount, 1, currentPrice * 1.01, "SE");
        }
    }
}
