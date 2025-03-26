using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{

    public class NonPlayerCharacter : Character
    {
        public bool IsHostile { get; set; }

        public NonPlayerCharacter(string name, int health, float attackSpeed, int level, int strength, int dexterity, int intelligence, bool isHostile, int gold, int experience, int mana, List<Ability> abilities)
            : base(name, health, attackSpeed, level, strength, dexterity, intelligence, gold, experience, mana, abilities)
        {
            IsHostile = isHostile;
        }
    }
}
