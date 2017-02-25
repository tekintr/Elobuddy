using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace LelBlanc
{
    internal class Extension
    {
        /// <summary>
        /// Leblanc Tether Range
        /// </summary>
        public const int TetherRange = 1000;

        /// <summary>
        /// Checks if Player should use W to return.
        /// </summary>
        public static bool LogicReturn(bool w2 = false)
        {
            var enemiesBeingE =
                EntityManager.Heroes.Enemies.Where(t => t.IsValidTarget(TetherRange) && IsBeingE(t))
                    .ToArray();

            if (enemiesBeingE.Any())
            {
                return false;
            }

            if (!enemiesBeingE.Any() && Program.E.IsReady() && Player.Instance.CountEnemiesInRange(Program.E.Range) > 0)
            {
                return false;
            }

            var enemiesNearLastPosition = Program.LastWPosition.CountEnemiesInRange(Player.Instance.AttackRange);
            var enemiesNearCurrentPosition = Player.Instance.CountEnemiesInRange(Player.Instance.AttackRange);
            var alliesNearLastPosition = Program.LastWPosition.CountAlliesInRange(Player.Instance.AttackRange);
            var alliesNearCurrentPosition = Player.Instance.CountAlliesInRange(Player.Instance.AttackRange);

            if (enemiesNearCurrentPosition < enemiesNearLastPosition ||
                alliesNearCurrentPosition > alliesNearLastPosition ||
                !Player.Instance.IsUnderTurret() && Program.LastWPosition.IsUnderTurret())
            {
                return false;
            }

            if (w2)
            {
                if (Program.RReturn.IsReady() &&
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).Name.ToLower() != "leblancrwreturn")
                {
                    Program.RReturn.Cast();
                    return true;
                }
                return false;
            }

            if (Program.WReturn.IsReady() &&
                Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() == "leblancwreturn")
            {
                Program.WReturn.Cast();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the Player has the spell.
        /// </summary>
        /// <param name="s">The Spell Name</param>
        /// <returns></returns>
        public static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.ToLower() == s) != null;
        }

        /// <summary>
        /// Contains the list of currently marked targets.
        /// </summary>
        //public static List<Tuple<bool, Obj_AI_Base>> MarkedTargets = new List<Tuple<bool, Obj_AI_Base>>();

        /// <summary>
        /// Checks to see if the target is being silenced
        /// </summary>
        /// <param name="target">The Target</param>
        /// <returns></returns>
        public static bool IsMarked(Obj_AI_Base target)
        {
            return target.HasBuff("LeblancPMark") && (Environment.TickCount - target.GetBuff("LeblancPMark").StartTime > 1500f);
        }

        /// <summary>
        /// Returns if player is using ultimate
        /// </summary>
        public static bool IsUsingUlt => Player.Instance.HasBuff("LeblancR");

        /// <summary>
        /// Checks to see if the target is being E'ed
        /// </summary>
        /// <param name="target">The Target</param>
        /// <returns></returns>
        public static bool IsBeingE(Obj_AI_Base target)
        {
            return target.HasBuff("LeblancEBeam") || target.HasBuff("LeblancREBeam");
        }

        /// <summary>
        /// Checks to see if target has been successfully rooted
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsRootE(Obj_AI_Base target)
        {
            return target.HasBuff("LeblancERoot") || target.HasBuff("LeblancRERoot");
        }

        internal class DamageLibrary
        {
            /// <summary>
            /// Calculates Damage for LeBlanc's Ultimates
            /// </summary>
            /// <param name="target"></param>
            /// <param name="qr"></param>
            /// <param name="wr"></param>
            /// <param name="er"></param>
            /// <returns></returns>
            public static float CalculateDamageUltimate(Obj_AI_Base target, bool qr, bool wr, bool er)
            {
                var totaldamage = 0f;

                if (target == null || !IsUsingUlt) return totaldamage;
                    
                if (qr)
                {
                    totaldamage += QUltimateDamage(target);
                }

                if (wr)
                {
                    totaldamage += WUltimateDamage(target);
                }

                if (er)
                {
                    totaldamage += EUltimateDamage(target);
                }

                if (IsMarked(target))
                {
                    totaldamage += MarkDamage(target);
                }

                return totaldamage;
            }

            /// <summary>
            /// Calculates Damage for LeBlanc
            /// </summary>
            /// <param name="target">The Target</param>
            /// <param name="q">The Q</param>
            /// <param name="w">The W</param>
            /// <param name="e">The E</param>
            /// <param name="r">The R</param>
            /// <param name="ignite"></param>
            /// <returns></returns>
            public static float CalculateDamage(Obj_AI_Base target, bool q, bool w, bool e, bool r, bool ignite)
            {
                var totaldamage = 0f;

                if (target == null) return totaldamage;

                if (!IsUsingUlt)
                {
                    if (q && Program.Q.IsReady())
                    {
                        totaldamage += QDamage(target);
                    }

                    if (w && Program.W.IsReady() &&
                        Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() == "leblancw")
                    {
                        totaldamage = WDamage(target);
                    }

                    if (e && Program.E.IsReady())
                    {
                        totaldamage += EDamage(target);
                    }

                    if (r && Program.RActive.IsReady())
                    {
                        totaldamage += (QUltimateDamage(target) + WUltimateDamage(target) + EUltimateDamage(target)) / 3;
                    }

                    if ((Program.Q.IsReady() || Program.W.IsReady() || Program.E.IsReady() || Program.RActive.IsReady()) && IsMarked(target))
                    {
                        totaldamage += MarkDamage(target);
                    }
                }

                if (r && IsUsingUlt)
                {
                    totaldamage += (QUltimateDamage(target) + WUltimateDamage(target) + EUltimateDamage(target))/3;

                    if (IsMarked(target))
                    {
                        totaldamage += MarkDamage(target);
                    }
                }

                if (ignite && Program.Ignite != null && Program.Ignite.IsReady() && Program.Ignite.IsInRange(target))
                {
                    totaldamage += Player.Instance.GetSummonerSpellDamage(target,
                        EloBuddy.SDK.DamageLibrary.SummonerSpells.Ignite);
                }

                return totaldamage;
            }

            /// <summary>
            /// Calculates the Damage done with the Mark
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            private static float MarkDamage(Obj_AI_Base target)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, 25 + 15*Player.Instance.Level + Player.Instance.TotalMagicalDamage*0.8f);
            }

            /// <summary>
            /// Calculates the Damage done with Q
            /// </summary>
            /// <param name="target">The Target</param>
            /// <returns>Returns the Damage done with Q</returns>
            private static float QDamage(Obj_AI_Base target)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                    new[] {0, 55, 80, 105, 130, 155}[Program.Q.Level] + (Player.Instance.TotalMagicalDamage*0.5f));
            }

            /// <summary>
            /// Calculates the Damage done with W
            /// </summary>
            /// <param name="target">The Target</param>
            /// <returns>Returns the Damage done with W</returns>
            private static float WDamage(Obj_AI_Base target)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                    new[] {0, 85, 125, 165, 205, 245}[Program.W.Level] + (Player.Instance.TotalMagicalDamage*0.6f));
            }

            /// <summary>
            /// Calculates the Damage done with E
            /// </summary>
            /// <param name="target">The Target</param>
            /// <returns>Returns the Damage done with E</returns>
            private static float EDamage(Obj_AI_Base target)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                    new[] {0, 40, 65, 90, 115, 140}[Program.E.Level] + (Player.Instance.TotalMagicalDamage*0.5f));
            }

            /// <summary>
            /// Returns the Damage of Q Ultimate Form
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            private static float QUltimateDamage(Obj_AI_Base target)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                    new[] {0, 150, 275, 400}[Program.RActive.Level] +
                    (Player.Instance.TotalMagicalDamage*0.6f));
            }

            /// <summary>
            /// Returns the Damage of W Ultimate Form
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            private static float WUltimateDamage(Obj_AI_Base target)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                    new[] {0, 125, 225, 325}[Program.RActive.Level] +
                    (Player.Instance.TotalMagicalDamage*0.5f));
            }

            /// <summary>
            /// Returns the Damage of E Ultimate Form
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            private static float EUltimateDamage(Obj_AI_Base target)
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                    new[] {0, 100, 160, 220}[Program.RActive.Level] +
                    (Player.Instance.TotalMagicalDamage*0.4f));
            }
        }
    }
}