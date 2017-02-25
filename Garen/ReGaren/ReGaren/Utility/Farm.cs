using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;

namespace ReGaren.Utility
{
    public static class Farm
    {
        public static void Execute()
        {
            if (ConfigList.Farm.FarmQ && SpellManager.Q.IsReady() && !Player.HasBuff("GarenE"))
            {
                var target = EntityManager.MinionsAndMonsters.EnemyMinions.Where(minion => minion.IsValidTarget(SpellManager.Q.Range * 2));
                if (target.Count() == 0)
                    target = EntityManager.MinionsAndMonsters.Monsters.Where(monster => monster.IsValidTarget(SpellManager.Q.Range * 2));

                if (target.Count() != 0)
                {
                    foreach (var select in target)
                    {
                        if (select.IsValidTarget(SpellManager.Q.Range) && select.Health < Damage.GetQDamage(select))
                        {
                            SpellManager.Q.Cast();
                            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, select), ConfigList.Misc.GetSpellDelay);
                            return;
                        }
                        else
                        {
                            if (select.Health - Damage.GetQDamage(select) > 300 && select.IsValidTarget(SpellManager.Q.Range))
                            {
                                SpellManager.Q.Cast();
                                Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, select), ConfigList.Misc.GetSpellDelay);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (Player.Instance.IsUnderEnemyturret())
                    {
                        SpellManager.Q.Cast();
                    }
                }
            }
            if (ConfigList.Farm.FarmE && SpellManager.E.IsReady())
            {
                int minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(minion => minion.IsValidTarget(SpellManager.E.Range)).Count();
                int monsters = EntityManager.MinionsAndMonsters.Monsters.Where(monster => monster.IsValidTarget(SpellManager.E.Range * 2)).Count();
                if ((minions + monsters) == 0)
                    return;

                if (minions >= ConfigList.Farm.FarmECount || (monsters > 0 && ConfigList.Farm.FarmEIgnore))
                    SpellManager.E.Cast();
            }
        }
    }
}
