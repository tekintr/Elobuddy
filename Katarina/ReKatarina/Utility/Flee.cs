using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System;
using System.Linq;

namespace ReKatarina.Utility
{
    public static class Flee
    {
        public static bool IsAlly(Vector3 pos)
        {
            return EntityManager.Heroes.Allies.Any(a => !a.IsMe && a.Distance(pos) <= ConfigList.Flee.JumpCursorRange);
        }
        public static bool IsAllyMinion(Vector3 pos)
        {
            return EntityManager.MinionsAndMonsters.AlliedMinions.Any(a => a.Distance(pos) <= ConfigList.Flee.JumpCursorRange);
        }
        public static bool IsEnemyMinion(Vector3 pos)
        {
            return EntityManager.MinionsAndMonsters.EnemyMinions.Any(a => a.Distance(pos) <= ConfigList.Flee.JumpCursorRange);
        }
        public static bool IsMonster(Vector3 pos)
        {
            return EntityManager.MinionsAndMonsters.Monsters.Any(a => a.Distance(pos) <= ConfigList.Flee.JumpCursorRange);
        }
        public static bool IsDagger(Vector3 pos)
        {
            return ObjectManager.Get<Obj_AI_Base>().Any(a => (a.Name == "HiddenMinion") && a.IsValid && a.Distance(pos) <= ConfigList.Flee.JumpCursorRange);
        }
        public static bool IsPlant(Vector3 pos)
        {
            return ObjectManager.Get<Obj_AI_Base>().Any(a => a.Name.Contains("Plant") && a.IsValid && a.Distance(pos) <= ConfigList.Flee.JumpCursorRange);
        }

        public static void Execute()
        {
            if (!SpellManager.E.IsReady() || Damage.HasRBuff())
                return;

            var mouse = Game.CursorPos;
            if (IsPlant(mouse))
            {
                var target = ObjectManager.Get<Obj_AI_Base>().
                    Where(a => a.Name.Contains("Plant") && a.IsValid && !a.IsMe && a.IsInRange(a, SpellManager.E.Range) && a.Distance(mouse) <= ConfigList.Flee.JumpCursorRange);
                SpellManager.E.Cast(target.FirstOrDefault().Position);
                return;
            }
            if (ConfigList.Flee.JumpToDagger)
            {
                if (IsDagger(mouse))
                {
                    var target = ObjectManager.Get<Obj_AI_Base>().
                        Where(a => (a.Name == "HiddenMinion") && a.IsValid && !a.IsMe && a.IsInRange(a, SpellManager.E.Range) && a.Distance(mouse) <= ConfigList.Flee.JumpCursorRange);
                    SpellManager.E.Cast(target.FirstOrDefault().Position);
                    return;
                }
            }
            if (ConfigList.Flee.JumpToAlly)
            {
                if (IsAlly(mouse))
                {
                    var target = EntityManager.Heroes.Allies.
                        Where(a => !a.IsMe && a.IsInRange(a, SpellManager.E.Range) && a.Distance(mouse) <= ConfigList.Flee.JumpCursorRange);
                    SpellManager.E.Cast(target.FirstOrDefault().Position);
                    return;
                }
            }
            if (ConfigList.Flee.JumpToAllyMinion)
            {
                if (IsAllyMinion(mouse))
                {
                    var target = EntityManager.MinionsAndMonsters.AlliedMinions.
                        Where(a => a.IsInRange(a, SpellManager.E.Range) && a.Distance(mouse) <= ConfigList.Flee.JumpCursorRange);
                    SpellManager.E.Cast(target.FirstOrDefault().Position);
                    return;
                }
            }
            if (ConfigList.Flee.JumpToEnemyMinion)
            {
                if (IsEnemyMinion(mouse))
                {
                    var target = EntityManager.MinionsAndMonsters.EnemyMinions.
                        Where(a => a.IsInRange(a, SpellManager.E.Range) && a.Distance(mouse) <= ConfigList.Flee.JumpCursorRange);
                    SpellManager.E.Cast(target.FirstOrDefault().Position);
                    return;
                }
            }
            if (ConfigList.Flee.JumpToMonster && (EloBuddy.Player.Instance.HealthPercent >= (float)ConfigList.Flee.JumpToMonsterHP))
            {
                if (IsMonster(mouse)) 
                {
                    var target = EntityManager.MinionsAndMonsters.Monsters.
                        Where(a => a.IsInRange(a, SpellManager.E.Range) && a.Distance(mouse) <= ConfigList.Flee.JumpCursorRange);
                    SpellManager.E.Cast(target.FirstOrDefault().Position);
                    return;
                }
            }
        }
    }
}
