// Copyright 2014 - 2014 Esk0r
// Config.cs is part of Evade.
// 
// Evade is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Evade is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Evade. If not, see <http://www.gnu.org/licenses/>.

#region

using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

#endregion

namespace Evade
{
    internal static class Config
    {
        public const bool PrintSpellData = false;
        public const bool TestOnAllies = false;
        public const int SkillShotsExtraRadius = 9;
        public const int SkillShotsExtraRange = 20;
        public const int GridSize = 10;
        public const int ExtraEvadeDistance = 15;
        public const int PathFindingDistance = 60;
        public const int PathFindingDistance2 = 35;

        public const int DiagonalEvadePointsCount = 7;
        public const int DiagonalEvadePointsStep = 20;

        public const int CrossingTimeOffset = 250;

        public const int EvadingFirstTimeOffset = 250;
        public const int EvadingSecondTimeOffset = 80;

        public const int EvadingRouteChangeTimeOffset = 250;

        public const int EvadePointChangeInterval = 300;
        public static int LastEvadePointChangeT = 0;

        public static Menu Menu, evadeSpells, skillShots, shielding, collision, drawings, misc;
        public static Color EnabledColor, DisabledColor, MissileColor;

        public static void CreateMenu()
        {
            #region Menu
            Menu = MainMenu.AddMenu("Evade#", "evade");
            Menu.Add("Enabled", new KeyBind("Aktif", true, KeyBind.BindTypes.PressToggle));
            Menu.Add("OnlyDangerous", new KeyBind("Sadece tehlikeden kac", false, KeyBind.BindTypes.HoldActive)); //
            Menu.AddSeparator(50);

            Menu.AddLabel("Nên để Delay between movements in milliseconds >= 240 để né tốt nhất"); 

            if (Menu == null)
            {
                Chat.Print("Yukleme Basarisiz", Color.Red);
                Console.WriteLine("Evade:: Yukleme Basarisiz");
                throw new NullReferenceException("Menu NullReferenceException");
            }

            #endregion Menu

            #region Evade Spells

            //Create the evade spells submenus.
            evadeSpells = Menu.AddSubMenu("Evade spells", "evadeSpells");
            foreach (var spell in EvadeSpellDatabase.Spells)
            {
                evadeSpells.AddGroupLabel(spell.Name);

                try
                {
                    evadeSpells.Add("DangerLevel" + spell.Name, new Slider("Danger level", spell._dangerLevel, 1, 5));
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (spell.IsTargetted && spell.ValidTargets.Contains(SpellValidTargets.AllyWards))
                {
                    evadeSpells.Add("WardJump" + spell.Name, new CheckBox("TotemeZipla"));
                }

                evadeSpells.Add("Enabled" + spell.Name, new CheckBox("Aktif"));
            }

            #endregion Evade Spells

            #region Skillshots

            //Create the skillshots submenus.
            skillShots = Menu.AddSubMenu("Skillshots", "Skillshots");

            foreach (var hero in ObjectManager.Get<AIHeroClient>())
            {
                if (hero.Team != ObjectManager.Player.Team || Config.TestOnAllies)
                {
                    foreach (var spell in SpellDatabase.Spells)
                    {
                        if (String.Equals(spell.ChampionName, hero.ChampionName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            skillShots.AddGroupLabel(spell.SpellName);
                            skillShots.Add("DangerLevel" + spell.MenuItemName, new Slider("Tehlike Seviyesi", spell.DangerValue, 1, 5));

                            skillShots.Add("IsDangerous" + spell.MenuItemName, new CheckBox("Tehlikeli", spell.IsDangerous));

                            skillShots.Add("Draw" + spell.MenuItemName, new CheckBox("Goster"));
                            skillShots.Add("Enabled" + spell.MenuItemName, new CheckBox("Aktif", !spell.DisabledByDefault));
                        }
                    }
                }
            }

            #endregion Skillshots

            #region Shielding

            shielding = Menu.AddSubMenu("Ally shielding", "Shielding");

            foreach (var ally in ObjectManager.Get<AIHeroClient>())
            {
                if (ally.IsAlly && !ally.IsMe)
                {
                    shielding.Add("shield" + ally.ChampionName, new CheckBox("Kalkan " + ally.ChampionName));
                }
            }

            #endregion Shielding

            #region Collision

            collision = Menu.AddSubMenu("Collision", "Collision");
            collision.Add("MinionCollision", new CheckBox("Minyon carpismasi", true));
            collision.Add("HeroCollision", new CheckBox("Sampiyon carpismasi", false));
            collision.Add("YasuoCollision", new CheckBox("Yasuo duvar carpismasi"));
            collision.Add("EnableCollision", new CheckBox("Aktif"));
            //TODO add mode.

            #endregion Collision

            #region Drawings

            drawings = Menu.AddSubMenu("Drawings", "Drawings");
            
            drawings.AddLabel("Enabled Draw Color = White");
            drawings.Add("EnabledDraw", new CheckBox("Gosterge Aktif"));
            EnabledColor = Color.White;

            drawings.AddLabel("Disabled Draw Color = Red");
            drawings.Add("DisabledDraw", new CheckBox("Gosterge Pasif"));
            DisabledColor = Color.Red;

            drawings.AddLabel("Missile Draw Color= Lime");
            drawings.Add("MissileDraw", new CheckBox("Fuzeleri Goster"));
            MissileColor = Color.Lime;

            //drawings.AddItem(new MenuItem("EnabledColor", "Enabled spell color").SetValue(Color.White));
            //drawings.AddItem(new MenuItem("DisabledColor", "Disabled spell color").SetValue(Color.Red));
            //drawings.AddItem(new MenuItem("MissileColor", "Missile color").SetValue(Color.LimeGreen));
            drawings.Add("Border", new Slider("Border Width", 1, 2, 1));

            drawings.Add("EnableDrawings", new CheckBox("Aktif"));
            drawings.Add("ShowEvadeStatus", new CheckBox("Koruma durumunu goster"));

            #endregion Drawings

            #region Misc

            misc = Menu.AddSubMenu("Misc", "Misc");
            misc.Add("BlockSpells", new ComboBox("Kacarken buyuleri engelle", 2, "Hayır", "Sadece tehlike", "Surekli"));
            //misc.Add("BlockSpells", "Block spells while evading").SetValue(new StringList(new []{"No", "Only dangerous", "Always"}, 1)));
            misc.Add("DisableFow", new CheckBox("Disable fog of war dodging", false));
            //misc.Add("ShowEvadeStatus", new CheckBox("Show Evade Status", false));
            if (ObjectManager.Player.BaseSkinName == "Olaf")
            {
                misc.Add("DisableEvadeForOlafR", new CheckBox("Olaf ultisi acikken evadeyi otomaik kapat!"));
            }
            #endregion Misc
        }
    }
}
