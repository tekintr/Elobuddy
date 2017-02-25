using System;
using EloBuddy.SDK;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aka_s_Vayne.Manager
{
    class MenuManager
    {
        private static Menu VMenu,
            Hotkeymenu,
            Qsettings,
            ComboMenu,
            CondemnMenu,
            HarassMenu,
            FleeMenu,
            LaneClearMenu,
            JungleClearMenu,
            MiscMenu,
            DrawingMenu;

        public static void Load()
        {
            Mainmenu();
            Hotkeys();
            Combomenu();
            QSettings();
            Condemnmenu();
            Harassmenu();
            Fleemenu();
            LaneClearmenu();
            JungleClearmenu();
            Miscmenu();
            Drawingmenu();
        }

        private static void Mainmenu()
        {
            VMenu = MainMenu.AddMenu("Aka´s Vayne", "akavayne");
            VMenu.AddGroupLabel("Hosgeldiniz.Yapimci Aka! :)");
            VMenu.AddGroupLabel("Ceviri TekinTR v7.2");
        }

        private static void Hotkeys()
        {
            Hotkeymenu = VMenu.AddSubMenu("Hotkeys", "Hotkeys");
            Hotkeymenu.Add("flashe", new KeyBind("Sicra E!", false, KeyBind.BindTypes.HoldActive, 'Y'));
            Hotkeymenu.Add("insece", new KeyBind("Sicra Insec!", false, KeyBind.BindTypes.HoldActive, 'Z'));
            Hotkeymenu.Add("rote", new KeyBind("Zz'Rot Itme!", false, KeyBind.BindTypes.HoldActive, 'N'));
            Hotkeymenu.Add("insecmodes", new ComboBox("Insec Mode", 0, "Takima dogru", "Kuleye dogru", "Fare yonu"));
            Hotkeymenu.Add("RnoAA", new KeyBind("Gorunmez iken AA engelle", false, KeyBind.BindTypes.PressToggle, 'T'));
            Hotkeymenu.Add("RnoAAif", new Slider("Gorunmez iken AA engelle >= menzildeki dusman", 2, 0, 5));
        }

        private static void Combomenu()
        {
            ComboMenu = VMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.AddGroupLabel("Q Mode");
            ComboMenu.Add("Qmode", new ComboBox("Q Mode", 1, "Fare yonu", "Aka", "Akilli", "Kite", "Yan", "Asla"));
            ComboMenu.AddGroupLabel("W Settings");
            ComboMenu.Add("UseW", new CheckBox("Odaklan W", false));
            ComboMenu.AddGroupLabel("E Settings");
            ComboMenu.Add("UseE", new CheckBox("Kullan E"));
            ComboMenu.Add("UseEKill", new CheckBox("Kullan E olucek ise"));
            ComboMenu.AddGroupLabel("R Settings");
            ComboMenu.Add("UseRQ", new CheckBox("Oto Q", false));
            ComboMenu.Add("UseR", new CheckBox("Kullan R", false));
            ComboMenu.Add("UseRif", new Slider("Kullan R dusman sayisi", 2, 1, 5));
        }

        public static void QSettings()
        {
            Qsettings = VMenu.AddSubMenu("Q Settings", "Q Settings");
            Qsettings.AddGroupLabel("Mechanical Settings");
            Qsettings.AddLabel("In Burstmode Vayne will Tumble in Walls for a faster Reset!");
            Qsettings.Add("UseMirinQ", new CheckBox("Agresifmod"));
            Qsettings.Add("QReset", new CheckBox("Hizlica AA yenile"));
            Qsettings.Add("UseQE", new CheckBox("Dene Q -> E"));
            Qsettings.AddGroupLabel("Safety Settings");
            Qsettings.Add("UseQEvade", new CheckBox("Kacis entegrasyonu"));
            Qsettings.Add("UseSafeQ", new CheckBox("Guvenlik icin Q", false));
            Qsettings.Add("UseQEnemies", new CheckBox("Dusmanlara Q yapma", false));
            Qsettings.Add("UseQWall", new CheckBox("Duvarlara Q yapma", false));
            Qsettings.Add("UseQTraps", new CheckBox("Tuzaklara Q yapma"));
            Qsettings.Add("UseQTower", new CheckBox("Kuleye Q yapma"));
            Qsettings.Add("UseQSpam", new CheckBox("Tumunu yok say", false));
        }

        public static void Condemnmenu()
        {
            CondemnMenu = VMenu.AddSubMenu("Condemn", "Condemn");
            CondemnMenu.AddGroupLabel("Condemn");
            CondemnMenu.AddLabel("Best>Shine>Old>Marksman");
            CondemnMenu.Add("Condemnmode", new ComboBox("E modu", 0, "En iyi", "Eski", "Marksman", "Parlak"));
            CondemnMenu.Add("UseEAuto", new CheckBox("Oto E"));
            CondemnMenu.Add("UseETarget", new CheckBox("Sadece secili hedefi stunla?", false));    
            CondemnMenu.Add("UseEHitchance", new Slider("E isabet sansi", 33, 1, 100));
            CondemnMenu.Add("UseEPush", new Slider("E itme uzakligi", 420, 350, 470));
            CondemnMenu.Add("UseEAA", new Slider("Kullanma E hedef su kadar AA da x olucek ise", 0, 0, 4));
            CondemnMenu.Add("AutoTrinket", new CheckBox("Caliya totem kullan?"));
            CondemnMenu.Add("J4Flag", new CheckBox("E kullan J4 bayragina?"));
        }

        private static void Harassmenu()
        {
            HarassMenu = VMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Harass");
            HarassMenu.Add("HarassCombo", new CheckBox("Durtme kombosu"));
            HarassMenu.Add("HarassMana", new Slider("Durtme kombosu mana ayari", 40));
        }

        private static void LaneClearmenu()
        {
            LaneClearMenu = VMenu.AddSubMenu("LaneClear", "LaneClear");
            LaneClearMenu.AddGroupLabel("LaneClear");
            LaneClearMenu.Add("UseQ", new CheckBox("Kullan Q"));
            LaneClearMenu.Add("UseQMana", new Slider("En yuksek mana kullanimi ({0}%)", 40));
        }

        private static void JungleClearmenu()
        {
            JungleClearMenu = VMenu.AddSubMenu("JungleClear", "JungleClear");
            JungleClearMenu.AddGroupLabel("JungleClear");
            JungleClearMenu.Add("UseQ", new CheckBox("Kullan Q"));
            JungleClearMenu.Add("UseE", new CheckBox("Kullan E"));
        }

        private static void Fleemenu()
        {
            FleeMenu = VMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.AddGroupLabel("Flee");
            FleeMenu.Add("UseQ", new CheckBox("Kullan Q"));
            FleeMenu.Add("UseE", new CheckBox("Kullan E"));
        }

        private static void Miscmenu()
        {
            MiscMenu = VMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Misc");
            MiscMenu.Add("GapcloseQ", new CheckBox("Atilma yapana Q"));
            MiscMenu.Add("GapcloseE", new CheckBox("Atilma yapana E"));
            MiscMenu.Add("InterruptE", new CheckBox("Engellemek icin E"));
            MiscMenu.Add("TowerQ", new CheckBox("Kuleye Q"));
            MiscMenu.Add("LowLifeE", new CheckBox("Dusuk canda E", false));
            MiscMenu.Add("LowLifeES", new Slider("Canim dusukse E =>", 20));

        }

        private static void Drawingmenu()
        {
            DrawingMenu = VMenu.AddSubMenu("Drawings", "Drawings");
            DrawingMenu.AddGroupLabel("Ranges");
            DrawingMenu.Add("DrawQ", new CheckBox("Goster Q", false));
            DrawingMenu.Add("DrawE", new CheckBox("Goster E", false));
            DrawingMenu.Add("DrawOnlyReady", new CheckBox("Sadece hazir olanlari goster"));
            DrawingMenu.AddGroupLabel("Prediction");
            DrawingMenu.Add("DrawCondemn", new CheckBox("Goster E itme"));
            DrawingMenu.Add("DrawTumble", new CheckBox("Goster Takla"));
        }

        #region checkvalues
        #region checkvalues:hotkeys
        public static bool FlashE
        {
            get { return (Hotkeymenu["flashe"].Cast<KeyBind>().CurrentValue); }
        }
        public static bool InsecE
        {
            get { return (Hotkeymenu["insece"].Cast<KeyBind>().CurrentValue); }
        }
        public static bool RotE
        {
            get { return (Hotkeymenu["rote"].Cast<KeyBind>().CurrentValue); }
        }
        public static int InsecPositions
        {
            get { return (Hotkeymenu["insecmodes"].Cast<ComboBox>().CurrentValue); }
        }
        public static bool RNoAA
        {
            get { return (Hotkeymenu["RnoAA"].Cast<KeyBind>().CurrentValue); }
        }
        public static int RNoAASlider
        {
            get { return (Hotkeymenu["RnoAAif"].Cast<Slider>().CurrentValue); }
        }
        #endregion checkvalues:hotkeys
        #region checkvalues:qsettings
        public static bool Burstmode
        {
            get { return (Qsettings["UseMirinQ"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool QReset
        {
            get { return (Qsettings["QReset"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQE
        {
            get { return (Qsettings["UseQE"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQDynamic
        {
            get { return (Qsettings["UseSafeQ"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQEnemies
        {
            get { return (Qsettings["UseQEnemies"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQSpam
        {
            get { return (Qsettings["UseQSpam"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQEvade
        {
            get { return (Qsettings["UseQEvade"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQWall
        {
            get { return (Qsettings["UseQWall"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQTraps
        {
            get { return (Qsettings["UseQTraps"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQTower
        {
            get { return (Qsettings["UseQTower"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #region checkvalues:Combo
        public static int UseQMode
        {
            get { return (ComboMenu["Qmode"].Cast<ComboBox>().CurrentValue); }
        }

        public static bool FocusW
        {
            get { return (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool UseE
        {
            get { return (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool UseEKill
        {
            get { return (ComboMenu["UseEKill"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool UseR
        {
            get { return (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool UseRQ
        {
            get { return (ComboMenu["UseRQ"].Cast<CheckBox>().CurrentValue); }
        }

        public static int UseRSlider
        {
            get { return (ComboMenu["UseRif"].Cast<Slider>().CurrentValue); }
        }
        //Condemn
        #endregion checkvalues:Combo
        #region checkvalues:Condemn
        public static int CondemnMode
        {
            get { return (CondemnMenu["Condemnmode"].Cast<ComboBox>().CurrentValue); }
        }

        public static bool AutoE
        {
            get { return (CondemnMenu["UseEAuto"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool OnlyStunCurrentTarget
        {
            get { return (CondemnMenu["UseETarget"].Cast<CheckBox>().CurrentValue); }
        }

        public static int CondemnHitchance
        {
            get { return (CondemnMenu["UseEHitchance"].Cast<Slider>().CurrentValue); }
        }

        public static int CondemnPushDistance
        {
            get { return (CondemnMenu["UseEPush"].Cast<Slider>().CurrentValue); }
        }

        public static int CondemnBlock
        {
            get { return (CondemnMenu["UseEAA"].Cast<Slider>().CurrentValue); }
        }

        public static bool AutoTrinket
        {
            get { return (CondemnMenu["AutoTrinket"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool J4Flag
        {
            get { return (CondemnMenu["J4Flag"].Cast<CheckBox>().CurrentValue); }
        }

        #endregion checkvalues:Condemn
        #region checkvalues:Harass

        public static bool HarassCombo
        {
            get { return (HarassMenu["HarassCombo"].Cast<CheckBox>().CurrentValue); }
        }

        public static int HarassMana
        {
            get { return (HarassMenu["HarassMana"].Cast<Slider>().CurrentValue); }
        }


        #endregion checkvalues:Harass
        #region checkvalues:LC

        public static bool UseQLC
        {
            get { return (LaneClearMenu["UseQ"].Cast<CheckBox>().CurrentValue); }
        }

        public static int UseQLCMana
        {
            get { return (LaneClearMenu["UseQMana"].Cast<Slider>().CurrentValue); }
        }


        #endregion checkvalues:LC
        #region checkvalues:JC

        public static bool UseQJC
        {
            get { return (JungleClearMenu["UseQ"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool UseEJC
        {
            get { return (JungleClearMenu["UseE"].Cast<CheckBox>().CurrentValue); }
        }

        #endregion checkvalues:JC
        #region checkvalues:Flee
        public static bool UseQFlee
        {
            get { return (FleeMenu["UseQ"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool UseEFlee
        {
            get { return (FleeMenu["UseE"].Cast<CheckBox>().CurrentValue); }
        }

        #endregion checkvalues:Flee
        #region checkvalues:Misc

        public static bool GapcloseQ
        {
            get { return (MiscMenu["GapcloseQ"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool GapcloseE
        {
            get { return (MiscMenu["GapcloseE"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool InterruptE
        {
            get { return (MiscMenu["InterruptE"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool LowLifeE
        {
            get { return (MiscMenu["LowLifeE"].Cast<CheckBox>().CurrentValue); }
        }

        public static int LowLifeESlider
        {
            get { return (MiscMenu["LowLifeES"].Cast<Slider>().CurrentValue); }
        }

        public static bool TowerQ
        {
            get { return (MiscMenu["TowerQ"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion checkvalues:Misc
        #region checkvalues:Drawing
        public static bool DrawQ
        {
            get { return (DrawingMenu["DrawQ"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool DrawE
        {
            get { return (DrawingMenu["DrawE"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool DrawCondemn
        {
            get { return (DrawingMenu["DrawCondemn"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool DrawTumble
        {
            get { return (DrawingMenu["DrawTumble"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool DrawAutoPos
        {
            get { return (DrawingMenu["DrawAutoPos"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool DrawOnlyRdy
        {
            get { return (DrawingMenu["DrawOnlyReady"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion checkvalues:Drawing
        #endregion checkvalues
    }
}
