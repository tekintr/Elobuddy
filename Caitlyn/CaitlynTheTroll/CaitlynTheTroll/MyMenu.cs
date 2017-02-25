using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace CaitlynTheTroll
{
    internal static class CaitlynTheTrollMeNu
    {
        private static Menu _myMenu;
        public static Menu ComboMenu, DrawMeNu, HarassMeNu, Activator, FarmMeNu, MiscMeNu, PredictionMenu;

        public static void LoadMenu()
        {
            MyCaitlynTheTrollPage();
            ComboMenuPage();
            PredictionMenuPage();
            FarmMeNuPage();
            HarassMeNuPage();
            ActivatorPage();
            MiscMeNuPage();
            DrawMeNuPage();
        }

        private static void MyCaitlynTheTrollPage()
        {
            _myMenu = MainMenu.AddMenu("Caitlyn The Troll", "main");
            _myMenu.AddLabel(" Caitlyn The Troll " + Program.Version);
            _myMenu.AddLabel(" Yapimci MeLoSenpai");
        }

        private static void DrawMeNuPage()
        {
            DrawMeNu = _myMenu.AddSubMenu("Draw  settings", "Draw");
            DrawMeNu.AddGroupLabel("Draw Settings:");
            DrawMeNu.Add("nodraw",
                new CheckBox("No Display Drawing", false));
            DrawMeNu.AddSeparator();
            DrawMeNu.Add("draw.Q",
                new CheckBox("Goster Q"));
            DrawMeNu.Add("draw.W",
                new CheckBox("Goster W"));
            DrawMeNu.Add("draw.E",
                new CheckBox("Goster E"));
            DrawMeNu.Add("draw.R",
                new CheckBox("Goster R"));
            DrawMeNu.AddLabel("Damage indicators");
            DrawMeNu.Add("healthbar", new CheckBox("Saglik cubugu"));
            DrawMeNu.Add("percent", new CheckBox("Verilicek hasari goster"));
        }

        private static void ComboMenuPage()
        {
            ComboMenu = _myMenu.AddSubMenu("Combo settings", "Combo");
            ComboMenu.AddGroupLabel("Combo settings:");
            ComboMenu.AddGroupLabel("Q settings:");
            ComboMenu.Add("combo.Q", new CheckBox("Kullan Q"));
            ComboMenu.Add("Qlogic", new ComboBox("Q Logic ", 0, "Normal", "After AA"));
            ComboMenu.AddLabel("E settings:");
            ComboMenu.Add("combo.E", new CheckBox("Kullan E"));
            ComboMenu.Add("Elogic", new ComboBox("E Logic ", 0, "Normal", "After AA"));
            ComboMenu.AddLabel("W settings:");
            ComboMenu.Add("combo.w",
                new CheckBox("Use W"));
            ComboMenu.Add("limittrap", new Slider("Kapan siniri ", 2, 1, 5));
            ComboMenu.AddLabel("R settings:");
            ComboMenu.Add("combo.R",
                new CheckBox("Akilli R kullan"));
            ComboMenu.AddLabel("Use Auto Skills On CC Target:");
            ComboMenu.Add("combo.CCQ", new CheckBox("Kullan Q CC"));
            ComboMenu.Add("combo.CC",
                new CheckBox("Kullan E CC"));
            ComboMenu.Add("combo.CCW",
                new CheckBox("Kullan W CC"));
            ComboMenu.AddLabel("Combo preferences:");
            ComboMenu.Add("comboEQbind",
                new KeyBind("Use E > Q > AA", false, KeyBind.BindTypes.HoldActive, 'Z'));
        }

        private static void PredictionMenuPage()
        {
            PredictionMenu = _myMenu.AddSubMenu("Prediction settings", "Prediction");
            PredictionMenu.AddGroupLabel("Prediction settings:");
            PredictionMenu.Add("Qpred", new Slider("Sec Q {0}(%) isabet sansi", 85));
            PredictionMenu.Add("Epred", new Slider("Sec E {0}(%) isabet sansi", 70));
            PredictionMenu.AddLabel("Work Only For Normal Q/E Logic");
            PredictionMenu.AddLabel(
                "Higher % ->Higher chance of spell landing on target but takes more time to get casted");
            PredictionMenu.AddLabel(
                "Lower % ->Faster casting but lower chance that the spell will land on your target. ");
        }

        private static void FarmMeNuPage()
        {
            FarmMeNu = _myMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            FarmMeNu.AddGroupLabel("Lane clear settings:");
            FarmMeNu.Add("Lane.Q",
                new CheckBox("Kullan Q"));
            FarmMeNu.Add("LaneMana",
                new Slider("En az gereken mana {0}(%)", 60));
            FarmMeNu.AddSeparator();
            FarmMeNu.AddLabel("Jungle Settings");
            FarmMeNu.Add("jungle.Q",
                new CheckBox("Kullan Q"));
        }

        private static void HarassMeNuPage()
        {
            HarassMeNu = _myMenu.AddSubMenu("Harass/Killsteal Settings", "hksettings");
            HarassMeNu.AddGroupLabel("Harass Settings:");
            HarassMeNu.AddSeparator();
            HarassMeNu.AddLabel("Use Q on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                HarassMeNu.Add("Harass.Q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            HarassMeNu.AddSeparator();
            HarassMeNu.Add("UseWharass", new CheckBox("Kullan W"));
            HarassMeNu.Add("harass.QE",
                new Slider("En az gereken mana {0}(%)", 55));
            HarassMeNu.AddSeparator();
            HarassMeNu.AddLabel("KillSteal Settings:");
            HarassMeNu.Add("killsteal.Q",
                new CheckBox("Kullan Q", false));
        }

        private static void ActivatorPage()
        {
            Activator = _myMenu.AddSubMenu("Activator Settings", "Items");
            Activator.AddGroupLabel("Auto QSS if :");
            Activator.Add("Blind",
                new CheckBox("Kor olunca", false));
            Activator.Add("Charm",
                new CheckBox("Cazibe"));
            Activator.Add("Fear",
                new CheckBox("Korku"));
            Activator.Add("Polymorph",
                new CheckBox("Polymorph"));
            Activator.Add("Stun",
                new CheckBox("Sabitlenme"));
            Activator.Add("Snare",
                new CheckBox("Kapan"));
            Activator.Add("Silence",
                new CheckBox("Susturma", false));
            Activator.Add("Taunt",
                new CheckBox("Taunt"));
            Activator.Add("Suppression",
                new CheckBox("Engellenme"));
            Activator.AddLabel("Items usage:");
            Activator.Add("bilgewater",
                new CheckBox("Kullan Bilgewater Palasi"));
            Activator.Add("bilgewater.HP",
                new Slider("Kullan Bilgewater Palasi canim az ise {0}(%)", 60));
            Activator.AddSeparator();
            Activator.Add("botrk",
                new CheckBox("Mahvolmus kullan"));
            Activator.Add("botrk.HP",
                new Slider("Mahvolmus icin gereken can {0}(%)", 60));
            Activator.AddSeparator();
            Activator.Add("youmus",
                new CheckBox("Kullan Youmus"));
            Activator.Add("items.Youmuss.HP",
                new Slider("Kullan Youmus icin gereken can {0}(%)", 60, 1));
            Activator.Add("youmus.Enemies",
                new Slider("Kullan Youmus yakindaki {0} dusman sayisi", 3, 1, 5));
            Activator.AddLabel("Potion Settings");
            Activator.Add("spells.Potions.Check",
                new CheckBox("Iksir kullan"));
            Activator.Add("spells.Potions.HP",
                new Slider("Canim sundan az ise iksir kullan {0}(%)", 60, 1));
            Activator.Add("spells.Potions.Mana",
                new Slider("Mana sundan az ise iksir kullan {0}(%)", 60, 1));
            Activator.AddLabel("Spells settings:");
            Activator.AddLabel("Heal settings:");
            Activator.Add("spells.Heal.Hp",
                new Slider("Canim sundan az ise sifa kullan {0}(%)", 30, 1));
            Activator.AddLabel("Ignite settings:");
            Activator.Add("spells.Ignite.Focus",
                new Slider("Hedef cani sundan az ise tutustur birak {0}(%)", 10, 1));
        }

        private static void MiscMeNuPage()
        {
            MiscMeNu = _myMenu.AddSubMenu("Misc Menu", "othermenu");
            MiscMeNu.AddGroupLabel("Settings for Flee");
            MiscMeNu.Add("useEmouse",
                new KeyBind("Use E To Mouse", false, KeyBind.BindTypes.HoldActive, "T".ToCharArray()[0]));
            MiscMeNu.AddLabel("Anti Gap Closer/Interrupt");
            MiscMeNu.Add("gapcloser.E",
                new CheckBox("Kullan E Atilma yapana"));
            MiscMeNu.Add("gapcloser.W",
                new CheckBox("Kullan W Atilma yapana"));
            MiscMeNu.Add("interupt.W",
                new CheckBox("Kullan W Interrupt"));
            MiscMeNu.AddLabel("Skin settings");
            MiscMeNu.Add("checkSkin",
                new CheckBox("Kullan skin secici:", false));
            MiscMeNu.Add("skin.Id",
                new Slider("Skin secici", 5, 0, 10));
        }

        public static bool Nodraw()
        {
            return DrawMeNu["nodraw"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsQ()
        {
            return DrawMeNu["draw.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsW()
        {
            return DrawMeNu["draw.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsE()
        {
            return DrawMeNu["draw.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsR()
        {
            return DrawMeNu["draw.R"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsT()
        {
            return DrawMeNu["draw.T"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboW()
        {
            return ComboMenu["combo.W"].Cast<CheckBox>().CurrentValue;
        }

        public static float ComboREnemies()
        {
            return ComboMenu["combo.REnemies"].Cast<Slider>().CurrentValue;
        }

        public static bool ComboR()
        {
            return ComboMenu["combo.R"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboQ()
        {
            return ComboMenu["combo.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboE()
        {
            return ComboMenu["combo.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboEq()
        {
            return ComboMenu["comboEQbind"].Cast<KeyBind>().CurrentValue;
        }

        public static bool Aaq()
        {
            return ComboMenu["Qlogic"].Cast<ComboBox>().CurrentValue == 1;
        }

        public static float PredE()
        {
            return PredictionMenu["Epred"].Cast<Slider>().CurrentValue;
        }

        public static float LimitTrap()
        {
            return ComboMenu["limittrap"].Cast<Slider>().CurrentValue;
        }

        public static float PredQ()
        {
            return PredictionMenu["Qpred"].Cast<Slider>().CurrentValue;
        }

        public static bool LogicQ()
        {
            return ComboMenu["Qlogic"].Cast<ComboBox>().CurrentValue == 0;
        }

        public static bool AaE()
        {
            return ComboMenu["Elogic"].Cast<ComboBox>().CurrentValue == 1;
        }

        public static bool LogicE()
        {
            return ComboMenu["Elogic"].Cast<ComboBox>().CurrentValue == 0;
        }

        public static bool LaneQ()
        {
            return FarmMeNu["lane.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static float LaneMana()
        {
            return FarmMeNu["LaneMana"].Cast<Slider>().CurrentValue;
        }

        public static bool JungleQ()
        {
            return FarmMeNu["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool HarassQ()
        {
            return HarassMeNu["harass.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool HarassW()
        {
            return HarassMeNu["UseWharass"].Cast<CheckBox>().CurrentValue;
        }

        public static float HarassQe()
        {
            return HarassMeNu["harass.QE"].Cast<Slider>().CurrentValue;
        }

        public static bool KillstealQ()
        {
            return HarassMeNu["killsteal.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Bilgewater()
        {
            return Activator["bilgewater"].Cast<CheckBox>().CurrentValue;
        }

        public static float BilgewaterHp()
        {
            return Activator["bilgewater.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool Botrk()
        {
            return Activator["botrk"].Cast<CheckBox>().CurrentValue;
        }

        public static float BotrkHp()
        {
            return Activator["botrk.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool Youmus()
        {
            return Activator["youmus"].Cast<CheckBox>().CurrentValue;
        }

        public static float YoumusEnemies()
        {
            return Activator["youmus.Enemies"].Cast<Slider>().CurrentValue;
        }

        public static float ItemsYoumuShp()
        {
            return Activator["items.Youmuss.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool SpellsPotionsCheck()
        {
            return Activator["spells.Potions.Check"].Cast<CheckBox>().CurrentValue;
        }

        public static float SpellsPotionsHp()
        {
            return Activator["spells.Potions.HP"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsPotionsM()
        {
            return Activator["spells.Potions.Mana"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsHealHp()
        {
            return Activator["spells.Heal.HP"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsIgniteFocus()
        {
            return Activator["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsBarrierHp()
        {
            return Activator["spells.Barrier.Hp"].Cast<Slider>().CurrentValue;
        }

        public static bool Blind()
        {
            return Activator["Blind"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Charm()
        {
            return Activator["Charm"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Fear()
        {
            return Activator["Fear"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Polymorph()
        {
            return Activator["Polymorph"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Stun()
        {
            return Activator["Stun"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Snare()
        {
            return Activator["Snare"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Silence()
        {
            return Activator["Silence"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Taunt()
        {
            return Activator["Taunt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Suppression()
        {
            return Activator["Suppression"].Cast<CheckBox>().CurrentValue;
        }

        public static int SkinId()
        {
            return MiscMeNu["skin.Id"].Cast<Slider>().CurrentValue;
        }

        public static bool GapcloserE()
        {
            return MiscMeNu["gapcloser.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool GapcloserW()
        {
            return MiscMeNu["gapcloser.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool InterupteW()
        {
            return MiscMeNu["interupt.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool SkinChanger()
        {
            return MiscMeNu["SkinChanger"].Cast<CheckBox>().CurrentValue;
        }

        public static bool CheckSkin()
        {
            return MiscMeNu["checkSkin"].Cast<CheckBox>().CurrentValue;
        }

        public static bool UseEmouse()
        {
            return MiscMeNu["useEmouse"].Cast<KeyBind>().CurrentValue;
        }
    }
}