using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System.Collections.Generic;

namespace ReKatarina
{
    public static class SpellManager
    {
        public static Spell.Targeted Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Active R { get; private set; }
        public static bool CastingUlt = false;
        public static int LastUltCast = 0;
        public static int LastJumpCast = 0;

        public static List<Spell.SpellBase> AllSpells { get; private set; }
        public static Dictionary<SpellSlot, Color> ColorTranslation { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Active(SpellSlot.W, 375);
            E = new Spell.Targeted(SpellSlot.E, 725);
            R = new Spell.Active(SpellSlot.R, 550);

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

        public static void CastE(this Spell.SpellBase spell, Obj_AI_Base target)
        {
            E.CastE(target.Position);
        }

        public static void CastE(this Spell.SpellBase spell, Vector3 position)
        {
            if (spell.Slot != SpellSlot.E) return;
            if (Player.Instance.HealthPercent <= ConfigList.Combo.ComboSaver && !Q.IsReady() && !W.IsReady()) return;
            if (position.IsUnderEnemyTurret() && ConfigList.Combo.GoUnderTower && Player.Instance.HealthPercent >= ConfigList.Combo.MinHPToGoUnderTower) E.Cast(position);
            else if (!position.IsUnderEnemyTurret()) E.Cast(position);
        }
    }
}