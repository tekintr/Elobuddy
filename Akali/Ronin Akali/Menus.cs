using System.Drawing;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Eclipse
{
    internal class Menus
    {
        public static Menu FirstMenu;
        public static Menu ComboMenu;
        public static Menu HarassMenu;
        public static Menu LaneClearMenu;
        public static Menu LasthitMenu;
        public static Menu JungleClearMenu;
        public static Menu KillStealMenu;
        public static Menu DrawingsMenu;
        public static Menu MiscMenu;

        public const string ComboMenuID = "combomenuid";
        public const string HarassMenuID = "harassmenuid";
        public const string LaneClearMenuID = "laneclearmenuid";
        public const string LastHitMenuID = "lasthitmenuid";
        public const string JungleClearMenuID = "jungleclearmenuid";
        public const string KillStealMenuID = "killstealmenuid";
        public const string DrawingsMenuID = "drawingsmenuid";
        public const string MiscMenuID = "miscmenuid";

        public static void CreateMenu()
        {
            FirstMenu = MainMenu.AddMenu("Ronin "+Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower() + "ronin");
			FirstMenu.AddGroupLabel("Yapimci by Taazuma / Kullandiginiz icin tesekkurler.");
            FirstMenu.AddLabel("Turkce Ceviri TekinTR");
            FirstMenu.AddLabel("Iyi oyunlar :)");
            ComboMenu = FirstMenu.AddSubMenu("Kombo", ComboMenuID);
            HarassMenu = FirstMenu.AddSubMenu("Durtme", HarassMenuID);
            LaneClearMenu = FirstMenu.AddSubMenu("Minyon", LaneClearMenuID);
            //LasthitMenu = FirstMenu.AddSubMenu("LastHit", LastHitMenuID);
            JungleClearMenu = FirstMenu.AddSubMenu("Orman", JungleClearMenuID);
            KillStealMenu = FirstMenu.AddSubMenu("Oldurme", KillStealMenuID);
            DrawingsMenu = FirstMenu.AddSubMenu("Cizimler", DrawingsMenuID);
            MiscMenu = FirstMenu.AddSubMenu("Karisik", MiscMenuID);

            ComboMenu.AddGroupLabel("Kombo");
            ComboMenu.AddGroupLabel("Sadece Kombo kullan");
            ComboMenu.AddSeparator(15);
            // --------------------------------------------------------------COMBO LOGICS-------------------------------------------------------------- //
            ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            ComboMenu.AddLabel("Logics");
            ComboMenu.AddSeparator(4);
            ComboMenu.Add("Comba", new ComboBox(" Kombo Turu ", 0, "Normal", "Q AA", "R Q AA", "Insec"));
            ComboMenu.Add("WC", new ComboBox(" W Pozisyonu ", 1, "W Fare", "W Dusman", "W Guvenli"));
            ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            ComboMenu.AddLabel("Spells");
            ComboMenu.CreateCheckBox(" - Kullan Q", "qUse");
            ComboMenu.CreateCheckBox(" - Kullan W", "wUse");
            ComboMenu.CreateCheckBox(" - Kullan E", "eUse");
            ComboMenu.CreateCheckBox(" - Kullan R", "rUse");
            ComboMenu.AddLabel("Humanizer Settings");
            ComboMenu.Add("Qdelay", new Slider("Q Gecikmesi (ms)", 0, 0, 300));
            ComboMenu.Add("Edelay", new Slider("E Gecikmesi (ms)", 0, 0, 300));
            ComboMenu.Add("Rdelay", new Slider("R Gecikmesi (ms)", 0, 0, 300));
            ComboMenu.AddSeparator(10);
            ComboMenu.AddLabel("Logic Infos");
            ComboMenu.AddLabel("1. Combo Q - AA - R - E - W");
            ComboMenu.AddSeparator(6);
            ComboMenu.AddLabel("2. Combo Q - AA - E - R - W");
            ComboMenu.AddSeparator(6);
            ComboMenu.AddLabel("3. Combo R - Q - AA - E - W");
            ComboMenu.AddSeparator(6);
            ComboMenu.AddLabel("4. Combo (0 Checks) in a sec");
            ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            HarassMenu.AddGroupLabel("Durtme");
            HarassMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            HarassMenu.CreateCheckBox(" - Kullan Q", "qUse", false);
            HarassMenu.CreateCheckBox(" - Kullan AA Reset", "qUse", false);
            HarassMenu.CreateCheckBox(" - Kullan E", "eUse");
            HarassMenu.AddGroupLabel("Settings");
            HarassMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            LaneClearMenu.AddGroupLabel("Minyon");
            LaneClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            LaneClearMenu.CreateCheckBox(" - Kullan Q", "qUse");
            LaneClearMenu.CreateCheckBox(" - Kullan AA Reset", "aaclear");
            LaneClearMenu.CreateCheckBox(" - Kullan E", "eUse");
            LaneClearMenu.CreateCheckBox(" - Kullan R", "rUse", false);
            LaneClearMenu.AddGroupLabel("Settings");
            LaneClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            JungleClearMenu.AddGroupLabel("Orman");
            JungleClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            JungleClearMenu.CreateCheckBox(" - Kullan Q", "qUse");
            JungleClearMenu.CreateCheckBox(" - Kullan AA reset", "abclear");
            JungleClearMenu.CreateCheckBox(" - Kullan E", "eUse");
            JungleClearMenu.CreateCheckBox(" - Kullan R", "rUse", false);
            JungleClearMenu.AddGroupLabel("Settings");
            JungleClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            KillStealMenu.AddGroupLabel("Oldurme");
            KillStealMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            KillStealMenu.CreateCheckBox(" - Kullan Q", "qUse", false);
            KillStealMenu.CreateCheckBox(" - Kullan E", "eUse");
            KillStealMenu.CreateCheckBox("- Kullan R", "rUse", false);
            KillStealMenu.AddGroupLabel("News");
            KillStealMenu.AddLabel("Yoyo yeni KS");
            KillStealMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            DrawingsMenu.AddGroupLabel("Ayarlar");
            DrawingsMenu.AddGroupLabel("Cizimler");
            DrawingsMenu.Add("me", new CheckBox("Benim konumu", false));
            DrawingsMenu.Add("ally", new CheckBox("Takim konumu", false));
            DrawingsMenu.Add("enemy", new CheckBox("Dusman konumu", true));
            DrawingsMenu.AddLabel("Tracker Misc");
            DrawingsMenu.Add("toggle", new KeyBind("Ac/Kapa Tusu", true, KeyBind.BindTypes.PressToggle, 'G'));
            DrawingsMenu.Add("eta", new CheckBox("Tahmini varis suresi (sadece ben)", true));
            DrawingsMenu.Add("name", new CheckBox("Sampiyon ismi", true));
            DrawingsMenu.Add("thick", new Slider("Cizim kalinligi", 2, 1, 5));
            DrawingsMenu.AddGroupLabel("Orbwalker kullanirken devre disi birak");
            DrawingsMenu.Add("combo", new CheckBox("Kombo", true));
            DrawingsMenu.Add("harass", new CheckBox("Durtme", true));
            DrawingsMenu.Add("laneclear", new CheckBox("Minyon", false));
            DrawingsMenu.Add("lasthit", new CheckBox("SonVurus", true));
            DrawingsMenu.Add("flee", new CheckBox("Kacis", false));

            MiscMenu.AddGroupLabel("Ayarlar");
            MiscMenu.CreateCheckBox("Otomatik Q", "autoq", false);
            MiscMenu.CreateCheckBox("W dusuk oldugunda", "wlow", false);
            MiscMenu.CreateCheckBox("Kullan Item", "useitems");
            MiscMenu.AddLabel("Seviye arttirma");
            MiscMenu.Add("lvlup", new CheckBox("Skill otomatik seviye arttir", false));
            MiscMenu.Add("Lvldelay", new Slider("Lvlup Gecikmesi (ms)", 0, 0, 500));
            MiscMenu.AddSeparator(15);
            MiscMenu.CreateCheckBox("Skin Secici Aktif", "skinhax", false);
            MiscMenu.Add("skinID", new ComboBox("Skin Hack", 0, "Default", "Crimson Akali", "Stinger Akali", "All-Star Akali", "Nurse Akali", "Blood Moon Akali", "Silverfang Akali", "Headhunter Akali", "Sashimi Akali"));
        }
        public static int Qdelay { get { return ComboMenu["Qdelay"].Cast<Slider>().CurrentValue; } }
        public static int Edelay { get { return ComboMenu["Edelay"].Cast<Slider>().CurrentValue; } }
        public static int Rdelay { get { return ComboMenu["Rdelay"].Cast<Slider>().CurrentValue; } }
        public static int Lvldelay { get { return MiscMenu["Lvldelay"].Cast<Slider>().CurrentValue; } }
    }
}
