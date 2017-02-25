using System;
using EloBuddy.SDK;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aka_s_Yasuo.Manager
{
    class MenuManager
    {
        private static Menu VMenu,
                            Hotkeymenu,
                            ComboMenu,
                            HarassMenu,
                            LaneClearMenu,
                            FleeMenu,
                            MiscMenu,
                            DrawingMenu;

        public static void Load()
        {
            Mainmenu();
            Hotkeys();
            Combomenu();
            Harassmenu();
            Fleemenu();
            LaneClearmenu();
            Miscmenu();
            Drawingmenu();
        }

        private static void Mainmenu()
        {
            VMenu = MainMenu.AddMenu("Aka´s Yasuo", "akayas");
            VMenu.AddGroupLabel("Welcome to my Yasuo Addon have fun! :)");
            VMenu.AddGroupLabel("Made by Aka *-*");
        }

        private static void Hotkeys()
        {
            Hotkeymenu = VMenu.AddSubMenu("Hotkeys", "Hotkeys");
            Hotkeymenu.Add("autoq", new KeyBind("Otomatik Q", true, KeyBind.BindTypes.PressToggle, 'T'));
            Hotkeymenu.Add("eqflash", new KeyBind("AkaData Key", false, KeyBind.BindTypes.HoldActive, 'Y'));
        }

        private static void Combomenu()
        {
            ComboMenu = VMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("UseQ", new CheckBox("Kullan Q"));
            ComboMenu.Add("UseI", new CheckBox("Kullan Tutustur"));
            ComboMenu.AddGroupLabel("Smart Settings");
            ComboMenu.AddLabel("Note: This isnt Evade");
            ComboMenu.Add("SUseW", new CheckBox("Kullan W", false));
            ComboMenu.Add("SUseE", new CheckBox("Kullan E", false));
            ComboMenu.AddGroupLabel("E Settings");
            ComboMenu.Add("UseE", new CheckBox("Kullan E"));
            ComboMenu.Add("UseETower", new CheckBox("Kule alti", false));
            ComboMenu.Add("UseEStack", new CheckBox("E yuk kas", false));
            ComboMenu.AddGroupLabel("R Settings");
            ComboMenu.Add("UseR", new CheckBox("Kullan R"));
            ComboMenu.Add("UseRDelay", new CheckBox("Gecikmeli kullan"));
            ComboMenu.Add("UseRHP", new Slider("If Dusman Hp <", 60));
            ComboMenu.Add("UseRif", new Slider("If Dusman Havaya Kalksin >=", 2, 1, 5));
            ComboMenu.AddGroupLabel("AkaData");
            ComboMenu.Add("AkaDataC", new CheckBox("Komboda"));
            ComboMenu.Add("AkaDataMHp", new Slider("If Benim Hp <=", 10));
            ComboMenu.Add("AkaDataEHp", new Slider("If Dusman Hp <=", 20));
        }

        private static void Harassmenu()
        {
            HarassMenu = VMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Harass");
            HarassMenu.Add("UseQ", new CheckBox("Kullan Q"));
            HarassMenu.Add("UseQ3", new CheckBox("Kullan Q3"));
            HarassMenu.Add("UseQLH", new CheckBox("Kullan Q Son vurus"));
            HarassMenu.AddGroupLabel("Auto Harass");
            HarassMenu.Add("UseQAuto", new CheckBox("Ototmatik Q"));
            HarassMenu.Add("UseQ3Auto", new CheckBox("Otomatik Q3"));
        }

        private static void LaneClearmenu()
        {
            LaneClearMenu = VMenu.AddSubMenu("Clear", "clear");
            LaneClearMenu.AddGroupLabel("Q Settings");
            LaneClearMenu.Add("UseQ", new CheckBox("Kullan Q"));
            LaneClearMenu.Add("UseQ3", new CheckBox("Kullan Q3"));
            LaneClearMenu.AddGroupLabel("E Settings");
            LaneClearMenu.Add("UseE", new CheckBox("Kullan E"));
            LaneClearMenu.Add("UseELH", new CheckBox("Sadece Son vurus", false));
            LaneClearMenu.Add("UseETower", new CheckBox("Kule alti", false));
        }

        private static void Fleemenu()
        {
            FleeMenu = VMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.Add("UseE", new CheckBox("Kullan E"));
            FleeMenu.Add("UseEStack", new CheckBox("E yuk kas"));
        }

        private static void Miscmenu()
        {
            MiscMenu = VMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Misc");
            MiscMenu.Add("KSQ", new CheckBox("Q ile oldur"));
            MiscMenu.Add("KSE", new CheckBox("E ile oldur"));
            MiscMenu.Add("KSR", new CheckBox("R ile oldur"));
        }

        private static void Drawingmenu()
        {
            DrawingMenu = VMenu.AddSubMenu("Drawings", "Drawings");
            DrawingMenu.Add("DrawQ", new CheckBox("Goster Q"));
            DrawingMenu.Add("DrawE", new CheckBox("Goster E", false));
            DrawingMenu.Add("DrawR", new CheckBox("Goster R", false));
            DrawingMenu.Add("DrawOnlyReady", new CheckBox("Sadece Hazir olan sklleri goster"));
            DrawingMenu.Add("DrawStatus", new CheckBox("Goster oto yuk kasma durumu"));
        }

        #region checkvalues
        #region checkvalues:hotkeys
        public static bool AutoStackQ
        {
            get { return (Hotkeymenu["autoq"].Cast<KeyBind>().CurrentValue); }
        }
        public static bool FlashEQ
        {
            get { return (Hotkeymenu["eqflash"].Cast<KeyBind>().CurrentValue); }
        }
        #endregion
        #region checkvalues:combo
        public static bool UseQC
        {
            get { return (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseEC
        {
            get { return (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseECTower
        {
            get { return (ComboMenu["UseETower"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseECStack
        {
            get { return (ComboMenu["UseEStack"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseRC
        {
            get { return (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseRCDelay
        {
            get { return (ComboMenu["UseRDelay"].Cast<CheckBox>().CurrentValue); }
        }
        public static int UseRCHP
        {
            get { return (ComboMenu["UseRHP"].Cast<Slider>().CurrentValue); }
        }
        public static int UseRCEnemies
        {
            get { return (ComboMenu["UseRif"].Cast<Slider>().CurrentValue); }
        }
        public static bool UseIgnite
        {
            get { return (ComboMenu["UseI"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SmartW
        {
            get { return (ComboMenu["SUseW"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SmartE
        {
            get { return (ComboMenu["SUseE"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool AkaData
        {
            get { return (ComboMenu["AkaDataC"].Cast<CheckBox>().CurrentValue); }
        }
        public static int AkaDataEnemy
        {
            get { return (ComboMenu["AkaDataEHp"].Cast<Slider>().CurrentValue); }
        }
        public static int AkaDatamy
        {
            get { return (ComboMenu["AkaDataMHp"].Cast<Slider>().CurrentValue); }
        }
        #endregion
        #region checkvalues:harass
        public static bool UseQH
        {
            get { return (HarassMenu["UseQ"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQ3H
        {
            get { return (HarassMenu["UseQ3"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQHLH
        {
            get { return (HarassMenu["UseQLH"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQHAuto
        {
            get { return (HarassMenu["UseQAuto"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQ3HAuto
        {
            get { return (HarassMenu["UseQ3Auto"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #region checkvalues:laneclear
        public static bool UseQLC
        {
            get { return (LaneClearMenu["UseQ"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseQ3LC
        {
            get { return (LaneClearMenu["UseQ3"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseELC
        {
            get { return (LaneClearMenu["UseE"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseELCLH
        {
            get { return (LaneClearMenu["UseELH"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseELCTower
        {
            get { return (LaneClearMenu["UseETower"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #region checkvalues:flee
        public static bool UseEFlee
        {
            get { return (FleeMenu["UseE"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool UseEStackFlee
        {
            get { return (FleeMenu["UseEStack"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #region checkvalues:misc
        public static bool KSQ
        {
            get { return (MiscMenu["KSQ"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool KSE
        {
            get { return (MiscMenu["KSE"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool KSR
        {
            get { return (MiscMenu["KSR"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool Skinhack
        {
            get { return (MiscMenu["Skinhack"].Cast<CheckBox>().CurrentValue); }
        }
        public static int SkinID
        {
            get { return (MiscMenu["SkinId"].Cast<ComboBox>().CurrentValue); }
        }
        public static bool Autolvl
        {
            get { return (MiscMenu["Autolvl"].Cast<CheckBox>().CurrentValue); }
        }
        public static int AutolvlSlider
        {
            get { return (MiscMenu["AutolvlS"].Cast<ComboBox>().CurrentValue); }
        }
        #endregion
        #region checkvalues:draw
        public static bool DrawQ
        {
            get { return (DrawingMenu["DrawQ"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool DrawE
        {
            get { return (DrawingMenu["DrawE"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool DrawR
        {
            get { return (DrawingMenu["DrawR"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool DrawOnlyRdy
        {
            get { return (DrawingMenu["DrawOnlyReady"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool DrawStatus
        {
            get { return (DrawingMenu["DrawStatus"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #endregion
    }
}
