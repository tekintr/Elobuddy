﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using static Eclipse.SpellsManager;
using static Eclipse.Menus;
using System.Diagnostics;
using EloBuddy.SDK.Menu;
using Eclipse.Managers;

namespace Eclipse.Modes
{
    internal class Active
    {
        public static readonly AIHeroClient Akali = ObjectManager.Player;
        public static bool getKeyBindItem(Menu m, string item)
        {
            return m[item].Cast<KeyBind>().CurrentValue;
        }
        public static void Execute()
        {

            if (Combo._player.IsDead || Combo._player.IsRecalling()) return;

            if (Player.HasBuff("zedulttargetmark") && MiscMenu.GetCheckBoxValue("wlow"))
            {
                if (W.IsReady())
                {
                    W.Cast(Game.CursorPos);
                }
            }

            var target = TargetSelector.GetTarget(Q.Range + 200, DamageType.Magical);

            if (target == null || target.IsInvulnerable || target.MagicImmune)
            {
                return;
            }

            if (MiscMenu.GetCheckBoxValue("autoq") && Q.IsReady() && target.IsValidTarget(Q.Range + 200) && Player.Instance.Mana <= 100)
            {
                Q.Cast(target);
            }

            if (Player.Instance.CountEnemiesInRange(W.Range) >= 2 || Player.Instance.HealthPercent <= 16 && W.IsReady() && MiscMenu.GetCheckBoxValue("wlow"))
            {
                W.Cast(Game.CursorPos);
            }

            if (KillStealMenu.GetCheckBoxValue("qUse")) // Start KS Q
            {
                var qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

                if (qtarget == null) return;

                if (Q.IsReady())
                {
                    var rDamage = DamageManager.GetQDamage(qtarget);
                    if (qtarget.Health + qtarget.AttackShield <= rDamage)
                    {
                        if (qtarget.IsValidTarget(Q.Range))
                        {
                            Q.Cast(qtarget);
                        }
                    }
                }
            }// END KS

            if (KillStealMenu.GetCheckBoxValue("eUse")) // Start KS E
            {
                var etarget = TargetSelector.GetTarget(E.Range, DamageType.Magical);

                if (etarget == null) return;

                if (E.IsReady() && etarget.Health + etarget.AttackShield <= Akali.GetSpellDamage(etarget, SpellSlot.E) && etarget.IsValidTarget(E.Range))
                {
                    E.Cast();
                }
            }// END KS

            if (KillStealMenu.GetCheckBoxValue("rUse")) // Start KS R
            {
                var rtarget = TargetSelector.GetTarget(R.Range, DamageType.Magical);

                if (rtarget == null) return;

                if (R.IsReady())
                {
                    //var passiveDamage = rtarget.HasPassive() ? rtarget.GetPassiveDamage() : 0f;
                    var rDamage = DamageManager.GetRDamage(rtarget);

                    if (rtarget.Health + rtarget.AttackShield <= rDamage)
                    {
                        if (rtarget.IsValidTarget(R.Range))
                        {
                            R.Cast(rtarget);
                        }
                    }
                }
            }// END KS


        }

    }
}
