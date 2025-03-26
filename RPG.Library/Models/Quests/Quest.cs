using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Models
{

    public abstract class Quest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Monster> RequirementForSuccess { get; set; }
        public int Reward { get; set; }
        public bool IsCompleted { get; set; }

        public Quest(string title, string description, List<Monster> requirementForSuccess, int reward)
        {
            Title = title;
            Description = description;
            RequirementForSuccess = requirementForSuccess;
            Reward = reward;
            IsCompleted = false;
        }

        public abstract void isCompleted();
    }
}
