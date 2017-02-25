using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using ReKatarina.Utility;
using ReKatarina.ReCore;
using System;
using SharpDX;

namespace ReKatarina
{
    class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Katarina") return;
            
            VersionChecker.Check();
            Loader.Initialize(); // ReCore BETA
            Humanizer.Initialize();
            Config.Initialize();
            Drawing.OnDraw += OnDraw;
            Game.OnTick += OnTick;
            Game.OnUpdate += OnTick;
            Orbwalker.OnUnkillableMinion += LastHit.OnUnkillableMinion;
            Drawing.OnEndScene += OnEndScene;
            Obj_AI_Base.OnPlayAnimation += Obj_AI_Base_OnPlayAnimation;
            Player.OnIssueOrder += Player_OnIssueOrder;

            Chat.Print("<font color='#FFFFFF'>TekinTR Tarafindan <font color='#CF2942'>Turkce Yapildi</font> v." + VersionChecker.AssVersion + " has been loaded.</font>");
        }

        private static void Player_OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (sender.IsMe && Damage.HasRBuff()) args.Process = false;
        }

        private static void OnEndScene(EventArgs args)
        {
            if (Player.Instance.IsDead || !ConfigList.Drawing.DrawDI)
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

        private static void Obj_AI_Base_OnPlayAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe || !Damage.HasRBuff() || (args.Animation != "Spell4"))
                return;

            Orbwalker.DisableAttacking = true;
            Orbwalker.DisableMovement = true;
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot == SpellSlot.R)
            {
                Damage.FreezePlayer();
            }

            if (SpellManager.CastingUlt && Player.Instance.Spellbook.IsChanneling)
            {
                args.Process = false;
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (Player.Instance.IsDead)
                return;

            if (Game.CursorPos.IsValid() && ConfigList.Drawing.DrawCJ)
                Circle.Draw(SharpDX.Color.Aqua, ConfigList.Flee.JumpCursorRange, Game.CursorPos);

            foreach (var spell in SpellManager.AllSpells)
            {
                switch (spell.Slot)
                {
                    case SpellSlot.Q:
                        if (!ConfigList.Drawing.DrawQ)
                            continue;
                        break;
                    case SpellSlot.W:
                        if (!ConfigList.Drawing.DrawW)
                            continue;
                        break;
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

            if (ConfigList.Drawing.DrawDagger)
            {
                foreach (var dagger in Dagger.GetDaggers())
                {
                    if (dagger.CountEnemyChampionsInRange(375) > 0 || dagger.CountEnemyMinionsInRange(375) > 0) Circle.Draw(Color.Green, 150, dagger);
                    else Circle.Draw(Color.Red, 150, dagger);
                }
            }
        }
    }
}
