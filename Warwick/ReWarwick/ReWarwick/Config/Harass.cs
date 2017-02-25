using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReWarwick.Utils;

namespace ReWarwick.Config
{
    public static class Harass
    {
        public static readonly Menu Menu;

        static Harass()
        {
            Menu = MenuLoader.Menu.AddSubMenu("Harass");
            Menu.AddGroupLabel("Harass settings");

            Menu.AddGroupLabel("Q settings");
            Menu.CreateCheckBox("Durtmede kullan", "Config.Harass.Q.Status");
            Menu.CreateCheckBox("Otomatik durtme kullan", "Config.AutoHarass.Q.Status");
            Menu.CreateSlider("Oto durtme sansi", "Config.AutoHarass.Q.Chance", 65, 1, 100);
            Menu.AddLabel("100% = always, 0% = never.", 15);
            Menu.CreateSlider("Mana ayari >= {0}%", "Config.Harass.Q.Mana", 45, 1, 100);

            Menu.AddGroupLabel("E settings");
            Menu.CreateCheckBox("Durterken kullan", "Config.Harass.E.Status");
            Menu.CreateCheckBox("Kullan otomatik durtmede", "Config.AutoHarass.E.Status");
            Menu.CreateCheckBox("Aninda kullan E2", "Config.Harass.E.After", false);
            Menu.CreateSlider("Oto durtme sansi", "Config.AutoHarass.E.Chance", 35, 1, 100);
            Menu.AddLabel("100% = surekli, 0% = asla", 15);
            Menu.CreateSlider("Mana ayari >= {0}%", "Config.Harass.E.Mana", 45, 1, 100);

            Menu.CreateSlider("Aktif otomatik durtme benim canim ise >= {0}%", "Config.AutoHarass.Health", 65, 1, 100);
            Menu.CreateSlider("Otomatik durtmeyi aktiflestir yakinda dusman varsa <= {0}", "Config.AutoHarass.Enemies", 1, 1, 5);
        }

        public static void Initialize()
        {
        }
    }
}