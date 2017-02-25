using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using SharpDX;
using Color = System.Drawing.Color;

namespace MoonyRiven
{
    class Riven
    {
        private AIHeroClient me;
        public Riven()
        {
            me = ObjectManager.Player;
            if (me.ChampionName != "Riven")
                return;

            RivenMenu.Init();
            Spells.Init();

            Obj_AI_Base.OnSpellCast += OnSpellCast;
            Dash.OnDash += DashOnOnDash;
            Obj_AI_Base.OnPlayAnimation += ObjAiBaseOnOnPlayAnimation;
            Game.OnUpdate += GameOnOnUpdate;
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += DrawingOnOnDraw;

            Interrupter.OnInterruptableSpell += InterrupterOnOnInterruptableSpell;
            Gapcloser.OnGapcloser += GapcloserOnOnGapcloser;

            if (Game.MapId != GameMapId.SummonersRift)
            {
                WallJumpData.WallJumpSpots.Clear();
                if (Game.MapId == GameMapId.HowlingAbyss)
                {
                    foreach (var aramSpot in WallJumpData.WallJumpSpotsAram)
                    {
                        WallJumpData.WallJumpSpots.Add(aramSpot.Key, aramSpot.Value);
                    }
                }

                if (Game.MapId == GameMapId.TwistedTreeline)
                {
                    foreach (var twistedTreeLineSpot in WallJumpData.WallJumpSpotsTwistedTreeline)
                    {
                        WallJumpData.WallJumpSpots.Add(twistedTreeLineSpot.Key, twistedTreeLineSpot.Value);
                    }
                }
            }
            Logger.Debug("====================================================");
        }

        private void GapcloserOnOnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloserEventArgs)
        {
            if (!sender.IsEnemy || !RivenMenu.Misc["antiGap"].Cast<CheckBox>().CurrentValue)
                return;

            var path = me.RealPath();
            var gapTime = gapcloserEventArgs.TickCount - Environment.TickCount;
            var pathPoint = path.OrderBy(p => Math.Abs(p.Distance(me)/me.MoveSpeed*1000 - gapTime)).First();
            if (gapcloserEventArgs.End.Distance(pathPoint) <= Spells.W.Range && Spells.W.IsReady())
            {
                Spells.ForceW();
            }
            //else if (Spells.W.IsReady() && Spells.E.IsReady())
            //{
            //    var ePos = me.Position.Extend(path.Last(), Spells.E.Range);
            //    pathPoint = path.OrderBy(p => Math.Abs(p.Distance(ePos) / me.MoveSpeed * 1000 + Spells.E.CastDelay - gapTime)).First();
            //    if (gapcloserEventArgs.End.Distance(pathPoint) <= Spells.W.Range)
            //    {
            //        Spells.ForceE(path.Last().To2D());
            //        Core.DelayAction(Spells.ForceW, Spells.E.CastDelay);
            //    }
            //}
        }

