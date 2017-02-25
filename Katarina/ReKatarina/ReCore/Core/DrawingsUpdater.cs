using EloBuddy;
using ReKatarina.ReCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReKatarina.ReCore.Core
{
    class DrawingsUpdater
    {
        public static void OnDraw(EventArgs args)
        {
            if (Player.Instance.IsDead) return;
            foreach (var module in SummonerList.modules) module.OnDraw();
            foreach (var module in ItemsList.modules) module.OnDraw();
        }

        public static void OnEndScene(EventArgs args)
        {
            if (Player.Instance.IsDead) return;
            foreach (var module in SummonerList.modules) module.OnEndScene();
            foreach (var module in ItemsList.modules) module.OnEndScene();
        }
    }
}
