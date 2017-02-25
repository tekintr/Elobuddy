using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace pEzreal.Extensions
{
    internal class Spells
    {
        public static readonly Item Youmuu = new Item(ItemId.Youmuus_Ghostblade, Config.MyHero.GetAutoAttackRange());
        public static readonly Item Botrk = new Item(ItemId.Blade_of_the_Ruined_King, 550);
        public static readonly Item Cutlass = new Item(ItemId.Bilgewater_Cutlass, 550);

        public static readonly Item QSS = new Item(ItemId.Quicksilver_Sash);
        public static readonly Item Mercurial = new Item(ItemId.Mercurial_Scimitar);

        public static readonly Item TearOfTheGoddess = new Item(ItemId.Tear_of_the_Goddess);
        public static readonly Item Manamune = new Item(ItemId.Manamune);

        public static void Initialize()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1050, SkillShotType.Linear, 250, 2000, 60)
            {AllowedCollisionCount = 0};
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Linear, 250, 1600, 80)
            {AllowedCollisionCount = int.MaxValue};
            E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Circular, 250, 2000, 60);
            R = new Spell.Skillshot(SpellSlot.R, 2500, SkillShotType.Linear, 1000, 2000, 160)
            {AllowedCollisionCount = int.MaxValue};
        }

        public static void CastQ(Obj_AI_Base target)
        {
            if (target == null || target.IsInvulnerable || !target.IsValidTarget() || target.IsDead) return;

            if (Q.IsReady()) Q.CastMinimumHitchance(target, HitChanceChooser());
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (target == null || target.IsInvulnerable || !target.IsValidTarget() || target.IsDead) return;

            if (W.IsReady()) W.CastMinimumHitchance(target, HitChanceChooser());
        }

        public static void R_CastIfWillHit(int enemies)
        {
            foreach (var hero in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(R.Range)))
            {
                if (!R.IsReady()) continue;
                var hits = new List<AIHeroClient>();
                var startPos = Config.MyHero.Position.To2D();
                var endPos = hero.Position.To2D();

                hits.Clear();
                var hero1 = hero;
                foreach (
                    var iHero in
                        EntityManager.Heroes.Enemies.Where(
                            iHero =>
                                !iHero.IsDead && iHero.IsValidTarget(R.Range) && iHero.IsInRange(hero1, R.Range)))
                {
                    if (Prediction.Position.Collision.LinearMissileCollision(iHero, startPos, endPos, R.Speed,
                        R.Width, R.CastDelay))
                    {
                        hits.Add(iHero);
                    }

                    if (hits.Count >= enemies)
                    {
                        R.CastMinimumHitchance(hero, HitChanceChooser());
                    }
                }
            }
        }

        public static HitChance HitChanceChooser()
        {
            switch (Config.HitchanceChosen)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                default:
                    return HitChance.High;
            }
        }

        public static Spell.Skillshot Q, W, E, R;
    }
}