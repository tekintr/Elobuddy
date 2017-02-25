using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using ReWarwick.Utils;

namespace ReWarwick
{
    class Damage
    {
        public static double GetQDamage(Obj_AI_Base target)
        {
            if (SpellManager.Q.IsReady())
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 0.06f, 0.07f, 0.08f, 0.09f, 0.1f }[SpellManager.Q.Level] * Player.Instance.MaxHealth + (1.2f * Player.Instance.TotalAttackDamage) + Player.Instance.TotalMagicalDamage));
            return 0;
        }

        public static double GetRDamage(Obj_AI_Base target)
        {
            if (SpellManager.R.IsReady())
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 175, 350, 525 }[SpellManager.R.Level] + 167.5f));
            return 0;
        }

        public static double GetTotalDamage(Obj_AI_Base target)
        {
            var damage = 0.0;
            damage += GetQDamage(target);
            damage += GetRDamage(target);
            damage += Player.Instance.GetAutoAttackDamage(target, true) * 2;
            return damage;
        }
    }
}
