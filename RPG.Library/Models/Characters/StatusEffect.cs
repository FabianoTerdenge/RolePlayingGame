using System;

namespace RPG.Models
{
    public class StatusEffect
    {
        public string Name { get; set; }
        public float Duration { get; set; }
        public Action<Character> Effect { get; set; }

        public StatusEffect(string name, float duration, Action<Character> effect)
        {
            Name = name;
            Duration = duration;
            Effect = effect;
        }

        public void Apply(Character target, float deltatime)
        {
            Effect(target);
            Duration-= deltatime;
        }
    }
}