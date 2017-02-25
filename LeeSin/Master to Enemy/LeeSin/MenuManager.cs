using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace LeeSin
{
    public static class MenuManager
    {
        public static Menu AddonMenu;
        public static Dictionary<string, Menu> SubMenu = new Dictionary<string, Menu>();
        public static void Init()
        {
            var AddonName = Champion.AddonName;
            var Author = Champion.Author;
            AddonMenu = MainMenu.AddMenu(AddonName, AddonName + " by " + Author + " v6.4.0");
            AddonMenu.AddLabel(AddonName + " made by " + Author);

            SubMenu["Prediction"] = AddonMenu.AddSubMenu("Prediction", "Prediction3");
            SubMenu["Prediction"].AddGroupLabel("Q Settings");
            SubMenu["Prediction"].Add("QCombo", new Slider("Combo HitChancePercent", 65));
            SubMenu["Prediction"].Add("QHarass", new Slider("Harass HitChancePercent", 70));

            //Combo
            SubMenu["Combo"] = AddonMenu.AddSubMenu("Combo", "Combo");
            SubMenu["Combo"].Add("Q", new CheckBox("Kullan Q", true));
            SubMenu["Combo"].Add("W", new CheckBox("Kullan W to GapClose", true));
            SubMenu["Combo"].Add("E", new CheckBox("Kullan E", true));
            SubMenu["Combo"].Add("Smite", new CheckBox("Kullan Carp", false));
            SubMenu["Combo"].Add("Items", new CheckBox("Kullan Saldiri Itemi", true));
            var switcher = SubMenu["Combo"].Add("Switcher", new KeyBind("Combo Switcher", false, KeyBind.BindTypes.HoldActive, (uint)'K'));
            switcher.OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (args.NewValue)
                {
                    var cast = GetSubMenu("Combo")["Mode"].Cast<Slider>();
                    if (cast.CurrentValue == cast.MaxValue)
                    {
                        cast.CurrentValue = 0;
                    }
                    else
                    {
                        cast.CurrentValue++;
                    }
                }
            };
            SubMenu["Combo"].AddStringList("Mode", "Combo Mode", new[] { "Normal Combo", "Star Combo", "Gank Combo" }, 0);
            SubMenu["Combo"]["Mode"].Cast<Slider>().CurrentValue = 0; //E L I M I N A R

            SubMenu["Combo"].AddGroupLabel("Normal Combo");
            SubMenu["Combo"].Add("Normal.R", new CheckBox("Kullan R Hedefe", false));
            SubMenu["Combo"].Add("Normal.Ward", new CheckBox("Kullan totem", false));
            SubMenu["Combo"].Add("Normal.Stack", new Slider("Baska bir skill kullanmadan once kac pasif kullanilsin", 1, 0, 2));
            SubMenu["Combo"].Add("Normal.W", new Slider("Kullan W canim dusukse", 25, 0, 100));
            SubMenu["Combo"].Add("Normal.R.Hit", new Slider("Kullan R Carpicak dusman sayisi >=", 3, 1, 5));

            SubMenu["Combo"].AddSeparator();

            SubMenu["Combo"].AddGroupLabel("Star Combo");
            SubMenu["Combo"].Add("Star.Ward", new CheckBox("Kullan totem", true));
            SubMenu["Combo"].Add("Star.Stack", new Slider("Baska bir skill kullanmadan once kac pasif kullanilsin", 0, 0, 2));
            SubMenu["Combo"].AddStringList("Star.Mode", "Star Combo Mode", new[] { "Q1 R Q2", "R Q1 Q2" }, 0);

            SubMenu["Combo"].AddSeparator();

            SubMenu["Combo"].AddGroupLabel("Gank Combo");
            SubMenu["Combo"].Add("Gank.R", new CheckBox("Kullan R", true));
            SubMenu["Combo"].Add("Gank.Ward", new CheckBox("Kullan totem", true));
            SubMenu["Combo"].Add("Gank.Stack", new Slider("Baska bir skill kullanmadan once kac pasif kullanilsin", 1, 0, 2));
            
            //Insec
            SubMenu["Insec"] = AddonMenu.AddSubMenu("Insec", "Insec");
            SubMenu["Insec"].Add("Key", new KeyBind("Insec Key (Bu tus benzersiz olmali)", false, KeyBind.BindTypes.HoldActive, (uint)'R'));
            SubMenu["Insec"].Add("Object", new CheckBox("Kullan Q dusman sampiyon/minyon hedefe vuramassan", true));
            SubMenu["Insec"].AddSeparator(0);
            SubMenu["Insec"].Add("Flash.Return", new CheckBox("Kullan sicra ile devam", false));
            SubMenu["Insec"].AddStringList("Priority", "Priority", new[] { "TotemeGit > Sicra", "Sicra > TotemeGit" }, 1);
            SubMenu["Insec"].AddStringList("Flash.Priority", "Flash Priority", new[] { "Sadece R -> Sicra", "Sadece sicra -> R", "R -> Sicra ve sicra -> R" }, 0);
            SubMenu["Insec"].AddStringList("Position", "Insec End Position", new[] { "Herkesi sec > Secilen Pozisyon > Kule > Dostlar yaninda > Suan ki pozisyon", "Fare pozisyonu", "Suan ki posizyon" }, 0);
            SubMenu["Insec"].Add("DistanceBetweenPercent", new Slider("% hedef ile arandaki mesafe", 20, 0, 100));
            SubMenu["Insec"].AddGroupLabel("Tips");
            SubMenu["Insec"].AddLabel("Bir muttefik secmek icin o muttefikin üzerine tiklayin.");
            SubMenu["Insec"].AddLabel("Bir hedef secmek icin o hedefte sol tiklama kullanin.");
            SubMenu["Insec"].AddLabel("Bir konumu secmek icin o konumdaki sol tiklamayi kullanin.");
            
            SubMenu["Harass"] = AddonMenu.AddSubMenu("Harass", "Harass");
            SubMenu["Harass"].Add("Q", new CheckBox("Kullan Q", true));
            SubMenu["Harass"].Add("W", new CheckBox("Kullan W kacis icin", true));
            SubMenu["Harass"].Add("E", new CheckBox("Kullan E", true));

            SubMenu["Smite"] = AddonMenu.AddSubMenu("Smite", "Smite");
            SubMenu["Smite"].Add("Q.Combo", new CheckBox("Kullan Q - Carp Komboda", true));
            SubMenu["Smite"].Add("Q.Harass", new CheckBox("Kullan Q - Carp Durtmede", false));
            SubMenu["Smite"].Add("Q.Insec", new CheckBox("Kullan Q - Carp in Insec", true));
            SubMenu["Smite"].Add("DragonSteal", new CheckBox("Kullan Carp on Ejder/Baron", true));
            //SubMenu["Smite"].Add("KillSteal", new CheckBox("Use Smite to KillSteal", true));
            
            SubMenu["JungleClear"] = AddonMenu.AddSubMenu("JungleClear", "JungleClear");
            SubMenu["JungleClear"].Add("Q", new CheckBox("Use Q", true));
            SubMenu["JungleClear"].Add("W", new CheckBox("Use W", true));
            SubMenu["JungleClear"].Add("E", new CheckBox("Use E", true));
            SubMenu["JungleClear"].Add("Smite", new CheckBox("Use Smite on Dragon/Baron", true));

            SubMenu["KillSteal"] = AddonMenu.AddSubMenu("KillSteal", "KillSteal");
            SubMenu["KillSteal"].Add("Ward", new CheckBox("Kullan Totem Atilma yapanlara", false));
            SubMenu["KillSteal"].Add("Q", new CheckBox("Kullan Q", false));
            SubMenu["KillSteal"].Add("W", new CheckBox("Kullan W Atilma yapanlara", true));
            SubMenu["KillSteal"].Add("E", new CheckBox("Kullan E", true));
            SubMenu["KillSteal"].Add("R", new CheckBox("Kullan R", false));
            SubMenu["KillSteal"].Add("Ignite", new CheckBox("Kullan Tutustur", true));
            SubMenu["KillSteal"].Add("Smite", new CheckBox("Kullan Carp", true));

            SubMenu["Drawings"] = AddonMenu.AddSubMenu("Drawings", "Drawings");
            SubMenu["Drawings"].Add("Disable", new CheckBox("Butun Gostergeler devre disi", false));
            SubMenu["Drawings"].Add("Q", new CheckBox("Goster Q Menzili", true));
            SubMenu["Drawings"].Add("W", new CheckBox("Goster W Menzili", true));
            SubMenu["Drawings"].Add("E", new CheckBox("Goster E Menzili", false));
            SubMenu["Drawings"].Add("R", new CheckBox("Goster R Menzili", false));
            SubMenu["Drawings"].Add("Combo.Mode", new CheckBox("Goster yaili olarak secili modu", true));
            SubMenu["Drawings"].Add("Insec.Line", new CheckBox("Goster insec hattini", true));
            SubMenu["Drawings"].Add("Target", new CheckBox("Hedefin uzerinde cember ciz", true));

            SubMenu["Flee"] = AddonMenu.AddSubMenu("Flee/WardJump", "Flee");
            SubMenu["Flee"].Add("WardJump", new CheckBox("Kullan Toteme Gitme", true));
            SubMenu["Flee"].Add("W", new CheckBox("Kullan W Fare yakinindaki nesneler üzerinde", true));

            SubMenu["Misc"] = AddonMenu.AddSubMenu("Misc", "Misc");
            SubMenu["Misc"].Add("Interrupter", new CheckBox("Kullan R Kanal buyulerini kesmek", true));
            SubMenu["Misc"].Add("Overkill", new Slider("Oluler uzerinde % hasar tahmini", 10, 0, 100));
            SubMenu["Misc"].Add("R.Hit", new Slider("Kullan R kisiye deyicekse Hit >=", 3, 1, 5));

        }
        
        public static int GetSliderValue(this Menu m, string s)
        {
            if (m != null)
                return m[s].Cast<Slider>().CurrentValue;
            return -1;
        }
        public static bool GetCheckBoxValue(this Menu m, string s)
        {
            if (m != null)
                return m[s].Cast<CheckBox>().CurrentValue;
            return false;
        }
        public static bool GetKeyBindValue(this Menu m, string s)
        {
            if (m != null)
                return m[s].Cast<KeyBind>().CurrentValue;
            return false;
        }
        public static void AddStringList(this Menu m, string uniqueId, string displayName, string[] values, int defaultValue = 0)
        {
            var mode = m.Add(uniqueId, new Slider(displayName, defaultValue, 0, values.Length - 1));
            mode.DisplayName = displayName + ": " + values[mode.CurrentValue];
            mode.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
            {
                sender.DisplayName = displayName + ": " + values[args.NewValue];
            };
        }
        public static Menu GetSubMenu(string s)
        {
            foreach (KeyValuePair<string, Menu> t in SubMenu)
            {
                if (t.Key.Equals(s))
                {
                    return t.Value;
                }
            }
            return null;
        }
        public static Menu MiscMenu
        {
            get
            {
                return GetSubMenu("Misc");
            }
        }
        public static Menu PredictionMenu
        {
            get
            {
                return GetSubMenu("Prediction");
            }
        }
        public static Menu DrawingsMenu
        {
            get
            {
                return GetSubMenu("Drawings");
            }
        }
        public static Menu SmiteMenu
        {
            get
            {
                return GetSubMenu("Smite");
            }
        }
    }
}
