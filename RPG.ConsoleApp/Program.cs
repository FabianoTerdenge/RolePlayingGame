// See https://aka.ms/new-console-template for more information
using System.Numerics;
using System;
using RPG;
using RPG.Models;
using RPG.Models.Quests;
using System.Collections.Generic;
using System.Linq;


// Spiel initialisieren
Game game = new Game();
game.StartGame();
//StatusEffect
StatusEffect poisonEffect = new StatusEffect("Vergiftung", 3f, (target) =>
{
    Console.WriteLine($"{target.Name} erleidet Vergiftungsschaden!");
    target.CurrentHealth -= 5;
    Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");

});
StatusEffect stunEffect = new StatusEffect("Betäubung", 2f, target =>
{
    Console.WriteLine($"{target.Name} ist betäubt und kann nicht angreifen!");
});
//Abilities
FireBallAbility fireball = new ("Feuerball", 20, 30, 3f, null);
HealAbility heal = new ("Heilung", -15, 20, 5f, null);
PoisonAbility poisonDart = new ("Giftpfeil", 10, 20, 2f, poisonEffect);
StunAbility stunAttack = new ("Betäubungsschlag", 5, 0, 3f, stunEffect);
// Monster erstellen
Monster dragon = new ("Feuer-Drache", 50, 1.51f, 1, 10, 8, 5, true, 10, 100, 10, new List<Ability> { fireball });
game.Monsters.Add(dragon);

// Spielercharakter erstellen
Player player1 = new ("Sir Lancelot", 100, 2,1, 10, 8, 5, 10, 1, 100, new List<Ability> { new FireBallAbility(fireball), heal, poisonDart,stunAttack });
Player player2 = new ("Lady Gwinevier", 100, 2,1, 10, 8, 5, 10, 1, 100, new List<Ability> { new FireBallAbility(fireball), new HealAbility(heal), new PoisonAbility(poisonDart),new StunAbility(stunAttack) });
game.Players.Add(player1);
game.Players.Add(player2);

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
var combined = new List<Character>(game.Players);
combined.AddRange(game.Monsters);
game.FightMonster(combined);
// Spiel beenden
game.EndGame();
