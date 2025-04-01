using RPG.Library.Models.Characters;
using RPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Library.Models.Abilities
{
    public static class AbilityFactory
    {
        public static Ability CreateFireBall() => new FireBallAbility("Feuerball", 20, 10f, 30, 3f, null);
        public static Ability CreateHeal() => new HealAbility("Heilung", -15, 3f, 20, 5f, null);
        public static Ability CreatePoisonDart() => new PoisonAbility("Giftpfeil", 10, 5f, 20, 2f, StatusEffectFactory.CreatePoisonEffect());
        public static Ability CreateStunAbility() => new StunAbility("Betäubungsschlag", 5, 1.5f, 0, 1f, StatusEffectFactory.CreateStunEffect());
        public static Ability CreateBlessingAbility() => new BlessingAbility("Läuterung", 5, 1.5f, 0, 1f, null);

    }
}
