using RPG.Library.Models;
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

        public override void isCompleted()
        {
            foreach (FightRecord fr in FightLog.Instance.GetLogs())
            {
                List<Character> dragon = fr.AllCharacters.FindAll(m => m.Name.Equals("Feuer-Drache"));
                if (dragon.Any())
                {
                    if (dragon.FindAll((d) => !d.IsAlive()).Count() > 0)
                    {
                        IsCompleted = true;
                        FightLog.Instance.Detach(this);
                        Console.WriteLine($"Quest '{Title}' abgeschlossen!");
                        //TODO player get some kind of reward
                        //attach quest to single player rather than globally
                        break;
                    }
                }
            }
        }

        public override void Update()
        {
            isCompleted();
        }
    }
}