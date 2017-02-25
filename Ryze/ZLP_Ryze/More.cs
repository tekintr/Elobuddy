using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;

namespace ZLP_Ryze
{
    public class More
    {
        public static AIHeroClient TargetQ, TargetE;
        public static Obj_AI_Minion Minion, Monster;
        public static Obj_AI_Base HitQ, HitE, HasE, DieE;
        public static int CountE, CountM;
        public static bool CollisionT, CollisionM, CollisionJ;

        public static void StopAuto(EventArgs args)
        {
            TargetQ = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            TargetE = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);

            if ((Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                 Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) ||
                 Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) &&
                (Spells.W.IsReady() || Spells.E.IsReady()) &&
                TargetQ != null && (TargetE == null || TargetE.Distance(Player.Instance) > 575))
            {
                Orbwalker.DisableAttacking = true;
                return;
            }

            Orbwalker.DisableAttacking = false;
        }

        public static void Combo()
        {
            TargetQ = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (TargetQ == null || !Spells.Q.IsReady()) return;
            var prediction = Spells.Q.GetPrediction(TargetQ);
            CollisionT = prediction.CollisionObjects.Length > 0;
            if (!CollisionT) return;
            TargetE = TargetSelector.GetTarget(Spells.E.Range + 300, DamageType.Magical);
            CountE = EntityManager.Enemies.Count(e => e.HasBuff("RyzeE"));
            HitQ = EntityManager.Enemies.Where(e => e.IsValidTarget(Spells.Q.Range) && e.HasBuff("RyzeE"))
                                        .OrderBy(e => e.Distance(Player.Instance.Position)).FirstOrDefault();

            if (TargetE != null)
            {
                HitE = EntityManager.Enemies.Where(e => e.IsValidTarget(Spells.E.Range))
                                            .OrderBy(e => e.Distance(TargetE.Position)).FirstOrDefault();
                HasE = EntityManager.Enemies.Where(e => e.IsValidTarget(Spells.E.Range) && e.HasBuff("RyzeE"))
                                            .OrderBy(e => e.Distance(TargetE.Position)).FirstOrDefault();
                DieE = EntityManager.Enemies
                       .Where(e => e.IsValidTarget(Spells.E.Range) && e.Health <= Spells.E.GetSpellDamage(e))
                       .OrderBy(e => e.Distance(TargetE.Position)).FirstOrDefault();
            }
        }

        public static void Lane()
        {
            Minion = EntityManager.MinionsAndMonsters.EnemyMinions
                     .Where(m => m.IsValidTarget(Spells.Q.Range) && m.Health <= Spells.Q.GetSpellDamage(m))
                     .OrderBy(m => m.Distance(Player.Instance.Position)).FirstOrDefault();
            TargetE = TargetSelector.GetTarget(Spells.E.Range + 300, DamageType.Magical);
            CountE = EntityManager.MinionsAndMonsters.EnemyMinions.Count(m => m.HasBuff("RyzeE"));

            if (Minion == null || !Spells.Q.IsReady())
                CollisionM = false;

            else
            {
                var prediction = Spells.Q.GetPrediction(Minion);
                CollisionM = prediction.CollisionObjects.Length > 0;
            }

            if (TargetE == null)
            {
                HitE = EntityManager.MinionsAndMonsters.EnemyMinions
                       .Where(m => m.IsValidTarget(Spells.E.Range))
                       .OrderBy(m => m.MaxHealth).LastOrDefault();
                HasE = EntityManager.MinionsAndMonsters.EnemyMinions
                       .Where(m => m.IsValidTarget(Spells.E.Range) && m.HasBuff("RyzeE"))
                       .OrderBy(m => m.MaxHealth).LastOrDefault();
                DieE = EntityManager.MinionsAndMonsters.EnemyMinions
                       .Where(m => m.IsValidTarget(Spells.E.Range) && m.Health <= Spells.E.GetSpellDamage(m))
                       .OrderBy(m => m.MaxHealth).LastOrDefault();
            }

            else
            {
                HitE = EntityManager.MinionsAndMonsters.EnemyMinions
                       .Where(m => m.IsValidTarget(Spells.E.Range))
                       .OrderBy(m => m.Distance(TargetE.Position)).FirstOrDefault();
                HasE = EntityManager.MinionsAndMonsters.EnemyMinions
                       .Where(m => m.IsValidTarget(Spells.E.Range) && m.HasBuff("RyzeE"))
                       .OrderBy(m => m.Distance(TargetE.Position)).FirstOrDefault();
                DieE = EntityManager.MinionsAndMonsters.EnemyMinions
                       .Where(m => m.IsValidTarget(Spells.E.Range) && m.Health <= Spells.E.GetSpellDamage(m))
                       .OrderBy(m => m.Distance(TargetE.Position)).FirstOrDefault();
            }
        }

        public static void Jungle()
        {
            Monster = EntityManager.MinionsAndMonsters.Monsters
                      .Where(m => m.IsValidTarget(Spells.Q.Range))
                      .OrderBy(m => m.MaxHealth).LastOrDefault();
            CountE = EntityManager.MinionsAndMonsters.Monsters.Count(m => m.HasBuff("RyzeE"));
            CountM = EntityManager.MinionsAndMonsters.Monsters.Count(m => m.IsInRange(Player.Instance.Position, 700));

            if (Monster == null || !Spells.Q.IsReady())
                CollisionJ = false;

            else
            {
                var prediction = Spells.Q.GetPrediction(Monster);
                CollisionJ = prediction.CollisionObjects.Length > 0;
            }

            HitE = EntityManager.MinionsAndMonsters.Monsters
                   .Where(m => m.IsValidTarget(Spells.E.Range))
                   .OrderBy(m => m.MaxHealth).LastOrDefault();
            HasE = EntityManager.MinionsAndMonsters.Monsters
                   .Where(m => m.IsValidTarget(Spells.E.Range) && m.HasBuff("RyzeE"))
                   .OrderBy(m => m.MaxHealth).LastOrDefault();
            DieE = EntityManager.MinionsAndMonsters.Monsters
                   .Where(m => m.IsValidTarget(Spells.E.Range) && m.Health <= Spells.E.GetSpellDamage(m))
                   .OrderBy(m => m.MaxHealth).LastOrDefault();
        }

        public static bool Unkillable(AIHeroClient target)
        {
            if (target.Buffs.Any(b => b.IsValid() && b.DisplayName == "UndyingRage"))
                return true;
            if (target.Buffs.Any(b => b.IsValid() && b.DisplayName == "ChronoShift"))
                return true;
            if (target.Buffs.Any(b => b.IsValid() && b.DisplayName == "JudicatorIntervention"))
                return true;
            if (target.Buffs.Any(b => b.IsValid() && b.DisplayName == "kindredrnodeathbuff"))
                return true;
            if (target.HasBuffOfType(BuffType.Invulnerability))
                return true;

            return target.IsInvulnerable;
        }

        public static float LastCast;

        public static bool CanCast()
        {
            var delay = new Random().Next(Menus.MinDelay.CurrentValue, Menus.MaxDelay.CurrentValue);
            return !Menus.Main["human"].Cast<CheckBox>().CurrentValue || 
                   LastCast * 1000 + delay <= Game.Time * 1000;
        }

        public static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || args.Slot == SpellSlot.Recall) return;
            LastCast = Game.Time;
        }

        public static HitChance Hit()
        {
            switch (Menus.Main["hit"].Cast<ComboBox>().CurrentValue)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                default:
                    return HitChance.Unknown;
            }
        }
    }
}