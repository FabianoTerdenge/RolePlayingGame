﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{
    public class Monster : NonPlayerCharacter
    {
        public int ExperienceReward { get; set; }

        public Monster(string name, int health, float attackSpeed, float movementSpeed, int level, int strength, int dexterity, int intelligence, bool isHostile, int gold, int experience, int mana, List<Ability> abilities, Vector3 position, Weapon equippedweapon)
            : base(name, health, attackSpeed, movementSpeed, level, strength, dexterity, intelligence, isHostile,  gold,  experience,  mana,  abilities, position, equippedweapon)
        {
            ExperienceReward = experience / 10;
        }

       

        public void UseAbility(Ability ability, Character target)
        {
            ability.Use(target);
        }

    }
}
