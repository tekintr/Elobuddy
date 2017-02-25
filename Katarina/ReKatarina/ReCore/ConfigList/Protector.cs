using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using System.Linq;
using EloBuddy.SDK.Menu.Values;
using ReKatarina.ReCore.Utility;

namespace ReKatarina.ReCore.ConfigList
{
    public static class Protector
    {
        public static readonly Menu Menu;

        static Protector()
        {
            Menu = Loader.Menu.AddSubMenu("Protector");
            Menu.AddGroupLabel("Protector settings");

            if (Managers.SummonerManager.PlayerHasHeal)
            {
                Menu.CreateCheckBox("Enable heal", "Protector.Heal.Status");
                Menu.CreateCheckBox("Heal only dangerous", "Protector.Heal.Dangerous");
                Menu.CreateSlider("Heal if my HP <= {0}%", "Protector.Heal.Health.Me", 10);
                Menu.CreateSlider("Heal if ally HP <= {0}%", "Protector.Heal.Health.Ally", 10);
                Menu.AddSeparator(10);
                Menu.AddLabel("Whitelist : ");
                foreach (var ally in EntityManager.Heroes.Allies.Where(a => !a.IsMe))
                    Menu.CreateCheckBox(ally.ChampionName + " (" + ally.Name + ")", "Protector.Heal.Use." + ally.ChampionName);
            }
            else
            {
                Menu.AddSeparator(10);
                Menu.AddLabel("You don't have any spell to protect your allies.");
            }

        }

        public static void Initialize()
        {
        }
    }
}