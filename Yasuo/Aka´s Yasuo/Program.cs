using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Aka_s_Yasuo
{
    class Program
    {
        private static void Main(string[] args1)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Variables._Player.ChampionName != "Yasuo") return;
            Chat.Print("Aka´s Yasuo yuklendi! Yapimci Aka.Ceviri TekinTR");
            Manager.Manager.Load();
            AkaCore.Program.Load();
        }
    }
}
