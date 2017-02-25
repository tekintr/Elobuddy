using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace ZLP_Ryze
{
    public class Combos
    {
        public static void Combo()
        {
            if (Menus.Combo["Qc"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
                if (target == null || More.Unkillable(target)) return;
                var prediction = Spells.Q.GetPrediction(target);
                if (target.IsValidTarget(Spells.Q.Range) && prediction.HitChance >= More.Hit())
                    Spells.Q.Cast(prediction.CastPosition);
            }

            if (Menus.Combo["Wc"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() &&
                (!Menus.Combo["Qc"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady()) &&
                (!Menus.Combo["Ec"].Cast<CheckBox>().CurrentValue || !Spells.E.IsReady()))
            {
                var target = TargetSelector.GetTarget(Spells.W.Range, DamageType.Magical);
                if (target == null || More.Unkillable(target)) return;
                if (target.IsValidTarget(Spells.W.Range))
                    Spells.W.Cast(target);
            }

            if (Menus.Combo["Ec"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady() &&
                (!Menus.Combo["Qc"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady()))
            {
                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
                if (target == null || More.Unkillable(target)) return;
                if (target.IsValidTarget(Spells.E.Range))
                    Spells.E.Cast(target);
            }
        }

        public static void Harass()
        {
            if (Menus.Combo["Qh"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
                if (target == null || More.Unkillable(target)) return;
                var prediction = Spells.Q.GetPrediction(target);
                if (target.IsValidTarget(Spells.Q.Range) && prediction.HitChance >= More.Hit())
                    Spells.Q.Cast(prediction.CastPosition);
            }

            if (Menus.Combo["Eh"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady() &&
                (!Menus.Combo["Qh"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady()))
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
                if (target == null || More.Unkillable(target)) return;
                if (target.IsValidTarget(Spells.E.Range))
                    Spells.E.Cast(target);
            }
        }

        public static void Flee()
        {
            if (Spells.Q.IsReady() && (Player.Instance.HasBuff("RyzeQIconNoCharge") ||
                                       Player.Instance.HasBuff("RyzeQIconFullCharge")))
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
                if (target == null) return;
                var prediction = Spells.Q.GetPrediction(target);
                if (target.IsValidTarget(Spells.Q.Range) && prediction.HitChance >= More.Hit())
                    Spells.Q.Cast(prediction.CastPosition);
                if (More.CollisionT && Player.Instance.HasBuff("RyzeQIconFullCharge"))
                    Spells.Q.Cast(Player.Instance.Position.Extend(target, Spells.Q.Range).To3DWorld());
            }

            if (Spells.W.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.W.Range, DamageType.Magical);
                if (target == null) return;
                if (target.IsValidTarget(Spells.W.Range) && target.HasBuff("RyzeE"))
                    Spells.W.Cast(target);
            }

            if (Spells.E.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
                if (target == null) return;
                if (target.IsValidTarget(Spells.E.Range) && (!Spells.Q.IsReady() || More.CollisionT || target.HasBuff("RyzeE")))
                    Spells.E.Cast(target);
            }
        }

        public static void Collision()
        {
            if (Menus.Combo["Qc"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady() &&
                More.HitQ != null && More.CountE > 1)
            {
                var prediction = Spells.Q.GetPrediction(More.HitQ);
                if (prediction.HitChance >= More.Hit())
                    Spells.Q.Cast(prediction.CastPosition);
            }

            if (Menus.Combo["Ec"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady())
            {
                if (More.DieE != null)
                {
                    var targetE = TargetSelector.GetTarget(300, DamageType.Magical, More.DieE.Position, true);
                    if (targetE != null)
                        Spells.E.Cast(More.DieE);
                }

                if (More.HasE != null)
                {
                    var targetE = TargetSelector.GetTarget(300, DamageType.Magical, More.HasE.Position, true);
                    if (targetE != null)
                        Spells.E.Cast(More.HasE);
                }

                if (More.HitE != null && More.HasE == null && More.DieE == null)
                {
                    var targetE = TargetSelector.GetTarget(300, DamageType.Magical, More.HitE.Position, true);
                    if (targetE != null)
                        Spells.E.Cast(More.HitE);
                }
            }
        }

        public static bool Casted;

        public static void Escape()
        {
            if (Spells.R.IsReady() || Casted)
            {
                Casted = Spells.Zhonya.IsOwned() && Spells.Zhonya.IsReady();
                var turret = EntityManager.Turrets.Allies.Where(x => !x.IsDead)
                             .OrderBy(x => x.Distance(Player.Instance.Position)).FirstOrDefault();

                if (turret == null || !Casted) return;

                if (Spells.R.IsInRange(turret))
                {
                    Spells.R.Cast(turret.Position);

                    if (!Spells.R.IsReady() && More.CanCast())
                    {
                        Spells.Zhonya.Cast();
                        Casted = false;
                    }
                }

                else
                {
                    Spells.R.Cast(Player.Instance.Position.Extend(turret, Spells.R.Range).To3DWorld());

                    if (!Spells.R.IsReady() && More.CanCast())
                    {
                        Spells.Zhonya.Cast();
                        Casted = false;
                    }
                }
            }
        }
    }
}