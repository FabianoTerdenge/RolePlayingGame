using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RPG.Models
{
    public class HealAbility : Ability
    {
        public HealAbility(string name, int damage, int manaCost, int cooldown) : base(name, damage, manaCost, cooldown)
        {
        }
        public override void Use(Character target)
        {
            Console.WriteLine($"{Name} wurde auf {target.Name} angewendet und heilt {Damage} Leben!");
            if (target.CurrentHealth - Damage > target.CurrentHealth)
            {
                target.CurrentHealth = target.MaxHealth;
             }
            else
            {
                target.CurrentHealth -= Damage;
            }
        }
    }
}
