using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Black_Swan_Akali.Assistants
{
    public static class MenuDesigner
    {
        public const string MenuName = "Black Swan Akali";

        public static readonly Menu AkaliUi, ComboUi, HarassUi, ClearUi, KsUi, MiscUi;

        static MenuDesigner()
        {
            // Black Swan Akali :: Main Menu
            AkaliUi = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            AkaliUi.AddGroupLabel("Versiyon 2.0.0.0");
            AkaliUi.AddSeparator();
            AkaliUi.AddLabel("Yapimci    :   Enelx");
            AkaliUi.AddLabel("Ceviri     :   TekinTR");

            // Black Swan Akali :: Combo Menu
            ComboUi = AkaliUi.AddSubMenu("Combo");
            ComboUi.AddGroupLabel("Combo :: Spells");
            ComboUi.Add("ComboQ", new CheckBox("Kullan Q"));
            ComboUi.Add("ComboW", new CheckBox("Kullan W"));
            ComboUi.Add("ComboE", new CheckBox("Kullan E"));
            ComboUi.Add("ComboR", new CheckBox("Kullan R"));

            // Black Swan Akali :: Harass Menu
            HarassUi = AkaliUi.AddSubMenu("Harass");
            HarassUi.AddGroupLabel("Harass :: Spells");
            HarassUi.Add("HarassQ", new CheckBox("Kullan Q"));

            // Black Swan Akali :: Clear Menu
            ClearUi = AkaliUi.AddSubMenu("Clear");
            ClearUi.AddGroupLabel("Last Hit :: Spells");
            ClearUi.Add("LastQ", new CheckBox("Kullan Q"));
            ClearUi.Add("LastE", new CheckBox("Kullan E", false));
            ClearUi.AddSeparator();
            ClearUi.AddGroupLabel("Lane Clear :: Spells / Settings");
            ClearUi.Add("ClearQ", new CheckBox("Kullan Q"));
            ClearUi.Add("ClearE", new CheckBox("Kullan E"));
            ClearUi.Add("ClearMana", new Slider("En az enerJi. %", 25));
            ClearUi.AddSeparator();
            ClearUi.AddGroupLabel("Jungle Clear :: Spells / Settings");
            ClearUi.Add("JungleQ", new CheckBox("Kullan Q"));
            ClearUi.Add("JungleE", new CheckBox("Kullan E"));
            ClearUi.Add("JungleMana", new Slider("En az enerJi. %", 10));

            // Black Swan Akali :: Killsteal Menu
            KsUi = AkaliUi.AddSubMenu("Killsteal");
            KsUi.AddGroupLabel("Killsteal :: Spells");
            KsUi.Add("KsQ", new CheckBox("Kullan Q"));
            KsUi.Add("KsR", new CheckBox("Kullan R"));

            // Black Swan Akali :: Misc Menu
            MiscUi = AkaliUi.AddSubMenu("Misc");
            MiscUi.AddGroupLabel("Misc :: Settings");
            MiscUi.Add("GapR", new CheckBox("Kullan R atilma yapanlara"));
            MiscUi.Add("FleeW", new CheckBox("Kullan W kacarken"));
            MiscUi.AddSeparator();
            MiscUi.AddGroupLabel("Misc :: Items");
            MiscUi.Add("UseItems", new CheckBox("Kullan Agresif item"));
            MiscUi.AddSeparator();
            MiscUi.AddGroupLabel("Misc :: Draw");
            MiscUi.Add("DrawQ", new CheckBox("Goster Q"));
            MiscUi.Add("DrawR", new CheckBox("Goster R"));
        }

        public static void Initialize()
        {
        }
    }
}