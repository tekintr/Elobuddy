using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;

namespace ReKatarina.Utility
{
    public static class Harass
    {
        public static void Execute()
        {
            if (!SpellManager.Q.IsReady() || !ConfigList.Harass.HarassWithQ)
                return;

            var target = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Mixed, Player.Instance.Position);
            if (target != null)
            {
                SpellManager.Q.Cast(target);
            }
        }
    }
}
