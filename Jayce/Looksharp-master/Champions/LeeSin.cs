using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Looksharp.Champions
{
    class LeeSin : Base
    {
        static LeeSin()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1800, 75) { AllowedCollisionCount = 0 };
            Q2 = new Spell.Active(SpellSlot.Q, 1300);
            W = new Spell.Targeted(SpellSlot.W, 700);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Targeted(SpellSlot.R, 375);

            CreateMenu();
            ModeMenu = PluginMenu.AddSubMenu("Modes", "Modes");
            ModeMenu.AddGroupLabel("Combo");
            ModeMenu.Add("nasus.combo.q", new CheckBox("Kullan Q"));
            ModeMenu.Add("nasus.combo.w", new CheckBox("Kullan W"));
            ModeMenu.Add("nasus.combo.e", new CheckBox("Kullan E"));
            ModeMenu.Add("nasus.combo.r", new CheckBox("Kullan R"));
        }

        public override void Update()
        {
            // update
        }

        public override void Draw()
        {
            // update
        }

        public override void Combo()
        {
            // combo
        }

        public override void Harass()
        {
            // update
        }

        public override void Flee()
        {
            // update
        }

        public override void Killsteal()
        {
            // update
        }
    }
}
