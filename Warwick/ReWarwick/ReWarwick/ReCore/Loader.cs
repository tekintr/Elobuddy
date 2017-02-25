using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReWarwick.ReCore.Config;
using ReWarwick.ReCore.Managers;
using ReWarwick.ReCore.Utility;
using System;

namespace ReWarwick.ReCore
{
    class Loader
    {
        public static readonly Menu Menu;
        public static readonly CheckBox Status;
        public static System.Version AssVersion { get { return System.Version.Parse("7.1.4"); } }

        static Loader()
        {
            Menu = MainMenu.AddMenu("ReCore", "ReCore");
            Menu.AddGroupLabel("Special thanks to MarioGK for his Mario's Lib.");

            SummonerManager.Initialize();
            ItemManager.Initialize();
            SummonerList.Initialize();
            ItemsList.Initialize();
            Utility.Initialize();
            DangerManager.Initialize();

            Game.OnTick += OnTick;
            Game.OnUpdate += OnTick;
            Drawing.OnDraw += Core.DrawingsUpdater.OnDraw;
            Drawing.OnEndScene += Core.DrawingsUpdater.OnEndScene;

            Menu.AddGroupLabel("Welcome to ReCore v." + AssVersion + " [BETA].");
            Status = Menu.Add("enableReCore", new CheckBox("Enable [NOT RECOMMEND]", false));
            Chat.Print("<font color='#FFFFFF'>ReCore v." + AssVersion + "</font> <font color='#CF2942'>[BETA]</font> <font color='#FFFFFF'>has been loaded.</font>");
        }

        private static void OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.IsRecalling() || !Status.CurrentValue || !TickLimiter.Check())
                return;

            Core.SummonerUpdater.Update();
            Core.ItemUpdater.Update();
        }

        public static void Initialize()
        {
        }

        public static class Utility
        {
            static Utility()
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
