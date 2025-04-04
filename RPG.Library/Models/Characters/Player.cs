﻿using RPG.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{
    [Serializable]

    public class Player : Character, ITrader
    {
        public List<Item> Inventory { get; }

        public float BuyPriceMultiplier { get; }

        public float SellPriceMultiplier { get; }
        public Player(string name, int health, float attackSpeed, float movementSpeed, int armor, int magicResist, float dodgeChance, int level, int strength, int dexterity, int intelligence, int charisma, int gold, int experience, int mana, List<Ability> abilities, Vector3 position, Weapon equippedweapon)
            : base(name, health, attackSpeed, movementSpeed, armor, magicResist, dodgeChance, level, strength, dexterity, intelligence,  charisma, gold, experience, mana, abilities, position, equippedweapon)
        {
            Inventory = new List<Item>(); 
            BuyPriceMultiplier = 1.2f; 
            SellPriceMultiplier = 0.8f;
        }

    }
}
