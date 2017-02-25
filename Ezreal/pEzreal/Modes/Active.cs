using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using pEzreal.Extensions;

namespace pEzreal.Modes
{
    internal class Active
    {
        public static void Initialize()
        {
            if (Config.MyHero.IsRecalling() || Config.MyHero.IsDead) return;

            Killsteal.Execute();
            if (Config.UseQSS) Cleanse();
            if (Config.TearStacking && Config.MyHero.IsInShopRange()) TearStacking();
        }

        private static void Cleanse()
        {
            if (!IsImmobile(Config.MyHero)) return;

            if (Spells.QSS.IsOwned() && Spells.QSS.IsReady())
            {
                Spells.QSS.Cast();
            }
            else if (Spells.Mercurial.IsOwned() && Spells.Mercurial.IsReady())
            {
                Spells.Mercurial.Cast();
            }
        }

        private static void TearStacking()
        {
            if (Spells.Manamune.IsOwned() || Spells.TearOfTheGoddess.IsOwned())
            {
                Spells.Q.Cast(Game.CursorPos);
                Spells.W.Cast(Game.CursorPos);
            }
        }

        private static bool IsImmobile(Obj_AI_Base target)
        {
            return target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare) ||
                   target.HasBuffOfType(BuffType.Knockup) || target.HasBuffOfType(BuffType.Charm) ||
                   target.HasBuffOfType(BuffType.Fear) || target.HasBuffOfType(BuffType.Knockback) ||
                   target.HasBuffOfType(BuffType.Taunt) || target.HasBuffOfType(BuffType.Suppression) ||
                   target.IsStunned;
        }

        /// <summary>
        /// Returns true if target is killable. Definitely not Kappa
        /// </summary>
        public static bool IsKillable(AIHeroClient target)
        {
            return !target.HasBuff("kindredrnodeathbuff") && !target.HasUndyingBuff(true) && !target.Buffs.Any(b => b.Name.ToLower().Contains("fioraw")) && !target.HasBuff("JudicatorIntervention") && !target.IsZombie
                   && !target.HasBuff("ChronoShift") && !target.HasBuff("UndyingRage") && !target.IsInvulnerable && !target.IsZombie && !target.HasBuff("bansheesveil") && !target.IsDead
                   && !target.IsPhysicalImmune && !target.HasBuffOfType(BuffType.Invulnerability) && !target.HasBuffOfType(BuffType.PhysicalImmunity) && target.IsValidTarget();
        }
    }
}