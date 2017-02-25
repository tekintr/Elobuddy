using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReWarwick.ReCore.Utility;
using System.Linq;

namespace ReWarwick.ReCore.Config
{
    public static class DItems
    {
        public static readonly Menu Menu;

        static DItems()
        {
            Menu = Loader.Menu.AddSubMenu("Deffensive items");
            Menu.AddGroupLabel("Deffensive items settings");

            #region Zhonya's Hourglass
            Menu.AddGroupLabel("Zhonya's Hourglass");
            Menu.CreateCheckBox("Kullan zonya", "Items.Deffensive.Zhonya.Status");
            Menu.CreateCheckBox("Sadece kombo modunda kullan", "Items.Deffensive.Zhonya.ComboOnly", false);

            Menu.CreateSlider("Kullan benim HP <= {0}%", "Items.Deffensive.Zhonya.Me.MinHealth", 5, 1, 100);
            Menu.CreateSlider("Yakinda dusman var ise kullan >= {0}", "Items.Deffensive.Zhonya.Enemies", 1, 1, 5);
            Menu.CreateSlider("Yakinda muttefik var ise kullan >= {0}", "Items.Deffensive.Zhonya.Allies", 1, 0, 4);
            Menu.AddSeparator(15);
            #endregion
            #region Locket of the Iron Solari
            Menu.AddGroupLabel("Locket of the Iron Solari");
            Menu.CreateCheckBox("Use Locket of the Iron Solari", "Items.Deffensive.Solari.Status");
            Menu.CreateCheckBox("Use only in combo mode", "Items.Deffensive.Solari.ComboOnly", false);

            Menu.CreateSlider("Use if my HP <= {0}%", "Items.Deffensive.Solari.Me.MinHealth", 35, 1, 100);
            Menu.CreateSlider("Use if ally HP <= {0}%", "Items.Deffensive.Solari.Ally.MinHealth", 35, 1, 100);
            Menu.CreateSlider("Use if sum of allies HP <= {0}%", "Items.Deffensive.Solari.Allies.MinHealth", 50, 1, 100);
            Menu.CreateSlider("Use only if enemies near >= {0}", "Items.Deffensive.Solari.Enemies", 1, 1, 5);
            Menu.CreateSlider("Use only if allies near >= {0}", "Items.Deffensive.Solari.Allies", 0, 0, 4);
            Menu.AddSeparator(15);
            #endregion
            #region Face of the Mountain
            Menu.AddGroupLabel("Face of the Mountain");
            Menu.CreateCheckBox("Use Face of the Mountain", "Items.Deffensive.Fotm.Status");
            Menu.CreateCheckBox("Use only in combo mode", "Items.Deffensive.Fotm.ComboOnly", false);
            
            Menu.CreateSlider("Use if my HP <= {0}%", "Items.Deffensive.Fotm.Me.MinHealth", 15, 1, 100);
            Menu.CreateSlider("Use if ally HP <= {0}%", "Items.Deffensive.Fotm.Ally.MinHealth", 15, 1, 100);
            foreach (var a in EntityManager.Heroes.Allies.Where(a => !a.IsMe))
            {
                Menu.CreateCheckBox($"Use on {a.ChampionName}", $"Items.Deffensive.Fotm.Use.{a.ChampionName}");
            }

            Menu.AddSeparator(15);
            #endregion
            #region Seraph's Embrace
            Menu.AddGroupLabel("Seraph's Embrace");
            Menu.CreateCheckBox("Use Seraph's Embrace", "Items.Deffensive.Seraph.Status");
            Menu.CreateCheckBox("Use only in combo mode", "Items.Deffensive.Seraph.ComboOnly", false);

            Menu.CreateSlider("Use if my HP <= {0}%", "Items.Deffensive.Seraph.Me.MinHealth", 20, 1, 100);
            Menu.CreateSlider("Use only if enemies near >= {0}", "Items.Deffensive.Seraph.Enemies", 1, 1, 5);
            Menu.AddSeparator(15);
            #endregion
            #region Redemption
            Menu.AddGroupLabel("Redemption");
            Menu.CreateCheckBox("Use Redemption", "Items.Deffensive.Redemption.Status");

            Menu.CreateSlider("Use if sum of allies HP <= {0}%", "Items.Deffensive.Redemption.Hp", 50, 1, 100);
            Menu.CreateSlider("Use only if allies in range >= {0}", "Items.Deffensive.Redemption.Allies", 2, 0, 4);
            Menu.CreateSlider("Use only if enemies near >= {0}", "Items.Deffensive.Redemption.Enemies", 1, 1, 5);
            Menu.AddSeparator(15);
            #endregion
            #region Randuin's Omen
            Menu.AddGroupLabel("Randuin's Omen");
            Menu.CreateCheckBox("Use Randuin's Omen", "Items.Deffensive.Omen.Status");
            Menu.CreateCheckBox("Use only in combo mode", "Items.Deffensive.Omen.ComboOnly");

            Menu.CreateSlider("Use only if my HP >= {0}%", "Items.Deffensive.Omen.Me.MinHealth", 35, 1, 100);
            Menu.CreateSlider("Use only if target HP <= {0}%", "Items.Deffensive.Omen.Enemy.MinHealth", 55, 1, 100);
            Menu.CreateSlider("Use only if enemies in range >= {0}", "Items.Deffensive.Omen.Enemy.Count", 1, 1, 5);
            Menu.AddSeparator(15);
            #endregion

            /*  TODO LIST
             *  - Mikael's Crucible
             *  - Zeke's Harbinger
             * */
        }

        public static void Initialize()
        {
        }
    }
}