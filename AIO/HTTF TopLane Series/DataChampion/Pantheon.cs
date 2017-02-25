using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Font = SharpDX.Direct3D9.Font;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;

namespace HTTF_TopLane_Series.DataChampion
{
    internal class Pantheon
    {
        public static Spell.Targeted Q;
        public static Spell.Targeted W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;
        public static Spell.Targeted Ignite;
        public static Item Botrk;
        public static Item Bil;
        public static Item Youmuu;
        public static Font Thm;
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        public static Menu Menu, ComboMenu, ClearMenu, Misc;

        public static void PantheonLoading()
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        // Menu

        private static void OnLoadingComplete(EventArgs args)
        {
            if (!_Player.ChampionName.Contains("Pantheon")) return;
            Chat.Print("HTTF TopLane Series Yuklendi Ceviri TekinTR!", Color.White);
            Bootstrap.Init(null);
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Skillshot(SpellSlot.E, 600, SkillShotType.Cone, 250, 2000, 70);
            E.AllowedCollisionCount = int.MaxValue;
            R = new Spell.Skillshot(SpellSlot.R, 2000, SkillShotType.Circular);
            R.AllowedCollisionCount = int.MaxValue;
            Youmuu = new Item(3142, 10);
            Botrk = new Item(ItemId.Blade_of_the_Ruined_King);
            Bil = new Item(3144, 475f);
            Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
            Menu = MainMenu.AddMenu("HTTF Pantheon", "Pantheon");
            ComboMenu = Menu.AddSubMenu("Kombo Ayarlari", "Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarlari");
            ComboMenu.Add("CQ", new CheckBox("Kullan Q "));
            ComboMenu.Add("CW", new CheckBox("Kullan W "));
            ComboMenu.Add("CE", new CheckBox("Kullan E "));

            ComboMenu.AddGroupLabel("Durtme Ayarlari");
            ComboMenu.Add("HQ", new CheckBox("Kullan Q "));
            ComboMenu.Add("HW", new CheckBox("Kullan W "));
            ComboMenu.Add("HE", new CheckBox("Kullan E "));
            ComboMenu.Add("HM", new Slider("Durtmek icin enaz mana %", 50, 0, 100));
            ComboMenu.AddGroupLabel("Otomatil Durtme Ayarlari");
            ComboMenu.Add("AutoQ", new CheckBox("Otomatik Q kullan"));
            ComboMenu.Add("AutoM", new Slider("Otomatik durtme icin enaz mana", 60, 0, 100));
            ComboMenu.AddGroupLabel("Otomatik Q Kullanilsin");
            foreach (var target in EntityManager.Heroes.Enemies)
            {
                ComboMenu.Add("HarassQ" + target.ChampionName, new CheckBox("" + target.ChampionName));
            }
            ComboMenu.AddGroupLabel("Oldurme Ayarlari");
            ComboMenu.Add("ign", new CheckBox("Oldururken tutustur kullan"));

            ClearMenu = Menu.AddSubMenu("Temizleme Ayarlari", "Clear");
            ClearMenu.AddGroupLabel("Koridor Temizleme Ayarlari");
            ClearMenu.Add("LQ", new CheckBox("Kullan Q "));
            ClearMenu.Add("LW", new CheckBox("Kullan W ", false));
            ClearMenu.Add("LE", new CheckBox("Kullan E ", false));
            ClearMenu.Add("ME", new Slider("En az kac minyon icin [E] kullanilsin", 3, 1, 6));
            ClearMenu.Add("LM", new Slider("Mana ayari", 60, 0, 100));
            ClearMenu.AddGroupLabel("SonVurus Ayari");
            ClearMenu.Add("LHQ", new CheckBox("Kullan Q SonVurus"));
            ClearMenu.Add("LHM", new Slider("Mana ayari", 60, 0, 100));

            ClearMenu.AddGroupLabel("Orman Temizleme ayari");
            ClearMenu.Add("JQ", new CheckBox("Settings Q "));
            ClearMenu.Add("JW", new CheckBox("Settings W "));
            ClearMenu.Add("JE", new CheckBox("Settings E "));
            ClearMenu.Add("JM", new Slider("Mana ayari", 20, 0, 100));


            Misc = Menu.AddSubMenu("Karisik", "Cizimler");
            Misc.AddGroupLabel("Atilma onleyici");
            Misc.Add("antiGap", new CheckBox("Atilma yapana W kullan", false));
            Misc.Add("inter", new CheckBox("Buyu engellemede W kullan"));
            Misc.AddGroupLabel("Cizim Ayarlari");
            Misc.Add("Draw_Disabled", new CheckBox("Cizimleri Kapat", false));
            Misc.Add("Draw", new CheckBox("Goster [Q/W/E] Mesafeleri"));






            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Interupt;
            Orbwalker.OnUnkillableMinion += Orbwalker_CantLasthit;
            Spellbook.OnCastSpell += OnCastSpell;
            Obj_AI_Base.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
            Obj_AI_Base.OnBuffLose += BuffLose;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClear();
            }

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
                LaneClear();
            }

