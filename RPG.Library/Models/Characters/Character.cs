using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{
    public abstract class Character
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        private int _currentHealth;  

        public int CurrentHealth {
            get { return _currentHealth > 0 ? _currentHealth : 0; }  
            set { _currentHealth = value; }
        }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int ExperiencePoints { get; set; }
        public int Gold { get; set; }
        public int Mana { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<StatusEffect> StatusEffects { get; set; }


        public Character(string name, int health, int level, int strength, int dexterity, int intelligence, int gold, int experience, int mana, List<Ability> abilities)
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
            StatusEffects = new List<StatusEffect>();
        }

        public void Attack()
        {
            Console.WriteLine($"{Name} greift an!");
        }

        public void Defend()
        {
            Console.WriteLine($"{Name} verteidigt sich!");
        }
        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }
        public void UseItem(Item item)
        {
            Console.WriteLine($"{Name} benutzt {item.Name}.");
            item.Use();
        }
        public void ProcessCooldowns()
        {
            foreach (var ability in Abilities)
            {
                ability.ReduceCooldown();
            }
        }
        public void ApplyStatusEffect(StatusEffect effect)
        {
            StatusEffects.RemoveAll(e => e.Name == effect.Name);

            StatusEffects.Add(effect);
            Console.WriteLine($"{Name} ist jetzt {effect.Name} ausgesetzt!");
        }

        public void ProcessStatusEffects()
        {
            for (int i = StatusEffects.Count - 1; i >= 0; i--)
            {
                StatusEffects[i].Apply(this);
                if (StatusEffects[i].Duration <= 0)
                {
                    Console.WriteLine($"{Name} ist nicht mehr {StatusEffects[i].Name} ausgesetzt.");
                    StatusEffects.RemoveAt(i);
                }
            }
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
