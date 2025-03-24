using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models.Quests
{
    public class KillFireDragon : Quest
    {
        public KillFireDragon(string title, string description, List<Monster> requirementForSuccess, int reward) : base(title, description, requirementForSuccess, reward)
        {

        }

        public override void isCompleted(List<FightRecord> fightRecords)
        {
        foreach (var record in fightRecords)
        {
            if (record.PlayerAttack)
            {
                bool foundFireDragonFight = false;

                foreach (FightRecord item in fightRecords)
                {
                    if (item.Monsters.FindAll(m => m.Name.Equals("Feuer-Drache")).Any())
                    {
                        foundFireDragonFight = true;
                    }
                }
                if (!foundFireDragonFight) { continue; }
                //check fightId for dragon defeat
                int questFightId = record.FightId;
                FightRecord lastFightRecord = fightRecords.FindLast(fr => fr.FightId == questFightId);
                if (lastFightRecord.Monsters.Count != 0) { continue; }
                IsCompleted = true;
                Console.WriteLine($"Quest '{Title}' abgeschlossen!");
                //player get some kind of reward

            }
        }
    }
    }
}