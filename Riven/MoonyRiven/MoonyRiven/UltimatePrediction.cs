using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace MoonyRiven
{
    static class UltimatePrediction
    {
        static Vector2 PointOnCircle(float angleInDegrees)
        {
            float x = Player.Instance.Position.X + (float)(Spells.R2.Range * Math.Cos(angleInDegrees * Math.PI / 180));
            float y = Player.Instance.Position.Y + (float)(Spells.R2.Range * Math.Sin(angleInDegrees * Math.PI / 180));

            return new Vector2(x, y);
        }

        static Vector2 RotateAroundPoint(Vector2 center, Vector2 vec, float theta)
        {
            float px = vec.X, py = vec.Y;
            float ox = center.X, oy = center.Y;

            float x = (float)Math.Cos(theta) * (px - ox) - (float)Math.Sin(theta) * (py - oy) + ox;
            float y = (float)Math.Sin(theta) * (px - ox) + (float)Math.Cos(theta) * (py - oy) + oy;
            return new Vector2(x, y);
        }

        static Geometry.Polygon CreateUltimateCone(Vector2 endVec)
        {
            Geometry.Polygon cone = new Geometry.Polygon();
            var edgePoint1 = endVec + (endVec - Player.Instance.Position.To2D()).Perpendicular2().Normalized() * 200;
            //var edgePoint2 = endVec + (endVec - me.Position.To2D()).Perpendicular().Normalized()*200;
            cone.Points.Add(Player.Instance.Position.To2D());

            for (int currentAngle = 0; currentAngle <= 40; currentAngle += 1)
            {
                var newP = RotateAroundPoint(Player.Instance.Position.To2D(), edgePoint1, currentAngle * ((float)Math.PI / 180));
                cone.Points.Add(newP);
            }

            return cone;
        }

        public static int GetUltimateHits()
        {
            int maxHitCount = 0;

            for (int i = 0; i < 360; i += 20)
            {
                var endVec = PointOnCircle(i);
                Geometry.Polygon Cone = CreateUltimateCone(endVec);

                int currentHits = EntityManager.Heroes.Enemies.Where(x => x.IsValid && !x.IsDead && !x.IsZombie).Count(
                     x =>
                     {
                         var pred = Prediction.Position.PredictUnitPosition(x, (int)(Spells.R2.CastDelay + x.Distance(Player.Instance) /
                             Spells.R2.Speed * 1000));
                         bool inside = Cone.IsInside(pred);

                         return inside;
                     });

                if (currentHits > maxHitCount)
                {
                    maxHitCount = currentHits;
                }
            }
            return maxHitCount;
        }
        public static void CheckUltimateHits()
        {
            int maxHitCount = 0;
            Vector2 bestEndVec = Vector2.Zero;
            Geometry.Polygon BestCone = null;

            for (int i = 0; i < 360; i += 20)
            {
                var endVec = PointOnCircle(i);
                Geometry.Polygon Cone = CreateUltimateCone(endVec);

                int currentHits = EntityManager.Heroes.Enemies.Where(x => x.IsValid && !x.IsDead && !x.IsZombie).Count(
                     x =>
                     {
                         var pred = Prediction.Position.PredictUnitPosition(x, (int)(Spells.R2.CastDelay + x.Distance(Player.Instance) / 
                             Spells.R2.Speed * 1000));
                         bool inside = Cone.IsInside(pred);

                         return inside;
                     });

                if (currentHits > maxHitCount)
                {
                    maxHitCount = currentHits;
                    bestEndVec = endVec;

                    BestCone = Cone;
                }
            }

            if (BestCone != null && maxHitCount >= RivenMenu.Combo["minR2Hits"].Cast<Slider>().CurrentValue)
                Spells.ForceR2(Player.Instance.Position.Extend(bestEndVec, 100));
        }

        public static double GetRDamage(Obj_AI_Base target)
        {
            var health = target.Health;
            var missinghealth = (target.MaxHealth - health) / target.MaxHealth > 0.75 ? 0.75 : (target.MaxHealth - health) / target.MaxHealth;
            var pluspercent = missinghealth * (8 / 3f);
            var rawdmg = new double[] { 80, 120, 160 }[Spells.R2.Level - 1] + 0.6 * Player.Instance.FlatPhysicalDamageMod;
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, (float)(rawdmg * (1 + pluspercent)));
        }
    }
}
