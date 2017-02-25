using EloBuddy;
using ReWarwick.ReCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReWarwick.ReCore.Managers
{
    public static class InfoManager
    {
        public static void Show(InventorySlot item, AIHeroClient target)
        {
            if (!MenuHelper.GetCheckBoxValue(Config.Settings.Menu, "Settings.Chat.Status")) return;
            string targetName = target.Name == Player.Instance.Name ? "me" : target.Name;
            Chat.Print($"<font color='#59c93a'>[ReCORE] </font><font color='#3fbffd'>{item.DisplayName} </font><font color='#f49e00'>has been used at </font><font color='#3fbffd'>{target.ChampionName} ({targetName})</font>");
        }
    }
}
