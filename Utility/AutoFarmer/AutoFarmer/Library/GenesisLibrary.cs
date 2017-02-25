﻿namespace Genesis.Library
{
    using System;

    using Genesis.Library.Spells;

    using EloBuddy;

    internal class SpellLibrary
    {
        public static SpellBase GetSpells(Champion heroChampion)
        {
            Type championType = Type.GetType("Genesis.Library.Spells." + heroChampion);
            if (championType != null)
            {
                return Activator.CreateInstance(championType) as SpellBase;
            }

            else
            {
                Chat.Print(heroChampion + " desteklenmiyor.");
                //throw new NotImplementedException();
                return null;
            }
        }

        public static bool IsOnCooldown(AIHeroClient hero, SpellSlot slot)
        {
            if (!hero.Spellbook.GetSpell(slot).IsLearned)
            {
                return true;
            }

            float cooldown = hero.Spellbook.GetSpell(slot).CooldownExpires - Game.Time;
            return cooldown > 0;
        }

        public static void Initialize()
        {
        }
    }
}