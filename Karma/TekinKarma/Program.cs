#region

using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

#endregion

namespace TekinKarma
{
    internal class Program
    {

        public static Spell.Skillshot _q;
        public static Spell.Targeted _w;
        public static Spell.Targeted _e;
        public static Spell.Active _r;

        private static int LastTick;

        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        private static Menu Menu;
        public static Menu comboMenu, harassMenu, miscMenu, drawMenu, laneMenu, ksMenu, fleeMenu;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Karma")
            {
                return;
            }

            _q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 1700, (int)60f);
            _w = new Spell.Targeted(SpellSlot.W, 675);
            _e = new Spell.Targeted(SpellSlot.E, 800);
            _r = new Spell.Active(SpellSlot.R);

            Menu = MainMenu.AddMenu("TekinKarma", "TekinKarma");

            comboMenu = Menu.AddSubMenu("Combo", "Combo");
            comboMenu.Add("MantraMode", new ComboBox("Ulti Ayari", 0, "Q", "W", "E", "Auto"));
            comboMenu.AddLabel("Q : uses R + Q on target");
            comboMenu.AddLabel("W : uses R + W on target");
            comboMenu.AddLabel("E : uses R + E on Self");
            comboMenu.AddSeparator();
            comboMenu.Add("Mantra", new KeyBind("Ulti Ayar sec", false, KeyBind.BindTypes.HoldActive, 'T'));


            harassMenu = Menu.AddSubMenu("Harass", "Harass");
            harassMenu.Add("HarassQ1", new CheckBox("Kullan Q", true));
            harassMenu.Add("HarassW1", new CheckBox("Kullan W", false));
            harassMenu.Add("HarassE1", new CheckBox("Kullan E", false));
            harassMenu.Add("HarassR", new CheckBox("Kullan R", true));
            harassMenu.Add("HarassQ", new Slider("Q Mana", 70, 0, 100));
            harassMenu.Add("HarassW", new Slider("W Mana", 70, 0, 100));
            harassMenu.Add("HarassE", new Slider("E Mana", 70, 0, 100));


            miscMenu = Menu.AddSubMenu("Misc", "Misc");
            miscMenu.Add("eactivated", new CheckBox("Kullan E"));
            miscMenu.Add("emyhp", new Slider("Dusman HP", 30, 0, 100));
            miscMenu.Add("eallyhp", new Slider("Dost HP", 30, 0, 100));
            miscMenu.Add("emana", new Slider("Enaz Mana % ", 50, 0, 100));
            miscMenu.Add("hitchance", new ComboBox("Q HitChance", 2, "Low", "Medium", "High"));
            miscMenu.Add("egapclose", new CheckBox("Kullan E atilma yapana"));
            miscMenu.Add("qgapclose", new CheckBox("Kullan Q atilma yapana"));


            laneMenu = Menu.AddSubMenu("LaneClear", "LaneClear");
            laneMenu.Add("laneq", new CheckBox("Kullan Q"));
            laneMenu.Add("lanemana", new Slider("Koridor Mana % ", 50, 0, 100));
            laneMenu.Add("minq", new Slider("Kac minyona Q ", 3, 1, 5));
            laneMenu.Add("laneqh", new CheckBox("Kullan Q son vurus"));


            ksMenu = Menu.AddSubMenu("Killsteal", "Killsteal");
            ksMenu.Add("ksq", new CheckBox("Kullan Q"));


            drawMenu = Menu.AddSubMenu("Drawings", "Drawings");
            drawMenu.Add("drawQ", new CheckBox("Q Menzili", true));
            drawMenu.Add("drawW", new CheckBox("W Menzili", true));
            drawMenu.Add("drawE", new CheckBox("E Menzili", true));


            fleeMenu = Menu.AddSubMenu("Flee", "Flee");
            fleeMenu.Add("fleee", new CheckBox("R + E Kacis", true));


