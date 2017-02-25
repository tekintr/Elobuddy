﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Moon_Walk_Evade.EvadeSpells;
using Moon_Walk_Evade.Skillshots;
using Moon_Walk_Evade.Skillshots.SkillshotTypes;
using Moon_Walk_Evade.Utils;
using SharpDX;
using Color = System.Drawing.Color;

namespace Moon_Walk_Evade.Evading
{
    public class MoonWalkEvade
    {
        #region Properties

        public int ServerTimeBuffer
        {
            get { return 0; }
        }

        public bool EvadeEnabled
        {
            get { return EvadeMenu.HotkeysMenu["enableEvade"].Cast<KeyBind>().CurrentValue; }
            set { EvadeMenu.HotkeysMenu["enableEvade"].Cast<KeyBind>().CurrentValue = value; }
        }

        public bool DodgeDangerousOnly
        {
            get { return EvadeMenu.HotkeysMenu["dodgeOnlyDangerousH"].Cast<KeyBind>().CurrentValue || EvadeMenu.HotkeysMenu["dodgeOnlyDangerousT"].Cast<KeyBind>().CurrentValue; }
        }

        public int ExtraEvadeRange
        {
            get { return EvadeMenu.HumanizerMenu["extraEvadeRange"].Cast<Slider>().CurrentValue; }
        }


        public bool RandomizeExtraEvadeRange
        {
            get { return EvadeMenu.HumanizerMenu["randomizeExtraEvadeRange"].Cast<CheckBox>().CurrentValue; }
        }

        public bool AllowRecalculateEvade
        {
            get { return EvadeMenu.MainMenu["recalculatePosition"].Cast<CheckBox>().CurrentValue; }
        }

        public int RecalculationDelay
        {
            get { return EvadeMenu.MainMenu["recalculationSpeed"].Cast<Slider>().CurrentValue; }
        }
        public int MinRecalculationAngle
        {
            get { return EvadeMenu.MainMenu["minRecalculationAngle"].Cast<Slider>().CurrentValue; }
        }

        public bool RestorePosition
        {
            get { return EvadeMenu.MainMenu["moveToInitialPosition"].Cast<CheckBox>().CurrentValue; }
        }

        public bool DisableDrawings
        {
            get { return EvadeMenu.DrawMenu["disableAllDrawings"].Cast<CheckBox>().CurrentValue; }
        }

        public bool DrawEvadePoint
        {
            get { return EvadeMenu.DrawMenu["drawEvadePoint"].Cast<CheckBox>().CurrentValue; }
        }

        public bool DrawEvadeStatus
        {
            get { return EvadeMenu.DrawMenu["drawEvadeStatus"].Cast<CheckBox>().CurrentValue; }
        }

        public enum DrawingType { Fancy, Fast }
        public DrawingType CurrentDrawingType
        {
            get { return (DrawingType)EvadeMenu.DrawMenu["drawType"].Cast<Slider>().CurrentValue; }
        }

        public int IssueOrderTickLimit
        {
            get { return 0 /*Game.Ping * 2*/; }
        }

        public enum StutterSearchType
        {
            MousePos, PlayerFaceDirection, FarestAway,
        }
        public StutterSearchType AntiStutterSearchType => (StutterSearchType)EvadeMenu.HumanizerMenu["stutterPointFindType"].Cast<Slider>().CurrentValue;

        public bool manageOrbwalker => EvadeMenu.DebugMenu["manageMovementDelay"].Cast<CheckBox>().CurrentValue;

        public bool ForceEvade => EvadeMenu.MainMenu["forceEvade"].Cast<CheckBox>().CurrentValue;

        public int minComfortDistance => EvadeMenu.MainMenu["minComfortDistance"].Cast<Slider>().CurrentValue;
        public int minEnemyComfortCount => EvadeMenu.MainMenu["enemyComfortCount"].Cast<Slider>().CurrentValue;
        public bool DontEvadeAtNoComfortPoint => EvadeMenu.MainMenu["dontEvadeComfort"].Cast<CheckBox>().CurrentValue;

        public bool BlockDangerousDashes => EvadeMenu.SpellBlockerMenu["blockDangerousDashes"].Cast<CheckBox>().CurrentValue;

