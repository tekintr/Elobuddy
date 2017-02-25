using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;
using ReWarwick.Utils;

namespace ReWarwick.Modes
{
    public static class PermaActive
    {
        private static bool chance(int chance)
        {
            return (Other.GetRandom.Next(0, 100) <= chance);
        }

        public static void Execute()
        {
            #region R Range update 
            SpellManager.R.Range = (uint)(Player.Instance.MoveSpeed * 2);
            #endregion
            #region KillSteal
            foreach (var e in EntityManager.Heroes.Enemies.Where(h => h.IsValid && h.IsAlive() && h.IsInRange(Player.Instance.Position, SpellManager.Q.Range) && !h.IsInvulnerable))
            {
                float health = Prediction.Health.GetPrediction(e, 500);
                if (Config.Misc.Menu.GetCheckBoxValue("Config.Misc.KillSteal.Q") && SpellManager.Q.IsReady() && health <= Damage.GetQDamage(e))
                {
                    SpellManager.Q.Cast(e);
                    break;  
                }
            }
            #endregion
            #region Auto harass
            if (Player.Instance.HealthPercent < Config.Harass.Menu.GetSliderValue("Config.AutoHarass.Health") || Player.Instance.CountEnemyChampionsInRange(800) > Config.Harass.Menu.GetSliderValue("Config.AutoHarass.Enemies")) return;

            var target = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Mixed);
            if (target == null) return;
            if (Player.Instance.Position.IsUnderEnemyTurret() || Player.Instance.Position.IsGrass() && Player.Instance.CountAllyChampionsInRange(150) >= 1 && !target.Position.IsGrass()) return; // anti trap destroyer Fappa

            if (chance(Config.Harass.Menu.GetSliderValue("Config.AutoHarass.Q.Chance")) && Config.Harass.Menu.GetCheckBoxValue("Config.AutoHarass.Q.Status") && SpellManager.Q.IsReady() && Player.Instance.ManaPercent >= Config.Harass.Menu.GetSliderValue("Config.Harass.Q.Mana"))
            {
                SpellManager.Q.Cast(target);
            }

            if (chance(Config.Harass.Menu.GetSliderValue("Config.AutoHarass.E.Chance")) && !Player.Instance.HasBuff("WarwickE") && Config.Harass.Menu.GetCheckBoxValue("Config.AutoHarass.E.Status") && SpellManager.E.IsReady() && Player.Instance.ManaPercent >= Config.Harass.Menu.GetSliderValue("Config.Harass.E.Mana"))
            {
                SpellManager.E.Cast(target);
                if (Config.Harass.Menu.GetCheckBoxValue("Config.Harass.E.After"))
                    Core.DelayAction(() => SpellManager.E.CastE2(), 1000);
            }
            #endregion
        }
    }
}
