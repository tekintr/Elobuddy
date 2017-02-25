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

namespace Malphite
{
    internal class Program
    {
        public static Menu Menu, ComboMenu, HarassMenu, JungleClearMenu, LaneClearMenu, KillStealMenu, Drawings;
        public static Font Thm;
        public static Font Thn;
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        public static Spell.Targeted Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Skillshot R;
        public static Spell.Targeted Ignite;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        static void OnLoadingComplete(EventArgs args)
        {
            if (!_Player.ChampionName.Contains("Malphite")) return;
            Chat.Print("Doctor's Malphite Yuklendi Ceviri TekinTR!", Color.Orange);
            Bootstrap.Init(null);
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Active(SpellSlot.W, 250);
            E = new Spell.Active(SpellSlot.E, 400);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Circular, 250, 700, 270);
            Thm = new Font(Drawing.Direct3DDevice, new FontDescription { FaceName = "Tahoma", Height = 32, Weight = FontWeight.Bold, OutputPrecision = FontPrecision.Default, Quality = FontQuality.ClearType });
            Thn = new Font(Drawing.Direct3DDevice, new FontDescription { FaceName = "Tahoma", Height = 20, Weight = FontWeight.Bold, OutputPrecision = FontPrecision.Default, Quality = FontQuality.ClearType });
            Ignite = new Spell.Targeted(_Player.GetSpellSlotFromName("summonerdot"), 600);
            Menu = MainMenu.AddMenu("Malphite", "Malphite");
            Menu.AddGroupLabel("Doctor7");
            ComboMenu = Menu.AddSubMenu("Kombo Ayarlari", "Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarlari");
            ComboMenu.Add("ComboQ", new CheckBox("Kullan [Q]"));
            ComboMenu.Add("ComboW", new CheckBox("Kullan [W]"));
            ComboMenu.Add("ComboE", new CheckBox("Kullan [E]"));
            ComboMenu.Add("DisQ", new Slider("Kullan [Q] dusman uzakligi >", 10, 0, 650));
            ComboMenu.AddLabel("[Q] Uzaklik < 125 = Herzaman [Q]");
            ComboMenu.AddGroupLabel("Ulti Ayarlari");
            ComboMenu.Add("ComboFQ", new KeyBind("Kullan [R] Secilen hedefe", false, KeyBind.BindTypes.HoldActive, 'T'));
            ComboMenu.Add("ComboR", new CheckBox("Kullan [R] Aoe"));
            ComboMenu.Add("MinR", new Slider("Kac dusmana isabet etsin [R] Aoe", 3, 1, 5));
            ComboMenu.AddGroupLabel("Buyu kesme Ayarlari");
            ComboMenu.Add("inter", new CheckBox("Kullan [R]", false));

            HarassMenu = Menu.AddSubMenu("Durtme Ayarlari", "Harass");
            HarassMenu.AddGroupLabel("Durtme Ayarlari");
            HarassMenu.Add("HarassQ", new CheckBox("Kullan [Q] "));
            HarassMenu.Add("HarassW", new CheckBox("Kullan [W] "));
            HarassMenu.Add("HarassE", new CheckBox("Kullan [E] "));
            HarassMenu.Add("DisQ2", new Slider("Kullan [Q] dusman uzakligi >", 350, 0, 650));
            HarassMenu.AddLabel("[Q] Uzaklik < 125 = Herzaman [Q]");
            HarassMenu.Add("ManaQ", new Slider("Durtmek icin mana", 40));

            JungleClearMenu = Menu.AddSubMenu("Orman Ayarlari", "JungleClear");
            JungleClearMenu.AddGroupLabel("Orman Ayarlari");
            JungleClearMenu.Add("QJungle", new CheckBox("Kullan [Q]"));
            JungleClearMenu.Add("WJungle", new CheckBox("Kullan [W]"));
            JungleClearMenu.Add("EJungle", new CheckBox("Kullan [E]"));
            JungleClearMenu.Add("JungleMana", new Slider("Orman Temizlemek icin mana", 20));

