using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;

namespace ReGaren.Utility
{
    public static class LastHit
    {
        public static void Execute()
        {
            if (ConfigList.Farm.FarmQLastHit && SpellManager.Q.IsReady())
            {
                var target = EntityManager.MinionsAndMonsters.EnemyMinions.Where(minion => minion.IsValidTarget(SpellManager.Q.Range * 2));
                if (target.Count() == 0)
                    target = EntityManager.MinionsAndMonsters.Monsters.Where(monster => monster.IsValidTarget(SpellManager.Q.Range * 2));

                if (target != null)
                {
                    foreach (var select in target)
                    {
                        if (select.IsValidTarget(SpellManager.Q.Range) && select.Health < Damage.GetQDamage(select))
                        {
                            SpellManager.Q.Cast();
                            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, select), ConfigList.Misc.GetSpellDelay);
                            return;
                        }
                    }
                }
            }
        }
    }
}
