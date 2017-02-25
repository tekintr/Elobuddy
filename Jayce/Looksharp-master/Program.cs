using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using Looksharp.Champions;
using Looksharp.Utilities;

namespace Looksharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            try
            {
                Base Instance = (Base)Activator.CreateInstance(null, "Looksharp.Champions." + Player.Instance.ChampionName).Unwrap();
                Chat.Print("LookSharp => " + Player.Instance.ChampionName + " Yuklendi!");
            }
            catch (Exception)
            {
                Chat.Print("LookSharp => " + Player.Instance.ChampionName + " Desteklemiyor!");
            }
            Load.Init();
        }
    }
}
