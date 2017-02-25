/*
 * By MarioGK
 * From Mario's Lib
 * https://github.com/mariogk/ItsMeMario/tree/master/Mario%60s%20Lib/Mario%60s%20Lib
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using ReKatarina.ReCore.Databases;
using SharpDX;
using ReKatarina.ReCore.ConfigList;
using ReKatarina.ReCore.Utility;

namespace ReKatarina.ReCore.Managers
{
    public class TargetSpell
    {
        public Obj_AI_Base Caster;
        public Champion Champ;
        public SpellSlot Slot;
        public Obj_AI_Base Target;

        public TargetSpell(Obj_AI_Base target, Obj_AI_Base caster, Champion champ, SpellSlot slot)
        {
            Target = target;
            Caster = caster;
            Champ = champ;
            Slot = slot;
        }
    }

    public class TurretAA
    {
        public Obj_AI_Base Target;

        public TurretAA(Obj_AI_Base target)
        {
            Target = target;
        }
    }

    public class NotMissile
    {
        public Obj_AI_Base Caster;
        public Champion Champ;
        public Vector3 End;
        public SpellSlot Slot;
        public string SName;
        public Vector3 Start;

        public NotMissile(Vector3 start, Vector3 end, Obj_AI_Base caster, Champion champ, SpellSlot slot, string sname)
        {
            Start = start;
            End = end;
            Caster = caster;
            Champ = champ;
            Slot = slot;
            SName = sname;
        }
    }

    public static class DangerManager
    {
        public static List<MissileClient> Missiles = new List<MissileClient>();
        public static List<TargetSpell> TargettedSpells = new List<TargetSpell>();
        public static List<TurretAA> TurretAAs = new List<TurretAA>();

        public static void Initialize()
        {
            //Missiles
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            //Targetted Spells
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            //Turrets AAs
            Obj_AI_Base.OnBasicAttack += Obj_AI_Base_OnBasicAttack;
        }

        public static bool IsNotNull(this Obj_AI_Base target)
        {
            return target != null;
        }

        private static void Obj_AI_Base_OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var tower = sender as Obj_AI_Turret;
            var target = args.Target as Obj_AI_Base;
            if (tower != null && target != null && target.IsAlly)
            {
                var a = new TurretAA(target);
                TurretAAs.Add(a);
                EloBuddy.SDK.Core.DelayAction(() => TurretAAs.Remove(a), 100);
            }
        }

        public static bool ReceivingTurretAttack(this Obj_AI_Base target)
        {
            var aa = TurretAAs.FirstOrDefault(t => t.Target == target);
            return aa != null;
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            var missile = sender as MissileClient;
            if (missile != null && missile.IsEnemy && missile.SpellCaster.IsEnemy)
            {
                var champion = missile.SpellCaster as AIHeroClient;
                if (champion.IsNotNull())
                {
                    Missiles.Add(missile);
                }
            }
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            var missile = sender as MissileClient;
            if (missile != null && missile.IsEnemy && missile.SpellCaster.IsEnemy)
            {
                var champion = missile.SpellCaster as AIHeroClient;
                if (champion.IsNotNull())
                {
                    Missiles.Remove(missile);
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var target = args.Target as Obj_AI_Base;
            var senderHero = sender as AIHeroClient;
            if (args.IsAutoAttack()) return;
            if (senderHero != null && senderHero.IsEnemy)
            {
                if (target != null && target.IsAlly)
                {
                    var targettedSpell = new TargetSpell(target, senderHero, senderHero.Hero, args.Slot);
                    TargettedSpells.Add(targettedSpell);
                    EloBuddy.SDK.Core.DelayAction(() => TargettedSpells.Remove(targettedSpell), 80);
                }
            }
        }

        public static bool IsInDanger(this AIHeroClient target, int percent)
        {
            if (target == null || target.IsDead || !target.IsValid || target.IsInShopRange()) return false;

            var sliderPercent = 10;
            var boundingRadius = target.BoundingRadius + 110;

            if (TargettedSpells.Any())
            {
                var normalTargSpell = TargettedSpells.FirstOrDefault(t => t.Target == target);

                var dangTargSpell =
                    DangerousSpells.Spells.FirstOrDefault(
                        s => s.Hero == normalTargSpell?.Champ && s.Slot == normalTargSpell.Slot);

                if (dangTargSpell != null && target.HealthPercent <= percent + sliderPercent)
                {
                    return true;
                }
                if (normalTargSpell != null && target.HealthPercent <= percent)
                {
                    return true;
                }
            }

            //Missiles
            var missile = Missiles.FirstOrDefault(m => m.IsInRange(target, 2500) && m.IsValid);
            var champion = missile?.SpellCaster as AIHeroClient;

            if (champion != null)
            {
                var spell1 =
                    missile.SpellCaster.Spellbook.Spells.FirstOrDefault(
                        s => s.Name.ToLower().Equals(missile.SData.Name.ToLower()));

                var spell2 =
                    missile.SpellCaster.Spellbook.Spells.FirstOrDefault(
                        s => (s.Name.ToLower() + "missile").Equals(missile.SData.Name.ToLower()));

                var slot = SpellSlot.Unknown;
                if (spell1 != null)
                {
                    slot = spell1.Slot;
                }
                else if (spell2 != null)
                {
                    slot = spell2.Slot;
                }

                var projection = target.Position.To2D()
                    .ProjectOn(missile.StartPosition.To2D(), missile.EndPosition.To2D());

                if (projection.IsOnSegment &&
                    projection.SegmentPoint.Distance(target.Position) <= missile.SData.CastRadius + boundingRadius)
                {
                    var DangSpell =
                        DangerousSpells.Spells.FirstOrDefault(
                            ds =>
                                ds.Slot == slot && champion.Hero == ds.Hero &&
                                missile.Distance(target) <= boundingRadius + 250 &&
                                Summoners.Menu.GetCheckBoxValue("dangSpell" + ds.Hero.ToString() + ds.Slot.ToString()));

                    if (DangSpell != null && target.HealthPercent <= percent + sliderPercent)
                    {
                        return true;
                    }
                    return missile.Distance(target) <= boundingRadius && target.HealthPercent <= percent;
                }

                return missile.Distance(target) <= boundingRadius && target.HealthPercent <= percent;
            }
            return false;
        }
    }
}