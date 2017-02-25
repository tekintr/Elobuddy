using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Lucian_The_Troll
{
    internal static class LucianTheTrollMenu
    {
        private static Menu _myMenu;
        public static Menu ComboMenu, DrawMeNu, HarassMeNu, Activator, FarmMeNu, MiscMeNu, FleeMenu;


        public static void LoadMenu()
        {
            MylucianTrollPage();
            ComboMenuPage();
            FarmMeNuPage();
            HarassMeNuPage();
            ActivatorPage();
            MiscMeNuPage();
            FleeMenuPage();
            DrawMeNuPage();
        }

        private static void MylucianTrollPage()
        {
            _myMenu = MainMenu.AddMenu("Lucian The Troll", "main");
            _myMenu.AddLabel(" Lucian The Troll " + Program.Version);
            _myMenu.AddLabel(" Yapýmcý MeLoSenpai");
        }

        private static void ComboMenuPage()
        {
            ComboMenu = _myMenu.AddSubMenu("Combo settings", "Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("ComboLogic", new ComboBox("KomboTürü", 0, "Akýcý", "Hýzlý", "AA-E-AA-Q-AA-W-AA-E-AA"));
            //  ComboMenu.Add("useQcombo", new CheckBox("Use Q"));
            //   ComboMenu.AddLabel("W Settings:");
            //   ComboMenu.Add("useWcombo", new CheckBox("Use W"));
            //   ComboMenu.Add("useWnopred", new CheckBox("Use No prediction W", false));
            ComboMenu.AddLabel("E Settings:");
            ComboMenu.Add("useEstartcombo", new CheckBox("Kullan E"));
            ComboMenu.Add("ELogic", new ComboBox("E Türü ", 0, "Yana", "FareYönü", "Otomatik"));
         //   ComboMenu.Add("combo.E.undertower", new Slider("E enemy under tower only if their health % under", 40));
            ComboMenu.AddLabel("R Settings");
            ComboMenu.Add("UseRcomboHp", new CheckBox("Kullan R"));
            ComboMenu.Add("Hp", new Slider("Kullan R düþmanýn caný %", 45, 0, 100));
            ComboMenu.Add("combo.REnemies", new Slider("En az düþman R için", 1, 1, 5));
            ComboMenu.Add("ForceR",
                new KeyBind("Force R On Target Selector", false, KeyBind.BindTypes.HoldActive, "T".ToCharArray()[0]));
        }


        private static void FarmMeNuPage()
        {
            FarmMeNu = _myMenu.AddSubMenu("Farm Settings", "FarmSettings");
            FarmMeNu.AddGroupLabel("Lane Clear Settings:");
            FarmMeNu.Add("useQFarm", new CheckBox("Kullan Q"));
            FarmMeNu.Add("useWFarm", new CheckBox("Kullan W"));
            FarmMeNu.Add("useEFarm", new CheckBox("Kullan E"));
            FarmMeNu.Add("useManalane", new Slider("Koridor temizlemek için gereken mana %", 70, 0, 100));
            FarmMeNu.AddLabel("Jungle Clear Setting :");
            FarmMeNu.Add("useQJungle", new CheckBox("Kullan Q"));
            FarmMeNu.Add("useWJungle", new CheckBox("Kullan W"));
            FarmMeNu.Add("useEJungle", new CheckBox("Kullan E"));
            FarmMeNu.Add("useManaJungle", new Slider("Orman temizlemek için gereken mana %", 70, 0, 100));
        }

        private static void HarassMeNuPage()
        {
            HarassMeNu = _myMenu.AddSubMenu("Harass/Killsteal Settings", "hksettings");
            HarassMeNu.AddGroupLabel("Harass Settings:");
            HarassMeNu.Add("useQHarass", new CheckBox("Kullan Q normal - uzun - Test"));
            HarassMeNu.Add("useWHarass", new CheckBox("Kullan W"));
            HarassMeNu.Add("useWHarassMana", new Slider("Dürtmek icin gerek mana %", 70, 0, 100));
            HarassMeNu.AddLabel("AutoHarass");
            HarassMeNu.Add("autoQHarass", new CheckBox("Otomatik Q uzun dürtme", false));
            HarassMeNu.Add("autoQHarassMana", new Slider("Oto dürtmeye gereken mana %", 70, 0, 100));
            HarassMeNu.AddLabel("Ks settings:");
            HarassMeNu.Add("UseQks", new CheckBox("Kullan Q ks"));
            HarassMeNu.Add("UseWks", new CheckBox("Kullan W ks"));
            HarassMeNu.Add("UseRks", new CheckBox("Kullan R ks"));
            HarassMeNu.Add("UseRksRange", new Slider("En fazla R menzili[KS]", 1000, 500, 1400));
        }

        private static void ActivatorPage()
        {
            Activator = _myMenu.AddSubMenu("Activator Settings", "Items");
            Activator.AddGroupLabel("Auto QSS if :");
            Activator.Add("Blind",
                new CheckBox("Kör", false));
            Activator.Add("Charm",
                new CheckBox("Cazibe"));
            Activator.Add("Fear",
                new CheckBox("Korku"));
            Activator.Add("Polymorph",
                new CheckBox("Polymorph"));
            Activator.Add("Stan",
                new CheckBox("Stun"));
            Activator.Add("Snare",
                new CheckBox("Kapan"));
            Activator.Add("Silence",
                new CheckBox("Susturma", false));
            Activator.Add("Taunt",
                new CheckBox("Taunt"));
            Activator.Add("Suppression",
                new CheckBox("Engellenme"));
            Activator.Add("delay", new Slider("Gecikme kullan", 100, 0, 500));
            Activator.AddGroupLabel("Items usage:");
            Activator.AddSeparator();
            Activator.Add("bilgewater",
                new CheckBox("Kullan Bilgewater Palasý"));
            Activator.Add("bilgewater.HP",
                new Slider("Eger hp daha düþük ise Bilgewater palasý kullan {0}(%)", 60));
            Activator.AddSeparator();
            Activator.Add("botrk",
                new CheckBox("Kullan Mahvolmus"));
            Activator.Add("botrk.HP",
                new Slider("Mahvolmus kullan hp daha düþükse {0}(%)", 60));
            Activator.AddSeparator();
            Activator.Add("youmus",
                new CheckBox("Kullan Youmu"));
            Activator.Add("items.Youmuss.HP",
                new Slider("Youmuss kullan hp daha düþükse {0}(%)", 60, 1));
            Activator.Add("youmus.Enemies",
                new Slider("Youmuu var olduðunda kullan {0} Menzilde düþmanlar", 3, 1, 5));
            Activator.AddSeparator();
            Activator.AddGroupLabel("Potion Settings");
            Activator.Add("spells.Potions.Check",
                new CheckBox("Kullan Ýksir"));
            Activator.Add("spells.Potions.HP",
                new Slider("HP daha düþük olduðunda Ýksir kullanýn {0}(%)", 60, 1));
            Activator.Add("spells.Potions.Mana",
                new Slider("Mana daha düþük olduðunda Ýksir kullanýn {0}(%)", 60, 1));
            Activator.AddSeparator();
            Activator.AddGroupLabel("Spells settings:");
            Activator.AddGroupLabel("Heal settings:");
            Activator.Add("spells.Heal.Hp",
                new Slider("HP daha düþük olduðunda Sifa kullanýn {0}(%)", 30, 1));
            Activator.AddGroupLabel("Ignite settings:");
            Activator.Add("spells.Ignite.Focus",
                new Slider("HP hedefi daha düþük olduðunda Tutuþtur kullanýn. {0}(%)", 10, 1));
        }
        private static void FleeMenuPage()
        {
            FleeMenu = _myMenu.AddSubMenu("Flee Settings", "FleeSettings");
            FleeMenu.AddGroupLabel("Flee Settings");
            FleeMenu.Add("FleeE", new CheckBox("Kullan E"));
            FleeMenu.Add("FleeW", new CheckBox("Kullan W"));
        }
     


        private static void MiscMeNuPage()
        {
            MiscMeNu = _myMenu.AddSubMenu("Misc Menu", "othermenu");
            MiscMeNu.AddGroupLabel("Skin settings");
            MiscMeNu.Add("checkSkin",
                new CheckBox("Kullan Skin Secici:", false));
            MiscMeNu.Add("skin.Id",
                new Slider("Skin Editor", 5, 0, 10));
        }

        private static void DrawMeNuPage()
        {
            DrawMeNu = _myMenu.AddSubMenu("Draw  settings", "Draw");
            DrawMeNu.AddGroupLabel("Draw Settings:");
            DrawMeNu.Add("nodraw",
                new CheckBox("Cizimleri kapat", false));
            DrawMeNu.AddSeparator();
            DrawMeNu.Add("draw.Q",
                new CheckBox("Göster Q"));
            DrawMeNu.Add("draw.Q1",
                new CheckBox("Göster Q Extend"));
            DrawMeNu.Add("draw.W",
                new CheckBox("Göster W"));
            DrawMeNu.Add("draw.E",
                new CheckBox("Göster E"));
            DrawMeNu.Add("draw.R",
                new CheckBox("Göster R"));
            DrawMeNu.AddLabel("Damage indicators");
            DrawMeNu.Add("healthbar", new CheckBox("Saglik cubugu kaplamasi"));
            DrawMeNu.Add("percent", new CheckBox("Hasar yüzdesi bilgi"));
        }


        public static
            bool Nodraw()
        {
            return DrawMeNu["nodraw"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsQ()
        {
            return DrawMeNu["draw.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsQ1()
        {
            return DrawMeNu["draw.Q1"].Cast<CheckBox>().CurrentValue;
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
        
        public static bool ComboEStart()
        {
            return ComboMenu["useEstartcombo"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboQ()
        {
            return ComboMenu["useQcombo"].Cast<CheckBox>().CurrentValue;
        }
        public static bool Eauto()
        {
            return ComboMenu["ELogic"].Cast<ComboBox>().CurrentValue == 2;
        }
        public static bool Eside()
        {
            return ComboMenu["ELogic"].Cast<ComboBox>().CurrentValue == 0;
        }
        public static float Eundertower()
        {
            return ComboMenu["combo.E.undertower"].Cast<Slider>().CurrentValue;
        }
        public static bool Ecursor()
        {
            return ComboMenu["ELogic"].Cast<ComboBox>().CurrentValue == 1;
        }

        public static bool Smooth()
        {
            return ComboMenu["ComboLogic"].Cast<ComboBox>().CurrentValue == 0;
        }

        public static bool AArange()
        {
            return ComboMenu["ComboLogic"].Cast<ComboBox>().CurrentValue == 2;
        }

        public static bool Fast()
        {
            return ComboMenu["ComboLogic"].Cast<ComboBox>().CurrentValue == 1;
        }

        public static bool ComboWNopred()
        {
            return ComboMenu["useWnopred"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboW()
        {
            return ComboMenu["useWcombo"].Cast<CheckBox>().CurrentValue;
        }


        public static float Hpslider()
        {
            return ComboMenu["Hp"].Cast<Slider>().CurrentValue;
        }

        public static bool ComboRhp()
        {
            return ComboMenu["UseRcomboHp"].Cast<CheckBox>().CurrentValue;
        }

        public static float MinenemyR()
        {
            return ComboMenu["combo.REnemies"].Cast<Slider>().CurrentValue;
        }

        public static bool ForceR()
        {
            return ComboMenu["ForceR"].Cast<KeyBind>().CurrentValue;
        }

        public static bool LaneQ()
        {
            return FarmMeNu["useQFarm"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LaneW()
        {
            return FarmMeNu["useWFarm"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LaneE()
        {
            return FarmMeNu["useEFarm"].Cast<CheckBox>().CurrentValue;
        }

        public static float LaneMana()
        {
            return FarmMeNu["useManalane"].Cast<Slider>().CurrentValue;
        }

        public static float Junglemana()
        {
            return FarmMeNu["useManaJungle"].Cast<Slider>().CurrentValue;
        }

        public static bool JungleQ()
        {
            return FarmMeNu["useQJungle"].Cast<CheckBox>().CurrentValue;
        }

        public static bool JungleE()
        {
            return FarmMeNu["useEJungle"].Cast<CheckBox>().CurrentValue;
        }

        public static bool JungleW()
        {
            return FarmMeNu["useWJungle"].Cast<CheckBox>().CurrentValue;
        }

        public static bool HarassQ()
        {
            return HarassMeNu["useQHarass"].Cast<CheckBox>().CurrentValue;
        }

        public static bool HarassW()
        {
            return HarassMeNu["useWHarass"].Cast<CheckBox>().CurrentValue;
        }

        public static float HarassMana()
        {
            return HarassMeNu["useWHarassMana"].Cast<Slider>().CurrentValue;
        }

        public static bool AutoQHarass()
        {
            return HarassMeNu["autoQHarass"].Cast<CheckBox>().CurrentValue;
        }

        public static float AutoHarassMana()
        {
            return HarassMeNu["autoQHarassMana"].Cast<Slider>().CurrentValue;
        }

        public static bool KillstealQ()
        {
            return HarassMeNu["UseQks"].Cast<CheckBox>().CurrentValue;
        }

        public static bool KillstealW()
        {
            return HarassMeNu["UseWks"].Cast<CheckBox>().CurrentValue;
        }

        public static bool KillstealR()
        {
            return HarassMeNu["UseRks"].Cast<CheckBox>().CurrentValue;
        }

        public static float KsRangeR()
        {
            return HarassMeNu["UseRksRange"].Cast<Slider>().CurrentValue;
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

        public static float SpellsIgniteFocus()
        {
            return Activator["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
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

        public static bool SkinChanger()
        {
            return MiscMeNu["SkinChanger"].Cast<CheckBox>().CurrentValue;
        }
        public static bool Fleee()
        {
            return FleeMenu["FleeE"].Cast<CheckBox>().CurrentValue;
        }
        public static bool Fleew()
        {
            return FleeMenu["FleeW"].Cast<CheckBox>().CurrentValue;
        }

        public static bool CheckSkin()
        {
            return MiscMeNu["checkSkin"].Cast<CheckBox>().CurrentValue;
        }
    }
}