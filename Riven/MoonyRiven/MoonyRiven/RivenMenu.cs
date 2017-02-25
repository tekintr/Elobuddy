using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace MoonyRiven
{
    public enum CastType
    {
        NONE = -1,AA,Q,W,E,R1,R2,H,F
    }
    public static class RivenMenu
    {
        public static Menu Main, Combo, Clear, Flee, Draw, Misc, WallJump;
        public static void Init()
        {
            Main = MainMenu.AddMenu("Moony Riven 2.0", "TRMoony Riven 2.0");
            Main.AddGroupLabel("By DanThePman");
            Combo = Main.AddSubMenu("Combo");
            Combo.AddGroupLabel("General");
            Combo.Add("Q", new CheckBox("Kullan Q"));
            Combo.Add("QGap", new CheckBox("Kullan Q Atilanlara"));
            Combo.Add("W", new CheckBox("Kullan W"));
            Combo.Add("InstaW", new CheckBox("Kullan W Menzildekilere Aninda"));
            Combo.Add("E", new CheckBox("Kullan E"));
            Combo.Add("EGap", new CheckBox("Kullan E Atilanlara"));
            Combo.Add("R1", new KeyBind("Kullan R1", false, KeyBind.BindTypes.PressToggle));
            Combo.Add("R2", new CheckBox("Kullan R2"));
            Combo.Add("H", new CheckBox("Kullan Hydra/Tiamat"));
            Combo.AddSeparator();
            Combo.Add("minR2Hits", new Slider("Sadere oldurmede R2 aktif X su kadar dusmana", 1, 1, 5));
            Combo.Add("onlyR2ToKs", new CheckBox("Sadece oldururken kullan R2 (Yavas kombo)", false));
            Combo.AddSeparator();
            Combo.Add("burstKey", new KeyBind("Shy Burst Key", false, KeyBind.BindTypes.PressToggle));
            Combo.AddLabel("Press combo while the burst key is activated");
            Combo.AddSeparator(50);

            /*AA, Q, W, E, R1, R2, Hydra*/
            Combo.AddGroupLabel("Advanced Priorities | Drawable");
            Combo.AddLabel("0 = Wont Be Used");
            Combo.AddSeparator(10);
            Combo.Add("Advanced_E_Q", new Slider("E -> Q", 0));
            Combo.Add("Advanced_E_W", new Slider("E -> W", 5));
            Combo.Add("Advanced_E_H", new Slider("E -> Hydra", 50));

            Combo.Add("Advanced_H_Q", new Slider("Hydra -> Q", 49));
            Combo.Add("Advanced_H_W", new Slider("Hydra -> W", 96));
            Combo.Add("Advanced_Q_W", new Slider("Q -> W", 0));
            Combo.Add("Advanced_W_Q", new Slider("W -> Q", 0));
            Combo.Add("Advanced_W_H", new Slider("W -> Hydra", 2));
            Combo.Add("Advanced_W_R2", new Slider("W -> R2", 100));//burst combo end old
            Combo.Add("Advanced_E_R1", new Slider("E -> R1", 97));//r1 init
            Combo.Add("Advanced_R1_W", new Slider("R1 -> W", 100));//r1
            Combo.Add("Advanced_R1_Q", new Slider("R1 -> Q", 0));
            Combo.Add("Advanced_R1_H", new Slider("R1 -> Hydra", 0));
            Combo.Add("Advanced_H_R1", new Slider("Hydra -> R1", 100));//r1 init
            Combo.Add("Advanced_R2_W", new Slider("R2 -> W", 96));//r2
            Combo.Add("Advanced_R2_Q", new Slider("R2 -> Q", 95));//r2
            Combo.Add("Advanced_H_R2", new Slider("Hydra -> R2", 99));//r2
            Combo.Add("Advanced_F_H", new Slider("Flash -> Hydra (Shy Burst)", 200, 0, 200));//r1 -> flash
            Combo.Add("Advanced_F_W", new Slider("Flash -> W (Burst)", 199, 0, 200));//r1 -> flash

            Combo.AddGroupLabel("DPS Mode");
            Combo.Add("Advanced_AA_E", new Slider("AA -> E", 101, 0, 110));
            Combo.Add("Advanced_AA_Q", new Slider("AA -> Q", 103, 0, 110));
            Combo.Add("Advanced_Q_AA", new Slider("Q -> AA", 103, 0, 110));
            Combo.Add("Advanced_AA_W", new Slider("AA -> W", 104, 0, 110));
            Combo.Add("Advanced_AA_H", new Slider("AA -> Hydra", 105, 0, 110));//aa -> hydra -> w
            Combo.Add("Advanced_AA_R1", new Slider("AA -> R1", 106, 0, 110));
            Combo.Add("Advanced_AA_R2", new Slider("AA -> R2", 107, 0, 110));//burst combo end new
            Combo.Add("Advanced_W_AA", new Slider("W -> AA", 110, 0, 110));//burst combo end new

            Clear = Main.AddSubMenu("Clears");
            Clear.AddGroupLabel("Lane Clear");
            Clear.Add("QL", new CheckBox("Kullan Q"));
            Clear.Add("WL", new CheckBox("Kullan W"));
            Clear.Add("EL", new CheckBox("Kullan E"));
            Clear.Add("HL", new CheckBox("Kullan Hydra/Tiamat"));
            Clear.AddSeparator();
            Clear.AddGroupLabel("Jungle Clear");
            Clear.Add("QJ", new CheckBox("Kullan Q"));
            Clear.Add("WJ", new CheckBox("Kullan W"));
            Clear.Add("EJ", new CheckBox("Kullan E"));
            Clear.Add("HJ", new CheckBox("Kullan Hydra/Tiamat"));

            Draw = Main.AddSubMenu("Drawings");
            Draw.Add("drawR1Status", new CheckBox("Goster R1 Aktifken"));
            Draw.Add("drawRExpiry", new CheckBox("Goster R2 KalanSure"));
            Draw.Add("drawBurstStatus", new CheckBox("Goster Seri Hamle"));
            Draw.Add("drawBurstRange", new CheckBox("Goster Patlama Menzili"));
            Draw.Add("drawSpots", new CheckBox("Goster Atlanabilicek yerler"));
            Draw.AddSeparator();
            Draw.AddGroupLabel("Combo Priorities");
            Draw.Add("drawDependencies", new CheckBox("Kombo baglilik diagramini ciz"));
            Draw.AddLabel("Draws the Combinations with the highest priority");
            Draw.AddSeparator();
            Draw.Add("onlyMaxPrioDependency", new CheckBox("Sadece enyuksek oncelikli komboyu goster", false));
            Draw.Add("dependencyRoot", new ComboBox("Baslangic Sklleri", 2, "Q", "W", "E", "R1", "R2", "H", "F"));

            Misc = Main.AddSubMenu("Misc");
            Misc.Add("qDelay", new Slider("Q1 ve Q2 Gecikmesi", 29));
            Misc.Add("q3Delay", new Slider("Q3 Gecikmesi", 42));
            Misc.Add("continueQ", new CheckBox("Sure dolunca devam Q"));
            Misc.Add("interrupt", new CheckBox("Interrupter"));
            Misc.Add("antiGap", new CheckBox("Atilma onleyicisi"));

            WallJump = Main.AddSubMenu("Wall Jump");
            WallJump.AddGroupLabel("How To Trigger a Wall Jump:");
            WallJump.AddLabel("1. Secenek: Kacma modunu etkinlestir ve kacicagin yere sol tikla");
            WallJump.AddLabel("2. Secenek: Bir noktaya cift tiklayin hangi modda olursaniz olun calisir");
            WallJump.AddSeparator();
            WallJump.AddGroupLabel("How To Abort a Wall Jump:");
            WallJump.AddLabel("1. Secenek Kacis modunu kapat");
            WallJump.AddLabel("2. Secenek Baska bir yere gitmek icin sag tikla");
        }

        public static Obj_AI_Base GetTarget
        {
            get
            {
                Obj_AI_Base target = null;
                if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
                    target = TargetSelector.GetTarget(250 + Player.Instance.AttackRange + 70, DamageType.Physical);
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                    var Mobs =
                        EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position,
                            250 + Player.Instance.AttackRange + 70)
                            .OrderByDescending(x => x.MaxHealth).ToList();
                    target = Mobs.FirstOrDefault();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    var Mobs =
                        EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, 250 + Player.Instance.AttackRange + 70)
                            .OrderByDescending(x => x.MaxHealth).ToList();
                    if (target == null)
                    {
                        target = Mobs.FirstOrDefault();
                    }
                }

                //if (RivenMenu.combo["burst"].Cast<KeyBind>().CurrentValue)
                //    target = TargetSelector.SelectedTarget;

                return target;
            }
        }

        public static bool IsEnabled(string spell)
        {
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass)
                return Combo[spell].Cast<CheckBox>().CurrentValue;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && 
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, 400).Any())
            {
                return Clear[spell + "L"].Cast<CheckBox>().CurrentValue;
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                EntityManager.MinionsAndMonsters.Monsters.Any(x => x.IsValid && x.Distance(Player.Instance) <= 400))
            {
                return Clear[spell + "J"].Cast<CheckBox>().CurrentValue;
            }
            return false;
        }

        public static float GetRDamage(Obj_AI_Base target)
        {
            if (!(target is AIHeroClient))
                return -1;
            float missinghealth = (target.MaxHealth - target.Health) / target.MaxHealth > 0.75f ? 0.75f : (target.MaxHealth - target.Health) / target.MaxHealth;
            float pluspercent = missinghealth * (8f / 3f);
            var rawdmg = new float[] { 0, 100, 150, 200 }[Spells.R2.Level] + 0.6f * Player.Instance.FlatPhysicalDamageMod;
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, rawdmg * (1 + pluspercent)*2);
        }

        public static bool IsSpellForTargetViable_NextSpell(this string s)
        {
            CastType t = (CastType)Enum.Parse(typeof (CastType), s);
            switch (t)
            {
                case CastType.AA: return true;
                case CastType.Q: return Spells.Q.IsReady() && IsEnabled("Q");
                case CastType.W: return Spells.W.IsReady() && IsEnabled("W");
                case CastType.E: return Spells.E.IsReady() && IsEnabled("E") && !Combo["burstKey"].Cast<KeyBind>().CurrentValue;
                case CastType.R1: return Spells.R1.IsReady() && GetTarget is AIHeroClient && GetTarget.IsValid &&
                        Player.Instance.Spellbook.GetSpell(SpellSlot.R).Name == "RivenFengShuiEngine" &&
                        !Combo["burstKey"].Cast<KeyBind>().CurrentValue && Combo["R1"].Cast<KeyBind>().CurrentValue;
                case CastType.R2:
                    var pred = Prediction.Position.PredictUnitPosition(GetTarget, 
                        (int)(Spells.R2.CastDelay + GetTarget.Distance(Player.Instance) / Spells.R2.Speed * 1000));

                    bool isViable =
                        (Combo["onlyR2ToKs"].Cast<CheckBox>().CurrentValue && GetRDamage(GetTarget) > GetTarget.Health &&
                            pred.Distance(Player.Instance) <= Spells.R2.Range)
                        ||
                        (UltimatePrediction.GetUltimateHits() >= Combo["minR2Hits"].Cast<Slider>().CurrentValue &&
                            !Combo["onlyR2ToKs"].Cast<CheckBox>().CurrentValue);

                    return Spells.R2.IsReady() && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Name == "RivenIzunaBlade" && isViable && IsEnabled("R2");
                case CastType.H:
                    if (Spells.Hydra.IsOwned()) return Spells.Hydra.IsReady() && IsEnabled("H");
                    if (Spells.Tiamat.IsOwned()) return Spells.Tiamat.IsReady() && IsEnabled("H");
                    break;

               /*Burst Mode Flash after R1*/
               case CastType.F: return Spells.Flash.IsReady() && Combo["burstKey"].Cast<KeyBind>().CurrentValue;

            }
            return false;
        }

        public static bool IsTargetInRange_NextSpell(this string s)
        {
            CastType t = (CastType)Enum.Parse(typeof(CastType), s);
            switch (t)
            {
                case CastType.AA: return GetTarget != null && Player.Instance.IsInAutoAttackRange(GetTarget);
                case CastType.Q: return GetTarget != null && Player.Instance.Distance(GetTarget) <= Spells.Q.Range;
                case CastType.W: return GetTarget != null && Player.Instance.Distance(GetTarget) <= Spells.W.Range;
                case CastType.E: return GetTarget != null && Player.Instance.Distance(GetTarget) <= Spells.E.Range;
                case CastType.R1: return true;
                case CastType.R2:
                    if (GetTarget != null)
                    {
                        var pred = Prediction.Position.PredictUnitPosition(GetTarget, (int)(Spells.R2.CastDelay + GetTarget.Distance(Player.Instance) / Spells.R2.Speed * 1000));
                        return Player.Instance.Distance(pred) <= Spells.R2.Range;
                    }
                    return false;
                case CastType.H:
                    if (Spells.Hydra.IsOwned()) return GetTarget != null && (Player.Instance.Distance(GetTarget) <= Spells.Hydra.Range || Combo["burstKey"].Cast<KeyBind>().CurrentValue);
                    if (Spells.Tiamat.IsOwned()) return GetTarget != null && (Player.Instance.Distance(GetTarget) <= Spells.Tiamat.Range || Combo["burstKey"].Cast<KeyBind>().CurrentValue);
                    break;
                
                /*Flash Burst from R1*/
                case CastType.F: return GetTarget != null && GetTarget.Distance(Player.Instance) <= Spells.Flash.Range;

            }
            return false;
        }

        public static CastType? GetNextCastTyoe(CastType LastCastType)
        {
            string s = $"Advanced_{LastCastType}_";
            string bestComboString = string.Empty;
            int highestPrio = 0;
            CastType bestCastType = CastType.NONE;

            foreach (CastType castType in Enum.GetValues(typeof(CastType)))
            {
                if (castType == CastType.NONE)
                    continue;

                string searched = s + castType;
                var valueBase = Combo[searched];
                if (valueBase != null) //combo exists
                {
                    int prio = valueBase.Cast<Slider>().CurrentValue;
                    string nextSpell = castType.ToString();
                    if (prio > highestPrio && nextSpell.IsSpellForTargetViable_NextSpell() && nextSpell.IsTargetInRange_NextSpell())
                    {
                        highestPrio = prio;
                        bestComboString = valueBase.Cast<Slider>().SerializationId;
                        bestCastType = (CastType) Enum.Parse(typeof (CastType), nextSpell);
                    }
                }
            }

            if (bestComboString == string.Empty)
                return null;
            return bestCastType;
        }
    }
}
