using RPG.Library.Interfaces;
using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Library.Models
{
    public class TradingSystem
    {
        public static bool TradeItem(ITrader seller, ITrader buyer, Item item)
        {
            int price = seller.GetSellPrice(item);

            if (!buyer.CanAfford(price))
                return false;

            if (!seller.Inventory.Contains(item))
                return false;

            // Execute trade
            buyer.RemoveGold(price);
            seller.AddGold(price);

            seller.RemoveItem(item);
            buyer.AddItem(item);

            return true;
        }
    }
}
