using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;

namespace ReKatarina.Utility
{
    public static class LaneClear
    {
        public static void Execute()
        {
            var minions = EntityManager.MinionsAndMonsters.
                GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, SpellManager.Q.Range).
                OrderByDescending(h => h.Health);
            {
                if (minions == null || !minions.Any()) return;
                if (ConfigList.Farm.FarmQ && SpellManager.Q.IsReady())
                {
                    if (minions.Count() < ConfigList.Farm.FarmQCount) return;
                    SpellManager.Q.Cast(minions.Last());
                }

                if (ConfigList.Farm.FarmW && SpellManager.W.IsReady())
                {
                    if (Player.Instance.CountEnemyMinionsInRange(SpellManager.W.Range) >= 5)
                        SpellManager.W.Cast();
                }

                if (ConfigList.Farm.FarmE && SpellManager.E.IsReady())
                {
                    var d = Dagger.GetClosestDagger();
                    if (d.IsInRange(Player.Instance.Position, SpellManager.E.Range))
                    {
                        if (!d.IsUnderEnemyTurret() && d.CountEnemyChampionsInRange(SpellManager.E.Range) <= 1 && Player.Instance.HealthPercent >= 50)
                        {
                            var best_pos = Damage.GetBestDaggerPoint(d, minions.FirstOrDefault());
                            if (best_pos.CountEnemyMinionsInRange(SpellManager.W.Range) <= 0) return;
                            SpellManager.E.Cast(best_pos);
                        }
                    }
                }
            }
        }
    }
}
