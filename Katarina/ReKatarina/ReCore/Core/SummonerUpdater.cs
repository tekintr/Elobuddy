using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReKatarina.ReCore.Utility;

namespace ReKatarina.ReCore.Core
{
    class SummonerUpdater
    {
        public static void Update()
        {
            foreach (var module in SummonerList.modules.Where(module => module.ShouldGetExecuted()))
            {
                module.Execute();
            }
        }
    }
}
