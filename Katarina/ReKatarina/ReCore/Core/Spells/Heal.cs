using EloBuddy;
using System.Linq;
using EloBuddy.SDK;
using ReKatarina.ReCore.ConfigList;
using ReKatarina.ReCore.Managers;
using ReKatarina.ReCore.Utility;

namespace ReKatarina.ReCore.Core.Spells
{
    class Heal : ISpell
    {
        public void Execute()
        {
            var enemies = Player.Instance.CountEnemyChampionsInRange(MenuHelper.GetSliderValue(ConfigList.Settings.Menu, "Settings.Range"));
            if (MenuHelper.GetCheckBoxValue(Protector.Menu, "Protector.Heal.Dangerous"))
            {
                if (enemies > 0 && Player.Instance.IsInDanger(MenuHelper.GetSliderValue(Protector.Menu, "Protector.Heal.Health.Me")))
                    SummonerManager.Heal.Cast();

                foreach (var d in EloBuddy.SDK.EntityManager.Heroes.Allies.Where(a => !a.IsMe && a.IsAlive() && !a.IsInvulnerable && a.IsInDanger(MenuHelper.GetSliderValue(Protector.Menu, "Protector.Heal.Health.Ally")) && MenuHelper.GetCheckBoxValue(Protector.Menu, $"Protector.Heal.Use.{a.ChampionName}")))
                    SummonerManager.Heal.Cast();
            }
            else
                if (Player.Instance.HealthPercent <= MenuHelper.GetSliderValue(Protector.Menu, "Protector.Heal.Health.Me"))
                    SummonerManager.Heal.Cast();
        }

        public bool ShouldGetExecuted()
        {
            if (!SummonerManager.Heal.IsReady() || !MenuHelper.GetCheckBoxValue(Protector.Menu, "Protector.Heal.Status"))
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
