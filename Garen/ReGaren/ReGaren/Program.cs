using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using SharpDX;
using System;
using System.Linq;
using System.Collections.Generic;
using Color = System.Drawing.Color;
using ReGaren.Utility;
using ReGaren.ReCore;

namespace ReGaren
{
    class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Garen")
            {
                return;
            }

            VersionChecker.Check();
            Loader.Initialize(); // ReCore BETA
            Config.Initialize();
            Drawing.OnDraw += OnDraw;
            Game.OnTick += OnTick;
            Game.OnUpdate += OnTick;
            Drawing.OnEndScene += OnEndScene;

            Chat.Print("ReGaren Yuklendi iyi oyunlar. Ceviri TekinTR;");
        }

        private static void OnEndScene(EventArgs args)
        {
            if (Player.Instance.IsDead || !ConfigList.Drawing.DrawDI)
                return;

            Indicator.Execute();
        }

        private static void OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead || Player.Instance.IsRecalling()) 
                return;

            PermaActive.Execute();
            var flags = Orbwalker.ActiveModesFlags;
            #region Flags checker
            if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                try
                {
                    Combo.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
            }
            if (flags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                try
                {
                    Harass.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
            }
            if (flags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                try
                {
                    LastHit.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
            }
            if (flags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                try
                {
                    Farm.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
            }
            if (flags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                try
                {
                    Farm.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
            }
            if (flags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                try
                {
                    Flee.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
            }
            #endregion
        }

        private static void OnDraw(EventArgs args)
        {
            if (Player.Instance.IsDead)
                return;

            foreach (var spell in SpellManager.AllSpells)
            {
                switch (spell.Slot)
                {
                    case SpellSlot.Q:
                        if (!ConfigList.Drawing.DrawQ)
                            continue;
                        break;
                    case SpellSlot.W:
                        continue;
                    case SpellSlot.E:
                        if (!ConfigList.Drawing.DrawE)
                            continue;
                        break;
                    case SpellSlot.R:
                        if (!ConfigList.Drawing.DrawR)
                            continue;
                        break;
                }
                Circle.Draw(spell.GetColor(), spell.Range, Player.Instance);
            }
        }
    }
}
