using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;

namespace ReKatarina.Utility
{
    public static class PermaActive
    {
        private static bool chance(int chance)
        {
            if (Damage.getrandom.Next(0, 100) <= chance)
                return true;
            return false;
        }

        public static void Execute()
        {
            #region R updater
            if (Core.GameTickCount - SpellManager.LastUltCast >= 2500)
                Damage.UnfreezePlayer();

            if (Damage.HasRBuff())
            {
                if (Player.Instance.CountEnemyChampionsInRange(SpellManager.R.Range) <= 0)
                {
                    Damage.UnfreezePlayer();
                    return;
                }

                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
                return;
            }
            #endregion
            #region Auto KS
            foreach (var e in EntityManager.Heroes.Enemies.Where(a => !a.IsDead && a.IsInRange(Player.Instance.Position, SpellManager.E.Range) && !(a.IsMinion || a.IsMonster)))
            {
                if (SpellManager.Q.IsReady() && ConfigList.Misc.KSWithQ)
                {
                    if (e.TotalShieldHealth() < Damage.GetQDamage(e))
                    {
                        SpellManager.Q.Cast(e);
                        break;
                    }
                }

                if (SpellManager.E.IsReady() && ConfigList.Misc.KSWithE && Player.Instance.HealthPercent >= ConfigList.Misc.KSMinE)
                {
                    if (e.TotalShieldHealth() < Damage.GetEDamage(e))
                    {
                        SpellManager.E.Cast(e.Position);
                        break;
                    }
                }

                if (SpellManager.Q.IsReady() && SpellManager.E.IsReady() && ConfigList.Misc.KSWithQ && ConfigList.Misc.KSWithE && Player.Instance.HealthPercent >= ConfigList.Misc.KSMinE)
                {
                    if (e.TotalShieldHealth() <= (Damage.GetQDamage(e) + Damage.GetEDamage(e)))
                    {
                        SpellManager.Q.Cast(e);
                        Core.DelayAction(() => SpellManager.E.Cast(e.Position), 100);
                        break;
                    }
                }
            }
            #endregion
            #region Auto harass
            var target = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Magical, Player.Instance.Position);
            if (target == null) return;

            if (!chance(ConfigList.Harass.AutoHarassChance)) return;
            if (SpellManager.Q.IsReady() && ConfigList.Harass.AutoHarassWithQ && !Player.Instance.IsUnderEnemyturret())
            {
                SpellManager.Q.Cast(target);
            }
            #endregion
        }
    }
}
