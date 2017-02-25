using EloBuddy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReKatarina.ReCore.Managers;
using SharpDX;
using EloBuddy.SDK;

namespace ReKatarina.ReCore.Managers
{
    static class EntityManager
    {
        public static int GetIgniteDamage()
        {
            return 50 + (20 * Player.Instance.Level);
        }

        public static string[] MonsterSmiteables =
        {
            "TT_Spiderboss", "TTNGolem", "TTNWolf", "TTNWraith",
            "SRU_Blue", "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Red", "SRU_Krug", "Sru_Crab", "SRU_Baron", "SRU_RiftHerald",
            "SRU_Dragon_Air", "SRU_Dragon_Water", "SRU_Dragon_Fire", "SRU_Dragon_Elder", "SRU_Dragon_Earth"
        };

        public static float GetSmiteDamage()
        {
            float[] Damages = new float[] { 390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760, 800, 850, 900, 950, 1000 };
            return Damages[Player.Instance.Level-1];
        }

        public static float GetSmiteKSDamage(SummonerManager.SmiteType type)
        {
            float damage = 0;
            switch (type)
            {
                case SummonerManager.SmiteType.Challenging:
                    damage = 54 + (6 * Player.Instance.Level);
                    break;
                case SummonerManager.SmiteType.Chilling:
                    damage = 20 + (8 * Player.Instance.Level);
                    break;
            }
            return damage;
        }

        public static int GetBarrierProtection()
        {
            return 95 + (20 * Player.Instance.Level);
        }

        public static int GetHealProtection()
        {
            return 75 + (15 * Player.Instance.Level);
        }

        public static bool IsUsingPotion(this Obj_AI_Base target)
        {
            return target.HasBuff("ItemDarkCrystalFlask") || target.HasBuff("ItemMiniRegenPotion") || target.HasBuff("ItemCrystalFlaskJungle") || target.HasBuff("Health Potion");
        }

        public static bool ShouldUseItem(this Obj_AI_Base target)
        {
            if (target.HasBuff("KatarinaR") || target.HasBuff("JhinRShot")) return false;
            return true;
        }

        public static Obj_AI_Minion GetBestFarmTarget(this Obj_AI_Base target, float range, int damage)
        {
            var minions = EloBuddy.SDK.EntityManager.MinionsAndMonsters.
                GetLaneMinions(EloBuddy.SDK.EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, range).
                OrderByDescending(h => h.Health);
            if (minions.FirstOrDefault() != null)
            {
                if (minions.FirstOrDefault().Health <= damage)
                    return minions.FirstOrDefault();
            }

            var monsters = EloBuddy.SDK.EntityManager.MinionsAndMonsters.
                GetJungleMonsters(Player.Instance.Position, range).
                OrderBy(h => h.Health);
            if (monsters.FirstOrDefault() != null)
            {
                return monsters.FirstOrDefault();
            }
            return new Obj_AI_Minion();
        }

        public static bool IsWallBetweenPlayer(Vector2 p)
        {
            AIHeroClient player = Player.Instance;
            var v1 = p - player.Position.To2D();
            for (float i = 0; i <= 1; i += 0.1f)
            {
                var v2 = player.Position.To2D() + i * v1;
                if (v2.IsWall()) return true;
            }

            return false;
        }

        public static int CountAlliesInPosition(this Vector3 position, float range)
        {
            return EloBuddy.SDK.EntityManager.Heroes.Allies.Where(e => e.IsAlive() && e.IsValid && !e.IsMe && e.IsInRange(position, range)).Count();
        }

        public static Vector3 GetAlliesGroup(float out_range, float inside_range, int allies)
        {
            foreach (var a in EloBuddy.SDK.EntityManager.Heroes.Allies.Where(a => a.IsAlive() && a.IsInRange(Player.Instance, out_range)))
                if (a.CountAllyChampionsInRange(inside_range) >= allies) return a.Position;
            return new Vector3();
        }

        public static Vector3 CenterOfVectors(List<Vector3> vectors)
        {
            Vector3 sum = Vector3.Zero;
            if (vectors == null || vectors.Count == 0) return sum;

            foreach (Vector3 vec in vectors) sum += vec;
            return sum / vectors.Count;
        }
    }
}
