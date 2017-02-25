using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace pEzreal.Extensions
{
    internal class Events
    {
        public static void Initialize()
        {
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Config.SkinChanger && Config.MyHero.SkinId != Config.SkinId)
                Config.MyHero.SetSkinId(Config.SkinId);

            var currentModes = Orbwalker.ActiveModesFlags.ToString();

            if (currentModes.Contains(Orbwalker.ActiveModes.Combo.ToString()))
                Modes.Combo.Execute();

            if (currentModes.Contains(Orbwalker.ActiveModes.Harass.ToString()))
                Modes.Harass.Execute();

            if (currentModes.Contains(Orbwalker.ActiveModes.LastHit.ToString()))
                Modes.Lasthit.Execute();

            if (currentModes.Contains(Orbwalker.ActiveModes.LaneClear.ToString()))
                Modes.LaneClear.Execute();

            if (currentModes.Contains(Orbwalker.ActiveModes.JungleClear.ToString()))
                Modes.JungleClear.Execute();

            Modes.Active.Initialize();
        }

        private static void OnDraw(EventArgs args)
        {
            if (Config.MyHero.IsDead) return;
            if (Config.DrawQ && (Config.Ready && Spells.Q.IsReady()) || !Config.Ready)
                Circle.Draw(Color.LightBlue, Spells.Q.Range, Config.MyHero);

            if (Config.DrawW && (Config.Ready && Spells.W.IsReady()) || !Config.Ready)
                Circle.Draw(Color.LightGoldenrodYellow, Spells.W.Range, Config.MyHero);

            if (Config.DrawE && (Config.Ready && Spells.E.IsReady()) || !Config.Ready)
                Circle.Draw(Color.LightPink, Spells.E.Range, Config.MyHero);

            if (Config.DrawR && (Config.Ready && Spells.R.IsReady()) || !Config.Ready)
                Circle.Draw(Color.LightSalmon, Spells.R.Range, Config.MyHero);
        }
    }
}