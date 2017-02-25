using EloBuddy;
using System.Linq;
using EloBuddy.SDK;
using ReGaren.ReCore.ConfigList;
using ReGaren.ReCore.Managers;
using ReGaren.ReCore.Utility;

namespace ReGaren.ReCore.Core.Spells
{
    class Exhaust : ISpell
    {
        public void Execute()
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                var enemy = EloBuddy.SDK.EntityManager.Heroes.Enemies.
                    Where(e =>
                        !e.IsDead &&
                        e.IsInRange(e, SummonerManager.Exhaust.Range) &&
                        e.TotalShieldHealth() <= MenuHelper.GetSliderValue(Summoners.Menu, "Summoners.Exhaust.Health"));
                SummonerManager.Exhaust.Cast(enemy.FirstOrDefault());
            }
        }

        public bool ShouldGetExecuted()
        {
            if (!SummonerManager.Exhaust.IsReady() || !MenuHelper.GetCheckBoxValue(Summoners.Menu, "Summoners.Exhaust.Status"))
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
