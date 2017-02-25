using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK;
using EloBuddy;
using SharpDX;
using Aka_s_Vayne.Logic;

namespace Aka_s_Vayne.Features.Module.Tumble
{
    class AutoQR : IModule
    {
        public void OnLoad()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.Slot == SpellSlot.R && ShouldGetExecuted())
            {
                var qCastPosition = Variables._Player.ServerPosition.Extend(Game.CursorPos, 300f).To3D();
                if (Logic.Tumble.IsSafeToQ(qCastPosition))
                {
                    Core.DelayAction(() =>
                    { Manager.SpellManager.Q2.Cast(qCastPosition); }, 250);
                }
            }
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public bool ShouldGetExecuted()
        {
            return Manager.SpellManager.Q.IsReady() && Manager.MenuManager.UseRQ;
        }

        public void OnExecute()
        {
        }
    }
}
