using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReKatarina.Utility
{
    class JungleClear
    {
        private static bool IsWallBetweenPlayer(Vector2 p)
        {
            AIHeroClient player = Player.Instance;
            var v1 = p - player.Position.To2D();
            for (float i = 0; i <= 1; i += 0.1f)
            {
                var v2 = player.Position.To2D() + i * v1;
                if (v2.IsWall()) return true;
            }

            return false;
        }

        public static void Execute()
        {
            var monsters = EntityManager.MinionsAndMonsters.
                GetJungleMonsters(Player.Instance.Position, SpellManager.Q.Range).
                OrderBy(h => h.Health);
            {
                if (monsters == null || !monsters.Any()) return;
                if (ConfigList.Farm.FarmQ && SpellManager.Q.IsReady())
                {
                    if (!ConfigList.Farm.FarmQIgnore && monsters.Count() < ConfigList.Farm.FarmQCount) return;
                    SpellManager.Q.Cast(monsters.Last());
                }

                if (ConfigList.Farm.FarmW && SpellManager.W.IsReady())
                {
                    if (monsters.Any(h => h.IsInRange(h, SpellManager.W.Range)))
                        SpellManager.W.Cast();
                }

                if (ConfigList.Farm.FarmE && SpellManager.E.IsReady())
                {
                    var d = Dagger.GetClosestDagger();
                    if (d.IsInRange(Player.Instance.Position, SpellManager.E.Range))
                        if (!IsWallBetweenPlayer(d.To2D()) && monsters.Last().IsInRange(d, SpellManager.W.Range + 75))
                            SpellManager.E.Cast(Damage.GetBestDaggerPoint(d, monsters.Last()));
                }
            }
        }
    }
}
