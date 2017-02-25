using System.Linq;
using EloBuddy.SDK;
using pEzreal.Extensions;

namespace pEzreal.Modes
{
    internal class LaneClear
    {
        public static void Execute()
        {
            if (Config.LaneClearMana >= Config.MyHero.ManaPercent) return;

            if (Config.LaneClearQ)
            {
                var minion = EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Spells.Q.Range));

                if (minion == null || !minion.IsValidTarget(Spells.Q.Range)) return;

                Spells.Q.Cast(minion);
            }
        }
    }
}