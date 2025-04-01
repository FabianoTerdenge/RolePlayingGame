using RPG.Library.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{
    public enum WeaponType { Melee, Ranged, Magic }

    public class Weapon : Item
    {
        public WeaponType Type { get; set; }
        public int Damage { get; set; }
        public float Range { get; set; }
        public StatusEffect AppliesStatusEffect { get; set; }
        public float CriticalChance { get; set; }
        public float CriticalDamage { get; set; }
        public float AttackSpeed { get; set; } // Attacks per second

        public static readonly Weapon Fists = new Weapon(
           "Fists",
             WeaponType.Melee,
             null,
             5,
             .5f,
             0.05f,
             1.2f,
             1.0f,
             0.0f,
             0);
        public static readonly Weapon Claws = new Weapon(
          "Claws",
            WeaponType.Melee,
            StatusEffectFactory.CreateBleedEffect(),
            15,
            .5f,
            0.1f,
            2.5f,
            0.8f,
            0.0f,
            0);
        public static readonly Weapon Longsword = new Weapon(
            "Longsword",
            WeaponType.Melee,
            null,
             12,
            .6f,
             0.1f,
             2.1f,
             0.8f,
             8.0f,
            15);

        public static readonly Weapon Bow = new Weapon(
            "Bow",
            WeaponType.Ranged,
            null,
             8,
             15.0f,
             0.15f,
             1.5f,
             1.2f,
             4.0f,
             10
        );

        public Weapon(string Name, WeaponType type, StatusEffect appliesStatusEffect , int damage, float range, float criticalChance, float criticalDamage, float attackSpeed, float weight, int value)
            : base(Name, weight, value)
        {
            Damage = damage;
            Range = range;
            Type = type;
            AppliesStatusEffect = appliesStatusEffect;
            CriticalChance = criticalChance;
            CriticalDamage = criticalDamage;
            AttackSpeed = attackSpeed;
        }

        public override void Use()
        {
            Console.WriteLine($"{Name} verursacht {Damage} Schaden!");
        }
    }
}
