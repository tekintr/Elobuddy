using EloBuddy;
using EloBuddy.SDK;
using pEzreal.Extensions;

namespace pEzreal.Modes
{
    internal class Combo
    {
        public static void Execute()
        {
            if (Config.ComboE)
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range + Spells.E.Range, DamageType.Magical);
                if (target == null || target.IsInvulnerable || !target.IsValidTarget() || target.IsDead) return;

                switch (Config.ComboEMode)
                {
                    case 0:
                        if (Spells.E.IsReady() && Spells.Q.IsReady() || Spells.W.IsReady())
                            Spells.E.Cast(Game.CursorPos);
                        break;
                    case 1:
                        if (Spells.E.IsReady() && Spells.Q.IsReady() || Spells.W.IsReady())
                            Spells.E.Cast(target);
                        break;
                    case 2:
                        break;
                }
            }

            if (Config.ComboQ)
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);
                Spells.CastQ(target);
            }

            if (Config.ComboW)
            {
                var target = TargetSelector.GetTarget(Spells.W.Range, DamageType.Magical);
                Spells.CastW(target);
            }

            if (Config.ComboR)
            {
                Spells.R_CastIfWillHit(Config.ComboREnemies);
            }

            if (Config.ItemsBotrk && (Spells.Botrk.IsOwned() && Spells.Botrk.IsReady())
                || (Spells.Cutlass.IsOwned() && Spells.Cutlass.IsReady()))
            {
                var target = TargetSelector.GetTarget(Spells.Botrk.Range, DamageType.Physical);
                if (target == null || target.IsInvulnerable || !target.IsValidTarget() || target.IsDead) return;
                if (Config.ItemsBotrkHealth >= Config.MyHero.HealthPercent) return;

                if (Spells.Botrk.IsOwned() && Spells.Botrk.IsReady()) Spells.Botrk.Cast(target);
                if (Spells.Cutlass.IsOwned() && Spells.Cutlass.IsReady()) Spells.Cutlass.Cast(target);
            }

            if (Config.ItemsYoumuu && Spells.Youmuu.IsOwned() && Spells.Youmuu.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.Youmuu.Range, DamageType.Physical);
                if (target == null || target.IsInvulnerable || !target.IsValidTarget() || target.IsDead) return;

                Spells.Youmuu.Cast(target);
            }
        }
    }
}