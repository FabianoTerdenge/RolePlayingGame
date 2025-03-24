using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RPG.Models
{
    public class FightRecord
    {
        private static int _FightId = 0;
        public int FightId { get; set; }
        public bool PlayerAttack { get; set; }
        public Player Player { get; set; }
        public List<Monster> Monsters { get; set; }
        public FightRecord(bool playerAttack,  List<Monster> monsters, Player player,bool newFight=false)
        {
            PlayerAttack = playerAttack;
            Monsters = monsters;
            Player = player;
            if (newFight)
                _FightId++;
            FightId = _FightId;

        }
    }
}