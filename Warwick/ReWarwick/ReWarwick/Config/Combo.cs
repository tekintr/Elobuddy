using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReWarwick.Utils;

namespace ReWarwick.Config
{
    public static class Combo
    {
        public static readonly Menu Menu;

        static Combo()
        {
            Menu = MenuLoader.Menu.AddSubMenu("Combo");
            Menu.AddGroupLabel("Combo settings");

            Menu.AddGroupLabel("Q settings");
            Menu.CreateCheckBox("Kombo icinde Kullan", "Config.Combo.Q.Status");

            Menu.AddGroupLabel("E settings");
            Menu.CreateCheckBox("Kombo icinde Kullan", "Config.Combo.E.Status");
            Menu.CreateCheckBox("Aninda Kullan E2", "Config.Combo.E.After", false);

            Menu.AddGroupLabel("R settings");
            Menu.CreateCheckBox("Kombo icinde Kullan", "Config.Combo.R.Status");
            Menu.CreateCheckBox("Kule altina atlamaya izin ver", "Config.Combo.R.Dive", false);
            Menu.CreateCheckBox("Guc modunda beyaz listeyi yoksay", "Config.Combo.R.IgnoreForce");
            Menu.CreateCheckBox("Orbwalk to mouse in Force Mode", "Config.Combo.R.OrbWalk");
            Menu.AddLabel("Whitelist :");
            foreach (var e in EntityManager.Heroes.Enemies)
            {
                Menu.CreateCheckBox($"Use on {e.ChampionName}", $"Config.Combo.R.Use.{e.ChampionName}");
            }
            Menu.CreateKeyBind("Zorla R kullan", "Config.Combo.R.Force", 85, KeyBind.UnboundKey, KeyBind.BindTypes.HoldActive, false);
            Menu.CreateSlider("Sadece kullan isabet sansi ise >= {0}%", "Config.Combo.R.HitChance", 80, 1, 100);
            Menu.CreateSlider("Kullan kombo icinde sadece hedef cani ise >= {0}%", "Config.Combo.R.TargetHealth", 40, 1, 100);
        }

        public static void Initialize()
        {
        }
    }
}