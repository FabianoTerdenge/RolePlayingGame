using RPG.Library.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{
    public abstract class Character
    {
        private int _id;
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        private int _currentHealth;
        public int CurrentHealth
        {
            get { return _currentHealth > 0 ? _currentHealth : 0; }
            set { _currentHealth = value; }
        }
        private float AttackTimer { get; set; } // current Attackdelta used for attackspeed calculation
        public float AttackSpeed { get; set; }

        public int Level { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int ExperiencePoints { get; set; }
        public int Gold { get; set; }
        public int Mana { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<StatusEffect> StatusEffects { get; set; }


        public Character(string name, int health, float attackSpeed, int level, int strength, int dexterity, int intelligence, int gold, int experience, int mana, List<Ability> abilities)
        {
            Id = _id++;
            Name = name;
            MaxHealth = health;
            CurrentHealth = health;
            AttackSpeed = attackSpeed;
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
        public void UpdateAttackTiming(float deltaTime, List<Character> characters)
        {
            AttackTimer += deltaTime;
            float interval = AttackSpeed;

            while (AttackTimer >= interval && !EndOfFight(characters))
            {
                Attack(characters);
                AttackTimer -= interval;
            }
        }

        public static bool EndOfFight(List<Character> characters)
        {
            if (!characters.OfType<Monster>().ToList().FindAll(m => m.CurrentHealth > 0).Any())
                return true;
            if (!characters.OfType<Player>().ToList().FindAll(p => p.CurrentHealth > 0).Any())
                return true;
            return false;
        }

        public void AutoAttackEnemy(Character character)
        {
            Console.WriteLine($"{Name} greift {character.Name} an und verursacht {Strength} Schaden!");
            character.CurrentHealth -= Strength;
            Console.WriteLine($"{character.Name} hat noch {character.CurrentHealth} Lebenspunkte.");
        }
        public void AttackLogicPlayer(List<Character> allCharacters)
        {
            List<Monster> monsters = allCharacters.OfType<Monster>().ToList();
            List<Player> players = allCharacters.OfType<Player>().ToList();
            List<Monster> aliveMonsters = monsters.FindAll(m => m.CurrentHealth > 0);
            List<Player> alivePlayers = players.FindAll(p => p.CurrentHealth > 0);

            Console.WriteLine($"\n{Name}: Was möchtest du tun?");
            Console.WriteLine("1. Angreifen");
            Console.WriteLine("2. Spezialfähigkeit verwenden\n");
            int attackChoice = int.Parse(Console.ReadLine());
            if (attackChoice == 1) // Normal Attack
            {
                // Zielmonster auswählen for player
                int monsterChoice = 0;
                if (aliveMonsters.Count > 1)
                {
                    Console.WriteLine("Wähle ein Monster zum Angreifen:");
                    for (int i = 0; i < aliveMonsters.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {aliveMonsters[i].Name} (HP: {aliveMonsters[i].CurrentHealth})");
                    }
                    monsterChoice = int.Parse(Console.ReadLine()) - 1;
                }

                if (monsterChoice >= 0 && monsterChoice < aliveMonsters.Count)
                {
                    AutoAttackEnemy(aliveMonsters[monsterChoice]);
                    if (!aliveMonsters[monsterChoice].IsAlive())
                    {
                        Console.WriteLine($"{aliveMonsters[monsterChoice].Name} wurde besiegt!");
                        GainExperience(aliveMonsters[monsterChoice].ExperienceReward);
                    }
                }
                else
                {
                    Console.WriteLine("Ungültige Auswahl!");
                }
                FightLog.records.Add(new FightRecord(true, new List<Character>(allCharacters), (Player)this));
            }
            else if (attackChoice == 2) // Ability Cast
            {
                if (Abilities.Count > 0)
                {
                    Console.WriteLine("\nWähle eine Fähigkeit:");
                    for (int i = 0; i < Abilities.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Abilities[i].Name} (Schaden: {Abilities[i].Damage}, Mana: {Abilities[i].ManaCost})");
                    }
                    int abilityChoice = int.Parse(Console.ReadLine()) - 1;

                    if (abilityChoice >= 0 && abilityChoice < Abilities.Count)
                    {
                        Ability selectedAbility = Abilities[abilityChoice];

                        // Bereichsangriff oder Einzelziel?
                        if (selectedAbility.Name != "Heilung") // Beispiel für einen Bereichsangriff
                        {
                            // Zeige verfügbare Monster an
                            int SelectedMonsterChoice = 1;
                            bool validInput = true;
                            aliveMonsters = monsters.FindAll(m => m.CurrentHealth > 0);
                            if (aliveMonsters.Count > 1)
                            {
                                Console.WriteLine("Wähle ein Monster zum Angreifen aus:");
                                for (int i = 0; i < aliveMonsters.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {aliveMonsters[i].Name} (HP: {aliveMonsters[i].CurrentHealth})");
                                }

                                // Spieler-Eingabe verarbeiten
                                validInput = int.TryParse(Console.ReadLine(), out SelectedMonsterChoice);
                            }

                            if (validInput && SelectedMonsterChoice > 0 && SelectedMonsterChoice <= aliveMonsters.Count)
                            {
                                Monster target = aliveMonsters[SelectedMonsterChoice - 1];
                                UseAbility(selectedAbility, target);

                                if (!target.IsAlive())
                                {
                                    Console.WriteLine($"{target.Name} wurde besiegt!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ungültige Eingabe!");
                            }

                        }
                        else // Heal on own player
                        {
                            UseAbility(selectedAbility, this);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Auswahl!");
                    }
                }
                else
                {
                    Console.WriteLine("Du hast keine Fähigkeiten!");
                }
            }
            aliveMonsters = monsters.Where((p) => p.CurrentHealth > 0).ToList();

        }
        public void AttackLogicMonster(List<Character> allCharacters)
        {
            List<Monster> monsters = allCharacters.OfType<Monster>().ToList();
            List<Player> players = allCharacters.OfType<Player>().ToList();
            List<Monster> aliveMonsters = monsters.FindAll(m => m.CurrentHealth > 0);
            List<Player> alivePlayers = players.FindAll(p => p.CurrentHealth > 0);

            Player playerTarget = alivePlayers[new Random().Next(0, alivePlayers.Count)];
            if (!alivePlayers.Any())
                return;
            //Ability Attack
            if (Abilities.Count > 0 && new Random().Next(0, 2) == 1) // 50% Chance, eine Fähigkeit zu verwenden
            {
                Ability monsterAbility = Abilities[new Random().Next(0, Abilities.Count)];
                //attackChoice
                UseAbility(monsterAbility, playerTarget);
            }
            else //normal Attack
            {
                AutoAttackEnemy(playerTarget);
            }

            FightLog.records.Add(new FightRecord(false, new List<Character>(allCharacters), this));
            alivePlayers = players.FindAll((p) => p.CurrentHealth > 0);
        }
        public void Attack(List<Character> characters)
        {
            //check if character is alive before attacking
            List<Monster> monsters = characters.OfType<Monster>().ToList();
            List<Player> players = characters.OfType<Player>().ToList();

            if (CurrentHealth <= 0)
                return;
            if (this is Player)
            {
                if (!monsters.FindAll(m => m.CurrentHealth > 0).Any())
                    return;
                FightLog.records.Add(new FightRecord(true, new List<Character>(characters), (Player)this, true));
                AttackLogicPlayer(characters);
            }

            else if (this is Monster)
            {
                if (!players.FindAll(p => p.CurrentHealth > 0).Any())
                    return;
                FightLog.records.Add(new FightRecord(false, new List<Character>(characters), this, true));
                AttackLogicMonster(characters);
            }

        }

        public void Defend()
        {
            Console.WriteLine($"{Name} verteidigt sich!");
        }
        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }
        public static bool CharacterAlive(List<Character> chracters)
        {
            return chracters.FindAll(c => c.IsAlive()).Any();
        }
        public void UseItem(Item item)
        {
            Console.WriteLine($"{Name} benutzt {item.Name}.");
            item.Use();
        }
        public void ProcessCooldowns(float deltatime)
        {
            foreach (var ability in Abilities)
            {
                ability.ReduceCooldown(deltatime);
            }
        }
        public void ApplyStatusEffect(StatusEffect effect)
        {
            StatusEffects.RemoveAll(e => e.Name == effect.Name);

            StatusEffects.Add(effect);
            Console.WriteLine($"{Name} ist jetzt {effect.Name} ausgesetzt!");
        }

        public void ProcessStatusEffects(float deltatime)
        {
            for (int i = StatusEffects.Count - 1; i >= 0; i--)
            {
                StatusEffects[i].Apply(this, deltatime);
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
    public class CharacterComparer : IEqualityComparer<Character>
    {
        public bool Equals(Character x, Character y) => x.Id == y.Id;


        public int GetHashCode(Character obj) => obj.Id.GetHashCode();

    }

}
