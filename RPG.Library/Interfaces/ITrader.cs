using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Library.Interfaces
{
    public interface ITrader
    {
        public string Name { get; }
        public int Gold { get; set; }
        public List<Item> Inventory { get; }
        public float BuyPriceMultiplier { get; } // e.g., 1.2f (20% markup)
        public float SellPriceMultiplier { get; } // e.g., 0.8f (20% discount)

        public bool CanAfford(int amount) => Gold >= amount;

        public void AddGold(int amount)
        {
            Gold = Math.Max(0, Gold + amount);
        }

        public void RemoveGold(int amount)
        {
            Gold = Math.Max(0, Gold - amount);
        }

        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }

        public void RemoveItem(Item item)
        {
            Inventory.Remove(item);
        }

        public int GetBuyPrice(Item item)
        {
            return (int)(item.Value * BuyPriceMultiplier);
        }

        public int GetSellPrice(Item item)
        {
            return (int)(item.Value * SellPriceMultiplier);
        }

        // Additional merchant-specific methods
        public void RestockInventory(List<Item> newItems)
        {
            Inventory.AddRange(newItems);
        }
        public void ShowInventory()
        {
            var inventorySummary = Inventory
                .GroupBy(item => item.Name)
                .Select(group => new
                {
                    Name = group.Key,
                    //AverageValue = group.Average(i => i.Value),
                    //TotalValue = group.Sum(i => i.Value),
                    ValuePerItem = group.First().Value,
                    Amount = group.Count()
                })
                .OrderByDescending(item => item.Amount);

            // Header with borders
            Console.WriteLine("┌──────────────────────┬──────────────┬────────┐");
            Console.WriteLine("| {0,-20} | {1,-12} | {2,-6} |", "Name", "Value", "Count");
            Console.WriteLine("├──────────────────────┼──────────────┼────────┤");

            foreach (var item in inventorySummary)
            {
                Console.WriteLine("| {0,-20} | {1,-12} | {2,6} |",
                    item.Name,
                    $"{item.ValuePerItem:N2} Gold",
                    item.Amount);
            }

            Console.WriteLine("└──────────────────────┴──────────────┴────────┘");
        }
    }
}
