using System;
using System.Text.RegularExpressions;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using Moon_Walk_Evade.Utils;
using SharpDX;
using Color = System.Drawing.Color;

namespace Moon_Walk_Evade.Skillshots.SkillshotTypes
{
    class VeigarE : EvadeSkillshot
    {
        public VeigarE()
        {
            Caster = null;
            SpawnObject = null;
            SData = null;
            OwnSpellData = null;
            Team = GameObjectTeam.Unknown;
            IsValid = true;
            TimeDetected = Environment.TickCount;
        }

        public Vector3 StartPosition { get; set; }

        public Vector3 EndPosition { get; set; }

        public MissileClient Missile => SpawnObject as MissileClient;

        private bool _missileDeleted;


        public override Vector3 GetCurrentPosition()
        {
            return EndPosition;
        }

        /// <summary>
        /// Creates an existing Class Object unlike the DataBase contains
        /// </summary>
        /// <returns></returns>
        public override EvadeSkillshot NewInstance(bool debug = false)
        {
            var newInstance = new VeigarE { OwnSpellData = OwnSpellData };
            if (debug)
            {
                bool isProjectile = EvadeMenu.DebugMenu["isProjectile"].Cast<CheckBox>().CurrentValue;
                var newDebugInst = new VeigarE
                {
                    OwnSpellData = OwnSpellData,
                    StartPosition = Debug.GlobalStartPos,
                    EndPosition = Debug.GlobalEndPos,
                    IsValid = true,
                    IsActive = true,
                    TimeDetected = Environment.TickCount,
                    SpawnObject = isProjectile ? new MissileClient() : null
                };
                return newDebugInst;
            }
            return newInstance;
        }

        public override void OnCreateUnsafe(GameObject obj)
        {
            EndPosition = Missile?.EndPosition ?? CastArgs.End;
        }

        public override void OnCreateObject(GameObject obj)
        {
            var missile = obj as MissileClient;

            if (SpawnObject == null && missile != null)
            {
                if (missile.SData.Name == OwnSpellData.ObjectCreationName && missile.SpellCaster.Index == Caster.Index)
                {
                    // Force skillshot to be removed
                    IsValid = false;
                }
            }
        }

        public override bool OnDeleteMissile(GameObject obj)
        {
            if (Missile != null && obj.Index == Missile.Index && !string.IsNullOrEmpty(OwnSpellData.ToggleParticleName))
            {
                _missileDeleted = true;
                return false;
            }

            return true;
        }

        public override void OnDeleteObject(GameObject obj)
        {
            if (Missile != null && _missileDeleted && !string.IsNullOrEmpty(OwnSpellData.ToggleParticleName))
            {
                var r = new Regex(OwnSpellData.ToggleParticleName);
                if (r.Match(obj.Name).Success && obj.Distance(EndPosition, true) <= 100 * 100)
                {
                    IsValid = false;
                }
            }
        }

        /// <summary>
        /// check if still valid
        /// </summary>
        public override void OnTick()
        {
            if (Missile == null)
            {
                if (Environment.TickCount > TimeDetected + OwnSpellData.Delay + 5000)
                    IsValid = false;
            }
            else if (Missile != null)
            {
                if (Environment.TickCount > TimeDetected + 6000)
                    IsValid = false;
            }
        }

        public override void OnDraw()
        {
            if (!IsValid)
            {
                return;
            }

            ToInnerPolygon().Draw(Color.White, 3);
            ToOuterPolygon().Draw(Color.White, 3);
        }

        public override Geometry.Polygon ToInnerPolygon()
        {
            return new Geometry.Polygon.Circle(EndPosition, OwnSpellData.RingRadius);
        }

        public override Geometry.Polygon ToOuterPolygon()
        {
            return new Geometry.Polygon.Circle(EndPosition, OwnSpellData.RingRadius + OwnSpellData.Radius);
        }

        Vector2 PointOnCircle(float radius, float angleInDegrees, Vector2 origin)
        {
            float x = origin.X + (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180));
            float y = origin.Y + (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180));

            return new Vector2(x, y);
        }

        public override Geometry.Polygon ToPolygon()
        {
            throw new Exception("Unnecessary ToPolygon call in VeigarE.cs");
        }

        public override int GetAvailableTime(Vector2 pos)
        {
            return Math.Max(0, OwnSpellData.Delay - (Environment.TickCount - TimeDetected));
        }

        public override bool IsFromFow()
        {
            return Missile != null && !Missile.SpellCaster.IsVisible;
        }

        public override bool IsSafe(Vector2? p = null)
        {
            return ToInnerPolygon().IsInside(p ?? Player.Instance.Position.To2D()) ||
                   ToOuterPolygon().IsOutside(p ?? Player.Instance.Position.To2D());
        }

        public override Vector2 GetMissilePosition(int extraTime)
        {
            return EndPosition.To2D();
        }

        public override bool IsSafePath(Vector2[] path, int timeOffset = 0, int speed = -1, int delay = 0)
        {
            if (path.Length <= 1) //lastissue = playerpos
            {
                if (!Player.Instance.IsRecalling())
                    return IsSafe();

                if (IsSafe())
                    return true;

                float timeLeft = (Player.Instance.GetBuff("recall").EndTime - Game.Time) * 1000;
                return GetAvailableTime(Player.Instance.Position.To2D()) > timeLeft;
            }

            timeOffset += Game.Ping;

            speed = speed == -1 ? (int)ObjectManager.Player.MoveSpeed : speed;

            var timeToExplode = TimeDetected + OwnSpellData.Delay - Environment.TickCount;
            var outter = ToOuterPolygon();
            var inner = ToInnerPolygon();

            if (timeToExplode <= 0)
            {
                return (inner.IsInside(path[0]) && inner.IsInside(path[1])) ||
                       (outter.IsOutside(path[0]) && outter.IsOutside(path[1]));
            }

            bool noIntersections = MyUtils.GetLineCircleIntersectionPoints(EndPosition.To2D(), OwnSpellData.RingRadius, path[0],
                       path[1]).Length == 0;
            if (!noIntersections && ToOuterPolygon().IsOutside(path[0]))
                return false;

            var myPositionWhenExplodesWithOffset = path.PositionAfter(timeToExplode, speed, delay + timeOffset);
            return IsSafe(myPositionWhenExplodesWithOffset);
        }
    }
}
