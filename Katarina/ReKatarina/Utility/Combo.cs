using System;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace ReKatarina.Utility
{
    public static class Combo
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Mixed, Player.Instance.Position);
            if (target == null || target.IsInvulnerable)
                return;

            if (Damage.HasRBuff())
            {
                if (Player.Instance.CountEnemyChampionsInRange(SpellManager.R.Range - 50) < 1 || SpellManager.R.IsReady())
                {
                    Damage.UnfreezePlayer();
                    GetBestCombo(target);
                }

                if (target.Health <= Damage.GetQDamage(target) + Damage.GetEDamage(target))
                {
                    SpellManager.E.CastE(target.Position);
                    SpellManager.Q.Cast(target);
                }
                return;
            }

            switch (ConfigList.Combo.ComboStyle)
            {
                case 0:
                    GetBestCombo(target);
                    break;
                case 1:
                    Combo1(target);
                    break;
                case 2:
                    Combo2(target);
                    break;
                default:
                    Combo3(target);
                    break;
            }
        }

        private static void GetBestCombo(AIHeroClient t)
        {
            if (t.IsInRange(Player.Instance, SpellManager.Q.Range) && !t.IsInRange(Player.Instance, SpellManager.E.Range)) Combo3(t);
            else Combo2(t);
        }
        private static void Combo1(AIHeroClient t)
        {
            if (SpellManager.E.IsReady() && t.IsInRange(Player.Instance.Position, SpellManager.E.Range) && ConfigList.Combo.ComboE)
            {
                var d = Dagger.GetClosestDagger();
                if (t.Position.IsInRange(d, SpellManager.W.Range)) SpellManager.E.CastE(Damage.GetBestDaggerPoint(d, t));
                else
                    if (Player.Instance.Distance(t) >= SpellManager.W.Range)
                    SpellManager.E.CastE(t.Position);
            }

            if (SpellManager.W.IsLearned && !SpellManager.W.IsOnCooldown && ConfigList.Combo.ComboW)
            {
                if (t.IsInRange(Player.Instance.Position, SpellManager.W.Range))
                    SpellManager.W.Cast();
            }

            if (SpellManager.Q.CanCast(t) && ConfigList.Combo.ComboQ)
            {
                SpellManager.Q.Cast(t);
            }

            if (SpellManager.R.IsLearned && !SpellManager.R.IsOnCooldown && ConfigList.Combo.ComboR)
            {
                if (Player.Instance.CountEnemyChampionsInRange(ConfigList.Combo.MaxRCastRange) < ConfigList.Combo.MinToUseR) return;
                if (Damage.GetQDamage(t) + Damage.GetPDamage(t) + Damage.GetEDamage(t) + Player.Instance.GetAutoAttackDamage(t, true) >= t.TotalShieldHealth()) return;
                if (Orbwalker.IsAutoAttacking && !Orbwalker.DisableAttacking)
                {
                    Orbwalker.DisableAttacking = true;
                    Orbwalker.ResetAutoAttack();
                }

                Damage.FreezePlayer();
                Core.DelayAction(() => SpellManager.R.Cast(), 100);
            }
        } // EWQR
        private static void Combo2(AIHeroClient t)
        {
            if (SpellManager.E.IsReady() && t.IsInRange(Player.Instance.Position, SpellManager.E.Range) && ConfigList.Combo.ComboE)
            {
                var d = Dagger.GetClosestDagger();
                if (t.Position.IsInRange(d, SpellManager.W.Range)) SpellManager.E.CastE(Damage.GetBestDaggerPoint(d, t));
                else
                    if (Player.Instance.Distance(t) >= SpellManager.W.Range)
                    SpellManager.E.CastE(t.Position);
            }

            if (SpellManager.Q.CanCast(t) && ConfigList.Combo.ComboQ)
            {
                SpellManager.Q.Cast(t);
            }

            if (SpellManager.W.IsLearned && !SpellManager.W.IsOnCooldown && ConfigList.Combo.ComboW)
            {
                if (t.IsInRange(Player.Instance.Position, SpellManager.W.Range))
                    SpellManager.W.Cast();
            }

            if (SpellManager.R.IsLearned && !SpellManager.R.IsOnCooldown && ConfigList.Combo.ComboR)
            {
                if (Player.Instance.CountEnemyChampionsInRange(ConfigList.Combo.MaxRCastRange) < ConfigList.Combo.MinToUseR) return;
                if (Damage.GetQDamage(t) + Damage.GetPDamage(t) + Damage.GetEDamage(t) + Player.Instance.GetAutoAttackDamage(t, true) >= t.TotalShieldHealth()) return;
                if (Orbwalker.IsAutoAttacking && !Orbwalker.DisableAttacking)
                {
                    Orbwalker.DisableAttacking = true;
                    Orbwalker.ResetAutoAttack();
                }

                Damage.FreezePlayer();
                Core.DelayAction(() => SpellManager.R.Cast(), 100);
            }
        } // EQWR
        private static void Combo3(AIHeroClient t)
        {
            if (SpellManager.Q.CanCast(t) && ConfigList.Combo.ComboQ)
            {
                SpellManager.Q.Cast(t);
            }

            if (SpellManager.E.IsReady() && t.IsInRange(Player.Instance.Position, SpellManager.E.Range) && ConfigList.Combo.ComboE)
            {
                var d = Damage.GetBestDaggerPoint(Dagger.GetClosestDagger(), t);
                if (t.Position.IsInRange(d, SpellManager.W.Range)) SpellManager.E.CastE(d);
                else
                    if (Player.Instance.Distance(t) >= SpellManager.W.Range)
                        SpellManager.E.CastE(t.Position);
            }

            if (SpellManager.W.IsLearned && !SpellManager.W.IsOnCooldown && ConfigList.Combo.ComboW)
            {
                if (t.IsInRange(Player.Instance.Position, SpellManager.W.Range))
                    SpellManager.W.Cast();
            }

            if (SpellManager.R.IsLearned && !SpellManager.R.IsOnCooldown && ConfigList.Combo.ComboR)
            {
                if (Player.Instance.CountEnemyChampionsInRange(ConfigList.Combo.MaxRCastRange) < ConfigList.Combo.MinToUseR) return;
                if (Damage.GetQDamage(t) + Damage.GetPDamage(t) + Damage.GetEDamage(t) + Player.Instance.GetAutoAttackDamage(t, true) >= t.TotalShieldHealth()) return;
                if (Orbwalker.IsAutoAttacking && !Orbwalker.DisableAttacking)
                {
                    Orbwalker.DisableAttacking = true;
                    Orbwalker.ResetAutoAttack();
                }

                Damage.FreezePlayer();
                Core.DelayAction(() => SpellManager.R.Cast(), 100);
            }
        } // QEWR        
    }
}