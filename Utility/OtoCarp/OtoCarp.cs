using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = System.Drawing;

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using EloBuddy.SDK.Rendering;

namespace OtoCarp
{
    class OtoCarp : Extensions
    {
        public static void Load(EventArgs args)
        {
            try
            {
                if (Keybind(Settings.Principal, "Enable"))
                {
                    foreach (var Monsters in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(x => x.IsHPBarRendered && MNames.Contains(x.BaseSkinName)))
                    {
                        if (Monsters.Position.Distance(Player.Instance.Position) <= 650 && CheckBox(Settings.Principal, Monsters.BaseSkinName))
                        {
                            if (Smite.IsReady())
                            {
                                if (Monsters.Health <= Player.Instance.GetSummonerSpellDamage(Monsters, DamageLibrary.SummonerSpells.Smite))
                                {
                                    Smite.Cast(Monsters);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[TICK]: " + e.Message);
            }
        }
        
        public static void Draw(EventArgs args)
        {
            try
            {
                if (!CheckBox(Settings.Principal, "Draw"))
                    return;

                Drawing.DrawText(Drawing.WorldToScreen(Player.Instance.Position).X - 40, Drawing.WorldToScreen(Player.Instance.Position).Y + 20, C.Color.White, "Smite");
                Drawing.DrawText(Drawing.WorldToScreen(Player.Instance.Position).X + 12, Drawing.WorldToScreen(Player.Instance.Position).Y + 20, Smite.IsReady() ? C.Color.LimeGreen : C.Color.Red, Smite.IsReady() ? "(Ready)" : "(Cooldown)");

                if (Smite.IsReady())
                {
                    Drawing.DrawCircle(Player.Instance.Position, Smite.Range, C.Color.DarkOrange);

                    foreach (var m in EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, 3000).Where(x => x.IsHPBarRendered))
                    {
                        if (MNames.Contains(m.CharData.BaseSkinName) || DNames.Contains(m.BaseSkinName))
                        {
                            var Pos = m.HPBarPosition;
                            var Value = Damage(m) / m.MaxHealth;
                            var Is = m.CharData.BaseSkinName;

                            if (DNames.Contains(m.BaseSkinName))
                            {
                                var Width = 145;
                                Drawing.DrawLine(new Vector2(Pos.X - 6, Pos.Y + 9), new Vector2(Pos.X - 6 + Width * Value, Pos.Y + 9), 10, C.Color.DarkOrange);
                            }

                            if (Is == "SRU_RiftHerald")
                            {
                                var Width = 145;
                                Drawing.DrawLine(new Vector2(Pos.X - 6, Pos.Y + 6), new Vector2(Pos.X - 6 + Width * Value, Pos.Y + 6), 11, C.Color.DarkOrange);
                            }

                            if (Is == "SRU_Red" || Is == "SRU_Blue")
                            {
                                var Width = 145;
                                Drawing.DrawLine(new Vector2(Pos.X - 4, Pos.Y + 8), new Vector2(Pos.X - 4 + Width * Value, Pos.Y + 8), 11, C.Color.DarkOrange);
                            }

                            if (Is == "SRU_Baron")
                            {
                                var Width = 194;
                                Drawing.DrawLine(new Vector2(Pos.X - 30, Pos.Y + 8), new Vector2(Pos.X - 30 + Width * Value, Pos.Y + 8), 16, C.Color.DarkOrange);
                            }

                            if (Is == "SRU_Gromp")
                            {
                                var Width = 92;
                                Drawing.DrawLine(new Vector2(Pos.X + 21, Pos.Y + 7), new Vector2(Pos.X + 21 + Width * Value, Pos.Y + 7), 4, C.Color.DarkOrange);
                            }

                            if (Is == "SRU_Murkwolf")
                            {
                                var Width = 92;
                                Drawing.DrawLine(new Vector2(Pos.X + 21, Pos.Y + 7), new Vector2(Pos.X + 21 + Width * Value, Pos.Y + 7), 4, C.Color.DarkOrange);
                            }

                            if (Is == "Sru_Crab")
                            {
                                var Width = 61;
                                Drawing.DrawLine(new Vector2(Pos.X + 38, Pos.Y + 22), new Vector2(Pos.X + 38 + Width * Value, Pos.Y + 22), 4, C.Color.DarkOrange);
                            }

                            if (Is == "SRU_Razorbeak")
                            {
                                var Width = 92;
                                Drawing.DrawLine(new Vector2(Pos.X + 21, Pos.Y + 7), new Vector2(Pos.X + 21 + Width * Value, Pos.Y + 7), 4, C.Color.DarkOrange);
                            }

                            if (m.CharData.BaseSkinName == "SRU_Krug")
                            {
                                var Width = 92;
                                Drawing.DrawLine(new Vector2(Pos.X + 21, Pos.Y + 7), new Vector2(Pos.X + 21 + Width * Value, Pos.Y + 7), 4, C.Color.DarkOrange);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[DRAWING]: " + e.Message);
            }
        }
    }
}
