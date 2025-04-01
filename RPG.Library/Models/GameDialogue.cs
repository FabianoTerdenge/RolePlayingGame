using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Library.Models
{
    public class GameDialogue
    {
        public static Dictionary<string, DialogueState> CreateDialogueTree()
        {
            return new Dictionary<string, DialogueState>
            {
                ["start"] = new DialogueState
                {
                    Id = "start",
                    Text = "Guard: Halt! Who goes there?",
                    Options = new List<DialogueOption>
                {
                    new DialogueOption
                    {
                        Text = "[Charisma] Just a friendly traveler",
                        NextStateId = "charisma_approach",
                        IsAvailable = (Character player) => player.Charisma >= 5
                    },
                    new DialogueOption
                    {
                        Text = "[Intimidate] Out of my way!",
                        NextStateId = "intimidate_approach",
                        IsAvailable = (Character player) => player.Strength >= 7
                    },
                    new DialogueOption
                    {
                        Text = "[Neutral] I'm here to see the king",
                        NextStateId = "neutral_approach"
                    }
                }
                },

                ["charisma_approach"] = new DialogueState
                {
                    Id = "charisma_approach",
                    Text = "Guard: Well, you seem harmless enough. What brings you here?",
                    OnEnter = (Character player) => { player.Reputation += 5; },
                    Options = new List<DialogueOption>
                {
                    new DialogueOption
                    {
                        Text = "I'm a merchant with rare goods",
                        NextStateId = "merchant_path"
                    },
                    new DialogueOption
                    {
                        Text = "I'm lost and need directions",
                        NextStateId = "directions_path"
                    },
                    new DialogueOption
                    {
                        Text = "Never mind, I'll be going",
                        NextStateId = "exit"
                    }
                }
                },
                ["directions_path"] = new DialogueState
                {
                    Id = "directions_path",
                    Text = "Guard: Well, you go straight to the clock tower. Right before you go right until you see a tavern with a black seagle as it flag.",
                    OnEnter = (Character player) => { player.Reputation += 5; },
                    Options = new List<DialogueOption>
                {
                    new DialogueOption
                    {
                        Text = "Weiter",
                        NextStateId = "exit"
                    }
                    }
                },

                ["intimidate_approach"] = new DialogueState
                {
                    Id = "intimidate_approach",
                    Text = "Guard: You'll regret that! (The guard reaches for his sword)",
                    OnEnter = (Character player) => Console.WriteLine($"{player.Name} :'intimidating stare'"),//CombatSystem.StartCombat(Guard.Instance),
                    Options = new List<DialogueOption>
                {
                    new DialogueOption
                    {
                        Text = "Fight!",
                        NextStateId = "combat"
                    },
                    new DialogueOption
                    {
                        Text = "Apologize and try to de-escalate",
                        NextStateId = "apologize",
                        IsAvailable = (Character player) => player.Charisma >= 3
                    }
                }
                },
                ["exit"] = new DialogueState
                {
                    Id = "exit",
                    Text = "",
                    Options = new List<DialogueOption>{}
                },
            };
        }
    }
}
