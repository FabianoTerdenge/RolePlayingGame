using System;

namespace RPG.Models
{
    public abstract class Ability
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int ManaCost { get; set; }
        public int Cooldown { get; set; }

        public Ability(string name, int damage, int manaCost, int cooldown)
        {
            Name = name;
            Damage = damage;
            ManaCost = manaCost;
            Cooldown = cooldown;
        }

        public abstract void Use(Character target);
    }
}