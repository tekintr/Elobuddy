using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using EloBuddy.SDK.Enumerations;

namespace ReGaren.Utility
{
    public static class Flee
    {
        public static void Execute()
        {
            if (!SpellManager.Q.IsReady() || !ConfigList.Misc.FleeWithQ)
                return;

            SpellManager.Q.Cast();
        }
    }
}
