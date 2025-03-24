// See https://aka.ms/new-console-template for more information
using System.Numerics;
using System;
using RPG;
using RPG.Models;
using RPG.Models.Quests;
using System.Collections.Generic;

class Program
{

    static void Main(string[] args)
    {
        // Spiel initialisieren
        Game game = new Game();
        game.StartGame();
        //Abilities
        Ability fireball = new FireBallAbility("Feuerball", 20, 30, 3);
        Ability heal = new HealAbility("Heilung", -15, 20, 5); // Negative Damage für Heilung
        // Monster erstellen
        Monster dragon = new Monster("Feuer-Drache", 50, 1, 10, 8, 5,true, 10, 100, 10, new List<Ability> { fireball });
        game.Monsters.Add(dragon);

        // Spielercharakter erstellen
        Player player = new Player("Held", 100, 1, 10, 8, 5,10,1,100,new List<Ability> { fireball,heal});
        game.Players.Add(player);

        //Quest erstellen
        Quest quest = new KillFireDragon("Feuer-Drachen besiegen", "Besiege den Feuer-Drachen im Wald.", new List<Monster>{ dragon }, 100);
        game.AddQuest(quest);

        // Inventar erstellen und Gegenstände hinzufügen
        Inventory inventory = new Inventory();
        Weapon sword = new Weapon("Schwert", 2.5f, 50, 15, 1);
        Potion potion = new Potion("Heiltrank", 0.5f, 20, 25);
        
        inventory.AddItem(sword);
        inventory.AddItem(potion);
        inventory.DisplayItems();
        
        // Spieleraktionen
        game.FightMonster(player,game.Monsters);

        // Spiel beenden
        game.EndGame();
    }

}