using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReWarwick.Utils
{
    static class Extensions
    {
        public static bool IsUnderEnemyTurret(this Vector3 d)
        {
            return EntityManager.Turrets.Enemies.Any((Obj_AI_Turret turret) => turret.IsInRange(d, turret.GetAutoAttackRange(null)) && turret.IsAlive());
        }

        public static bool IsCollision(Vector2 start, Vector2 end, float width)
        {
            Geometry.Polygon.Rectangle r = new Geometry.Polygon.Rectangle(start, end, width);
            foreach (Obj_AI_Base aiBase in ObjectManager.Get<Obj_AI_Base>().Where(x => !x.IsAlly && x.IsValid && (x is AIHeroClient)))
            {
                if (r.IsInside(aiBase.Position) || r.Points.Any(x => x.Distance(aiBase.Position) <= aiBase.BoundingRadius))
                    return true;
            }
            return false;
        }
    }
}
