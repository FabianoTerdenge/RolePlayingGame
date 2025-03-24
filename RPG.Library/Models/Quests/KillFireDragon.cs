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
            foreach (FightRecord fr in fightRecords)
            {
                List<Monster> dragon = fr.Monsters.FindAll(m => m.Name.Equals("Feuer-Drache"));
                if (dragon.Any())
                {
                    if (dragon.FindAll((d) => d.CurrentHealth <= 0).Count() > 0)
                    {
                        IsCompleted = true;
                        Console.WriteLine($"Quest '{Title}' abgeschlossen!");
                        //player get some kind of reward
                        break;
                    }
                }
            }
        }
    }
}