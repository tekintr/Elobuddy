using System;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace TwistedFate
{
    internal class Menus
    {
        public static Menu FirstMenu = null;
        public static Menu ComboMenu = null;
        public static Menu CardMenu = null;
        public static Menu MiscMenu = null;
        public static Menu DrawingsMenu = null;

        public static void CreateMenu()
        {
            FirstMenu = MainMenu.AddMenu("Twisted Fate", "TwistedFate");
            CardMenu = FirstMenu.AddSubMenu("Card Selection");
            MiscMenu = FirstMenu.AddSubMenu("Misc");
            DrawingsMenu = FirstMenu.AddSubMenu("Drawings");

            CardMenu.AddGroupLabel("Card Selection");
            CardMenu.Add("combo.selectGoldCard", new KeyBind("Sari Kart Sec", false, KeyBind.BindTypes.HoldActive, 'Z'));
            CardMenu.Add("combo.selectBlueCard", new KeyBind("Mavi kart Sec", false, KeyBind.BindTypes.HoldActive, 'E'));
            CardMenu.Add("combo.selectRedCard", new KeyBind("Kirmisi Kart Sec", false, KeyBind.BindTypes.HoldActive, 'T'));
            CardMenu.Add("combo.goldAfterUlt", new CheckBox("Ultiden sonra Sari kart sec", true));

            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("misc.PingOnKillable", new CheckBox("Oldurulecek Hedefe Ping", true));
            MiscMenu.Add("misc.QImmobileChamps", new CheckBox("Hareketsiz sampiyonlara oto Q", true));

            DrawingsMenu.AddGroupLabel("Drawings");
            DrawingsMenu.Add("drawing.q_Range", new CheckBox("Goster Q Menzili", true));
            DrawingsMenu.Add("drawing.separator1", new CheckBox("Ayirici", false)).IsVisible = false;
            DrawingsMenu.Add("drawing.r_Range", new CheckBox("Goster R Menzili", true));
        }

        public static bool SelectGoldCard()
        {
            return CardMenu["combo.selectGoldCard"].Cast<KeyBind>().CurrentValue;
        }

        public static bool SelectBlueCard()
        {
            return CardMenu["combo.selectBlueCard"].Cast<KeyBind>().CurrentValue;
        }

        public static bool SelectRedCard()
        {
            return CardMenu["combo.selectRedCard"].Cast<KeyBind>().CurrentValue;
        }
    }
}
