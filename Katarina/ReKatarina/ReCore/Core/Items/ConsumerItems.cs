using EloBuddy;
using ReKatarina.ReCore.Managers;
using ReKatarina.ReCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReKatarina.ReCore.Core.Items
{
    class ConsumerItems : IItem
    {
        public void Execute()
        {
            if (MenuHelper.GetCheckBoxValue(ConfigList.Settings.Menu, "Settings.PreventCanceling") && !Player.Instance.ShouldUseItem()) return;

            if (!MenuHelper.GetCheckBoxValue(ConfigList.CItems.Menu, "Items.Consumer.Potions.Status") || Player.Instance.IsUsingPotion() || Player.Instance.HealthPercent > MenuHelper.GetSliderValue(ConfigList.CItems.Menu, "Items.Consumer.Health")) return;

            foreach (var item in Player.Instance.InventoryItems)
            {
                if (EloBuddy.SDK.Core.GameTickCount - ItemManager.GetLastUse(item.Id) < 500 || !item.CanUseItem()) continue;

                switch (item.Id)
                {
                    case ItemId.Total_Biscuit_of_Rejuvenation:
                    case ItemId.Health_Potion:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.CItems.Menu, "Items.Consumer.HealthPotion.Status")) continue;
                        item.Cast();
                        ItemManager.SetLastUse(item.Id);
                        break;

                    case ItemId.Refillable_Potion:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.CItems.Menu, "Items.Consumer.RefillablePotion.Status")) continue;
                        item.Cast();
                        ItemManager.SetLastUse(item.Id);
                        break;

                    case ItemId.Hunters_Potion:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.CItems.Menu, "Items.Consumer.HuntersPotion.Status")) continue;
                        item.Cast();
                        ItemManager.SetLastUse(item.Id);
                        break;

                    case ItemId.Corrupting_Potion:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.CItems.Menu, "Items.Consumer.CorruptingPotion.Status")) continue;
                        item.Cast();
                        ItemManager.SetLastUse(item.Id);
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