            LaneClearMenu = Menu.AddSubMenu("Koridor Ayarlari", "LaneClear");
            LaneClearMenu.AddGroupLabel("Koridor Temizleme Ayarlari");
            LaneClearMenu.Add("LaneClearQ", new CheckBox("Kullan [Q] "));
            LaneClearMenu.Add("LaneClearW", new CheckBox("Kullan [W] "));
            LaneClearMenu.Add("LaneClearE", new CheckBox("Kullan [E] "));
            LaneClearMenu.Add("ManaLC", new Slider("Koridor temizlemek icin mana", 50));
            LaneClearMenu.AddGroupLabel("SonVurus Ayarlari");
            LaneClearMenu.Add("LastHitQ", new CheckBox("Kullan [Q] SonVurus"));
            LaneClearMenu.Add("ManaLH", new Slider("SonVurus icin mana", 50));

            KillStealMenu = Menu.AddSubMenu("Oldurme Ayarlari", "KillSteal");
            KillStealMenu.AddGroupLabel("Oldurme Ayarlari");
            KillStealMenu.Add("KsQ", new CheckBox("Kullan [Q] Oldururken"));
            KillStealMenu.Add("KsE", new CheckBox("Kullan [E] Oldururken"));
            KillStealMenu.Add("ign", new CheckBox("Kullan [Tutustur] Oldururken"));
            KillStealMenu.AddSeparator();
            KillStealMenu.AddGroupLabel("Ulti Ayarlari");
            KillStealMenu.Add("KsR", new CheckBox("Kullan [R] Oldururken"));
            KillStealMenu.Add("minKsR", new Slider("En az uzaklik [R] ile oldurmek icin", 100, 1, 1000));
            KillStealMenu.Add("RKb", new KeyBind("[R] Yari otomatik tusu", false, KeyBind.BindTypes.HoldActive, 'Y'));
            KillStealMenu.AddGroupLabel("Onerilen uzaklik 600");

            Drawings = Menu.AddSubMenu("Cizim ayarlari", "Draw");
            Drawings.AddGroupLabel("Cizim ayarlari");
            Drawings.Add("DrawQ", new CheckBox("[Q] Menzilini Goster"));
            Drawings.Add("DrawE", new CheckBox("[E] Menzilini Goster"));
            Drawings.Add("DrawR", new CheckBox("[R] Menzilini Goster"));
            Drawings.Add("DrawRhit", new CheckBox("[R] isabet sayisini Goster"));
            Drawings.Add("Notifications", new CheckBox("[R] ile Oldurulebilir uyarisini goster"));
            Drawings.Add("Draw_Disabled", new CheckBox("Cizimleri kapat"));

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Interrupter.OnInterruptableSpell += Interupt;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_Player.IsDead) return;
			
            if (Drawings["Draw_Disabled"].Cast<CheckBox>().CurrentValue) return;
			
            if (Drawings["DrawQ"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Orange, BorderWidth = 2f, Radius = Q.Range }.Draw(_Player.Position);
            }
			
            if (Drawings["DrawE"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Orange, BorderWidth = 2f, Radius = E.Range }.Draw(_Player.Position);
            }
			
