using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;

namespace ReKatarina.Utility
{
    public static class LastHit
    {
        public static void Execute()
        {
            var creeps = EntityManager.MinionsAndMonsters.
                Get(EntityManager.MinionsAndMonsters.EntityType.Both, EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, SpellManager.Q.Range).
                OrderBy(h => h.Health);
            {
                if (ConfigList.Farm.LastHitQ && SpellManager.Q.IsReady())
                {
                    foreach(var creep in creeps)
                    {
                        if (creep.IsValidTarget(SpellManager.Q.Range) && (creep.TotalShieldHealth() + 5) <= Damage.GetQDamage(creep) && creep.Distance(Player.Instance.Position) >= (Player.Instance.AttackRange*2))
                            SpellManager.Q.Cast(creep);
                    }
                }
            }
        }

        public static void OnUnkillableMinion(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            if (ConfigList.Farm.LastHitQ && SpellManager.Q.IsReady() && !Damage.HasRBuff() && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear | Orbwalker.ActiveModes.LastHit))
                if (target.IsInRange(Player.Instance, SpellManager.Q.Range))
                    SpellManager.Q.Cast(target);
        }
    }
}