        private void InterrupterOnOnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs interruptableSpellEventArgs)
        {
            if (!sender.IsEnemy || !RivenMenu.Misc["interrupt"].Cast<CheckBox>().CurrentValue)
                return;

            if (Prediction.Position.PredictUnitPosition(sender, Spells.W.CastDelay).Distance(me) <= Spells.W.Range && Spells.W.IsReady())
                Spells.ForceW();
        }

        public IEnumerator<long> FindPrimeNumber(int n)
        {
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                int prime = 1;// to check if found a prime
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if (prime > 0)
                    count++;
                a++;
                yield return 0;
            }
            yield return (--a);
        }

        private void DashOnOnDash(Obj_AI_Base sender, Dash.DashEventArgs dashEventArgs)
        {
            if (!sender.IsMe)
                return;

            var slot = !Spells.E.IsReady() ? SpellSlot.E : SpellSlot.Q;

            CastType currentCastType = CastType.NONE;
            if (slot == SpellSlot.Q)
            {
                LastQ = Environment.TickCount;
                currentCastType = CastType.Q;
            }
            if (slot == SpellSlot.E)
                currentCastType = CastType.E;

            CastType? nextCast = RivenMenu.GetNextCastTyoe(currentCastType);
            if (nextCast == null || GetTarget == null)
                return;

            switch (nextCast)
            {
                case CastType.Q: Spells.ForceQ(GetTarget.Position.To2D()); break;
                case CastType.W: Spells.ForceW(); break;
                case CastType.E: Spells.ForceE(GetTarget.Position.To2D()); break;
                case CastType.R1: Spells.ForceR1(); break;
                case CastType.R2: TryR2Cast(); break;
                case CastType.H: Spells.ForceItem(); break;
                case CastType.F: Spells.ForceFlash(TargetSelector.SelectedTarget.Position.To2D()); break;
            }
        }

        private int LastRTick;
        private Text rCdText = new Text("", new Font("Gill Sans MT Pro Book", 20f, FontStyle.Bold));
        private void DrawingOnOnDraw(EventArgs args)
        {
            if (RivenMenu.Draw["drawSpots"].Cast<CheckBox>().CurrentValue)
                DrawWallJumpSpots();

            var heropos = Drawing.WorldToScreen(Player.Instance.Position);
            if (RivenMenu.Draw["drawR1Status"].Cast<CheckBox>().CurrentValue)
            {
                bool useR1 = RivenMenu.Combo["R1"].Cast<KeyBind>().CurrentValue;

                Drawing.DrawText(heropos.X - 40, heropos.Y + 20, Color.DodgerBlue, "Use R1  [     ]");
                Drawing.DrawText(heropos.X + 20, heropos.Y + 20, useR1 ? Color.DodgerBlue : Color.Red,
                    useR1 ? "On" : "Off");
            }

            if (RivenMenu.Draw["drawBurstStatus"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawText(heropos.X - 40, heropos.Y + 40,
                    RivenMenu.Combo["burstKey"].Cast<KeyBind>().CurrentValue ? Color.DodgerBlue : Color.Red, "Shy Burst");
            }

            if (me.Level > 5 && Spells.R2.IsReady() && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Name == "RivenIzunaBlade" &&
                RivenMenu.Draw["drawRExpiry"].Cast<CheckBox>().CurrentValue)
            {
                float rCD_Sec = (15000 - (float)(Environment.TickCount - LastRTick)) / 1000;
                string rCd_Str = rCD_Sec.ToString("0.0");

                rCdText.TextValue = rCd_Str;
                rCdText.Color = rCD_Sec <= 5 && rCD_Sec > 2 ? Color.DarkOrange : Color.Red;

                rCdText.Position = heropos - new Vector2((float)rCdText.Bounding.Width / 2, -100);
                if (rCD_Sec <= 5 && rCD_Sec > 0)
                    rCdText.Draw();
            }

            if (RivenMenu.Draw["drawBurstRange"].Cast<CheckBox>().CurrentValue)
            {
                float maxRange = !Spells.Flash.IsReady()
                    ? Spells.E.Range + me.GetAutoAttackRange(TargetSelector.SelectedTarget)
                    : 700;
                new Circle {Color = Color.DodgerBlue, Radius = maxRange}.Draw(me.Position);
            }
        }

        private void DrawWallJumpSpots()
        {
            foreach (var jumpSpot in WallJumpData.WallJumpSpots.Where(x => x.Key.Distance(me) <= 1500))
            {
                Vector2 startpos = jumpSpot.Key;
                var endpos = startpos + (jumpSpot.Value - startpos) * 0.4f;
                var drawEndPos = startpos + (endpos - startpos) * 0.97f;

                Color c = Color.Red;
                if (!LastLeftClickPos.IsZero && !NearestJumpSpot.IsZero &&
                    (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Flee || TriggerWallJumpByDoubleClick))
                {
                    if (jumpSpot.Key == NearestJumpSpot)
                        c = Color.White;
                }
                new Circle { BorderWidth = 3, Color = c, Radius = 50 }.Draw(startpos.To3D());
                var blue = Color.DodgerBlue;

                Drawing.DrawLine(startpos.To3D().WorldToScreen(), drawEndPos.To3D().WorldToScreen(), 3, blue);
            }
        }

        private Vector2 Q2Pos;
        private int tick;
        private void GameOnOnUpdate(EventArgs args)
        {
            ContinueQ();
            #region Jump spot search
            //int qStacks = me.Buffs.FirstOrDefault(b => b.Name == "RivenTriCleave")?.Count ?? 0;
            //if (qStacks == 2)
            //{
            //    tick = Environment.TickCount;
            //    Q2Pos = Player.Instance.Position.To2D();

            //}
            //if (qStacks == 0 && Environment.TickCount - tick <= 500)
            //{
            //    if (Q2Pos.Distance(me) > 200)
            //    {
            //        WallJumpData.WallJumpSpots.Add(Q2Pos, me.Position.To2D());
            //        Chat.Print("From: " + Q2Pos + " To: " + me.Position.To2D());
            //        Logger.Debug(Q2Pos + " | " + me.Position.To2D());
            //        tick = 0;
            //    }
            //}

            //if (WallJumpData.WallJumpSpots.Any())
            //    Chat.Print(WallJumpData.WallJumpSpots.OrderBy(x => x.Key.Distance(Game.CursorPos)).First());
            #endregion

            CheckWallJumpSpots();
            CheckFlee();
            if (GetTarget != null)
            {
                CheckInstantSpells();
                CheckBurst();
            }
            CheckLaneClearExtra();
            CheckJungleClearExtra();
        }

        private void CheckJungleClearExtra()
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) && GetTarget != null && GetTarget.Distance(me) <= 300 &&
                !EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, me.Position, 600).Any(x => x.Equals(GetTarget)))
                if (!Orbwalker.CanAutoAttack && RivenMenu.IsEnabled("E") && GetTarget.Health < GetTarget.MaxHealth)
                    Spells.ForceE(Game.CursorPos.To2D());
        }

        private void CheckInstantSpells()
        {
            float facingAngle = GetTarget.Direction.To2D().Perpendicular().AngleBetween(me.Direction.To2D().Perpendicular());
            bool isRunningAway = facingAngle < 25;

            if (!RivenMenu.Combo["burstKey"].Cast<KeyBind>().CurrentValue && Spells.E.IsReady()
                && Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo && RivenMenu.Combo["EGap"].Cast<CheckBox>().CurrentValue)
            {
                if (Prediction.Position.PredictUnitPosition(GetTarget, Spells.E.CastDelay).Distance(me) < Spells.E.Range + me.GetAutoAttackRange() 
                    && GetTarget.Distance(me) > me.GetAutoAttackRange())
                {
                    Spells.ForceE(GetTarget.Position.To2D());
                }
            }
            else if (!RivenMenu.Combo["burstKey"].Cast<KeyBind>().CurrentValue && Spells.Q.IsReady() && !Spells.E.IsReady()
                && Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo && RivenMenu.Combo["QGap"].Cast<CheckBox>().CurrentValue)
            {
                if (Farming.GetQCone().IsInside(Prediction.Position.PredictUnitPosition(GetTarget, Spells.Q.CastDelay)) && 
                    GetTarget.Distance(me) > me.GetAutoAttackRange() && isRunningAway)
                {
                    Spells.ForceQ(GetTarget.Position.To2D());
                }
            }
            else if (isRunningAway && RivenMenu.Combo["QGap"].Cast<CheckBox>().CurrentValue &&
                     RivenMenu.Combo["EGap"].Cast<CheckBox>().CurrentValue &&
                     Spells.Q.IsReady() && Spells.E.IsReady() &&
                     Prediction.Position.PredictUnitPosition(GetTarget, Spells.E.CastDelay).Distance(me) >
                     Spells.E.Range + me.GetAutoAttackRange() &&
                     Prediction.Position.PredictUnitPosition(GetTarget, Spells.E.CastDelay + Spells.Q.CastDelay)
                         .Distance(me) <= Spells.Q.Range + Spells.E.Range + me.GetAutoAttackRange())
            {
                Spells.ForceE(GetTarget.Position.To2D());
                Core.DelayAction(Spells.ForceQ, Spells.E.CastDelay);
            }

            bool laneClear = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, me.Position, 750).Any() &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
            if (RivenMenu.Combo["InstaW"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() && !laneClear)
            {
                var pred = Prediction.Position.PredictUnitPosition(GetTarget, Spells.W.CastDelay /*+ 250*/);
                if (pred.Distance(Player.Instance) <= Spells.W.Range)
                {
                    Spells.ForceItem();
                    Core.DelayAction(Spells.ForceW, 250);
                }
            }
        }

        private void ContinueQ()
        {
            if (!RivenMenu.Misc["continueQ"].Cast<CheckBox>().CurrentValue || Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None)
                return;

            int qStacks = me.Buffs.FirstOrDefault(b => b.Name == "RivenTriCleave")?.Count ?? 0;
            if (Environment.TickCount - LastQ >= 3600 && qStacks > 0 &&
                !me.IsRecalling() && Spells.Q.IsReady())
            {
                Spells.Q.Cast(me.Position);
            }
        }

        private int LastQ, ClearTick1;
        private bool ClearBool1;
        private void CheckLaneClearExtra()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                return;

            if (GetTarget != null && GetTarget.Distance(me) > me.GetAutoAttackRange() && GetTarget.Distance(me) <= Spells.E.Range &&
                Prediction.Health.GetPrediction(GetTarget, Spells.E.CastDelay + (int) (Orbwalker.AttackDelay*1000)) > 0 &&
                Orbwalker.CanAutoAttack && !Spells.W.IsReady() && !Spells.Hydra.IsReady() && !Spells.Tiamat.IsReady() && RivenMenu.IsEnabled("E"))
            {
                Spells.ForceE(GetTarget.Position.To2D());
            }

            bool couldQ = Spells.Q.IsReady() && RivenMenu.IsEnabled("Q");
            bool couldW = RivenMenu.IsEnabled("W") && Spells.W.IsReady();
            bool couldH = RivenMenu.IsEnabled("H") && (Spells.Hydra.IsReady() || Spells.Tiamat.IsReady());
            bool couldE = Spells.E.IsReady() && RivenMenu.IsEnabled("E");
            if (couldW || couldH)
            {
                var minRadius = Spells.W.IsReady() ? Spells.W.Range : Spells.Hydra.Range;
                var location = Farming.GetBestFarmLocation(minRadius, couldE ? Spells.E.Range : 0);
                if (location.HasValue)
                {
                    Spells.ForceE(location.Value);
                    if (couldH)
                        Spells.ForceItem();

                    ClearTick1 = Environment.TickCount;
                    ClearBool1 = true;
                    if (!couldH) /*skip to faster w usage*/
                        ClearTick1 -= 250;
                }
            }

            if (Environment.TickCount - ClearTick1 >= 250 && ClearBool1)
            {
                if (couldW)
                    Spells.ForceW();
                ClearBool1 = false;
            }

            if (Farming.CanKillMinionWithQ() && couldQ)
            {
                var min = Farming.GetQKillableMinion();
                if (!me.IsInAutoAttackRange(min))
                    Spells.ForceQ(min.Position.To2D());
            }

            //if (Farming.GetMinionsInQ() > 1)
            //    Spells.ForceQ();
        }

        private void CheckFlee()
        {
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Flee && LastLeftClickPos.IsZero)
            {
                if (!Spells.E.IsReady())
                    Spells.ForceQ();
                else Spells.ForceE(Game.CursorPos.To2D());
            }
        }

        private void CheckBurst()
        {
            if (Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.Combo || !RivenMenu.Combo["burstKey"].Cast<KeyBind>().CurrentValue)
                return;

            var target = TargetSelector.SelectedTarget;
            if (target == null || !target.IsValidTarget() || target.IsZombie || target.IsInvulnerable)
            {
                return;
            }

            if (!Spells.R1.IsReady() || !Spells.E.IsReady() || !Spells.W.IsReady() ||
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).Name != "RivenFengShuiEngine")
            {
                return;
            }

            if (Spells.Flash.IsReady())
            {
                if (me.Distance(target.Position) > 700 || me.Distance(target) < Spells.E.Range + me.AttackRange)
                {
                    return;
                }

                Spells.E.Cast(target.Position);
                Spells.ForceR1();
                Core.DelayAction(() =>
                {
                    var targett = TargetSelector.SelectedTarget;
                    if (target == null || !target.IsValidTarget() || target.IsZombie || target.IsInvulnerable) return;

                    Core.DelayAction(() => Spells.ForceFlash(targett.Position.To2D()), 10);
                    //Core.DelayAction(Spells.ForceItem, 11);
                }, 180);
            }
            else if (target.Distance(me) <= Spells.E.Range + Spells.W.Range/2f)
            {
                Spells.E.Cast(target.Position);
                Spells.ForceR1();
                //Core.DelayAction(Spells.ForceItem, 1);
            }
        }

        private Vector2 LastLeftClickPos, NearestJumpSpot, JumpSpotEnd, NearestPreMoveSpot;

        private void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg == (uint) WindowMessages.LeftButtonDown)
            {
                LastLeftClickPos = Game.CursorPos.To2D();
                NearestJumpSpot = Vector2.Zero;
                NearestPreMoveSpot = Vector2.Zero;
            }

            /*Double Click triggers wall jump*/
            if (args.Msg == (uint) WindowMessages.LeftButtonDoubleClick && 
                WallJumpData.WallJumpSpots.Any(x => x.Key.Distance(Game.CursorPos) <= 300))
            {
                TriggerWallJumpByDoubleClick = true;
                LastLeftClickPos = Game.CursorPos.To2D();
                NearestJumpSpot = Vector2.Zero;
                NearestPreMoveSpot = Vector2.Zero;
            }

            if (args.Msg == (uint) WindowMessages.RightButtonDown)
            {
                TriggerWallJumpByDoubleClick = false;
                Orbwalker.DisableMovement = false;
                LastLeftClickPos = Vector2.Zero;
                NearestJumpSpot = Vector2.Zero;
                NearestPreMoveSpot = Vector2.Zero;
                WaitPrePos = false;
                WaitMainPos = false;
                CanQ3WallJump = false;
            }
        }

        bool WaitPrePos, WaitMainPos, CanQ3WallJump, TriggerWallJumpByDoubleClick;
        private void CheckWallJumpSpots()
        {
            int qStacks = me.Buffs.FirstOrDefault(b => b.Name == "RivenTriCleave")?.Count ?? 0;

            if (!LastLeftClickPos.IsZero && (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Flee || TriggerWallJumpByDoubleClick) 
                && NearestPreMoveSpot.IsZero && NearestJumpSpot.IsZero)
            {
                var bestSpot = WallJumpData.WallJumpSpots.OrderBy(x => x.Key.Distance(LastLeftClickPos)).First();
                NearestJumpSpot = bestSpot.Key;
                JumpSpotEnd = bestSpot.Value;

                float extraTurnRange = !Spells.E.IsReady() ? 150 : 0;
                var preMove = bestSpot.Value.Extend(bestSpot.Key, bestSpot.Key.Distance(bestSpot.Value) + extraTurnRange);
                NearestPreMoveSpot = preMove;

                WaitPrePos = true;
                WaitMainPos = true;
            }

            if (WaitPrePos && !NearestPreMoveSpot.IsZero && me.Distance(NearestPreMoveSpot) > 80)
            {
                if (qStacks < 2 && me.Distance(NearestPreMoveSpot) > Spells.Q.Range)
                    Spells.Q.Cast(me.Position);
                Orbwalker.DisableMovement = true;
                Player.IssueOrder(GameObjectOrder.MoveTo, NearestPreMoveSpot.To3D());
            }
            else if (WaitPrePos && !NearestPreMoveSpot.IsZero && me.Distance(NearestPreMoveSpot) <= 80)
            {
                NearestPreMoveSpot = Vector2.Zero;
                WaitPrePos = false;
            }

            if (!WaitPrePos && WaitMainPos && !NearestJumpSpot.IsZero && me.Distance(NearestJumpSpot) > 80)
            {
                Orbwalker.DisableMovement = true;
                Player.IssueOrder(GameObjectOrder.MoveTo, NearestJumpSpot.To3D());
            }
            else if (!WaitPrePos && WaitMainPos && !NearestJumpSpot.IsZero && me.Distance(NearestJumpSpot) <= 80)
            {
                Spells.ForceE(JumpSpotEnd);
                Orbwalker.DisableMovement = true;
                WaitMainPos = false;
                CanQ3WallJump = true;
            }

            //if (CanQ3WallJump && qStacks == 2 && !Spells.E.IsReady())
            //Chat.Print(CanQ3WallJump && qStacks == 2 && !Spells.E.IsReady());
            else if (CanQ3WallJump)
            {
                if (qStacks < 2)
                    Spells.Q.Cast(me.Position);
                else Core.DelayAction(() => Spells.Q.Cast(me.Position), (int)(Spells.E.CastDelay + (float)Spells.E.Range/Spells.E.Speed*1000f));
            }

            if (CanQ3WallJump && qStacks == 0)
            {
                Orbwalker.DisableMovement = false;
                LastLeftClickPos = Vector2.Zero;
                NearestJumpSpot = Vector2.Zero;
                NearestPreMoveSpot = Vector2.Zero;
                WaitPrePos = false;
                WaitMainPos = false;
                CanQ3WallJump = false;
                TriggerWallJumpByDoubleClick = false;
            }
        }

        private void ObjAiBaseOnOnPlayAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe)
                return;
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.None || Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Flee)
                return;

            int QD = RivenMenu.Misc["qDelay"].Cast<Slider>().CurrentValue, QLD = RivenMenu.Misc["q3Delay"].Cast<Slider>().CurrentValue;
            
            switch (args.AnimationHash.ToString())
            {
                case "-1030080981":
                case "-1013303362":
                    Core.DelayAction(ResetAutoAttack, 10 * QD + 1);
                    break;
                case "-996525743":
                    Core.DelayAction(ResetAutoAttack, 10 * QLD + 1);
                    break;
                case "-1242153695": /*W*/
                        Core.DelayAction(ResetAutoAttack, 10 * QD + 1);/*just try to*/
                    break;
            }
        }

        private void ResetAutoAttack()
        {
            Player.DoEmote(Emote.Dance);
            Orbwalker.ResetAutoAttack();
        }

        private Obj_AI_Base GetTarget
        {
            get
            {
                Obj_AI_Base target = null;
                if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
                    target = TargetSelector.GetTarget(250 + me.AttackRange + 70, DamageType.Physical);
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                    var Mobs =
                        EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, me.Position,
                            250 + me.AttackRange + 70)
                            .OrderByDescending(x => x.MaxHealth).ToList();
                    target = Mobs.FirstOrDefault();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    var Mobs =
                        EntityManager.MinionsAndMonsters.GetJungleMonsters(me.Position, 250 + me.AttackRange + 70)
                            .OrderByDescending(x => x.MaxHealth).ToList();
                    if (target == null)
                    {
                        target = Mobs.FirstOrDefault();
                    }
                }

                if (target != null && (!target.IsValid || target.IsInvulnerable || target.IsZombie))
                    return null;

                return target;
            }
        }

        private void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
                return;

            CastType currentCastType = CastType.NONE;
            if (args.SData.Name.Contains("BasicAttack"))
            {
                currentCastType = CastType.AA;

                if (ObjectManager.Get<Obj_AI_Turret>().Any(x => me.Distance(x) <= me.GetAutoAttackRange() + x.BoundingRadius + 200))
                {
                    Spells.ForceQ();
                }

                if (me.GetAutoAttackDamage(args.Target as Obj_AI_Base) >= (args.Target as Obj_AI_Base).Health)
                    return;
            }
            if (args.Slot == SpellSlot.W)
                currentCastType = CastType.W;
            if (args.SData.Name.Contains("RivenFengShuiEngine")) //firstR
            {
                currentCastType = CastType.R1;
                LastRTick = Environment.TickCount;
            }
            if (args.SData.Name.Contains("RivenIzunaBlade")) //secondR
            {
                currentCastType = CastType.R2;
                RivenMenu.Combo["burstKey"].Cast<KeyBind>().CurrentValue = false;
            }
            if (args.SData.Name.Contains("ItemTiamatCleave"))
                currentCastType = CastType.H;
            if (args.SData.Name.ToLower().Contains("flash"))
                currentCastType = CastType.F;

            if (currentCastType == CastType.NONE)
                return;

            if (GetTarget == null)
            {
                return;
            }

            CastType? nextCast = RivenMenu.GetNextCastTyoe(currentCastType);
            if (nextCast == null)
                return;

            switch (nextCast)
            {
                case CastType.Q: Spells.ForceQ(GetTarget.Position.To2D()); break;
                case CastType.W: Spells.ForceW(); break;
                case CastType.E: Spells.ForceE(GetTarget.Position.To2D()); break;
                case CastType.R1: Spells.ForceR1(); break;
                case CastType.R2: TryR2Cast(); break;
                case CastType.H: Spells.ForceItem(); break;
                case CastType.F: Spells.ForceFlash(TargetSelector.SelectedTarget.Position.To2D()); break;
            }
        }

        private void TryR2Cast()
        {
            /*check min hits (by default 1) => doesnt influence the combo*/
            if (!RivenMenu.Combo["onlyR2ToKs"].Cast<CheckBox>().CurrentValue)
            {
                UltimatePrediction.CheckUltimateHits();
                return;
            }

            /*kill only*/
            var pred = Prediction.Position.PredictUnitPosition(GetTarget, (int)(Spells.R2.CastDelay + GetTarget.Distance(Player.Instance) / Spells.R2.Speed * 1000));
            Spells.ForceR2(me.Position.Extend(pred, 100));
        }
    }
}
