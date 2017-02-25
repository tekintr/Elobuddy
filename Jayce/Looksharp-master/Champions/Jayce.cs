using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Looksharp.Champions
{
    internal class Jayce : Base
    {
        private static Spell.SpellBase QE;
        private static Vector3 gatePos;
        private static float LastCast;

        static Jayce()
        {
            QE = new Spell.Skillshot(SpellSlot.Q, 1650, SkillShotType.Linear, 25, 1900, 70);
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W, 285);
            E = new Spell.Targeted(SpellSlot.E, 240);
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1030, SkillShotType.Linear, 25, 1450, 70)
            {
                MinimumHitChance = HitChance.High
            };
            W2 = new Spell.Active(SpellSlot.W);
            E2 = new Spell.Skillshot(SpellSlot.E, 650, SkillShotType.Circular, 1, int.MaxValue, 120);
            R = new Spell.Active(SpellSlot.R);

            CreateMenu();
            ModeMenu = PluginMenu.AddSubMenu("Modes", "Modes");
            ModeMenu.AddGroupLabel("Combo");
            ModeMenu.Add("jayce.combo.q", new CheckBox("Kullan Q Cekic"));
            ModeMenu.Add("jayce.combo.q2", new CheckBox("Kullan Q Top"));
            ModeMenu.Add("jayce.combo.w", new CheckBox("Kullan W Cekic"));
            ModeMenu.Add("jayce.combo.w2", new CheckBox("Kullan W Top"));
            ModeMenu.Add("jayce.combo.e", new CheckBox("Kullan E Cekic"));
            ModeMenu.Add("jayce.combo.qe", new CheckBox("Kullan QE Top"));
            ModeMenu.Add("jayce.combo.r", new CheckBox("Gecis formu"));
            
            ModeMenu.AddGroupLabel("Harass");
            ModeMenu.Add("jayce.harass.q2", new CheckBox("Kullan Q Top"));
            ModeMenu.Add("jayce.harass.w2", new CheckBox("Kullan W Top"));
            ModeMenu.Add("jayce.harass.qe", new CheckBox("Kullan QE Top"));

            ModeMenu.AddGroupLabel("Flee");
            ModeMenu.Add("jayce.flee.q", new CheckBox("Kullan Q Cekic"));
            ModeMenu.Add("jayce.flee.e", new CheckBox("Kullan E Cekic"));
            ModeMenu.Add("jayce.flee.e2", new CheckBox("Kullan E Top"));

            MiscMenu = PluginMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Key Binds");
            MiscMenu.Add("jayce.quickscope", new KeyBind("Quickscope", false, KeyBind.BindTypes.HoldActive, 'A'));
            MiscMenu.Add("jayce.insec", new KeyBind("Insec", false, KeyBind.BindTypes.HoldActive, 'G'));
            MiscMenu.Add("jayce.flashinsec", new CheckBox("->Kullan Sicra Insec"));

            MiscMenu.AddGroupLabel("Settings");
            MiscMenu.Add("jayce.gapcloser", new CheckBox("Kullan E Atilma yapana"));
            MiscMenu.Add("jayce.interrupt", new CheckBox("Kullan E kesici"));
            MiscMenu.Add("jayce.parallel", new CheckBox("Kapiyi paralel yerlestr", false));
            MiscMenu.Add("jayce.gatedistance", new Slider("Kapi uzakligi", 60, 60, 400));

            MiscMenu.AddGroupLabel("Kill Steal");
            MiscMenu.Add("jayce.killsteal.qe", new CheckBox("Kullan QE Top"));
            MiscMenu.Add("jayce.killsteal.q2", new CheckBox("Kullan Q Top"));
            MiscMenu.Add("jayce.killsteal.e", new CheckBox("Kullan E Cekic"));

            DrawMenu = PluginMenu.AddSubMenu("Drawing", "Drawing");
            DrawMenu.AddGroupLabel("Spell Ranges");
            DrawMenu.Add("jayce.draw.q", new CheckBox("Goster Q Cekic"));
            DrawMenu.Add("jayce.draw.q2", new CheckBox("Goster Q Top"));
            DrawMenu.Add("jayce.draw.qe", new CheckBox("Goster QE Top"));

            DrawMenu.AddGroupLabel("Other");
            DrawMenu.Add("jayce.draw.cds", new CheckBox("Goster Bekleme suresi"));

            Gapcloser.OnGapcloser += OnGapCloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        private static void OnGapCloser(AIHeroClient target, Gapcloser.GapcloserEventArgs spell)
        {
            if (MiscMenu["jayce.gapcloser"].Cast<CheckBox>().CurrentValue && target.IsEnemy && CD[2] == 0 && E.IsInRange(target) && (isMelee || ((!isMelee && R.IsReady() && R.Cast()))))
            {
                E.Cast(target);
            }
        }

        private static void OnInterruptableSpell(Obj_AI_Base target, Interrupter.InterruptableSpellEventArgs spell)
        {
            if (MiscMenu["jayce.interrupt"].Cast<CheckBox>().CurrentValue && target.IsEnemy && CD[2] == 0 && E.IsInRange(target) && (isMelee || ((!isMelee && R.IsReady() && R.Cast()))))
            {
                E.Cast(target);
            }
        }

        public override void Update()
        {
            ShouldE();
            UpdateCooldowns();
            if (MiscMenu["jayce.quickscope"].Cast<KeyBind>().CurrentValue)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                if (!isMelee && Q.IsReady() && E.IsReady())
                {
                    CastQE(Game.CursorPos);
                }
            }
            if (MiscMenu["jayce.insec"].Cast<KeyBind>().CurrentValue)
            {
                Insec();
            }
        }

        public override void Draw()
        {
            if (DrawMenu["jayce.draw.q"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(CD[0] == 0 ? Color.Green : Color.Red, Q.Range, hero.Position);
            if (DrawMenu["jayce.draw.q2"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(CD[3] == 0 ? Color.Green : Color.Red, Q2.Range, hero.Position);
            if (DrawMenu["jayce.draw.qe"].Cast<CheckBox>().CurrentValue)
                Circle.Draw((CD[3] == 0 && CD[5] == 0) ? Color.Green : Color.Red, QE.Range, hero.Position);

            if (DrawMenu["jayce.draw.cds"].Cast<CheckBox>().CurrentValue)
            {
                DrawCooldowns();
            }

            if (MiscMenu["jayce.insec"].Cast<KeyBind>().CurrentValue)
            {
                DrawInsec(TargetSelector.GetTarget(QE.Range, DamageType.Physical));
            }
        }

        public override void Combo()
        {
            AIHeroClient target = TargetSelector.GetTarget(QE.Range, DamageType.Physical);
            if (IsValidTarget(target))
            {
                if (isMelee)
                {
                    if (ModeMenu["jayce.combo.q"].Cast<CheckBox>().CurrentValue && Q.IsReady() && Q.IsInRange(target))
                    {
                        Q.Cast(target);
                    }
                    if (ModeMenu["jayce.combo.w"].Cast<CheckBox>().CurrentValue && W.IsReady() && W.IsInRange(target))
                    {
                        W.Cast();
                    }
                    if (ModeMenu["jayce.combo.r"].Cast<CheckBox>().CurrentValue && !Q.IsReady() && !W.IsReady() && R.IsReady())
                    {
                        R.Cast();
                    }
                }
                else
                {

                    ShouldE();
                    if (ModeMenu["jayce.combo.qe"].Cast<CheckBox>().CurrentValue && Q.IsReady() && E.IsReady() && E.IsLearned)
                    {
                        CastQE(target);
                    }
                    if (ModeMenu["jayce.combo.q2"].Cast<CheckBox>().CurrentValue && Q2.IsReady() && !E.IsReady())
                    {
                        Q2.Cast(target);
                    }
                    if (ModeMenu["jayce.combo.w2"].Cast<CheckBox>().CurrentValue && W2.IsReady() && hero.Distance(target.Position) < hero.AttackRange + 100)
                    {
                        W2.Cast();
                    }
                    if (ModeMenu["jayce.combo.r"].Cast<CheckBox>().CurrentValue && !Q2.IsReady() && !W2.IsReady() && R.IsReady() && hero.Distance(target.Position) < Q.Range + 20 && CD[0] == 0)
                    {
                        R.Cast();
                    }
                }
            }
        }

        public override void Harass()
        {
            AIHeroClient target = TargetSelector.GetTarget(QE.Range, DamageType.Physical);
            if (IsValidTarget(target))
            {
                if (!isMelee || ((isMelee && R.IsReady() && R.Cast())))
                {
                    ShouldE();
                    if (ModeMenu["jayce.harass.qe"].Cast<CheckBox>().CurrentValue && Q.IsReady() && E.IsReady() && E.IsLearned)
                    {
                        CastQE(target);
                    }
                    if (ModeMenu["jayce.harass.q2"].Cast<CheckBox>().CurrentValue && Q2.IsReady() && !E.IsReady())
                    {
                        Q2.Cast(target);
                    }
                    if (ModeMenu["jayce.harass.w2"].Cast<CheckBox>().CurrentValue && W2.IsReady() && hero.Distance(target.Position) <= hero.AttackRange + 25)
                    {
                        W2.Cast();
                    }
                }
            }
        }

        public override void Flee()
        {
            if (isMelee)
            {
                if (ModeMenu["jayce.flee.q"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {
                    AIHeroClient bestChampion = EntityManager.Heroes.Enemies.OrderBy(x => x.Distance(Game.CursorPos))
                    .Where(x => Q.IsInRange(x) && x.Distance(Game.CursorPos) + 200 < hero.Distance(Game.CursorPos)).FirstOrDefault();
                    if (bestChampion != null)
                    {
                        Q.Cast(bestChampion);
                    }
                    else
                    {
                        Obj_AI_Minion bestMinion = EntityManager.MinionsAndMonsters.CombinedAttackable.OrderBy(x => x.Distance(Game.CursorPos))
                        .Where(x => Q.IsInRange(x) && x.Distance(Game.CursorPos) + 200 < hero.Distance(Game.CursorPos)).FirstOrDefault();
                        if (bestMinion != null)
                        {
                            Q.Cast(bestMinion);
                        }
                    }
                }
            }
            else
            {
                if (ModeMenu["jayce.flee.e2"].Cast<CheckBox>().CurrentValue && E.IsReady())
                {
                    E2.Cast(Helper.extend(hero.Position, Game.CursorPos, 80, 1));
                }
            }
            if (R.IsReady()) R.Cast();
        }

        public override void Killsteal()
        {
            foreach (AIHeroClient target in EntityManager.Heroes.Enemies.OrderByDescending(x => x.Health)
                .Where(x => x.IsValidTarget(QE.Range) && x.Health < hero.Level * 150 && IsKillable(x)))
            {
                // QE cannon
                if (MiscMenu["jayce.killsteal.qe"].Cast<CheckBox>().CurrentValue && (Q2dmg(target) * 1.4f) > target.Health &&
                    CD[3] == 0 && CD[5] == 0 && ((Spell.Skillshot)QE).GetPrediction(target).HitChance >= HitChance.High &&
                    (!isMelee || ((isMelee && R.IsReady() && R.Cast()))))
                {
                    CastQE(target);
                    return;
                }
                // Q cannon
                if (MiscMenu["jayce.killsteal.q2"].Cast<CheckBox>().CurrentValue && (Q2dmg(target)) > target.Health &&
                    CD[3] == 0 && CD[5] != 0 && (!isMelee || ((isMelee && R.IsReady() && R.Cast()))))
                {
                    Q2.Cast(target);
                    return;
                }
                // E hammer
                if (MiscMenu["jayce.killsteal.e"].Cast<CheckBox>().CurrentValue && (Edmg(target)) > target.Health &&
                    CD[2] == 0 && E.IsInRange(target) && (isMelee || ((!isMelee && R.IsReady() && R.Cast()))))
                {
                    E.Cast(target);
                    return;
                }
            }
        }

        private static void CastQE(AIHeroClient target)
        {
            PredictionResult QEPred = ((Spell.Skillshot)QE).GetPrediction(target);
            if (QEPred.HitChance >= HitChance.High)
            {
                CastQE(QEPred.CastPosition);
            }
            else if (QEPred.HitChance == HitChance.Collision)
            {
                Obj_AI_Base collision = QEPred.CollisionObjects.OrderBy(x => x.Distance(hero.Position)).First();
                if (Helper.extend(collision.Position, QEPred.UnitPosition, collision.BoundingRadius, -1).Distance(QEPred.UnitPosition) < 160)
                {
                    CastQE(QEPred.CastPosition);
                }
            }
        }

        private static void CastQE(Vector3 position)
        {
            LastCast = Game.Time;
            gatePos = Helper.extend(hero.Position, position, MiscMenu["jayce.gatedistance"].Cast<Slider>().CurrentValue, 1); // in front, horizontal

            if (MiscMenu["jayce.parallel"].Cast<CheckBox>().CurrentValue)
            {
                gatePos = new Vector3(hero.Position.Y + hero.Position.X - gatePos.Y, hero.Position.Y - hero.Position.X + gatePos.X, hero.Position.Z);
            }
            QE.Cast(position);
        }

        private static void Insec()
        {
            AIHeroClient target = TargetSelector.GetTarget(QE.Range, DamageType.Physical);
            if (IsValidTarget(target))
            {
                Vector3 insecPos = Helper.extend(target.Position, Game.CursorPos, 150, -1);
                Player.IssueOrder(GameObjectOrder.MoveTo, insecPos);

                if ((isMelee || ((!isMelee && R.IsReady() && R.Cast()))) && E.IsReady()) // melee and e is ready
                {
                    if (ShouldInsec(target.Position, insecPos, E.Range))
                    {
                        E.Cast(target);
                        return;
                    }
                    if (hero.Distance(target.Position) + 30 < hero.Distance(insecPos))
                    {
                        // jumping/flashing to target will be faster than walking there
                        if (Q.IsReady() && Q.IsInRange(target))
                        {
                            Q.Cast(target);
                        }
                        if (MiscMenu["jayce.flashinsec"].Cast<CheckBox>().CurrentValue && hero.Distance(insecPos) < 410 && hero.Distance(insecPos) > 160)
                        {
                            SpellDataInst spell = hero.Spellbook.Spells.FirstOrDefault(a => a.Name.ToLower().Contains("summonerflash"));
                            if (spell != null && spell.IsReady)
                            {
                                hero.Spellbook.CastSpell(spell.Slot, insecPos);
                            }
                        }
                    }
                }
            }
        }

        private static void ShouldE()
        {
            if (CD[5] == 0 && Game.Time - LastCast < 0.20)
            {
                E2.Cast(gatePos);
            }
        }

        private static float Qdmg(Obj_AI_Base target)
        {
            return hero.CalculateDamageOnUnit(target, DamageType.Physical, new float[] { 0, 40, 80, 120, 160, 200, 240 }[Q.Level] + 1.2f * hero.FlatPhysicalDamageMod);
        }

        private static float Edmg(Obj_AI_Base target)
        {
            return hero.CalculateDamageOnUnit(target, DamageType.Magical, new float[] { 0, 8f, 10.4f, 12.8f, 15.2f, 17.6f, 20f }[E.Level] * (target.MaxHealth / 100) + hero.FlatPhysicalDamageMod);
        }

        private static float Q2dmg(Obj_AI_Base target)
        {
            return hero.CalculateDamageOnUnit(target, DamageType.Physical, new float[] { 0, 70, 120, 170, 220, 270, 320 }[Q.Level] + 1.2f * hero.FlatPhysicalDamageMod);
        }
    }
}
