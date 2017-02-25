﻿using System;
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

namespace UnsignedCamille
{
    class MenuHandler
    {
        public static Menu mainMenu, Combo, Harass, Killsteal, LaneClear, JungleClear, LastHit, Flee, Items, Drawing;

        public static void Initialize()
        {
            #region CreateMenus
            mainMenu = MainMenu.AddMenu("Unsigned Camille", "Main Menu");
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
            mainMenu.Add("Creator", new Label("Bu Script, Chaos tarafindan yapilan Unsigned Serisinin bir parcasi."));
            AddComboBox(mainMenu, "Prediction Type:", 0, "EloBuddy", "Current Position");

            AddCheckboxes(ref Combo, "Kullan Q1", "Kullan Q2", "Kullan W", /*"Force Follow in W Range",*/ "Kullan E1", "Kullan E2", "Kullan R", "Kullan Items", "Kullan Carp", "Kullan Tutustur");
            AddSlider(Combo, "Düsmanlar uzerinde W", 1, 1, 6);
            AddCheckboxes(ref Harass, "Kullan Q1", "Kullan Q2", "Kullan W", /*"Force Follow in W Range",*/ "Kullan E1", "Kullan E2", "Kullan R_false", "Kullan Carp_false", "Kullan Items");
            AddSlider(Harass, "Düsmanlar uzerinde W", 1, 1, 6);

            AddCheckboxes(ref JungleClear, "Kullan Q1", "Kullan Q2", "Kullan W", /*"Force Follow in W Range",*/ "Kullan E1_false", "Kullan E2_false", "Kullan Carp", "Kullan Carp icin HP", "Kullan Items");
            AddSlider(JungleClear, "Düsmanlar uzerinde W", 1, 1, 6);
            AddCheckboxes(ref LaneClear, "Kullan Q1", "Kullan Q2", "Kullan W", /*"Force Follow in W Range",*/ "Kullan E1_false", "Kullan E2_false", "Kullan Items_false");
            AddSlider(LaneClear, "Düsmanlar uzerinde W", 3, 1, 6);
            AddCheckboxes(ref LastHit, "Kullan Q1", "Kullan Q2", "Kullan W", /*"Force Follow in W Range",*/ "Kullan E1_false", "Kullan E2_false", "Kullan Items");
            AddSlider(LastHit, "Düsmanlar uzerinde W", 2, 1, 6);

            AddCheckboxes(ref Killsteal, "Killsteal", "Kullan Q1", "Kullan Q2", "Kullan W", /*"Force Follow in W Range",*/ "Kullan E1", "Kullan E2", "Kullan Carp", "Kullan Items", "Kullan Tutustur");
            AddSlider(Killsteal, "Düsmanlar uzerinde W", 1, 1, 6);
            AddCheckboxes(ref Flee, "Kullan E1", "Kullan E2");

            AddCheckboxes(ref Items, "Civali kullan", "Kullan Mercurials Scimitar", "Kullan Tiamat", "Kullan vahsi Hydra", "Kullan hasmetli Hydra", "Kullan Youmuus", "Kullan Bilgewater Palasi", "Kullan Hextech Silahkilic", "Kullan Mahvolmus");
            AddSlider(Items, "Minions to use Tiamat/Ravenous Hydra On", 2, 1, 10);
            AddSlider(Items, "Champions to use Tiamat/Ravenous Hydra on", 1, 1, 10);

            AddCheckboxes(ref Drawing, "Goster W ic menzili", "Goster W dis menzili", "Goster E menzili", "Goster R menzili", "Goster Combo hasari", "E icin duvarlari ciz E_false");
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
