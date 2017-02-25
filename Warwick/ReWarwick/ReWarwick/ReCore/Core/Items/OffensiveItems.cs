using EloBuddy;
using EloBuddy.SDK;
using ReWarwick.ReCore.Managers;
using ReWarwick.ReCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReWarwick.ReCore.Core.Items
{
    class OffensiveItems : IItem
    {
        public void Execute()
        {
            if (MenuHelper.GetCheckBoxValue(Config.Settings.Menu, "Settings.PreventCanceling") && !Player.Instance.ShouldUseItem()) return;

            var target = TargetSelector.GetTarget(700.0f, DamageType.Mixed, Player.Instance.Position);
            if (target == null || target.IsInvulnerable) return;
            int enemies = Player.Instance.CountEnemyChampionsInRange(MenuHelper.GetSliderValue(Config.Settings.Menu, "Settings.Range"));

            foreach (var item in Player.Instance.InventoryItems)
            {
                if (EloBuddy.SDK.Core.GameTickCount - ItemManager.GetLastUse(item.Id) < 500 || !item.CanUseItem()) continue;

                switch (item.Id)
                {
                    case ItemId.Ravenous_Hydra:
                    case ItemId.Tiamat:
                        if (!MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Tiamat.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Tiamat.Combo") && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                            if (target.IsInRange(Player.Instance, 400))
                                item.Cast();

                        if (MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Tiamat.Farm") && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                            if (Player.Instance.GetBestFarmTarget(400.0f, (int)(Player.Instance.BaseAttackDamage * 0.6)) != null)
                                item.Cast();
                       break;

                    case ItemId.Titanic_Hydra:
                        if (!MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.TitanicHydra.Status")) continue;
                        if (!target.IsInRange(Player.Instance, MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.TitanicHydra.Distance"))) continue;
                        if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                            item.Cast();
                        break;

                    case ItemId.Youmuus_Ghostblade:
                        if (!MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Youmuu.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Youmuu.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (enemies < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Youmuu.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Youmuu.Me.MinHealth") ||  target.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Youmuu.Enemy.MinHealth")) continue;
                        item.Cast();
                        ItemManager.SetLastUse(item.Id);
                        InfoManager.Show(item, target);
                        break;

                    case ItemId.Bilgewater_Cutlass:
                        if (!MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Cutlass.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Cutlass.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (!target.IsInRange(Player.Instance, 550.0f)) continue;
                        if (enemies < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Cutlass.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Cutlass.Me.MinHealth") || target.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Cutlass.Enemy.MinHealth")) continue;
                        item.Cast(target);
                        ItemManager.SetLastUse(item.Id);
                        InfoManager.Show(item, target);
                        break;

                    case ItemId.Blade_of_the_Ruined_King:
                        if (!MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Botrk.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Botrk.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (!target.IsInRange(Player.Instance, 550.0f)) continue;
                        if (enemies < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Botrk.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Botrk.Me.MinHealth") || target.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Botrk.Enemy.MinHealth")) continue;
                        item.Cast(target);
                        ItemManager.SetLastUse(item.Id);
                        InfoManager.Show(item, target);
                        break;

                    case ItemId.Hextech_Gunblade:
                        if (!MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Gunblade.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Gunblade.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (!target.IsInRange(Player.Instance, 700.0f)) continue;
                        if (enemies < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Gunblade.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Gunblade.Me.MinHealth") || target.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Gunblade.Enemy.MinHealth")) continue;
                        item.Cast(target);
                        ItemManager.SetLastUse(item.Id);
                        InfoManager.Show(item, target);
                        break;

                    case ItemId.Hextech_Protobelt_01:
                        if (!MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Protobelt.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Protobelt.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (enemies < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Protobelt.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Protobelt.Me.MinHealth") || target.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.Protobelt.Enemy.MinHealth")) continue;
                        if (!target.IsInRange(Player.Instance, 400.0f)) continue;
                        if (target.IsMelee && !MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Protobelt.Melee")) continue;
                        if (Managers.EntityManager.IsWallBetweenPlayer(target.Position.To2D()) && MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.Protobelt.EnableWall")) continue;
                        item.Cast(target.Position);
                        ItemManager.SetLastUse(item.Id);
                        InfoManager.Show(item, target);
                        break;

                    case ItemId.Hextech_GLP_800:
                        if (!MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.HextechGLP.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(Config.OItems.Menu, "Items.Offensive.HextechGLP.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (enemies < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.HextechGLP.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.HextechGLP.Me.MinHealth") || target.HealthPercent < MenuHelper.GetSliderValue(Config.OItems.Menu, "Items.Offensive.HextechGLP.Enemy.MinHealth")) continue;
                        if (!target.IsInRange(Player.Instance, 400.0f)) continue;
                        item.Cast(target.Position);
                        ItemManager.SetLastUse(item.Id);
                        InfoManager.Show(item, target);
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
