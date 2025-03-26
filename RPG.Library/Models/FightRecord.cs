using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace RPG.Models
{
    public class FightRecord
    {
        private static int _FightId = 0;
        public int FightId { get; set; }
        public bool PlayerAttack { get; set; }
        public Character Attacker { get; set; }
        public List<Character> AllCharacters { get; set; }
        public FightRecord(bool playerAttack,  List<Character> opponents, Character attacker,bool newFight=false)
        {
            PlayerAttack = playerAttack;
            Attacker = attacker;
            AllCharacters = opponents;
            if (newFight)
                _FightId++;
            FightId = _FightId;

        }
        public FightRecord(bool playerAttack, List<Player> players, List<Monster> monsters, Character attacker, bool newFight = false)
        {
            PlayerAttack = playerAttack;
            Attacker = attacker;
            AllCharacters = new List<Character>();
            AllCharacters.AddRange(players.Cast<Character>());
            AllCharacters.AddRange(monsters.Cast<Character>());
            if (newFight)
                _FightId++;
            FightId = _FightId;

        }
    }
}