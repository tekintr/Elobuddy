using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTTBOTDarius
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

        public static Menu Main, drawMenu, comboMenu, harassMenu, farmMenu, miscMenu;

        public static void LoadMenu()
        {
            Main = MainMenu.AddMenu("Darius", "Darius");
                      
            comboMenu = Main.AddSubMenu("Kombo");
            comboMenu.AddGroupLabel("Q Ayarlari");
            comboMenu.Add("useQ", new CheckBox("Kullan Q", true));
            comboMenu.Add("LockQ", new CheckBox("Hedefe kilitlen Q", true));
            comboMenu.AddGroupLabel("W Config");
            comboMenu.Add("useW", new CheckBox("Kullan W", true));
            comboMenu.Add("autoW", new CheckBox("Otomatik W", true));
            comboMenu.AddGroupLabel("E Config");
            comboMenu.Add("useE", new CheckBox("Kullan E", true));
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.Team != ComboManager.Player.Team))
                comboMenu.Add("Eon" + enemy.ChampionName, new CheckBox("E kullan : " + enemy.ChampionName));
            
            comboMenu.AddGroupLabel("R Ayarlari");
            comboMenu.Add("autoR", new CheckBox("Otomatik R", true));
            comboMenu.Add("useR", new CheckBox("Kullan R", true));
            comboMenu.Add("useRManual", new KeyBind("Yarı manuel R kullanma tusu", false, KeyBind.BindTypes.HoldActive, 'T'));
            comboMenu.Add("autoRbuff", new CheckBox("Otomatik coklu R kullanma suresi biticekse ", true));
            comboMenu.Add("autoRdeath", new CheckBox("Otomatik coklu R kullan ve altinda ise 10 % canin", true));
            comboMenu.Add("adjustDmg", new Slider("Ayarla ulti hasari", 0, -150, 150));

            harassMenu = Main.AddSubMenu("Durtme");
            harassMenu.AddGroupLabel("Q Ayari");
            harassMenu.Add("useQ", new CheckBox("Kullan Q", true));
            harassMenu.Add("ManaHarass", new Slider("Mana ayari", 60, 0, 100));

            miscMenu = Main.AddSubMenu("Karisik");
            miscMenu.Add("InterruptEQ", new CheckBox("Gelen buyuleri engelle E ile?"));
            miscMenu.Add("R.KillSteal", new CheckBox("Oldururken R"));
            miscMenu.Add("skinHack", new CheckBox("Kostum Sec"));
            miscMenu.Add("SkinID", new Slider("Skin", 0, 0, 8));


            farmMenu = Main.AddSubMenu("Minyon Kesme");
            farmMenu.Add("farmW", new CheckBox("Minyona W", true));
            farmMenu.Add("farmQ", new CheckBox("Minyona Q", true));

            drawMenu = Main.AddSubMenu("Cizimler");
            drawMenu.AddGroupLabel("Draw Spell");
            drawMenu.Add("qRange", new CheckBox("Goster Q Menzili", false));
            drawMenu.Add("eRange", new CheckBox("Goster E Menzili", false));
            drawMenu.Add("rRange", new CheckBox("Goster R Menzili", false));
            drawMenu.Add("onlyRdy", new CheckBox("Sadece hazir olanlari goster", true));
        }
    }
}
