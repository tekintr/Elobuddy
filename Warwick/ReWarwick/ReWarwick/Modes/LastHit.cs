using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;
using ReWarwick.Utils;

namespace ReWarwick.Modes
{
    public static class LastHit
    {
        public static void Execute()
        {
            if (Config.Farm.Menu.GetCheckBoxValue("Config.Farm.Q.LastHit") && Player.Instance.ManaPercent >= Config.Farm.Menu.GetSliderValue("Config.Farm.Q.Mana"))
            {
                foreach (var e in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy).Where(m => m.IsValid && m.IsInRange(Player.Instance, SpellManager.Q.Range)).OrderByDescending(m => m.Health))
                {
                    if (e.Health <= Damage.GetQDamage(e))
                        SpellManager.Q.Cast(e);
                }
            }
        }

        public static void OnUnkillableMinion(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            if (!Config.Farm.Menu.GetCheckBoxValue("Config.Farm.Q.Unkillable") || Player.Instance.ManaPercent < Config.Farm.Menu.GetSliderValue("Config.Farm.Q.Mana")) return;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (SpellManager.Q.IsReady() && target.CountEnemyChampionsInRange(550) <= 1 && Player.Instance.HealthPercent >= 30)
                {
                    if (target.IsInRange(Player.Instance, SpellManager.Q.Range))
                        SpellManager.Q.Cast(target);
                }
            }
        }
    }
}
