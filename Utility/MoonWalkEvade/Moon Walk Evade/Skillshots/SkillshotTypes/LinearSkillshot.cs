using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using Moon_Walk_Evade.Evading;
using Moon_Walk_Evade.Utils;
using SharpDX;
using Color = System.Drawing.Color;

namespace Moon_Walk_Evade.Skillshots.SkillshotTypes
{
    public class LinearSkillshot : EvadeSkillshot
    {
        public LinearSkillshot()
        {
            Caster = null;
            SpawnObject = null;
            SData = null;
            OwnSpellData = null;
            Team = GameObjectTeam.Unknown;
            IsValid = true;
            TimeDetected = Environment.TickCount;
        }

        public Vector3 FixedStartPos;
        public Vector3 FixedEndPos;
        private bool DoesCollide;
        private Vector2 LastCollisionPos;

        public MissileClient Missile => OwnSpellData.IsPerpendicular ? null : SpawnObject as MissileClient;

        public Vector3 RealCurrentPosition
        {
            get
            {
                bool debugMode = EvadeMenu.DebugMenu["debugMode"].Cast<KeyBind>().CurrentValue;
                if (Missile == null)
                {
                    if (debugMode)
                        return Debug.GlobalEndPos.ExtendVector3(Debug.GlobalStartPos,
                            Debug.GlobalStartPos.Distance(Debug.GlobalEndPos) + Player.Instance.BoundingRadius);

                    return FixedEndPos.ExtendVector3(FixedStartPos, FixedEndPos.Distance(FixedStartPos) + Player.Instance.BoundingRadius);
                }

                if (debugMode)//Simulate Position
                {
                    float speed = OwnSpellData.MissileSpeed;
                    float timeElapsed = Environment.TickCount - TimeDetected - OwnSpellData.Delay;
                    float traveledDist = speed * timeElapsed / 1000;
                    return Debug.GlobalStartPos.Extend(Debug.GlobalEndPos, traveledDist - Player.Instance.BoundingRadius).To3D();
                }

                if (DoesCollide && Missile.Position.Distance(Missile.StartPosition) >= LastCollisionPos.Distance(Missile.StartPosition))
                    return LastCollisionPos.To3D();

                return Missile.Position.ExtendVector3(Missile.StartPosition, -Player.Instance.BoundingRadius);
            }
        }

        public Vector3 NormalCurrentPosition
        {
            get
            {
                bool debugMode = EvadeMenu.DebugMenu["debugMode"].Cast<KeyBind>().CurrentValue;
                if (Missile == null)
                {
                    if (debugMode)
                        return Debug.GlobalStartPos;

                    return FixedStartPos;
                }

                if (debugMode)//Simulate Position
                {
                    float speed = OwnSpellData.MissileSpeed;
                    float timeElapsed = Environment.TickCount - TimeDetected - OwnSpellData.Delay;
                    float traveledDist = speed * timeElapsed / 1000;
                    return Debug.GlobalStartPos.Extend(Debug.GlobalEndPos, traveledDist).To3D();
                }

                if (DoesCollide && Missile.Position.Distance(Missile.StartPosition) >= LastCollisionPos.Distance(Missile.StartPosition))
                    return LastCollisionPos.To3D();

                return Missile.Position;
            }
        }

        public Vector3 NormalEndPosition
        {
            get
            {

                bool debugMode = EvadeMenu.DebugMenu["debugMode"].Cast<KeyBind>().CurrentValue;
                if (debugMode)
                    return Debug.GlobalEndPos;

                if (Missile == null)
                {
                    return FixedEndPos;
                }

                if (DoesCollide)
                    return LastCollisionPos.To3D();

                return Missile.StartPosition.ExtendVector3(Missile.EndPosition, OwnSpellData.Range);
            }
        }

