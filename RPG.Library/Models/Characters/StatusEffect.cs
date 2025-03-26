using System;

namespace RPG.Models
{
    public class StatusEffect
    {
        public string Name { get; set; }
        public float Duration { get; set; }
        public float ApplyEffectInterval { get; set; }
        public float RemainingCooldown { get; set; }
        public Action<Character> Effect { get; set; }
        private float EffectTimer { get; set; } // current EffectDelta used for statuseffect calculation

        public StatusEffect(string name, float duration, float applyEffectInterval, Action<Character> effect)
        {
            Name = name;
            Duration = duration; 
            RemainingCooldown = 0;
            Effect = effect;
            ApplyEffectInterval = applyEffectInterval;
        }

        public void Apply(Character target, float deltaTime)
        {
            EffectTimer += deltaTime;
            float interval = ApplyEffectInterval;

            if (EffectTimer >= interval)
            {
                Effect(target);
                EffectTimer -= interval;
            }
        }
        public void ReduceCooldown(float deltatime)
        {
            if (RemainingCooldown > 0)
            {
                RemainingCooldown = Math.Max(RemainingCooldown - deltatime, 0);
            }
        }
    }
}