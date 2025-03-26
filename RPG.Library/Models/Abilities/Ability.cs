using System;
using System.Collections.Generic;

namespace RPG.Models
{
    public abstract class Ability
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int ManaCost { get; set; }
        public float Cooldown { get; set; } //in s
        public float RemainingCooldown { get; set; }  //in s
        public StatusEffect StatusEffect { get; set; }


        public Ability(string name, int damage, int manaCost, float cooldown, StatusEffect statusEffect)
        {
            Name = name;
            Damage = damage;
            ManaCost = manaCost;
            Cooldown = cooldown;
            RemainingCooldown = 0;
            StatusEffect = statusEffect;
        }

        public virtual void Use(Character target)
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
                RemainingCooldown-= deltatime;
            }
        }
    }
}