        public Vector3 RealEndPosition
        {
            get
            {
                bool debugMode = EvadeMenu.DebugMenu["debugMode"].Cast<KeyBind>().CurrentValue;
                if (debugMode)
                    return Debug.GlobalStartPos.ExtendVector3(Debug.GlobalEndPos, 
                        Debug.GlobalStartPos.Distance(Debug.GlobalEndPos) + Player.Instance.BoundingRadius);

                if (Missile == null)
                {
                    return FixedStartPos.ExtendVector3(FixedEndPos, FixedStartPos.Distance(FixedEndPos) + Player.Instance.BoundingRadius);
                }

                if (DoesCollide)
                    return LastCollisionPos.To3D();

                return Missile.StartPosition.ExtendVector3(Missile.EndPosition, OwnSpellData.Range + Player.Instance.BoundingRadius);
            }
        }

        public override Vector3 GetCurrentPosition()
        {
            try
            {
                return RealCurrentPosition;
            }
            catch
            {
                return Vector3.Zero;
            }
        }

        public override EvadeSkillshot NewInstance(bool debug = false)
        {
            var newInstance = new LinearSkillshot { OwnSpellData = OwnSpellData };
            if (debug)
            {
                bool isProjectile = EvadeMenu.DebugMenu["isProjectile"].Cast<CheckBox>().CurrentValue;
                var newDebugInst = new LinearSkillshot
                {
                    OwnSpellData = OwnSpellData,
                    FixedStartPos = Debug.GlobalStartPos,
                    FixedEndPos = Debug.GlobalEndPos,
                    IsValid = true,
                    IsActive = true,
                    TimeDetected = Environment.TickCount,
                    SpawnObject = isProjectile ? new MissileClient() : null
                };
                return newDebugInst;
            }
            return newInstance;
        }

        public override void OnCreateObject(GameObject obj)
        {
            var missile = obj as MissileClient;

            bool debugMode = EvadeMenu.DebugMenu["debugMode"].Cast<KeyBind>().CurrentValue;
            if (SpawnObject == null && missile != null && !debugMode)
            {
                if (missile.SData.Name == OwnSpellData.ObjectCreationName && missile.SpellCaster.Index == Caster.Index)
                {
                    IsValid = false;
                }
            }
        }

        public override void OnCreateUnsafe(GameObject obj)
        {
            var missile = obj as MissileClient;
            if (missile != null) //missle
            {
                Vector2 collision = this.GetCollisionPoint();
                DoesCollide = !collision.IsZero;
                LastCollisionPos = collision;
            }
        }

        public override void OnSpellDetection(Obj_AI_Base sender)
        {
            if (!OwnSpellData.IsPerpendicular)
            {
                FixedStartPos = Caster.ServerPosition;
                FixedEndPos = FixedStartPos.ExtendVector3(CastArgs.End, OwnSpellData.Range);
            }
            else
            {
                OwnSpellData.Direction = (CastArgs.End - CastArgs.Start).To2D().Normalized();

                var direction = OwnSpellData.Direction;
                FixedStartPos = (CastArgs.End.To2D() - direction.Perpendicular() * OwnSpellData.SecondaryRadius).To3D();

                FixedEndPos = (CastArgs.End.To2D() + direction.Perpendicular() * OwnSpellData.SecondaryRadius).To3D();
            }
        }

        public override void OnTick()
        {
            var debug = EvadeMenu.DebugMenu["debugMode"].Cast<KeyBind>().CurrentValue;
            if (Missile == null)
            {
                if (Environment.TickCount > TimeDetected + OwnSpellData.Delay + 250)
                {
                    IsValid = false;
                    return;
                }
            }
            else if (Missile != null && !debug)
            {
                if (Environment.TickCount > TimeDetected + 6000)
                {
                    IsValid = false;
                    return;
                }
            }

            if (debug)
            {
                float speed = OwnSpellData.MissileSpeed;
                float timeElapsed = Environment.TickCount - TimeDetected - OwnSpellData.Delay;
                float traveledDist = speed * timeElapsed / 1000;

                if (traveledDist >= Debug.GlobalStartPos.Distance(Debug.GlobalEndPos) - 50)
                {
                    IsValid = false;
                    return;
                }
            }
        }

