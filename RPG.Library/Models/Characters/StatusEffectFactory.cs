using RPG.Library.Models.Abilities;
using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Library.Models.Characters
{
    public static class StatusEffectFactory
    {
        public static StatusEffect CreatePoisonEffect() => new StatusEffect("Vergiftung", 3f, 1f, (target) =>
        {
            Console.WriteLine($"{target.Name} erleidet Vergiftungsschaden!");
            target.CurrentHealth -= 5;
            Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");
        });
        public static StatusEffect CreateBleedEffect() => new StatusEffect("Blutung", 6f, 1f, (target) =>
        {
            Console.WriteLine($"{target.Name} erleidet Blutungsschaden!");
            target.CurrentHealth -= 3;
            Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");
        });
        public static StatusEffect CreateStunEffect() => new StatusEffect("Betäubung", 2f, 1f, target =>
        {
            Console.WriteLine($"{target.Name} ist betäubt und kann nicht angreifen!");
        });

    }
}
