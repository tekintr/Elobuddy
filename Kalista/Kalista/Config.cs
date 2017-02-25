using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Hellsing.Kalista
{
    public static class Config
    {
        private const string MenuName = "Kalista";
        public static Menu Menu { get; private set; }

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, "kalistaMenu");
            Menu.AddGroupLabel("Introduction");
            Menu.AddLabel("Hosgeldiniz!");

            // All modes
            Modes.Initialize();

            // Misc
            Misc.Initialize();

            // Items
            Items.Initialize();

            // Drawing
            Drawing.Initialize();

            // Specials
            Specials.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            private static Menu Menu { get; set; }

            static Modes()
            {
                // Initialize modes menu
                Menu = Config.Menu.AddSubMenu("Modes", "modes");

                // Combo
                Combo.Initialize();

                // Harass
                Menu.AddSeparator();
                Harass.Initialize();

                // WaveClear
                Menu.AddSeparator();
                LaneClear.Initialize();

                // JungleClear
                Menu.AddSeparator();
                JungleClear.Initialize();

                // Flee
                Menu.AddSeparator();
                Flee.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useQAA;
                private static readonly CheckBox _useAA;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useEslow;
                private static readonly Slider _numE;
                private static readonly Slider _mana;

                private static readonly CheckBox _useItems;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseQAA
                {
                    get { return _useQAA.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }
                public static int MinNumberE
                {
                    get { return _numE.CurrentValue; }
                }
                public static bool UseAA
                {
                    get { return _useAA.CurrentValue; }
                }
                public static bool UseESlow
                {
                    get { return _useEslow.CurrentValue; }
                }
                public static bool UseItems
                {
                    get { return _useItems.CurrentValue; }
                }
                public static int ManaQ
                {
                    get { return _mana.CurrentValue; }
                }

                static Combo()
                {
                    Menu.AddGroupLabel("Combo");

                    _useQ = Menu.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useQAA = Menu.Add("comboUseQAA", new CheckBox("Kullan Q sadece oto ataktan sonra"));
                    _useE = Menu.Add("comboUseE", new CheckBox("Kullan E"));
                    _useEslow = Menu.Add("comboUseEslow", new CheckBox("Düsman yavaslicaksa E ile minyon kes"));
                    _useAA = Menu.Add("comboUseAA", new CheckBox("Minyona atak yap korunmak icin"));
                    _useItems = Menu.Add("comboUseItems", new CheckBox("Item kullan"));
                    _numE = Menu.Add("comboNumE", new Slider("En az kac yuk kullanmak icin E", 5, 1, 50));
                    _mana = Menu.Add("comboMana", new Slider("Yuzde Q icin Mana kullanımı ({0}%)", 30));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly Slider _mana;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static int MinMana
                {
                    get { return _mana.CurrentValue; }
                }

                static Harass()
                {
                    Menu.AddGroupLabel("Harass");

                    _useQ = Menu.Add("harassUseQ", new CheckBox("Kullan Q"));
                    _mana = Menu.Add("harassMana", new Slider("En az mana in %", 30));
                }

                public static void Initialize()
                {
                }
            }

            public static class LaneClear
            {
                private static readonly CheckBox _useQ;
                private static readonly Slider _numQ;
                private static readonly CheckBox _useE;
                private static readonly Slider _numE;
                private static readonly Slider _mana;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static int MinNumberQ
                {
                    get { return _numQ.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }
                public static int MinNumberE
                {
                    get { return _numE.CurrentValue; }
                }
                public static int MinMana
                {
                    get { return _mana.CurrentValue; }
                }

                static LaneClear()
                {
                    Menu.AddGroupLabel("LaneClear");

                    _useQ = Menu.Add("laneUseQ", new CheckBox("Kullan Q"));
                    _useE = Menu.Add("laneUseE", new CheckBox("Kullan E"));
                    _numQ = Menu.Add("laneNumQ", new Slider("Olucek minyon sayısı Q", 3, 1, 10));
                    _numE = Menu.Add("laneNumE", new Slider("Olucek minyon sayısı E", 2, 1, 10));
                    Menu.AddSeparator();
                    _mana = Menu.Add("laneMana", new Slider("En az mana in %", 30));
                }

                public static void Initialize()
                {
                }
            }

            public static class JungleClear
            {
                private static readonly CheckBox _useE;

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                static JungleClear()
                {
                    Menu.AddGroupLabel("JungleClear");

                    _useE = Menu.Add("jungleUseE", new CheckBox("Kullan E"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Flee
            {
                private static readonly CheckBox _walljump;
                private static readonly CheckBox _autoAttack;

                public static bool UseWallJumps
                {
                    get { return _walljump.CurrentValue; }
                }
                public static bool UseAutoAttacks
                {
                    get { return _autoAttack.CurrentValue; }
                }

                static Flee()
                {
                    Menu.AddGroupLabel("Flee");

                    _walljump = Menu.Add("fleeWalljump", new CheckBox("Kullan WallJumps"));
                    _autoAttack = Menu.Add("fleeAutoattack", new CheckBox("Kullan Otomatik atak"));
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class Misc
        {
            private static Menu Menu { get; set; }

            private static readonly CheckBox _killsteal;
            private static readonly CheckBox _bigE;
            private static readonly CheckBox _saveSoulbound;
            private static readonly CheckBox _secureE;
            private static readonly CheckBox _harassPlus;
            private static readonly Slider _autoBelowHealthE;
            private static readonly Slider _reductionE;

            public static bool UseKillsteal
            {
                get { return _killsteal.CurrentValue; }
            }
            public static bool UseEBig
            {
                get { return _bigE.CurrentValue; }
            }
            public static bool SaveSouldBound
            {
                get { return _saveSoulbound.CurrentValue; }
            }
            public static bool SecureMinionKillsE
            {
                get { return _secureE.CurrentValue; }
            }
            public static bool UseHarassPlus
            {
                get { return _harassPlus.CurrentValue; }
            }
            public static int AutoEBelowHealth
            {
                get { return _autoBelowHealthE.CurrentValue; }
            }
            public static int DamageReductionE
            {
                get { return _reductionE.CurrentValue; }
            }

            static Misc()
            {
                Menu = Config.Menu.AddSubMenu("Misc");

                Menu.AddGroupLabel("Misc features");
                _killsteal = Menu.Add("killsteal", new CheckBox("E ile oldur"));
                _bigE = Menu.Add("bigE", new CheckBox("Buyuk minyona surekli E kullan"));
                _saveSoulbound = Menu.Add("saveSoulbound", new CheckBox("Bagli kisiyi kurtarmak icin R kullan"));
                _secureE = Menu.Add("secureE", new CheckBox("Olmeyecek minyon icin E kullan"));
                _harassPlus = Menu.Add("harassPlus", new CheckBox("Bir minyon olucekse ve dusmanlarin 1+ yigini varsa Oto E"));
                _autoBelowHealthE = Menu.Add("autoBelowHealthE", new Slider("Sagligimiz altindayken otomatik E ({0}%) yuzde", 10));
                _reductionE = Menu.Add("reductionE", new Slider("E hasarini {0} puan azalt", 20));

                // Initialize other misc features
                Sentinel.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Sentinel
            {
                private static readonly CheckBox _enabled;
                private static readonly CheckBox _noMode;
                private static readonly CheckBox _alert;
                private static readonly Slider _mana;

                private static readonly CheckBox _baron;
                private static readonly CheckBox _dragon;
                private static readonly CheckBox _mid;
                private static readonly CheckBox _blue;
                private static readonly CheckBox _red;

                public static bool Enabled
                {
                    get { return _enabled.CurrentValue; }
                }
                public static bool NoModeOnly
                {
                    get { return _noMode.CurrentValue; }
                }
                public static bool Alert
                {
                    get { return _alert.CurrentValue; }
                }
                public static int Mana
                {
                    get { return _mana.CurrentValue; }
                }

                public static bool SendBaron
                {
                    get { return _baron.CurrentValue; }
                }
                public static bool SendDragon
                {
                    get { return _dragon.CurrentValue; }
                }
                public static bool SendMid
                {
                    get { return _mid.CurrentValue; }
                }
                public static bool SendBlue
                {
                    get { return _blue.CurrentValue; }
                }
                public static bool SendRed
                {
                    get { return _red.CurrentValue; }
                }

                static Sentinel()
                {
                    Menu.AddGroupLabel("Sentinel (W) usage");

                    if (Game.MapId != GameMapId.SummonersRift)
                    {
                        Menu.AddLabel("Only on Summoners Rift, sorry.");
                    }
                    else
                    {
                        _enabled = Menu.Add("enabled", new CheckBox("Etkin"));
                        _noMode = Menu.Add("noMode", new CheckBox("Yalnizca etkin mod yokken kullanin"));
                        _alert = Menu.Add("alert", new CheckBox("Muhafizlar hasar aldiginda uyari yap"));
                        _mana = Menu.Add("mana", new Slider("Kullanmak icin gereken mana W ({0}%)", 40));

                        Menu.AddLabel("Send to the following locations (no specific order):");
                        (_baron = Menu.Add("baron", new CheckBox("Baron (W bugu kullan)"))).OnValueChange += OnValueChange;
                        (_dragon = Menu.Add("dragon", new CheckBox("Dragon (W bugu kullan)"))).OnValueChange += OnValueChange;
                        (_mid = Menu.Add("mid", new CheckBox("Mid lane brush"))).OnValueChange += OnValueChange;
                        (_blue = Menu.Add("blue", new CheckBox("Mavi buff"))).OnValueChange += OnValueChange;
                        (_red = Menu.Add("red", new CheckBox("Kirmisi buff"))).OnValueChange += OnValueChange;
                        SentinelManager.RecalculateOpenLocations();
                    }
                }

                private static void OnValueChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                {
                    SentinelManager.RecalculateOpenLocations();
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class Items
        {
            private static Menu Menu { get; set; }

            private static readonly CheckBox _cutlass;
            private static readonly CheckBox _botrk;
            private static readonly CheckBox _ghostblade;

            public static bool UseCutlass
            {
                get { return _cutlass.CurrentValue; }
            }
            public static bool UseBotrk
            {
                get { return _botrk.CurrentValue; }
            }
            public static bool UseGhostblade
            {
                get { return _ghostblade.CurrentValue; }
            }

            static Items()
            {
                Menu = Config.Menu.AddSubMenu("Items");

                _cutlass = Menu.Add("cutlass", new CheckBox("Kullan Bilgewater Palası"));
                _botrk = Menu.Add("botrk", new CheckBox("Mahvolmus kullan"));
                _ghostblade = Menu.Add("ghostblade", new CheckBox("Youmu kullan"));
            }

            public static void Initialize()
            {
            }
        }

        public static class Drawing
        {
            private static Menu Menu { get; set; }

            private static readonly CheckBox _drawQ;
            private static readonly CheckBox _drawW;
            private static readonly CheckBox _drawE;
            private static readonly CheckBox _drawEleaving;
            private static readonly CheckBox _drawR;

            private static readonly CheckBox _healthbar;
            private static readonly CheckBox _percent;

            public static bool DrawQ
            {
                get { return _drawQ.CurrentValue; }
            }
            public static bool DrawW
            {
                get { return _drawW.CurrentValue; }
            }
            public static bool DrawE
            {
                get { return _drawE.CurrentValue; }
            }
            public static bool DrawELeaving
            {
                get { return _drawEleaving.CurrentValue; }
            }
            public static bool DrawR
            {
                get { return _drawR.CurrentValue; }
            }
            public static bool IndicatorHealthbar
            {
                get { return _healthbar.CurrentValue; }
            }
            public static bool IndicatorPercent
            {
                get { return _percent.CurrentValue; }
            }

            static Drawing()
            {
                Menu = Config.Menu.AddSubMenu("Drawing");

                Menu.AddGroupLabel("Spell ranges");
                _drawQ = Menu.Add("drawQ", new CheckBox("Goster Q Menzili"));
                _drawW = Menu.Add("drawW", new CheckBox("Goster W Menzili"));
                _drawE = Menu.Add("drawE", new CheckBox("Goster E Menzili"));
                _drawEleaving = Menu.Add("drawEleaving", new CheckBox("E tetikleme mesafesi (kombo gor)", false));
                _drawR = Menu.Add("drawR", new CheckBox("Goster R Menzili", false));

                Menu.AddGroupLabel("Damage indicators (Rend - E)");
                _healthbar = Menu.Add("healthbar", new CheckBox("Saglik cubugu kaplamasi"));
                _percent = Menu.Add("percent", new CheckBox("Hasar yuzdesi bilgi"));
            }

            public static void Initialize()
            {
            }
        }

        public static class Specials
        {
            private static Menu Menu { get; set; }

            private static CheckBox _useBalista;
            private static CheckBox _balistaComboOnly;
            private static CheckBox _balistaMoreHealth;
            private static Slider _balistaTriggerRange;

            public static bool UseBalista
            {
                get { return _useBalista != null && _useBalista.CurrentValue; }
            }
            public static bool BalistaComboOnly
            {
                get { return _balistaComboOnly.CurrentValue; }
            }
            public static bool BalistaMoreHealthOnly
            {
                get { return _balistaMoreHealth.CurrentValue; }
            }
            public static int BalistaTriggerRange
            {
                get { return _balistaTriggerRange.CurrentValue; }
            }

            static Specials()
            {
                Menu = Config.Menu.AddSubMenu("Specials");

                Menu.AddGroupLabel("Balista");
                if (EntityManager.Heroes.Allies.Any(o => o.ChampionName == "Blitzcrank"))
                {
                    Menu.Add("infoLabel", new Label("You have no soul bound yet!"));
                    Game.OnTick += BalistaCheckSoulBound;
                }
                else
                {
                    Menu.AddLabel("Blitzcrank takiminda yok, bu yuzden Ballista yok :(");
                }
            }

            private static void BalistaCheckSoulBound(EventArgs args)
            {
                if (SoulBoundSaver.SoulBound != null)
                {
                    Game.OnTick -= BalistaCheckSoulBound;
                    Menu.Remove("infoLabel");

                    if (SoulBoundSaver.SoulBound.ChampionName != "Blitzcrank")
                    {
                        Menu.AddLabel("Blitzcrank bagli değilsin!");
                        Menu.AddLabel("Yeniden baglamaya karar verdiyseniz, lutfen eklentiyi yeniden yukleyin");
                        Menu.AddLabel("Yeni baglamani tanir! Simdi eglenelim oynamaya baslayin!");
                        return;
                    }

                    _useBalista = Menu.Add("useBalista", new CheckBox("Enabled"));
                    Menu.AddSeparator(0);
                    _balistaComboOnly = Menu.Add("balistaComboOnly", new CheckBox("Yanlizca kombo modunda", false));
                    _balistaMoreHealth = Menu.Add("moreHealth", new CheckBox("Sadece daha fazla sağligim varsa"));

                    const int blitzcrankQrange = 925;
                    _balistaTriggerRange = Menu.Add("balistaTriggerRange",
                        new Slider("Sizinle yakalanan hedef arasindaki tetikleme araligi", (int) SpellManager.R.Range, (int) SpellManager.R.Range,
                            (int) (SpellManager.R.Range + blitzcrankQrange * 0.8f)));

                    // Handle Blitzcrank hooks in Kalista.OnTickBalistaCheck
                    Obj_AI_Base.OnBuffGain += delegate(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs eventArgs)
                    {
                        if (eventArgs.Buff.DisplayName == "RocketGrab" && eventArgs.Buff.Caster.NetworkId == SoulBoundSaver.SoulBound.NetworkId)
                        {
                            Game.OnTick += Kalista.OnTickBalistaCheck;
                        }
                    };
                    Obj_AI_Base.OnBuffLose += delegate(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs eventArgs)
                    {
                        if (eventArgs.Buff.DisplayName == "RocketGrab" && eventArgs.Buff.Caster.NetworkId == SoulBoundSaver.SoulBound.NetworkId)
                        {
                            Game.OnTick -= Kalista.OnTickBalistaCheck;
                        }
                    };
                }
            }

            public static void Initialize()
            {
            }
        }
    }
}
