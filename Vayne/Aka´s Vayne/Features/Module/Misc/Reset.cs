using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace Aka_s_Vayne.Features.Module.Misc
{
    class Reset : IModule
    {
        public void OnLoad()
        {
            Obj_AI_Base.OnPlayAnimation += Obj_AI_Base_OnPlayAnimation;
        }

        private void Obj_AI_Base_OnPlayAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe) return;
            if (Manager.MenuManager.QReset)
            {
                switch (args.Animation)
                {
                    case "Spell1":
                        Orbwalker.ResetAutoAttack();
                        break;
                    case "Spell3":
                        Orbwalker.ResetAutoAttack();
                        break;
                    case "Spell4":
                        Orbwalker.ResetAutoAttack();
                        break;
                }
            }
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public bool ShouldGetExecuted()
        {
            return false;
        }

        public void OnExecute()
        {
        }
    }
}
