using EloBuddy;
using SharpDX;

namespace Moon_Walk_Evade.Skillshots
{
    public class SpellData
    {
        private int _radius;
        public string DisplayName { get; set; }
        public string SpellName { get; set; }
        public string ObjectCreationName { get; set; }
        public SpellSlot Slot { get; set; }
        public int Delay { get; set; }

        public int ExtraExistingTime { get; set; }
        public int Range { get; set; }

        public int Radius
        {
            get
            {
                int boundingRad = !IsCircular ? (int) Player.Instance.BoundingRadius : 0;
                return _radius + boundingRad + 20;
            }
            set { _radius = value; }
        }

        public float MissileSpeed { get; set; }
        public int DangerValue { get; set; }    
        public bool IsDangerous { get; set; }
        public string ChampionName { get; set; }
        public string ToggleParticleName { get; set; }
        public bool AddHitbox { get; set; }
        public int ExtraMissiles { get; set; }

        public bool EnabledByDefault { get; set; } = true;

        public bool MinionCollision { get; set; } = false;

        public bool IsGlobal => Range > 10000;

        public float ConeAngle { get; set; }


        public int RingRadius { get; set; } = 0;
        public bool IsVeigarE
        {
            get { return RingRadius > 0; }
        }

        public bool IsPerpendicular { get; set; }
        public int SecondaryRadius { get; set; }
        public Vector2 Direction { get; set; }
        public bool ForbidCrossing { get; set; }
        public bool NoMissile { get; set; }
        public bool IsCircular { get; set; }

        public SpellData()
        {
            AddHitbox = true;
        }
    }
}