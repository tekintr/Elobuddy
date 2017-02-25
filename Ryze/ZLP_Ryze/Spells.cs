using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ZLP_Ryze
{
    public class Spells
    {
        public static Spell.Skillshot Q, R;
        public static Spell.Targeted W, E, Ignite;
        public static Item Zhonya, Seraph, Archangel, Tear;

        public static void Initialize()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 60);
            W = new Spell.Targeted(SpellSlot.W, 615);
            E = new Spell.Targeted(SpellSlot.E, 615);
            R = new Spell.Skillshot(SpellSlot.R, 1750, SkillShotType.Circular, 2250, int.MaxValue, 475);

            var slot = Player.Instance.GetSpellSlotFromName("summonerdot");
            if (slot != SpellSlot.Unknown)
                Ignite = new Spell.Targeted(slot, 600);

            Zhonya = new Item(ItemId.Zhonyas_Hourglass);
            Seraph = new Item(ItemId.Seraphs_Embrace);
            Archangel = new Item(ItemId.Archangels_Staff);
            Tear = new Item(ItemId.Tear_of_the_Goddess);
        }

        public static void Ultimate(EventArgs args)
        {
            if (R.Level == 2)
                R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Circular, 2250, int.MaxValue, 475);
        }
    }
}