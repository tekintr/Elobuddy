﻿using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace LeeSinBuddy
{
    internal class StateManager
    {
        public static Menu ComboMenu, HarassMenu, FarmMenu, JungleMenu, KillstealMenu;
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static bool CastQAgain
        {
            get { return Program.LastCast[Program.Spells["Q1"]] + 2900 <= Environment.TickCount; }
        }

        public static void Init()
        {
            ComboMenu = Program.menu.AddSubMenu("Combo Settings");
            ComboMenu.AddGroupLabel("Combo Settings");

            ComboMenu.AddLabel("Q-1 Ayarlari");
            ComboMenu.Add("useQ1", new CheckBox("Kullan Q-1"));
            ComboMenu.AddLabel("Q-2 Ayarlari");
            ComboMenu.Add("qBeforeExpire", new CheckBox("Sure bitmeden once Q"));
            ComboMenu.Add("qOutOfRange", new CheckBox("Q Menzil disinda"));
            ComboMenu.Add("qExecute", new CheckBox("Q Olduruken"));
            ComboMenu.AddLabel("W-1 Ayarlari");
            ComboMenu.Add("useW1", new CheckBox("W1 Pasif icin"));
            ComboMenu.Add("wCatchUp", new CheckBox("Kullan W takim arkadasina atilmada"));
            ComboMenu.AddLabel("W-2 Ayarlari");
            ComboMenu.Add("useW2", new CheckBox("W2 Pasif icin"));
            ComboMenu.AddLabel("E-1 Ayarlari");
            ComboMenu.Add("useE1", new CheckBox("Kullan E1"));
            ComboMenu.AddLabel("E-2 Ayarlari");
            ComboMenu.Add("useE2", new CheckBox("E2"));
            ComboMenu.AddLabel("R Ayarlari");
            ComboMenu.Add("useR", new CheckBox("Kullan R"));
            ComboMenu.AddLabel("Pasif Ayarlari");
            ComboMenu.Add("minPassiveSliderCombo", new Slider("Skillerden sonra kac pasif kullanilsin", 1, 0, 2));

            HarassMenu = Program.menu.AddSubMenu("Durtme Ayarlari");
            HarassMenu.AddGroupLabel("Durtme Ayarlari");
            HarassMenu.AddLabel("Q-1 Ayarlari");
            HarassMenu.Add("useQ1H", new CheckBox("Kullan Q-1"));
            HarassMenu.AddLabel("Q-2 Ayarlari");
            HarassMenu.Add("qBeforeExpireH", new CheckBox("Sure bitmeden once Q"));
            HarassMenu.Add("qOutOfRangeH", new CheckBox("Q Menzil disinda"));
            HarassMenu.Add("qExecuteH", new CheckBox("Q Olduruken"));
            HarassMenu.AddLabel("E-1 Ayarlari");
            HarassMenu.Add("useE1H", new CheckBox("Kullan E1"));
            HarassMenu.AddLabel("E-2 Ayarlari");
            HarassMenu.Add("useE2H", new CheckBox("Kullan"));

            FarmMenu = Program.menu.AddSubMenu("Farm Ayarlari");
            FarmMenu.AddGroupLabel("SonVurus Ayarlari");
            FarmMenu.Add("useQ1F", new CheckBox("Kullan Q-1"));
            FarmMenu.Add("qExecuteF", new CheckBox("Q Olduruken"));
            FarmMenu.AddGroupLabel("SonVurus Ayarlari");
            FarmMenu.Add("useQ1WC", new CheckBox("Kullan Q-1"));
            FarmMenu.Add("qExecuteWC", new CheckBox("Q Olduruken"));
            FarmMenu.Add("eExecuteWC", new CheckBox("E Olduruken"));

            JungleMenu = Program.menu.AddSubMenu("Orman Ayarlari");
            JungleMenu.AddGroupLabel("Orman Ayarlari");
            JungleMenu.Add("useQ1J", new CheckBox("Kullan Q-1"));
            JungleMenu.Add("useQ2J", new CheckBox("Kullan Q-2"));
            JungleMenu.Add("useW1J", new CheckBox("Kullan W-1"));
            JungleMenu.Add("useW2J", new CheckBox("Kullan W-2"));
            JungleMenu.Add("useE1J", new CheckBox("Kullan E-1"));
            JungleMenu.Add("useE2J", new CheckBox("Kullan E-2"));
            JungleMenu.Add("minPassiveSlider", new Slider("Skillerden sonra kac pasif kullanilsin", 1, 0, 2));

            KillstealMenu = Program.menu.AddSubMenu("Oldurme Ayarlari");
            KillstealMenu.AddGroupLabel("Oldurme Ayarlari");
            KillstealMenu.Add("KSQ1", new CheckBox("Kullan Q1"));
            KillstealMenu.Add("KSQ2", new CheckBox("Kullan Q2"));
            KillstealMenu.Add("KSE1", new CheckBox("Kullan E"));
            KillstealMenu.Add("KSR1", new CheckBox("Kullan R"));

            Game.OnTick += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) Harass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)) LastHit();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Jungle();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) WaveClear();
        }

        public static void LastHit()
        {
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.Distance(Player.Instance) < 1400).OrderBy(a => a.Health);
            var minion =
                minions.FirstOrDefault(
                    a => Extended.BuffedEnemy != null && a.NetworkId == Extended.BuffedEnemy.NetworkId) ??
                minions.FirstOrDefault();
            if (minion == null || !minion.IsValidTarget()) return;
            if (FarmMenu["qExecuteF"].Cast<CheckBox>().CurrentValue && Extended.BuffedEnemy != null && Extended.BuffedEnemy.NetworkId == minion.NetworkId)
            {
                if (Program.Q.IsReady() && Damage.Q2Damage(minion, 0, true) > minion.Health)
                {
                    Program.Q2.Cast();
                }
            }
            if (FarmMenu["useQ1F"].Cast<CheckBox>().CurrentValue && (Damage.QDamage(minion) > minion.Health || Damage.QDamage(minion) + Damage.Q2Damage(minion, (float) Damage.QDamage(minion)) > minion.Health) && Program.Q.IsReady())
            {
                Program.Q.Cast(minion);
            }
        }

        public static void WaveClear()
        {
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.Distance(Player.Instance) < 1400).OrderBy(a => a.Health);
            var minion = (Extended.BuffedEnemy != null && Extended.BuffedEnemy.IsValidTarget(1400)) ? Extended.BuffedEnemy : minions.FirstOrDefault();
            if (minion == null || Program.LastSpellTime + 200 > Environment.TickCount) return;

            if (FarmMenu["qExecuteWC"].Cast<CheckBox>().CurrentValue && Extended.BuffedEnemy != null && Extended.BuffedEnemy.NetworkId == minion.NetworkId)
            {
                if (Program.Q.IsReady() && Damage.Q2Damage(minion, 0, true) > minion.Health)
                {
                    Program.Q2.Cast();
                    Program.LastSpellTime = Environment.TickCount;
                }
            }

            if (FarmMenu["eExecuteWC"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() && minions.Any(a => a.Health <= Damage.EDamage(a)
                                 && Program.E.Instance().Name == Program.Spells["E1"] &&
                                 a.Distance(Player.Instance) < 430))
            {
                Program.E.Cast();
                Program.LastSpellTime = Environment.TickCount;
            }
            
            if (FarmMenu["useQ1WC"].Cast<CheckBox>().CurrentValue && (Damage.QDamage(minion) > minion.Health || Damage.QDamage(minion) + Damage.Q2Damage(minion, (float)Damage.QDamage(minion)) > minion.Health) && Program.Q.IsReady())
            {
                Program.Q.Cast(minion);
                Program.LastSpellTime = Environment.TickCount;
            }
        }

        public static void Jungle()
        {
            var source = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(a => a.MaxHealth).FirstOrDefault(b => b.Distance(Player.Instance) < 1300);
            if (source == null || !source.IsValidTarget()) return;

            if (CastQAgain && Extended.BuffedEnemy != null && Extended.BuffedEnemy.NetworkId == source.NetworkId &&
                Program.Q.IsReady() && Program.Q.Instance().Name == Program.Spells["Q2"])
            {
                Program.Q2.Cast();
                Program.LastSpellTime = Environment.TickCount;
            }

            if (Program.PassiveStacks > 0 || Program.LastSpellTime + 600 > Environment.TickCount) return;

            if (Program.Q.IsReady()
                && Program.Q.Instance().Name == Program.Spells["Q2"] && source.HasQBuff()
                && (CastQAgain
                    || _Player.Distance(source) > _Player.GetAutoAttackRange(source)
                    || Damage.Q2Damage(source) > source.Health + source.AttackShield))
            {
                Program.Q2.Cast();
                Program.LastSpellTime = Environment.TickCount;
                return;
            }

            if (JungleMenu["useQ1J"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady()
                && Program.Q.Instance().Name == Program.Spells["Q1"] && source.Distance(_Player) < Program.Q.Range)
            {
                Program.Q.Cast(source);
                Program.LastSpellTime = Environment.TickCount;
                return;
            }

            if (JungleMenu["useW1J"].Cast<CheckBox>().CurrentValue && Program.W.IsReady()
                && Program.W.Instance().Name == Program.Spells["W1"] &&
                source.Distance(_Player) < _Player.GetAutoAttackRange(source))
            {
                Program.W.Cast(_Player);
                Program.LastSpellTime = Environment.TickCount;
                return;
            }

            if (JungleMenu["useW2J"].Cast<CheckBox>().CurrentValue && Program.W.IsReady()
                && Program.W.Instance().Name == Program.Spells["W2"])
            {
                Program.W.Cast(_Player);
                Program.LastSpellTime = Environment.TickCount;
                return;
            }

            if (JungleMenu["useE2J"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() &&
                Program.E.Instance().Name == Program.Spells["E2"]
                && Program.PassiveStacks <= 1
                && source.HasEBuff() && source.Distance(_Player) < 600)
            {
                Program.E.Cast();
                Program.LastSpellTime = Environment.TickCount;
                return;
            }

            if (JungleMenu["useE1J"].Cast<CheckBox>().CurrentValue && Program.E.IsReady()
                && Program.E.Instance().Name == Program.Spells["E1"] && source.Distance(_Player) < 430)
            {
                Program.E.Cast();
                Program.LastSpellTime = Environment.TickCount;
            }
        }

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(1300, DamageType.Physical);
            if (target == null || !target.IsValidTarget())
                return;

            if (ComboMenu["useR"].Cast<CheckBox>().CurrentValue && Program.R.IsReady() &&
                target.Distance(_Player) <= Program.R.Range
                && (Damage.RDamage(target) >= target.Health + target.AttackShield || target.HasQBuff() && target.Health < Damage.RDamage(target) + Damage.Q2Damage(target, Damage.RDamage(target))))
            {
                Program.R.Cast(target);
                return;
            }

            if (ComboMenu["qBeforeExpire"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady()
                && Program.Q.Instance().Name == Program.Spells["Q2"] && target.HasQBuff()
                && (CastQAgain))
            {
                Program.Q2.Cast();
                return;
            }

            if (_Player.Distance(target) <= _Player.AttackRange + 100 + target.BoundingRadius &&
                Program.PassiveStacks > ComboMenu["minPassiveSliderCombo"].Cast<Slider>().CurrentValue || Program.LastSpellTime + 500 > Environment.TickCount)
                return;

            if (Program.Q.IsReady()
                && Program.Q.Instance().Name == Program.Spells["Q2"] && target.HasQBuff()
                && (CastQAgain && ComboMenu["qBeforeExpire"].Cast<CheckBox>().CurrentValue
                    ||
                    ComboMenu["qOutOfRange"].Cast<CheckBox>().CurrentValue &&
                    _Player.Distance(target) > _Player.GetAutoAttackRange(target)
                    ||
                    ComboMenu["qExecute"].Cast<CheckBox>().CurrentValue &&
                    Damage.Q2Damage(target) > target.Health + target.AttackShield))
            {
                Program.Q2.Cast();
                Program.LastSpellTime = Environment.TickCount;
                return;
            }
            if (ComboMenu["useQ1"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady()
                && Program.Q.Instance().Name == Program.Spells["Q1"] && target.Distance(_Player) < Program.Q.Range)
            {
                SpellClass.SmiteQCast(target);
                return;
            }
            if (ComboMenu["useE2"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() &&
                Program.E.Instance().Name == Program.Spells["E2"]
                && target.Distance(_Player) < Program.E2.Range && _Player.Distance(target) > _Player.AttackRange ||
                Program.PassiveStacks == 0
                && target.HasEBuff() && target.Distance(_Player) < 600)
            {
                Program.E.Cast();
                return;
            }
            if (ComboMenu["useE1"].Cast<CheckBox>().CurrentValue && Program.E.IsReady()
                && Program.E.Instance().Name == Program.Spells["E1"] && target.Distance(_Player) < 430)
            {
                Program.E.Cast();
                return;
            }

            if (ComboMenu["useW2"].Cast<CheckBox>().CurrentValue &&  target.Distance(_Player) <= Player.Instance.GetAutoAttackRange(target) && Program.W.IsReady()
                && Program.W.Instance().Name == Program.Spells["W2"])
            {
                var unit =
                    ObjectManager
                        .Get<Obj_AI_Base>(
                            ).FirstOrDefault(a => a.Distance(target) < Player.Instance.GetAutoAttackRange(target) && a.IsAlly &&
                                a.Distance(target) < Player.Instance.Distance(target) && a.Distance(Player.Instance) < Program.W.Range);
                if (unit != null)
                {
                    Player.CastSpell(SpellSlot.W, Player.Instance);
                    Program.LastSpellTime = Environment.TickCount;
                    return;
                }
            }
            if (target.Distance(_Player) <= Player.Instance.GetAutoAttackRange(target) &&
                ComboMenu["useW1"].Cast<CheckBox>().CurrentValue && Program.W.IsReady()
                && Program.W.Instance().Name == Program.Spells["W1"] && Program.LastSpellTime + 200 < Environment.TickCount)
            {
                Player.CastSpell(SpellSlot.W, Player.Instance);
                Program.LastSpellTime = Environment.TickCount;

            }
            if (ComboMenu["wCatchUp"].Cast<CheckBox>().CurrentValue && target.Distance(_Player) > 430 && Program.W.IsReady()
                && Program.W.Instance().Name == Program.Spells["W1"] && Program.LastSpellTime + 200 < Environment.TickCount)
            {
                var unit =
                       ObjectManager
                           .Get<Obj_AI_Base>(
                               ).FirstOrDefault(a => a.Distance(target) < Player.Instance.GetAutoAttackRange(target) && a.IsAlly &&
                                   a.Distance(target) < Player.Instance.Distance(target) && a.Distance(Player.Instance) < Program.W.Range);
                if (unit != null)
                {
                    Player.CastSpell(SpellSlot.W, unit);
                    Program.LastSpellTime = Environment.TickCount;
                }
            }
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(1300, DamageType.Physical);
            if (target == null || !target.IsValidTarget())
                return;

            if (HarassMenu["qBeforeExpireH"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady()
                && Program.Q.Instance().Name == Program.Spells["Q2"] && target.HasQBuff()
                && (CastQAgain))
            {
                Program.Q2.Cast();
                return;
            }

            if (_Player.Distance(target) <= _Player.GetAutoAttackRange(target) &&
                Program.PassiveStacks > 1)
                return;

            if (Program.Q.IsReady()
                && Program.Q.Instance().Name == Program.Spells["Q2"] && target.HasQBuff()
                && (CastQAgain && HarassMenu["qBeforeExpireH"].Cast<CheckBox>().CurrentValue
                    ||
                    HarassMenu["qOutOfRangeH"].Cast<CheckBox>().CurrentValue &&
                    _Player.Distance(target) > _Player.GetAutoAttackRange(target)
                    ||
                    HarassMenu["qExecuteH"].Cast<CheckBox>().CurrentValue &&
                    Damage.Q2Damage(target) > target.Health + target.AttackShield))
            {
                Program.Q2.Cast();
                return;
            }
            if (HarassMenu["useQ1H"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady()
                && Program.Q.Instance().Name == Program.Spells["Q1"] && target.Distance(_Player) < Program.Q.Range)
            {
                if (!Program.Q.GetPrediction(target).HitChance.HasFlag(HitChance.Collision))
                {
                    Program.Q.Cast(target);
                    Program.LastSpellTime = Environment.TickCount;
                    return;
                }
            }
            if (HarassMenu["useE2H"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() &&
                Program.E.Instance().Name == Program.Spells["E2"]
                && target.Distance(_Player) < Program.E2.Range && _Player.Distance(target) > _Player.AttackRange ||
                Program.PassiveStacks == 0
                && target.HasEBuff() && target.Distance(_Player) < 600)
            {
                Program.E.Cast();
                return;
            }
            if (HarassMenu["useE1H"].Cast<CheckBox>().CurrentValue && Program.E.IsReady()
                && Program.E.Instance().Name == Program.Spells["E1"] && target.Distance(_Player) < 430)
            {
                Program.E.Cast();
            }
        }

        public static void KillSteal()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(a => !a.IsDead && !a.IsZombie && a.Health > 0))
            {
                if (KillstealMenu["KSE1"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() && enemy.Health < Damage.EDamage(enemy) && Program.E.Instance().Name == Program.Spells["E1"] &&
                    enemy.Distance(_Player) < 430)
                {
                    Program.E.Cast();
                    return;
                }
                if (KillstealMenu["KSQ2"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && enemy.HasQBuff() && enemy.Health < Damage.Q2Damage(enemy) && Program.Q.Instance().Name == Program.Spells["Q2"] && enemy.Distance(_Player) < 1400)
                {
                    Program.Q2.Cast();
                    return;
                }
                if (KillstealMenu["KSQ1"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && enemy.Health < Damage.QDamage(enemy) && Program.Q.Instance().Name == Program.Spells["Q1"] && enemy.Distance(_Player) < 1100)
                {
                    Program.Q.Cast(enemy);
                    return;
                }
                if (KillstealMenu["KSR1"].Cast<CheckBox>().CurrentValue && Program.R.IsReady() && enemy.Health < Damage.QDamage(enemy) &&
                    enemy.Distance(_Player) < Program.R.Range)
                {
                    Program.R.Cast(enemy);
                }
            }
        }
    }
}