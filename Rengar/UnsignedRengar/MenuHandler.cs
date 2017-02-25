using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace UnsignedRengar
{
    class MenuHandler
    {
        public static Menu mainMenu, Combo, Harass, Killsteal, LaneClear, JungleClear, LastHit, Flee, Items, Drawing;

        public static void Initialize()
        {
            #region CreateMenus
            mainMenu = MainMenu.AddMenu("Unsigned Rengar", "Main Menu");
            Combo = AddSubMenu(mainMenu, "Combo");
            Harass = AddSubMenu(mainMenu, "Harass");
            Killsteal = AddSubMenu(mainMenu, "Killsteal");
            LaneClear = AddSubMenu(mainMenu, "Lane Clear");
            JungleClear = AddSubMenu(mainMenu, "Jungle Clear");
            LastHit = AddSubMenu(mainMenu, "Last Hit");
            Flee = AddSubMenu(mainMenu, "Flee");
            Items = AddSubMenu(mainMenu, "Items");
            Drawing = AddSubMenu(mainMenu, "Drawing");
            #endregion

            #region Set Menu Values
            mainMenu.Add("Creator", new Label("Chaos tarafindan yapilan Unsigned Serisinin bir parcasi."));
            AddComboBox(mainMenu, "Prediction Type:", 0, "EloBuddy", "Current Position");

            AddCheckboxes(ref Combo, "Kullan Q", "Kullan Dort yuk Q", "Kullan W", "Kullan Dort yuk W", "Kullan E", "Kullan Dort yuk E",
                "Dort yuk icin W kullan", "Kullan W for damage_false", "Kullan Dort yuk W for damage_false", "Kullan Dort yuk W stan CC",
                "Kullan Item", "Kullan Tutustur", "Kullan Carp");
            AddSlider(Combo, "Kullan W at % eksik can", 15, 1, 100);
            AddSlider(Combo, "Kullan Dort yuk W at % eksik can", 15, 1, 100);

            AddCheckboxes(ref Harass, "Kullan Q", "Kullan Dort yuk Q", "Kullan W", "Kullan Dort yuk W", "Kullan E", "Kullan Dort yuk E",
                "Dort yuk icin W kullan", "Kullan W for damage_false", "Use Empowered W for damage_false", "Kullan Dort yuk W to stop CC",
                "Kullan Item", "Kullan Carp");
            AddSlider(Harass, "Kullan W at % eksik can", 8, 1, 100);
            AddSlider(Harass, "Kullan Dort yuk W at % eksik can", 8, 1, 100);

            AddCheckboxes(ref JungleClear, "Kullan Q", "Kullan Dort yuk Q", "Kullan W", "Kullan Dort yuk W_false", "Kullan E", "Kullan Dort yuk E_false",
                "Kullan W for damage_false", "Kullan Dort yuk W for damage_false", "Dort yuk icin W kullan",
                "Kullan Items", "Kullan Carp", "Kullan Carp for HP");
            AddSlider(JungleClear, "Kullan W at % eksik can", 8, 1, 100);
            AddSlider(JungleClear, "Kullan Dort yuk W at % eksik can", 8, 1, 100);

            AddCheckboxes(ref LaneClear, "Kullan Q", "Kullan Dort yuk Q", "Kullan W", "Kullan Dort yuk W", "Kullan E", "Kullan Dort yuk E_false",
                "Kullan W for damage_false", "Kullan Dort yuk W for damage_false", "Dort yuk icin W kullan",  "Kaydet vahset",
                "Kullan Items_false");
            AddSlider(LaneClear, "Kullan W at % eksik can", 8, 1, 100);
            AddSlider(LaneClear, "Kullan Dort yuk W at % eksik can", 8, 1, 100);

            AddCheckboxes(ref LastHit, "Kullan Q", "Kullan Dort yuk Q", "Kullan W_false", "Kullan Dort yuk W_false", "Kullan E", "Kullan Dort yuk E_false", 
                "Kaydet Vahset",
                "Kullan Items");
            AddSlider(LastHit, "W kullanmak icin minyon", 2, 0, 5);
            AddSlider(LastHit, "Dort yuk W kullanmak icin minyon", 2, 0, 5);

            AddCheckboxes(ref Killsteal, "Killsteal", "Kullan Q", "Kullan Dort yuk Q", "Kullan W", "Kullan Dort yuk W",
                "Kullan E", "Kullan Dort yuk E", "Kullan Item", "Kullan Tutusur", "Kullan Carp");

            AddCheckboxes(ref Flee, "Kullan Dort yuk W", "Kullan E", "Kullan Dort yuk E", "Kullan Dort yuk W to stop CC", "Jump from Brush");

            AddCheckboxes(ref Items, "Kullan Civali Sash", "Kullan Civali", "Kullan Tiamat", "Kullan Vahsi Hydra", "Kullan Hasmetli Hydra", "Kullan Youmuus", "Kullan Bilgewater Palasi", "Kullan Hextech Silahkilic", "Kullan Mahvolmus Kralin Kilici");
            AddSlider(Items, "Su kadar minyona tiamat/hydra kullan", 2, 1, 10);
            AddSlider(Items, "Su kadar sampiyon tiamat/hydra kullan", 2, 1, 10);

            AddCheckboxes(ref Drawing, "Draw Q_false", "Q yaricapini ciz", "Goster W", "Goster E", "Goster R Algilama Araligi", "Draw Arrow to R Target_false", "Goster Oldurulebilir Yazisi", "Kombodan Sonra Dusman Sagligi Goster");
            AddSlider(Drawing, "Autos in Combo", 2, 0, 5);
            #endregion
        }

        public static void AddCheckboxes(ref Menu menu, params string[] checkBoxValues)
        {
            foreach (string s in checkBoxValues)
            {
                if (s.Length > "_false".Length && s.Contains("_false"))
                    AddCheckbox(ref menu, s.Remove(s.IndexOf("_false"), 6), false);
                else
                    AddCheckbox(ref menu, s, true);
            }
        }
        public static Menu AddSubMenu(Menu startingMenu, string text)
        {
            Menu menu = startingMenu.AddSubMenu(text, text + ".");
            menu.AddGroupLabel(text + " Settings");
            return menu;
        }
        public static CheckBox AddCheckbox(ref Menu menu, string text, bool defaultValue = true)
        {
            return menu.Add(menu.UniqueMenuId + text, new CheckBox(text, defaultValue));
        }
        public static CheckBox GetCheckbox(Menu menu, string text)
        {
            return menu.Get<CheckBox>(menu.UniqueMenuId + text);
        }
        public static bool GetCheckboxValue(Menu menu, string text)
        {
            CheckBox checkbox = GetCheckbox(menu, text);

            if (checkbox == null)
                Console.WriteLine("Checkbox (" + text + ") not found under menu (" + menu.DisplayName + "). Unique ID (" + menu.UniqueMenuId + text + ")");

            return checkbox.CurrentValue;
        }
        public static ComboBox AddComboBox(Menu menu, string text, int defaultValue = 0, params string[] values)
        {
            return menu.Add(menu.UniqueMenuId + text, new ComboBox(text, defaultValue, values));
        }
        public static ComboBox GetComboBox(Menu menu, string text)
        {
            return menu.Get<ComboBox>(menu.UniqueMenuId + text);
        }
        public static string GetComboBoxText(Menu menu, string text)
        {
            return menu.Get<ComboBox>(menu.UniqueMenuId + text).SelectedText;
        }
        public static Slider GetSlider(Menu menu, string text)
        {
            return menu.Get<Slider>(menu.UniqueMenuId + text);
        }
        public static int GetSliderValue(Menu menu, string text)
        {
            return menu.Get<Slider>(menu.UniqueMenuId + text).CurrentValue;
        }
        public static Slider AddSlider(Menu menu, string text, int defaultValue, int minimumValue, int maximumValue)
        {
            return menu.Add(menu.UniqueMenuId + text, new Slider(text, defaultValue, minimumValue, maximumValue));
        }
    }
}
