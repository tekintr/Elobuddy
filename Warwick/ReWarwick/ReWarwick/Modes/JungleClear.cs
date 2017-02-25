using EloBuddy;
using EloBuddy.SDK;
using ReWarwick.Utils;
using System;
using System.Linq;

namespace ReWarwick.Modes
{
    class JungleClear
    {
        public static void Execute()
        {
            var monsters = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, SpellManager.Q.Range);
            if (monsters == null || !monsters.Any()) return;

            if (Config.Farm.Menu.GetCheckBoxValue("Config.Farm.Q.Status") && SpellManager.Q.IsReady() && (Player.Instance.Level < 4 || Player.Instance.ManaPercent >= Config.Farm.Menu.GetSliderValue("Config.Farm.Q.Mana")))
            {
                var target = monsters.OrderByDescending(h => h.Health).FirstOrDefault();
                SpellManager.Q.Cast(target);
            }

            if (Config.Farm.Menu.GetCheckBoxValue("Config.Farm.E.Status") && !Player.Instance.HasBuff("WarwickE") && SpellManager.E.IsReady() && (Player.Instance.Level < 4 || Player.Instance.ManaPercent >= Config.Farm.Menu.GetSliderValue("Config.Farm.E.Mana")))
            {
                if (monsters.Count() >= 2 || Player.Instance.HealthPercent <= 40)
                    SpellManager.E.Cast();
            }
        }
    }
}
