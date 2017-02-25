using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Rendering;
using SharpDX;


namespace Looksharp.Champions
{
    public abstract class Base
    {
        protected static AIHeroClient hero = Player.Instance;
        protected static Menu PluginMenu, ModeMenu, MiscMenu, DrawMenu;
        protected static Spell.SpellBase Q, W, E, Q2, W2, E2, R;

        protected static float[] CD = new float[6], CDtemp = new float[6]; //Q melee to E ranged
        protected static bool isMelee { get { return !hero.HasBuff("jaycestancegun"); } }

        protected Base()
        {
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }

        protected virtual void OnUpdate(EventArgs args)
        {
            if (hero.IsDead || Shop.IsOpen) return;
            Update();
            Killsteal();
            var activemode = Orbwalker.ActiveModesFlags;
            if (activemode.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
            if (activemode.HasFlag(Orbwalker.ActiveModes.Harass)) Harass();
            if (activemode.HasFlag(Orbwalker.ActiveModes.LaneClear)) LaneClear();
            if (activemode.HasFlag(Orbwalker.ActiveModes.LastHit)) LastHit();
            if (activemode.HasFlag(Orbwalker.ActiveModes.JungleClear)) JungleClear();
            if (activemode.HasFlag(Orbwalker.ActiveModes.Flee)) Flee();
        }

        protected virtual void OnDraw(EventArgs args)
        {
            if (hero.IsDead) return;
            Draw();
        }

        public virtual void Update() { }
        public virtual void Draw() { }
        public virtual void Combo() { }
        public virtual void Harass() { }
        public virtual void LaneClear() { }
        public virtual void JungleClear() { }
        public virtual void LastHit() { }
        public virtual void Flee() { }
        public virtual void Killsteal() { }


        // ##############################################################
        // ##                    Helper Methods                        ##
        // ##############################################################

        protected static void CreateMenu()
        {
            PluginMenu = MainMenu.AddMenu(hero.ChampionName, hero.ChampionName);
            PluginMenu.AddGroupLabel("Information");
            PluginMenu.AddLabel("Made by Lookaside");
        }
        
        protected static void UpdateCooldowns()
        {
            if (isMelee)
            {
                CDtemp[0] = hero.Spellbook.GetSpell(SpellSlot.Q).CooldownExpires;
                CDtemp[1] = hero.Spellbook.GetSpell(SpellSlot.W).CooldownExpires;
                CDtemp[2] = hero.Spellbook.GetSpell(SpellSlot.E).CooldownExpires;
            }
            else
            {
                CDtemp[3] = hero.Spellbook.GetSpell(SpellSlot.Q).CooldownExpires;
                CDtemp[4] = hero.Spellbook.GetSpell(SpellSlot.W).CooldownExpires;
                CDtemp[5] = hero.Spellbook.GetSpell(SpellSlot.E).CooldownExpires;
            }
            for (int i = 0; i < 6; ++i)
            {
                CD[i] = CDtemp[i] - Game.Time < 0 ? 0 : CDtemp[i] - Game.Time;
            }
        }

        protected static void DrawCooldowns()
        {
            Vector2 wts = Drawing.WorldToScreen(hero.Position);
            wts[0] -= 40;
            wts[1] += 20;
            if (!isMelee)
                for (int i = 0; i < 3; ++i)
                    if (CD[i] == 0)
                        Drawing.DrawText(wts[0] + (i * 30), wts[1], System.Drawing.Color.Lime, "UP");
                    else
                        Drawing.DrawText(wts[0] + (i * 30), wts[1], System.Drawing.Color.Orange, CD[i].ToString("0.0"));
            else
                for (int i = 3; i < 6; ++i)
                    if (CD[i] == 0)
                        Drawing.DrawText(wts[0] + ((i - 3) * 30), wts[1], System.Drawing.Color.Lime, "UP");
                    else
                        Drawing.DrawText(wts[0] + ((i - 3) * 30), wts[1], System.Drawing.Color.Orange, CD[i].ToString("0.0"));
        }

        protected static void DrawInsec(AIHeroClient target)
        {
            if (IsValidTarget(target))
            {
                Vector3 insecPos = Helper.extend(target.Position, Game.CursorPos, 150, -1);
                Vector2 wtsx = Drawing.WorldToScreen(Game.CursorPos);
                Vector2 wts = Drawing.WorldToScreen(target.Position);
                Drawing.DrawLine(wts[0], wts[1], wtsx[0], wtsx[1], 2, System.Drawing.Color.Red);
                Circle.Draw(Color.Red, 110, insecPos);
            }
        }

        protected static bool ShouldInsec(Vector3 target, Vector3 insecPos, float range)
        {
            float tolerance = 0.3f;

            // ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) above/below point
            // a = target.position
            // b = line
            // c = myHero.position
            
            Vector3 lineA = Helper.RotateAroundPoint(insecPos, target, tolerance);
            Vector3 lineB = Helper.RotateAroundPoint(insecPos, target, -tolerance);
            float checkA = (lineA.X - target.X) * (hero.Position.Y - target.Y) - (lineA.Y - target.Y) * (hero.Position.X - target.X);
            float checkB = (lineB.X - target.X) * (hero.Position.Y - target.Y) - (lineB.Y - target.Y) * (hero.Position.X - target.X);

            return (checkA < 0 && checkB > 0 && hero.Distance(target) < range && hero.Distance(target) > 30);
        }

        protected static bool IsKillable(AIHeroClient target)
        {
            return  IsValidTarget(target) &&
                !target.HasBuffOfType(BuffType.Invulnerability) && !target.HasBuffOfType(BuffType.PhysicalImmunity) &&
                !target.Buffs.Any(b => b.Name.ToLower().Contains("fioraw")) &&
                !target.HasBuff("kindredrnodeathbuff") &&
                !target.HasBuff("JudicatorIntervention") &&
                !target.HasBuff("ChronoShift") &&
                !target.HasBuff("UndyingRage") &&
                !target.HasBuff("bansheesveil");
        }

        protected static bool IsValidTarget(AIHeroClient target)
        {
            return target.IsValid && !target.IsDead && !target.IsInvulnerable && !target.IsZombie && !target.IsPhysicalImmune;
        }


    }
}
