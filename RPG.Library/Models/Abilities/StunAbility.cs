// See https://aka.ms/new-console-template for more information
using RPG.Models;
using System;
using static System.Net.Mime.MediaTypeNames;

public class StunAbility : Ability
{
    public StunAbility(StunAbility clone) : base(clone.Name, clone.Damage, clone.ManaCost, clone.Cooldown, clone.StatusEffect)
    {
    }

    public StunAbility(string name, int damage, int manaCost, float cooldown, StatusEffect statusEffect) : base( name, damage, manaCost, cooldown, statusEffect)
    {
    }

    public override void Use(Character target)
    {
        if (RemainingCooldown > 0)
        {
            Console.WriteLine($"{Name} konnte nicht angewendet werden, da es noch {RemainingCooldown} auf Cooldown ist.");
            return;
        }
        Console.WriteLine($"{Name} wurde auf {target.Name} angewendet und er verliert {Damage} Leben");
        if (target.CurrentHealth - Damage > target.CurrentHealth)
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