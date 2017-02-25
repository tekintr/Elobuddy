using System.Drawing;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using static Eclipse.SpellsManager;
using static Eclipse.Menus;
namespace Eclipse
{
    internal class Menus
    {
        public static Menu FirstMenu;
        public static Menu ComboMenu;
        public static Menu HarassMenu;
        public static Menu AutoHarassMenu;
        public static Menu LaneClearMenu;
        public static Menu LasthitMenu;
        public static Menu JungleClearMenu;
        public static Menu KillStealMenu;
        public static Menu DrawingsMenu;
        public static Menu MiscMenu;

        public const string ComboMenuID = "combomenuid";
        public const string HarassMenuID = "harassmenuid";
        public const string AutoHarassMenuID = "autoharassmenuid";
        public const string LaneClearMenuID = "laneclearmenuid";
        public const string LastHitMenuID = "lasthitmenuid";
        public const string JungleClearMenuID = "jungleclearmenuid";
        public const string KillStealMenuID = "killstealmenuid";
        public const string DrawingsMenuID = "drawingsmenuid";
        public const string MiscMenuID = "miscmenuid";

        public static void CreateMenu()
        {
            FirstMenu = MainMenu.AddMenu("God "+Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower() + "god");
            FirstMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            FirstMenu.AddGroupLabel("Addon by Taazuma / Thanks for using it");
            FirstMenu.AddLabel("If you found any bugs report it on my Thread");
            FirstMenu.AddLabel("Have fun with Playing");
            FirstMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            ComboMenu = FirstMenu.AddSubMenu("Combo", ComboMenuID);
            HarassMenu = FirstMenu.AddSubMenu("Harass", HarassMenuID);
            //AutoHarassMenu = FirstMenu.AddSubMenu("AutoHarass", AutoHarassMenuID);
            LaneClearMenu = FirstMenu.AddSubMenu("LaneClear", LaneClearMenuID);
            //LasthitMenu = FirstMenu.AddSubMenu("LastHit", LastHitMenuID);
            JungleClearMenu = FirstMenu.AddSubMenu("JungleClear", JungleClearMenuID);
            //KillStealMenu = FirstMenu.AddSubMenu("KillSteal", KillStealMenuID);
            MiscMenu = FirstMenu.AddSubMenu("Misc", MiscMenuID);
            DrawingsMenu = FirstMenu.AddSubMenu("Drawings", DrawingsMenuID);

            ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            ComboMenu.AddLabel("ComboLogics");
            ComboMenu.Add("Comba", new ComboBox(" Combo Logic ", 0, "GankCombo", "Teamfight Normal"));
            ComboMenu.AddSeparator(10);
            ComboMenu.AddLabel("Information:");
            ComboMenu.AddSeparator(5);
            ComboMenu.AddLabel("Gank: Q - when CC -> R - W - E");
            ComboMenu.AddSeparator(5);
            ComboMenu.AddLabel("Team: Q - R - W - E");
            ComboMenu.AddSeparator(10);
            ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            ComboMenu.AddStringList("QQ", "Use Q", new[] { "Never", "Only on target", "On any enemy" }, 1);
            ComboMenu.AddSeparator(10);
            ComboMenu.CreateCheckBox("Kullan E", "eUse");
            ComboMenu.AddSeparator(10);
            ComboMenu.CreateCheckBox("Kullan W", "wUse");
            ComboMenu.CreateCheckBox("Kullan R", "rUse");
            ComboMenu.AddSeparator(10);
            ComboMenu.Add("enemyr", new Slider("Takim savasinda R icin ", 3, 1, 5));
            ComboMenu.AddSeparator(10);
            ComboMenu.AddLabel("Humanizer Settings");
            ComboMenu.Add("Qdelay", new Slider("Q Gecikme (ms)", 0, 0, 300));
            ComboMenu.Add("Wdelay", new Slider("W Gecikme (ms)", 0, 0, 300));
            ComboMenu.Add("Edelay", new Slider("E Gecikme (ms)", 0, 0, 300));

            HarassMenu.AddGroupLabel("Harass");
            HarassMenu.CreateCheckBox("Kullan E", "eUse");
            HarassMenu.AddGroupLabel("Settings");
            HarassMenu.CreateSlider("Mana yuksek ise [{0}%] durtme kullan", "manaSlider", 50);

            LaneClearMenu.AddGroupLabel("LaneClear");
            LaneClearMenu.CreateCheckBox("Kullan E", "eUse");
            LaneClearMenu.AddGroupLabel("Settings");
            LaneClearMenu.CreateSlider("Mana yuksek ise [{0}%] durtme kullan", "manaSlider", 50);

            JungleClearMenu.AddGroupLabel("JungleClear");
            JungleClearMenu.CreateCheckBox("Kullan Q", "qUse");
            JungleClearMenu.CreateCheckBox("Kullan W", "wUse");
            JungleClearMenu.CreateCheckBox("Kullan E", "eUse");
            JungleClearMenu.AddGroupLabel("Settings");
            JungleClearMenu.CreateSlider("Mana yuksek ise [{0}%] durtme kullan", "manaSlider", 20);

            MiscMenu.AddGroupLabel("Settings");
            MiscMenu.AddSeparator(7);
            MiscMenu.Add("smartW", new CheckBox("Oto kapat W (Akilli)"));
            MiscMenu.Add("WSdelay", new Slider("Akilli W Gecikmesi (ms)", 0, 0, 500));
            MiscMenu.AddSeparator(12);
            MiscMenu.AddLabel("Level Up Function");
            MiscMenu.Add("lvlup", new CheckBox("Oto seviye arttir", false));
            MiscMenu.AddSeparator(15);
            MiscMenu.Add("skinhax", new CheckBox("Skin secici"));
            MiscMenu.Add("skinID", new ComboBox("Skin secici", 0, "Default", "Sad Robot Amumu", "Little Knight Amumu", "Emumu", "Almost-Prom King Amumu", "Pharaoh Amumu", "Surprise Party Amumu", "Re-Gifted Amumu", "Vancouver Amumu"));

            DrawingsMenu.AddGroupLabel("Settings");
            DrawingsMenu.AddGroupLabel("Spells");
            DrawingsMenu.CreateCheckBox("Goster Q.", "qDraw");
            DrawingsMenu.CreateCheckBox("Goster W.", "wDraw", false);
            DrawingsMenu.CreateCheckBox("Goster E.", "eDraw", false);
            DrawingsMenu.CreateCheckBox("Goster R.", "rDraw", false);

        }
        public static int Qdelay { get { return ComboMenu["Qdelay"].Cast<Slider>().CurrentValue; } }
        public static int Wdelay { get { return ComboMenu["Wdelay"].Cast<Slider>().CurrentValue; } }
        public static int Edelay { get { return ComboMenu["Edelay"].Cast<Slider>().CurrentValue; } }
        public static int WSdelay { get { return MiscMenu["WSdelay"].Cast<Slider>().CurrentValue; } }
    }
}
