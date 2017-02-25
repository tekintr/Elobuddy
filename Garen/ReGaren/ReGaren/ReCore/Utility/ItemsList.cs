using ReGaren.ReCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReGaren.ReCore.Utility
{
    class ItemsList
    {
        public static List<IItem> modules = new List<IItem>();
        public static void Initialize()
        {
            modules.Add(new Core.Items.OffensiveItems());
            modules.Add(new Core.Items.DeffensiveItems());
            modules.Add(new Core.Items.ConsumerItems());
        }
    }
}
