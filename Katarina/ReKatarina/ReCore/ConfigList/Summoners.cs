using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReKatarina.ReCore.Utility;

namespace ReKatarina.ReCore.ConfigList
{
    public static class Summoners
    {
        public static readonly Menu Menu;

        static Summoners()
        {
            Menu = Loader.Menu.AddSubMenu("Summoners");
            Menu.AddGroupLabel("Summoners settings");
            #region Smite
            if (Managers.SummonerManager.PlayerHasSmite)
            {
                Menu.AddGroupLabel("Smite");
                Menu.CreateKeyBind("Enable smite", "Summoners.Smite.Keybind", 'S', 'B');
                Menu.AddSeparator(10);
                Menu.CreateCheckBox("Draw smite range", "Summoners.Smite.Draw.Range", false);
                Menu.CreateCheckBox("Draw smite status", "Summoners.Smite.Draw.Status");
                Menu.CreateCheckBox("Draw smite damage", "Summoners.Smite.Draw.Damage");
                Menu.AddSeparator(10);
                Menu.CreateSlider("Keep {0} smite's", "Summoners.Smite.Keep.Count", 1, 0, 2);
                Menu.CreateSlider("Subtract {0} from damage calculations", "Summoners.Smite.Substract", 5, 0, 35);
                Menu.CreateCheckBox("Use smite on champions", "Summoners.Smite.Champions");
                Menu.CreateSlider("Smite if enemies HP <= {0}%", "Summoners.Smite.Champions.Health", 45, 0, 100);
                Menu.CreateCheckBox("KS with smite", "Summoners.Smite.KillSteal");
                Menu.AddSeparator(10);
                switch (Game.MapId)
                {
                    case GameMapId.SummonersRift:
                        Menu.AddLabel("Epic monsters");
                        Menu.CreateCheckBox("Smite Baron", "Smite.Monster." + "SRU_Baron");
                        Menu.CreateCheckBox("Smite Herald", "Smite.Monster." + "SRU_RiftHerald");
                        Menu.CreateCheckBox("Smite Cloud Drake", "Smite.Monster." + "SRU_Dragon_Air");
                        Menu.CreateCheckBox("Smite Infernal Drake", "Smite.Monster." + "SRU_Dragon_Fire");
                        Menu.CreateCheckBox("Smite Mountain Drake", "Smite.Monster." + "SRU_Dragon_Earth");
                        Menu.CreateCheckBox("Smite Ocean Drake", "Smite.Monster." + "SRU_Dragon_Water");
                        Menu.CreateCheckBox("Smite Elder Drake", "Smite.Monster." + "SRU_Dragon_Elder");
                        Menu.AddLabel("Normal monsters");
                        Menu.CreateCheckBox("Smite Red", "Smite.Monster." + "SRU_Red");
                        Menu.CreateCheckBox("Smite Blue", "Smite.Monster." + "SRU_Blue");
                        Menu.AddLabel("Other monsters");
                        Menu.CreateCheckBox("Smite Gromp", "Smite.Monster." + "SRU_Gromp", false);
                        Menu.CreateCheckBox("Smite Murkwolf", "Smite.Monster." + "SRU_Murkwolf", false);
                        Menu.CreateCheckBox("Smite Razorbeak", "Smite.Monster." + "SRU_Razorbeak", false);
                        Menu.CreateCheckBox("Smite Krug", "Smite.Monster." + "SRU_Krug", false);
                        Menu.CreateCheckBox("Smite Crab", "Smite.Monster." + "Sru_Crab", false);
                        break;
                    case GameMapId.TwistedTreeline:
                        Menu.AddLabel("Epic monsters");
                        Menu.CreateCheckBox("Smite Vilemaw", "Smite.Monster." + "TT_Spiderboss");
                        Menu.AddLabel("Normal monsters");
                        Menu.CreateCheckBox("Smite Golem", "Smite.Monster." + "TTNGolem");
                        Menu.CreateCheckBox("Smite Wolf", "Smite.Monster." + "TTNWolf");
                        Menu.CreateCheckBox("Smite Wraith", "Smite.Monster." + "TTNWraith");
                        break;
                }
                Menu.AddSeparator(10);
            }
            #endregion
            #region Ignite
            if (Managers.SummonerManager.PlayerHasIgnite)
            {
                Menu.AddGroupLabel("Ignite");
                Menu.CreateCheckBox("Enable ignite", "Summoners.Ignite.Status");
                Menu.CreateCheckBox("KS with ignite", "Summoners.Ignite.KillSteal");
                Menu.CreateSlider("Ignite if enemies HP <= {0}%", "Summoners.Ignite.Health", 15);
                Menu.AddSeparator(10);
            }
            #endregion
            #region Exhaust
            if (Managers.SummonerManager.PlayerHasExhaust)
            {
                Menu.AddGroupLabel("Exhaust");
                Menu.CreateCheckBox("Enable exhaust", "Summoners.Exhaust.Status");
                Menu.CreateSlider("Exhaust if enemies HP <= {0}%", "Summoners.Exhaust.Health", 35);
                Menu.AddSeparator(10);
            }
            #endregion
            #region Heal
            if (Managers.SummonerManager.PlayerHasHeal)
            {
                Menu.AddGroupLabel("Heal");
                Menu.AddLabel("Heal can be configurable in Protector section.");
                Menu.AddSeparator(10);
            }
            #endregion
            #region Barrier
            if (Managers.SummonerManager.PlayerHasBarrier)
            {
                Menu.AddGroupLabel("Barrier");
                Menu.CreateCheckBox("Enable barrier", "Summoners.Barrier.Status");
                Menu.CreateCheckBox("Barrier only dangerous", "Summoners.Barrier.Dangerous");
                Menu.CreateSlider("Barrier if my HP <= {0}%", "Summoners.Barrier.Health", 10);
                Menu.AddSeparator(10);
            }
            #endregion
            #region Snowball
            if (Managers.SummonerManager.PlayerHasSnowball)
            {
                Menu.AddGroupLabel("Snowball");
                Menu.CreateCheckBox("Enable snowball", "Summoners.Snowball.Status");
                Menu.AddSeparator(10);
            }
            #endregion
            #region Cleanse
            if (Managers.SummonerManager.PlayerHasCleanse)
            {
                Menu.AddGroupLabel("Cleanse");
                Menu.AddLabel("Cleanse can be configurable in Cleansers section.");
                Menu.AddSeparator(10);
            }
            #endregion
        }

        public static void Initialize()
        {
        }
    }
}