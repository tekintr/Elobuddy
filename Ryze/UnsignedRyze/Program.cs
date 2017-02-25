using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Linq;
using System.Collections.Generic;

namespace UnsignedRyze
{
    internal class Program
    {
        public static Menu ComboMenu, DrawingsMenu, SettingsMenu, LaneClear, LastHit, Killsteal, Harass, menu;
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Skillshot R;
        public static Spell.Targeted Ignite;
        public static int PassiveStacks
        {
            get
            {
                int stacks = 0;
                if(_Player.HasBuff("pyromania"))
                    stacks = _Player.GetBuff("pyromania").Count;
                return stacks;
            } 
        }
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static int Mana { get { return (int)_Player.Mana; } }
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Ryze")
                return;

            //Hacks.AntiAFK = true;
            Bootstrap.Init(null);

            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 100, DamageType.Magical);
            W = new Spell.Targeted(SpellSlot.W, 615, DamageType.Magical);
            E = new Spell.Targeted(SpellSlot.E, 615, DamageType.Magical);
            R = new Spell.Skillshot(SpellSlot.R, 1750, SkillShotType.Circular);

            menu = MainMenu.AddMenu("Unsigned Ryze", "UnsignedRyze");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("QU", new CheckBox("Kullan Q"));
            ComboMenu.Add("WU", new CheckBox("Kullan W"));
            ComboMenu.Add("EU", new CheckBox("Kullan E"));
            //ComboMenu.Add("RU", new CheckBox("Use R"));
            ComboMenu.Add("IU", new CheckBox("Kullan Item"));
            ComboMenu.Add("IgU", new CheckBox("Kullan Tutustur"));

            LaneClear = menu.AddSubMenu("Lane Clear", "laneclear");
            LaneClear.AddGroupLabel("Lane Clear Settings");
            LaneClear.Add("LCQ", new CheckBox("Kullan Q"));
            LaneClear.Add("LCE", new CheckBox("Kullan E"));

            Harass = menu.AddSubMenu("Harass", "harass");
            Harass.AddGroupLabel("Harass Settings");
            Harass.Add("HQ", new CheckBox("Kullan Q"));
            Harass.Add("HE", new CheckBox("Kullan E"));

            LastHit = menu.AddSubMenu("Last Hit", "lasthitmenu");
            LastHit.AddGroupLabel("Last Hit Settings");
            LastHit.Add("LHQ", new CheckBox("Kullan Q"));
            LastHit.Add("LHE", new CheckBox("Kullan E"));

            Killsteal = menu.AddSubMenu("Killsteal", "killstealmenu");
            Killsteal.AddGroupLabel("Killsteal Settings");
            Killsteal.Add("KSER", new CheckBox("Oldürme aktif"));
            Killsteal.Add("KSQ", new CheckBox("Kullan Q"));
            Killsteal.Add("KSW", new CheckBox("Kullan W"));
            Killsteal.Add("KSE", new CheckBox("Kullan E"));
            Killsteal.Add("KSI", new CheckBox("Kullan tutustur"));

            DrawingsMenu = menu.AddSubMenu("Drawings", "drawingsmenu");
            DrawingsMenu.AddGroupLabel("Drawings Settings");
            DrawingsMenu.Add("DQ", new CheckBox("Göster Q"));
            DrawingsMenu.Add("DWE", new CheckBox("Göster W/E"));
            DrawingsMenu.Add("DR", new CheckBox("Göster R"));
            
            SettingsMenu = menu.AddSubMenu("Settings", "settingsmenu");
            SettingsMenu.AddGroupLabel("Settings");
            SettingsMenu.Add("SHM", new CheckBox("Otomatik iksir kullan"));
            SettingsMenu.Add("ST", new CheckBox("Basede gözyası kas"));

            SpellDataInst Sum1 = _Player.Spellbook.GetSpell(SpellSlot.Summoner1);
            SpellDataInst Sum2 = _Player.Spellbook.GetSpell(SpellSlot.Summoner2);
            if (Sum1.Name == "SummonerDot")
                Ignite = new Spell.Targeted(SpellSlot.Summoner1, 600);
            else if (Sum2.Name == "SummonerDot")
                Ignite = new Spell.Targeted(SpellSlot.Summoner2, 600);
            
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Chat.Print(Sum1.Name);
            Chat.Print(Sum2.Name);
        }
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
            {
                Drawing.DrawCircle(_Player.Position, Q.Range, System.Drawing.Color.BlueViolet);
            }

            if (DrawingsMenu["DWE"].Cast<CheckBox>().CurrentValue && (E.IsLearned || W.IsLearned))
            {
                Drawing.DrawCircle(_Player.Position, E.Range, System.Drawing.Color.BlueViolet);
            }

            if (DrawingsMenu["DR"].Cast<CheckBox>().CurrentValue && R.IsLearned)
            {
                if(R.Level == 1)
                    Drawing.DrawCircle(_Player.Position, 0, System.Drawing.Color.BlueViolet);
                if (R.Level == 2)
                    Drawing.DrawCircle(_Player.Position, 0, System.Drawing.Color.BlueViolet);
            }

            foreach (Obj_AI_Base minion in ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy && !a.IsDead && a.IsInRange(_Player, 0)))
                Drawing.DrawCircle(minion.Position, 0, System.Drawing.Color.Aqua);

        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                RyzeFunctions.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                RyzeFunctions.LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                RyzeFunctions.Harrass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                RyzeFunctions.LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                RyzeFunctions.Flee();
            }
            if (SettingsMenu["ST"].Cast<CheckBox>().CurrentValue)
            {
                RyzeFunctions.StackMode();
            }
            if (Killsteal["KSER"].Cast<CheckBox>().CurrentValue)
            {
                RyzeFunctions.KillSteal();
            }
            if (SettingsMenu["SHM"].Cast<CheckBox>().CurrentValue)
            {
                RyzeFunctions.UseItems();
            }
        }
    }
}
