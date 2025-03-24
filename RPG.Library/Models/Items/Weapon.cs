using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{

    public class Weapon : Item
    {
        public int Damage { get; set; }
        public int Range { get; set; }

        public Weapon(string name, float weight, int value, int damage, int range)
            : base(name, weight, value)
        {
            Damage = damage;
            Range = range;
        }

        public override void Use()
        {
            Console.WriteLine($"{Name} verursacht {Damage} Schaden!");
        }
    }
}