            AutoHarass();


        }

        // Drawings

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_Player.IsDead) return;

            if (Misc["Draw_Disabled"].Cast<CheckBox>().CurrentValue) return;

            if (Misc["Draw"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.BlanchedAlmond, BorderWidth = 2, Radius = E.Range }.Draw(_Player.Position);
            }


        }



        public static bool ECasting
        {
            get { return Player.Instance.HasBuff("PantheonESound"); }
        }

        // OnCastSpell

        private static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!sender.Owner.IsMe)
            {
                return;
            }

            if (args.Slot == SpellSlot.Q || args.Slot == SpellSlot.W)
            {
                if (ECasting)
                {
                    args.Process = false;
                }
            }
        }


        private static void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.Slot == SpellSlot.E)
            {
                Orbwalker.DisableMovement = true;
                Orbwalker.DisableAttacking = true;
            }
        }

        private static void BuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.Buff.DisplayName == "PantheonESound")
            {
                Orbwalker.DisableMovement = false;
                Orbwalker.DisableAttacking = false;
            }
        }

        // Damage Lib

        public static float QDamage(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Physical,
                (float)(new[] { 0, 65, 105, 145, 185, 225 }[Q.Level] + 1.4f * _Player.FlatPhysicalDamageMod));
        }

        public static float WDamage(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 0, 50, 75, 100, 125, 150 }[W.Level] + 1.0f * _Player.FlatMagicDamageMod));
        }

        public static float RDamage(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 0, 200, 350, 500 }[R.Level] + 0.5f * _Player.FlatMagicDamageMod));
        }

        // Interrupt

        public static void Interupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs i)
        {
            var Inter = Misc["inter"].Cast<CheckBox>().CurrentValue;
            if (!sender.IsEnemy || !(sender is AIHeroClient) || Player.Instance.IsRecalling())
            {
                return;
            }

            if (Inter && W.IsReady() && i.DangerLevel == DangerLevel.High && _Player.Distance(sender) <= W.Range)
            {
                W.Cast(sender);
            }
        }

        // AntiGap

        private static void Gapcloser_OnGapCloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            var useW = Misc["antiGap"].Cast<CheckBox>().CurrentValue;
            if (useW && W.IsReady() && sender.IsEnemy && args.Sender.Distance(_Player) <= 325)
            {
                W.Cast(sender);
            }
        }

        //Harass Mode

        private static void Harass()
        {
            var useQ = ComboMenu["HQ"].Cast<CheckBox>().CurrentValue;
            var useW = ComboMenu["HW"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["HE"].Cast<CheckBox>().CurrentValue;
            var mana = ComboMenu["HM"].Cast<Slider>().CurrentValue;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (Player.Instance.ManaPercent <= mana)
            {
                return;
            }

            if (target != null)
            {
                if (useW && W.CanCast(target) && !target.HasBuffOfType(BuffType.Stun))
                {
                    W.Cast(target);
                }

                if (useQ && Q.CanCast(target))
                {
                    Q.Cast(target);
                }

                if (useE && E.CanCast(target))
                {
                    var pred = E.GetPrediction(target);
                    if (!W.IsReady() || !W.IsLearned)
                    {
                        if (pred.HitChance >= HitChance.Medium)
                        {
                            E.Cast(pred.CastPosition);
                        }
                    }

                    else if (target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Knockup))
                    {
                        E.Cast(target.Position);
                    }
                }
            }
        }

        //Combo Mode

        private static void Combo()
        {
            var useQ = ComboMenu["CQ"].Cast<CheckBox>().CurrentValue;
            var useW = ComboMenu["CW"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["CE"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (target != null)
            {
                if (useW && W.CanCast(target) && !target.HasBuffOfType(BuffType.Stun))
                {
                    W.Cast(target);
                }

                if (useQ && Q.CanCast(target))
                {
                    Q.Cast(target);
                }

                if (useE && E.CanCast(target))
                {
                    var pred = E.GetPrediction(target);
                    if (!Q.IsReady() && !W.IsReady())
                    {
                        if (pred.HitChance >= HitChance.Medium)
                        {
                            E.Cast(pred.CastPosition);
                        }
                    }

                    if (target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Knockup))
                    {
                        E.Cast(target.Position);
                    }
                }
            }
        }

        //LaneClear Mode

        private static void LaneClear()
        {
            var useQ = ClearMenu["LQ"].Cast<CheckBox>().CurrentValue;
            var useW = ClearMenu["LW"].Cast<CheckBox>().CurrentValue;
            var useE = ClearMenu["LE"].Cast<CheckBox>().CurrentValue;
            var MinE = ClearMenu["ME"].Cast<Slider>().CurrentValue;
            var mana = ClearMenu["LM"].Cast<Slider>().CurrentValue;
            var minionQ = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(e => e.IsValidTarget(Q.Range));
            var quang = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minionQ, E.Width, (int)E.Range);

            if (Player.Instance.ManaPercent <= mana)
            {
                return;
            }

            if (ECasting)
            {
                return;
            }

            foreach (var minion in minionQ)
            {
                if (useQ && Q.CanCast(minion))
                {
                    if (minion.Health <= QDamage(minion))
                    {
                        Q.Cast(minion);
                    }
                }

                if (useW && W.CanCast(minion))
                {
                    if (minion.Health <= WDamage(minion))
                    {
                        if (useQ)
                        {
                            if (!Q.IsReady())
                            {
                                W.Cast(minion);
                            }
                        }
                        else
                        {
                            W.Cast(minion);
                        }
                    }
                }

                if (useE && E.CanCast(minion))
                {
                    if (quang.HitNumber >= MinE)
                    {
                        if (useQ)
                        {
                            if (!Q.IsReady())
                            {
                                E.Cast(quang.CastPosition);
                            }
                        }
                        else
                        {
                            E.Cast(quang.CastPosition);
                        }
                    }
                }
            }
        }

        // LastHit Mode

        private static void Orbwalker_CantLasthit(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            var useQ = ClearMenu["LHQ"].Cast<CheckBox>().CurrentValue;
            var mana = ClearMenu["LHM"].Cast<Slider>().CurrentValue;
            var unit = (useQ && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && Player.Instance.ManaPercent >= mana);

            if (target == null)
            {
                return;
            }

            if (unit && Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                if (QDamage(target) >= Prediction.Health.GetPrediction(target, Q.CastDelay) && !Orbwalker.IsAutoAttacking)
                {
                    Q.Cast(target);
                }
            }
        }
        // JungleClear Mode

        private static void JungleClear()
        {
            var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(j => j.Health).FirstOrDefault(j => j.IsValidTarget(Q.Range));
            var useQ = ClearMenu["JQ"].Cast<CheckBox>().CurrentValue;
            var useW = ClearMenu["JW"].Cast<CheckBox>().CurrentValue;
            var useE = ClearMenu["JE"].Cast<CheckBox>().CurrentValue;
            var mana = ClearMenu["JM"].Cast<Slider>().CurrentValue;

            if (Player.Instance.ManaPercent <= mana)
            {
                return;
            }

            if (ECasting)
            {
                return;
            }

            if (monster != null)
            {
                if (useQ && Q.CanCast(monster))
                {
                    Q.Cast(monster);
                }

                if (useW && W.CanCast(monster))
                {
                    W.Cast(monster);
                }

                if (useE && E.CanCast(monster))
                {
                    if (useQ && useW)
                    {
                        if (!Q.IsReady() && !W.IsReady())
                        {
                            E.Cast(monster.Position);
                        }
                    }
                    else
                    {
                        E.Cast(monster.Position);
                    }
                }
            }
        }

        public static void DrawFont(Font vFont, string vText, float vPosX, float vPosY, ColorBGRA vColor)
        {
            vFont.DrawText(null, vText, (int)vPosX, (int)vPosY, vColor);
        }



        // Auto Harass

        private static void AutoHarass()
        {
            var useQ = ComboMenu["AutoQ"].Cast<CheckBox>().CurrentValue;
            var mana = ComboMenu["AutoM"].Cast<Slider>().CurrentValue;

            if (Player.Instance.ManaPercent <= mana)
            {
                return;
            }

            foreach (var target in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(Q.Range) && !e.IsDead && !e.IsZombie))
            {
                if (useQ && Q.CanCast(target) && (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)))
                {
                    if (ComboMenu["HarassQ" + target.ChampionName].Cast<CheckBox>().CurrentValue)
                    {
                        Q.Cast(target);
                    }
                }
            }
        }

    }
}