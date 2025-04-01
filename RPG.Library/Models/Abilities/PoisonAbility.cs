// See https://aka.ms/new-console-template for more information
using RPG.Models;
using System;
using static System.Net.Mime.MediaTypeNames;

public class PoisonAbility : Ability
{
    public PoisonAbility(PoisonAbility clone) : base(clone.Name, clone.Damage,clone.Range, clone.ManaCost, clone.Cooldown, clone.StatusEffect)
    {
    }

    public PoisonAbility(string name, int damage, float range, int manaCost, float cooldown, StatusEffect statusEffect) : base(name, damage, range, manaCost, cooldown, statusEffect)
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

        Console.WriteLine($"{Name} wurde auf {target.Name} angewendet und er verliert {dmg} Leben");
        target.CurrentHealth = Math.Min(0, target.CurrentHealth - dmg);
        Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");
        base.Use(attacker, target);
    }
}