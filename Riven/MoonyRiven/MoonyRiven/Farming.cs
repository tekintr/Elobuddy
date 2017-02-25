using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace MoonyRiven
{
    public static class Farming
    {
        private static AIHeroClient me => Player.Instance;

        public static int GetMinionsAfterTime(Vector2 p, float radius, float time)
        {
            int i = 0;
            foreach (var min in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, p.To3D(), radius))
            {
                if (Prediction.Health.GetPrediction(min, (int)time) > 0)
                    i++;
            }
            return i;
        }

        public static bool IsValidAfterTime(this Obj_AI_Base min, float time)
            => Prediction.Health.GetPrediction(min, (int)time) > 0;

        public static int CurrentMinions(float spellRadius) =>
            EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, me.Position, spellRadius).Count();

        public static Vector2 PointOnCircle(float radius, float angleInDegrees, Vector2 origin)
        {
            float x = origin.X + (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180));
            float y = origin.Y + (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180));

            return new Vector2(x, y);
        }

        public static Vector2? GetBestFarmLocation(float spellRadius, float? dashRadius = null)
        {
            dashRadius = dashRadius ?? Spells.E.Range;
            Vector2 bestLocation = Vector2.Zero;
            int maxMins = 0;
            for (int angle = 0; angle < 360; angle += 10)
            {
                var p = PointOnCircle(dashRadius.Value, angle, me.Position.To2D());
                var mins = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, p.To3D(), spellRadius).
                    Where(x => x.IsValidAfterTime(500));
                if (mins.Count() > maxMins)
                {
                    maxMins = mins.Count();
                    bestLocation = p;
                }

                if (dashRadius == 0)
                    break;
            }

            bool worth = (maxMins > CurrentMinions(spellRadius) && maxMins > 1) || (dashRadius == 0 && maxMins > 0);
            if (worth)
                return bestLocation;

            return null;
        }

        public static Vector2 RotateAroundPoint(Vector2 around, Vector2 b, float theta)
        {
            float px = b.X, py = b.Y;
            float ox = around.X, oy = around.Y;

            float x = (float)Math.Cos(theta) * (px - ox) - (float)Math.Sin(theta) * (py - oy) + ox;
            float y = (float)Math.Sin(theta) * (px - ox) + (float)Math.Cos(theta) * (py - oy) + oy;
            return new Vector2(x, y);
        }

        public static Geometry.Polygon GetQCone()
        {
            Vector2 endVec = me.Position.To2D() + me.Direction().Normalized()*Spells.Q.Range;
            Geometry.Polygon cone = new Geometry.Polygon();
            //var edgePoint1 = endVec + (endVec - me.Position.To2D()).Perpendicular2().Normalized() * 200;
            cone.Points.Add(me.Position.To2D());

            for (int currentAngle =-Spells.Q.ConeAngleDegrees / 2; currentAngle <= Spells.Q.ConeAngleDegrees/2; currentAngle += 1)
            {
                var newP = RotateAroundPoint(me.Position.To2D(), endVec, currentAngle * ((float)Math.PI / 180));
                cone.Points.Add(newP);
            }

            return cone;
        }

        public static int GetMinionsInQ()
        {
            var poly = GetQCone();
            var mins = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, me.Position, Spells.Q.Range).
                    Where(x => x.IsValidAfterTime(Spells.Q.CastDelay));
            return mins.Count(m => poly.IsInside(m));
        }

        public static bool CanKillMinionWithQ()
        {
            var poly = GetQCone();
            var mins = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, me.Position, Spells.Q.Range).
                    Where(x => x.IsValidAfterTime(250) && poly.IsInside(x));
            return mins.Any(x => Spells.Q.GetSpellDamage(x) >= x.Health && Prediction.Health.GetPrediction(x, 500) > 0);
        }

        public static Obj_AI_Base GetQKillableMinion()
        {
            var poly = GetQCone();
            var mins = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, me.Position, Spells.Q.Range).
                    Where(x => x.IsValidAfterTime(250) && poly.IsInside(x));
            return mins.First(x => Spells.Q.GetSpellDamage(x) >= x.Health);
        }
    }
}
