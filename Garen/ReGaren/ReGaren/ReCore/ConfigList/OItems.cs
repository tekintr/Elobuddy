using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReGaren.ReCore.Utility;
using System.Linq;

namespace ReGaren.ReCore.ConfigList
{
    public static class OItems
    {
        public static readonly Menu Menu;

        static OItems()
        {
            Menu = Loader.Menu.AddSubMenu("Offensive items");
            Menu.AddGroupLabel("Offensive items settings");

            #region Youmuu's Ghostblade
            Menu.AddGroupLabel("Youmuu's Ghostblade");
            Menu.CreateCheckBox("Use Youmuu's Ghostblade", "Items.Offensive.Youmuu.Status");
            Menu.CreateCheckBox("Use only in combo mode", "Items.Offensive.Youmuu.ComboOnly");

            Menu.CreateSlider("Use only if my HP >= {0}%", "Items.Offensive.Youmuu.Me.MinHealth", 35, 1, 100);
            Menu.CreateSlider("Use only if target HP >= {0}%", "Items.Offensive.Youmuu.Enemy.MinHealth", 50, 1, 100);
            Menu.CreateSlider("Use only if enemies near >= {0}", "Items.Offensive.Youmuu.Enemies", 1, 1, 5);
            Menu.AddSeparator(15);
            #endregion
            #region Bilgewater Cutlass
            Menu.AddGroupLabel("Bilgewater Cutlass");
            Menu.CreateCheckBox("Use Bilgewater Cutlass", "Items.Offensive.Cutlass.Status");
            Menu.CreateCheckBox("Use only in combo mode", "Items.Offensive.Cutlass.ComboOnly");

            Menu.CreateSlider("Use only if my HP >= {0}%", "Items.Offensive.Cutlass.Me.MinHealth", 35, 1, 100);
            Menu.CreateSlider("Use only if target HP >= {0}%", "Items.Offensive.Cutlass.Enemy.MinHealth", 50, 1, 100);
            Menu.CreateSlider("Use only if enemies near >= {0}", "Items.Offensive.Cutlass.Enemies", 1, 1, 5);
            Menu.AddSeparator(15);
            #endregion
            #region Blade of the Ruined King
            Menu.AddGroupLabel("Blade of the Ruined King");
            Menu.CreateCheckBox("Use Blade of the Ruined King", "Items.Offensive.Botrk.Status");
            Menu.CreateCheckBox("Use only in combo mode", "Items.Offensive.Botrk.ComboOnly");

            Menu.CreateSlider("Use only if my HP >= {0}%", "Items.Offensive.Botrk.Me.MinHealth", 35, 1, 100);
            Menu.CreateSlider("Use only if target HP >= {0}%", "Items.Offensive.Botrk.Enemy.MinHealth", 50, 1, 100);
            Menu.CreateSlider("Use only if enemies near >= {0}", "Items.Offensive.Botrk.Enemies", 1, 1, 5);
            Menu.AddSeparator(15);
            #endregion
            #region Hextech Gunblade
            Menu.AddGroupLabel("Hextech Gunblade");
            Menu.CreateCheckBox("Use Hextech Gunblade", "Items.Offensive.Gunblade.Status");
            Menu.CreateCheckBox("Use only in combo mode", "Items.Offensive.Gunblade.ComboOnly");

            Menu.CreateSlider("Use only if my HP >= {0}%", "Items.Offensive.Gunblade.Me.MinHealth", 35, 1, 100);
            Menu.CreateSlider("Use only if target HP >= {0}%", "Items.Offensive.Gunblade.Enemy.MinHealth", 50, 1, 100);
            Menu.CreateSlider("Use only if enemies near >= {0}", "Items.Offensive.Gunblade.Enemies", 1, 1, 5);
            Menu.AddSeparator(15);
            #endregion

            Menu.CreateSlider("Enemy champions detection range", "Items.Offensive.Range", 700, 300, 1000);
        }

        public static void Initialize()
        {
        }
    }
}