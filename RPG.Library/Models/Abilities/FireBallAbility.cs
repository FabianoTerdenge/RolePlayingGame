using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace RPG.Models
{
    public class FireBallAbility : Ability
    {
            public FireBallAbility(string name, int damage, int manaCost, int cooldown) : base(name, damage, manaCost, cooldown)
            {
            }
            public override void Use(Character target)
            {
                Console.WriteLine($"{Name} wurde auf {target.Name} angewendet und verursacht {Damage} Schaden!");
                target.CurrentHealth -= Damage;
            }
        }
    }
