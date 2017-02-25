using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace ZLP_Ryze
{
    public class Modes
    {
        public static void Combo()
        {
            if (!More.CanCast()) return;
            More.Combo();

            if (Player.Instance.HealthPercent <= Menus.ComboHealth.CurrentValue && Menus.ComboHealth.CurrentValue != 0)
                Combos.Flee();

            else
            {
                if (!More.CollisionT)
                    Combos.Combo();
                else
                    Combos.Collision();
            }
        }

        public static void Harass()
        {
            if (Player.Instance.ManaPercent <= Menus.HarassMana.CurrentValue || !More.CanCast()) return;
            More.Combo();

            if (!More.CollisionT)
                Combos.Harass();
            else
                Combos.Collision();
        }

        public static void LaneClear()
        {
            if (Player.Instance.ManaPercent <= Menus.LaneMana.CurrentValue || !More.CanCast()) return;
            More.Lane();

            if (Menus.Clear["Qlc"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady())
            {
                if (More.Minion != null && !More.CollisionM)
                    Spells.Q.Cast(More.Minion);

                else
                {
                    if (More.CountE == 0 && (!Menus.Clear["Elc"].Cast<CheckBox>().CurrentValue || Spells.E.IsReady()))
                    {
                        var minion = EntityManager.MinionsAndMonsters.EnemyMinions
                                     .Where(m => m.IsValidTarget(Spells.Q.Range))
                                     .OrderBy(m => m.Distance(Player.Instance.Position)).FirstOrDefault();
                        if (minion == null) return;
                        Spells.Q.Cast(minion);
                    }

                    if (More.CountE > 1)
                    {
                        var minion = EntityManager.MinionsAndMonsters.EnemyMinions
                                     .Where(m => m.IsValidTarget(Spells.Q.Range) && m.HasBuff("RyzeE"))
                                     .OrderBy(m => m.Distance(Player.Instance.Position)).FirstOrDefault();
                        if (minion == null) return;
                        Spells.Q.Cast(minion);
                    }
                }
            }

            if (Menus.Clear["Elc"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady())
            {
                if (More.DieE != null)
                    Spells.E.Cast(More.DieE);

                else
                {
                    if (!Spells.Q.IsLearned) return;

                    if (More.HitE != null && More.CountE == 0 &&
                        (!Menus.Clear["Qlc"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady()))
                        Spells.E.Cast(More.HitE);

                    if (More.HasE != null && More.CountE == 1 || 
                        (More.CountE > 1 && (!Menus.Clear["Qlc"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady())))
                        Spells.E.Cast(More.HasE);
                }
            }
        }

        public static void JungleClear()
        {
            if (Player.Instance.ManaPercent <= Menus.JungleMana.CurrentValue || !More.CanCast()) return;
            More.Jungle();

            if (Menus.Clear["Qjc"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady())
            {
                if (More.Monster != null && !More.CollisionJ)
                    Spells.Q.Cast(More.Monster);

                else
                {
                    if ((More.CountE == 0 && (!Menus.Clear["Ejc"].Cast<CheckBox>().CurrentValue || Spells.E.IsReady())) ||
                        More.CountM == 1)
                    {
                        var monster = EntityManager.MinionsAndMonsters.Monsters
                                     .Where(m => m.IsValidTarget(Spells.Q.Range))
                                     .OrderBy(m => m.Distance(Player.Instance.Position)).FirstOrDefault();
                        if (monster == null) return;
                        Spells.Q.Cast(monster);
                    }

                    if (More.CountE > 1)
                    {
                        var monster = EntityManager.MinionsAndMonsters.Monsters
                                     .Where(m => m.IsValidTarget(Spells.Q.Range) && m.HasBuff("RyzeE"))
                                     .OrderBy(m => m.Distance(Player.Instance.Position)).FirstOrDefault();
                        if (monster == null) return;
                        Spells.Q.Cast(monster);
                    }
                }
            }

            if (Menus.Clear["Wjc"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() && More.CountM == 1 &&
                (!Menus.Clear["Qjc"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady()) &&
                (!Menus.Clear["Ejc"].Cast<CheckBox>().CurrentValue || !Spells.E.IsReady()))
            {
                var monster = EntityManager.MinionsAndMonsters.Monsters
                              .Where(m => m.IsValidTarget(Spells.W.Range))
                              .OrderBy(m => m.MaxHealth).LastOrDefault();
                if (monster == null) return;
                Spells.W.Cast(monster);
            }

            if (Menus.Clear["Ejc"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady())
            {
                if (More.DieE != null)
                    Spells.E.Cast(More.DieE);

                else
                {
                    if (More.HitE != null && More.CountE == 0 &&
                        (!Menus.Clear["Qlc"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady()))
                        Spells.E.Cast(More.HitE);

                    if (More.HasE != null && More.CountE == 1 ||
                        (More.CountE > 1 && (!Menus.Clear["Qlc"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady())))
                        Spells.E.Cast(More.HasE);
                }
            }
        }

        public static void LastHit(Obj_AI_Base unit, Orbwalker.UnkillableMinionArgs args)
        {
            if (Player.Instance.ManaPercent <= Menus.LastMana.CurrentValue || unit == null) return;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                if (Menus.Clear["Qlh"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady() &&
                    unit.Health <= Spells.Q.GetSpellDamage(unit))
                    Spells.Q.Cast(unit);

                if (Menus.Clear["Wlh"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() &&
                    (!Menus.Clear["Qlh"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady() ||
                     More.Minion == null || More.CollisionM) &&
                    (!Menus.Clear["Elh"].Cast<CheckBox>().CurrentValue || !Spells.E.IsReady()) &&
                    unit.Health <= Spells.W.GetSpellDamage(unit))
                    Spells.W.Cast(unit);

                if (Menus.Clear["Elh"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady() &&
                    unit.Health <= Spells.E.GetSpellDamage(unit))
                    Spells.E.Cast(unit);
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (Menus.Clear["Qlc"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady() &&
                    unit.Health <= Spells.Q.GetSpellDamage(unit))
                    Spells.Q.Cast(unit);

                if (Menus.Clear["Wlc"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() &&
                    (!Menus.Clear["Qlc"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady() ||
                     More.Minion == null || More.CollisionM) &&
                    (!Menus.Clear["Elc"].Cast<CheckBox>().CurrentValue || !Spells.E.IsReady()) &&
                    unit.Health <= Spells.W.GetSpellDamage(unit))
                    Spells.W.Cast(unit);

                if (Menus.Clear["Elc"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady() &&
                    unit.Health <= Spells.E.GetSpellDamage(unit))
                    Spells.E.Cast(unit);
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                if (Menus.Clear["Qjc"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady() &&
                    unit.Health <= Spells.Q.GetSpellDamage(unit))
                    Spells.Q.Cast(unit);

                if (Menus.Clear["Wjc"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() &&
                    (!Menus.Clear["Qjc"].Cast<CheckBox>().CurrentValue || !Spells.Q.IsReady() ||
                     More.Minion == null || More.CollisionM) &&
                    (!Menus.Clear["Ejc"].Cast<CheckBox>().CurrentValue || !Spells.E.IsReady()) &&
                    unit.Health <= Spells.W.GetSpellDamage(unit))
                    Spells.W.Cast(unit);

                if (Menus.Clear["Ejc"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady() &&
                    unit.Health <= Spells.E.GetSpellDamage(unit))
                    Spells.E.Cast(unit);
            }
        }

        public static void Flee()
        {
            if (!More.CanCast()) return;
            More.Combo();

            Combos.Flee();
        }

        public static void Escape(EventArgs args)
        {
            var count = Player.Instance.CountEnemiesInRange(800);

            if (Menus.Misc["esc"].Cast<KeyBind>().CurrentValue ||
                (Menus.Auto.CurrentValue != 0 && Player.Instance.HealthPercent <= Menus.Auto.CurrentValue && count >= 1))
                Combos.Escape();
        }

        public static void AutoHarass(EventArgs args)
        {
            if (Menus.Combo["auto"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady() &&
                Player.Instance.ManaPercent > Menus.HarassMana.CurrentValue && More.CanCast())
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
                if (target == null || More.Unkillable(target)) return;
                var prediction = Spells.Q.GetPrediction(target);
                if (target.IsValidTarget(Spells.Q.Range) && prediction.HitChance >= More.Hit())
                    Spells.Q.Cast(target);
            }
        }

        public static void KillSteal(EventArgs args)
        {
            if (Menus.Misc["Q"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
                if (target == null || More.Unkillable(target)) return;
                var prediction = Spells.Q.GetPrediction(target);
                if (target.IsValidTarget(Spells.Q.Range) && prediction.HitChance >= More.Hit() &&
                    target.TotalShieldHealth() <= Spells.Q.GetSpellDamage(target))
                    Spells.Q.Cast(target);
            }

            if (Menus.Misc["W"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.W.Range, DamageType.Magical);
                if (target == null || More.Unkillable(target)) return;
                if (target.IsValidTarget(Spells.W.Range) &&
                    target.TotalShieldHealth() <= Spells.W.GetSpellDamage(target))
                    Spells.W.Cast(target);
            }

            if (Menus.Misc["E"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
                if (target == null || More.Unkillable(target)) return;
                if (target.IsValidTarget(Spells.E.Range) &&
                    target.TotalShieldHealth() <= Spells.E.GetSpellDamage(target))
                    Spells.E.Cast(target);
            }

            if (Menus.Misc["ignite"].Cast<CheckBox>().CurrentValue && 
                Spells.Ignite != null && Spells.Ignite.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.Ignite.Range, DamageType.True);
                if (target == null || More.Unkillable(target)) return;
                if (target.IsValidTarget(Spells.Ignite.Range) && target.TotalShieldHealth() <=
                    Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite))
                    Spells.Ignite.Cast(target);
            }
        }

        public static void Stack(EventArgs args)
        {
            if (!Menus.Misc["stack"].Cast<CheckBox>().CurrentValue || Spells.Seraph.IsOwned() ||
                Player.Instance.IsRecalling() || !More.CanCast()) return;

            var count = EntityManager.MinionsAndMonsters.Monsters.Count(m => m.IsInRange(Player.Instance, 1000));
            if ((Spells.Tear.IsOwned() || Spells.Archangel.IsOwned()) &&
                ((Player.Instance.Position.CountEnemiesInRange(2500) < 1 &&
                 Player.Instance.Position.CountEnemyMinionsInRange(2500) < 1 && count < 1) ||
                 Player.Instance.IsInShopRange()))
                Spells.Q.Cast(Game.CursorPos);
        }

        public static void OnGap(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Menus.Misc["gap"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() && More.CanCast() &&
                sender != null && sender.IsEnemy && e.End.Distance(Player.Instance) <= Spells.W.Range)
                Spells.W.Cast(sender);
        }
    }
}