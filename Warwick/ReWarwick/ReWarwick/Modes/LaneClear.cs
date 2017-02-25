using EloBuddy;
using EloBuddy.SDK;
using ReWarwick.Utils;
using System.Linq;

namespace ReWarwick.Modes
{
    public static class LaneClear
    {
        public static void Execute()
        {
            if (SpellManager.Q.IsReady() && Config.Farm.Menu.GetCheckBoxValue("Config.Farm.Q.Status") && Player.Instance.ManaPercent >= Config.Farm.Menu.GetSliderValue("Config.Farm.Q.Mana"))
            {
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, SpellManager.Q.Range);
                if (minions.Any(h => h.Health <= Damage.GetQDamage(h)))
                    SpellManager.Q.Cast(minions.FirstOrDefault(h => h.Health <= Damage.GetQDamage(h)));
                else
                    if (minions.OrderByDescending(h => h.Health).FirstOrDefault() != null)
                        SpellManager.Q.Cast(minions.OrderByDescending(h => h.Health).FirstOrDefault());
            }

            if (SpellManager.E.IsReady() && Player.Instance.HasBuff("WarwickE") && Config.Farm.Menu.GetCheckBoxValue("Config.Farm.E.Status") && Player.Instance.ManaPercent >= Config.Farm.Menu.GetSliderValue("Config.Farm.E.Mana"))
            {
                if (Player.Instance.CountEnemyMinionsInRangeWithPrediction((int)SpellManager.E.Range, 1000) >= Config.Farm.Menu.GetSliderValue("Config.Farm.E.Near"))
                {
                    SpellManager.E.Cast();
                    Core.DelayAction(() => SpellManager.E.Cast(), 1000);
                }
            }
        }
    }
}
