using System.Linq;
using EloBuddy.SDK;
using pEzreal.Extensions;

namespace pEzreal.Modes
{
    internal class JungleClear
    {
        public static void Execute()
        {
            if (Config.JungleClearMana >= Config.MyHero.ManaPercent) return;

            if (Config.JungleClearQ)
            {
                var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Spells.Q.Range));

                if (monster == null || !monster.IsValidTarget(Spells.Q.Range)) return;

                Spells.Q.Cast(monster);
            }
        }
    }
}