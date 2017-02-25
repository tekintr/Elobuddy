﻿using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

namespace HTTF_TopLane_Series.DataChampion
{
    class Poppy
    {
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Skillshot E2;
        public static Spell.Chargeable R;



        public static Menu Menu, ComboMenu, JungleLaneMenu,  MiscMenu, DrawMenu,  AutoPotHealMenu, Humanizer;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }




        public static void PoppyLoading()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }


        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Poppy)
            {
                return;
            }


            Q = new Spell.Skillshot(SpellSlot.Q, 430, SkillShotType.Linear, 250, null, 100);
            Q.AllowedCollisionCount = int.MaxValue;
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Targeted(SpellSlot.E, 425);
            E2 = new Spell.Skillshot(SpellSlot.E, 525, SkillShotType.Linear, 250, 1250);
            R = new Spell.Chargeable(SpellSlot.R, 500, 1200, 4000, 250, int.MaxValue, 90);





            Menu = MainMenu.AddMenu("Poppy HTTF", "Poppy");


            ComboMenu = Menu.AddSubMenu("Kombo Ayarlari", "Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarlari");
            ComboMenu.Add("useQCombo", new CheckBox("Kullan Q"));
            ComboMenu.Add("useWCombo", new CheckBox("Kullan W"));
            ComboMenu.Add("useECombo", new CheckBox("Kullan E"));
            ComboMenu.Add("useRcombo", new CheckBox("Kullan R"));
            ComboMenu.Add("combo.REnemies", new Slider("R kullanmak icin dusman sayisi", 1, 1, 5));
            ComboMenu.AddGroupLabel("Durtme Ayarlari");
            ComboMenu.Add("useQHarass", new CheckBox("Use Q"));
            ComboMenu.Add("useWHarassMana", new Slider("Durtmek icin enaz mana %", 70, 0, 100));
            ComboMenu.AddGroupLabel("E Ayarlari");
            ComboMenu.Add("useEwall", new CheckBox("Kullan E duvara her zaman?"));
            ComboMenu.Add("useEeq", new CheckBox("Kullan E kacarken ?"));

            JungleLaneMenu = Menu.AddSubMenu("Temizleme Ayarlari", "FarmSettings");
            JungleLaneMenu.AddGroupLabel("Koridor temizleme");
            JungleLaneMenu.Add("useQFarm", new CheckBox("Kullan Q"));
            JungleLaneMenu.Add("useWManalane", new Slider("Koridor temizleme icin enaz mana %", 70, 0, 100));
            JungleLaneMenu.AddLabel("Orman temizleme");
            JungleLaneMenu.Add("useQJungle", new CheckBox("Kullan Q"));
            JungleLaneMenu.Add("useWMana", new Slider("Orman temizleme icin enaz mana %", 70, 0, 100));


            MiscMenu = Menu.AddSubMenu("Karisik ayarlar", "KarisikAyarlar");
            MiscMenu.AddGroupLabel("Atilma onleyici ve skill enlleyici  ayarlari");
            MiscMenu.Add("gapcloser", new CheckBox("Otomatik W  Atilma yapana"));
            MiscMenu.Add("gapcloserE", new CheckBox("Otomatik E  Atilma yapana"));
            MiscMenu.Add("InterruptE", new CheckBox("Otomatik E  gelen skilli engellemede"));
            MiscMenu.Add("interruptR", new CheckBox("Otomatik R  gelen skilli engellemede"));
            MiscMenu.AddGroupLabel("Flee Settings");
            MiscMenu.Add("FleeW", new CheckBox("Use W"));



            DrawMenu = Menu.AddSubMenu("Cizim ayarlari");
            DrawMenu.AddGroupLabel("Cizim ayarlari");
            DrawMenu.Add("drawQ", new CheckBox("Goster Q Mesafesi"));
            DrawMenu.Add("drawW", new CheckBox("Goster W Mesafesi"));
            DrawMenu.Add("drawE", new CheckBox("Goster E Mesafesi"));
            DrawMenu.Add("drawR", new CheckBox("Goster R Mesafesi"));


            Game.OnUpdate += OnGameUpdate;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        public static
            void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
            {
                new Circle { Color = Color.DeepSkyBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue)
            {
                new Circle { Color = Color.DeepSkyBlue, Radius = W.Range, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue)
            {
                new Circle { Color = Color.DeepSkyBlue, Radius = E.Range, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
            {
                new Circle { Color = Color.DeepSkyBlue, Radius = R.Range, BorderWidth = 2f }.Draw(_Player.Position);
            }

            
        }

        public static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (MiscMenu["gapcloser"].Cast<CheckBox>().CurrentValue && sender.IsEnemy &&
                e.End.Distance(_Player) <= 600)
            {
                W.Cast();
            }
            if (MiscMenu["gapcloserE"].Cast<CheckBox>().CurrentValue && sender.IsEnemy &&
                e.End.Distance(_Player) < E.Range)
            {
                E.Cast(e.Sender);
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            var useE = MiscMenu["interruptE"].Cast<CheckBox>().CurrentValue;
            var useR = MiscMenu["interruptR"].Cast<CheckBox>().CurrentValue;

            {
                if (useE)
                {
                    if (sender.IsEnemy && E.IsReady() && e.DangerLevel == DangerLevel.High &&
                        sender.Distance(_Player) <= E.Range)
                    {
                        E.Cast(sender);
                    }
                    else if (useR)
                    {
                        if (sender.IsEnemy && R.IsReady() && e.DangerLevel == DangerLevel.High &&
                            sender.Distance(_Player) <= R.Range)
                        {
                            if (R.IsCharging)
                            {
                                R.Cast(sender);
                                return;
                            }
                            R.StartCharging();
                        }
                    }
                }
            }
        }



        

        public static
            void OnGameUpdate(EventArgs args)
        {
            Orbwalker.ForcedTarget = null;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();

            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                WaveClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None))
            {

            }
        }


        public static void Flee()
        {
            var fleeW = MiscMenu["FleeW"].Cast<CheckBox>().CurrentValue;
            if (fleeW && W.IsReady())
            {
                W.Cast();
            }
        }

        


        public static
            void JungleClear()
        {
            var useQ = JungleLaneMenu["useQJungle"].Cast<CheckBox>().CurrentValue;
            var junglemana = JungleLaneMenu["useWMana"].Cast<Slider>().CurrentValue;

            {
                var junleminions =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters()
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault(a => a.IsValidTarget(900));

                if (useQ && _Player.ManaPercent > junglemana && Q.IsReady() && junleminions.IsValidTarget(Q.Range))
                {
                    Q.Cast(junleminions);
                }
            }
        }

        public static void WaveClear()
        {
            var useQ = JungleLaneMenu["useQFarm"].Cast<CheckBox>().CurrentValue;
            var lanemana = JungleLaneMenu["useWManalane"].Cast<Slider>().CurrentValue;


            var count =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, _Player.ServerPosition,
                    _Player.AttackRange, false).Count();
            var source =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderBy(a => a.MaxHealth)
                    .FirstOrDefault(a => a.IsValidTarget(Q.Range));
            if (count == 0) return;
            if (useQ && _Player.ManaPercent > lanemana && Q.IsReady())
            {
                Q.Cast(source);
            }
        }

        public static
            void Harass()
        {
            var wmana = ComboMenu["useWHarassMana"].Cast<Slider>().CurrentValue;
            var qharass = ComboMenu["useQHarass"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (Q.IsReady() && qharass && target.IsValidTarget(Q.Range) && _Player.ManaPercent >= wmana)
            {
                var predQ = (Q.GetPrediction(target));
                if (predQ.HitChance >= HitChance.High)
                {
                    Q.Cast(predQ.CastPosition);
                }
            }
        }

        public static void Combo()
        {
            var useQ = ComboMenu["useQcombo"].Cast<CheckBox>().CurrentValue;
            var useW = ComboMenu["useWcombo"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["useEcombo"].Cast<CheckBox>().CurrentValue;
            var useR = ComboMenu["useRcombo"].Cast<CheckBox>().CurrentValue;
            var rEnemies = ComboMenu["combo.REnemies"].Cast<Slider>().CurrentValue;
            var target = TargetSelector.GetTarget(1400, DamageType.Magical);
            if (!target.IsValidTarget(Q.Range) || target == null)
            {
                return;
            }
            if (useE && E.IsReady() && target.IsValidTarget(E.Range))
            {
                if (Wall(target))
                {
                    E.Cast(target);
                }
            }
            if (useQ && Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                var predQ = (Q.GetPrediction(target));
                if (predQ.HitChance >= HitChance.High)
                {
                    Q.Cast(predQ.CastPosition);
                }
            }
            if (useW && W.IsReady() && target.IsValidTarget(W.Range))
            {
                W.Cast();
            }
            if (useR && _Player.CountEnemiesInRange(R.Range) == rEnemies && R.IsReady() && target.IsValidTarget(R.Range))
            {
                var predR = (R.GetPrediction(target));
                if (predR.HitChance >= HitChance.High)
                {
                    if (R.IsCharging)
                    {
                        R.Cast(target.Position);
                        return;
                    }
                    R.StartCharging();
                }
            }
        }

        private static bool Wall(Obj_AI_Base target)
        {
            var distance = target.BoundingRadius + target.ServerPosition.Extend(Player.Instance.ServerPosition, -355);

            if (distance.IsWall())
            {
                return true;
            }

            return false;
        }



        public static float ComboDamage(Obj_AI_Base hero)
        {
            var damage = _Player.GetAutoAttackDamage(hero);
            if (R.IsReady())
                damage = _Player.GetSpellDamage(hero, SpellSlot.R);
            if (E.IsReady())
                damage = _Player.GetSpellDamage(hero, SpellSlot.E);
            if (W.IsReady())
                damage = _Player.GetSpellDamage(hero, SpellSlot.W);
            if (Q.IsReady())
                damage = _Player.GetSpellDamage(hero, SpellSlot.Q);

            return damage;
        }


    }
}