using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace RPG.Library.Models.Abilities
{
    //removes all StatusEffects on a target
    public class BlessingAbility : Ability
    {
        public BlessingAbility(BlessingAbility clone) : base(clone.Name, clone.Damage, clone.Range, clone.ManaCost, clone.Cooldown, clone.StatusEffect)
        {
        }

        public BlessingAbility(string name, int damage, float range, int manaCost, float cooldown, StatusEffect statusEffect) : base(name, damage, range, manaCost, cooldown, statusEffect)
        {
        }
        public override void Use(Character target)
        {
            if (RemainingCooldown > 0)
            {
                Console.WriteLine($"{Name} konnte nicht angewendet werden, da es noch {RemainingCooldown} auf Cooldown ist.");
                return;
            }
            Console.WriteLine($"{Name} wurde auf {target.Name} angewendet und läutert alle Statuseffekte!");
            target.StatusEffects.Clear();
            Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");
            base.Use(target);
        }
    }
}
