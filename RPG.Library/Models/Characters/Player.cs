using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{

    public class Player : Character
    {

        public Player(string name, int health, float attackSpeed, int level, int strength, int dexterity, int intelligence, int gold, int experience, int mana, List<Ability> abilities)
            : base(name, health, attackSpeed, level, strength, dexterity, intelligence,gold, experience,  mana, abilities)
        {
        }


        public void Trade(Item item, int gold)
        {
            Gold += gold;
            Console.WriteLine($"{Name} verkauft {item.Name} für {gold} Gold.");
        }
    }
}
