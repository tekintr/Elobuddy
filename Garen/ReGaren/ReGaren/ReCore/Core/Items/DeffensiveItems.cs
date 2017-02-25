using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReGaren.ReCore.Core.Items
{
    class DeffensiveItems : IItem
    {
        public void Execute()
        {
            var target = TargetSelector.GetTarget(700.0f, DamageType.Mixed, Player.Instance.Position);
            foreach (var item in Player.Instance.InventoryItems)
            {
                switch (item.Id)
                {
                    default:
                        break;
                }
            }
        }

        public void OnDraw()
        {
            return;
        }

        public void OnEndScene()
        {
            return;
        }
    }
}
