using System;

namespace RPG.Models
{
    public class StatusEffect
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public Action<Character> Effect { get; set; }

        public StatusEffect(string name, int duration, Action<Character> effect)
        {
            Name = name;
            Duration = duration;
            Effect = effect;
        }

        public void Apply(Character target)
        {
            Effect(target);
            Duration--;
        }
    }
}