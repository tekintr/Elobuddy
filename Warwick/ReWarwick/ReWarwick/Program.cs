using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using ReWarwick.Modes;
using ReWarwick.ReCore;
using System;
using ReWarwick.Utils;

namespace ReWarwick
{
    class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Warwick") return;
            
            //VersionChecker.Check();
            Loader.Initialize(); // ReCore BETA
            Humanizer.Initialize();
            MenuLoader.Initialize();
            Drawing.OnDraw += OnDraw;
            Game.OnTick += OnTick;
            Game.OnUpdate += OnTick;
            Orbwalker.OnUnkillableMinion += LastHit.OnUnkillableMinion;
            Drawing.OnEndScene += OnEndScene;

            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;

            Chat.Print("<font color='#FFFFFF'>ReWarwick v." + VersionChecker.AssVersion + " Yuklendi Ceviri TekinTR.</font>");
        }

        private static void OnEndScene(EventArgs args)
        {
            if (Player.Instance.IsDead || !Config.Drawing.Menu.GetCheckBoxValue("Config.Drawing.Indicator"))
                return;

            Indicator.Execute();
        }

        public static void OnTick(EventArgs args)
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
                    LaneClear.Execute();
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
                    JungleClear.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
            }
            if (Config.Combo.Menu.GetKeyBindValue("Config.Combo.R.Force"))
            {
                if (!Player.Instance.HasBuff("WarwickR") && Config.Combo.Menu.GetCheckBoxValue("Config.Combo.R.OrbWalk"))
                    Orbwalker.OrbwalkTo(Game.CursorPos);
                Other.ForceRUsage();
            }
            #endregion
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!Config.Misc.Menu.GetCheckBoxValue("Config.Misc.Another.Gapcloser") || !sender.IsValidTarget(SpellManager.E.Range)) return;
            
            if (SpellManager.E.IsReady() && sender.IsInRange(Player.Instance, SpellManager.E.Range))
            {
                Core.DelayAction(() => SpellManager.E.Cast(), Config.Misc.Menu.GetSliderValue("Config.Misc.Another.Delay"));
                Core.DelayAction(() => SpellManager.E.CastE2(), Config.Misc.Menu.GetSliderValue("Config.Misc.Another.Delay") + 1000);
                return;
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!Config.Misc.Menu.GetCheckBoxValue("Config.Misc.Another.Interrupter") || !sender.IsValidTarget(SpellManager.E.Range)) return;

            if (SpellManager.E.IsReady() && sender.IsInRange(Player.Instance, SpellManager.E.Range))
            {
                Core.DelayAction(() => SpellManager.E.Cast(), Config.Misc.Menu.GetSliderValue("Config.Misc.Another.Delay"));
                Core.DelayAction(() => SpellManager.E.CastE2(), Config.Misc.Menu.GetSliderValue("Config.Misc.Another.Delay") + 1000);
                return;
            }
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
                        if (!Config.Drawing.Menu.GetCheckBoxValue("Config.Drawing.Q")) continue;
                        break;
                    case SpellSlot.W:
                        if (!Config.Drawing.Menu.GetCheckBoxValue("Config.Drawing.W")) continue;
                        break;
                    case SpellSlot.E:
                        if (!Config.Drawing.Menu.GetCheckBoxValue("Config.Drawing.E")) continue;
                        break;
                    case SpellSlot.R:
                        if (!Config.Drawing.Menu.GetCheckBoxValue("Config.Drawing.R")) continue;
                        break;
                }
                Circle.Draw(spell.GetColor(), spell.Range, Player.Instance);
            }
        }
    }
}
