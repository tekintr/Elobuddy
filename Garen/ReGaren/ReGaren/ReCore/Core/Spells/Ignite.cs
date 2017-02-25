using EloBuddy;
using System.Linq;
using EloBuddy.SDK;
using ReGaren.ReCore.ConfigList;
using ReGaren.ReCore.Managers;
using ReGaren.ReCore.Utility;

namespace ReGaren.ReCore.Core.Spells
{
    class Ignite : ISpell
    {
        public void Execute()
        {
            if (Summoners.Menu.GetCheckBoxValue("Summoners.Ignite.KillSteal"))
            {
                Obj_AI_Base ks = EloBuddy.SDK.EntityManager.Heroes.Enemies.FirstOrDefault(p =>
                                Prediction.Health.GetPrediction(p, Game.Ping) <= ReGaren.ReCore.Managers.EntityManager.GetIgniteDamage() &&
                                p.IsValidTarget(SummonerManager.Ignite.Range));
                if (ks != null && ks.IsValid)
                    SummonerManager.Ignite.Cast(ks);
            }
            Obj_AI_Base target = TargetSelector.GetTarget(SummonerManager.Ignite.Range, DamageType.True);
            if (target == null || !target.IsValid) return;
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;
            if (target.HealthPercent <= Summoners.Menu.GetSliderValue("Summoners.Ignite.Health"))
                SummonerManager.Ignite.Cast(target);
        }

        public bool ShouldGetExecuted()
        {
            if (!SummonerManager.Ignite.IsReady() || !MenuHelper.GetCheckBoxValue(Summoners.Menu, "Summoners.Ignite.Status"))
                return false;
            return true;
        }

        public void OnDraw()
        {

        }

        public void OnEndScene()
        {

        }
    }
}
