using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using YasuoBuddy.EvadePlus;

namespace YasuoBuddy
{
    internal class Yasuo
    {
        public static Menu Menu, ComboMenu, HarassMenu, FarmMenu, FleeMenu, DrawMenu, MiscSettings;
        private static int _cleanUpTime;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Yasuo) return;

            Menu = MainMenu.AddMenu("YasuoBuddy", "yasuobuddyfluxy");

            ComboMenu = Menu.AddSubMenu("Kombo", "yasuCombo");
            ComboMenu.AddGroupLabel("Kombo Ayarlari");
            ComboMenu.Add("combo.Q", new CheckBox("Kullan Q"));
            ComboMenu.Add("combo.E", new CheckBox("Kullan E"));
            ComboMenu.Add("combo.stack", new CheckBox("Biriktir Q"));
            ComboMenu.Add("combo.leftclickRape", new CheckBox("Sol tiklama ile durt"));
            ComboMenu.AddSeparator();
            ComboMenu.AddLabel("R Ayarlari");
            ComboMenu.Add("combo.R", new CheckBox("Kullan R"));
            ComboMenu.Add("combo.RTarget", new CheckBox("Kullan R secilen hedefe her zaman"));
            ComboMenu.Add("combo.RKillable", new CheckBox("Kullan R Olucek ise"));
            ComboMenu.Add("combo.MinTargetsR", new Slider("Kullan R Enaz dusman sayisi", 2, 1, 5));

            HarassMenu = Menu.AddSubMenu("Durtme", "yasuHarass");
            HarassMenu.AddGroupLabel("Durtme Ayarlari");
            HarassMenu.Add("harass.Q", new CheckBox("Kullan Q"));
            HarassMenu.Add("harass.E", new CheckBox("Kullan E"));
            HarassMenu.Add("harass.stack", new CheckBox("Biriktir Q"));

            FarmMenu = Menu.AddSubMenu("Farm Ayarlari", "yasuoFarm");
            FarmMenu.AddGroupLabel("Farm Ayarlari");
            FarmMenu.AddLabel("SonVurus");
            FarmMenu.Add("LH.Q", new CheckBox("Kullan Q"));
            FarmMenu.Add("LH.E", new CheckBox("Kullan E"));
            FarmMenu.Add("LH.EUnderTower", new CheckBox("Kullan E Kule altinda", false));

            FarmMenu.AddLabel("DalgaTemizleme");
            FarmMenu.Add("WC.Q", new CheckBox("Kullan Q"));
            FarmMenu.Add("WC.E", new CheckBox("Kullan E"));
            FarmMenu.Add("WC.EUnderTower", new CheckBox("Kullan E Kule altinda", false));

            FarmMenu.AddLabel("Orman");
            FarmMenu.Add("JNG.Q", new CheckBox("Kullan Q"));
            FarmMenu.Add("JNG.E", new CheckBox("Kullan E"));

            FleeMenu = Menu.AddSubMenu("Kacis/Evade", "yasuoFlee");
            FleeMenu.AddGroupLabel("Kacis Ayarlari");
            FleeMenu.Add("Flee.E", new CheckBox("Kullan E"));
            FleeMenu.Add("Flee.stack", new CheckBox("Biriktir Q"));
            FleeMenu.AddGroupLabel("Evade Ayarlari");
            FleeMenu.Add("Evade.E", new CheckBox("Kullan E skillden kacmak icin"));
            FleeMenu.Add("Evade.W", new CheckBox("Kullan W skillden kacmak icin"));
            FleeMenu.Add("Evade.WDelay", new Slider("Gecikme (ms)", 0, 0, 1000));

            MiscSettings = Menu.AddSubMenu("Karisik Ayarlar");
            MiscSettings.AddGroupLabel("Oldurme Ayarlari");
            MiscSettings.Add("KS.Q", new CheckBox("Kullan Q"));
            MiscSettings.Add("KS.E", new CheckBox("Kullan E"));
            MiscSettings.AddGroupLabel("Otomatik Q Ayarlari");
            MiscSettings.Add("Auto.Q3", new CheckBox("Kullan Q3"));
            MiscSettings.Add("Auto.Active", new KeyBind("Otomatik Q Dusmana", false, KeyBind.BindTypes.PressToggle, 'M'));

            Program.Main(null);

            DrawMenu = Menu.AddSubMenu("Cizimler", "yasuoDraw");
            DrawMenu.AddGroupLabel("Gosterge Ayarlari");

            DrawMenu.Add("Draw.Q", new CheckBox("Goster Q Menzili", false));
            DrawMenu.AddColourItem("Draw.Q.Colour");
            DrawMenu.AddSeparator();

            DrawMenu.Add("Draw.E", new CheckBox("Goster E Menzili", false));
            DrawMenu.AddColourItem("Draw.E.Colour");
            DrawMenu.AddSeparator();

            DrawMenu.Add("Draw.R", new CheckBox("Goster R Menzili", false));
            DrawMenu.AddColourItem("Draw.R.Colour");
            DrawMenu.AddSeparator();

            DrawMenu.AddLabel("Asagidan rek secin = ");
            DrawMenu.AddColourItem("Draw.Down", 7);
            
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            EEvader.Init();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu["Draw.Q"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(
                    SpellManager.Q.IsReady() ? DrawMenu.GetColour("Draw.Q.Colour") : DrawMenu.GetColour("Draw.Down"),
                    SpellManager.Q.Range, Player.Instance.Position);
            }
            if (DrawMenu["Draw.R"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(
                    SpellManager.R.IsReady() ? DrawMenu.GetColour("Draw.R.Colour") : DrawMenu.GetColour("Draw.Down"),
                    SpellManager.R.Range, Player.Instance.Position);
            }
            if (DrawMenu["Draw.E"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(
                    SpellManager.E.IsReady() ? DrawMenu.GetColour("Draw.E.Colour") : DrawMenu.GetColour("Draw.Down"),
                    SpellManager.E.Range, Player.Instance.Position);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (_cleanUpTime < Environment.TickCount)
            {
                GC.Collect();
                _cleanUpTime = Environment.TickCount + 1000000;
            }
            StateManager.KillSteal();
            if (MiscSettings["Auto.Active"].Cast<KeyBind>().CurrentValue)
            {
                StateManager.AutoQ();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                StateManager.Flee();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                StateManager.Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateManager.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                StateManager.LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateManager.WaveClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                StateManager.Jungle();
            }
        }
    }
}