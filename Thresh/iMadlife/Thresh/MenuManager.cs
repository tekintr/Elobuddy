using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Thresh
{
    public static class MenuManager
    {
        public static Menu AddonMenu;
        public static Dictionary<string, Menu> SubMenu = new Dictionary<string, Menu>();

        public static Menu MiscMenu
        {
            get { return GetSubMenu("Misc"); }
        }

        public static Menu PredictionMenu
        {
            get { return GetSubMenu("Prediction"); }
        }

        public static Menu DrawingsMenu
        {
            get { return GetSubMenu("Drawings"); }
        }

        public static void Init(EventArgs args)
        {
            var addonName = Champion.AddonName;
            var author = Champion.Author;
            AddonMenu = MainMenu.AddMenu(addonName, addonName + " by " + author + " v1.0000");
            AddonMenu.AddLabel(addonName + " made by " + author);

            SubMenu["Prediction"] = AddonMenu.AddSubMenu("Prediction", "Prediction 2.0");
            SubMenu["Prediction"].AddGroupLabel("Q Settings");
            SubMenu["Prediction"].Add("QCombo", new Slider("Komboda vurma yuzdesi", 70));
            SubMenu["Prediction"].Add("QHarass", new Slider("Durtmede vurma yuzdesi", 75));
            SubMenu["Prediction"].AddGroupLabel("E Settings");
            SubMenu["Prediction"].Add("ECombo", new Slider("Komboda vurma yuzdesi", 90));
            SubMenu["Prediction"].Add("EHarass", new Slider("Durtmede vurma yuzdesi", 95));

            SubMenu["Combo"] = AddonMenu.AddSubMenu("Combo", "Combo");
            SubMenu["Combo"].AddStringList("Q1", "Kullan Q1", new[] {"Asla", "Sadece Hedefe", "Rastgele Dusmana"}, 1);
            SubMenu["Combo"].AddStringList("Q2", "Kullan Q2",
                new[] {"Asla", "Sadece hedef yakalandiysa", "Yakalanan hedefe yakinsa"}, 1);
            SubMenu["Combo"].AddStringList("W1", "Kullan W", new[] { "Asla", "Sadece hedef yakalaninca", "Always"}, 1);
            SubMenu["Combo"].Add("W2", new Slider("Kullan W arkadasin cani azsa <= {0}", 20));
            SubMenu["Combo"].AddStringList("E1", "E Mode", new[] { "Asla", "Cek", "Ittir", "Ekibe gore can yuzdesi" }, 1);
            SubMenu["Combo"].AddStringList("E2", "Use E", new[] { "Asla", "Sadece hedefe", "Rastgele Dusmana" }, 3);
            SubMenu["Combo"].Add("R1", new Slider("Kullan R can yuzdesi <= {0}", 15));
            SubMenu["Combo"].Add("R2", new Slider("Kullan R dusman sayisi >= {0}", 3, 0, 5));

            SubMenu["Harass"] = AddonMenu.AddSubMenu("Harass", "Harass");
            SubMenu["Harass"].AddStringList("Q1", "Kullan Q1", new[] { "Asla", "Sadece hedefe", "Rastgele Dusmana" }, 1);
            SubMenu["Harass"].AddStringList("Q2", "Kullan Q2",
                new[] {"Asla", "Hedef yakalaninca", "Yakalanan hedefe yakinsa"}, 1);
            SubMenu["Harass"].AddStringList("W1", "Kullan W", new[] { "Asla", "Sadece hedef yakalaninca", "Surekli"}, 1);
            SubMenu["Harass"].Add("W2", new Slider("Kullan W herkese cani dusukse <= {0}", 35));
            SubMenu["Harass"].AddStringList("E1", "E Mode", new[] { "Asla", "Cek", "Ittir", "Ekibe gore can yuzdesi" }, 3);
            SubMenu["Harass"].AddStringList("E2", "Kullan E", new[] { "Asla", "Sadece hedefe", "Rastgele Dusmana" }, 3);
            SubMenu["Harass"].Add("Mana", new Slider("Min. Mana Percent:", 20));

            SubMenu["JungleClear"] = AddonMenu.AddSubMenu("JungleClear", "JungleClear");
            SubMenu["JungleClear"].Add("Q", new CheckBox("Kullan Q"));
            SubMenu["JungleClear"].Add("E", new CheckBox("Kullan E"));
            SubMenu["JungleClear"].Add("Mana", new Slider("Min. Mana Percent:", 20));

            SubMenu["KillSteal"] = AddonMenu.AddSubMenu("KillSteal", "KillSteal");
            SubMenu["KillSteal"].Add("Q", new CheckBox("Kullan Q", false));
            SubMenu["KillSteal"].Add("E", new CheckBox("Kullan E", false));
            SubMenu["KillSteal"].Add("R", new CheckBox("Kullan R", false));
            SubMenu["KillSteal"].Add("Ignite", new CheckBox("Kullan Tutustur"));

            SubMenu["Flee"] = AddonMenu.AddSubMenu("Flee", "Flee");
            SubMenu["Flee"].Add("W", new CheckBox("Kullan W herkese"));
            SubMenu["Flee"].Add("E", new CheckBox("Kullan E dusmani itmede"));

            SubMenu["Drawings"] = AddonMenu.AddSubMenu("Drawings", "Drawings");
            SubMenu["Drawings"].Add("Disable", new CheckBox("Disable all drawings", false));
            SubMenu["Drawings"].AddSeparator();
            SubMenu["Drawings"].Add("Q", new CheckBox("Goster Q Menzili"));
            SubMenu["Drawings"].Add("W", new CheckBox("Goster W Menzili", false));
            SubMenu["Drawings"].Add("E", new CheckBox("Goster E Menzili"));
            SubMenu["Drawings"].Add("R", new CheckBox("Goster R Menzili", false));
            SubMenu["Drawings"].Add("Enemy.Target", new CheckBox("Goster daire ile hedefi"));
            SubMenu["Drawings"].Add("Ally.Target", new CheckBox("Goster daire ile tum hedefleri"));

            SubMenu["Misc"] = AddonMenu.AddSubMenu("Misc", "Misc");
            SubMenu["Misc"].Add("W", new Slider("Su kadar dusman varsa W kullan >= {0}", 3, 0, 5));
            SubMenu["Misc"].Add("GapCloser.E", new CheckBox("Kullan E atilmalari kesmede (Ittir veye Cek)"));
            SubMenu["Misc"].Add("GapCloser.Q", new CheckBox("Kullan Q kacanlari yakalamada"));
            SubMenu["Misc"].Add("Interrupter", new CheckBox("Kullan Q/E buyuleri bozmak icin"));
            SubMenu["Misc"].Add("Turret.Q", new CheckBox("Kullan Q dusman kule altinda"));
            SubMenu["Misc"].Add("Turret.E", new CheckBox("Kullan E dusman kule altinda"));
        }

        public static int GetSliderValue(this Menu m, string s)
        {
            if (m != null)
                return m[s].Cast<Slider>().CurrentValue;
            return -1;
        }

        public static bool GetCheckBoxValue(this Menu m, string s)
        {
            return m != null && m[s].Cast<CheckBox>().CurrentValue;
        }

        public static bool GetKeyBindValue(this Menu m, string s)
        {
            return m != null && m[s].Cast<KeyBind>().CurrentValue;
        }

        public static void AddStringList(this Menu m, string uniqueId, string displayName, string[] values,
            int defaultValue = 0)
        {
            var mode = m.Add(uniqueId, new Slider(displayName, defaultValue, 0, values.Length - 1));
            mode.DisplayName = displayName + ": " + values[mode.CurrentValue];
            mode.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    sender.DisplayName = displayName + ": " + values[args.NewValue];
                };
        }

        public static Menu GetSubMenu(string s)
        {
            return (from t in SubMenu where t.Key.Equals(s) select t.Value).FirstOrDefault();
        }
    }
}