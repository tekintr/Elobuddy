using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK;
using EloBuddy;

namespace Aka_s_Vayne.Features.Module.Misc
{
    class TowerQ : IModule
    {
        public void OnLoad()
        {
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
        }

        private void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (ShouldGetExecuted() && target is Obj_AI_Turret && Logic.Tumble.IsSafeEx(Game.CursorPos))
            {
                Manager.SpellManager.Q2.Cast(Game.CursorPos);
            }
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public bool ShouldGetExecuted()
        {
            return Manager.MenuManager.TowerQ && Manager.SpellManager.Q.IsReady();
        }

        public void OnExecute()
        { }
    }
}

