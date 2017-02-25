using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using Color = System.Drawing.Color;

namespace ReKatarina.Utility
{
    public static class Indicator
    {
        private static void DrawIndicator(Obj_AI_Base target)
        {
            var damage = Damage.GetTotalDamage(target);
            if (damage <= 0)
                return;

            var barPos = target.HPBarPosition;

            var percentHealthAfterDamage = Math.Max(0, target.TotalShieldHealth() - damage) /
                                           (target.MaxHealth + target.AllShield + target.AttackShield + target.MagicShield);
            var currentHealthPercentage = target.TotalShieldHealth() /
                                          (target.MaxHealth + target.AllShield + target.AttackShield + target.MagicShield);

            var startX = barPos.X + 2 + percentHealthAfterDamage * 104;
            var endX = barPos.X + 2 + currentHealthPercentage * 104;
            var y = barPos.Y + 10;

            Drawing.DrawLine((float)startX, y, (float)endX, y, 10, Color.OrangeRed);
        }
        public static void Execute()
        {
            foreach (var target in EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget() && u.IsHPBarRendered))
            {
                DrawIndicator(target);
            }
        }
    }
}
