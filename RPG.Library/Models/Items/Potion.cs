using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{

    // Heiltrank
    public class Potion : Item
    {
        public int HealingAmount { get; set; }

        public Potion(string name, float weight, int value, int healingAmount)
            : base(name, weight, value)
        {
            HealingAmount = healingAmount;
        }

        public override void Use()
        {
            Console.WriteLine($"{Name} heilt {HealingAmount} Lebenspunkte!");
        }
    }

}
