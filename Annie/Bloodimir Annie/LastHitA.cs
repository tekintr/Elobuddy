﻿using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace Bloodimir_Annie
{
    internal static class LastHitA
    {
        private static AIHeroClient Annie
        {
            get { return ObjectManager.Player; }
        }

        private static float Qcalc(Obj_AI_Base target)
        {
            return Annie.CalculateDamageOnUnit(target, DamageType.Magical,
                (new float[] {0, 80, 115, 150, 185, 220}[Program.Q.Level] +
                 (0.80f*Annie.FlatMagicDamageMod)));
        }

        private static Obj_AI_Base MinionLh(GameObjectType type)
        {
            return ObjectManager.Get<Obj_AI_Base>().OrderBy(a => a.Health).FirstOrDefault(a => a.IsEnemy
                                                                                               && a.Type == type
                                                                                               &&
                                                                                               a.Distance(Annie) <=
                                                                                               Program.Q.Range
                                                                                               && !a.IsDead
                                                                                               && !a.IsInvulnerable
                                                                                               &&
                                                                                               a.IsValidTarget(
                                                                                                   Program.Q.Range)
                                                                                               &&
                                                                                               a.Health <= Qcalc(a));
        }

        public static void LastHitB()
        {
            var qcheck = Program.LastHit["LHQ"].Cast<CheckBox>().CurrentValue;
            var qready = Program.Q.IsReady();
            if (!qcheck || !qready || Annie.HasBuff("pyromania_particle")) return;
            var minion = (Obj_AI_Minion) MinionLh(GameObjectType.obj_AI_Minion);
            if (minion != null)
            {
                Program.Q.Cast(minion);
            }
        }
    }
}