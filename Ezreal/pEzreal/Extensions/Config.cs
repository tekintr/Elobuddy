using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace pEzreal.Extensions
{
    internal class Config
    {
        public static Menu Settings, Combo, Harass, Lasthit, LaneClear, JungleClear, Killsteal, Items, Misc, Drawing;
        public static AIHeroClient MyHero => Player.Instance;

        public static void Initialize()
        {
            Settings = MainMenu.AddMenu("pEzreal", "pEzreal");
            Settings.AddLabel("Yapımcı Zimmer.");
            Settings.AddLabel("Türkce ceviri TekinTR.");
            Settings.AddSeparator();
            Settings.AddLabel("Herhangi bir hata bulursanız yapımcı ile iletisime gecin.");

            //Combo Menu
            Combo = Settings.AddSubMenu("Combo", "ComboMenu");

            Combo.AddGroupLabel("Gizemli Atis");
            Combo.Add("Q", new CheckBox("Kullan"));

            Combo.AddGroupLabel("Ozut Akisi");
            Combo.Add("W", new CheckBox("Kullan"));

            Combo.AddGroupLabel("Sihir Gecisi");
            Combo.Add("E", new CheckBox("Kullan"));
            Combo.Add("E_mode",new ComboBox("Mode", 0, "Fareye dogru", "Dusmana", "Kapalı"));

            Combo.AddGroupLabel("Isabet Dalgasi");
            Combo.Add("R", new CheckBox("Kullan"));
            Combo.Add("REnemies", new Slider("Minimum enemies", 3, 0, 5));

            //Harass Menu
            Harass = Settings.AddSubMenu("Harass", "HarassMenu");

            Harass.AddGroupLabel("Gizemli Atis");
            Harass.Add("Q", new CheckBox("Kullan"));

            Harass.AddGroupLabel("Ozut Akisi");
            Harass.Add("W", new CheckBox("Kullan"));

            Harass.AddSeparator();
            Harass.Add("Mana", new Slider("Mana ayari", 30));

            //Lasthit Menu
            Lasthit = Settings.AddSubMenu("Lasthit", "LasthitMenu");

            Lasthit.AddGroupLabel("Gizemli Atis");
            Lasthit.Add("Q", new CheckBox("Kullan"));

            Lasthit.AddSeparator();
            Lasthit.Add("Mana", new Slider("Mana ayari", 30));

            //LaneClear Menu
            LaneClear = Settings.AddSubMenu("LaneClear", "LaneClearMenu");

            LaneClear.AddGroupLabel("Gizemli Atis");
            LaneClear.Add("Q", new CheckBox("Kullan"));

            LaneClear.AddSeparator();
            LaneClear.Add("Mana", new Slider("Mana ayari", 30));

            //JungleClear Menu
            JungleClear = Settings.AddSubMenu("JungleClear", "JungleClearMenu");

            JungleClear.AddGroupLabel("Gizemli Atis");
            JungleClear.Add("Q", new CheckBox("Kullan"));

            JungleClear.AddSeparator();
            JungleClear.Add("Mana", new Slider("Mana ayari", 30));

            //Killsteal Menu
            Killsteal = Settings.AddSubMenu("Killsteal", "KillstealMenu");

            Killsteal.AddGroupLabel("Gizemli Atis");
            Killsteal.Add("Q", new CheckBox("Aktif"));

            Killsteal.AddGroupLabel("Ozut akisi");
            Killsteal.Add("W", new CheckBox("Aktif"));

            Killsteal.AddGroupLabel("Isabet Dalgasi");
            Killsteal.Add("R", new CheckBox("Aktif"));

            //Drawing Menu
            Drawing = Settings.AddSubMenu("Drawing", "DrawingMenu");

            Drawing.AddGroupLabel("Gizemli Atis");
            Drawing.Add("Q", new CheckBox("Aktif"));

            Drawing.AddGroupLabel("Ozut akisi");
            Drawing.Add("W", new CheckBox("Aktif"));

            Drawing.AddGroupLabel("Sihir Gecisi");
            Drawing.Add("E", new CheckBox("Aktif"));

            Drawing.AddGroupLabel("Isabet Dalgasi");
            Drawing.Add("R", new CheckBox("Aktif"));

            Drawing.AddSeparator();
            Drawing.AddGroupLabel("Options");
            Drawing.Add("ready", new CheckBox("Sadece hazir olan skilleri goster?"));

            //Items Menu
            Items = Settings.AddSubMenu("Items", "ItemsMenu");

            Items.AddGroupLabel("Offensive");
            Items.Add("botrk", new CheckBox("Kullan Mahvolmus/Bilgewater"));
            Items.Add("botrkHealth", new Slider("Enaz Can", 65));
            Items.Add("youmuu", new CheckBox("Kullan Youmuu's Hayaletkilic"));

            Items.AddGroupLabel("Defensive");
            Items.Add("qss", new CheckBox("Kullan Civali", false));

            //Misc Menu
            Misc = Settings.AddSubMenu("Miscellaneous", "MiscMenu");

            Misc.AddGroupLabel("Skinchanger");
            Misc.Add("_skinChanger", new CheckBox("Aktif"));
            Misc.Add("skinID",
                new ComboBox("Current skin", 0, "Default", "Nottingham", "Striker", "Frosted", "Explorer", "Pulsefire",
                    "TPA", "Debonair", "Ace of Spades", "Arcade"));

            Misc.AddGroupLabel("Hitchance");
            Misc.Add("_hitchance", new ComboBox("Isabet sansini sec", 2, "Low", "Medium", "High"));

            Misc.AddGroupLabel("Tear stacking");
            Misc.Add("tearStacking", new KeyBind("Aktif", true, KeyBind.BindTypes.PressToggle, 'K'));
        }

        //Combo values
        public static bool ComboQ => Combo["Q"].Cast<CheckBox>().CurrentValue;
        public static bool ComboW => Combo["W"].Cast<CheckBox>().CurrentValue;
        public static bool ComboE => Combo["E"].Cast<CheckBox>().CurrentValue;
        public static int ComboEMode => Combo["E_mode"].Cast<ComboBox>().CurrentValue;
        public static bool ComboR => Combo["R"].Cast<CheckBox>().CurrentValue;
        public static int ComboREnemies => Combo["REnemies"].Cast<Slider>().CurrentValue;

        //Harass values
        public static bool HarassQ => Harass["Q"].Cast<CheckBox>().CurrentValue;
        public static bool HarassW => Harass["W"].Cast<CheckBox>().CurrentValue;
        public static int HarassMana => Harass["Mana"].Cast<Slider>().CurrentValue;

        //Lasthit values
        public static bool LasthitQ => Lasthit["Q"].Cast<CheckBox>().CurrentValue;
        public static int LasthitMana => Lasthit["Mana"].Cast<Slider>().CurrentValue;

        //LaneClear values
        public static bool LaneClearQ => LaneClear["Q"].Cast<CheckBox>().CurrentValue;
        public static int LaneClearMana => LaneClear["Mana"].Cast<Slider>().CurrentValue;

        //JungleClear values
        public static bool JungleClearQ => JungleClear["Q"].Cast<CheckBox>().CurrentValue;
        public static int JungleClearMana => JungleClear["Mana"].Cast<Slider>().CurrentValue;

        //Killsteal values
        public static bool KillstealQ => Killsteal["Q"].Cast<CheckBox>().CurrentValue;
        public static bool KillstealW => Killsteal["W"].Cast<CheckBox>().CurrentValue;
        public static bool KillstealR => Killsteal["R"].Cast<CheckBox>().CurrentValue;

        //Drawing values
        public static bool DrawQ => Drawing["Q"].Cast<CheckBox>().CurrentValue;
        public static bool DrawW => Drawing["W"].Cast<CheckBox>().CurrentValue;
        public static bool DrawE => Drawing["E"].Cast<CheckBox>().CurrentValue;
        public static bool DrawR => Drawing["R"].Cast<CheckBox>().CurrentValue;
        public static bool Ready => Drawing["ready"].Cast<CheckBox>().CurrentValue;

        //Items values
        public static bool UseQSS => Items["qss"].Cast<CheckBox>().CurrentValue;
        public static bool ItemsBotrk => Items["botrk"].Cast<CheckBox>().CurrentValue;
        public static int ItemsBotrkHealth => Items["botrkHealth"].Cast<Slider>().CurrentValue;
        public static bool ItemsYoumuu => Items["youmuu"].Cast<CheckBox>().CurrentValue;

        //Misc values
        public static bool SkinChanger => Misc["_skinChanger"].Cast<CheckBox>().CurrentValue;
        public static int SkinId => Misc["skinID"].Cast<ComboBox>().CurrentValue;
        public static int HitchanceChosen => Misc["_hitchance"].Cast<ComboBox>().CurrentValue;
        public static bool TearStacking => Misc["tearStacking"].Cast<KeyBind>().CurrentValue;
    }
}