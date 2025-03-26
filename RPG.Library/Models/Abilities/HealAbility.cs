﻿using System;

namespace RPG.Models
{
    public class HealAbility : Ability
    {
        public HealAbility(HealAbility clone):base(clone.Name,clone.Damage, clone.ManaCost,clone.Cooldown,clone.StatusEffect)
        {
        }

        public HealAbility(string name, int damage, int manaCost, float cooldown, StatusEffect statusEffect) : base(name, damage, manaCost, cooldown, statusEffect)
        {
        }
        public override void Use(Character target)
        {
            if (RemainingCooldown > 0)
            {
                Console.WriteLine($"{Name} konnte nicht angewendet werden, da es noch {RemainingCooldown} auf Cooldown ist.");
                return;
            }
            Console.WriteLine($"{Name} wurde auf {target.Name} angewendet und heilt {-Damage} Leben!");
            if (target.CurrentHealth - Damage > target.MaxHealth)
            {
                target.CurrentHealth = target.MaxHealth;
            }
            else
            {
                target.CurrentHealth -= Damage;
            }
            Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");
            base.Use(target);
        }
    }
}
