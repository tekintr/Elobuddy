using ReKatarina.ReCore.Core;
using System.Collections.Generic;

namespace ReKatarina.ReCore.Utility
{
    class SummonerList
    {
        public static List<ISpell> modules = new List<ISpell>();
        public static void Initialize()
        {
            if (Managers.SummonerManager.PlayerHasBarrier) modules.Add(new Core.Spells.Barrier());
            if (Managers.SummonerManager.PlayerHasCleanse) modules.Add(new Core.Spells.Cleanse());
            if (Managers.SummonerManager.PlayerHasExhaust) modules.Add(new Core.Spells.Exhaust());
            if (Managers.SummonerManager.PlayerHasHeal) modules.Add(new Core.Spells.Heal());
            if (Managers.SummonerManager.PlayerHasIgnite) modules.Add(new Core.Spells.Ignite());
            if (Managers.SummonerManager.PlayerHasSmite) modules.Add(new Core.Spells.Smite());
            if (Managers.SummonerManager.PlayerHasSnowball) modules.Add(new Core.Spells.Snowball());
        }
    }
}
