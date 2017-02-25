using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BaseUltPlusPlus
{
    public class Program
    {
        public static Menu BaseUltMenu { get; set; }

        public static void Main(string[] args)
        {
            // Wait till the name has fully loaded
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            //Menu
            BaseUltMenu = MainMenu.AddMenu("BaseUlt-3x", "BUP");
            BaseUltMenu.Add("baseult", new CheckBox("BaseUlti"));
            BaseUltMenu.Add("showrecalls", new CheckBox("B atanlari goster"));
            BaseUltMenu.Add("checkcollision", new CheckBox("Carpisma Kontrol"));
            BaseUltMenu.AddSeparator();
            BaseUltMenu.Add("timeLimit", new Slider("FOW Zaman siniri (SEC)", 0, 0, 120));
            BaseUltMenu.AddSeparator();
            BaseUltMenu.Add("nobaseult", new KeyBind("Yok BaseUlt suresi", false, KeyBind.BindTypes.HoldActive, 32));
            BaseUltMenu.AddSeparator();
            BaseUltMenu.AddGroupLabel("BaseUlt-3x Targets");
            foreach (var unit in EntityManager.Heroes.Enemies)
            {
                BaseUltMenu.Add("target" + unit.ChampionName,
                    new CheckBox(string.Format("{0} ({1})", unit.ChampionName, unit.Name)));
            }

            BaseUltMenu.AddGroupLabel("BaseUlt-3x - By TekinTR");
            BaseUltMenu.AddLabel("Ayarlandi Roach_ versiyonuna gore Addon");
            BaseUltMenu.AddLabel("BestAkaliAfrica Tarafindan yapildi");

            Chat.Print("<font color = \"#6B9FE3\">BaseUlt-3x</font><font color = \"#E3AF6B\"> by TekinTR</font>.Tarafindan Turkce Yapildi.");
            // Initialize the Addon
            OfficialAddon.Initialize();

            // Listen to the two main events for the Addon
            Game.OnUpdate += args1 => OfficialAddon.Game_OnUpdate();
            Drawing.OnPreReset += args1 => OfficialAddon.Drawing_OnPreReset(args1);
            Drawing.OnPostReset += args1 => OfficialAddon.Drawing_OnPostReset(args1);
            Drawing.OnDraw += args1 => OfficialAddon.Drawing_OnDraw(args1);
            Teleport.OnTeleport += OfficialAddon.Teleport_OnTeleport;
        }
    }
}