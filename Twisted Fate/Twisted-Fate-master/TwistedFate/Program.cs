using System;

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using EloBuddy.SDK.Rendering;
using System.Collections.Generic;
using EloBuddy.SDK.Enumerations;
using System.Linq;
using System.Threading.Tasks;

namespace TwistedFate
{
    internal class Program
    {
        private static Spell.Skillshot Q { get; set; }
        private static int LastPingTick = 0;
        private static int PingCount = 0;
        private static int PingDelay = 30000;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.TwistedFate)
                return;

            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
            Drawing.OnEndScene += OnEndScene;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;

            Q = new Spell.Skillshot(SpellSlot.Q, 1450, SkillShotType.Linear, 0, 1000, 40) { AllowedCollisionCount = int.MaxValue };

            Menus.CreateMenu();
        }

        // Credits to ?? for the math - Not sure who the original person was
        private static void DrawCricleMinimap(System.Drawing.Color color, float radius, Vector3 center, int thickness = 5, int quality = 30)
        {
            var pointList = new List<Vector3>();
            for (var i = 0; i < quality; i++)
            {
                var angle = i * Math.PI * 2 / quality;
                pointList.Add(new Vector3(center.X + radius * (float)Math.Cos(angle), center.Y + radius * (float)Math.Sin(angle), center.Z));
            }

            for (var i = 0; i < pointList.Count; i++)
            {
                var a = pointList[i];
                var b = pointList[i == pointList.Count - 1 ? 0 : i + 1];

                var aonScreen = Drawing.WorldToMinimap(a);
                var bonScreen = Drawing.WorldToMinimap(b);

                //Fixed drawing bug
                if (aonScreen.X == 0 || bonScreen.X == 0)
                    continue;

                Drawing.DrawLine(aonScreen.X, aonScreen.Y, bonScreen.X, bonScreen.Y, thickness, color);
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
                return;

            if (args.SData.Name.Equals("Gate", StringComparison.InvariantCultureIgnoreCase) && Menus.CardMenu["combo.goldAfterUlt"].Cast<CheckBox>().CurrentValue)
                CardSelector.StartSelecting(Cards.Gold);
        }

        private static void OnEndScene(EventArgs args)
        {
            if (Menus.DrawingsMenu["drawing.r_Range"].Cast<CheckBox>().CurrentValue)
            {
                DrawCricleMinimap(System.Drawing.Color.White, 5500, Player.Instance.ServerPosition, 2, 100);
            }

            foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => x.IsHPBarRendered))
            {

                float hp = 0.0f;
                Vector2 hpPos = enemy.HPBarPosition;
                Vector2 startPos = new Vector2(0, 0);
                Vector2 endPos = new Vector2(0, 0);
                System.Drawing.Color color = System.Drawing.Color.Lime;

                float dmgAfterCombo = GetComboDamage(enemy);

                if (enemy.TotalShieldHealth() - dmgAfterCombo > 0.0f)
                    hp = enemy.TotalShieldHealth() - dmgAfterCombo;
                else
                    hp = 0.0f;

                hp = hp / (enemy.MaxHealth + enemy.AllShield + enemy.AttackShield + enemy.MagicShield);

                if (enemy.ChampionName == "Annie")
                {
                    startPos = new Vector2(enemy.HPBarPosition.X + hp * 107, enemy.HPBarPosition.Y + -7.0f);
                    endPos = new Vector2(enemy.HPBarPosition.X + hp * 107, enemy.HPBarPosition.Y + 2.0f);
                }
                else
                {
                    startPos = new Vector2(enemy.HPBarPosition.X + hp * 107, enemy.HPBarPosition.Y + 4.0f);
                    endPos = new Vector2(enemy.HPBarPosition.X + hp * 107, enemy.HPBarPosition.Y + 13.0f);
                }

                if (enemy.TotalShieldHealth() - dmgAfterCombo <= 0.0f)
                    color = System.Drawing.Color.Red;

                Drawing.DrawLine(startPos, endPos, 2, color);
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (Menus.DrawingsMenu["drawing.q_Range"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(Color.Yellow, 1450, ObjectManager.Player.Position);

            if (Menus.DrawingsMenu["drawing.r_Range"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(Color.White, 5500, ObjectManager.Player.Position);
        }


        private static void OnUpdate(EventArgs args)
        {
            SelectCard();
            PingIfKillable();

            if (Menus.MiscMenu["misc.QImmobileChamps"].Cast<CheckBox>().CurrentValue)
                QImmobile();
        }

        private static void PingIfKillable()
        {
            if (Menus.MiscMenu["misc.PingOnKillable"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(x =>
                    ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready 
                    && x.IsValidTarget() 
                    && GetComboDamage(x) > x.Health))
                {
                    Ping(enemy.Position);
                }
            }
        }

        private static void Ping(Vector3 pos)
        {
            if (Environment.TickCount - LastPingTick < PingDelay)
                return;

            if(PingCount >= 5)
            {
                PingDelay = 30000;
                PingCount = 0;
                return;
            }

            LastPingTick = Environment.TickCount;

            PingCount += 1;
            PingDelay = 150;

            TacticalMap.ShowPing(PingCategory.Fallback, pos, true);
        }

        private static float GetComboDamage(Obj_AI_Base target)
        {
            float totalDmg = 0;

            if (ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.Q) == SpellState.Ready)
                totalDmg += ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q);

            if (ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.W) == SpellState.Ready)
                totalDmg += ObjectManager.Player.GetSpellDamage(target, SpellSlot.W);

            totalDmg += ObjectManager.Player.GetAutoAttackDamage(target) * 2;

            if (ObjectManager.Player.GetSpellSlotFromName("SummonerIgnite") != SpellSlot.Unknown)
                if(ObjectManager.Player.Spellbook.CanUseSpell(ObjectManager.Player.GetSpellSlotFromName("SummonerIgnite")) == SpellState.Ready)
                    totalDmg += ObjectManager.Player.CalculateDamageOnUnit(target, DamageType.True, 50 + 20 * Player.Instance.Level - (target.HPRegenRate / 5 * 3));

            return totalDmg;
        }

        private static void QImmobile()
        {
            if (Player.Instance.IsDead)
                return;

            if (!ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).IsReady)
                return;

            var target = TargetSelector.GetTarget(1500, DamageType.Magical);

            if (target == null)
                return;

            if (!Q.IsInRange(target))
                return;

            if (!target.CanMove || target.IsRecalling())
            {
                Q.Cast(target);
            }                
        }

        private static void SelectCard()
        {
            if (Menus.SelectGoldCard())
                CardSelector.StartSelecting(Cards.Gold);
            else if (Menus.SelectRedCard())
                CardSelector.StartSelecting(Cards.Red);
            else if (Menus.SelectBlueCard())
                CardSelector.StartSelecting(Cards.Blue);
        }
    }
}
