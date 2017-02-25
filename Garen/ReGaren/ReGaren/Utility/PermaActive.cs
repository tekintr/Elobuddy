using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;

namespace ReGaren.Utility
{
    public static class PermaActive
    {
        public static void Execute()
        {
            // Skin manager
            if (ConfigList.Misc.GetSkinManagerStatus && Player.Instance.SkinId != ConfigList.Misc.GetSkinManager)
            {
                Player.Instance.SetSkinId(ConfigList.Misc.GetSkinManager);
            }

            // Auto KS 
            if (!SpellManager.R.IsReady() || !ConfigList.Misc.KSWithR)
                return;

            var target = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Magical, Player.Instance.Position);
            if (target != null)
            {
                if (Damage.GetRDamage(target) - 5 >= target.Health && !target.IsInvulnerable)
                {
                    SpellManager.R.Cast(target);
                }
            }
        }
    }
}
