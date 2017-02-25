using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System.Collections.Generic;
using System.Linq;

namespace ReWarwick
{
    public static class SpellManager
    {
        public static Spell.Targeted Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        public static List<Spell.SpellBase> AllSpells { get; private set; }
        public static Dictionary<SpellSlot, Color> ColorTranslation { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 350, DamageType.Magical);
            W = new Spell.Active(SpellSlot.W, 4000);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Skillshot(SpellSlot.R, 110, SkillShotType.Linear, 0, 1600, 150, DamageType.Magical)
            {
                AllowedCollisionCount = -1
            };

            AllSpells = new List<Spell.SpellBase>(new Spell.SpellBase[] { Q, W, E, R });
            ColorTranslation = new Dictionary<SpellSlot, Color>
            {
                { SpellSlot.Q, Color.LimeGreen.ToArgb(150) },
                { SpellSlot.W, Color.CornflowerBlue.ToArgb(150) },
                { SpellSlot.E, Color.YellowGreen.ToArgb(150) },
                { SpellSlot.R, Color.OrangeRed.ToArgb(150) }
            };
        }

        public static void Initialize()
        {
        }

        public static Color ToArgb(this Color color, byte a) // by Hellsing 
        {
            return new ColorBGRA(color.R, color.G, color.B, a);
        }

        public static Color GetColor(this Spell.SpellBase spell) // by Hellsing 
        {
            return ColorTranslation.ContainsKey(spell.Slot) ? ColorTranslation[spell.Slot] : Color.Wheat;
        }

        public static void CastE2(this Spell.SpellBase spell)
        {
            if (spell.Slot != SpellSlot.E) return;

            if (Player.Instance.CountEnemyChampionsInRange(E.Range) > 0)
                spell.Cast();
        } 
    }
}