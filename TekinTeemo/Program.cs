using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace TekinTeemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static Spell.Targeted Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Skillshot R;
        public static Menu TeemoMenu, ComboMenu, HarassMenu, FarmMenu, FleeMenu;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            TargetSelector2.init();
            Bootstrap.Init(null);

            Q = new Spell.Targeted(SpellSlot.Q, 680);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Skillshot(SpellSlot.R, 300, SkillShotType.Circular, 500, 1000, 120);

            TeemoMenu = MainMenu.AddMenu("TekinTeemo", "TekinTeemo");
            TeemoMenu.AddGroupLabel("TekinTeemo");
            TeemoMenu.AddSeparator();
            TeemoMenu.AddLabel("Made By TekinTR");
            TeemoMenu.AddLabel("Good Luck :)");

            ComboMenu = TeemoMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.AddSeparator();
            ComboMenu.Add("useQCombo", new CheckBox("Use Q"));
            ComboMenu.Add("useWCombo", new CheckBox("Use W"));
            ComboMenu.Add("useRCombo", new CheckBox("Use R"));

            HarassMenu = TeemoMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.AddSeparator();
            HarassMenu.Add("useQHarass", new CheckBox("Use Q"));
            HarassMenu.Add("useWHarass", new CheckBox("Use W"));

            FarmMenu = TeemoMenu.AddSubMenu("LaneClear", "LaneClear");
            FarmMenu.AddGroupLabel("LaneClear Settings");
            FarmMenu.AddSeparator();
            FarmMenu.Add("useQFarmLH", new CheckBox("Use Q for LastHit"));
            FarmMenu.Add("useQ", new CheckBox("Use Q LaneClear"));
            FarmMenu.Add("useR", new CheckBox("Use R LaneClear"));

            FleeMenu = TeemoMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.AddGroupLabel("Flee Settings");
            FleeMenu.AddSeparator();
            FleeMenu.Add("useRFlee", new CheckBox("Use R"));
            FleeMenu.Add("useWFlee", new CheckBox("Use W"));

            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateHandler.Combo();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                StateHandler.Harass();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateHandler.WaveClear();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                StateHandler.LastHit();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                StateHandler.Flee();
            }
        }
    }
}
