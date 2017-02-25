using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;
using ReWarwick.Utils;

namespace ReWarwick.Modes
{
    public static class Harass
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(EntityManager.Heroes.Enemies, DamageType.Magical);
            if (target == null) return;

            if (Config.Harass.Menu.GetCheckBoxValue("Config.Harass.Q.Status") && SpellManager.Q.IsReady() && Player.Instance.ManaPercent >= Config.Harass.Menu.GetSliderValue("Config.Harass.Q.Mana"))
            {
                if (Player.Instance.IsInRange(target, SpellManager.Q.Range))
                {
                    SpellManager.Q.Cast(target);
                }
            }

            if (Config.Harass.Menu.GetCheckBoxValue("Config.Harass.E.Status") && SpellManager.E.IsReady() && !Player.Instance.HasBuff("WarwickE") && Player.Instance.ManaPercent >= Config.Harass.Menu.GetSliderValue("Config.Harass.E.Mana"))
            {
                if (Player.Instance.IsInRange(target, SpellManager.E.Range))
                {
                    SpellManager.E.Cast();
                    if (Config.Harass.Menu.GetCheckBoxValue("Config.Harass.E.After"))
                        Core.DelayAction(() => SpellManager.E.CastE2(), 1000);
                }
            }
        }
    }
}
