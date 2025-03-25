using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace RPG.Models
{
    public class FireBallAbility : Ability
    {
        public FireBallAbility(string name, int damage, int manaCost, int cooldown, StatusEffect statusEffect) : base(name, damage, manaCost, cooldown, statusEffect)
        {
        }
        public FireBallAbility(FireBallAbility clone) : base(clone.Name, clone.Damage, clone.ManaCost, clone.Cooldown, clone.StatusEffect)
        {
        }
        public override void Use(Character target)
        {
            if (RemainingCooldown > 0)
            {
                Console.WriteLine($"{Name} konnte nicht angewendet werden, da es noch {RemainingCooldown} auf Cooldown ist.");
                return;
            }

            target.CurrentHealth -= Damage;

            Console.WriteLine($"{Name} wurde auf {target.Name} angewendet und verursacht {Damage} Schaden!");
            Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");
            base.Use(target);
        }
    }
}
