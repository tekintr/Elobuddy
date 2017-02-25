using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using pEzreal.Extensions;

namespace pEzreal.Modes
{
    internal class Killsteal
    {
        public static void Execute()
        {
            if (Config.KillstealQ)
            {
                var enemy =
                    EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(Spells.Q.Range) && !e.IsDead)
                        .FirstOrDefault(e => Config.MyHero.GetSpellDamage(e, SpellSlot.Q) > e.TotalShieldHealth());

                if (enemy == null || !Active.IsKillable(enemy) || !enemy.IsValidTarget(Spells.Q.Range)) return;
                Spells.CastQ(enemy);
            }

            if (Config.KillstealW)
            {
                var enemy =
                    EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(Spells.W.Range) && !e.IsDead)
                        .FirstOrDefault(e => Config.MyHero.GetSpellDamage(e, SpellSlot.W) > e.TotalShieldHealth());

                if (enemy == null || !Active.IsKillable(enemy) || !enemy.IsValidTarget(Spells.W.Range)) return;
                Spells.CastW(enemy);
            }

            if (Config.KillstealR)
            {
                var enemy =
                    EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(Spells.R.Range) && !e.IsDead)
                        .FirstOrDefault(e => Config.MyHero.GetSpellDamage(e, SpellSlot.R) > e.TotalShieldHealth());

                if (enemy == null || !Active.IsKillable(enemy) || !enemy.IsValidTarget(Spells.R.Range)) return;
                Spells.R.Cast(enemy);
            }
        }
    }
}