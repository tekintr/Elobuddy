using EloBuddy.SDK.Events;
using System;
using EloBuddy;

namespace Aka_s_Vayne
{
    class Program
    {
        private static void Main(string[] args1)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Variables._Player.ChampionName != "Vayne") return;
            AkaCore.Program.Load();
            Manager.Manager.Load();
            Chat.Print("Aka´s Vayne Yuklendi! Yapimci Aka.Ceviri TekinTR v7.2");
        }
    }
}
