using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{
    public class Inventory
    {
        public List<Item> Items { get; set; }

        public Inventory()
        {
            Items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
            Console.WriteLine($"{item.Name} wurde dem Inventar hinzugefügt.");
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
            Console.WriteLine($"{item.Name} wurde aus dem Inventar entfernt.");
        }

        public void DisplayItems()
        {
            Console.WriteLine("Inventar:");
            foreach (var item in Items)
            {
                Console.WriteLine($"- {item.Name} (Gewicht: {item.Weight}, Wert: {item.Value})");
            }
        }
    }

}