        public enum EvadeMode { Smooth, Fast}
        public EvadeMode CurrentEvadeMode => EvadeMenu.MainMenu["evadeMode"].Cast<ComboBox>().SelectedIndex == 0 ? EvadeMode.Smooth : EvadeMode.Fast;

        public bool UseFastInSmoothMode => EvadeMenu.HumanizerMenu["useFastInSmooth"].Cast<CheckBox>().CurrentValue;

        public int MinDangerTurretEvade => EvadeMenu.MainMenu["minDangerTurretEvade"].Cast<Slider>().CurrentValue;

        public bool LetAAPass => EvadeMenu.MainMenu["letAAPass"].Cast<CheckBox>().CurrentValue;

        #endregion

        #region Vars

        public SpellDetector SkillshotDetector { get; set; }

        public EvadeSkillshot[] Skillshots { get; private set; }
        public Geometry.Polygon[] Polygons { get; private set; }
        public List<Geometry.Polygon> ClippedPolygons { get; private set; }

        public Vector2 LastIssueOrderPos
        {
            get { return _lastIssueOrderPos; }
            set
            {
                _lastIssueOrderPos = value;
                Debug.LastIssueOrderPos = value;
            }
        }

        private readonly Dictionary<EvadeSkillshot, Geometry.Polygon> _skillshotPolygonCache;

        private int LastEvadeSetTick;
        public EvadeResult CurrentEvadeResult
        {
            get { return _currentEvadeResult; }
            set
            {
                _currentEvadeResult = value;
                LastEvadeSetTick = Environment.TickCount;
            }
        }

        private Text StatusText, WarnText;
        private int EvadeIssurOrderTime;
        private EvadeResult _currentEvadeResult;
        private Vector2 _lastIssueOrderPos;

        #endregion

        public MoonWalkEvade(SpellDetector detector)
        {
            Skillshots = new EvadeSkillshot[] { };
            Polygons = new Geometry.Polygon[] { };
            ClippedPolygons = new List<Geometry.Polygon>();
            StatusText = new Text("MoonWalkEvade Enabled", new Font("Euphemia", 10F, FontStyle.Bold)); //Calisto MT
            WarnText = new Text("", new Font("Euphemia", 18F, FontStyle.Bold)); //Calisto MT
            _skillshotPolygonCache = new Dictionary<EvadeSkillshot, Geometry.Polygon>();

            SkillshotDetector = detector;
            SkillshotDetector.OnUpdateSkillshots += OnUpdateSkillshots;
            SkillshotDetector.OnSkillshotActivation += OnSkillshotActivation;
            SkillshotDetector.OnSkillshotDetected += OnSkillshotDetected;
            SkillshotDetector.OnSkillshotDeleted += OnSkillshotDeleted;

            Player.OnIssueOrder += PlayerOnIssueOrder;
            Spellbook.OnCastSpell += SpellbookOnOnCastSpell;
            Dash.OnDash += OnDash;
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }

        private void OnUpdateSkillshots(EvadeSkillshot skillshot, bool remove, bool isProcessSpell)
        {
            CacheSkillshots();
        }

        private void OnSkillshotActivation(EvadeSkillshot skillshot)
        {
            CacheSkillshots();
        }

        private void OnSkillshotDetected(EvadeSkillshot skillshot, bool isProcessSpell)
        {
            if (CurrentEvadeResult != null && CurrentEvadeResult.EnoughTime)
                if (!skillshot.IsSafePath(Player.Instance.GetPath(CurrentEvadeResult.WalkPoint).ToVector2(),
                     ServerTimeBuffer))
                {
                    CurrentEvadeResult = null;
                }
        }

