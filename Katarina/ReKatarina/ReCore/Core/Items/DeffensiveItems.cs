using EloBuddy;
using EloBuddy.SDK;
using ReKatarina.ReCore.Managers;
using ReKatarina.ReCore.Utility;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReKatarina.ReCore.Core.Items
{
    class DeffensiveItems : IItem
    {
        public void Execute()
        {
            var target = TargetSelector.GetTarget(700.0f, DamageType.Mixed, Player.Instance.Position);
            int enemies = Player.Instance.CountEnemyChampionsInRange(MenuHelper.GetSliderValue(ConfigList.Settings.Menu, "Settings.Range")),
                allies = Player.Instance.CountAllyChampionsInRange(MenuHelper.GetSliderValue(ConfigList.Settings.Menu, "Settings.Range"));

            foreach (var item in Player.Instance.InventoryItems)
            { 
                if (EloBuddy.SDK.Core.GameTickCount - ItemManager.GetLastUse(item.Id) < 500 || !item.CanUseItem()) continue;

                switch (item.Id)
                {
                    case ItemId.Zhonyas_Hourglass:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Zhonya.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Zhonya.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (enemies < MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Zhonya.Enemies") || allies < MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Zhonya.Allies")) continue;
                        if (Player.Instance.HealthPercent > MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Zhonya.Me.MinHealth")) continue;
                        item.Cast();
                        ItemManager.SetLastUse(item.Id);
                        break;

                    case ItemId.Locket_of_the_Iron_Solari:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Solari.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Solari.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (enemies < MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Solari.Enemies") || allies < MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Solari.Allies")) continue;
                        if (Player.Instance.HealthPercent <= MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Solari.Me.MinHealth"))
                            item.Cast();

                        var allies_list = EloBuddy.SDK.EntityManager.Heroes.Allies.Where(c => c.IsAlive() && c.IsInRange(Player.Instance, 600));
                        foreach (var a in allies_list.Where(a => !a.IsMe && a.IsInRange(Player.Instance, 1050)))
                        {
                            if (a.HealthPercent <= MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Solari.Ally.MinHealth"))
                            {
                                item.Cast();
                                break;
                            }
                        }

                        if (allies_list.Sum(a => a.HealthPercent) / allies_list.Count() <= MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Solari.Allies.MinHealth"))
                            item.Cast();
                        break;

                    case ItemId.Face_of_the_Mountain:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Fotm.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Fotm.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (Player.Instance.HealthPercent <= MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Fotm.Me.MinHealth"))
                        {
                            InfoManager.Show(item, Player.Instance);
                            item.Cast(Player.Instance);
                            ItemManager.SetLastUse(item.Id);
                            continue;
                        }

                        foreach (var a in EloBuddy.SDK.EntityManager.Heroes.Allies.Where(a => !a.IsMe))
                        {
                            if (MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, $"Items.Deffensive.Fotm.Use.{a.ChampionName}") && a.HealthPercent <= MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Fotm.Ally.MinHealth"))
                            {
                                InfoManager.Show(item, a);
                                item.Cast(a);
                                ItemManager.SetLastUse(item.Id);
                                continue;
                            }
                        }
                        break;

                    case ItemId.Seraphs_Embrace:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Seraph.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Seraph.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (enemies < MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Seraph.Enemies")) continue;
                        if (Player.Instance.HealthPercent > MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Seraph.Me.MinHealth")) continue;
                        InfoManager.Show(item, Player.Instance);
                        item.Cast();
                        ItemManager.SetLastUse(item.Id);
                        break;

                    case ItemId.Redemption:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Redemption.Status")) continue;

                        var rAllies = EloBuddy.SDK.EntityManager.Heroes.Allies.Where(a => a.IsInRange(Managers.EntityManager.GetAlliesGroup(5500, 550, MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Redemption.Allies")), 550));
                        if (rAllies.Count() < MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Redemption.Allies")) continue;
                        if (rAllies.Sum(e => Prediction.Health.GetPrediction(e, 2500)) / rAllies.Sum(e => e.TotalMaxHealth()) > MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Redemption.Hp")) continue;

                        List<Vector3> vectors = new List<Vector3>();
                        foreach (var a in rAllies) vectors.Add(a.Position);
                        item.Cast(Managers.EntityManager.CenterOfVectors(vectors));
                        ItemManager.SetLastUse(item.Id);
                        InfoManager.Show(item, Player.Instance);
                        break;

                    case ItemId.Randuins_Omen:
                        if (!MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Omen.Status")) continue;
                        if (MenuHelper.GetCheckBoxValue(ConfigList.DItems.Menu, "Items.Deffensive.Omen.ComboOnly") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) continue;
                        if (Player.Instance.CountEnemyChampionsInRange(550) < MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Omen.Enemies")) continue;
                        if (Player.Instance.HealthPercent < MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Omen.Me.MinHealth")) continue;
                        if (target.HealthPercent > MenuHelper.GetSliderValue(ConfigList.DItems.Menu, "Items.Deffensive.Omen.Enemy.MinHealth")) continue;
                        ItemManager.SetLastUse(item.Id);
                        item.Cast();
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
