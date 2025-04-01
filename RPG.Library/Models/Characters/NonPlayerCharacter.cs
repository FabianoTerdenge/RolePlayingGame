using RPG.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{

    public class NonPlayerCharacter : Character, ITrader
    {
        public bool IsHostile { get; set; }

        string ITrader.Name => throw new NotImplementedException();

        int ITrader.Gold { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        List<Item> ITrader.Inventory => new List<Item>();

        float ITrader.BuyPriceMultiplier => 1.2f;

        float ITrader.SellPriceMultiplier => 0.8f;

        public NonPlayerCharacter(string name, int health, float attackSpeed, float movementSpeed,int armor, int magicResist,float dodgeChance, int level, int strength, int dexterity, int intelligence, bool isHostile, int gold, int experience, int mana, List<Ability> abilities, Vector3 position, Weapon equippedweapon)
            : base(name, health, attackSpeed, movementSpeed, armor, magicResist, dodgeChance, level, strength, dexterity, intelligence, gold, experience, mana, abilities, position, equippedweapon)
        {
            IsHostile = isHostile;
        }

    }
}
