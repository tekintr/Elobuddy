﻿#region Licensing
// ---------------------------------------------------------------------
// <copyright file="Quinn.cs" company="EloBuddy">
// 
// Marksman Master
// Copyright (C) 2016 by gero
// All rights reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// <summary>
// 
// Email: geroelobuddy@gmail.com
// PayPal: geroelobuddy@gmail.com
// </summary>
// ---------------------------------------------------------------------
#endregion
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using EloBuddy.SDK.Rendering;
using Marksman_Master.Utils;

namespace Marksman_Master.Plugins.Quinn
{
    internal class Quinn : ChampionPlugin
    {
        protected static Spell.Skillshot Q { get; }
        protected static Spell.Active W { get; }
        protected static Spell.Targeted E { get; }
        protected static Spell.Active R { get; }

        internal static Menu ComboMenu { get; set; }
        internal static Menu HarassMenu { get; set; }
        internal static Menu LaneClearMenu { get; set; }
        internal static Menu DrawingsMenu { get; set; }
        internal static Menu MiscMenu { get; set; }

        private static ColorPicker[] ColorPicker { get; }

        private static bool _changingRangeScan;

        private static readonly Dictionary<int, Dictionary<float, float>> Damages =
            new Dictionary<int, Dictionary<float, float>>();

        protected static byte[] QMana { get; } = { 0, 50, 55, 60, 65, 70 };
        protected static byte EMana { get; } = 50;
        protected static byte[] RMana { get; } = { 0, 100, 50, 0};

        protected static bool IsAfterAttack { get; private set; }
        protected static bool IsPreAttack { get; private set; }

        protected static bool HasWBuff(Obj_AI_Base unit)
            => unit.Buffs.Any(x => x.IsActive && (x.Name.ToLower() == "quinnw"));

        protected static BuffInstance GetWBuff(Obj_AI_Base unit)
            => unit.Buffs.FirstOrDefault(x => x.IsActive && (x.Name.ToLower() == "quinnw"));

        protected static bool HasRBuff => Player.Instance.Buffs.Any(x => x.IsActive && (x.Name.ToLower() == "quinnr"));

        private static readonly Text Text;

        static Quinn()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1025, SkillShotType.Linear, 250, 1550, 60)
            {
                AllowedCollisionCount = 0
            };
            W = new Spell.Active(SpellSlot.W, 2100);
            E = new Spell.Targeted(SpellSlot.E, 760);
            R = new Spell.Active(SpellSlot.R);

            ColorPicker = new ColorPicker[3];

            ColorPicker[0] = new ColorPicker("QuinnQ", new ColorBGRA(10, 106, 138, 255));
            ColorPicker[1] = new ColorPicker("QuinnE", new ColorBGRA(255, 134, 0, 255));
            ColorPicker[2] = new ColorPicker("QuinnHpBar", new ColorBGRA(255, 134, 0, 255));

            DamageIndicator.Initalize(ColorPicker[2].Color, 1400);
            DamageIndicator.DamageDelegate = HandleDamageIndicator;

            ColorPicker[2].OnColorChange +=
                (a, b) =>
                {
                    DamageIndicator.Color = b.Color;
                };

            Orbwalker.OnPostAttack += (sender, args) =>
            {
                IsAfterAttack = true;
                IsPreAttack = false;
            };

            Obj_AI_Base.OnProcessSpellCast += (sender, args) =>
            {
                if (sender.IsMe && (args.Slot == SpellSlot.E))
                {
                    Orbwalker.ResetAutoAttack();
                }

            };

            Orbwalker.OnPreAttack += (target, args) => IsPreAttack = true;
            Game.OnPostTick += args => { IsAfterAttack = false; };

            Text = new Text("", new Font("calibri", 15, FontStyle.Regular));

