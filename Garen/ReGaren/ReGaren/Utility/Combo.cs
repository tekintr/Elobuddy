using System;
using EloBuddy;
using EloBuddy.SDK;

namespace ReGaren.Utility
{
    public static class Combo
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Mixed, Player.Instance.Position);
            if (target == null)
                return;

            foreach (var spell in SpellManager.AllSpells)
            {
                switch (spell.Slot)
                {
                    case SpellSlot.Q:
                    {
                        if (!ConfigList.Combo.ComboQ)
                            continue;
                        
                        if (SpellManager.Q.IsReady() && !Player.HasBuff("GarenE"))
                        {
                            SpellManager.Q.Cast();
                            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), ConfigList.Misc.GetSpellDelay);
                        }
                        break;
                    }
                    case SpellSlot.W:
                    {
                        if (!ConfigList.Combo.ComboW)
                            continue;

                        if (SpellManager.W.IsReady())
                            Core.DelayAction(() => SpellManager.W.Cast(), ConfigList.Misc.GetSpellDelay - 25);
                        break;
                    }
                    case SpellSlot.E:
                    {
                        if (!ConfigList.Combo.ComboE || Player.HasBuff("GarenQ"))
                            continue;

                        if (SpellManager.E.IsReady() && !SpellManager.Q.IsReady())
                            Core.DelayAction(() => SpellManager.E.Cast(), ConfigList.Misc.GetSpellDelay - 50);
                        break;
                    }
                    case SpellSlot.R:
                    {
                        if (!ConfigList.Combo.ComboR)
                            continue;

                        if (Damage.GetRDamage(target) - 5 >= target.Health && !target.IsInvulnerable)
                        {
                            SpellManager.R.Cast(target);
                        }
                            
                        break;
                    }
                }
            }
        }
    }
}
