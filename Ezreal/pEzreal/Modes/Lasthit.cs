using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using pEzreal.Extensions;

namespace pEzreal.Modes
{
    internal class Lasthit
    {
        public static void Execute()
        {
            if (Config.LasthitMana >= Config.MyHero.ManaPercent) return;

            if (Config.LasthitQ)
            {
                var minion = EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(
                        m =>
                            m.IsValidTarget(Spells.Q.Range) &&
                            m.Health <= Config.MyHero.GetSpellDamage(m, SpellSlot.Q));

                if (minion == null) return;

                Spells.Q.Cast(minion);
            }
        }
    }
}