            ChampionTracker.Initialize(ChampionTrackerFlags.VisibilityTracker);
        }
        
        private static float HandleDamageIndicator(Obj_AI_Base unit)
        {
            if (!Settings.Drawings.DrawDamageIndicator)
            {
                return 0;
            }

            return unit.GetType() != typeof(AIHeroClient) ? 0 : GetComboDamage(unit);
        }

        protected static float GetComboDamage(Obj_AI_Base unit)
        {
            if (Damages.ContainsKey(unit.NetworkId) &&
                !Damages.Any(x => (x.Key == unit.NetworkId) && x.Value.Any(k => Game.Time * 1000 - k.Key > 1000)))
                return Damages[unit.NetworkId].Values.FirstOrDefault();

            var damage = 0f;

            if (unit.IsValidTarget(Q.Range) && Q.IsReady())
                damage += Player.Instance.GetSpellDamage(unit, SpellSlot.Q);

            if (unit.IsValidTarget(E.Range) && E.IsReady())
                damage += Player.Instance.GetSpellDamage(unit, SpellSlot.E);
            
            if (Player.Instance.IsInAutoAttackRange(unit))
                damage += Player.Instance.GetAutoAttackDamage(unit, true)*1.5f * (HasRBuff ? 4 : 3);

            Damages[unit.NetworkId] = new Dictionary<float, float> {{Game.Time*1000, damage}};
            
            return damage;
        }

        protected override void OnDraw()
        {
            if (_changingRangeScan)
                Circle.Draw(SharpDX.Color.White,
                    LaneClearMenu["Plugins.Quinn.LaneClearMenu.ScanRange"].Cast<Slider>().CurrentValue, Player.Instance);

            if (Settings.Drawings.DrawQ && (!Settings.Drawings.DrawSpellRangesWhenReady || Q.IsReady()))
                Circle.Draw(ColorPicker[0].Color, Q.Range, Player.Instance);
            if (Settings.Drawings.DrawE && (!Settings.Drawings.DrawSpellRangesWhenReady || E.IsReady()))
                Circle.Draw(ColorPicker[1].Color, E.Range, Player.Instance);

            if (!Settings.Drawings.DrawDamageIndicator)
                return;

            foreach (var source in EntityManager.Heroes.Enemies.Where(x => x.IsVisible && x.IsHPBarRendered && x.Position.IsOnScreen() && HasWBuff(x)))
            {
                var hpPosition = source.HPBarPosition;
                hpPosition.Y = hpPosition.Y + 30; // tracker friendly.
                var timeLeft = GetWBuff(source).EndTime - Game.Time;
                var endPos = timeLeft * 0x3e8 / 0x25;

                var degree = Misc.GetNumberInRangeFromProcent(timeLeft * 1000d / 4000d * 100d, 3, 110);
                var color = new Misc.HsvColor(degree, 1, 1).ColorFromHsv();

                Text.X = (int)(hpPosition.X + endPos);
                Text.Y = (int)hpPosition.Y + 15; // + text size 
                Text.Color = color;
                Text.TextValue = timeLeft.ToString("F1");
                Text.Draw();

                Drawing.DrawLine(hpPosition.X + endPos, hpPosition.Y, hpPosition.X, hpPosition.Y, 1, color);
            }
        }

        protected override void OnInterruptible(AIHeroClient sender, InterrupterEventArgs args)
        {
            if (!Settings.Misc.EAgainstGapclosers || !E.IsReady() || !sender.IsValidTarget(E.Range))
                return;

            if (args.Delay == 0)
                E.Cast(sender);
            else Core.DelayAction(() => E.Cast(sender), args.Delay);
        }

        protected override void OnGapcloser(AIHeroClient sender, GapCloserEventArgs args)
        {
            if (!Settings.Misc.EAgainstGapclosers || !E.IsReady() || !(args.End.Distance(Player.Instance) < 250) ||
                !sender.IsValidTarget(E.Range))
                return;

            if (args.Delay == 0)
                E.Cast(sender);
            else Core.DelayAction(() => E.Cast(sender), args.Delay);
        }

        protected override void CreateMenu()
        {
            ComboMenu = MenuManager.Menu.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Combo mode settings for Quinn addon");

            ComboMenu.AddLabel("Blinding Assault (Q) settings :");
            ComboMenu.Add("Plugins.Quinn.ComboMenu.UseQ", new CheckBox("Kullan Q"));
            ComboMenu.AddSeparator(5);

            ComboMenu.AddLabel("Heightened Senses (W) settings :");
            ComboMenu.Add("Plugins.Quinn.ComboMenu.UseW", new CheckBox("Kullan W"));
            ComboMenu.AddSeparator(5);

            ComboMenu.AddLabel("Vault (E) settings :");
            ComboMenu.Add("Plugins.Quinn.ComboMenu.UseE", new CheckBox("Kullan E"));
            
            HarassMenu = MenuManager.Menu.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Harass mode settings for Quinn addon");

            HarassMenu.AddLabel("Blinding Assault (Q) settings :");
            HarassMenu.Add("Plugins.Quinn.HarassMenu.UseQ", new CheckBox("Kullan Q", false));
            HarassMenu.Add("Plugins.Quinn.HarassMenu.MinManaQ", new Slider("Gereken en az mana ({0}%) Q kullan icin", 75, 1));

            LaneClearMenu = MenuManager.Menu.AddSubMenu("Clear");
            LaneClearMenu.AddGroupLabel("Lane clear settings for Quinn addon");

            LaneClearMenu.AddLabel("Basic settings :");
            LaneClearMenu.Add("Plugins.Quinn.LaneClearMenu.EnableLCIfNoEn", new CheckBox("Dusman yoksa koridor temizleme aktif", false));
            var scanRange = LaneClearMenu.Add("Plugins.Quinn.LaneClearMenu.ScanRange", new Slider("Dusman uzakligi", 1500, 300, 2500));
            scanRange.OnValueChange += (a, b) =>
            {
                _changingRangeScan = true;
                Core.DelayAction(() =>
                {
                    if (!scanRange.IsLeftMouseDown && !scanRange.IsMouseInside)
                    {
                        _changingRangeScan = false;
                    }
                }, 2000);
            };
            LaneClearMenu.Add("Plugins.Quinn.LaneClearMenu.AllowedEnemies", new Slider("İzin verilen dusman", 1, 0, 5));
            LaneClearMenu.AddSeparator(5);

            LaneClearMenu.AddLabel("Blinding Assault (Q) settings :");
            LaneClearMenu.Add("Plugins.Quinn.LaneClearMenu.UseQInLaneClear", new CheckBox("Koridor temizlemede Q kullan"));
            LaneClearMenu.Add("Plugins.Quinn.LaneClearMenu.MinMinionsKilledForQ", new Slider("Minyonlara son vurusda Q kullan", 3, 1, 6));
            LaneClearMenu.AddSeparator(5);
            LaneClearMenu.Add("Plugins.Quinn.LaneClearMenu.UseQInJungleClear", new CheckBox("Orman temizlemede Q kullan"));
            LaneClearMenu.Add("Plugins.Quinn.LaneClearMenu.MinManaQ", new Slider("Gereken en az mana ({0}%) to use Q", 50, 1));

            MiscMenu = MenuManager.Menu.AddSubMenu("Misc");
            MiscMenu.AddGroupLabel("Misc settings for Quinn addon");
            MiscMenu.AddLabel("Basic settings :");
            MiscMenu.Add("Plugins.Quinn.MiscMenu.EnableKillsteal", new CheckBox("Oldurme sekli aktif"));
            MiscMenu.AddSeparator(5);

            MiscMenu.AddLabel("Vault (E) settings :");
            MiscMenu.Add("Plugins.Quinn.MiscMenu.EAgainstGapclosers", new CheckBox("Kullan E atilma yapan sampiyonlara"));
            MiscMenu.Add("Plugins.Quinn.MiscMenu.EAgainstInterruptible", new CheckBox("Keserken E kullan"));

            MenuManager.BuildAntiGapcloserMenu();
            MenuManager.BuildInterrupterMenu();

            DrawingsMenu = MenuManager.Menu.AddSubMenu("Drawings");
            DrawingsMenu.AddGroupLabel("Drawings settings for Quinn addon");

            DrawingsMenu.AddLabel("Basic settings :");
            DrawingsMenu.Add("Plugins.Quinn.DrawingsMenu.DrawSpellRangesWhenReady", new CheckBox("Sadece hazir olan buyulerin menzilini goster"));
            DrawingsMenu.AddSeparator(5);

            DrawingsMenu.AddLabel("Blinding Assault (Q) settings :");
            DrawingsMenu.Add("Plugins.Quinn.DrawingsMenu.DrawQ", new CheckBox("Goster Q menzili", false));
            DrawingsMenu.Add("Plugins.Quinn.DrawingsMenu.DrawQColor", new CheckBox("Renk sec", false)).OnValueChange += (a, b) =>
            {
                if (!b.NewValue)
                    return;

                ColorPicker[0].Initialize(Color.Aquamarine);
                a.CurrentValue = false;
            };
            DrawingsMenu.AddSeparator(5);

            DrawingsMenu.AddLabel("Vault (E) settings :");
            DrawingsMenu.Add("Plugins.Quinn.DrawingsMenu.DrawE", new CheckBox("Goster E menzili"));
            DrawingsMenu.Add("Plugins.Quinn.DrawingsMenu.DrawEColor", new CheckBox("Renk sec", false)).OnValueChange += (a, b) =>
            {
                if (!b.NewValue)
                    return;

                ColorPicker[1].Initialize(Color.Aquamarine);
                a.CurrentValue = false;
            };

            DrawingsMenu.AddLabel("Damage indicator settings :");
            DrawingsMenu.Add("Plugins.Quinn.DrawingsMenu.DrawDamageIndicator", new CheckBox("Verilicek hasari goster")).OnValueChange += (a, b) =>
            {
                if (b.NewValue)
                    DamageIndicator.DamageDelegate = HandleDamageIndicator;
                else if (!b.NewValue)
                    DamageIndicator.DamageDelegate = null;
            };
            DrawingsMenu.Add("Plugins.Quinn.DrawingsMenu.DamageIndicatorColor", new CheckBox("Renk sec", false)).OnValueChange += (a, b) =>
            {
                if (!b.NewValue)
                    return;

                ColorPicker[2].Initialize(Color.Aquamarine);
                a.CurrentValue = false;
            };
        }

        protected override void PermaActive()
        {
            Modes.PermaActive.Execute();
        }

        protected override void ComboMode()
        {
            Modes.Combo.Execute();
        }

        protected override void HarassMode()
        {
            Modes.Harass.Execute();
        }

        protected override void LaneClear()
        {
            Modes.LaneClear.Execute();
        }

        protected override void JungleClear()
        {
            Modes.JungleClear.Execute();
        }

        protected override void LastHit()
        {
            Modes.LastHit.Execute();
        }

        protected override void Flee()
        {
            Modes.Flee.Execute();
        }

        protected internal static class Settings
        {
            internal static class Combo
            {
                public static bool UseQ => MenuManager.MenuValues["Plugins.Quinn.ComboMenu.UseQ"];

                public static bool UseW => MenuManager.MenuValues["Plugins.Quinn.ComboMenu.UseW"];

                public static bool UseE => MenuManager.MenuValues["Plugins.Quinn.ComboMenu.UseE"];
            }

            internal static class Harass
            {
                public static bool UseQ => MenuManager.MenuValues["Plugins.Quinn.HarassMenu.UseQ"];

                public static int MinManaQ => MenuManager.MenuValues["Plugins.Quinn.HarassMenu.MinManaQ", true];
            }

            internal static class LaneClear
            {
                public static bool EnableIfNoEnemies => MenuManager.MenuValues["Plugins.Quinn.LaneClearMenu.EnableLCIfNoEn"];

                public static int ScanRange => MenuManager.MenuValues["Plugins.Quinn.LaneClearMenu.ScanRange", true];

                public static int AllowedEnemies => MenuManager.MenuValues["Plugins.Quinn.LaneClearMenu.AllowedEnemies", true];

                public static bool UseQInLaneClear => MenuManager.MenuValues["Plugins.Quinn.LaneClearMenu.UseQInLaneClear"];

                public static int MinMinionsKilledForQ => MenuManager.MenuValues["Plugins.Quinn.LaneClearMenu.MinMinionsKilledForQ", true];

                public static bool UseQInJungleClear => MenuManager.MenuValues["Plugins.Quinn.LaneClearMenu.UseQInJungleClear"];

                public static int MinManaQ => MenuManager.MenuValues["Plugins.Quinn.LaneClearMenu.MinManaQ", true];
            }

            internal static class Misc
            {
                public static bool EnableKillsteal => MenuManager.MenuValues["Plugins.Quinn.MiscMenu.EnableKillsteal"];

                public static bool EAgainstGapclosers => MenuManager.MenuValues["Plugins.Quinn.MiscMenu.EAgainstGapclosers"];

                public static bool EAgainstInterruptible => MenuManager.MenuValues["Plugins.Quinn.MiscMenu.EAgainstInterruptible"];
            }

            internal static class Drawings
            {
                public static bool DrawSpellRangesWhenReady => MenuManager.MenuValues["Plugins.Quinn.DrawingsMenu.DrawSpellRangesWhenReady"];

                public static bool DrawQ => MenuManager.MenuValues["Plugins.Quinn.DrawingsMenu.DrawQ"];

                public static bool DrawE => MenuManager.MenuValues["Plugins.Quinn.DrawingsMenu.DrawE"];

                public static bool DrawDamageIndicator => MenuManager.MenuValues["Plugins.Quinn.DrawingsMenu.DrawDamageIndicator"];
            }
        }
    }
}