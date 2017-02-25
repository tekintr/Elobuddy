using EloBuddy;
using EloBuddy.SDK;
using System.Linq;
using ReGaren.ReCore.ConfigList;
using ReGaren.ReCore.Managers;
using ReGaren.ReCore.Utility;
using System;
using System.Collections.Generic;
using EloBuddy.SDK.Rendering;
using System.Drawing;
using SharpDX;

namespace ReGaren.ReCore.Core.Spells
{
    class Smite : ISpell
    {
        private static Text SmiteStatusText;
        public static void Initialize()
        {
            SmiteStatusText = new Text("Smite status", new Font("Euphemia", 10F, FontStyle.Bold)); 
        }
        public void Execute()
        {
            if (SummonerManager.Smite.Handle.Ammo <= MenuHelper.GetSliderValue(Summoners.Menu, "Summoners.Smite.Keep.Count")) return;
            Obj_AI_Minion Monster;
            Monster = EloBuddy.SDK.EntityManager.MinionsAndMonsters.GetJungleMonsters().
                        FirstOrDefault(m => Managers.EntityManager.MonsterSmiteables.Contains(m.BaseSkinName) &&
                                   Summoners.Menu.GetCheckBoxValue("Smite.Monster." + m.BaseSkinName) &&
                                   m.TotalShieldHealth() <= Managers.EntityManager.GetSmiteDamage() - Summoners.Menu.GetSliderValue("Summoners.Smite.Substract")              
                        );
            if (Monster != null)
                SummonerManager.Smite.Cast(Monster);

            if (!Summoners.Menu.GetCheckBoxValue("Summoners.Smite.Champions")) return;
            if (SummonerManager.Smite.Handle.Ammo <= MenuHelper.GetSliderValue(Summoners.Menu, "Summoners.Smite.Keep.Count")) return;

            if (Summoners.Menu.GetCheckBoxValue("Summoners.Smite.KillSteal"))
            {
                SummonerManager.SmiteType smiteType = SummonerManager.SmiteType.None;
                switch (SummonerManager.Smite.Handle.Name)
                {
                    case "s5_SummonerSmitePlayerGanker": smiteType = SummonerManager.SmiteType.Chilling; break;
                    case "s5_SummonerSmiteDuel": smiteType = SummonerManager.SmiteType.Challenging; break;
                }
                if (smiteType != SummonerManager.SmiteType.None)
                {
                    Obj_AI_Base ks = EloBuddy.SDK.EntityManager.Heroes.Enemies.FirstOrDefault(p =>
                                        Prediction.Health.GetPrediction(p, Game.Ping) <= Managers.EntityManager.GetSmiteKSDamage(smiteType) - Summoners.Menu.GetSliderValue("Summoners.Smite.Substract") &&
                                        p.IsValidTarget(SummonerManager.Smite.Range)
                                        );
                    if (ks != null)
                        SummonerManager.Smite.Cast(ks);
                }
            }

            if (SummonerManager.Smite.Handle.Ammo <= MenuHelper.GetSliderValue(Summoners.Menu, "Summoners.Smite.Keep.Count")) return;

            Obj_AI_Base target = TargetSelector.GetTarget(SummonerManager.Smite.Range, DamageType.Mixed);
            if (target == null || !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;
            switch (SummonerManager.Smite.Handle.Name)
            {
                case "S5_SummonerSmitePlayerGanker": // Blue Smite
                    if (target.HealthPercent <= Summoners.Menu.GetSliderValue("Summoners.Smite.Champions.Health") && target.IsInAutoAttackRange(Player.Instance))
                        SummonerManager.Smite.Cast(target);
                    break;
                case "S5_SummonerSmiteDuel": // Red Smite
                    if (target.HealthPercent <= Summoners.Menu.GetSliderValue("Summoners.Smite.Champions.Health") && target.IsInRange(Player.Instance, SummonerManager.Smite.Range / 2))
                        SummonerManager.Smite.Cast(target);
                    break;
                default:
                    break;
            }
        }

        public bool ShouldGetExecuted()
        {
            if (!SummonerManager.Smite.IsReady() || SummonerManager.Smite.IsOnCooldown || !MenuHelper.GetKeyBindValue(Summoners.Menu, "Summoners.Smite.Keybind"))
                return false;
            return true;
        }

        public void OnDraw()
        {
            if (!MenuHelper.GetKeyBindValue(Summoners.Menu, "Summoners.Smite.Keybind")) return;
            if (Summoners.Menu.GetCheckBoxValue("Summoners.Smite.Draw.Range"))
            {
                Circle.Draw(SharpDX.Color.Blue, SummonerManager.Smite.Range, Player.Instance);
            }
            if (Summoners.Menu.GetCheckBoxValue("Summoners.Smite.Draw.Status"))
            {
                SmiteStatusText.Position = Player.Instance.Position.WorldToScreen() - new Vector2(SmiteStatusText.Bounding.Width / 2f, -40);
                SmiteStatusText.TextValue = "Smite status";
                int charges = SummonerManager.Smite.Handle.Ammo;
                if (charges <= MenuHelper.GetSliderValue(Summoners.Menu, "Summoners.Smite.Keep.Count") && charges != 0)
                {
                    SmiteStatusText.TextValue += " - KEEP SMITE";
                    SmiteStatusText.Color = System.Drawing.Color.Orange;
                }
                else
                    switch(charges > 0)
                    {
                        case true:
                            SmiteStatusText.TextValue += " - READY";
                            SmiteStatusText.Color = System.Drawing.Color.Green;
                            break;
                        case false:
                            SmiteStatusText.TextValue += " - NOT READY";
                            SmiteStatusText.Color = System.Drawing.Color.Red;
                            break;
                    }
                SmiteStatusText.Draw();
            }
        }

        internal static int Height;
        internal static int Width;
        internal static int OffsetX;
        internal static int OffsetY;

        public void OnEndScene()
        {
            if (Summoners.Menu.GetCheckBoxValue("Summoners.Smite.Draw.Damage") && SummonerManager.Smite.IsReady() && !SummonerManager.Smite.IsOnCooldown)
            {
                foreach (var unit in EloBuddy.SDK.EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsHPBarRendered && !m.Name.Contains("Mini") && m.IsValidTarget(SummonerManager.Smite.Range + 350)))
                {
                    #region Monsters
                    if (unit.Name.Contains("Herald"))
                    {
                        Height = 10;
                        Width = 142;
                        OffsetX = -4;
                        OffsetY = 7;
                    }
                    else if (unit.Name.Contains("Baron"))
                    {
                        Height = 12;
                        Width = 191;
                        OffsetX = -29;
                        OffsetY = 6;
                    }
                    else if (unit.Name.Contains("Dragon"))
                    {
                        Height = 10;
                        Width = 143;
                        OffsetX = -4;
                        OffsetY = 8;
                    }
                    else if (unit.Name.Contains("Blue"))
                    {
                        Height = 9;
                        Width = 142;
                        OffsetX = -4;
                        OffsetY = 7;
                    }
                    else if (unit.Name.Contains("Red"))
                    {
                        Height = 9;
                        Width = 142;
                        OffsetX = -4;
                        OffsetY = 7;
                    }
                    else if (unit.Name.Contains("Gromp"))
                    {
                        Height = 4;
                        Width = 92;
                        OffsetX = 21;
                        OffsetY = 8;
                    }
                    else if (unit.Name.Contains("Krug"))
                    {
                        Height = 4;
                        Width = 91;
                        OffsetX = 21;
                        OffsetY = 7;
                    }
                    else if (unit.Name.Contains("Murkwolf"))
                    {
                        Height = 4;
                        Width = 92;
                        OffsetX = 21;
                        OffsetY = 7;
                    }
                    else if (unit.Name.Contains("Razorbeak"))
                    {
                        Height = 4;
                        Width = 92;
                        OffsetX = 21;
                        OffsetY = 7;
                    }
                    else if (unit.Name.Contains("Crab"))
                    {
                        Height = 2;
                        Width = 61;
                        OffsetX = 36;
                        OffsetY = 21;
                    }
                    #endregion
                    DrawSmiteDamage(unit);
                }
            }
        }

        private static void DrawSmiteDamage(Obj_AI_Base unit)
        {
            var barPos = unit.HPBarPosition;
            bool killable = unit.Health < Managers.EntityManager.GetSmiteDamage() - Summoners.Menu.GetSliderValue("Summoners.Smite.Substract") ? true : false;
            var percentHealthAfterDamage = Math.Max(0, unit.Health - Managers.EntityManager.GetSmiteDamage()) /
                                           unit.MaxHealth;
            var currentHealthPercentage = unit.Health /
                                          unit.MaxHealth;

            var startX = barPos.X + OffsetX + (percentHealthAfterDamage * Width);
            var endX = barPos.X + OffsetX + (currentHealthPercentage * Width);
            var y = barPos.Y + OffsetY;

            Drawing.DrawLine(new Vector2(startX, y), new Vector2(endX, y), Height, killable ? System.Drawing.Color.FromArgb(51, 204, 102) : System.Drawing.Color.FromArgb(230, 126, 34));
        }
    }
}
