using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace Aka_s_Vayne.Logic
{
    public static class Tumble
    {
        #region TumbleLogicss
        public static Vector3 TumbleOrderPos = Vector3.Zero;

        //private static AJSProvider Provider = new AJSProvider();

        public static void PreCastTumble(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(Variables._Player.AttackRange + 65f + 65f + 300f))
            {
                return;
            }

            var smartQPosition = QECombo();
            var smartQCheck = smartQPosition != Vector3.Zero;
            var QPosition = smartQCheck ? smartQPosition : Game.CursorPos;

            OnCastTumble(target, QPosition);
        }

        public static void OnCastTumble(Obj_AI_Base target, Vector3 position)
        {
            var mode = Manager.MenuManager.UseQMode;
            var afterTumblePosition = Variables._Player.ServerPosition.Extend(position, 300f);
            var distanceToTarget = afterTumblePosition.Distance(target.ServerPosition, true);
            if ((distanceToTarget < Math.Pow(Variables._Player.AttackRange + 65, 2) && distanceToTarget > 110 * 110)
                || Manager.MenuManager.UseQSpam)
            {
                switch (mode)
                {
                    case 0:
                        //To mouse
                        DefaultQCast(position, target);
                        break;
                 /*   case 1:
                        //smart logic!
                        var smartQPosition = QECombo();
                        var smartQCheck = smartQPosition != Vector3.Zero;
                        var QPosition = smartQCheck ? smartQPosition : Game.CursorPos;
                        var QPosition2 = Provider.AkaQPosition() != Vector3.Zero ? Provider.AkaQPosition() : QPosition;

                        if (!Variables.UnderEnemyTower((Vector2)QPosition2) || (Variables.UnderEnemyTower((Vector2)QPosition2) && Variables.UnderEnemyTower((Vector2)Variables._Player.Position)))
                        {
                            CastQ(QPosition2);
                        }
                        break;
                        */
                    case 1:
                        var QPosition3rd = GetAJSPosition();

                        if (!Variables.UnderEnemyTower((Vector2)QPosition3rd) || (Variables.UnderEnemyTower((Vector2)QPosition3rd) && Variables.UnderEnemyTower((Vector2)Variables._Player.Position)))
                        {
                            CastQ(QPosition3rd);
                        }
                        break;
                    case 3:
                        //Away from melee enemies
                        if (Variables.MeleeEnemiesTowardsMe.Any() &&
                            !Variables.MeleeEnemiesTowardsMe.All(m => m.HealthPercent <= 15))
                        {
                            var Closest =
                                Variables.MeleeEnemiesTowardsMe.OrderBy(m => m.Distance(Variables._Player)).First();
                            var whereToQ = (Vector3)Closest.ServerPosition.Extend(
                                Variables._Player.ServerPosition, Closest.Distance(Variables._Player) + 300f);

                            if (whereToQ.IsSafeToQ())
                            {
                                CastQ(whereToQ);
                            }
                        }
                        else
                        {
                            DefaultQCast(position, target);
                        }
                        break;
                    case 2:
                        //Prada
                        var Target = TargetSelector.GetTarget((int)Variables._Player.GetAutoAttackRange(), DamageType.Physical);
                        if (Target == null) return;
                        var tumblePosition = Target.GetTumblePos();
                        CastQ(tumblePosition);
                        break;
                    case 4:
                        //Sebby
                        CastDash();
                        break;
                }
            }
        }

        private static void CastQ(Vector3 Position)
        {
            var endPosition = Position;

            if (Manager.MenuManager.Burstmode)
            {
                var qBurstModePosition = GetQBurstModePosition();
                if (qBurstModePosition != null)
                {
                    endPosition = (Vector3)qBurstModePosition;
                }
            }

            if (endPosition.IsSafeToQ(true))
            {
                Player.CastSpell(SpellSlot.Q, endPosition);
            }
        }

        public static bool IsSafeToQ(this Vector3 position, bool noQIntoEnemiesCheck = false)
        {

            if (Manager.MenuManager.UseQTower && (Variables.UnderEnemyTower((Vector2)position) && !Variables.UnderEnemyTower((Vector2)Variables._Player.Position)))
            {
                return false;
            }

            var allies = position.CountAllyChampionsInRange(Variables._Player.AttackRange);
            var enemies = position.CountAllyChampionsInRange(Variables._Player.AttackRange);
            var lhEnemies = position.GetLhEnemiesNear(Variables._Player.AttackRange, 15).Count();

            if (enemies <= 1) ////It's a 1v1, safe to assume I can Q
            {
                return true;
            }

            if (position.UnderAllyTurret_Ex())
            {
                var nearestAllyTurret = ObjectManager.Get<Obj_AI_Turret>().Where(a => a.IsAlly).OrderBy(d => d.Distance(position, true)).FirstOrDefault();

                if (nearestAllyTurret != null)
                {
                    ////We're adding more allies, since the turret adds to the firepower of the team.
                    allies += 2;
                }
            }

            ////Adding 1 for my Player
            var normalCheck = (allies + 1 > enemies - lhEnemies);
            var QEnemiesCheck = true;
            var TrapCheck = true;
            var EvadeCheck = true;
            var WallCheck = true;

            if (Manager.MenuManager.UseQEvade && !Manager.MenuManager.UseQSpam)
            {
                EvadeCheck = AkaCore.Manager.EvadeManager.EvadeSkillshot.IsSafePoint(position.To2D()).IsSafe;
            }

            if (Manager.MenuManager.UseQTraps && !Manager.MenuManager.UseQSpam)
            {
                TrapCheck = !(Manager.Traps.EnemyTraps.Any(t => position.Distance(t.Position) < 125));
            }

            if (Manager.MenuManager.UseQWall && !Manager.MenuManager.UseQSpam && !Manager.MenuManager.Burstmode)
            {
                WallCheck = !(NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Wall) || NavMesh.GetCollisionFlags(position).HasFlag(CollisionFlags.Building));
            }

            if (Manager.MenuManager.UseQEnemies && noQIntoEnemiesCheck)
            {
                if (!Manager.MenuManager.UseQEnemies)
                {
                    var Vector2Position = position.To2D();
                    var enemyPoints = Manager.MenuManager.UseQDynamic
                        ? GetEnemyPoints()
                        : GetEnemyPoints(false);
                    if (enemyPoints.Contains(Vector2Position) &&
                        !Manager.MenuManager.UseQSpam)
                    {
                        QEnemiesCheck = false;
                    }

                    var closeEnemies =
                    EntityManager.Heroes.Enemies.FindAll(en => en.IsValidTarget(1500f) && !(en.Distance(Variables._Player.ServerPosition) < en.AttackRange + 65f))
                    .OrderBy(en => en.Distance(position));

                    if (
                        !closeEnemies.All(
                            enemy =>
                                position.CountEnemyChampionsInRange(
                                    Manager.MenuManager.UseQDynamic
                                        ? enemy.AttackRange
                                        : 405f) <= 1))
                    {
                        QEnemiesCheck = false;
                    }
                }
                else
                {
                    var closeEnemies =
                    EntityManager.Heroes.Enemies.FindAll(en => en.IsValidTarget(1500f)).OrderBy(en => en.Distance(position));
                    if (closeEnemies.Any())
                    {
                        QEnemiesCheck =
                            !closeEnemies.All(
                                enemy =>
                                    position.CountEnemyChampionsInRange(
                                        Manager.MenuManager.UseQDynamic
                                            ? enemy.AttackRange
                                            : 405f) <= 1);
                    }
                }

            }

            return normalCheck && QEnemiesCheck && EvadeCheck && WallCheck && TrapCheck;
        }

        private static Vector3 QECombo()
        {
            if (!Manager.MenuManager.UseQE &&
    !Manager.SpellManager.E.IsReady())
            {
                return Vector3.Zero;
            }

            const int currentStep = 30;
            var direction = Variables._Player.Direction.To2D().Perpendicular();
            for (var i = 0f; i < 360f; i += currentStep)
            {
                var angleRad = Geometry.DegreeToRadian(i);
                var rotatedPosition = Variables._Player.Position.To2D() + (300f * direction.Rotated(angleRad));
                if (Condemn.GetCondemnTarget(rotatedPosition.To3D()).IsValidTarget() && rotatedPosition.To3D().IsSafeToQ())
                {
                    return rotatedPosition.To3D();
                }
            }

            return Vector3.Zero;
        }

        #endregion 

        #region smart

        private static void DefaultQCast(Vector3 position, Obj_AI_Base Target)
        {
            var afterTumblePosition = GetAfterTumblePosition(Game.CursorPos);
            var CursorPos = Game.CursorPos;
            var EnemyPoints = GetEnemyPoints();
            if (afterTumblePosition.IsSafeToQ(true) || (!EnemyPoints.Contains(Game.CursorPos.To2D())) || (Variables.EnemiesClose.Count() == 1))
            {
                if (afterTumblePosition.Distance(Target.ServerPosition) <= Variables._Player.GetAutoAttackRange(Target))
                {
                    CastQ(position);
                }
            }
        }

        #region smart variables

        private static List<Vector2> GetEnemyPoints(bool dynamic = true)
        {
            var staticRange = 360f;
            var polygonsList = Variables.EnemiesClose.Select(enemy => new AkaCore.AkaLib.AGeometry.Circle(enemy.ServerPosition.To2D(), (dynamic ? (enemy.IsMelee ? enemy.AttackRange * 1.5f : enemy.AttackRange) : staticRange) + enemy.BoundingRadius + 20).ToPolygon()).ToList();
            var pathList = AkaCore.AkaLib.AGeometry.ClipPolygons(polygonsList);
            var pointList = pathList.SelectMany(path => path, (path, point) => new Vector2(point.X, point.Y)).Where(currentPoint => !currentPoint.IsWall()).ToList();
            return pointList;
        }

        private static Vector3 GetAfterTumblePosition(Vector3 endPosition)
        {
            return (Vector3)Variables._Player.ServerPosition.Extend(endPosition, 300f);
        }

        private static Vector3? GetQBurstModePosition()
        {
            var positions =
                GetWallQPositions(70).ToList().OrderBy(pos => pos.Distance(Variables._Player.ServerPosition, true));

            foreach (var position in positions)
            {
                var collFlags = NavMesh.GetCollisionFlags(position);
                if (collFlags.HasFlag(CollisionFlags.Wall) || collFlags.HasFlag(CollisionFlags.Building) && position.IsSafeToQ(true))
                {
                    return position;
                }
            }

            return null;
        }


        private static Vector3[] GetWallQPositions(float Range)
        {
            Vector3[] vList =
            {
                (Variables._Player.ServerPosition.To2D() + Range * Variables._Player.Direction.To2D()).To3D(),
                (Variables._Player.ServerPosition.To2D() - Range * Variables._Player.Direction.To2D()).To3D()

            };

            return vList;
        }
        #endregion
        #endregion smart

        #region old

        private static Vector3 GetAggressiveTumblePos(this Obj_AI_Base target)
        {
            var cursorPos = Game.CursorPos;

            if (cursorPos.IsSafeToQ(true)) return cursorPos;
            //if the target is not a melee and he's alone he's not really a danger to us, proceed to 1v1 him :^ )
            if (!target.IsMelee && Variables._Player.CountEnemyChampionsInRange(800) == 1) return cursorPos;

            var aRC = new Geometry.Polygon.Circle(Variables._Player.ServerPosition.To2D(), 300).ToClipperPath();
            var targetPosition = target.ServerPosition;


            foreach (var p in aRC)
            {
                var v3 = new Vector2(p.X, p.Y).To3D();
                var dist = v3.Distance(targetPosition);
                if (dist > 325 && dist < 450)
                {
                    return v3;
                }
            }
            return Vector3.Zero;
        }

        private static Vector3 GetTumblePos(this Obj_AI_Base target)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                return GetAggressiveTumblePos(target);

            var cursorPos = Game.CursorPos;

            if (cursorPos.IsSafeToQ(true)) return cursorPos;
            //if the target is not a melee and he's alone he's not really a danger to us, proceed to 1v1 him :^ )
            if (!target.IsMelee && Variables._Player.CountEnemyChampionsInRange(800) == 1) return cursorPos;

            var aRC = new Geometry.Polygon.Circle(Variables._Player.ServerPosition.To2D(), 300).ToClipperPath();
            var targetPosition = target.ServerPosition;
            var pList = (from p in aRC
                         select new Vector2(p.X, p.Y).To3D()
                    into v3
                         let dist = v3.Distance(targetPosition)
                         where v3.IsSafeToQ(true) && dist < 500
                         select v3).ToList();

            if (Variables.UnderEnemyTower((Vector2)Variables._Player.ServerPosition) || Variables._Player.CountEnemyChampionsInRange(800) == 1 ||
                cursorPos.CountEnemyChampionsInRange(450) <= 1)
            {
                return pList.Count > 1 ? pList.OrderBy(el => el.Distance(cursorPos)).FirstOrDefault() : Vector3.Zero;
            }
            return pList.Count > 1
                ? pList.OrderByDescending(el => el.Distance(cursorPos)).FirstOrDefault()
                : Vector3.Zero;
        }
        #endregion old

        #region new

        private static Vector3 CastDash()
        {
            Vector3 bestpoint = Vector3.Zero;

            var orbT = TargetSelector.GetTarget((int)Variables._Player.GetAutoAttackRange(),
DamageType.Physical);
            if (orbT != null)
            {
                Vector2 start = Variables._Player.Position.To2D();
                Vector2 end = orbT.Position.To2D();
                var dir = (end - start).Normalized();
                var pDir = dir.Perpendicular();

                var rightEndPos = end + pDir * Variables._Player.Distance(orbT);
                var leftEndPos = end - pDir * Variables._Player.Distance(orbT);

                var rEndPos = new Vector3(rightEndPos.X, rightEndPos.Y, Variables._Player.Position.Z);
                var lEndPos = new Vector3(leftEndPos.X, leftEndPos.Y, Variables._Player.Position.Z);

                if (Game.CursorPos.Distance(rEndPos) < Game.CursorPos.Distance(lEndPos))
                {
                    bestpoint = (Vector3)Variables._Player.Position.Extend(rEndPos, Manager.SpellManager.Q.Range);
                    if (bestpoint.IsSafeToQ(true))
                        CastQ(bestpoint);
                }
                else
                {
                    bestpoint = (Vector3)Variables._Player.Position.Extend(lEndPos, Manager.SpellManager.Q.Range);
                    if (bestpoint.IsSafeToQ(true))
                        CastQ(bestpoint);
                }
            }

            if (!bestpoint.IsZero && bestpoint.CountEnemyChampionsInRange(Variables._Player.BoundingRadius + Variables._Player.AttackRange + 100) == 0)
                return Vector3.Zero;

            return bestpoint;
        }

        private static List<Vector3> CirclePoints(float CircleLineSegmentN, float radius, Vector3 position)
        {
            List<Vector3> points = new List<Vector3>();
            for (var i = 1; i <= CircleLineSegmentN; i++)
            {
                var angle = i * 2 * Math.PI / CircleLineSegmentN;
                var point = new Vector3(position.X + radius * (float)Math.Cos(angle), position.Y + radius * (float)Math.Sin(angle), position.Z);
                points.Add(point);
            }
            return points;
        }

        #endregion new

        #region AJS
        public static List<Vector3> GetRotatedQPositions()
        {
            const int currentStep = 30;
            var direction = Variables._Player.Direction.To2D().Perpendicular();

            var list = new List<Vector3>();
            for (var i = -105; i <= 105; i += currentStep)
            {
                var angleRad = Geometry.DegreeToRadian(i);
                var rotatedPosition = Variables._Player.Position.To2D() + (300f * direction.Rotated(angleRad));
                list.Add(rotatedPosition.To3D());
            }
            return list;
        }

        public static Vector3 GetAJSPosition()
        {
            var positions = GetRotatedQPositions();
            var enemyPositions = GetEnemyPoints();
            var safePositions = positions.Where(pos => !enemyPositions.Contains(pos.To2D())).ToList();
            var BestPosition = Variables._Player.ServerPosition.Extend(Game.CursorPos, 300f).To3D();         
            var AverageDistanceWeight = .65f;
            var ClosestDistanceWeight = .35f;
            var EndPosition = Vector3.Zero;
            var bestWeightedAvg = 0f;

            var enemiesNear = EntityManager.Heroes.Enemies.Where(m => m.IsValidTarget(Variables._Player.GetAutoAttackRange(m) + 300f + 65f)).ToList();

            var highHealthEnemiesNear = EntityManager.Heroes.Enemies.Where(m => !m.IsMelee && m.IsValidTarget(1300f) && m.HealthPercent > 7).ToList();

            if (Variables.MeleeEnemiesTowardsMe.Any() && Variables.MeleeEnemiesTowardsMe.Any(m => m.Position.Distance(Variables._Player) <= 300) &&
                !Variables.MeleeEnemiesTowardsMe.All(m => m.HealthPercent <= 35))
            {
                var Closest =
    Variables.MeleeEnemiesTowardsMe.OrderBy(m => m.Distance(Variables._Player)).First();
                var position = (Vector3)Closest.ServerPosition.Extend(
                    Variables._Player.ServerPosition, Closest.Distance(Variables._Player) + 300f);

               EndPosition = position.IsSafeToQ(true) ? position : Vector3.Zero;
            }

            if (enemiesNear.All(x => x.Health > Variables._Player.Health - 300) && enemiesNear.Any(x => x.Position.Distance(Variables._Player.Position) >= 450))
            {
                var closest = enemiesNear.OrderBy(x => x.Distance(Variables._Player)).First();
                var position = (Vector3)closest.ServerPosition.Extend(Variables._Player.ServerPosition, closest.Distance(Variables._Player) + 300f);

                EndPosition = position.IsSafeToQ(true) ? position : Vector3.Zero;
            }



            if (Variables._Player.CountEnemyChampionsInRange(1300f) == 1 && Variables.MeleeEnemiesTowardsMe.Count() == 0 && enemiesNear.All(x => x.HealthPercent >= 35))
            {
                var position = Variables._Player.ServerPosition.Extend(Game.CursorPos, 300f).To3D();
                EndPosition = position.IsSafeToQ(true) ? position : Vector3.Zero;
            }

            if (enemiesNear.Any(t => t.Health + 15 < Variables._Player.GetAutoAttackDamage(t) * 2 + DamageLibrary.GetSpellDamage(Variables._Player, t, SpellSlot.Q, DamageLibrary.SpellStages.Default) && t.Distance(Variables._Player) < Variables._Player.GetAutoAttackRange(t) + 80f))
            {
                var QPosition = Variables._Player.ServerPosition.Extend(enemiesNear.OrderBy(t => t.Health).First().ServerPosition, 300f).To3D();

                if (!QPosition.IsUnderTurret())
                {
                    EndPosition = QPosition;
                }
            }

            if (enemiesNear.Count() <= 2)
            {
                if (
                    enemiesNear.Any(
                        t =>
                            t.Health + 15 <
                            Variables._Player.GetAutoAttackDamage(t) + DamageLibrary.GetSpellDamage(Variables._Player, t, SpellSlot.Q, DamageLibrary.SpellStages.Default)
                            && t.Distance(Variables._Player) < Variables._Player.GetAutoAttackRange(t) + 80f))
                {
                    var QPosition =
                        Variables._Player.ServerPosition.Extend(
                            highHealthEnemiesNear.OrderBy(t => t.Health).First().ServerPosition, 300f).To3D();

                    if (!QPosition.IsUnderTurret())
                    {
                        EndPosition = QPosition;
                    }
                }
            }

            if (enemiesNear.Count() >= 2 && enemiesNear.All(x => x.HealthPercent >= 35))
            {
                foreach (var position in safePositions)
                {
                    //Start le calculations    
                    var enemy = GetClosestEnemy(position);
                    if (enemy == null)
                    {
                        continue;
                    }

                    if (Variables._Player.Distance(enemy) < enemy.AttackRange - 85 && !enemy.IsMelee)
                    {
                        return Variables._Player.ServerPosition.Extend(Game.CursorPos, 300f).To3D();
                    }

                    var avgDist = GetAvgDistance(position);
                    if (avgDist > -1)
                    {
                        var closestDist = Variables._Player.ServerPosition.Distance(enemy.ServerPosition);
                        var weightedAvg = closestDist * ClosestDistanceWeight + avgDist * AverageDistanceWeight;
                        if (weightedAvg > bestWeightedAvg && position.IsSafeToQ())
                        {
                            bestWeightedAvg = weightedAvg;
                            BestPosition = position;
                        }
                    }
                }


                EndPosition = (BestPosition.IsSafeToQ(true) && IsSafeEx(BestPosition)) ? BestPosition : Vector3.Zero;
            }

           if (EndPosition == Vector3.Zero && Game.CursorPos.IsSafeToQ(true))
            {
                EndPosition = Game.CursorPos;
            }

            return EndPosition;
        }

        public static Obj_AI_Base GetClosestEnemy(Vector3 from)
        {
            if (Orbwalker.GetTarget() is AIHeroClient)
            {
                var owAI = Orbwalker.GetTarget() as AIHeroClient;
                if (owAI.IsValidTarget(Variables._Player.GetAutoAttackRange(null) + 120f, true, from))
                {
                    return owAI;
                }
            }
           
            return
                EntityManager.Heroes.Enemies
                    .FirstOrDefault(en => en.IsValidTarget(Variables._Player.GetAutoAttackRange(null), true, from));
        }

        public static bool IsSafeEx(Vector3 position)
        {
            var closeEnemies =
                    EntityManager.Heroes.Enemies.FindAll(en => en.IsValidTarget(1500f) && !(en.Distance(Variables._Player.ServerPosition) < en.AttackRange + 65f))
                    .OrderBy(en => en.Distance(position));

            return closeEnemies.All(
                                enemy =>
                                    position.CountEnemyChampionsInRange(
                                        Manager.MenuManager.UseQDynamic
                                            ? enemy.AttackRange
                                            : 405f) <= 1);
        }

        public static float GetAvgDistance(Vector3 from)
        {
            var numberOfEnemies = from.CountEnemyChampionsInRange(1000f);
            if (numberOfEnemies != 0)
            {
                var enemies = EntityManager.Heroes.Enemies.Where(en => en.IsValidTarget(1000f, true, from)
                                                    &&
                                                    en.Health >
                                                    Variables._Player.GetAutoAttackDamage(en) * 3 +
                                                    Manager.DamageManager.Wdmg(en) +
                                                    Variables._Player.GetSpellDamage(en, SpellSlot.Q)).ToList();
                var enemiesEx = EntityManager.Heroes.Enemies.Where(en => en.IsValidTarget(1000f, true, from)).ToList();
                var LHEnemies = enemiesEx.Count() - enemies.Count();
                var totalDistance = 0f;

                totalDistance = (LHEnemies > 1 && enemiesEx.Count() > 2) ?
                    enemiesEx.Sum(en => en.Distance(Variables._Player.ServerPosition)) :
                    enemies.Sum(en => en.Distance(Variables._Player.ServerPosition));

                return totalDistance / numberOfEnemies;
            }
            return -1;
        }
        #endregion
    }
}