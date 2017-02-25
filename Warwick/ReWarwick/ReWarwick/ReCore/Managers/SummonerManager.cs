using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReWarwick.ReCore.Managers
{
    class SummonerManager
    {
        public static Spell.Targeted Ignite;
        public static bool PlayerHasIgnite;

        public static Spell.Active Heal;
        public static bool PlayerHasHeal;

        public static Spell.Active Cleanse;
        public static bool PlayerHasCleanse;

        public static Spell.Targeted Exhaust;
        public static bool PlayerHasExhaust;

        public static Spell.Active Barrier;
        public static bool PlayerHasBarrier;

        public static Spell.Skillshot Snowball;
        public static bool PlayerHasSnowball;

        public static Spell.Targeted Smite;
        public static bool PlayerHasSmite;
        public enum SmiteType
        {
            Chilling,
            Challenging,
            None
        }

        public static void Initialize()
        {
            #region Ignite
            var ignite = Player.Spells.FirstOrDefault(s => s.Name.ToLower().Contains("summonerdot"));
            if (ignite != null)
            {
                Ignite = new Spell.Targeted(ignite.Slot, 600);
                PlayerHasIgnite = true;
            }
            #endregion
            #region Heal
            var heal = Player.Spells.FirstOrDefault(s => s.Name.ToLower().Contains("summonerheal"));
            if (heal != null)
            {
                Heal = new Spell.Active(heal.Slot, 850);
                PlayerHasHeal = true;
            }
            #endregion
            #region Cleanse
            var cleanse = Player.Spells.FirstOrDefault(s => s.Name.ToLower().Contains("summonerboost"));
            if (cleanse != null)
            {
                Cleanse = new Spell.Active(cleanse.Slot);
                PlayerHasCleanse = true;
            }
            #endregion
            #region Exhaust
            var exhaust = Player.Spells.FirstOrDefault(s => s.Name.ToLower().Contains("summonerexhaust"));
            if (exhaust != null)
            {
                Exhaust = new Spell.Targeted(exhaust.Slot, 650);
                PlayerHasExhaust = true;
            }
            #endregion
            #region Barrier
            var barrier = Player.Spells.FirstOrDefault(s => s.Name.ToLower().Contains("summonerbarrier"));
            if (barrier != null)
            {
                Barrier = new Spell.Active(barrier.Slot);
                PlayerHasBarrier = true;
            }
            #endregion
            #region Snowball
            var snowball = Player.Spells.FirstOrDefault(s => s.Name.ToLower().Contains("summonersnowball"));
            if (snowball != null)
            {
                Snowball = new Spell.Skillshot(snowball.Slot, (uint)snowball.SData.CastRange - 50, SkillShotType.Linear, 250, 1500, (int)snowball.SData.LineWidth);
                PlayerHasSnowball = true;
            }
            #endregion
            #region Smite
            var smite = Player.Spells.FirstOrDefault(s => s.Name.ToLower().Contains("summonersmite"));
            if (smite != null)
            {
                Smite = new Spell.Targeted(smite.Slot, 570);
                PlayerHasSmite = true;
                Core.Spells.Smite.Initialize();
            }
            #endregion
        }
    }
}
