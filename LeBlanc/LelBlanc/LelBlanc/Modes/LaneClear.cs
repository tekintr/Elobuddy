using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace LelBlanc.Modes
{
    internal class LaneClear
    {
        public static bool UseQ => Config.LaneClearMenu["useQ"].Cast<CheckBox>().CurrentValue;

        public static bool UseW => Config.LaneClearMenu["useW"].Cast<CheckBox>().CurrentValue;

        public static int SliderQ => Config.LaneClearMenu["sliderQ"].Cast<Slider>().CurrentValue;

        public static int SliderW => Config.LaneClearMenu["sliderW"].Cast<Slider>().CurrentValue;

        public static void Execute()
        {
            if (UseQ && Program.Q.IsReady())
            {
                var minion = EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Minion,
                    EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition, Program.Q.Range)
                    .FirstOrDefault(
                        t => Extension.DamageLibrary.CalculateDamage(t, true, false, false, false, false) >= t.Health);

                if (minion != null)
                {
                    var minionCount =
                        EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Minion,
                            EntityManager.UnitTeam.Enemy, minion.ServerPosition, 300)
                            .Count(
                                t =>
                                    Extension.IsMarked(t) &&
                                    t.Health <=
                                    Extension.DamageLibrary.CalculateDamage(t, true, false, false, false, false));

                    if (minionCount >= SliderQ)
                    {
                        Program.Q.Cast(minion);
                    }
                }
            }
            if (UseW && Program.W.IsReady() &&
                Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() == "leblancw")
            {
                var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition, Program.W.Range)
                    .Where(t => Extension.DamageLibrary.CalculateDamage(t, false, true, false, false, false) >= t.Health);
                var wAoe = Program.W.GetBestCircularCastPosition(minion);

                if (wAoe.HitNumber >= SliderW)
                {
                    Program.W.Cast(wAoe.CastPosition);
                }
            }
            if (UseW && Program.WReturn.IsReady() &&
                Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() == "leblancwreturn")
            {
                Program.WReturn.Cast();
            }
        }
    }
}