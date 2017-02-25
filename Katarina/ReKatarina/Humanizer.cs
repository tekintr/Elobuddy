using EloBuddy;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Based on 
 * https://github.com/NentoR/EloBuddy-Humanizers/blob/master/LittleHumanizer/Program.cs
 * */

namespace ReKatarina
{
    class Humanizer
    {
        public static int BlockedCount = 0;
        public static LastSpellCast LastSpell = new LastSpellCast();
        public static List<LastSpellCast> LastSpellsCast = new List<LastSpellCast>();

        public static void Initialize()
        {
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!ConfigList.Misc.GetHumanizerStatus) return;
            if (!sender.Owner.IsMe) return;
            if (!(new SpellSlot[] {SpellSlot.Q,SpellSlot.W,SpellSlot.E,SpellSlot.R,SpellSlot.Summoner1,SpellSlot.Summoner2
                ,SpellSlot.Item1,SpellSlot.Item2,SpellSlot.Item3,SpellSlot.Item4,SpellSlot.Item5,SpellSlot.Item6,SpellSlot.Trinket})
                .Contains(args.Slot)) return;
            if (Environment.TickCount - LastSpell.CastTick < GetDelay())
            {
                args.Process = false;
                BlockedCount += 1;
            }
            else
            {
                LastSpell = new LastSpellCast() { Slot = args.Slot, CastTick = Environment.TickCount };
            }
            if (LastSpellsCast.Any(x => x.Slot == args.Slot))
            {
                LastSpellCast spell = LastSpellsCast.FirstOrDefault(x => x.Slot == args.Slot);
                if (spell != null)
                {
                    if (Environment.TickCount - spell.CastTick <= 250 + Game.Ping / 2)
                    {
                        args.Process = false;
                        BlockedCount += 1;
                    }
                    else
                    {
                        LastSpellsCast.RemoveAll(x => x.Slot == args.Slot);
                        LastSpellsCast.Add(new LastSpellCast() { Slot = args.Slot, CastTick = Environment.TickCount });
                    }
                }
                else
                {
                    LastSpellsCast.Add(new LastSpellCast() { Slot = args.Slot, CastTick = Environment.TickCount });
                }
            }
            else
            {
                LastSpellsCast.Add(new LastSpellCast() { Slot = args.Slot, CastTick = Environment.TickCount });
            }
        }

        public class LastSpellCast
        {
            public int CastTick = 0;
            public SpellSlot Slot = SpellSlot.Unknown;
        }

        // Other
        public static int GetDelay()
        {
            return ConfigList.Misc.GetSpellDelay + Damage.GetAditionalDelay();
        }
        
    }
}
