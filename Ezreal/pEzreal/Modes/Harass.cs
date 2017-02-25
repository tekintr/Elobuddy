using EloBuddy;
using EloBuddy.SDK;
using pEzreal.Extensions;

namespace pEzreal.Modes
{
    internal class Harass
    {
        public static void Execute()
        {
            if (Config.HarassMana >= Config.MyHero.ManaPercent) return;

            if (Config.HarassQ)
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);
                Spells.CastQ(target);
            }

            if (Config.HarassW)
            {
                var target = TargetSelector.GetTarget(Spells.W.Range, DamageType.Magical);
                Spells.CastW(target);
            }
        }
    }
}