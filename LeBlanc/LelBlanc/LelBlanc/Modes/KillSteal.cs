﻿using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace LelBlanc.Modes
{
    internal class KillSteal
    {
        #region Properties

        public static bool UseQ => Config.KillStealMenu["useQ"].Cast<CheckBox>().CurrentValue;

        public static bool UseW => Config.KillStealMenu["useW"].Cast<CheckBox>().CurrentValue;

        public static bool UseReturn => Config.KillStealMenu["useReturn"].Cast<CheckBox>().CurrentValue;

        public static bool UseE => Config.KillStealMenu["useE"].Cast<CheckBox>().CurrentValue;

        public static bool UseQr => Config.KillStealMenu["useQR"].Cast<CheckBox>().CurrentValue;

        public static bool UseWr => Config.KillStealMenu["useWR"].Cast<CheckBox>().CurrentValue;

        public static bool UseReturn2 => Config.KillStealMenu["useReturn2"].Cast<CheckBox>().CurrentValue;

        public static bool UseEr => Config.KillStealMenu["useER"].Cast<CheckBox>().CurrentValue;

        public static bool UseIgnite => Config.KillStealMenu["useIgnite"].Cast<CheckBox>().CurrentValue;

        public static bool UsePrediction => Config.KillStealMenu["usePrediction"].Cast<CheckBox>().CurrentValue;

        public static bool ResetW { get; set; }

        #endregion

        #region Methods

        public static void Execute()
        {
            #region Ignite

            if (Program.Ignite != null && Program.Ignite.IsReady())
            {
                var ignitableEnemies =
                    EntityManager.Heroes.Enemies.Where(
                        t =>
                            t.IsValidTarget(Program.Ignite.Range) && !t.HasUndyingBuff() &&
                            Extension.DamageLibrary.CalculateDamage(t, false, false, false, false, true) >=
                            (UsePrediction ? Prediction.Health.GetPrediction(t, 5000) : t.Health));
                var igniteEnemy = TargetSelector.GetTarget(ignitableEnemies, DamageType.True);

                if (igniteEnemy != null)
                {
                    if (Program.Ignite != null && UseIgnite)
                    {
                        if (Program.Ignite.IsInRange(igniteEnemy))
                        {
                            Program.Ignite.Cast(igniteEnemy);
                        }
                    }
                }
            }

            #endregion

            if (Player.Instance.IsUnderTurret() || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                return;

            var killableEnemies =
                EntityManager.Heroes.Enemies.Where(
                    t =>
                        t.IsValidTarget() && !t.HasUndyingBuff() &&
                        Extension.DamageLibrary.CalculateDamage(t, UseQ, UseW, UseE, false, false) +
                        Extension.DamageLibrary.CalculateDamageUltimate(t, true, false, false) >= t.Health);
            var target = TargetSelector.GetTarget(killableEnemies, DamageType.Magical);

            if (target == null) return;

            if (UseQ &&
                target.Health <= Extension.DamageLibrary.CalculateDamage(target, true, false, false, false, false))
            {
                CastQ(target);
            }

            else if (UseW &&
                     target.Health <=
                     Extension.DamageLibrary.CalculateDamage(target, false, true, false, false, false))
            {
                CastW(target, true);
            }

            else if (UseE &&
                     target.Health <=
                     Extension.DamageLibrary.CalculateDamage(target, false, false, true, false, false))
            {
                CastE(target);
            }

            else if (target.Health <= Extension.DamageLibrary.CalculateDamageUltimate(target, true, false, false))
            {
                CastR(target, true, SpellSlot.Q);
            }

            else if (target.Health <= Extension.DamageLibrary.CalculateDamageUltimate(target, false, true, false))
            {
                CastR(target, true, SpellSlot.W);
            }

            else if (target.Health <= Extension.DamageLibrary.CalculateDamageUltimate(target, false, false, true))
            {
                CastR(target, true, SpellSlot.E);
            }

            else if (UseQ && UseW &&
                     target.Health <=
                     Extension.DamageLibrary.CalculateDamage(target, true, true, false, false, false))
            {
                if (!Program.Q.IsReady() || !Program.W.IsReady()) return;

                CastQ(target);
                Core.DelayAction(() =>
                {
                    if (!target.IsDead &&
                        Extension.DamageLibrary.CalculateDamage(target, false, true, false, false, false) >=
                        target.Health)
                    {
                        CastW(target, true);
                        Core.DelayAction(() =>
                        {
                            if (target.IsDead &&
                                Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() == "leblancwreturn")
                            {
                                ResetW = true;
                            }
                        }, Program.W.CastDelay);
                    }
                }, Program.Q.CastDelay);
            }

            else if (UseQ && (UseQr || UseWr || UseEr) &&
                     target.Health <=
                     Extension.DamageLibrary.CalculateDamage(target, true, false, false, false, false) +
                     Extension.DamageLibrary.CalculateDamageUltimate(target, true, false, false))
            {
                if (!Program.Q.IsReady() || !Program.RReturn.IsReady()) return;

                CastQ(target);
                Core.DelayAction(() =>
                {
                    if (!target.IsDead)
                    {
                        CastR(target, UseReturn2, SpellSlot.Q);
                    }
                }, Program.Q.CastDelay);
            }

            else if (UseQ && UseW && UseE &&
                     target.Health <=
                     Extension.DamageLibrary.CalculateDamage(target, true, true, true, false, false))
            {
                if (!Program.Q.IsReady() || !Program.W.IsReady() || !Program.E.IsReady())
                {
                    return;
                }

                CastQ(target);
                Core.DelayAction(() =>
                {
                    if (!target.IsDead)
                    {
                        CastW(target, false);
                        Core.DelayAction(() =>
                        {
                            if (!target.IsDead)
                            {
                                CastE(target);
                                ResetW = UseReturn;
                            }
                            else if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() ==
                                     "leblancwreturn")
                            {
                                ResetW = UseReturn;
                            }
                        }, Program.W.CastDelay);
                    }
                }, Program.Q.CastDelay);
            }

            else
            {
                if (!Program.Q.IsReady() || !Program.W.IsReady() || !Program.E.IsReady() ||
                    !Program.QUltimate.IsReady()) return;

                CastQ(target);
                Core.DelayAction(() =>
                {
                    if (!target.IsDead)
                    {
                        CastR(target, false, SpellSlot.Q);
                        Core.DelayAction(() =>
                        {
                            if (!target.IsDead &&
                                Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() ==
                                "leblancw")
                            {
                                CastW(target, false);
                                Core.DelayAction(() =>
                                {
                                    if (!target.IsDead)
                                    {
                                        CastE(target);
                                        ResetW = UseReturn;
                                    }
                                    else if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() ==
                                             "leblancwreturn")
                                    {
                                        ResetW = UseReturn;
                                    }
                                }, Program.W.CastDelay);
                            }
                        }, Program.QUltimate.CastDelay);
                    }
                }, Program.Q.CastDelay);
            }
        }

        private static void CastQ(AIHeroClient target)
        {
            if (!Program.Q.IsReady()) return;

            if (Program.Q.IsInRange(target))
            {
                Program.Q.Cast(target);
            }
        }

        private static void CastW(AIHeroClient target, bool useWReturn)
        {
            if (!Program.W.IsReady()) return;

            if (Program.W.IsInRange(target) &&
                Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() == "leblancw")
            {
                Program.W.Cast(target);
                if (UseReturn)
                {
                    ResetW = useWReturn;
                }
            }
        }

        private static void CastE(AIHeroClient target)
        {
            if (!Program.E.IsReady()) return;

            if (Program.E.IsInRange(target))
            {
                Program.E.Cast(target);
            }
        }

        private static void CastR(AIHeroClient target, bool useWReturn, SpellSlot spell)
        {
            if (!Program.RActive.IsReady())
            {
                return;
            }

            Program.RActive.Cast();

            Core.DelayAction(() =>
            {
                switch (spell)
                {
                    case SpellSlot.Q:
                        if (UseQr && Program.QUltimate.IsInRange(target))
                        {
                            Program.QUltimate.Cast(target);
                        }
                        break;
                    case SpellSlot.W:
                        if (UseWr && Program.WUltimate.IsInRange(target))
                        {
                            Program.WUltimate.Cast(target);
                            if (UseReturn2)
                            {
                                ResetW = useWReturn;
                            }
                        }
                        break;
                    case SpellSlot.E:
                        if (UseEr && Program.EUltimate.IsInRange(target))
                        {
                            Program.EUltimate.Cast(target);
                        }
                        break;
                }
            }, Program.RActive.CastDelay);
        }

        #endregion
    }
}