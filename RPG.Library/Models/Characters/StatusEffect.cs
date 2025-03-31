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
        //StatusEffect
        public static readonly StatusEffect poisonEffect = new StatusEffect("Vergiftung", 3f, 1f, (target) =>
        {
            Console.WriteLine($"{target.Name} erleidet Vergiftungsschaden!");
            target.CurrentHealth -= 5;
            Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");
        });
        public static readonly StatusEffect bleedEffect = new StatusEffect("Blutung", 6f, 1f, (target) =>
        {
            Console.WriteLine($"{target.Name} erleidet Blutungsschaden!");
            target.CurrentHealth -= 3;
            Console.WriteLine($"{target.Name} hat noch {target.CurrentHealth} Lebenspunkte.");
        });
        public static readonly StatusEffect stunEffect = new StatusEffect("Betäubung", 2f, 1f, target =>
        {
            Console.WriteLine($"{target.Name} ist betäubt und kann nicht angreifen!");
        });
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