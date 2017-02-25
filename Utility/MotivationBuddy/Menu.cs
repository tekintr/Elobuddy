using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotivationBuddy
{ 
    internal class Menus
    {
        public static Menu FirstMenu;



        public static void CreateMenu()
        {

            FirstMenu = MainMenu.AddMenu("Motivation " + "Buddy", Player.Instance.ChampionName.ToLower() + "Buddy");

            FirstMenu.AddGroupLabel("Ayarlar");
            FirstMenu.Add("EnableM", new CheckBox("- Motive etme"));
            FirstMenu.Add("EnableT", new CheckBox("- Tilt etme"));
            FirstMenu.Add("Delay", new Slider("- Gecikme suresi", 100, 0, 10000));
        }
    }
}
