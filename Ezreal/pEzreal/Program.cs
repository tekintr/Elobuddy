using System;
using EloBuddy.SDK.Events;
using pEzreal.Extensions;

namespace pEzreal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += GameLoaded;
        }

        private static void GameLoaded(EventArgs args)
        {
            if (Config.MyHero.ChampionName != "Ezreal") return;

            Events.Initialize();
            Config.Initialize();
            Spells.Initialize();
        }
    }
}
