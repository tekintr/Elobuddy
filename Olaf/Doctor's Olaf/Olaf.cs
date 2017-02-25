﻿using System;
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

namespace Olaf7
{
    internal class Program
    {
        public static Menu Menu, ComboMenu, HarassMenu, LaneClearMenu, LastHitMenu, JungleClearMenu, Misc, KillStealMenu, Skin, Drawings;
        public static Item Botrk;
        public static Item Bil;
        public static Item Hydra;
        public static Item Tiamat;
        public static Item Titanic;
        public static GameObject Axe { get; set; }
        public static float Time { get; set; }
        public static float End { get; set; }
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static Spell.Targeted Ignite;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        static void OnLoadingComplete(EventArgs args)
        {
            if (!_Player.ChampionName.Contains("Olaf")) return;
            Chat.Print("Doctor's Olaf Yuklendi.Ceviri TekinTR!", Color.Orange);
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1550, 75);
            Q.AllowedCollisionCount = int.MaxValue;
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 325);
            R = new Spell.Active(SpellSlot.R);
            Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
            Botrk = new Item(ItemId.Blade_of_the_Ruined_King);
            Bil = new Item(3144, 475f);
            Tiamat = new Item(ItemId.Tiamat_Melee_Only, 400);
            Hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only, 400);
            Titanic = new Item(ItemId.Titanic_Hydra, Player.Instance.GetAutoAttackRange());
            Menu = MainMenu.AddMenu("Olaf", "Olaf");
            Menu.AddGroupLabel("Doctor7");
            ComboMenu = Menu.AddSubMenu("Combo Settings", "Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("ComboQ", new CheckBox("Kullan [Q]"));
            ComboMenu.Add("ComboW", new CheckBox("Kullan [W]"));
            ComboMenu.Add("ComboE", new CheckBox("Kullan [E]"));
            ComboMenu.AddGroupLabel("Items Settings");
            ComboMenu.Add("item", new CheckBox("Kullan [Mahvolmus]"));
            ComboMenu.Add("hyd", new CheckBox("Kullan [Hydra] Reset AA"));
            ComboMenu.Add("ihp", new Slider("Benim HP Kullan Mahvolmus <=", 50));
            ComboMenu.Add("ihpp", new Slider("Dusman HP Kullan Mahvolmus <=", 50));

            HarassMenu = Menu.AddSubMenu("Harass Settings", "Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("HarassQ", new CheckBox("Kullan [Q]"));
            HarassMenu.Add("HarassW", new CheckBox("Kullan [W]", false));
            HarassMenu.Add("HarassE", new CheckBox("Kullan [E]"));
            HarassMenu.Add("ManaQ", new Slider("Durtmek icin enaz mana", 40));

            LaneClearMenu = Menu.AddSubMenu("LaneClear Settings", "LaneClear");
            LaneClearMenu.AddGroupLabel("LaneClear Settings");
            LaneClearMenu.Add("ClearQ", new CheckBox("Kullan [Q]"));
            LaneClearMenu.Add("CantLC", new CheckBox("Sadece [Q] Oldurulemiyecek minyonlara", false));
            LaneClearMenu.Add("minQ", new Slider("Enaz minyona carpicaksa kullan [Q]", 3, 1, 6));
            LaneClearMenu.Add("ClearE", new CheckBox("Kullan [E]"));
            LaneClearMenu.Add("ClearW", new CheckBox("Kullan [W]", false));
            LaneClearMenu.Add("ManaLC", new Slider("Koridortemizleme icin enaz mana", 60));

            LastHitMenu = Menu.AddSubMenu("LastHit Settings", "LastHit");
            LastHitMenu.AddGroupLabel("LastHit Settings");
            LastHitMenu.Add("LastQ", new CheckBox("Kullan [Q] SonVurus", false));
            LastHitMenu.Add("LhAA", new CheckBox("Sadece [Q] Oldurulemiyecek minyonlara"));
            LastHitMenu.Add("LastE", new CheckBox("Kullan [E] SonVurus"));
            LastHitMenu.Add("LhMana", new Slider("SonVurus icin enaz mana", 60));

            JungleClearMenu = Menu.AddSubMenu("JungleClear Settings", "JungleClear");
            JungleClearMenu.AddGroupLabel("JungleClear Settings");
            JungleClearMenu.Add("QJungle", new CheckBox("Kullan [Q]"));
            JungleClearMenu.Add("WJungle", new CheckBox("Kullan [W]"));
            JungleClearMenu.Add("EJungle", new CheckBox("Kullan [E]"));
            JungleClearMenu.Add("MnJungle", new Slider("Orman icin enaz mana", 30));

            Misc = Menu.AddSubMenu("Ultimate Settings", "Misc");
            Misc.Add("Ulti", new CheckBox("Kullan Ulti"));
            Misc.AddGroupLabel("Kullanilsin [R]");
            Misc.Add("stun", new CheckBox("Sabitlenme"));
            Misc.Add("rot", new CheckBox("Root"));
            Misc.Add("knockup", new CheckBox("Itilme"));
            Misc.Add("tunt", new CheckBox("Taunt"));
            Misc.Add("charm", new CheckBox("Cekilme", false));
            Misc.Add("snare", new CheckBox("Kapan"));
            Misc.Add("blind", new CheckBox("Korlesme", false));
            Misc.Add("disarm", new CheckBox("Engellenme", false));
            Misc.Add("fear", new CheckBox("Korku", false));
            Misc.Add("silence", new CheckBox("Susturma", false));
            Misc.Add("supperss", new CheckBox("Supperss", false));
            Misc.Add("slow", new CheckBox("Yavaslama", false));
            Misc.Add("poison", new CheckBox("Zehir", false));
            Misc.Add("knockback", new CheckBox("Tekmelenince", false));
            Misc.Add("nearsight", new CheckBox("Gorus Engellenince", false));
            Misc.Add("poly", new CheckBox("Polymorph", false));
            Misc.AddGroupLabel("Ultimate Setting");
            Misc.Add("healulti", new Slider("Kullanmak icin enaz HP [R]", 60));
            Misc.Add("Rulti", new Slider("Enaz dusman sayisi [R]", 1, 1, 5));
            Misc.AddGroupLabel("Ultimate Delay");
            Misc.Add("delay", new Slider("Gecikme", 0, 0, 1000));

            KillStealMenu = Menu.AddSubMenu("KillSteal Settings", "KillSteal");
            KillStealMenu.AddGroupLabel("KillSteal Settings");
            KillStealMenu.Add("KsQ", new CheckBox("Kullan [Q] Oldururken"));
            KillStealMenu.Add("KsE", new CheckBox("Kullan [E] Oldururken"));
            KillStealMenu.Add("KsIgnite", new CheckBox("Kullan [Tutustur] Oldururken"));

            Skin = Menu.AddSubMenu("Skin Changer", "SkinChanger");
            Skin.AddGroupLabel("Skin Settings");
            Skin.Add("checkSkin", new CheckBox("Skin Secici kullan"));
            Skin.Add("skin.Id", new ComboBox("Skin Mode", 3, "Default", "1", "2", "3", "4", "5", "6"));

            Drawings = Menu.AddSubMenu("Misc Settings", "Draw");
            Drawings.AddGroupLabel("Misc Setting");
            Drawings.Add("QStun", new CheckBox("Kullan [Q] eger dusman CC'lenirse(Stan vb.)", false));
            Drawings.Add("AntiGap", new CheckBox("Kullan [Q] Atilma Yapanlara"));
            Drawings.AddGroupLabel("Drawing Settings");
            Drawings.Add("DrawQ", new CheckBox("Goster Q Menzili"));
            Drawings.Add("DrawE", new CheckBox("Goster E Menzili", false));
            Drawings.Add("Axe", new CheckBox("Baltayi Goster"));

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Orbwalker.OnPostAttack += ResetAttack;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            Orbwalker.OnUnkillableMinion += Orbwalker_CantLasthit;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Drawings["DrawQ"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Orange, BorderWidth = 1, Radius = Q.Range }.Draw(_Player.Position);
            }
			
            if (Drawings["DrawE"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Orange, BorderWidth = 1, Radius = E.Range }.Draw(_Player.Position);
            }
			
            if (Drawings["Axe"].Cast<CheckBox>().CurrentValue && Axe != null)
            {
                new Circle() { Color = Color.GreenYellow, BorderWidth = 6, Radius = 100 }.Draw(Axe.Position);
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        { 
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
			
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }
			
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClear();
            }
			
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }
			
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHit();
            }
			
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
			
            KillSteal();
            RStun();
            Ult();

            if (_Player.SkinId != Skin["skin.Id"].Cast<ComboBox>().CurrentValue)
            {
                if (checkSkin())
                {
                    Player.SetSkinId(SkinId());
                }
            }
        }

        public static int SkinId()
        {
            return Skin["skin.Id"].Cast<ComboBox>().CurrentValue;
        }
		
        public static bool checkSkin()
        {
            return Skin["checkSkin"].Cast<CheckBox>().CurrentValue;
        }
		
        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var useQ = ComboMenu["ComboQ"].Cast<CheckBox>().CurrentValue;
            var useW = ComboMenu["ComboW"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["ComboE"].Cast<CheckBox>().CurrentValue;
            var item = ComboMenu["item"].Cast<CheckBox>().CurrentValue;
            var Minhp = ComboMenu["ihp"].Cast<Slider>().CurrentValue;
            var Minhpp = ComboMenu["ihpp"].Cast<Slider>().CurrentValue;
            if (target != null)
            {
                var pos = Q.GetPrediction(target).CastPosition.Extend(Player.Instance.Position, -90);
                if (useQ && Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    if (_Player.Distance(target) > 375)
                    {
                        Q.Cast(pos.To3DWorld());
                    }
                    else
                    {
                        Q.Cast(target.Position);
                    }
                }
				
                if (useW && W.IsReady() && target.IsValidTarget(E.Range))
         	    {				
                    W.Cast();
	    		}
				
                if (useE && E.IsReady() && target.IsValidTarget(E.Range) && _Player.Distance(target) > Player.Instance.GetAutoAttackRange(target))
                {
                    E.Cast(target);
                }
				
                if (item && Bil.IsReady() && Bil.IsOwned() && target.IsValidTarget(475))
                {
                    Bil.Cast(target);
                }
				
                if ((item && Botrk.IsReady() && Botrk.IsOwned() && target.IsValidTarget(475)) && (Player.Instance.HealthPercent <= Minhp || target.HealthPercent < Minhpp))
                {
                    Botrk.Cast(target);
                }
            }
        }

        public static void ResetAttack(AttackableUnit e, EventArgs args)
        {
            if (!(e is AIHeroClient)) return;
            var target = TargetSelector.GetTarget(325, DamageType.Physical);
            var champ = (AIHeroClient)e;
            var useriu = ComboMenu["hyd"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["ComboE"].Cast<CheckBox>().CurrentValue;
            if (champ == null || champ.Type != GameObjectType.AIHeroClient || !champ.IsValid) return;
            if (target != null)
            {
                if ((useriu) && (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)))
                {
                    if (Hydra.IsOwned(Player.Instance) && Hydra.IsReady() && target.IsValidTarget(250))
                    {
                        Hydra.Cast();
                    }

                    if (Tiamat.IsOwned(Player.Instance) && Tiamat.IsReady() && target.IsValidTarget(250))
                    {
                        Tiamat.Cast();
                    }
                }

                if ((useE && E.IsReady()) && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && target.IsValidTarget(325) && _Player.Distance(target) < Player.Instance.GetAutoAttackRange(target))
                {
                    E.Cast(target);
                }
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {	
            if (Drawings["AntiGap"].Cast<CheckBox>().CurrentValue && sender.IsEnemy && e.Sender.Distance(_Player) < 300 && !e.Sender.ChampionName.ToLower().Contains("MasterYi"))
            {
                Q.Cast(e.Sender);
            }
        }

        public static void JungleClear()
        {
            var useQ = JungleClearMenu["QJungle"].Cast<CheckBox>().CurrentValue;
            var useW = JungleClearMenu["WJungle"].Cast<CheckBox>().CurrentValue;
            var useE = JungleClearMenu["EJungle"].Cast<CheckBox>().CurrentValue;
            var mana = JungleClearMenu["MnJungle"].Cast<Slider>().CurrentValue;
            var jungle = EntityManager.MinionsAndMonsters.GetJungleMonsters(_Player.Position, Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
			
            if (jungle != null)
            {
                if (useQ && Q.IsReady() && Q.IsInRange(jungle) && Player.Instance.ManaPercent >= mana)
                {
                    Q.Cast(jungle);
                }
				
                if (useW && W.IsReady() && jungle.IsValidTarget(325) && Player.Instance.ManaPercent >= mana)
                {
                    W.Cast();
                }
				
                if (useE && E.IsReady() && E.IsInRange(jungle))
                {
                    E.Cast(jungle);
                }
            }
        }

        private static void LaneClear()
        {
            var useQ = LaneClearMenu["ClearQ"].Cast<CheckBox>().CurrentValue;
            var useW = LaneClearMenu["ClearW"].Cast<CheckBox>().CurrentValue;
            var useE = LaneClearMenu["ClearE"].Cast<CheckBox>().CurrentValue;
            var mana = LaneClearMenu["ManaLC"].Cast<Slider>().CurrentValue;
            var MinQ = LaneClearMenu["minQ"].Cast<Slider>().CurrentValue;
            var minionE = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(e => e.IsValidTarget(Q.Range)).ToArray();
            var quang = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minionE, Q.Width, (int) 600);
			
            foreach (var minion in minionE)
            {
                if (useW && W.IsReady() && Player.Instance.ManaPercent >= mana && minion.IsValidTarget(E.Range))
                {
                    W.Cast();
                }

                if (useE && E.IsReady() && !minion.IsValidTarget(_Player.AttackRange) && minion.Health <= Player.Instance.GetSpellDamage(minion, SpellSlot.E))
                {
                    E.Cast(minion);
                }

                if (useQ && Q.IsReady() && Player.Instance.ManaPercent >= mana && minion.IsValidTarget(600) && quang.HitNumber >= MinQ)
                {
                    Q.Cast(quang.CastPosition);
                }
            }
        }

        private static void Orbwalker_CantLasthit(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            var useCant = LaneClearMenu["CantLC"].Cast<CheckBox>().CurrentValue;
            var laneQMN = LaneClearMenu["ManaLC"].Cast<Slider>().CurrentValue;
            var useAA = LastHitMenu["LhAA"].Cast<CheckBox>().CurrentValue;
            var LhM = LastHitMenu["LhMana"].Cast<Slider>().CurrentValue;
            var unit = (useCant && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Player.Instance.ManaPercent >= laneQMN)
            || (useAA && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && Player.Instance.ManaPercent >= LhM);
			
            if (target == null)
            {
                return;
            }

            if (unit && Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                if (Player.Instance.GetSpellDamage(target, SpellSlot.Q) >= Prediction.Health.GetPrediction(target, Q.CastDelay))
                {
                    Q.Cast(target);
                }
            }
        }

        private static void Flee()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target != null)
            {
                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    Q.Cast(target.Position);
                }
            }
        }

        public static void Harass()
        {
            var useQ = HarassMenu["HarassQ"].Cast<CheckBox>().CurrentValue;
            var useW = HarassMenu["HarassW"].Cast<CheckBox>().CurrentValue;
            var useE = HarassMenu["HarassE"].Cast<CheckBox>().CurrentValue;
            var ManaQ = HarassMenu["ManaQ"].Cast<Slider>().CurrentValue;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target != null)
            {
                var pos = Q.GetPrediction(target).CastPosition.Extend(Player.Instance.Position, -90);
                if (useQ && Q.IsReady() && target.IsValidTarget(Q.Range) && Player.Instance.ManaPercent >= ManaQ)
                {
                    if (_Player.Distance(target) > 375)
                    {
                        Q.Cast(pos.To3DWorld());
                    }
                    else
                    {
                        Q.Cast(target.Position);
                    }
                }
				
                if (useW && W.IsReady() && target.IsValidTarget(325) && Player.Instance.ManaPercent >= ManaQ)
                {				
                    W.Cast();
		    	}
				
                if (E.IsReady() && useE && target.IsValidTarget(E.Range))
                {
                    E.Cast(target);
                }
            }
        }

        public static void LastHit()
        {
            var useE = LastHitMenu["LastE"].Cast<CheckBox>().CurrentValue;
            var useQ = LastHitMenu["LastQ"].Cast<CheckBox>().CurrentValue;
            var LhM = LastHitMenu["LhMana"].Cast<Slider>().CurrentValue;
            var minion = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(a => a.Distance(Player.Instance) <= Q.Range).OrderBy(a => a.Health).FirstOrDefault();
            if (minion != null)
            {
                if (useQ && Q.IsReady() && Player.Instance.ManaPercent > LhM && minion.IsValidTarget(Q.Range) && Player.Instance.GetSpellDamage(minion, SpellSlot.Q) >= minion.TotalShieldHealth())
                {
                    Q.Cast(minion);
                }

                if (useE && E.IsReady() && Player.Instance.GetSpellDamage(minion, SpellSlot.E) >= minion.TotalShieldHealth())
                {
                    E.Cast(minion);
                }
            }
        }

        public static void RStun()
		{
            var Rstun = Drawings["QStun"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (Rstun && Q.IsReady())
            {
                if (target != null)
                {
                    if (target.HasBuffOfType(BuffType.Taunt) || target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Knockup))
                    {
                        Q.Cast(target.Position);
                    }
                }
            }
        }

        public static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name.ToLower().Contains("olaf_base_q_axe"))
            {
                Axe = sender;
                Time = Game.Time;
                End = Game.Time + 8;
            }
        }

        public static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (sender.Name.ToLower().Contains("olaf_base_q_axe"))
            {
                Axe = null;
            }
        }

        public static void KillSteal()
        {
            var KsQ = KillStealMenu["KsQ"].Cast<CheckBox>().CurrentValue;
            var KsE = KillStealMenu["KsE"].Cast<CheckBox>().CurrentValue;
            foreach (var target in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(Q.Range) && !hero.HasBuff("JudicatorIntervention") && !hero.HasBuff("kindredrnodeathbuff") && !hero.HasBuff("Undying Rage") && !hero.IsDead && !hero.IsZombie))
            {
                if (KsQ && Q.IsReady())
                {
                    var pos = Q.GetPrediction(target).CastPosition.Extend(Player.Instance.Position, -80);
                    if (target.Health <= Player.Instance.GetSpellDamage(target, SpellSlot.Q))
                    {
                        Q.Cast(pos.To3DWorld());
                    }
                }

                if (KsE && E.IsReady())
                {
                    if (target.Health <= Player.Instance.GetSpellDamage(target, SpellSlot.E))
                    {
                        E.Cast(target);
                    }
                }

                if (Ignite != null && KillStealMenu["KsIgnite"].Cast<CheckBox>().CurrentValue && Ignite.IsReady())
                {
                    if (target.Health <= _Player.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite))
                    {
                        Ignite.Cast(target);
                    }
                }
            }
        }

        private static void Ult()
        {
            var ulti = Misc["Ulti"].Cast<CheckBox>().CurrentValue;
            var heal = Misc["healulti"].Cast<Slider>().CurrentValue;
            var minR = Misc["Rulti"].Cast<Slider>().CurrentValue;
            var cc = (Misc["silence"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Silence))
            || (Misc["snare"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Snare))
            || (Misc["supperss"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Suppression))
            || (Misc["poly"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Polymorph))
            || (Misc["disarm"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Disarm))
            || (Misc["nearsight"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.NearSight))
            || (Misc["knockback"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Knockback))
            || (Misc["knockup"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Knockup))
            || (Misc["slow"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Slow))
            || (Misc["poison"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Poison))
            || (Misc["blind"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Blind))
            || (Misc["charm"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Charm))
            || (Misc["tunt"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Taunt))
            || (Misc["stun"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Stun))
            || (Misc["rot"].Cast<CheckBox>().CurrentValue && _Player.IsRooted)
            || (Misc["fear"].Cast<CheckBox>().CurrentValue && Player.HasBuffOfType(BuffType.Fear));
            if (R.IsReady() && ulti && cc && _Player.Position.CountEnemyChampionsInRange(800) >= minR && Player.Instance.HealthPercent <= heal)
            {
                Core.DelayAction(() => R.Cast(), Misc["delay"].Cast<Slider>().CurrentValue);
            }
        }
    }
}