        private void OnSkillshotDeleted(EvadeSkillshot skillshot)
        {
            if (RestorePosition && !SkillshotDetector.DetectedSkillshots.Any())
            {
                if (CurrentEvadeResult != null && Player.Instance.IsMovingTowards(CurrentEvadeResult.EvadePoint))
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, LastIssueOrderPos.To3DWorld(), false);
                }
            }
        }

        private int LastRecalcTick, LastUpdateTick;
        private void OnUpdate(EventArgs args)
        {
            if (Environment.TickCount - LastUpdateTick < 1)
                return;
            LastUpdateTick = Environment.TickCount;

            if (!EvadeEnabled || b_PassAA || Player.Instance.IsDead || Player.Instance.IsDashing() || Player.Instance.IsInFountainRange())
            {
                CurrentEvadeResult = null;
                Orbwalker.DisableMovement = false;
                return;
            }

            CheckEvade();

            if (CurrentEvadeResult != null && CurrentEvadeResult.EnoughTime)
            {
                if (CurrentEvadeResult.ShouldPreventStuttering)
                {
                    var newPoints = GetEvadePoints(CurrentEvadeResult.WalkPoint.To2D());
                    var point = newPoints.FirstOrDefault();
                    if (point != default(Vector2))
                    {
                        CurrentEvadeResult.EvadePoint = point;
                    }
                }
                else if (AllowRecalculateEvade && Environment.TickCount - LastRecalcTick >= RecalculationDelay)
                {
                    LastRecalcTick = Environment.TickCount;
                    var newP = GetEvadePoints(Player.Instance.Position.To2D()).FirstOrDefault();
                    float angle =(newP.To3D() - Player.Instance.Position).To2D()
                            .AngleBetween((CurrentEvadeResult.WalkPoint - Player.Instance.Position).To2D());
                    bool betterPos = newP.Distance(Game.CursorPos) < CurrentEvadeResult.EvadePoint.Distance(Game.CursorPos);
                    if (angle >= MinRecalculationAngle && betterPos)
                    {
                        CurrentEvadeResult.EvadePoint = newP;
                    }
                }

                MoveTo(CurrentEvadeResult.WalkPoint, false);
            }
        }


        /// <summary>
        /// set evade
        /// </summary>
        /// <returns></returns>
        private void CheckEvade()
        {
            bool isOrbwalking = Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None;
            CacheSkillshots();

            bool goodPath = IsPathSafeEx(isOrbwalking && !Orbwalker.IsAutoAttacking ? Game.CursorPos.To2D() : LastIssueOrderPos);
            if (!goodPath && CurrentEvadeResult == null)
            {
                var evade = CalculateEvade(LastIssueOrderPos);
                if (evade.IsValid && evade.EnoughTime)
                {
                    CurrentEvadeResult = evade;
                    Orbwalker.DisableMovement = true;
                }
            }
            else if (goodPath)
            {
                CurrentEvadeResult = null;
                Orbwalker.DisableMovement = false;
            }
        }

        private bool b_PassAA;
        void PassAA()
        {
            int extraWindUpTime = Orbwalker.ExtraWindUpTime;
            switch (Player.Instance.Hero)
            {
                case Champion.Jinx:
                    extraWindUpTime += 150;
                    break;
                case Champion.Rengar:
                    extraWindUpTime += 150;
                    break;
            }

            float aaDelay = Orbwalker.AttackCastDelay * 1000.0f + extraWindUpTime + Game.Ping / 10.0f + 50;
            CurrentEvadeResult = null;
            b_PassAA = true;
            Core.DelayAction(() => b_PassAA = false, (int)Math.Ceiling(aaDelay));
        }

        private void PlayerOnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.Order == GameObjectOrder.AttackUnit)
            {
                LastIssueOrderPos = Player.Instance.Distance(args.Target) > Player.Instance.GetAutoAttackRange(args.Target as AttackableUnit)
                        ? args.Target.Position.Extend(Player.Instance, Player.Instance.GetAutoAttackRange(args.Target as AttackableUnit))
                        : Player.Instance.Position.To2D();
                
                if (EvadeEnabled && !b_PassAA && Player.Instance.IsInAutoAttackRange(args.Target as AttackableUnit))
                {
                    if (!IsPointSafe(Player.Instance.Position.To2D()))
                    {
                        if (!LetAAPass)
                        {
                            args.Process = false;
                            return;
                        }

                        int extraWindUpTime = Orbwalker.ExtraWindUpTime;
                        switch (Player.Instance.Hero)
                        {
                            case Champion.Jinx:
                                extraWindUpTime += 150;
                                break;
                            case Champion.Rengar:
                                extraWindUpTime += 150;
                                break;
                        }
                        float aaDelay = Orbwalker.AttackCastDelay*1000.0f + extraWindUpTime + Game.Ping/10.0f + 50;
                        var points = GetEvadePoints(delay: aaDelay);

                        if (!points.Any())
                            args.Process = false;
                        else
                        {
                            PassAA();
                        }
                    }
                    else /*point safe*/
                    {
                        PassAA();
                    }
                }
            }
            else
            {
                LastIssueOrderPos = (args.Target?.Position ?? args.TargetPosition).To2D();
            }

            if (CurrentEvadeResult != null && CurrentEvadeResult.EnoughTime)
                args.Process = false;
        }

        private void SpellbookOnOnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!sender.Owner.IsMe || !EvadeEnabled)
            {
                return;
            }

            if (SpellBlocker.WillBlock(args.Slot) && IsHeroInDanger())
            {
                args.Process = false;
                return;
            }

            if (BlockDangerousDashes && EvadeSpellManager.IsDashSpell(args.Slot) && !IsHeroInDanger())
            {
                if (!EvadeSpellManager.IsDashSafe(EvadeSpellDatabase.Spells.First(x => x.Slot == args.Slot), args.EndPosition.To2D(), this))
                    args.Process = false;
            }
        }

        private void OnDash(Obj_AI_Base sender, Dash.DashEventArgs dashEventArgs)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (CurrentEvadeResult != null)
            {
                CurrentEvadeResult = null;
                Player.IssueOrder(GameObjectOrder.MoveTo, LastIssueOrderPos.To3DWorld(), false);
            }
        }

        private IEnumerable<Vector2> GetPathDots(Vector2 start, Vector2 end, int dotRadius = 30, int space = 100)
        {
            List<Vector2> vecList = new List<Vector2>();
            //x*(dotRadius*2+space) = pathLength
            int dotAmount = (int)Math.Floor((start - end).Length() / (dotRadius * 2 + space));

            float currentDist = 0;
            for (int i = 0; i < dotAmount; i++)
            {
                vecList.Add(start.Extend(end, currentDist));
                currentDist += dotRadius + space;
            }

            return vecList;
        }
        private void OnDraw(EventArgs args)
        {

            if (DisableDrawings)
            {
                return;
            }

            if (DrawEvadePoint && CurrentEvadeResult != null)
            {
                if (CurrentEvadeResult.IsValid && CurrentEvadeResult.EnoughTime && !CurrentEvadeResult.Expired())
                {
                    var color = !CurrentEvadeResult.IsForced ? new ColorBGRA(255, 255, 255, 255) : new ColorBGRA(255, 165, 0, 255);
                    foreach (var dot in GetPathDots(CurrentEvadeResult.WalkPoint.To2D(), Player.Instance.Position.To2D(), 10, 25))
                    {
                        new Circle(color, 10, 2, true).Draw(dot.To3D());
                    }
                }
            }

            if (DrawEvadeStatus)
            {
                StatusText.Color = EvadeEnabled ? Color.White : Color.Red;
                if (DodgeDangerousOnly)
                    StatusText.Color = Color.Orange;
                StatusText.TextValue = "MoonWalkEvade " + (EvadeEnabled ? "Enabled" : "Disabled");
                StatusText.Position = Player.Instance.Position.WorldToScreen() - new Vector2(StatusText.Bounding.Width / 2f, -25);
                StatusText.Draw();
            }

            /*Check spell block paradoxon*/
            for (int i = 0; i < 4; i++)
            {
                var slot = (SpellSlot)i;
                var evadeSpell = EvadeMenu.MenuEvadeSpells.FirstOrDefault(spell => spell.Slot == slot);

                if (EvadeMenu.SpellBlockerMenu["block" + Player.Instance.ChampionName + "/" + slot].Cast<CheckBox>().CurrentValue
                    && EvadeMenu.IsEvadeSkillhotEnabled(evadeSpell))
                {
                    WarnText.Color = Color.Red;
                    WarnText.TextValue = "Evade spell '" + evadeSpell.SpellName + "' is checked in SpellBlocker!";
                    WarnText.Position = Player.Instance.Position.WorldToScreen() - new Vector2(WarnText.Bounding.Width / 2f, -50);
                    WarnText.Draw();
                }
            }

            //if (EvadeMenu.DebugMenu["debugMode"].Cast<KeyBind>().CurrentValue)
            //    foreach (var evadePoint in GetEvadePoints(null, 900, 50))
            //    {
            //        Circle.Draw(new ColorBGRA(0, 255, 0, 255), Player.Instance.BoundingRadius, 2, evadePoint.To3D());
            //    }
        }

        private void CacheSkillshots()
        {
            Skillshots =
                (DodgeDangerousOnly
                    ? SkillshotDetector.ActiveSkillshots.Where(c => c.OwnSpellData.IsDangerous)
                    : SkillshotDetector.ActiveSkillshots).ToArray();

            _skillshotPolygonCache.Clear();

            Polygons = Skillshots.Select(c =>
            {
                Geometry.Polygon pol;
                if (c.OwnSpellData.IsVeigarE)
                {
                    var skill = c as VeigarE;
                    var inner = skill.ToInnerPolygon();
                    var outer = skill.ToOuterPolygon();

                    if (!skill.IsSafe())
                        pol = outer;
                    else
                    {
                        pol = inner.IsInside(Player.Instance) ? inner : outer;
                    }
                }
                else pol = c.ToPolygon();

                _skillshotPolygonCache.Add(c, pol);
                return pol;
            }).ToArray();

            ClippedPolygons = Geometry.ClipPolygons(Polygons).ToPolygons();
        }

        public bool IsPointSafe(Vector2 point)
        {
            return _skillshotPolygonCache.All(s => s.Key.IsSafe(point));
        }

        public bool IsHeroInDanger(AIHeroClient hero = null)
        {
            hero = hero ?? Player.Instance;
            return !IsPointSafe(hero.ServerPosition.To2D());
        }

        public int GetTimeAvailable(AIHeroClient hero = null)
        {
            hero = hero ?? Player.Instance;
            var skillshots = Skillshots.Where(c => !c.IsSafe(hero.Position.To2D())).ToArray();

            if (!skillshots.Any())
            {
                return short.MaxValue;
            }

            var times =
                skillshots.Select(c => c.GetAvailableTime(hero.ServerPosition.To2D()))
                    .Where(t => t > 0)
                    .OrderByDescending(t => t);

            return times.Any() ? times.Last() : short.MaxValue;
        }

        public int GetDangerValue(AIHeroClient hero = null)
        {
            hero = hero ?? Player.Instance;
            var skillshots = Skillshots.Where(c => !c.IsSafe(hero.Position.To2D())).ToArray();

            if (!skillshots.Any())
                return 0;

            var values = skillshots.Select(c => c.OwnSpellData.DangerValue).OrderByDescending(t => t);
            return values.Any() ? values.First() : 0;
        }

        private bool MayEvadeUnderTurret() => GetDangerValue() >= MinDangerTurretEvade;
        private bool TurretValid(Vector2 p) => MayEvadeUnderTurret() || !p.IsUnderTurret();

        public bool IsPathSafeEx(Vector2[] path, int speed = -1, int delay = 0)
        {
            return Skillshots.All(evadeSkillshot =>
            {
                bool safe = evadeSkillshot.IsSafePath(path, ServerTimeBuffer, speed, delay);
                //if (path.Length == 2 && path[1].Distance(LastIssueOrderPos) <= 50)
                //    if (!safe && evadeSkillshot.OwnSpellData.IsVeigarE)
                //        Chat.Print(Game.Time);
                return safe;
            });
        }

        public bool IsPathSafeEx(Vector2 end, float speed = -1, float delay = 0)
        {
            return IsPathSafeEx(Player.Instance.GetPath(end.To3D()).ToVector2(), (int)speed, (int)delay);
        }

        public List<Vector2> GetEvadePoints(Vector2? awayFrom = null, float speed = -1, float delay = 0)
        {
            bool inside = IsHeroInDanger();
            if (CurrentEvadeMode == EvadeMode.Fast && inside)
            {
                var p = GetClosestEvadePoint(Player.Instance.Position.To2D());
                if (IsPathSafeEx(p, speed, delay))
                    return new List<Vector2> {GetClosestEvadePoint(Player.Instance.Position.To2D())};
            }

            speed = speed == -1 ? Player.Instance.MoveSpeed : speed;

            int posChecked = 0;
            const int maxPosToCheck = 150;
            const int posRadius = 50;
            int radiusIndex = 0;
            var heroPoint = Player.Instance.Position;
            var points = new List<Vector2>();

            while (posChecked < maxPosToCheck)
            {
                radiusIndex++;

                int curRadius = radiusIndex * 2 * posRadius;
                int curCircleChecks = (int)Math.Ceiling(2 * Math.PI * curRadius / (2 * (double)posRadius));

                for (int i = 1; i < curCircleChecks; i++)
                {
                    posChecked++;
                    var cRadians = 2 * Math.PI / (curCircleChecks - 1) * i; //check decimals
                    var pos = new Vector2((float)Math.Floor(heroPoint.X + curRadius * Math.Cos(cRadians)), (float)Math.Floor(heroPoint.Y + curRadius * Math.Sin(cRadians)));

                    points.Add(pos);
                }
            }

            if (UseFastInSmoothMode && inside)
            {
                var closestNormalPoint = points.Where(p => IsPathSafeEx(p, speed, delay)).OrderBy(x => x.Distance(Player.Instance)).FirstOrDefault();
                points.AddRange(GetCloserEvadePoints(closestNormalPoint));
            }

            return !awayFrom.HasValue ?
                points.Where(p => TurretValid(p) && IsPointSafe(p) && IsPathSafeEx(p, speed, delay) && !p.IsWallBetweenPlayer()).ToList() :
                points.Where(p => TurretValid(p) && IsPointSafe(p) && IsPathSafeEx(p, speed, delay) && !p.IsWallBetweenPlayer() && 
                p.Distance(awayFrom.Value) >= 225).OrderBy(p =>
                {
                    if (AntiStutterSearchType == StutterSearchType.PlayerFaceDirection)
                    {
                        var faceVec = 500 * ObjectManager.Player.Direction.To2D().Perpendicular();
                        var destVec = p - Player.Instance.Position.To2D();
                        return faceVec.AngleBetween(destVec);
                    }

                    if (AntiStutterSearchType == StutterSearchType.FarestAway)
                        return -p.Distance(Player.Instance);

                    /*AntiStutterSearchType == StutterSearchType.MousePos*/
                    return p.Distance(Game.CursorPos);
                }).ToList();
        }

        public Vector2 GetClosestEvadePoint(Vector2 from)
        {
            var polygons = ClippedPolygons.Where(p => p.IsInside(from)).ToArray();

            var polPoints =
                polygons.Select(pol => pol.ToDetailedPolygon())
                    .SelectMany(pol => pol.Points)
                    .OrderByDescending(p => p.Distance(from, true));

            return !polPoints.Any() ? Vector2.Zero : polPoints.Last();
        }

        public IOrderedEnumerable<Vector2> GetCloserEvadePoints(Vector2 closestNormalEvadePoint)
        {
            float closestNormalDistance = closestNormalEvadePoint.Distance(Player.Instance);
            var polygons = ClippedPolygons.Where(p => p.IsInside(Player.Instance)).ToArray();

            var polPoints =
                polygons.Select(pol => pol.ToDetailedPolygon())
                    .SelectMany(pol => pol.Points)
                    .Where(p => p.Distance(Player.Instance) < closestNormalDistance)
                    .OrderByDescending(p => p.Distance(Player.Instance));

            return polPoints;
        }

        public bool IsComfortPoint(Vector2 p) => !EntityManager.Heroes.Enemies.Any(x => x.IsValid && !x.IsDead && x.Distance(p) <= minComfortDistance);
        public bool DoesComfortPointExist(IEnumerable<Vector2> points) => points.Any(IsComfortPoint);
        public bool HasToAttendComfort() =>
            EntityManager.Heroes.Enemies.Count(x => x.IsValid && !x.IsDead && x.Distance(Player.Instance) <= 1000) >= minEnemyComfortCount;

        public EvadeResult CalculateEvade(Vector2 anchor, Vector2? awayFrom = null)
        {
            var playerPos = Player.Instance.ServerPosition.To2D();
            var maxTime = GetTimeAvailable();
            var time = Math.Max(0, maxTime - (Game.Ping + ServerTimeBuffer));

            var points = GetEvadePoints(awayFrom ?? Player.Instance.Position.To2D());

            if (!points.Any())
            {
                Vector2 evadeSpellEvadePoint;
                if (!EvadeSpellManager.TryEvadeSpell(time, this, out evadeSpellEvadePoint))
                {
                    var closest = GetClosestEvadePoint(playerPos);
                    if (HasToAttendComfort() && !DoesComfortPointExist(new[] { closest }) &&DontEvadeAtNoComfortPoint)
                    {
                        return new EvadeResult(this, Vector2.Zero, anchor, 0, 0, false);
                    }
                    return new EvadeResult(this, closest, anchor, maxTime, time, ForceEvade) { IsForced = ForceEvade };
                }

                if (HasToAttendComfort() && !DoesComfortPointExist(new []{evadeSpellEvadePoint}) && DontEvadeAtNoComfortPoint)
                {
                    return new EvadeResult(this, Vector2.Zero, anchor, 0, 0, false);
                }

                //can use evade spell
                CurrentEvadeResult = new EvadeResult(this, evadeSpellEvadePoint, anchor, maxTime, time, true);

            }

            if (HasToAttendComfort())
            {
                if (DoesComfortPointExist(points))
                    points.RemoveAll(p => !IsComfortPoint(p));
                else if (DontEvadeAtNoComfortPoint)
                {
                    return new EvadeResult(this, Vector2.Zero, anchor, 0, 0, false);
                }
            }

            var evadePoint = points.OrderBy(p => !p.IsUnderTurret()).ThenBy(p => p.Distance(Game.CursorPos)).FirstOrDefault();
            return new EvadeResult(this, evadePoint, anchor, maxTime, time, true);
        }

        public bool MoveTo(Vector2 point, bool limit = true)
        {
            if (limit && EvadeIssurOrderTime + IssueOrderTickLimit >= Environment.TickCount)
            {
                return false;
            }

            EvadeIssurOrderTime = Environment.TickCount;
            Player.IssueOrder(GameObjectOrder.MoveTo, point.To3DWorld(), false);

            return true;
        }

        public bool MoveTo(Vector3 point, bool limit = true)
        {
            return MoveTo(point.To2D(), limit);
        }

        public class EvadeResult
        {
            private MoonWalkEvade moonWalkEvadeInstance;


            public bool IsOutsideEvade { get; set; }

            public int StutterDistance => EvadeMenu.HumanizerMenu["stutterDistanceTrigger"].Cast<Slider>().CurrentValue;
            public bool ShouldPreventStuttering => /*IsOutsideEvade &&*/ Player.Instance.Distance(WalkPoint) <= StutterDistance;



            private int ExtraRange { get; set; }

            public int Time { get; set; }
            public Vector2 PlayerPos { get; set; }
            public Vector2 EvadePoint { get; set; }
            public Vector2 AnchorPoint { get; set; }
            public int TimeAvailable { get; set; }
            public int TotalTimeAvailable { get; set; }
            public bool EnoughTime { get; set; }

            public bool IsForced { get; set; }

            public bool IsValid
            {
                get { return !EvadePoint.IsZero; }
            }

            public Vector3 WalkPoint
            {
                get
                {
                    var walkPoint = EvadePoint.Extend(PlayerPos, -80);
                    var newPoint = walkPoint.Extend(PlayerPos, -ExtraRange);

                    if (moonWalkEvadeInstance.IsPointSafe(newPoint))
                    {
                        return newPoint.To3DWorld();
                    }

                    return walkPoint.To3DWorld();
                }
            }

            public EvadeResult(MoonWalkEvade _moonWalkEvadeInstance, Vector2 evadePoint, Vector2 anchorPoint, int totalTimeAvailable,
                int timeAvailable,
                bool enoughTime)
            {
                moonWalkEvadeInstance = _moonWalkEvadeInstance;
                PlayerPos = Player.Instance.Position.To2D();
                Time = Environment.TickCount;

                EvadePoint = evadePoint;
                AnchorPoint = anchorPoint;
                TotalTimeAvailable = totalTimeAvailable;
                TimeAvailable = timeAvailable;
                EnoughTime = enoughTime;

                // extra moonWalkEvadeInstance range
                if (moonWalkEvadeInstance.ExtraEvadeRange > 0)
                {
                    ExtraRange = moonWalkEvadeInstance.RandomizeExtraEvadeRange
                        ? Utils.MyUtils.Random.Next(moonWalkEvadeInstance.ExtraEvadeRange / 3, moonWalkEvadeInstance.ExtraEvadeRange)
                        : moonWalkEvadeInstance.ExtraEvadeRange;
                }
            }

            public bool Expired(int time = 4000)
            {
                return Elapsed(time);
            }

            public bool Elapsed(int time)
            {
                return Elapsed() > time;
            }

            public int Elapsed()
            {
                return Environment.TickCount - Time;
            }
        }
    }
}