using EloBuddy;
using EloBuddy.SDK;
using ReGaren.ReCore.Managers;
using ReGaren.ReCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReGaren.ReCore.Core.Items
{
    class OffensiveItems : IItem
    {
        public void Execute()
        {
            var target = TargetSelector.GetTarget(700.0f, DamageType.Mixed, Player.Instance.Position);
            if (target == null) return;
            int enemies = Player.Instance.CountEnemyChampionsInRange(MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Range"));

            foreach (var item in Player.Instance.InventoryItems)
            {
                if (EloBuddy.SDK.Core.GameTickCount - ItemManager.GetLastUse(item.Id) < 500 || !item.CanUseItem()) continue;

                switch (item.Id)
                {
                    case ItemId.Youmuus_Ghostblade:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.OItems.Menu, "Items.Offensive.Youmuu.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(ConfigList.OItems.Menu, "Items.Offensive.Youmuu.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (enemies < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Youmuu.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Youmuu.Me.MinHealth") ||  target.HealthPercent < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Youmuu.Enemy.MinHealth")) continue;
                        item.Cast();
                        ItemManager.SetLastUse(item.Id);
                        break;

                    case ItemId.Bilgewater_Cutlass:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.OItems.Menu, "Items.Offensive.Cutlass.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(ConfigList.OItems.Menu, "Items.Offensive.Cutlass.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (enemies < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Cutlass.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Cutlass.Me.MinHealth") || target.HealthPercent < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Cutlass.Enemy.MinHealth")) continue;
                        item.Cast(target);
                        ItemManager.SetLastUse(item.Id);
                        break;

                    case ItemId.Blade_of_the_Ruined_King:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.OItems.Menu, "Items.Offensive.Botrk.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(ConfigList.OItems.Menu, "Items.Offensive.Botrk.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (enemies < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Botrk.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Botrk.Me.MinHealth") || target.HealthPercent < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Botrk.Enemy.MinHealth")) continue;
                        item.Cast(target);
                        ItemManager.SetLastUse(item.Id);
                        break;

                    case ItemId.Hextech_Gunblade:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.OItems.Menu, "Items.Offensive.Gunblade.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(ConfigList.OItems.Menu, "Items.Offensive.Gunblade.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (enemies < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Gunblade.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Gunblade.Me.MinHealth") || target.HealthPercent < MenuHelper.GetSliderValue(ConfigList.OItems.Menu, "Items.Offensive.Gunblade.Enemy.MinHealth")) continue;
                        item.Cast(target);
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
