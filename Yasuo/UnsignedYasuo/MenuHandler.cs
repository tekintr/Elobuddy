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

namespace UnsignedYasuo
{
    class MenuHandler
    {
        public static Menu mainMenu, Combo, Harass, AutoHarass, Killsteal, LaneClear, JungleClear, LastHit, Flee, Ult, Items, Drawing;

        public static void Initialize()
        {
            #region CreateMenus
            mainMenu = MainMenu.AddMenu("Unsigned Yasuo", "UnsignedYasuo");
            Combo = AddSubMenu(mainMenu, "Kombo");
            Harass = AddSubMenu(mainMenu, "Durtme");
            AutoHarass = AddSubMenu(mainMenu, "Oto Durtme");
            LaneClear = AddSubMenu(mainMenu, "KoridorTemizle");
            JungleClear = AddSubMenu(mainMenu, "Orman");
            LastHit = AddSubMenu(mainMenu, "SonVurus");
            Killsteal = AddSubMenu(mainMenu, "Oldurme Sekli");
            Flee = AddSubMenu(mainMenu, "Kacis");
            Ult = AddSubMenu(mainMenu, "Ulti");
            Drawing = AddSubMenu(mainMenu, "Cizimler");
            Items = AddSubMenu(mainMenu, "Itemler");
            #endregion

            #region Set Menu Values
            mainMenu.Add("Creator", new Label("Yapimci Chaos Ceviri TekinTR"));
            AddComboBox(mainMenu, "Prediction Type:", 0, "EloBuddy", "Current Position");

            AddCheckboxes(ref Combo, "Kullan Q", "Kullan Q3", "Kullan E_false", "Kullan EQ_false", "Kullan E Kule Alti_false", "Kullan R", "Kullan Items", "Beyblade");
            AddComboBox(Combo, "Hamle Modu: ", 0, "AtilmeOnleyici", "Fareye", "Kapali");
            AddCheckboxes(ref Harass, "Kullan Q", "Kullan Q3", "Kullan Q minyonlara son vurus_false", "Kullan Q3 minyonlara son vurus_false", "Kullan E_false", "Kullan EQ_false", "Kullan E Kule Alti_false", "Kullan R_false", "Kullan Items");
            AddCheckboxes(ref AutoHarass, "Kullan Q", "Kullan Q3", "Kullan E_false", "Kullan EQ_false", "Kullan E Kule Alti_false", "Kullan Items");
            AddCheckboxes(ref LaneClear, "Kullan Q", "Kullan Q3", "Kullan E_false", "Kullan E sadece son vurus", "Kullan EQ_false", "Kullan E Kule Alti_false", "Kullan Items");
            AddCheckboxes(ref JungleClear, "Kullan Q", "Kullan Q3", "Kullan E", "Kullan E sadece son vurus_false", "Kullan EQ_false", "Kullan Items");
            AddCheckboxes(ref LastHit, "Kullan Q", "Kullan Q3", "Kullan E", "Kullan EQ_false", "Kullan E Kule Alti_false", "Kullan Items");
            AddCheckboxes(ref Killsteal, "Oldurme Aktif", "Kullan Q", "Kullan Q3", "Kullan E", "Kullan EQ", "Kullan E Kule Alti", "Kullan Items", "Kullan Tutustur");
            AddCheckboxes(ref Flee, "Kullan E", "Kullan E Kule Alti", "Yuk Kas Q", "DuvardanGec");
            AddSlider(Flee, "DuvardanGecme Extra Uzaklik", 20, 10, 200);
            AddCheckboxes(ref Ult, "Kullan R Son saniyede", "Kullan R butun dusmanlar menzildeyken", "Kullan R takip ederken_false", "Kullan R at 10% HP_false", "Kullan R Secilen hedefe");
            AddSlider(Ult, "Kullan R x dusman yada fazlasi icin:", 3, 1, 5);
            AddSlider(Ult, "Kullan R secilen hedef ve x daha fazla dusman varsa:", 2, 1, 4);

            AddCheckboxes(ref Drawing, "Goster Q", "Goster W_false", "Goster E", "Goster E Bitis pozisyonu hedef_false", "Goster E Bitis pozisyonu hedef - Detayli_false", "Goster EQ_false", "Goster EQ hedef uzerinde_false", "Goster R", "Goster Beyblade", "Goster Duvardan Gecis", "Goster Kule menzili", "Goster Kombo hasarimi");
            AddSlider(Drawing, "Otomatik kombo kullan", 2, 0, 5);
            AddSlider(Drawing, "Q's kombo icinde kullan", 2, 0, 5);
            AddCheckboxes(ref Items, "Kullan Civali", "Kullan Civa Yatagan", "Kullan Tiamat", "Kullan Vahsi Hydra", "Kullan Hasmetli Hydra", "Kullan Youmuus", "Kullan Bilgewater Palasi", "Kullan Hextech Silahkilic", "Kullan Mahvolmus");
            WindWall.Initialize();
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
