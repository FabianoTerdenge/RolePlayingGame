using RPG.Library.Interfaces;
using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Library.Models.Characters
{
    [Serializable]

    public class Merchant : Character, ITrader
    {
        public Merchant(string name, int health, float movementSpeed, float attackSpeed, int armor, int magicResist, float dodgeChance, int level, int strength, int dexterity, int intelligence, int charisma, int gold, int experience, int mana, List<Ability> abilities, Vector3 position, Weapon equippedweapon) : base(name, health, movementSpeed, attackSpeed, armor, magicResist, dodgeChance, level, strength, dexterity, intelligence,charisma, gold, experience, mana, abilities, position, equippedweapon)
        {
        }

        // ITrader implementation
        public List<Item> Inventory { get; } = new List<Item>();
        public float BuyPriceMultiplier { get; } = 1.2f; // Charges 20% more
        public float SellPriceMultiplier { get; } = 0.8f; // Pays 20% less

    }
}
