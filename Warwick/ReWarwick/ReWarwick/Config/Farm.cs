using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReWarwick.Utils;

namespace ReWarwick.Config
{
    public static class Farm
    {
        public static readonly Menu Menu;

        static Farm()
        {
            Menu = MenuLoader.Menu.AddSubMenu("Farm");
            Menu.AddGroupLabel("Farm settings");

            Menu.AddGroupLabel("Q settings");
            Menu.CreateCheckBox("Kullan koridor / orman temizlemede", "Config.Farm.Q.Status");
            Menu.CreateCheckBox("Kullan son vurusda", "Config.Farm.Q.LastHit");
            Menu.CreateCheckBox("Kullan oldurulemiyecek minyona", "Config.Farm.Q.Unkillable");
            Menu.CreateSlider("mana yuzdesi >= {0}%", "Config.Farm.Q.Mana", 45, 1, 100);

            Menu.AddGroupLabel("E settings");
            Menu.CreateCheckBox("Kullan koridor / orman temizlemede", "Config.Farm.E.Status");
            Menu.CreateSlider("Sadece yakindaki minyonlara kullan >= {0}", "Config.Farm.E.Near", 3, 1, 5);
            Menu.CreateSlider("mana yuzdesi >= {0}%", "Config.Farm.E.Mana", 45, 1, 100);
        }

        public static void Initialize()
        {
        }
    }
}