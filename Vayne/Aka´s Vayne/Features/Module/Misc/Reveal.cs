using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK;
using EloBuddy;

namespace Aka_s_Vayne.Features.Module.Misc
{
    class Reveal : IModule
    {
        private static readonly Item Trinket = new Item(3364, 600f);
        private static int lastWarded;

        public void OnLoad()
        {
            foreach (var heros in EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.Distance(Player.Instance) < 1000))
            {
                var path = heros.Path.LastOrDefault();

                if (NavMesh.IsWallOfGrass(path, 1))
                {
                    if (heros.Distance(path) > 200) return;
                    if (NavMesh.IsWallOfGrass(Player.Instance.Position, 1) && Player.Instance.Distance(path) < 200) return;

                    if (Player.Instance.Distance(path) < 500)
                    {
                        foreach (var obj in ObjectManager.Get<AIHeroClient>().Where(x => x.Name.ToLower().Contains("ward") && x.IsAlly && x.Distance(path) < 300))
                        {
                            if (NavMesh.IsWallOfGrass(obj.Position, 1)) return;
                        }

                        if (Trinket != null && Environment.TickCount - lastWarded > 1000)
                        {
                            Trinket.Cast(path);
                            lastWarded = Environment.TickCount;
                        }
                    }
                }
            }
        }
    
        public ModuleType GetModuleType()
        {
            return ModuleType.Other;
        }

        public bool ShouldGetExecuted()
        {
            return true;
        }

        public void OnExecute() { }
    }
}
