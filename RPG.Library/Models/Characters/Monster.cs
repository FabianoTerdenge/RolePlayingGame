using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{
    public class Monster : NonPlayerCharacter
    {
        public int ExperienceReward { get; set; }

        public Monster(string name, int health, int level, int strength, int dexterity, int intelligence, bool isHostile, int gold, int experience, int mana, List<Ability> abilities)
            : base(name, health, level, strength, dexterity, intelligence, isHostile,  gold,  experience,  mana,  abilities)
        {
            ExperienceReward = experience / 10;
        }

        public void AttackPlayer(Player player)
        {
            Console.WriteLine($"{Name} greift {player.Name} an und verursacht {Strength} Schaden!");
            player.CurrentHealth -= Strength;
            if (player.CurrentHealth <= 0)
            {
                Console.WriteLine($"{player.Name} wurde besiegt!");
            }
        }

        public void UseAbility(Ability ability, Character target)
        {
            ability.Use(target);
        }
    }
}
