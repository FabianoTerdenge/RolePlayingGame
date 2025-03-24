// See https://aka.ms/new-console-template for more information
using System.Numerics;
using System;
using RPG;
using RPG.Models;
using RPG.Models.Quests;
using System.Collections.Generic;


// Spiel initialisieren
Game game = new Game();
game.StartGame();
//StatusEffect
StatusEffect poisonEffect = new StatusEffect("Vergiftung", 3, (target) =>
{
    Console.WriteLine($"{target.Name} erleidet Vergiftungsschaden!");
    target.CurrentHealth -= 5;
    Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");

});
StatusEffect stunEffect = new StatusEffect("Betäubung", 2, target =>
{
    Console.WriteLine($"{target.Name} ist betäubt und kann nicht angreifen!");
});
//Abilities
Ability fireball = new FireBallAbility("Feuerball", 20, 30, 3, null);
Ability heal = new HealAbility("Heilung", -15, 20, 5, null); 
Ability poisonDart = new PoisonAbility("Giftpfeil", 10, 20, 2, poisonEffect);
Ability stunAttack = new StunAbility("Betäubungsschlag", 5, 0, 3, stunEffect);
// Monster erstellen
Monster dragon = new Monster("Feuer-Drache", 50, 1, 10, 8, 5, true, 10, 100, 10, new List<Ability> { fireball });
game.Monsters.Add(dragon);

// Spielercharakter erstellen
Player player = new Player("Held", 100, 1, 10, 8, 5, 10, 1, 100, new List<Ability> { fireball, heal, poisonDart,stunAttack });
game.Players.Add(player);

//Quest erstellen
Quest quest = new KillFireDragon("Feuer-Drachen besiegen", "Besiege den Feuer-Drachen im Wald.", new List<Monster> { dragon }, 100);
game.AddQuest(quest);

// Inventar erstellen und Gegenstände hinzufügen
Inventory inventory = new Inventory();
Weapon sword = new Weapon("Schwert", 2.5f, 50, 15, 1);
Potion potion = new Potion("Heiltrank", 0.5f, 20, 25);

inventory.AddItem(sword);
inventory.AddItem(potion);
inventory.DisplayItems();

// Spieleraktionen
game.FightMonster(player, game.Monsters);

// Spiel beenden
game.EndGame();