        public override void OnDraw()
        {
            if (!IsValid)
            {
                return;
            }

            int boundingRad = (int) Player.Instance.BoundingRadius;

            if (EvadeMenu.DrawMenu["drawType"].Cast<Slider>().CurrentValue == 0)
                MyUtils.Draw3DRect(NormalCurrentPosition, NormalEndPosition, (OwnSpellData.Radius - boundingRad) * 2, Color.White, 2, false);

            MyUtils.Draw3DRect(RealCurrentPosition, RealEndPosition, OwnSpellData.Radius * 2, Color.White, 3);
        }

        public override Geometry.Polygon ToPolygon()
        {
            float extrawidth = 0;
            return new Geometry.Polygon.Rectangle(RealCurrentPosition, RealEndPosition.ExtendVector3(RealCurrentPosition, -extrawidth), OwnSpellData.Radius + extrawidth);
        }

        public override int GetAvailableTime(Vector2 pos)
        {
            if (Missile == null)
            {
                return Math.Max(0, OwnSpellData.Delay - (Environment.TickCount - TimeDetected));
            }

            var proj = pos.ProjectOn(RealCurrentPosition.To2D(), RealEndPosition.To2D());
            if (!proj.IsOnSegment)
                return short.MaxValue;

            float skillDist = proj.SegmentPoint.Distance(RealCurrentPosition) - OwnSpellData.Radius;
            return Math.Max(0, (int)(skillDist / OwnSpellData.MissileSpeed * 1000));
        }

        public override bool IsFromFow()
        {
            return Missile != null && !Missile.SpellCaster.IsVisible;
        }

        public override bool IsSafe(Vector2? p = null)
        {
            return ToPolygon().IsOutside(p ?? Player.Instance.Position.To2D());
        }

        /*ping attened from caller*/
        public override Vector2 GetMissilePosition(int extraTime)
        {
            if (Missile == null)
                return FixedStartPos.To2D();//Missile not even created

            float dist = OwnSpellData.MissileSpeed / 1000f * extraTime;
            if (dist > RealCurrentPosition.Distance(RealEndPosition))
                dist = RealCurrentPosition.Distance(RealEndPosition);


            return RealCurrentPosition.Extend(RealEndPosition, dist);
        }

