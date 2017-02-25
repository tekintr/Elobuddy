using System.Reflection;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Drawing;
using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using HTTF_Yasuo.Utils;

namespace HTTF_Yasuo
    {
    class Yasuo
    {
        public static Menu Principal, Combo, Misc, Flee, Clean, Draw, Evadee;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
            Chat.Print("Yasuo HTTF (Beta) Yuklendi.Ceviri TekinTR.");
            Game.OnTick += Game_OnTick;
            Game.OnUpdate += OnGameUpdate;

        }       

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Principal = MainMenu.AddMenu("HTTF Yasuo ", "Yasuo");
            Principal.AddLabel("HTTF Yasuo v" + Assembly.GetExecutingAssembly().GetName().Version);

            //combo+harasse
            Combo = Principal.AddSubMenu("Combo", "Combo");
            Combo.AddSeparator(3);
            Combo.AddLabel("• Kombo Ayarlari");
            Combo.Add("UseQCombo", new CheckBox("Kullan Q"));
            Combo.Add("UseWCombo", new CheckBox("Kullan W"));
            Combo.Add("UseECombo", new CheckBox("Kullan E"));
            Combo.Add("UseRCombo", new CheckBox("Kullan R"));
            Combo.Add("stack.combo", new CheckBox("Biriktir Q?"));
            Combo.Add("combo.leftclickRape", new CheckBox("Sol tik ile hedef sec?"));
            Combo.Add("PredictQ2", new ComboBox("Tahmini Q2", 1, "Dusuk", "Orta", "Yuksek"));

            Combo.AddLabel("• R Ayarlari ");
            Combo.Add("combo.RTarget", new CheckBox("Kullan R secili hedef icin her zaman"));
            Combo.Add("combo.RKillable", new CheckBox("Kullan R Oldururken"));
            Combo.Add("combo.MinTargetsR", new Slider("Kullan R En az hedef sayisi", 2, 1, 5));

            Combo.AddLabel("• Durtme •");
            Combo.Add("Auto.Q3", new CheckBox("Otomatik Q3 ?"));
            Combo.Add("harass.Q", new CheckBox("Kullan Q"));
            Combo.Add("harass.E", new CheckBox("Kullan E"));
            Combo.Add("harass.stack", new CheckBox("Biriktir Q durterken?"));
            //clean
            Clean = Principal.AddSubMenu("Clean", "Temizleme Ayarlari");
            Clean.AddSeparator(3);
            Clean.AddLabel("•Son Vurus•");
            Clean.Add("LastE", new CheckBox("Kullan E"));
            Clean.Add("LastQ", new CheckBox("Kullan Q"));
            Clean.Add("LaseEUT", new CheckBox("Kullan E Kule Altinda"));
            Clean.AddLabel("•Dalga Temizleme•");
            Clean.Add("WC.Q", new CheckBox("Kullan Q"));
            Clean.Add("WC.E", new CheckBox("Kullan E"));
            Clean.AddLabel("•Orman Temizleme•");
            Clean.Add("JungQ", new CheckBox("Kullan Q"));
            Clean.Add("JungE", new CheckBox("Kullan E"));

            //flee
            Flee = Principal.AddSubMenu("Flee", "Kacis Ayarlari");
            Flee.AddSeparator(3);
            Flee.AddLabel("•Flee•");
            Flee.Add("FleeE", new CheckBox("Kullan E"));
            Flee.Add("Flee.stack", new CheckBox("Biriktir Q kacarken?"));
            //Draw
            Draw = Principal.AddSubMenu("Draw", "Cizim Ayarlari");
            Draw.AddSeparator(3);
            Draw.Add("DrawE", new CheckBox("Goster E Mesafesi"));
            Draw.Add("DrawQ", new CheckBox("Goster Q Mesafesi"));
            Draw.Add("DrawR", new CheckBox("Goster R Mesafesi"));
            //Misc
            Misc = Principal.AddSubMenu("Misc", "Karisik");
            Misc.AddSeparator(3);
            Misc.Add("EGapclos", new CheckBox("Atilma yapana E"));
            Misc.Add("Einter", new CheckBox("Buyulerden kacmak icin E"));
            Misc.AddLabel("• Kostum Hilesi •");
            Misc.Add("checkSkin", new CheckBox("Kostum Sec"));
            Misc.Add("Skinid", new Slider("Skin ID", 0, 0, 10));

            //Evade
            Evadee = Principal.AddSubMenu("Evadee", "Evadee Ayarlari");
            Evadee.AddSeparator(3);
            Evadee.AddLabel("•Yakinda Eklenicek•");







            Utils.ForDash.Init();


        }
        private static void OnGameUpdate(EventArgs args)
        {
            if (CheckSkin())
            {
                EloBuddy.Player.SetSkinId(SkinId());
            }
        }
        private static int SkinId()
        {
            return Misc["Skinid"].Cast<Slider>().CurrentValue;
        }

        private static bool CheckSkin()
        {
            return Misc["checkSkin"].Cast<CheckBox>().CurrentValue;
        }
    
        private static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                StateLogic.Flee();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                StateLogic.Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateLogic.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                StateLogic.LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateLogic.WaveClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                StateLogic.Jungle();
            }
        }
        



            public static bool CheckBox(Menu m, string s)
            {
                return m[s].Cast<CheckBox>().CurrentValue;
            }

            public static int Slider(Menu m, string s)
            {
                return m[s].Cast<Slider>().CurrentValue;
            }

            public static bool Keybind(Menu m, string s)
            {
                return m[s].Cast<KeyBind>().CurrentValue;
            }

            public static int ComboBox(Menu m, string s)
            {
                return m[s].Cast<ComboBox>().SelectedIndex;
            }
        }
    }
