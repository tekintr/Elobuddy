using System;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace VodkaJanna
{
    public static class Config
    {
        private const string MenuName = "VodkaJanna";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Hosgeldiniz VodkaJanna");
            Menu.AddLabel("Yapimci by Haker");
            Menu.AddLabel("Turkce Ceviri TekinTR.");
            ModesMenu.Initialize();
            PredictionMenu.Initialize();
            var shielderSubMenu = Config.Menu.AddSubMenu("Shielder");
            Shielder.Shielder.Initialize(shielderSubMenu);
            ManaManagerMenu.Initialize();
            MiscMenu.Initialize();
            DrawingMenu.Initialize();
            DebugMenu.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class ModesMenu
        {
            private static readonly Menu MenuModes;

            static ModesMenu()
            {
                MenuModes = Config.Menu.AddSubMenu("Modes");

                Combo.Initialize();
                MenuModes.AddSeparator();

                Harass.Initialize();
                MenuModes.AddSeparator();

                LaneClear.Initialize();
                MenuModes.AddSeparator();

                JungleClear.Initialize();
                MenuModes.AddSeparator();

                LastHit.Initialize();
                MenuModes.AddSeparator();

                Flee.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                static Combo()
                {
                    MenuModes.AddGroupLabel("Combo");
                    _useQ = MenuModes.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("comboUseW", new CheckBox("Kullan W"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                static Harass()
                {
                    MenuModes.AddGroupLabel("Harass");
                    _useQ = MenuModes.Add("harassUseQ", new CheckBox("Kullan Q", false));
                    _useW = MenuModes.Add("harassUseW", new CheckBox("Kullan W"));
                }

                public static void Initialize()
                {
                }
            }

            public static class LaneClear
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly Slider _minQTargets;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static int MinQTargets
                {
                    get { return _minQTargets.CurrentValue; }
                }

                static LaneClear()
                {
                    MenuModes.AddGroupLabel("LaneClear");
                    _useQ = MenuModes.Add("laneUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("laneUseW", new CheckBox("Kullan W", false));
                    _useE = MenuModes.Add("laneUseE", new CheckBox("Kullan E (Kendine)"));
                    _minQTargets = MenuModes.Add("minQTargetsLC", new Slider("Hedef sayisi Q", 4, 1, 10));
                }

                public static void Initialize()
                {
                }
            }

            public static class LastHit
            {
                private static readonly CheckBox _useW;

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }


                static LastHit()
                {
                    MenuModes.AddGroupLabel("LastHit");
                    _useW = MenuModes.Add("lasthitUseW", new CheckBox("Kullan W", false));
                }

                public static void Initialize()
                {
                }
            }

            public static class JungleClear
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly Slider _minQTargets;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static int MinQTargets
                {
                    get { return _minQTargets.CurrentValue; }
                }

                static JungleClear()
                {
                    MenuModes.AddGroupLabel("JungleClear");
                    _useQ = MenuModes.Add("jungleUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("jungleUseW", new CheckBox("Kullan W"));
                    _useE = MenuModes.Add("jungleUseE", new CheckBox("Kullan E (Kendine)"));
                    _minQTargets = MenuModes.Add("minQTargetsJC", new Slider("Hedef sayisi Q", 2, 1, 10));
                }

                public static void Initialize()
                {
                }


            }

            public static class Flee
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                static Flee()
                {
                    MenuModes.AddGroupLabel("Flee");
                    _useQ = MenuModes.Add("fleeUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("fleeUseW", new CheckBox("Kullan W"));
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class MiscMenu
        {
            private static readonly Menu MenuMisc;
            private static readonly CheckBox _interrupterQ;
            private static readonly CheckBox _interrupterR;
            private static readonly CheckBox _antigapcloserQ;
            private static readonly CheckBox _antigapcloserR;
            private static readonly CheckBox _ksQ;
            private static readonly CheckBox _ksW;
            private static readonly CheckBox _ksIgnite;
            private static readonly CheckBox _autoR;
            private static readonly CheckBox _potion;
            private static readonly Slider _potionMinHP;
            private static readonly Slider _potionMinMP;
            private static readonly Slider _autoRMinHP;
            private static readonly Slider _autoRMinEnemies;

            public static bool InterrupterUseQ
            {
                get { return _interrupterQ.CurrentValue; }
            }
            public static bool InterrupterUseR
            {
                get { return _interrupterR.CurrentValue; }
            }
            public static bool AntigapcloserUseQ
            {
                get { return _antigapcloserQ.CurrentValue; }
            }
            public static bool AntigapcloserUseR
            {
                get { return _antigapcloserR.CurrentValue; }
            }
            public static bool KsQ
            {
                get { return _ksQ.CurrentValue; }
            }
            public static bool KsW
            {
                get { return _ksW.CurrentValue; }
            }
            public static bool KsIgnite
            {
                get { return _ksIgnite.CurrentValue; }
            }
            public static bool AutoR
            {
                get { return _autoR.CurrentValue; }
            }
            public static int AutoRMinHP
            {
                get { return _autoRMinHP.CurrentValue; }
            }
            public static int AutoRMinEnemies
            {
                get { return _autoRMinEnemies.CurrentValue; }
            }
            public static bool Potion
            {
                get { return _potion.CurrentValue; }
            }
            public static int potionMinHP
            {
                get { return _potionMinHP.CurrentValue; }
            }
            public static int potionMinMP
            {
                get { return _potionMinMP.CurrentValue; }
            }

            static MiscMenu()
            {
                MenuMisc = Config.Menu.AddSubMenu("Misc");
                MenuMisc.AddGroupLabel("Interrupter");
                _interrupterQ = MenuMisc.Add("interrupterUseQ", new CheckBox("Kullan Q buyuleri kesmek icin"));
                _interrupterR = MenuMisc.Add("interrupterUseR", new CheckBox("Kullan R buyuleri kesmek icin", false));
                MenuMisc.AddGroupLabel("Anti Gapcloser");
                _antigapcloserQ = MenuMisc.Add("antigapcloserUseQ", new CheckBox("Kullan Q atilma yapanlara"));
                _antigapcloserR = MenuMisc.Add("antigapcloserUseR", new CheckBox("Kullan R atilma yapanlara", false));
                MenuMisc.AddGroupLabel("KillSteal");
                _ksQ = MenuMisc.Add("ksQ", new CheckBox("Q ile oldur"));
                _ksW = MenuMisc.Add("ksW", new CheckBox("W ile oldur"));
                _ksIgnite = MenuMisc.Add("ksIgnite", new CheckBox("Tutustur ile oldur", false));
                MenuMisc.AddGroupLabel("Auto R usage");
                _autoR = MenuMisc.Add("autoR", new CheckBox("Arkadasin cani az ise otomatik R kullan", false));
                _autoRMinHP = MenuMisc.Add("autoRMinHP", new Slider("Arkada cani az ise HP % R kullan", 15));
                _autoRMinEnemies = MenuMisc.Add("autoRMinEnemies", new Slider("R menzilinde kac dusman olsun", 1, 0, 5));
                MenuMisc.AddGroupLabel("Auto pot usage");
                _potion = MenuMisc.Add("potion", new CheckBox("Kullan iksir"));
                _potionMinHP = MenuMisc.Add("potionminHP", new Slider("Can % iksir kullanmak icin", 30));
                _potionMinMP = MenuMisc.Add("potionMinMP", new Slider("Mana % iksir kullanmak icin", 20));
            }

            public static void Initialize()
            {
            }
        }

        public static class ManaManagerMenu
        {
            private static readonly Menu MenuManaManager;
            private static readonly Slider _minQMana;
            private static readonly Slider _minWMana;
            private static readonly Slider _minEMana;

            public static int MinQMana
            {
                get { return _minQMana.CurrentValue; }
            }
            public static int MinWMana
            {
                get { return _minWMana.CurrentValue; }
            }
            public static int MinEMana
            {
                get { return _minEMana.CurrentValue; }
            }

            static ManaManagerMenu()
            {
                MenuManaManager = Config.Menu.AddSubMenu("Mana Manager");
                _minQMana = MenuManaManager.Add("minQMana", new Slider("Enaz Mana % Q kullanmak icin", 20, 0, 100));
                _minWMana = MenuManaManager.Add("minWMana", new Slider("Enaz Mana % W kullanmak icin", 20, 0, 100));
                _minEMana = MenuManaManager.Add("minEMana", new Slider("Enaz Mana % E kullanmak icin", 60, 0, 100));
            }

            public static void Initialize()
            {
            }
        }

        public static class DrawingMenu
        {
            private static readonly Menu MenuDrawing;
            private static readonly CheckBox _drawQ;
            private static readonly CheckBox _drawQMax;
            private static readonly CheckBox _drawW;
            private static readonly CheckBox _drawE;
            private static readonly CheckBox _drawR;
            private static readonly CheckBox _drawOnlyReady;

            public static bool DrawQ
            {
                get { return _drawQ.CurrentValue; }
            }
            public static bool DrawQMax
            {
                get { return _drawQMax.CurrentValue; }
            }
            public static bool DrawW
            {
                get { return _drawW.CurrentValue; }
            }
            public static bool DrawE
            {
                get { return _drawE.CurrentValue; }
            }
            public static bool DrawR
            {
                get { return _drawR.CurrentValue; }
            }
            public static bool DrawOnlyReady
            {
                get { return _drawOnlyReady.CurrentValue; }
            }

            static DrawingMenu()
            {
                MenuDrawing = Config.Menu.AddSubMenu("Drawing");
                _drawQ = MenuDrawing.Add("drawQ", new CheckBox("Goster Q"));
                _drawQMax = MenuDrawing.Add("drawQMax", new CheckBox("Goster Max Q Menzili"));
                _drawW = MenuDrawing.Add("drawW", new CheckBox("Goster W"));
                _drawE = MenuDrawing.Add("drawE", new CheckBox("Goster E"));
                _drawR = MenuDrawing.Add("drawR", new CheckBox("Goster R"));
                _drawOnlyReady = MenuDrawing.Add("drawOnlyReady", new CheckBox("Sadece hazir olani goster"));
            }

            public static void Initialize()
            {
            }
        }

        public static class DebugMenu
        {
            private static readonly Menu MenuDebug;
            private static readonly CheckBox _debugChat;
            private static readonly CheckBox _debugConsole;

            public static bool DebugChat
            {
                get { return _debugChat.CurrentValue; }
            }
            public static bool DebugConsole
            {
                get { return _debugConsole.CurrentValue; }
            }

            static DebugMenu()
            {
                MenuDebug = Config.Menu.AddSubMenu("Debug");
                MenuDebug.AddLabel("Bu sadece hata ayiklama amaclidir.");
                _debugChat = MenuDebug.Add("debugChat", new CheckBox("Sohbete hata mesajlarini gonder", false));
                _debugConsole = MenuDebug.Add("debugConsole", new CheckBox("Konsola hata mesajlarini gonder", false));
            }

            public static void Initialize()
            {

            }
        }

        public static class PredictionMenu
        {
            private static readonly Menu MenuPrediction;
            private static readonly Slider _minQHCCombo;
            private static readonly Slider _minQHCHarass;
            private static readonly Slider _minQHCKillSteal;
            private static readonly Slider _minQHCFlee;

            public static HitChance MinQHCCombo
            {
                get { return Util.GetHCSliderHitChance(_minQHCCombo); }
            }

            public static HitChance MinQHCHarass
            {
                get { return Util.GetHCSliderHitChance(_minQHCHarass); }
            }

            public static HitChance MinQHCKillSteal
            {
                get { return Util.GetHCSliderHitChance(_minQHCKillSteal); }
            }

            public static HitChance MinQHCFlee
            {
                get { return Util.GetHCSliderHitChance(_minQHCFlee); }
            }
            
            static PredictionMenu()
            {
                MenuPrediction = Config.Menu.AddSubMenu("Prediction");
                MenuPrediction.AddLabel("Burada,atma becerileri icin enaz vurma sansini kontrol edebilirsin.");
                MenuPrediction.AddGroupLabel("Q Prediction");
                MenuPrediction.AddGroupLabel("Combo");
                _minQHCCombo = Util.CreateHCSlider("comboMinQHitChance", "Combo", HitChance.High, MenuPrediction);
                MenuPrediction.AddGroupLabel("Harass");
                _minQHCHarass = Util.CreateHCSlider("harassMinQHitChance", "Harass", HitChance.High, MenuPrediction);
                MenuPrediction.AddGroupLabel("Kill Steal");
                _minQHCKillSteal = Util.CreateHCSlider("killStealMinQHitChance", "Kill Steal", HitChance.Medium, MenuPrediction);
                MenuPrediction.AddGroupLabel("Flee");
                _minQHCFlee = Util.CreateHCSlider("fleeMinQHitChance", "Flee", HitChance.Low, MenuPrediction);
            }

            public static void Initialize()
            {

            }
        }
    }
}
