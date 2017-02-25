using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System.Collections.Generic;
using System.Linq;

namespace ReGaren
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Targeted R { get; private set; }
        public static Spell.Targeted Ignite { get; private set; }
        public static bool PlayerHasIgnite = true;

        public static List<Spell.SpellBase> AllSpells { get; private set; }
        public static Dictionary<SpellSlot, Color> ColorTranslation { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 300);
            W = new Spell.Active(SpellSlot.W, 20);
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Targeted(SpellSlot.R, 400);
            Ignite = new Spell.Targeted(Player.Instance.FindSummonerSpellSlotFromName("ignite"), 550);
            if (Player.Instance.FindSummonerSpellSlotFromName("ignite") == SpellSlot.Unknown)
                PlayerHasIgnite = false;

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
    }
}