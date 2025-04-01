using RPG.Library.Interfaces;
using RPG.Library;
using RPG.Library.Models;
using RPG.Library.Models.Characters;
using RPG.Models;
using RPG.Models.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RPG.Library.Models.Abilities;

namespace RPG
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public List<Quest> Quests { get; set; }
        public List<Monster> Monsters { get; set; }
        public TradingSystem tradingTest { get; set; }
        private DialogueManager DialogueManager { get; set; }

        public Game()
        {
            Players = new List<Player>();
            Quests = new List<Quest>();
            Monsters = new List<Monster>();
            tradingTest = new TradingSystem();
        }

        public void StartGame()
        {
            Console.WriteLine("Das Spiel beginnt!");

            // Monster erstellen
            Monster dragon = new("Feuer-Drache", 50, 1.51f, 3.5f, 10, 10, 0.15f, 1, 10, 8, 5, 0, true, 10, 100, 100, new List<Ability> { AbilityFactory.CreateFireBall() }, new Vector3(1, 1, 0), Weapon.Claws); // (x,y) 2D opsition,z=Character Height
            Monsters.Add(dragon);

            // Spielercharakter erstellen
            Player player1 = new("Sir Lancelot", 100, 2f, 1.5f, 5, 5, 0.1f, 1, 10, 8, 5, 8, 10, 1, 100, new List<Ability> { AbilityFactory.CreateFireBall(), AbilityFactory.CreateHeal(), AbilityFactory.CreatePoisonDart(), AbilityFactory.CreateStunAbility() }, new Vector3(0, 0, 0), Weapon.Longsword);
            Player player2 = new("Lady Gwinevier", 100, 2f, 1.5f, 5, 5, 0.1f, 1, 10, 8, 5, 11, 10, 1, 100, new List<Ability> { AbilityFactory.CreateFireBall(), AbilityFactory.CreateHeal(), AbilityFactory.CreateBlessingAbility(), AbilityFactory.CreatePoisonDart(), AbilityFactory.CreateStunAbility() }, new Vector3(1, 0, 0), Weapon.Longsword);
            Players.Add(player1);
            //Players.Add(player2);

            //Quest erstellen
            Quest quest = new KillFireDragon("Feuer-Drachen besiegen", "Besiege den Feuer-Drachen im Wald.", new List<Monster> { dragon }, 100);
            AddQuest(quest);

            //Gegenstände
            Potion potion = new Potion("Heiltrank", 0.5f, 20, 25);
            Potion manaPotion = new Potion("Manatrank", 0.5f, 20, 25);
            Console.WriteLine($"{player1.Name} Inventory:");
            ((ITrader)player1).AddItem(potion);
            ((ITrader)player1).ShowInventory();

            ITrader merchant = new Merchant("Brandur Tukk", 100, 0.3f, 0.5f, 1, 0, 0.01f, 1, 1, 2, 10, 9, 100, 1, 0, new List<Ability>(), new Vector3(-1, -1, -1), Weapon.Fists);
            merchant.RestockInventory(new List<Item>() { potion, potion, potion, manaPotion });
            merchant.ShowInventory();

            ITrader blacksmith = new Merchant("Blacksmith Hekler & Koch", 100, 0.3f, 0.5f, 7, 0, 0.005f, 1, 1, 2, 10, 5, 100, 1, 0, new List<Ability>(), new Vector3(-1, -1, -1), Weapon.Fists);
            blacksmith.AddItem(new Item("Iron Sword", 12f, 30));
            blacksmith.AddItem(new Item("Steel Shield", 20f, 25));

            // Spieleraktionen
            var combined = new List<Character>(Players);
            combined.AddRange(Monsters);

            CombatManager combatManager = new CombatManager(combined);
            combatManager.FightMonster();
            StartConversation(player1);
            // Spiel beenden
            EndGame();
        }
        private bool IsConversationEnded()
        {
            string currentId = DialogueManager.CurrentState.Id;
            return currentId == "exit" || currentId == "combat";
        }
        public void StartConversation(Character player)
        {
            var dialogueTree = GameDialogue.CreateDialogueTree();
            DialogueManager = new DialogueManager(dialogueTree, "start", player);

            while (true)
            {
                int availableOptions = DialogueManager.DisplayCurrentState();

                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.Escape)
                {
                    DialogueManager.GoBack();
                    continue;
                }
                if (availableOptions == 1)
                {
                    DialogueManager.SelectOption(0);
                }
                if (char.IsDigit(input.KeyChar))
                {
                    int selection = input.KeyChar - '0' - 1;
                    DialogueManager.SelectOption(selection);
                }

                // Check for end state
                if (IsConversationEnded())
                {
                    break;
                }
            }
        }
        public void EndGame()
        {
            Console.WriteLine("Das Spiel endet.");
        }

        public void AddQuest(Quest quest)
        {
            Quests.Add(quest);
            FightLog.Instance.Attach(quest);
            Console.WriteLine($"Quest '{quest.Title}' wurde hinzugefügt.");
        }

    }
}

//TODO Exp for all Players or only for the player that slayed a monster ?