        public override bool IsSafePath(Vector2[] path, int timeOffset = 0, int speed = -1, int delay = 0)
        {
            timeOffset += Game.Ping;
            speed = speed == -1 ? (int)Player.Instance.MoveSpeed : speed;

            if (path.Length <= 1) //lastissue = playerpos
            {
                //timeNeeded = -11;
                if (!Player.Instance.IsRecalling())
                    return IsSafe();

                if (IsSafe())
                    return true;

                float timeLeft = (Player.Instance.GetBuff("recall").EndTime - Game.Time) * 1000;
                return GetAvailableTime(Player.Instance.Position.To2D()) - timeOffset > timeLeft;
            }

            //Skillshot with missile.
            if (!string.IsNullOrEmpty(OwnSpellData.ObjectCreationName) && !OwnSpellData.NoMissile)
            {
                float r = Missile == null ? TimeDetected + OwnSpellData.Delay - Environment.TickCount : 0;
                r -= timeOffset + delay;

                Vector3 pathDir = path[1].To3D() - path[0].To3D();
                Vector3 skillDir = RealEndPosition - RealCurrentPosition;
                
                float a = path[0].X;
                float w = path[0].Y;
                float m = path[0].To3D().Z;

                float v = RealCurrentPosition.X;
                float k = RealCurrentPosition.Y;
                float o = RealCurrentPosition.Z;

                float b = pathDir.X;
                float j = pathDir.Y;
                float n = pathDir.Z;

                float f = skillDir.X;
                float l = skillDir.Y;
                float p = skillDir.Z;

                float c = speed;
                float d = pathDir.Length();
                
                float g = OwnSpellData.MissileSpeed;
                float h = skillDir.Length();

                /*nullstelle d/dt - min distance*/
                double t = ((1000 * Math.Pow(d, 2) * g * h * l - 1000 * c * d * Math.Pow(h, 2) * j) * w + (1000 * b * c * d * 
                            Math.Pow(h, 2) - 1000 * Math.Pow(d, 2) * f * g * h) * v + (c * d * g * h * n * p - Math.Pow(c, 2) * 
                            Math.Pow(h, 2) * Math.Pow(n, 2) + c * d * g * h * j * l - Math.Pow(c, 2) * Math.Pow(h, 2) * Math.Pow(j, 2) -
                            Math.Pow(b, 2) * Math.Pow(c, 2) * Math.Pow(h, 2) + b * c * d * f * g * h) * r + (1000 * Math.Pow(d, 2) * g * 
                            h * m - 1000 * Math.Pow(d, 2) * g * h * o) * p + 1000 * c * d * Math.Pow(h, 2) * n * o - 1000 * c * d * 
                            Math.Pow(h, 2) * m * n - 1000 * Math.Pow(d, 2) * g * h * k * l + 1000 * c * d *
                            Math.Pow(h, 2) * j * k - 1000 * a * b * c * d * Math.Pow(h, 2) + 1000 * a * Math.Pow(d, 2) * f * g * h)/
                            (1000 * Math.Pow(d, 2) * Math.Pow(g, 2) * Math.Pow(p, 2) - 2000 * c * d * g * h * n * p +
                            1000 * Math.Pow(c, 2) * Math.Pow(h, 2) * Math.Pow(n, 2) + 1000 * Math.Pow(d, 2) * Math.Pow(g, 2) * Math.Pow(l, 2) -
                            2000 * c * d * g * h * j * l + 1000 * Math.Pow(c, 2) * Math.Pow(h, 2) * Math.Pow(j, 2) + 1000 * Math.Pow(b, 2) *
                            Math.Pow(c, 2) * Math.Pow(h, 2) - 2000 * b * c * d * f * g * h + 1000 * Math.Pow(d, 2) * Math.Pow(f, 2) * 
                            Math.Pow(g, 2));
                
                Vector3 myPosition = path[0].To3D() + (float)t * pathDir * c / pathDir.Length();
                Vector3 misPosition = RealCurrentPosition + (float)t * skillDir * g / skillDir.Length();

                bool valid = myPosition.Distance(Player.Instance) <= path[0].Distance(path[1]) &&
                    misPosition.Distance(RealCurrentPosition) <= RealCurrentPosition.Distance(RealEndPosition) && t >= 0;

                if (!valid && t >= 0)
                {
                    /*t out of skill range => set t to skillshot maxrange*/
                    if (misPosition.Distance(RealCurrentPosition) > RealCurrentPosition.Distance(RealEndPosition))
                    {
                        t = RealCurrentPosition.Distance(RealEndPosition)/OwnSpellData.MissileSpeed + r/1000;

                        myPosition = path[0].To3D() + (float) t*pathDir*c/pathDir.Length();
                        misPosition = RealEndPosition;

                        return myPosition.Distance(misPosition) > OwnSpellData.Radius;
                    }

                    /*t out of path range*/
                    if (myPosition.Distance(Player.Instance) > Player.Instance.Distance(path[1]))
                    {
                        t = path[0].Distance(path[1]) / speed;

                        myPosition = path[1].To3D();
                        misPosition = RealCurrentPosition + (float)t * skillDir * g / skillDir.Length();
                        bool pathEndSafe = myPosition.Distance(misPosition) > OwnSpellData.Radius;

                        return pathEndSafe && ToPolygon().IsOutside(path[1]);
                    }
                }

                return !valid || myPosition.Distance(misPosition) > OwnSpellData.Radius;
            }

            var timeToExplode = TimeDetected + OwnSpellData.Delay - Environment.TickCount;
            if (timeToExplode <= 0)
            {
                int timeLeft = OwnSpellData.ExtraExistingTime - timeToExplode - timeOffset - delay;
                bool intersects;
                var intersectionP = 
                    MyUtils.GetLinesIntersectionPoint(RealCurrentPosition.To2D(), RealEndPosition.To2D(), path[0], path[1], out intersects);
                float walkDistance = intersectionP.Distance(path[0])/speed*1000;
                return ToPolygon().IsOutside(Player.Instance.Position.To2D()) && (!intersects || timeLeft < walkDistance);
            }

            var myPositionWhenExplodes = path.PositionAfter(timeToExplode, speed, delay + timeOffset);
            return IsSafe(myPositionWhenExplodes);
        }
    }
}