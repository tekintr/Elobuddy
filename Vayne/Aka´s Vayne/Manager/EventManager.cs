using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using System.Linq;
using Aka_s_Vayne.Features.Module;
using AkaCore.Features.Utility.Modules;

namespace Aka_s_Vayne.Manager
{
    class EventManager
    {

        public static void Load()
        {
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Game.OnUpdate += Game_OnUpdate;
            Logic.Mechanics.LoadFlash();
            Traps.Load();

            foreach (var module in Variables.moduleList)
            {
                module.OnLoad();
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {

            foreach (var module in Variables.moduleList.Where(module => module.GetModuleType() == ModuleType.OnUpdate
    && module.ShouldGetExecuted()))
            {
                module.OnExecute();
            }

            Logic.Mechanics.Insec();
            Logic.Mechanics.RotE();


            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) Features.Modes.Harass.HarassCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Features.Modes.JungleClear.Load();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) Features.Modes.Flee.Load();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Features.Modes.Combo.Load();
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            Features.Modes.LaneClear.SpellCast(sender, args);
        }
    }
}
