using RPG.Library.Models;
using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Library
{
    public class CombatManager
    {
        private List<Character> _combatants;
        private static readonly float _deltaTime = 0.1f;

        public CombatManager(List<Character> participants)
        {
            _combatants = participants;
        }

        public void FightMonster(Player player, List<Monster> monsters)
        {
            AnouncmentBeforeFight(new List<Player> { player }, monsters);

            FightLog.records.Add(new FightRecord(true, new List<Player> { player }, new List<Monster>(monsters), player, true));
            while (player.CurrentHealth > 0 && monsters.Find((m) => m.IsAlive()) != null)
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
                        Console.WriteLine($"{player.Name} greift {monsters[monsterChoice].Name} an");
                        monsters[monsterChoice].CurrentHealth -= player.Strength;
                        Console.WriteLine($"{monsters[monsterChoice].Name} hat noch {monsters[monsterChoice].CurrentHealth} Lebenspunkte.");

                        if (!monsters[monsterChoice].IsAlive())
                        {
                            Console.WriteLine($"{monsters[monsterChoice].Name} wurde besiegt!");
                            player.GainExperience(monsters[monsterChoice].ExperienceReward);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Auswahl!");
                    }
                    FightLog.records.Add(new FightRecord(true, new List<Player> { player }, new List<Monster>(monsters), player));
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
                                    if (!monster.IsAlive())
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
                    FightLog.records.Add(new FightRecord(true, new List<Player> { player }, new List<Monster>(monsters), player));
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
                        ((Character)monster).AutoAttackEnemy(player);
                    }

                    if (!player.IsAlive())
                    {
                        Console.WriteLine($"\n{player.Name} wurde besiegt!\n");
                        return; // Kampf beenden
                    }
                    FightLog.records.Add(new FightRecord(false, new List<Player> { player }, new List<Monster>(monsters), player));
                }
                EndOfRound();

            }
            //check for Quest Completion
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
                foreach (var attackingPlayer in alivePlayer)
                {
                    aliveMonsters = monsters.Where((p) => p.IsAlive()).ToArray();

                    if (!aliveMonsters.Any())
                        continue;
                    FightLog.records.Add(new FightRecord(true, new List<Player>(players), new List<Monster>(monsters), attackingPlayer, true));

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
                            Console.WriteLine($"{attackingPlayer.Name} greift {monsters[monsterChoice].Name} an");
                            aliveMonsters[monsterChoice].CurrentHealth -= attackingPlayer.Strength;
                            Console.WriteLine($"{aliveMonsters[monsterChoice].Name} hat noch {aliveMonsters[monsterChoice].CurrentHealth} Lebenspunkte.");

                            if (!aliveMonsters[monsterChoice].IsAlive())
                            {
                                Console.WriteLine($"{aliveMonsters[monsterChoice].Name} wurde besiegt!");
                                attackingPlayer.GainExperience(aliveMonsters[monsterChoice].ExperienceReward);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ungültige Auswahl!");
                        }
                        FightLog.records.Add(new FightRecord(true, new List<Player>(players), new List<Monster>(monsters), attackingPlayer));
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
                        FightLog.records.Add(new FightRecord(true, new List<Player>(players), new List<Monster>(monsters), attackingPlayer));
                    }
                }
                aliveMonsters = monsters.Where((p) => p.CurrentHealth > 0).ToArray();

                // Monsteraktionen
                foreach (var monster in aliveMonsters)
                {
                    Player playerTarget = alivePlayer[new Random().Next(0, alivePlayer.Length)];
                    if (!alivePlayer.Any())
                        continue;
                    //Ability Attack
                    if (monster.Abilities.Count > 0 && new Random().Next(0, 2) == 1) // 50% Chance, eine Fähigkeit zu verwenden
                    {
                        Ability monsterAbility = monster.Abilities[new Random().Next(0, monster.Abilities.Count)];
                        //attackChoice
                        monster.UseAbility(monsterAbility, playerTarget);
                    }
                    else //normal Attack
                    {
                        ((Character)monster).AutoAttackEnemy(playerTarget);
                    }

                    if (!playerTarget.IsAlive())
                    {
                        Console.WriteLine($"\n{playerTarget.Name} wurde besiegt!\n");
                        return; // Kampf beenden
                    }
                    FightLog.records.Add(new FightRecord(false, new List<Player>(players), new List<Monster>(monsters), playerTarget));
                    alivePlayer = players.Where((p) => p.CurrentHealth > 0).ToArray();

                }

                EndOfRound();

            }
            if (monsters.Find((m) => m.CurrentHealth > 0) == null)
            {
                Console.WriteLine("Alle Monster wurden besiegt!");
            }
        }
        //characters attack is related to their attackspeed
        public void FightMonster()
        {
            AnouncmentBeforeFight(_combatants);
            Player[] alivePlayer = _combatants.OfType<Player>().ToArray();
            Monster[] aliveMonsters = _combatants.OfType<Monster>().ToArray();
            while (!Character.EndOfFight(_combatants))
            {
                _combatants.ForEach((c) => c.UpdateAttackTiming(_deltaTime, _combatants));
                EndOfRound();

                if (!aliveMonsters.ToList().FindAll((m) => m.IsAlive()).Any())
                {
                    Console.WriteLine("Alle Monster wurden besiegt!");
                }
            }

        }

        private void AnouncmentBeforeFight(List<Player> players, List<Monster> monsters)
        {
            Console.WriteLine($" \nEin Kampf beginnt zwischen ");
            Console.WriteLine("┌──────────────────────┬──────────────────────┐");

            Console.WriteLine("| {0,-20} | {1,-20} |", "Helden", "Monster");
            Console.WriteLine("├──────────────────────┼──────────────────────┤");

            int rowCount = 0;
            while (Math.Max(players.Count, monsters.Count) > rowCount)
            {
                Console.WriteLine("| {0,-20} | {1,-20} |", players.Count > rowCount ? players[rowCount].Name : "", monsters.Count > rowCount ? monsters[rowCount].Name : "");

                rowCount++;
            }

            Console.WriteLine("└──────────────────────┴──────────────────────┘");
        }
        private void AnouncmentBeforeFight(List<Character> characters)
        {
            Console.WriteLine($" \nEin Kampf beginnt zwischen ");
            Console.WriteLine("┌──────────────────────┬──────────────────────┐");

            Console.WriteLine("| {0,-20} | {1,-20} |", "Helden", "Monster");
            Console.WriteLine("├──────────────────────┼──────────────────────┤");

            List<Player> players = characters.OfType<Player>().ToList();
            List<Monster> monsters = characters.OfType<Monster>().ToList();
            int rowCount = 0;
            while (Math.Max(players.Count, monsters.Count) > rowCount)
            {
                Console.WriteLine("| {0,-20} | {1,-20} |", players.Count > rowCount ? players[rowCount].Name : "", monsters.Count > rowCount ? monsters[rowCount].Name : "");

                rowCount++;
            }
            Console.WriteLine("└──────────────────────┴──────────────────────┘");

        }
        private void EndOfRound()
        {
            _combatants.ForEach(p =>
            {
                p.ProcessAbilityCooldowns(_deltaTime);
                p.ProcessStatusEffects(_deltaTime);
            });
        }
        private Character FindNearestEnemy(Character source)
        {
            return _combatants
                .Where(c => c != source && c.IsAlive())
                .OrderBy(c => Vector3.Distance(source.Position, c.Position))
                .FirstOrDefault();
        }

        private void MoveTowardTarget(Character mover, Character target, float deltaTime)
        {
            Vector3 direction = target.Position - mover.Position;
            float distance = direction.Length();

            if (distance > 0.1f)
            {
                direction = direction / distance; // Normalize
                mover.Position += direction * mover.MovementSpeed * deltaTime;
            }
        }
    }
}
