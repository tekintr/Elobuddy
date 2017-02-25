﻿using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace LelBlanc
{
    internal class Program
    {
        /// <summary>
        /// Position for Last W
        /// </summary>
        public static Vector3 LastWPosition { get; set; }

        /// <summary>
        /// Position for Last W Ultimate
        /// </summary>
        public static Vector3 LastWUltimatePosition { get; set; }

        /// <summary>
        /// Position for Last W End Position
        /// </summary>
        public static Vector3 LastWEndPosition { get; set; }

        /// <summary>
        /// Position for Last W End Position
        /// </summary>
        public static Vector3 LastWUltimateEndPosition { get; set; }

        /// <summary>
        /// Contains All Active Spells
        /// </summary>
        public static Spell.Active WReturn, RActive, RReturn;

        /// <summary>
        /// Contains All Targeted Spells
        /// </summary>
        public static Spell.Targeted Q, QUltimate, RTargeted, Ignite;

        /// <summary>
        /// Contains All Skillshots
        /// </summary>
        public static Spell.Skillshot W, E, WUltimate, EUltimate;

        /// <summary>
        /// Contains the DamageIndicator
        /// </summary>
        public static DamageIndicator Indicator;

        /// <summary>
        /// Contains the Color Picker
        /// </summary>
        public static ColorPicker Picker;

        /// <summary>
        /// Called when the Program is Initialized
        /// </summary>
        private static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        /// <summary>
        /// Called when the Game is finished loading
        /// </summary>
        /// <param name="args"></param>
        private static void Loading_OnLoadingComplete(System.EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Leblanc)
            {
                return;
            }

            Q = new Spell.Targeted(SpellSlot.Q, 700);

            W = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Circular, 0, 1600, 260)
            {
                AllowedCollisionCount = -1
            };

            WReturn = new Spell.Active(SpellSlot.W) {CastDelay = 250};

            RReturn = new Spell.Active(SpellSlot.R);

            E = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear, 250, 1750, 55)
            {
                AllowedCollisionCount = 0
            };

            QUltimate = new Spell.Targeted(SpellSlot.Q, 700);

            WUltimate = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Circular, 0, 1600, 250)
            {
                AllowedCollisionCount = -1
            };

            EUltimate = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear, 250, 1750, 55)
            {
                AllowedCollisionCount = 0
            };

            RActive = new Spell.Active(SpellSlot.R);

            RTargeted = new Spell.Targeted(SpellSlot.R, int.MaxValue);

            if (Extension.HasSpell("summonerdot"))
            {
                Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
                Chat.Print("LelBlanc: Ignite Loaded", System.Drawing.Color.Red);
            }

            Chat.Print("LelBlanc: Addon Yuklendi.Ceviri TekinTR GG!", System.Drawing.Color.Blue);

            // Methods
            Config.Initialize();

            // Constructors
            Picker = new ColorPicker(Config.DrawingMenu, "draw_", System.Drawing.Color.FromArgb(255, 255, 0, 0),
                "Color Settings for Damage Indicator");
            Indicator = new DamageIndicator();

            // Events
            Game.OnUpdate += Game_OnUpdate;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Obj_AI_Base.OnNewPath += Obj_AI_Base_OnNewPath;
            Drawing.OnDraw += OnDraw.DrawRange;
        }

        /// <summary>
        /// Called when a Object gets created
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="args">The Args</param>
        private static void GameObject_OnCreate(GameObject sender, System.EventArgs args)
        {
            if (sender.Name == Player.Instance.Name)
            {
                Pet.LeBlancPet = sender;
            }
        }

        /// <summary>
        /// Called when a Object gets deleted
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="args">The Args</param>
        private static void GameObject_OnDelete(GameObject sender, System.EventArgs args)
        {
            if (sender.Name == Player.Instance.Name)
            {
                Pet.LeBlancPet = null;
            }
        }

        /// <summary>
        /// Gets Player New Path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void Obj_AI_Base_OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (sender.IsMe)
            {
                var path = Player.Instance.Position.Extend(args.Path[0], 1000);
                var extendedPath = new Vector3(path, NavMesh.GetHeightForPosition(path.X, path.Y));
                Pet.NewPath = extendedPath;
            }
        }

        /// <summary>
        /// Called when Processing Spell Cast
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name.ToLower() == "leblancw")
            {
                if (Extension.IsUsingUlt)
                {
                    LastWUltimatePosition = args.Start;
                    LastWUltimateEndPosition = args.End;
                }
                else
                {
                    LastWPosition = args.Start;
                    LastWEndPosition = args.End;
                }
            }
        }

        /// <summary>
        /// Called whenever the game is being runned.
        /// </summary>
        /// <param name="args"></param>
        private static void Game_OnUpdate(System.EventArgs args)
        {
            if (Config.MiscMenu["pet"].Cast<CheckBox>().CurrentValue)
            {
                Pet.MovePet();
            }
            if (Config.KillStealMenu["toggle"].Cast<CheckBox>().CurrentValue)
            {
                if (!LastWEndPosition.IsZero &&
                    Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() == "leblancw")
                {
                    LastWEndPosition = Vector3.Zero;
                }
                if (!LastWUltimateEndPosition.IsZero &&
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).Name.ToLower() != "leblancrwreturn")
                {
                    LastWUltimatePosition = Vector3.Zero;
                }
                if (Modes.KillSteal.ResetW && Player.Instance.ServerPosition.IsInRange(LastWEndPosition, 100))
                {
                    Modes.KillSteal.ResetW = Extension.LogicReturn();

                    if (Modes.KillSteal.ResetW == false)
                        LastWEndPosition = Vector3.Zero;
                }
                if (Modes.KillSteal.ResetW && Player.Instance.ServerPosition.IsInRange(LastWUltimateEndPosition, 100))
                {
                    Modes.KillSteal.ResetW = Extension.LogicReturn(true);

                    if (Modes.KillSteal.ResetW == false)
                        LastWEndPosition = Vector3.Zero;
                }

                Modes.KillSteal.Execute();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Modes.Combo.Execute();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Modes.Harass.Execute();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Modes.LaneClear.Execute();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                //Modes.JungleClear.Execute();
            }
        }
    }
}