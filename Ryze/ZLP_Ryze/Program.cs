using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace ZLP_Ryze
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        public static void OnLoad(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Ryze") return;

            Chat.Print("ZLP Ryze Yuklendi. Ceviri TekinTR!");

            Menus.Initialize();
            Spells.Initialize();

            Game.OnTick += OnTick;
            Game.OnTick += Spells.Ultimate;
            Game.OnTick += Modes.Escape;
            Game.OnTick += Modes.AutoHarass;
            Game.OnTick += Modes.KillSteal;
            Game.OnTick += Modes.Stack;
            Game.OnTick += More.StopAuto;

            Orbwalker.OnUnkillableMinion += Modes.LastHit;
            Gapcloser.OnGapcloser += Modes.OnGap;
            Obj_AI_Base.OnProcessSpellCast += More.OnCast;
            Drawing.OnDraw += Drawings.OnDraw;
        }

        private static void OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.IsRecalling()) return;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                Modes.Combo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                Modes.Harass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                Modes.LaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                Modes.JungleClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                Modes.Flee();
        }
    }
}