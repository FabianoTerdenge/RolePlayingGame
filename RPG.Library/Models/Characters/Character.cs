using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{
    public class Character
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int ExperiencePoints { get; set; }
        public int Gold { get; set; }
        public int Mana { get; set; }
        public List<Ability> Abilities { get; set; }

        public Character(string name, int health, int level, int strength, int dexterity, int intelligence, int gold, int experience, int mana,List<Ability> abilities)
        {
            Name = name;
            MaxHealth = health;
            CurrentHealth = health;
            Level = level;
            Strength = strength;
            Dexterity = dexterity;
            Intelligence = intelligence;
            ExperiencePoints = experience;
            Gold = gold;
            Mana = mana;
            Abilities = abilities;
    }

        public void Attack()
        {
            Console.WriteLine($"{Name} greift an!");
        }

        public void Defend()
        {
            Console.WriteLine($"{Name} verteidigt sich!");
        }

        public void UseItem(Item item)
        {
            Console.WriteLine($"{Name} benutzt {item.Name}.");
            item.Use();
        }

        public void LevelUp()
        {
            Level++;
            Console.WriteLine($"{Name} ist jetzt Level {Level}!");
        }

        public void GainExperience(int points)
        {
            ExperiencePoints += points;
            Console.WriteLine($"{Name} erhält {points} Erfahrungspunkte!");
        }

        public void Trade(Item item, int gold)
        {
            Gold += gold;
            Console.WriteLine($"{Name} verkauft {item.Name} für {gold} Gold.");
        }

        public void UseAbility(Ability ability, Character target)
        {
            if (Mana >= ability.ManaCost)
            {
                Mana -= ability.ManaCost;
                ability.Use(target);
            }
            else
            {
                Console.WriteLine($"{Name} hat nicht genug Mana, um {ability.Name} zu verwenden!");
            }
        }
    }

}
