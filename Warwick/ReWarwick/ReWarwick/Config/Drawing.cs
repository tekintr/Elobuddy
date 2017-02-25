using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReWarwick.Utils;

namespace ReWarwick.Config
{
    public static class Drawing
    {
        public static readonly Menu Menu;

        static Drawing()
        {
            Menu = MenuLoader.Menu.AddSubMenu("Drawing");
            Menu.AddGroupLabel("Drawing settings");

            Menu.CreateCheckBox("Goster Q Menzili", "Config.Drawing.Q", true);
            Menu.CreateCheckBox("Goster W Menzili", "Config.Drawing.W", false);
            Menu.CreateCheckBox("Goster E Menzili", "Config.Drawing.E", false);
            Menu.CreateCheckBox("Goster R Menzili", "Config.Drawing.R", true);
            Menu.CreateCheckBox("Goster verilicek hasar", "Config.Drawing.Indicator", true);
        }

        public static void Initialize()
        {
        }
    }
}