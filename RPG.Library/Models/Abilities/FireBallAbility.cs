﻿using System;
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
        public FireBallAbility(string name, int damage, float range, int manaCost, float cooldown, StatusEffect statusEffect) : base(name, damage,range, manaCost, cooldown, statusEffect)
        {
        }
        public FireBallAbility(FireBallAbility clone) : base(clone.Name, clone.Damage,clone.Range,clone.ManaCost, clone.Cooldown, clone.StatusEffect)
        {
        }
        public override void Use(Character attacker,Character target)
        {
            if (RemainingCooldown > 0)
            {
                Console.WriteLine($"{Name} konnte nicht angewendet werden, da es noch {RemainingCooldown} auf Cooldown ist.");
                return;
            }
            int dmg = attacker.CalculateDamage(target, Damage, true);
            target.CurrentHealth = Math.Max(0, Math.Min(target.CurrentHealth - dmg, target.MaxHealth));
            Console.WriteLine($"{Name} wurde auf {target.Name} angewendet und verursacht {dmg} Schaden!");
            Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");
            base.Use(attacker, target);
        }
    }
}
