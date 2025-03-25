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
        //one player fight n monsters with a round stil combat system
        public void FightMonster(Player player, List<Monster> monsters)
        {
            AnouncmentBeforeFight(new List<Player> { player }, monsters);

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
        public void FightMonster(List<Player> players, List<Monster> monsters)
        {
            AnouncmentBeforeFight(players, monsters);
            Player[] alivePlayer = players.ToArray();
            Monster[] aliveMonsters = monsters.ToArray();

            while (alivePlayer.Length > 0 && aliveMonsters.Length > 0)
            {
                //alle Player greifen an
                foreach (var attackingPlayer in (List<Player>)players.Intersect(alivePlayer, new CharacterComparer())) // ToList() vermeidet Änderungen während der Iteration
                {
                    FightRecords.Add(new FightRecord(true, new List<Monster>(monsters), attackingPlayer, true));

                    // Spieleraktionen
                    Console.WriteLine($"\n{attackingPlayer.Name}: Was möchtest du tun?");
                    Console.WriteLine("1. Angreifen");
                    Console.WriteLine("2. Spezialfähigkeit verwenden\n");
                    string attackChoice = Console.ReadLine();

                    if (attackChoice == "1") // Normal Attack
                    {
                        // Zielmonster auswählen
                        int monsterChoice = 0;
                        if (aliveMonsters.Length > 1)
                        {
                            Console.WriteLine("Wähle ein Monster zum Angreifen:");
                            for (int i = 0; i < aliveMonsters.Length; i++)
                            {
                                Console.WriteLine($"{i + 1}. {aliveMonsters[i].Name} (HP: {aliveMonsters[i].CurrentHealth})");
                            }
                            monsterChoice = int.Parse(Console.ReadLine()) - 1;
                        }

                        if (monsterChoice >= 0 && monsterChoice < aliveMonsters.Length)
                        {
                            attackingPlayer.Attack();
                            aliveMonsters[monsterChoice].CurrentHealth -= attackingPlayer.Strength;
                            Console.WriteLine($"{aliveMonsters[monsterChoice].Name} hat noch {aliveMonsters[monsterChoice].CurrentHealth} Lebenspunkte.");

                            if (aliveMonsters[monsterChoice].CurrentHealth <= 0)
                            {
                                Console.WriteLine($"{aliveMonsters[monsterChoice].Name} wurde besiegt!");
                                attackingPlayer.GainExperience(aliveMonsters[monsterChoice].ExperienceReward);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ungültige Auswahl!");
                        }
                        FightRecords.Add(new FightRecord(true, new List<Monster>(monsters), attackingPlayer));
                    }
                    else if (attackChoice == "2") // Ability Cast
                    {
                        if (attackingPlayer.Abilities.Count > 0)
                        {
                            Console.WriteLine("\nWähle eine Fähigkeit:");
                            for (int i = 0; i < attackingPlayer.Abilities.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {attackingPlayer.Abilities[i].Name} (Schaden: {attackingPlayer.Abilities[i].Damage}, Mana: {attackingPlayer.Abilities[i].ManaCost})");
                            }
                            int abilityChoice = int.Parse(Console.ReadLine()) - 1;

                            if (abilityChoice >= 0 && abilityChoice < attackingPlayer.Abilities.Count)
                            {
                                Ability selectedAbility = attackingPlayer.Abilities[abilityChoice];

                                // Bereichsangriff oder Einzelziel?
                                if (selectedAbility.Name != "Heilung") // Beispiel für einen Bereichsangriff
                                {
                                    // Zeige verfügbare Monster an
                                    int SelectedMonsterChoice = 1;
                                    bool validInput = true;
                                    if (aliveMonsters.Length > 1)
                                    {
                                        Console.WriteLine("Wähle ein Monster zum Angreifen aus:");
                                        for (int i = 0; i < aliveMonsters.Length; i++)
                                        {
                                            Console.WriteLine($"{i + 1}. {aliveMonsters[i].Name} (HP: {aliveMonsters[i].CurrentHealth})");
                                        }

                                        // Spieler-Eingabe verarbeiten
                                        validInput = int.TryParse(Console.ReadLine(), out SelectedMonsterChoice);
                                    }

                                    if (validInput && SelectedMonsterChoice > 0 && SelectedMonsterChoice <= aliveMonsters.Length)
                                    {
                                        Monster target = aliveMonsters[SelectedMonsterChoice - 1];
                                        attackingPlayer.UseAbility(selectedAbility, target);

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
                                    attackingPlayer.UseAbility(selectedAbility, attackingPlayer);
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
                        FightRecords.Add(new FightRecord(true, new List<Monster>(monsters), attackingPlayer));
                    }
                }
                aliveMonsters = monsters.Where((p) => p.CurrentHealth > 0).ToArray();

                // Monsteraktionen
                foreach (var monster in (List<Monster>)monsters.Intersect(aliveMonsters, new CharacterComparer()))  // ToList() vermeidet Änderungen während der Iteration
                {
                    alivePlayer = players.Where((p) => p.CurrentHealth > 0).ToArray();
                    Player playerTarget = alivePlayer[new Random().Next(0, alivePlayer.Length)];
                    //Ability Attack
                    if (monster.Abilities.Count > 0 && new Random().Next(0, 2) == 1) // 50% Chance, eine Fähigkeit zu verwenden
                    {
                        Ability monsterAbility = monster.Abilities[new Random().Next(0, monster.Abilities.Count)];
                        //attackChoice
                        monster.UseAbility(monsterAbility, alivePlayer[new Random().Next(0, alivePlayer.Length)]);
                    }
                    else //normal Attack
                    {
                        monster.AttackPlayer(alivePlayer[new Random().Next(0, alivePlayer.Length)]);
                    }

                    if (playerTarget.CurrentHealth <= 0)
                    {
                        Console.WriteLine($"\n{playerTarget.Name} wurde besiegt!\n");
                        return; // Kampf beenden
                    }
                    FightRecords.Add(new FightRecord(false, new List<Monster>(monsters), playerTarget));
                }
                alivePlayer = players.Where((p) => p.CurrentHealth > 0).ToArray();

                EndOfRound();

            }
            //check for Quest Completion
            Quests.ForEach(q => q.isCompleted(FightRecords));
            if (monsters.Find((m) => m.CurrentHealth > 0) == null)
            {
                Console.WriteLine("Alle Monster wurden besiegt!");
            }
        }

        private void AnouncmentBeforeFight(List<Player> players, List<Monster> monsters)
        {
            Console.WriteLine($" \nEin Kampf beginnt zwischen ");
            Console.WriteLine("| {0,-20} | {1,-20} |", "Helden", "Monster");
            Console.WriteLine("| -------------------- | -------------------- |");

            int rowCount = 0;
            while (Math.Max(players.Count, monsters.Count) > rowCount)
            {
                Console.WriteLine("| {0,-20} | {1,-20} |", players[rowCount].Name, monsters[rowCount].Name);

                rowCount++;
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
