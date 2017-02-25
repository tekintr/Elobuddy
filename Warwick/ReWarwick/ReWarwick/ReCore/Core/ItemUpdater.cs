using EloBuddy;
using ReWarwick.ReCore.Managers;
using ReWarwick.ReCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReWarwick.ReCore.Core
{
    class ItemUpdater
    {
        public static void Update()
        {
            foreach (var module in ItemsList.modules)
            {
                module.Execute();
            }
        }
    }
}
