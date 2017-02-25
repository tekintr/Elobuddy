using EloBuddy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReGaren.ReCore.Managers;

namespace ReGaren.ReCore.Managers
{
    class EntityManager
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
    }
}
