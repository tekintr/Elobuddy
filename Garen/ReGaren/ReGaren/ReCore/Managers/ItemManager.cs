using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReGaren.ReCore.Managers
{
    static class ItemManager
    {
        private static Dictionary<ItemId, int> LastUse = new Dictionary<ItemId, int>();
        public static int GetLastUse(ItemId item)
        {
            if (LastUse.ContainsKey(item)) return LastUse[item];
            return 0;
        }

        public static void SetLastUse(ItemId item)
        {
            if (LastUse.ContainsKey(item)) LastUse[item] = EloBuddy.SDK.Core.GameTickCount;
            else LastUse.Add(item, EloBuddy.SDK.Core.GameTickCount);
        }

        public static List<Item> OffensiveItems = new List<Item>
        {
            new Item(ItemId.Titanic_Hydra, Player.Instance.GetAutoAttackRange()),
            new Item(ItemId.Ravenous_Hydra, 400),
            new Item(ItemId.Bilgewater_Cutlass, 550),
            new Item(ItemId.Blade_of_the_Ruined_King, 550),
            new Item(ItemId.Youmuus_Ghostblade, 1000),
            new Item(ItemId.Hextech_Gunblade, 700),
            new Item(ItemId.Frost_Queens_Claim)
        };

        public static List<Item> DeffensiveItems = new List<Item>
        {
            new Item(ItemId.Talisman_of_Ascension),
            new Item(ItemId.Locket_of_the_Iron_Solari, 600),
            new Item(ItemId.Zhonyas_Hourglass),
            new Item(ItemId.Face_of_the_Mountain, 1050),
            new Item(ItemId.Randuins_Omen, 500),
            new Item(ItemId.Seraphs_Embrace),
            new Item(ItemId.Ohmwrecker, 850),
            new Item(ItemId.Edge_of_Night),
            new Item(ItemId.Redemption, 5500)
        };

        public static List<Item> CleansersItems = new List<Item>
        {
            new Item(ItemId.Dervish_Blade),
            new Item(ItemId.Mikaels_Crucible, 750),
            new Item(ItemId.Mercurial_Scimitar),
            new Item(ItemId.Quicksilver_Sash),
        };

        public static List<Item> ComsumableItems = new List<Item>
        {
            new Item(ItemId.Dervish_Blade),
            new Item(ItemId.Mikaels_Crucible, 750),
            new Item(ItemId.Mercurial_Scimitar),
            new Item(ItemId.Quicksilver_Sash),
        };

        public static bool IsUsingPotion(this Obj_AI_Base target)
        {
            return target.HasBuff("ItemDarkCrystalFlask") || target.HasBuff("ItemMiniRegenPotion") || target.HasBuff("ItemCrystalFlaskJungle") || target.HasBuff("Health Potion");
        }

        public static void Initialize()
        {
            
        }
    }
}
