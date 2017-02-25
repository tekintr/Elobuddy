using System;
using EloBuddy.SDK;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Carry_Vayne.Manager
{
    class MenuManager
    {
        private static Menu VMenu,
            Hotkeymenu,
    ComboMenu,
    CondemnMenu,
    HarassMenu,
    LaneClearMenu,
    JungleClearMenu,
    FleeMenu,
    MiscMenu,
    DrawingMenu;

        public static void Load()
        {
            Mainmenu();
            Hotkeys();
            PackageLoader();
            if (Variables.Combo == true) Combomenu();
            if (Variables.Condemn == true) Condemnmenu();
            if (Variables.Harass == true) Harassmenu();
            if (Variables.LC == true) LaneClearmenu();
            if (Variables.JC == true) JungleClearmenu();
            if (Variables.Flee == true) Fleemenu();
            if (Variables.Misc == true) Miscmenu();
            if (Variables.Draw == true) Drawingmenu();
        }

        private static void Mainmenu()
        {
            VMenu = MainMenu.AddMenu("Auto Carry Vayne", "akavayne");
            VMenu.AddGroupLabel("Made by Aka.");
            VMenu.AddSeparator();
        }

        private static void PackageLoader()
        {
            VMenu.AddGroupLabel("PackageLoader");
            VMenu.AddLabel("'Let me modify the Options myself plz'");
            VMenu.AddLabel("-Press F5 after ticking!-");
            VMenu.Add("Combo", new CheckBox("Daha fazla kombo ayari", false));
            VMenu.Add("Condemn", new CheckBox("Daha fazla Condemn ayari", false));
            VMenu.Add("Harass", new CheckBox("Daha fazla Durtme Ayari", false));
            VMenu.Add("Flee", new CheckBox("Daha fazla Kacis ayari", false));
            VMenu.Add("LC", new CheckBox("Daha fazla lanetemizleme ayari", false));
            VMenu.Add("JC", new CheckBox("Daha fazla Ormantemizleme ayari", false));
            VMenu.Add("Misc", new CheckBox("Daha fazla Ek Ayar", false));
            VMenu.Add("Drawing", new CheckBox("Daha fazla Gosterge Ayari", false));
            #region CheckMenu

            //Combo
            if (VMenu["Combo"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Combo = true;
            }
            //Condemn
            if (VMenu["Condemn"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Condemn = true;
            }
            //Harass
            if (VMenu["Harass"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Harass = true;
            }
            //Flee
            if (VMenu["Flee"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Flee = true;
            }
            //LC
            if (VMenu["LC"].Cast<CheckBox>().CurrentValue)
            {
                Variables.LC = true;
            }
            //JC
            if (VMenu["JC"].Cast<CheckBox>().CurrentValue)
            {
                Variables.JC = true;
            }
            //Misc
            if (VMenu["Misc"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Misc = true;
            }
            //Drawing
            if (VMenu["Drawing"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Draw = true;
            }

            #endregion CheckMenu
        }

        private static void Hotkeys()
        {
            Hotkeymenu = VMenu.AddSubMenu("Hotkeys", "Hotkeys");
            Hotkeymenu.Add("flashe", new KeyBind("Flash Condemn!", false, KeyBind.BindTypes.HoldActive, 'Y'));
            Hotkeymenu.Add("insece", new KeyBind("Flash Insec!", false, KeyBind.BindTypes.HoldActive, 'Z'));
            Hotkeymenu.Add("rote", new KeyBind("Zz'Rot Condemn!", false, KeyBind.BindTypes.HoldActive, 'N'));
            Hotkeymenu.Add("insecmodes", new ComboBox("Insec Mode", 0, "To Allys", "To Tower", "To Mouse"));
            Hotkeymenu.Add("RnoAA", new KeyBind("No AA while Stealth", false, KeyBind.BindTypes.PressToggle, 'T'));
            Hotkeymenu.Add("RnoAAif", new Slider("No AA stealth when >= enemy in range", 2, 0, 5));
        }

        private static void Combomenu()
        {
            ComboMenu = VMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.Add("UseQ", new CheckBox("Kullan Q"));
            ComboMenu.Add("UseQStacks", new CheckBox("Q için 2W yuk", false));
            ComboMenu.Add("UseQMode", new ComboBox("Q Mode", 0, "Auto", "Mouse"));
            ComboMenu.Add("UseW", new CheckBox("W odaklan", false));
            ComboMenu.Add("UseE", new CheckBox("Kullan E"));
            ComboMenu.Add("UseEKill", new CheckBox("Olecekse hedefe E?"));
            ComboMenu.Add("UseR", new CheckBox("Kullan R", false));
            ComboMenu.Add("UseRif", new Slider("Kullan R if", 2, 1, 5));

        }

        private static void Condemnmenu()
        {
            CondemnMenu = VMenu.AddSubMenu("Condemn", "Condemn");
            CondemnMenu.Add("UseEAuto", new CheckBox("Otomatik E"));
            CondemnMenu.Add("UseETarget", new CheckBox("Sadece sabitleyecekse?", false));
            CondemnMenu.Add("UseEHitchance", new Slider("Condemn isabet oranı", 2, 1, 4));
            CondemnMenu.Add("UseEPush", new Slider("Condemn(E) itme mesafesi(atma)", 420, 350, 470));
            CondemnMenu.Add("UseEAA", new Slider("Eger hedef AA ile olecekse E kullanma", 0, 0, 4));
            CondemnMenu.Add("AutoTrinket", new CheckBox("Ota totem at?"));
            CondemnMenu.Add("J4Flag", new CheckBox("Jarvanı it(ulti atarken veya ultisi varsa)?"));
        }

        private static void Harassmenu()
        {
            HarassMenu = VMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.Add("HarassCombo", new CheckBox("Durtme kombosu"));
            HarassMenu.Add("HarassMana", new Slider("Harass Combo Mana", 40));
        }

        private static void LaneClearmenu()
        {
            LaneClearMenu = VMenu.AddSubMenu("LaneClear", "LaneClear");
            LaneClearMenu.Add("UseQ", new CheckBox("Kullan Q"));
            LaneClearMenu.Add("UseQMana", new Slider("Maximum mana usage in percent ({0}%)", 40));
        }

        private static void JungleClearmenu()
        {
            JungleClearMenu = VMenu.AddSubMenu("JungleClear", "JungleClear");
            JungleClearMenu.Add("UseQ", new CheckBox("Kullan Q"));
            JungleClearMenu.Add("UseE", new CheckBox("Kullan E"));
        }

        private static void Fleemenu()
        {
            FleeMenu = VMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.Add("UseQ", new CheckBox("Kullan Q"));
            FleeMenu.Add("UseE", new CheckBox("Kullan E"));
        }

        private static void Miscmenu()
        {
            MiscMenu = VMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Misc");
            MiscMenu.Add("GapcloseQ", new CheckBox("Gapclose Q"));
            MiscMenu.Add("GapcloseE", new CheckBox("Gapclose E"));
            MiscMenu.Add("InterruptE", new CheckBox("Interrupt E"));
            MiscMenu.Add("LowLifeE", new CheckBox("Low Life E", false));
            MiscMenu.Add("LowLifeES", new Slider("Low Life E if =>", 20));
        }

        private static void Drawingmenu()
        {
            DrawingMenu = VMenu.AddSubMenu("Drawings", "Drawings");
            DrawingMenu.Add("DrawQ", new CheckBox("Goster Q", false));
            DrawingMenu.Add("DrawE", new CheckBox("Goster E", false));
            DrawingMenu.Add("DrawOnlyReady", new CheckBox("Goster sadece buyuler hazirsa"));
            DrawingMenu.AddGroupLabel("Prediction");
            DrawingMenu.Add("DrawCondemn", new CheckBox("Goster Condemn"));
            DrawingMenu.Add("DrawTumble", new CheckBox("Goster Tumble"));
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
        #region checkvalues:Combo
        public static bool UseQ
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int UseQMode
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseQMode"].Cast<ComboBox>().CurrentValue : 1); }
        }

        public static bool UseQStacks
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseQStacks"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static bool FocusW
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseW"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static bool UseE
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseE"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool UseEKill
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseEKill"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool UseR
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseR"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static int UseRSlider
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseRif"].Cast<Slider>().CurrentValue : 2); }
        }
        //Condemn
        #endregion checkvalues:Combo
        #region checkvalues:Condemn

        public static bool AutoE
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["UseEAuto"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool OnlyStunCurrentTarget
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["UseETarget"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static int CondemnHitchance
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["UseEHitchance"].Cast<Slider>().CurrentValue : 2); }
        }

        public static int CondemnPushDistance
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["UseEPush"].Cast<Slider>().CurrentValue : 420); }
        }

        public static int CondemnBlock
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["UseEAA"].Cast<Slider>().CurrentValue : 1); }
        }

        public static bool AutoTrinket
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["AutoTrinket"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool J4Flag
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["J4Flag"].Cast<CheckBox>().CurrentValue : true); }
        }

        #endregion checkvalues:Condemn
        #region checkvalues:Harass

        public static bool HarassCombo
        {
            get { return (VMenu["Harass"].Cast<CheckBox>().CurrentValue ? HarassMenu["HarassCombo"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int HarassMana
        {
            get { return (VMenu["Harass"].Cast<CheckBox>().CurrentValue ? HarassMenu["HarassMana"].Cast<Slider>().CurrentValue : 0); }
        }


        #endregion checkvalues:Harass
        #region checkvalues:LC

        public static bool UseQLC
        {
            get { return (VMenu["LC"].Cast<CheckBox>().CurrentValue ? LaneClearMenu["UseQ"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int UseQLCMana
        {
            get { return (VMenu["LC"].Cast<CheckBox>().CurrentValue ? LaneClearMenu["UseQMana"].Cast<Slider>().CurrentValue : 40); }
        }


        #endregion checkvalues:LC
        #region checkvalues:JC

        public static bool UseQJC
        {
            get { return (VMenu["JC"].Cast<CheckBox>().CurrentValue ? JungleClearMenu["UseQ"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool UseEJC
        {
            get { return (VMenu["JC"].Cast<CheckBox>().CurrentValue ? JungleClearMenu["UseE"].Cast<CheckBox>().CurrentValue : true); }
        }

        #endregion checkvalues:JC
        #region checkvalues:Flee
        public static bool UseQFlee
        {
            get { return (VMenu["Flee"].Cast<CheckBox>().CurrentValue ? FleeMenu["UseQ"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool UseEFlee
        {
            get { return (VMenu["Flee"].Cast<CheckBox>().CurrentValue ? FleeMenu["UseE"].Cast<CheckBox>().CurrentValue : true); }
        }

        #endregion checkvalues:Flee
        #region checkvalues:Misc

        public static bool GapcloseQ
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["GapcloseQ"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool GapcloseE
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["GapcloseE"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool InterruptE
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["InterruptE"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool LowLifeE
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["LowLifeE"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static int LowLifeESlider
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["LowLifeES"].Cast<Slider>().CurrentValue : 20); }
        }
        #endregion checkvalues:Misc

        #region checkvalues:Drawing
        public static bool DrawQ
        {
            get { return (VMenu["Drawing"].Cast<CheckBox>().CurrentValue ? DrawingMenu["DrawQ"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static bool DrawE
        {
            get { return (VMenu["Drawing"].Cast<CheckBox>().CurrentValue ? DrawingMenu["DrawE"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static bool DrawCondemn
        {
            get { return (VMenu["Drawing"].Cast<CheckBox>().CurrentValue ? DrawingMenu["DrawCondemn"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool DrawTumble
        {
            get { return (VMenu["Drawing"].Cast<CheckBox>().CurrentValue ? DrawingMenu["DrawTumble"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool DrawOnlyRdy
        {
            get { return (VMenu["Drawing"].Cast<CheckBox>().CurrentValue ? DrawingMenu["DrawOnlyReady"].Cast<CheckBox>().CurrentValue : false); }
        }
        #endregion checkvalues:Drawing
        #endregion checkvalues
    }
}
