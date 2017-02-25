using System;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System.Collections.Generic;
using ReWarwick.Utils;
using System.Linq;

namespace ReWarwick.Modes
{
    public static class Combo
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Mixed, Player.Instance.Position);
            if (target == null || target.IsInvulnerable)
                return;

            if (SpellManager.Q.IsReady() && Config.Combo.Menu.GetCheckBoxValue("Config.Combo.Q.Status"))
            {
                if (target.IsInRange(Player.Instance, SpellManager.Q.Range))
                {
                    SpellManager.Q.Cast(target);
                }
            }

            if (SpellManager.E.IsReady() && !Player.Instance.HasBuff("WarwickE") && Config.Combo.Menu.GetCheckBoxValue("Config.Combo.E.Status"))
            {
                if (target.IsInRange(Player.Instance, SpellManager.E.Range))
                {
                    SpellManager.E.Cast();
                    if (Config.Combo.Menu.GetCheckBoxValue("Config.Combo.E.After"))
                        Core.DelayAction(() => SpellManager.E.CastE2(), 1000);
                }
            }

            if (SpellManager.R.IsReady() && Config.Combo.Menu.GetCheckBoxValue("Config.Combo.R.Status") && target.IsInRange(Player.Instance, SpellManager.R.Range))
            {
                if (Config.Combo.Menu.GetCheckBoxValue($"Config.Combo.R.Use.{target.ChampionName}") && !target.HasSpellshield() && target.HealthPercent >= Config.Combo.Menu.GetSliderValue("Config.Combo.R.TargetHealth"))
                {
                    var prediction = SpellManager.R.GetPrediction(target);
                    if (prediction.CastPosition.IsUnderEnemyTurret() && !Config.Combo.Menu.GetCheckBoxValue("Config.Combo.R.Dive")) return;

                    if (!prediction.Collision && prediction.HitChancePercent >= Config.Combo.Menu.GetSliderValue("Config.Combo.R.HitChance"))
                        SpellManager.R.Cast(prediction.CastPosition);
                }
            }
        }
    }
    
}