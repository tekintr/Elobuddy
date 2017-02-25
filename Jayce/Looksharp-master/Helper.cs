using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace Looksharp
{
    internal static class Helper
    {
        public static bool WillKill(Spell.SpellBase spell, Obj_AI_Base target, float multiplier = 1)
        {
            return Player.Instance.GetSpellDamage(target, spell.Slot) * multiplier >= spell.GetHealthPrediction(target);
        }

        public static Vector3 extend(Vector3 position, Vector3 target, float distance, int towards) // towards/away from target
        {
            return position + Vector3.Normalize(towards * (target - position)) * distance;
        }

        public static Vector3 RotateAroundPoint(Vector3 rotated, Vector3 around, float angle)
        {
            double sin = Math.Sin((double)angle);
            double cos = Math.Cos((double)angle);

            double x = ((rotated.X - around.X) * cos) - ((rotated.Y - around.Y) * sin) + around.X;
            double y = ((rotated.X - around.X) * sin) + ((rotated.Y - around.Y) * cos) + around.Y;

            return new Vector3((float)x, (float)y, rotated.Z);
        }

        public static bool HasSmite(AIHeroClient target)
        {
            return target.Spellbook.Spells.Any(spell => spell.Name.ToLower().Contains("smite"));
        }
    }
}
