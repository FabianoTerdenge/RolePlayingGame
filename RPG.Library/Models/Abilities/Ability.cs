using RPG.Library.Interfaces;
using RPG.Library.Models.Abilities;
using System;
using System.Collections.Generic;

namespace RPG.Models
{
    public abstract class Ability
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public float Range { get; set; }
        public int ManaCost { get; set; }
        public float Cooldown { get; set; } //in s
        public float RemainingCooldown { get; set; }  //in s
        public StatusEffect StatusEffect { get; set; }
        //Abilities
        public static readonly FireBallAbility fireball = new("Feuerball", 20, 10f, 30, 3f, null);
        public static readonly HealAbility heal = new("Heilung", -15, 3f, 20, 5f, null);
        public static readonly PoisonAbility poisonDart = new("Giftpfeil", 10, 5f, 20, 2f, StatusEffect.poisonEffect);
        public static readonly StunAbility stunAttack = new("Betäubungsschlag", 5, 1.5f, 0, 1f, StatusEffect.stunEffect);
        public static readonly BlessingAbility blessing = new("Läuterung", 5, 1.5f, 0, 1f, null);

        public Ability(string name, int damage, float range, int manaCost, float cooldown, StatusEffect statusEffect)
        {
            Name = name;
            Damage = damage;
            Range = range;
            ManaCost = manaCost;
            Cooldown = cooldown;
            RemainingCooldown = 0;
            StatusEffect = statusEffect;
        }

        public virtual void Use(Character attacker,Character target)
        {
            if (StatusEffect != null)
            {
                target.ApplyStatusEffect(StatusEffect);
            }
            RemainingCooldown = Cooldown;
        }
        public void ReduceCooldown(float deltatime)//reduce cooldown and apply effects
        {
            if (RemainingCooldown > 0)
            {
                RemainingCooldown = Math.Max(RemainingCooldown - deltatime, 0);
            }
        }
    }
}