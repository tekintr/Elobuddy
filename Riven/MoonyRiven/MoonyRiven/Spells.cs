using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace MoonyRiven
{
    static class Spells
    {
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Active R1;
        public static Spell.Skillshot R2;
        public static Spell.Targeted Flash;
        public static Item Hydra, Tiamat;

        public static void Init()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 275, SkillShotType.Circular, 250, 2200, 100);
            W = new Spell.Active(SpellSlot.W, 250);
            E = new Spell.Skillshot(SpellSlot.E, 310, SkillShotType.Linear);
            R1 = new Spell.Active(SpellSlot.R);
            R2 = new Spell.Skillshot(SpellSlot.R, 800, SkillShotType.Cone, 250, 1600, 125);
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name == "SummonerFlash")
            {
                Flash = new Spell.Targeted(SpellSlot.Summoner1, 425);
            }
            else if (Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name == "SummonerFlash")
            {
                Flash = new Spell.Targeted(SpellSlot.Summoner2, 425);
            }

            Hydra = new Item((int)ItemId.Ravenous_Hydra, 350);
            Tiamat = new Item((int)ItemId.Tiamat, 350);

            Game.OnUpdate += GameOnOnUpdate;
            AIHeroClient.OnProcessSpellCast += AiHeroClientOnOnSpellCast;
        }

        private static void AiHeroClientOnOnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
                return;

            if (args.SData.Name.Contains("ItemTiamatCleave")) forceItem = false;
            if (args.SData.Name.Contains("RivenTriCleave")) forceQ = false;
            if (args.SData.Name.Contains("RivenMartyr")) forceW = false;
            if (args.SData.Name == "RivenFengShuiEngine") forceR1 = false;
            if (args.SData.Name == "RivenIzunaBlade") forceR2 = false;
            if (args.SData.Name.ToLower().Contains("RivenIzunaBlade")) forceFlash = false;
        }

        private static void GameOnOnUpdate(EventArgs args)
        {
            if (forceQ)
            {
                Q.Cast(QTo?.To3D() ?? Player.Instance.Position);
            }

            if (forceW)
            {
                W.Cast();
            }

            if (forceE)
                E.Cast(ETo.To3D());

            if (forceR1 && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Name == "RivenFengShuiEngine")
                R1.Cast();

            if (forceR2 && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Name == "RivenIzunaBlade")
            {
                R2.Cast(R2To.To3D());
            }

            if (forceItem && RivenMenu.Combo["H"].Cast<CheckBox>().CurrentValue)
            {
                if (forceItem && Hydra.IsOwned() && Hydra.IsReady()) Hydra.Cast();
                if (forceItem && Tiamat.IsOwned() && Tiamat.IsReady()) Tiamat.Cast();
            }

            if (forceFlash)
            {
                Flash.Cast(FlashTo.To3D());
            }
        }

        private static bool forceQ, forceW, forceE, forceR1, forceR2, forceItem, forceFlash;
        private static Vector2? QTo;
        static Vector2 ETo, R2To, FlashTo;
        public static void ForceQ() => forceQ = true;

        public static void ForceQ(Vector2 to)
        {
            QTo = to;
            forceQ = true;
            Core.DelayAction(() => forceQ = false, 2000);
        }

        public static void ForceW()
        {
            forceW = true;
            Core.DelayAction(() => forceW = false, 2000);
        }

        public static void ForceE(Vector2 to)
        {
            ETo = to.Distance(Player.Instance.Position) > E.Range ? Player.Instance.Position.Extend(to, E.Range) : to;
            forceE = true;
            Core.DelayAction(() => forceE = false, 750);
        }

        public static void ForceR1()
        {
            forceR1 = true;
            Core.DelayAction(() => forceR1 = false, 750);
        }

        public static void ForceR2(Vector2 to)
        {
            R2To = to;
            forceR2 = true;
            Core.DelayAction(() => forceR2 = false, 750);
        }

        public static void ForceItem()
        {
            forceItem = true;
            Core.DelayAction(() => forceItem = false, 2000);
        }

        public static void ForceAA()
        {
            Orbwalker.ResetAutoAttack();
        }

        public static void ForceFlash(Vector2 to)
        {
            FlashTo = to.Distance(Player.Instance.Position) > Flash.Range ? Player.Instance.Position.Extend(to, Flash.Range) : to;
            forceFlash = true;
            Core.DelayAction(() => forceFlash = false, 750);
        }
    }
}