            Gapcloser.OnGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Game_OnDraw;
        }

        public static bool getCheckBoxItem(Menu m, string item)
        {
            return m[item].Cast<CheckBox>().CurrentValue;
        }

        public static int getSliderItem(Menu m, string item)
        {
            return m[item].Cast<Slider>().CurrentValue;
        }

        public static bool getKeyBindItem(Menu m, string item)
        {
            return m[item].Cast<KeyBind>().CurrentValue;
        }

        public static int getBoxItem(Menu m, string item)
        {
            return m[item].Cast<ComboBox>().CurrentValue;
        }

        private static HitChance QHC()
        {
            switch (getBoxItem(miscMenu, "hitchance"))
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                default:
                    return HitChance.High;
            }
        }


        private static void AntiGapcloser_OnEnemyGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {
            if (gapcloser.Sender.IsValidTarget(300f) && _e.IsReady() && getCheckBoxItem(miscMenu, "egapclose"))
            {
                _e.Cast(ObjectManager.Player);
            }
            if (gapcloser.Sender.IsValidTarget(300f) && _q.IsReady() && getCheckBoxItem(miscMenu, "qgapclose"))
            {
                _q.Cast(gapcloser.Sender);
            }

        }

        private static void ChangeMantra()
        {
            var changetime = Environment.TickCount - LastTick;


            if (getKeyBindItem(comboMenu, "Mantra"))
            {
                if (getBoxItem(comboMenu, "MantraMode") == 0 && LastTick + 400 < Environment.TickCount)
                {
                    LastTick = Environment.TickCount;
                    comboMenu["MantraMode"].Cast<ComboBox>().CurrentValue = 1;
                }

                if (getBoxItem(comboMenu, "MantraMode") == 1 && LastTick + 400 < Environment.TickCount)
                {
                    LastTick = Environment.TickCount;
                    comboMenu["MantraMode"].Cast<ComboBox>().CurrentValue = 2;
                }
                if (getBoxItem(comboMenu, "MantraMode") == 2 && LastTick + 400 < Environment.TickCount)
                {
                    LastTick = Environment.TickCount;
                    comboMenu["MantraMode"].Cast<ComboBox>().CurrentValue = 3;
                }
                if (getBoxItem(comboMenu, "MantraMode") == 3 && LastTick + 400 < Environment.TickCount)
                {
                    LastTick = Environment.TickCount;
                    comboMenu["MantraMode"].Cast<ComboBox>().CurrentValue = 0;
                }

            }
        }

        private static void Combo()
        {
            var Target = TargetSelector.GetTarget(_q.Range, DamageType.Magical);
            {
                switch (getBoxItem(comboMenu, "MantraMode"))
                {
                    case 0:
                        if (_r.IsReady() && _q.IsReady() && Target.Distance(Player) <= _q.Range + 50)
                        {
                            _r.Cast();
                            var predQ = _q.GetPrediction(Target);
                            if (_q.IsReady() && predQ.HitChance >= QHC())
                                _q.Cast(Target);
                        }
                        else if (_q.IsReady())
                        {
                            var predQ = _q.GetPrediction(Target);
                            if (_q.IsReady() && predQ.HitChance >= QHC())
                                _q.Cast(Target);
                        }
                        else if (_w.IsReady())
                        {
                            _w.Cast(Target);
                        }
                        else if (_e.IsReady())
                        {
                            _e.Cast(Player);
                        }
                        break;
                    case 1:
                        if (_r.IsReady() && _w.IsReady() && Target.Distance(Player) <= _q.Range + 50)
                        {
                            _r.Cast();
                        }
                        else if (_w.IsReady())
                        {
                            _w.Cast(Target);
                        }
                        else if (_e.IsReady())
                        {
                            _e.Cast(Player);
                        }
                        else if (_q.IsReady())
                        {
                            var predQ = _q.GetPrediction(Target);
                            if (_q.IsReady() && predQ.HitChance >= QHC())
                                _q.Cast(Target);
                        }
                        break;
                    case 2:
                        if (_r.IsReady() && _e.IsReady() && Target.Distance(Player) <= _q.Range + 50)
                        {
                            _r.Cast();

                        }
                        else if (_e.IsReady())
                        {
                            _e.Cast(Player);
                        }
                        else if (_w.IsReady())
                        {
                            _w.Cast(Target);
                        }
                        else if (_q.IsReady())
                        {
                            var predQ = _q.GetPrediction(Target);
                            if (_q.IsReady() && predQ.HitChance >= QHC())
                                _q.Cast(Target);
                        }
                        break;
                    // Auto
                    case 3:
                        if (Player.HealthPercent <= 30 && Target.HealthPercent >= 50)
                        {
                            goto case 2;
                        }
                        if (!Target.IsFacing(Player))
                        {
                            goto case 1;
                        }
                        goto case 0;
                }
             }
        }

        private static void Harass()
        {
            var Target = TargetSelector.GetTarget(_q.Range, DamageType.Magical);
            if (Target == null || !Target.IsValid()) return;

            if (getCheckBoxItem(harassMenu, "HarassR") && _r.IsReady())
            {
                if (_q.IsReady() || _e.IsReady())
                {
                    _r.Cast();
                }
            }
            if (getCheckBoxItem(harassMenu, "HarassQ1") && _q.IsReady())
            {
                if (!(Player.ManaPercent >= getSliderItem(harassMenu, "HarassQ"))) return;
                {
                    var predQ = _q.GetPrediction(Target);
                    if (Target.IsValidTarget(_q.Range) && _q.IsReady() && predQ.HitChance >= QHC())
                        _q.Cast(Target);
                }
            }
            if (getCheckBoxItem(harassMenu, "HarassW1") && _w.IsReady())
            {
                if (!(Player.ManaPercent >= getSliderItem(harassMenu, "HarassW"))) return;
                {
                    _w.Cast(Target);
                }
            }
            if (getCheckBoxItem(harassMenu, "HarassE1") && _e.IsReady())
            {
                if (!(Player.ManaPercent >= getSliderItem(harassMenu, "HarassE"))) return;
                {
                    _e.Cast(Player);
                }
            }
        }

        private static void LaneClear()
        {
            var allMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, _q.Range);

            if (Player.ManaPercent > getSliderItem(laneMenu, "lanemana"))
            {
                if (getCheckBoxItem(laneMenu, "laneq") && _q.IsReady())

                    foreach (var minion in allMinions)
                    {
                        if (minion.IsValidTarget())
                        {
                            _q.Cast(minion);
                        }

                    }
            }
        }

        private static void LastHit()
        {
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault
                    (m =>
                        m.IsValidTarget(_q.Range) &&
                        (Player.GetSpellDamage(m, SpellSlot.Q) > m.TotalShieldHealth() && m.IsEnemy && !m.IsDead && m.IsValid && !m.IsInvulnerable));

            if (minions == null)
            {
                return;
            }

            if (Player.ManaPercent >= getSliderItem(laneMenu, "lanemana"))
            {
                if (_q.IsReady() && getCheckBoxItem(laneMenu, "laneqh"))
                {
                    _q.Cast(minions);
                }
            }

        }

        private static void Flee()
        {
            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                return;
            }

            if (_r.IsReady() && _e.IsReady() && getCheckBoxItem(fleeMenu, "fleee"))
            {
                _r.Cast();
                _e.Cast(Player);
            }
            else if (_e.IsReady())
            {
                _e.Cast(Player);
            }
        }

        private static void KS()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(a => !a.IsDead && !a.IsZombie && a.Health > 0))
            {
                if (enemy == null) return;
                if (enemy.HealthPercent <= 40 && enemy.IsValidTarget(_q.Range))
                {
                    if (getCheckBoxItem(ksMenu, "ksq") && _q.IsReady() && _q.IsInRange(enemy) && KillSteal.IsKillable(enemy, _q.Slot))//!enemy.IsInvulnerable && DamageLib.QCalc(enemy) >= enemy.Health)
                    {
                        var Qp = _q.GetPrediction(enemy);
                        if (Qp.HitChance >= QHC())
                            _q.Cast(Qp.CastPosition);
                    }
                }
            }
        }

        private static void Emanager()
        {
            var useE = getCheckBoxItem(miscMenu, "eactivated");
            var eMana = getSliderItem(miscMenu, "emana");
            var myhp = getSliderItem(miscMenu, "eallyhp");
            var allyHp = getSliderItem(miscMenu, "emyhp");

            if (Player.IsRecalling() || !useE
                || Player.ManaPercent < eMana || !_e.IsReady())
            {
                return;
            }

            if ((Player.Health / Player.MaxHealth) * 100 <= myhp)
            {
                _e.Cast();
            }

            foreach (var hero in ObjectManager.Get<AIHeroClient>().Where(h => h.IsAlly && !h.IsMe))
            {
                if ((hero.Health / hero.MaxHealth) * 100 <= allyHp && _e.IsInRange(hero))
                {
                    _e.Cast();
                }
            }
        }

        private static void Game_OnDraw(EventArgs args)
        {
            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);

            if (getCheckBoxItem(drawMenu, "drawQ"))
            {
                if (_q.IsReady() && _q.IsLearned)
                {
                    Circle.Draw(Color.Cyan, _q.Range, Player.Position);
                }
            }
            if (getCheckBoxItem(drawMenu, "drawW"))
            {
                if (_w.IsReady() && _w.IsLearned)
                {
                    Circle.Draw(Color.Cyan, _w.Range, Player.Position);
                }
            }
            if (getCheckBoxItem(drawMenu, "drawE"))
            {
                if (_e.IsReady() && _e.IsLearned)
                {
                    Circle.Draw(Color.Cyan, _e.Range, Player.Position);
                }
            }

            if (getBoxItem(comboMenu, "MantraMode") == 0)
                {
                    Drawing.DrawText(heropos.X - 15, heropos.Y + 40, System.Drawing.Color.White, "Selected Prio: Q");
                }
                if (getBoxItem(comboMenu, "MantraMode") == 1)
                {
                    Drawing.DrawText(heropos.X - 15, heropos.Y + 40, System.Drawing.Color.White, "Selected Prio: W");
                }
                if (getBoxItem(comboMenu, "MantraMode") == 2)
                {
                    Drawing.DrawText(heropos.X - 15, heropos.Y + 40, System.Drawing.Color.White, "Selected Prio: E");
                }
                if (getBoxItem(comboMenu, "MantraMode") == 3)
                {
                    Drawing.DrawText(heropos.X - 15, heropos.Y + 40, System.Drawing.Color.White, "Selected Prio: Auto");
                }
            }
      

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
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
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHit();
            }

            ChangeMantra();
            Emanager();
            KS();
        }
    }

}