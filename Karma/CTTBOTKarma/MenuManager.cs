using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CTTBOTKarma
{
    class MenuManager
    {
        public static bool getCheckBoxItem(Menu m, string item)
        {
            return m[item].Cast<CheckBox>().CurrentValue;
        }

        public static int getSliderItem(Menu m, string item)
        {
            return m[item].Cast<Slider>().CurrentValue;
        }

        public static bool getKeyBindItem(Menu m, string item)
        {
            return m[item].Cast<KeyBind>().CurrentValue;
        }

        public static int getBoxItem(Menu m, string item)
        {
            return m[item].Cast<ComboBox>().CurrentValue;
        }

        public static Menu Main, drawMenu, comboMenu, harassMenu, miscMenu;

        public static void LoadMenu()
        {
            Main = MainMenu.AddMenu("Karma", "Karma");

            comboMenu = Main.AddSubMenu("Kombo");
            comboMenu.Add("UseQ", new CheckBox("Kullan Q", true));
            comboMenu.Add("UseW", new CheckBox("Kullan W", true));
            comboMenu.Add("UseR", new CheckBox("Kullan R", true));          

            harassMenu = Main.AddSubMenu("Durtme");
            harassMenu.Add("UseQ", new CheckBox("Kullan Q", true));
            harassMenu.Add("UseR", new CheckBox("Kullan R", true));
            harassMenu.Add("ManaHarass", new Slider("Durtme icin mana ayari", 60, 0, 100));

            miscMenu = Main.AddSubMenu("Karisik");
            miscMenu.Add("ESheild", new CheckBox("Yap E Kalkan"));
            miscMenu.Add("egapclose", new CheckBox("Kullan E atilma yapana"));
            miscMenu.Add("qgapclose", new CheckBox("Kullan Q atilma yapana"));
            miscMenu.Add("skinHack", new CheckBox("Kostum Sec"));
            miscMenu.Add("SkinID", new Slider("Skin", 0, 0, 8));

            drawMenu = Main.AddSubMenu("Cizimler");
            drawMenu.AddGroupLabel("Draw Spell");
            drawMenu.Add("qRange", new CheckBox("Q Menzili", false));
            drawMenu.Add("wRange", new CheckBox("W Menzili", false));
            drawMenu.Add("eRange", new CheckBox("E Menzili", false));
            drawMenu.Add("onlyRdy", new CheckBox("Sadece Hazir olanlari goster", true));
        }
    }
}
