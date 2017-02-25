using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReWarwick.ReCore.Utility;
using System.Linq;

namespace ReWarwick.ReCore.Config
{
    public static class OItems
    {
        public static readonly Menu Menu;

        static OItems()
        {
            Menu = Loader.Menu.AddSubMenu("Offensive items");
            Menu.AddGroupLabel("Offensive items settings");

            #region Tiamat / Ravenous Hydra
            Menu.AddGroupLabel("Tiamat / Ravenous Hydra");
            Menu.CreateCheckBox("Use Tiamat / Ravenous Hydra", "Items.Offensive.Tiamat.Status");
            Menu.CreateCheckBox("Use in combo", "Items.Offensive.Tiamat.Combo");
            Menu.CreateCheckBox("Use in farm", "Items.Offensive.Tiamat.Farm");
            Menu.AddSeparator(15);  
            #endregion
            #region Titanic Hydra
            Menu.AddGroupLabel("Titanic Hydra");
            Menu.CreateCheckBox("Use Titanic Hydra", "Items.Offensive.TitanicHydra.Status");
            Menu.CreateSlider("Use only if target distance <= {0} units", "Items.TitanicHydra.Distance", 150, 100, 500);
            Menu.AddSeparator(15);
            #endregion
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
            #region Hextech GLP-800
            Menu.AddGroupLabel("Hextech GLP-800");
            Menu.CreateCheckBox("Use Hextech Hextech GLP-800", "Items.Offensive.HextechGLP.Status");
            Menu.CreateCheckBox("Use only in combo mode", "Items.Offensive.HextechGLP.ComboOnly");

            Menu.CreateSlider("Use only if my HP >= {0}%", "Items.Offensive.HextechGLP.Me.MinHealth", 35, 1, 100);
            Menu.CreateSlider("Use only if target HP >= {0}%", "Items.Offensive.HextechGLP.Enemy.MinHealth", 50, 1, 100);
            Menu.CreateSlider("Use only if enemies near >= {0}", "Items.Offensive.HextechGLP.Enemies", 1, 1, 5);
            Menu.AddSeparator(15);
            #endregion
            #region Hextech Protobelt-01
            Menu.AddGroupLabel("Hextech Protobelt-01");
            Menu.CreateCheckBox("Use Hextech Hextech GLP-800", "Items.Offensive.Protobelt.Status");
            Menu.CreateCheckBox("Use only in combo mode", "Items.Offensive.Protobelt.ComboOnly");
            Menu.CreateCheckBox("Use when is wall between me and target", "Items.Offensive.Protobelt.EnableWall", false);
            Menu.CreateCheckBox("Use versus melee champions", "Items.Offensive.Protobelt.Melee", false);

            Menu.CreateSlider("Use only if my HP >= {0}%", "Items.Offensive.Protobelt.Me.MinHealth", 35, 1, 100);
            Menu.CreateSlider("Use only if target HP >= {0}%", "Items.Offensive.Protobelt.Enemy.MinHealth", 50, 1, 100);
            Menu.CreateSlider("Use only if enemies near >= {0}", "Items.Offensive.Protobelt.Enemies", 1, 1, 5);
            Menu.AddSeparator(15);
            #endregion
        }

        public static void Initialize()
        {
        }
    }
}