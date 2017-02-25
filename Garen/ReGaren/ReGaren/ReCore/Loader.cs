using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReGaren.ReCore.ConfigList;
using ReGaren.ReCore.Managers;
using ReGaren.ReCore.Utility;
using System;

namespace ReGaren.ReCore
{
    class Loader
    {
        public static readonly Menu Menu;
        public static readonly CheckBox Status;
        public static System.Version AssVersion { get { return System.Version.Parse("7.1.1"); } }

        static Loader()
        {
            Menu = MainMenu.AddMenu("ReCore", "ReCore");
            Menu.AddGroupLabel("Special thanks to MarioGK for his Mario's Lib.");

            SummonerManager.Initialize();
            ItemManager.Initialize();
            SummonerList.Initialize();
            ItemsList.Initialize();
            Modes.Initialize();
            DangerManager.Initialize();

            Game.OnTick += OnTick;
            Game.OnUpdate += OnTick;
            Drawing.OnDraw += Core.DrawingsUpdater.OnDraw;
            Drawing.OnEndScene += Core.DrawingsUpdater.OnEndScene;

            Menu.AddGroupLabel("Welcome to ReCore v." + AssVersion + " [BETA].");
            Status = Menu.Add("enableReCore", new CheckBox("Aktif et [Tavsiye Etmem]", false));
            Chat.Print("<font color='#FFFFFF'>ReCore v." + AssVersion + "</font> <font color='#CF2942'>[BETA]</font> <font color='#FFFFFF'>has been loaded.</font>");
        }

        private static void OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.IsRecalling() || !Status.CurrentValue)
                return;

            Core.SummonerUpdater.Update();
            Core.ItemUpdater.Update();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            static Modes()
            {
                // Menu
                Summoners.Initialize();
                OItems.Initialize();
                DItems.Initialize();
                CItems.Initialize();
                Protector.Initialize();
                Cleansers.Initialize();
                Settings.Initialize();
            }

            public static void Initialize()
            {
            }
        }
    }
}
