using RPG.Library;
using RPG.Library.Interfaces;
using RPG.Library.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
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
        public float AttackSpeed {get; set;}
        public float MovementSpeed { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int ExperiencePoints { get; set; }
        public int Gold { get; set; }
        public int Mana { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<StatusEffect> StatusEffects { get; set; }
        public Vector3 Position { get; set; }
        public Weapon EquippedWeapon { get; set; }


        public Character(string name, int health, float attackSpeed, float movementSpeed, int level, int strength, int dexterity, int intelligence, int gold, int experience, int mana, List<Ability> abilities, Vector3 position, Weapon equippedweapon)
        {
            Id = _id++;
            Name = name;
            MaxHealth = health;
            CurrentHealth = health;
            AttackSpeed = attackSpeed;
            MovementSpeed = movementSpeed;
            Level = level;
            Strength = strength;
            Dexterity = dexterity;
            Intelligence = intelligence;
            ExperiencePoints = experience;
            Gold = gold;
            Mana = mana;
            Abilities = abilities;
            StatusEffects = new List<StatusEffect>();
            Position = position;
            EquippedWeapon = equippedweapon;
        }
        public void UpdateAttackTiming(float deltaTime, List<Character> characters)
        {
            AttackTimer += deltaTime;
            float interval = AttackSpeed;

            while (AttackTimer >= interval && !EndOfFight(characters))
            {
                //TODO add move to other attack patterns but auto attack
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
            int dmg = CalculateDamage();
            Console.WriteLine($"{Name} greift {character.Name} an und verursacht {dmg} Schaden!");
            character.CurrentHealth -= dmg;
            Console.WriteLine($"{character.Name} hat noch {character.CurrentHealth} Lebenspunkte.");
            //apply statusEffect of Weapon
            if (EquippedWeapon.AppliesStatusEffect != null)
            {
                character.ApplyStatusEffect(EquippedWeapon.AppliesStatusEffect);
            }
        }
        public void AttackLogicPlayer(List<Character> allCharacters)
        {
            List<Monster> monsters = allCharacters.OfType<Monster>().ToList();
            List<Player> players = allCharacters.OfType<Player>().ToList();
            List<Monster> aliveMonsters = monsters.FindAll(m => m.IsAlive());
            List<Player> alivePlayers = players.FindAll(p => p.IsAlive());

            //TODO fix show map of characters 
            CombatMap combatMap = new CombatMap(allCharacters);
            combatMap.DrawMap();

            Console.WriteLine($"\n{Name}: Was möchtest du tun?");
            Console.WriteLine("1. Angreifen");
            Console.WriteLine("2. Spezialfähigkeit verwenden");
            Console.WriteLine("3. Bewege dich\n");
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
                //else
                //{
                //    Console.WriteLine($"{1}. {aliveMonsters[0].Name} (HP: {aliveMonsters[0].CurrentHealth})");
                //}

                if (monsterChoice >= 0 && monsterChoice < aliveMonsters.Count)
                {
                    // Find nearest enemy
                    //var target = FindNearestEnemy(attacker);

                    //if (target != null)
                    // {
                    // Move toward target if out of range
                    if (!CanAttack(aliveMonsters[monsterChoice], EquippedWeapon.Range))
                    {
                        MoveTowardTarget(aliveMonsters[monsterChoice]);
                    }
                    else
                    {
                        AutoAttackEnemy(aliveMonsters[monsterChoice]);
                        if (!aliveMonsters[monsterChoice].IsAlive())
                        {
                            Console.WriteLine($"{aliveMonsters[monsterChoice].Name} wurde besiegt!");
                            GainExperience(aliveMonsters[monsterChoice].ExperienceReward);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ungültige Auswahl!");
                }
                FightLog.Instance.AddFightRecord(new FightRecord(true, new List<Character>(allCharacters), (Player)this));
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
                                if (!CanAttack(target, selectedAbility.Range))
                                {
                                    MoveTowardTarget(aliveMonsters[SelectedMonsterChoice]);
                                }
                                else
                                {
                                    UseAbility(selectedAbility, target);

                                    if (!target.IsAlive())
                                    {
                                        Console.WriteLine($"{target.Name} wurde besiegt!");
                                    }
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
            else if (attackChoice == 3)
            {
                moveCharacter();
            }
            aliveMonsters = monsters.Where((p) => p.IsAlive()).ToList();
        }

        private void moveCharacter()
        {
            //get direction with xy input
            Console.WriteLine("\nWähle eine Wegpunkt aus: (x,y)");
            bool correctInputMovement = false;
            string movementCoordinates = "";
            while (!correctInputMovement)
            {
                movementCoordinates = Console.ReadLine();
                if (movementCoordinates.Split(",").Length == 2 && float.TryParse(movementCoordinates.Split(",")[0], out float xCoord) && float.TryParse(movementCoordinates.Split(",")[1], out float yCoord))
                    correctInputMovement = true;
                else
                    Console.WriteLine("\nBitte geben Sie eine x und eine y Koordinate ein mit einem Komma getrennt");
            }
            //TODO check input syntax
            //check if in movement range
            Vector3 movement = new Vector3(float.Parse(movementCoordinates.Split(",")[0]), float.Parse(movementCoordinates.Split(",")[1]), 0);
            if (Vector3.Distance(movement, Position) < MovementSpeed)
            {
                //move or move as wide as get Vector3 direction = target.Position - mover.Position;
                Position = movement;
            }
            else
            {
                //move as far as it gets in this direction
                Vector3 direction = movement - Position;
                float distance = direction.Length();

                direction = direction / distance; // Normalize
                Position += direction * MovementSpeed;
                //if pos should be int value
                Position = new Vector3((float)Math.Round(Position.X), (float)Math.Round(Position.Y), (float)Math.Round(Position.Z));
            }
        }

        private void MoveTowardTarget(Character target)
        {
            Vector3 direction = target.Position - Position;
            float distance = direction.Length();
            Vector3 oldPos = Position;

            if (distance > EquippedWeapon.Range)
            {
                float tempDistance = distance - (EquippedWeapon.Range - 0.1f);
                direction /= distance; // Normalize
                Position += direction * Math.Min(tempDistance, MovementSpeed);
                Console.WriteLine($"Position accurate:{Position}");
                Position = new Vector3((float)Math.Round(Position.X, MidpointRounding.AwayFromZero), (float)Math.Round(Position.Y, MidpointRounding.AwayFromZero), (float)Math.Round(Position.Z, MidpointRounding.AwayFromZero));
                Console.WriteLine($"{Name} läuft von Position:{oldPos} zu round {Position}");
            }
        }

        public void AttackLogicMonster(List<Character> allCharacters)
        {
            List<Monster> monsters = allCharacters.OfType<Monster>().ToList();
            List<Player> players = allCharacters.OfType<Player>().ToList();
            List<Monster> aliveMonsters = monsters.FindAll(m => m.IsAlive());
            List<Player> alivePlayers = players.FindAll(p => p.IsAlive());

            CombatMap combatMap = new CombatMap(allCharacters);
            combatMap.DrawMap();

            Player playerTarget = alivePlayers[new Random().Next(0, alivePlayers.Count)];
            if (!alivePlayers.Any())
                return;

            //Ability Attack
            if (Abilities.Count > 0 && new Random().Next(0, 2) == 1) // 50% Chance, eine Fähigkeit zu verwenden
            {
                Ability monsterAbility = Abilities[new Random().Next(0, Abilities.Count)];
                if (!CanAttack(playerTarget, monsterAbility.Range))
                {
                    MoveTowardTarget(playerTarget);
                }
                else
                {
                    //attackChoice
                    UseAbility(monsterAbility, playerTarget);
                }
            }
            else //normal Attack
            {
                if (!CanAttack(playerTarget, EquippedWeapon.Range))
                {
                    MoveTowardTarget(playerTarget);
                }
                else
                {
                    AutoAttackEnemy(playerTarget);
                }
            }

            FightLog.Instance.AddFightRecord(new FightRecord(false, new List<Character>(allCharacters), this));
            alivePlayers = players.FindAll((p) => p.CurrentHealth > 0);
        }
        public void Attack(List<Character> characters)
        {
            //check if character is alive before attacking
            List<Monster> monsters = characters.OfType<Monster>().ToList();
            List<Player> players = characters.OfType<Player>().ToList();

            if (!IsAlive())
                return;

            if (StatusEffects.FindAll((se) => se.Name.Equals("Betäubung")).Any())
            {
                Console.WriteLine($"{Name} ist immer noch betäubt und kann nicht angreifen");
                return;
            }
            if (this is Player)
            {
                if (!monsters.FindAll(m => m.IsAlive()).Any())
                    return;
                FightLog.Instance.AddFightRecord(new FightRecord(true, new List<Character>(characters), (Player)this, true));
                AttackLogicPlayer(characters);
            }

            else if (this is Monster)
            {
                if (!players.FindAll(p => p.IsAlive()).Any())
                    return;
                FightLog.Instance.AddFightRecord(new FightRecord(false, new List<Character>(characters), this, true));
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
        public void ProcessAbilityCooldowns(float deltatime)
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
        public bool CanAttack(Character target, float attackRange)
        {
            return target.IsAlive() && this.IsInRange(target, attackRange);
        }
        public virtual int CalculateDamage()
        {
            int baseDamage = EquippedWeapon.Damage;
            return (int)(baseDamage * (CheckCriticalHit() ? 2 : 1));
        }
        public virtual bool CheckCriticalHit()
        {
            Random _random = new Random();
            return _random.Next(100) < EquippedWeapon.CriticalChance;
        }
        public bool IsInRange(Character target, float attackRange)
        {
            //check Range with Ability or WeaponRange
            // Check basic distance first
            if (Vector3.Distance(Position, target.Position) > attackRange)
                return false;

            // Then check line of sight
            return true; //TODO implement HasLineOfSight(target); with every opstical (e.g. player, monster,npc, tree,...)
        }

        private bool HasLineOfSight(Character target)
        {
            /*   Vector3 direction = target.Position - this.Position;
               float distance = direction.magnitude;

               // Raycast to check for obstacles
               if (Physics.Raycast(this.Position, direction, out RaycastHit hit, distance))
               {
                   return hit.collider.gameObject == target.gameObject;
               }
            */
            return true;
        }
    }
    public class CharacterComparer : IEqualityComparer<Character>
    {
        public bool Equals(Character x, Character y) => x.Id == y.Id;


        public int GetHashCode(Character obj) => obj.Id.GetHashCode();

    }

}