            if (Drawings["DrawR"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Orange, BorderWidth = 2f, Radius = R.Range }.Draw(_Player.Position);
            }
			
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (Drawings["Notifications"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                Vector2 ft = Drawing.WorldToScreen(_Player.Position);
                if (target != null && target.IsValidTarget(R.Range) && Player.Instance.GetSpellDamage(target, SpellSlot.R) > target.Health + target.AttackShield)
                {
                    DrawFont(Thm, "R Kullan Oldur :) " + target.ChampionName, (float)(ft[0] - 140), (float)(ft[1] + 80), SharpDX.Color.Red);
                }
            }
			
            if (Drawings["DrawRhit"].Cast<CheckBox>().CurrentValue && target != null && R.IsReady() && target.IsValidTarget(R.Range))
            {
                var RPred = R.GetPrediction(target);
                var MinR = ComboMenu["MinR"].Cast<Slider>().CurrentValue;
                if (RPred.CastPosition.CountEnemyChampionsInRange(250) >= MinR)
                {
                    Vector2 ft = Drawing.WorldToScreen(_Player.Position);
                    DrawFont(Thm, "[R] isabet edicek " + RPred.CastPosition.CountEnemyChampionsInRange(250), (float)(ft[0] - 90), (float)(ft[1] + 20), SharpDX.Color.Orange);
                }
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }
			
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHit();
            }
			
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }
			
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClear();
            }
			
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
			
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
			
            KillSteal();
            RSelect();
			
            if (ComboMenu["ComboFQ"].Cast<KeyBind>().CurrentValue)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
        }

        private static void Combo()
        {
            var useR = ComboMenu["ComboR"].Cast<CheckBox>().CurrentValue;
            var MinR = ComboMenu["MinR"].Cast<Slider>().CurrentValue;
            var useW = ComboMenu["ComboW"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["ComboE"].Cast<CheckBox>().CurrentValue;
            var useQ = ComboMenu["ComboQ"].Cast<CheckBox>().CurrentValue;
            var disQ = ComboMenu["DisQ"].Cast<Slider>().CurrentValue;
            foreach (var target in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(R.Range) && !e.IsDead))
            {
                if (useQ && Q.IsReady() && target.IsValidTarget(Q.Range) && disQ < target.Distance(Player.Instance))
                {
                    Q.Cast(target);
                }
				
                if (useW && W.IsReady() && target.IsValidTarget(W.Range))
                {
                    W.Cast();
                }
				
                if (useE && E.IsReady() && target.IsValidTarget(E.Range))
                {
                    E.Cast();
                }

                if (useR && R.IsReady() && target.IsValidTarget(R.Range))
                {
                    var RPred = R.GetPrediction(target);
                    if (RPred.CastPosition.CountEnemyChampionsInRange(250) >= MinR && RPred.HitChance >= HitChance.High)
                    {
                        R.Cast(RPred.CastPosition);
                    }
                }
            }
        }

        private static void RSelect()
        {
            var targetF = TargetSelector.SelectedTarget;
            var useFQ = ComboMenu["ComboFQ"].Cast<KeyBind>().CurrentValue;

            if (targetF == null)
            {
                return;
            }

            if (useFQ && R.IsReady())
            {
                if (targetF.IsValidTarget(R.Range))
                {
                    R.Cast(targetF.Position);
                }
            }
        }

        private static void LaneClear()
        {
            var mana = LaneClearMenu["ManaLC"].Cast<Slider>().CurrentValue;
            var useQ = LaneClearMenu["LaneClearQ"].Cast<CheckBox>().CurrentValue;
            var useW = LaneClearMenu["LaneClearW"].Cast<CheckBox>().CurrentValue;
            var useE = LaneClearMenu["LaneClearE"].Cast<CheckBox>().CurrentValue;
            var minion = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(a => a.Distance(Player.Instance) <= Q.Range).OrderBy(a => a.Health).FirstOrDefault();
            if (Player.Instance.ManaPercent < mana) return;
            if (minion != null)
            {
                if (useQ && Q.IsReady() && minion.IsValidTarget(Q.Range) && minion.Health < Player.Instance.GetSpellDamage(minion, SpellSlot.Q))
                {
                    Q.Cast(minion);
                }
				
                if (useW && W.IsReady() && minion.IsValidTarget(W.Range) && _Player.Position.CountEnemyMinionsInRange(Q.Range) >= 3)
                {
                    W.Cast();
                }
				
                if (useE && E.IsReady() && minion.IsValidTarget(E.Range) && _Player.Position.CountEnemyMinionsInRange(E.Range) >= 3)
                {
                    E.Cast();
                }
            }
        }

        private static void LastHit()
        {
            var mana = LaneClearMenu["ManaLH"].Cast<Slider>().CurrentValue;
            var useQ = LaneClearMenu["LastHitQ"].Cast<CheckBox>().CurrentValue;
            var minion = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(a => a.Distance(Player.Instance) <= Q.Range).OrderBy(a => a.Health).FirstOrDefault();
            if (Player.Instance.ManaPercent < mana) return;

            if (minion != null)
            {
                if (useQ && Q.IsReady() && minion.IsValidTarget(Q.Range) && minion.Health <= Player.Instance.GetSpellDamage(minion, SpellSlot.Q) && _Player.Distance(minion) > 175)
                {
                    Q.Cast(minion);
                }
            }
        }

        public static void JungleClear()
        {
            var useQ = JungleClearMenu["QJungle"].Cast<CheckBox>().CurrentValue;
            var useE = JungleClearMenu["EJungle"].Cast<CheckBox>().CurrentValue;
            var useW = JungleClearMenu["WJungle"].Cast<CheckBox>().CurrentValue;
            var mana = JungleClearMenu["JungleMana"].Cast<Slider>().CurrentValue;
            var monters = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(j => j.Health).FirstOrDefault(j => j.IsValidTarget(R.Range));
            if (Player.Instance.ManaPercent <= mana) return;

            if (monters != null)
            {
                if (useQ && Q.IsReady() && monters.IsValidTarget(Q.Range))
                {
                    Q.Cast(monters);
                }
				
                if (useE && E.IsReady() && monters.IsValidTarget(E.Range))
                {
                    E.Cast();
                }
				
                if (useW && W.IsReady() && monters.IsValidTarget(W.Range))
                {
                    W.Cast();
                }
            }
        }

        private static void Harass()
        {
            var useW = HarassMenu["HarassW"].Cast<CheckBox>().CurrentValue;
            var useE = HarassMenu["HarassE"].Cast<CheckBox>().CurrentValue;
            var ManaQ = HarassMenu["ManaQ"].Cast<Slider>().CurrentValue;
            var useQ = HarassMenu["HarassQ"].Cast<CheckBox>().CurrentValue;
            var disQ = HarassMenu["DisQ2"].Cast<Slider>().CurrentValue;
            if (Player.Instance.ManaPercent < ManaQ) return;

            foreach (var target in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(Q.Range) && !e.IsDead))
            {
                if (useQ && Q.IsReady() && target.IsValidTarget(Q.Range) && disQ <= target.Distance(Player.Instance))
                {
                    Q.Cast(target);
                }

                if (useW && W.IsReady() && target.IsValidTarget(W.Range))
                {
                    W.Cast();
                }

                if (useE && E.IsReady() && target.IsValidTarget(E.Range))
                {
                    E.Cast();
                }
            }
        }

        private static void Flee()
        {
            if (R.IsReady())
            {
                var cursorPos = Game.CursorPos;
                var castPos = Player.Instance.Position.Distance(cursorPos) <= R.Range ? cursorPos : Player.Instance.Position.Extend(cursorPos, R.Range).To3D();
                R.Cast(castPos);
            }
        }

        public static void Interupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs i)
        {
            var Inter = ComboMenu["inter"].Cast<CheckBox>().CurrentValue;
            if (!sender.IsEnemy || !(sender is AIHeroClient) || Player.Instance.IsRecalling())
            {
                return;
            }

            if (Inter && R.IsReady() && i.DangerLevel == DangerLevel.High && R.IsInRange(sender))
            {
                R.Cast(sender.Position);
            }
        }

        public static void DrawFont(Font vFont, string vText, float vPosX, float vPosY, ColorBGRA vColor)
        {
            vFont.DrawText(null, vText, (int)vPosX, (int)vPosY, vColor);
        }

        private static void KillSteal()
        {
            var KsQ = KillStealMenu["KsQ"].Cast<CheckBox>().CurrentValue;
            var KsE = KillStealMenu["KsE"].Cast<CheckBox>().CurrentValue;
            var KsR = KillStealMenu["KsR"].Cast<CheckBox>().CurrentValue;
            var minKsR = KillStealMenu["minKsR"].Cast<Slider>().CurrentValue;
            foreach (var target in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(R.Range) && !hero.HasBuff("JudicatorIntervention") && !hero.HasBuff("kindredrnodeathbuff") && !hero.HasBuff("Undying Rage") && !hero.IsDead && !hero.IsZombie))
            {
                if (KsQ && Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    if (target.Health + target.AttackShield <= Player.Instance.GetSpellDamage(target, SpellSlot.Q))
                    {
                        Q.Cast(target);
                    }
                }
				
                if (KsE && E.IsReady() && target.IsValidTarget(E.Range))
                {
                    if (target.Health + target.AttackShield <= Player.Instance.GetSpellDamage(target, SpellSlot.E))
                    {
                        E.Cast();
                    }
                }
				
                if (KsR && R.IsReady())
                {
                    if (target.Health + target.AttackShield <= Player.Instance.GetSpellDamage(target, SpellSlot.R) && !target.IsInRange(Player.Instance, minKsR))
                    {
                        R.Cast(target);
                    }
                }
				
                if (R.IsReady() && KillStealMenu["RKb"].Cast<KeyBind>().CurrentValue)
                {
                    if (target.Health + target.AttackShield <= Player.Instance.GetSpellDamage(target, SpellSlot.R))
                    {
                        var pred = R.GetPrediction(target);
                        if (pred.HitChancePercent >= 70)
                        {
                            R.Cast(pred.CastPosition);
                        }
                    }
                }
				
                if (Ignite != null && KillStealMenu["ign"].Cast<CheckBox>().CurrentValue && Ignite.IsReady())
                {
                    if (target.Health <= _Player.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite))
                    {
                        Ignite.Cast(target);
                    }
                }
            }
        }
    }
}
