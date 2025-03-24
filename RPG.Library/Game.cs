using RPG.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{

    public class Game
    {
        public List<Player> Players { get; set; }
        public List<Quest> Quests { get; set; }
        public List<Monster> Monsters { get; set; }
        public List<FightRecord> FightRecords { get; set; }

        public Game()
        {
            Players = new List<Player>();
            Quests = new List<Quest>();
            Monsters = new List<Monster>();
            FightRecords = new List<FightRecord>();
        }

        public void StartGame()
        {
            Console.WriteLine("Das Spiel beginnt!");
        }

        public void EndGame()
        {
            Console.WriteLine("Das Spiel endet.");
        }

        public void AddQuest(Quest quest)
        {
            Quests.Add(quest);
            Console.WriteLine($"Quest '{quest.Title}' wurde hinzugefügt.");
        }
        public void FightMonster(Player player, List<Monster> monsters)
        {
            if (monsters.Count == 1)
                Console.WriteLine($" \nEin Kampf beginnt zwischen {player.Name} und {monsters[0].Name}!\n");
            else
                Console.WriteLine($" \nEin Kampf beginnt zwischen {player.Name} und {monsters.Count} Monstern!\n");
            
            FightRecords.Add(new FightRecord(true, new List<Monster>(monsters), player, true));
            while (player.CurrentHealth > 0 && monsters.Find((m) => m.CurrentHealth > 0) != null)
            {
                // Spieleraktionen
                Console.WriteLine($"\n{player.Name}: Was möchtest du tun?");
                Console.WriteLine("1. Angreifen");
                Console.WriteLine("2. Spezialfähigkeit verwenden\n");
                string attackChoice = Console.ReadLine();

                if (attackChoice == "1") // Normal Attack
                {
                    // Zielmonster auswählen
                    int monsterChoice = 0;
                    if (monsters.Count > 1)
                    {
                        Console.WriteLine("Wähle ein Monster zum Angreifen:");
                        for (int i = 0; i < monsters.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {monsters[i].Name} (HP: {monsters[i].CurrentHealth})");
                        }
                        monsterChoice = int.Parse(Console.ReadLine()) - 1;
                    }

                    if (monsterChoice >= 0 && monsterChoice < monsters.Count)
                    {
                        player.Attack();
                        monsters[monsterChoice].CurrentHealth -= player.Strength;
                        Console.WriteLine($"{monsters[monsterChoice].Name} hat noch {monsters[monsterChoice].CurrentHealth} Lebenspunkte.");

                        if (monsters[monsterChoice].CurrentHealth <= 0)
                        {
                            Console.WriteLine($"{monsters[monsterChoice].Name} wurde besiegt!");
                            player.GainExperience(monsters[monsterChoice].ExperienceReward);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Auswahl!");
                    }
                    FightRecords.Add(new FightRecord(true, new List<Monster>(monsters), player));
                }
                else if (attackChoice == "2") // Ability Cast
                {
                    if (player.Abilities.Count > 0)
                    {
                        Console.WriteLine("\nWähle eine Fähigkeit:");
                        for (int i = 0; i < player.Abilities.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {player.Abilities[i].Name} (Schaden: {player.Abilities[i].Damage}, Mana: {player.Abilities[i].ManaCost})");
                        }
                        int abilityChoice = int.Parse(Console.ReadLine()) - 1;

                        if (abilityChoice >= 0 && abilityChoice < player.Abilities.Count)
                        {
                            Ability selectedAbility = player.Abilities[abilityChoice];

                            // Bereichsangriff oder Einzelziel?
                            if (selectedAbility.Name != "Heilung") // Beispiel für einen Bereichsangriff
                            {
                                /* //attack all monsters
                                 * Console.WriteLine($"{selectedAbility.Name} Angriff auf alle Monster!");
                                foreach (var monster in monsters)
                                {
                                    player.UseAbility(ability, monster);
                                    if (monster.CurrentHealth <= 0)
                                    {
                                        Console.WriteLine($"{monster.Name} wurde besiegt!");
                                        player.GainExperience(monster.ExperienceReward);
                                    }
                                }*/
                                // Zeige verfügbare Monster an
                                int SelectedMonsterChoice = 1;
                                bool validInput = true;
                                if (monsters.Count > 1)
                                {
                                    Console.WriteLine("Wähle ein Monster zum Angreifen aus:");
                                    for (int i = 0; i < monsters.Count; i++)
                                    {
                                        Console.WriteLine($"{i + 1}. {monsters[i].Name} (HP: {monsters[i].CurrentHealth})");
                                    }

                                    // Spieler-Eingabe verarbeiten
                                    validInput = int.TryParse(Console.ReadLine(), out SelectedMonsterChoice);
                                }

                                if (validInput && SelectedMonsterChoice > 0 && SelectedMonsterChoice <= monsters.Count)
                                {
                                    Monster target = monsters[SelectedMonsterChoice - 1];
                                    player.UseAbility(selectedAbility, target);

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
                            else // Heal
                            {
                                player.UseAbility(selectedAbility, player);
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
                    FightRecords.Add(new FightRecord(true, new List<Monster>(monsters), player));
                }

                // Monsteraktionen
                foreach (var monster in monsters.ToList()) // ToList() vermeidet Änderungen während der Iteration
                {
                    if (monster.Abilities.Count > 0 && new Random().Next(0, 2) == 1) // 50% Chance, eine Fähigkeit zu verwenden
                    {
                        Ability monsterAbility = monster.Abilities[new Random().Next(0, monster.Abilities.Count)];
                        monster.UseAbility(monsterAbility, player);
                    }
                    else
                    {
                        monster.AttackPlayer(player);
                    }

                    if (player.CurrentHealth <= 0)
                    {
                        Console.WriteLine($"\n{player.Name} wurde besiegt!\n");
                        return; // Kampf beenden
                    }
                    FightRecords.Add(new FightRecord(false, new List<Monster>(monsters), player));
                }
                EndOfRound();

            }
            //check for Quest Completion
            Quests.ForEach(q => q.isCompleted(FightRecords));
            if (monsters.Find((m) => m.CurrentHealth > 0) == null)
            {
                Console.WriteLine("Alle Monster wurden besiegt!");
            }
        }

        private void EndOfRound()
        {
            Players.ForEach(p =>
            {
                p.ProcessCooldowns();
                p.ProcessStatusEffects();
            });
            Monsters.ForEach(m =>
            {
                m.ProcessCooldowns();
                m.ProcessStatusEffects();
            });
        }
    }
}
