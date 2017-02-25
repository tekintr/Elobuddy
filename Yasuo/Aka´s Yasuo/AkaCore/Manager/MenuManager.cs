﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Menu;
using AkaCore.AkaLib.Evade;

namespace AkaCore.Manager
{
    class MenuManager
    {
        private static Menu HMenu, ActivatorMenu, UtilityMenu, OrbwalkerMenu, FPS, HumanizerMenu;
        public static Menu EvadeMenu;


        public static void Load()
        {
            Hauptmenu();
            Activatormenu();
            Utilitymenu();
            Orbwalkermenu();
            Evademenu();
            FPSMenu();
            Humanizer();
        }

        private static void Hauptmenu()
        {
            HMenu = MainMenu.AddMenu("AkaCore", "akacdsore");
            HMenu.AddGroupLabel("Extension for my addons.");
        }

        private static void Activatormenu()
        {
            ActivatorMenu = HMenu.AddSubMenu("Activator", "Activator");
            ActivatorMenu.AddGroupLabel("Aggressive Items");
            ActivatorMenu.Add("AItems", new CheckBox("Kullan Items"));
            ActivatorMenu.Add("Botrk", new CheckBox("Kullan Botrk/Bilge"));
            ActivatorMenu.Add("Hydra", new CheckBox("Kullan Hydra/Tiamat"));
            ActivatorMenu.Add("Titanic", new CheckBox("Kullan Titanic"));
            ActivatorMenu.Add("HGB", new CheckBox("Kullan Hextech Silahkilic"));
            ActivatorMenu.Add("HGLP", new CheckBox("Kullan Hextech GLP"));
            ActivatorMenu.Add("HPB", new CheckBox("Kullan Hextech Kemeri"));
            ActivatorMenu.Add("Queens", new CheckBox("Kullan Buzdis"));
            ActivatorMenu.Add("QueensDis", new Slider("Dusman uzakligi =>", 1500, 0, 3000));
            ActivatorMenu.Add("Glory", new CheckBox("Kullan Gorkemli zafer"));
            ActivatorMenu.Add("GloryDis", new Slider("Dusman uzakligi =>", 600, 0, 1000));
            ActivatorMenu.Add("Talis", new CheckBox("Kullan Tilsim"));
            ActivatorMenu.Add("TalisDis", new Slider("Dusman uzakligi =>", 1000, 0, 2000));
            ActivatorMenu.Add("You", new CheckBox("Kullan HayaletKilic"));
            ActivatorMenu.AddGroupLabel("Defensive Items");
            ActivatorMenu.Add("DItems", new CheckBox("Kullan Items"));
            ActivatorMenu.Add("FaceMe", new CheckBox("Kullan Dagin sureti ben")); // The identifier tho kappa
            ActivatorMenu.Add("FaceMeHp", new Slider("If benim hp <=", 20, 0, 100));
            ActivatorMenu.Add("FaceAlly", new CheckBox("Kullan Dagin sureti Takim"));
            ActivatorMenu.Add("FaceAllyHp", new Slider("If Takim hp <=", 20, 0, 100));
            ActivatorMenu.Add("SolariMe", new CheckBox("Kullan Demir solari ben"));
            ActivatorMenu.Add("SolariMeHp", new Slider("If benim hp <=", 20, 0, 100));
            ActivatorMenu.Add("SolariAlly", new CheckBox("Kullan Demir solari Takim"));
            ActivatorMenu.Add("SolariAllyHp", new Slider("If Takim hp <=", 20, 0, 100));
            ActivatorMenu.Add("Omen", new CheckBox("Kullan Randuni"));
            ActivatorMenu.Add("OmenHp", new Slider("If benim hp <=", 20, 0, 100));
            ActivatorMenu.Add("Seraphs", new CheckBox("Kullan Seraph"));
            ActivatorMenu.Add("SeraphsHp", new Slider("If benim hp <=", 20, 0, 100));
            ActivatorMenu.Add("Talis2", new CheckBox("Kullan Talisman"));
            ActivatorMenu.Add("Talis2Hp", new Slider("If benim hp <=", 20, 0, 100));
            ActivatorMenu.Add("Glory2", new CheckBox("Kullan Gorkemli zafer"));
            ActivatorMenu.Add("Glory2Hp", new Slider("If benim hp <=", 20, 0, 100));
            ActivatorMenu.Add("Zhonyas", new CheckBox("Kullan zonya"));
            ActivatorMenu.Add("ZhonyasHp", new Slider("If benim hp <=", 20, 0, 100));
            ActivatorMenu.AddGroupLabel("Pots");
            ActivatorMenu.Add("PItems", new CheckBox("Kullan iksir"));
            ActivatorMenu.Add("HPPot", new CheckBox("Kullan saglik iksiri"));
            ActivatorMenu.Add("HPPotHp", new Slider("If benim hp <=", 40, 0, 100));
            ActivatorMenu.Add("Biscuit", new CheckBox("Use Biscuits"));
            ActivatorMenu.Add("BiscuitHp", new Slider("If benim hp <=", 40, 0, 100));
            ActivatorMenu.Add("RefillPot", new CheckBox("Kullan yenilenebilir iksir"));
            ActivatorMenu.Add("RefillPotHp", new Slider("If benim hp <=", 60, 0, 100));
            ActivatorMenu.Add("HunterPot", new CheckBox("Kullan avci iksiri"));
            ActivatorMenu.Add("HunterPotHp", new Slider("If benim hp <=", 60, 0, 100));
            ActivatorMenu.Add("CorruptPot", new CheckBox("Kullan musubet iksiri"));
            ActivatorMenu.Add("CourrptPotHp", new Slider("If benim hp <=", 60, 0, 100));
            ActivatorMenu.AddGroupLabel("Summoners");
            ActivatorMenu.Add("Heal", new CheckBox("Sifa"));
            ActivatorMenu.Add("HealHp", new Slider("Sifa if benim HP <=", 20, 0, 100));
            ActivatorMenu.Add("HealAlly", new CheckBox("Sifa Takim"));
            ActivatorMenu.Add("HealAllyHp", new Slider("Sifa if Takim HP <=", 20, 0, 100));
            ActivatorMenu.Add("Barrier", new CheckBox("Bariyer"));
            ActivatorMenu.Add("BarrierHp", new Slider("Bariyer if benim HP <=", 20, 0, 100));
            ActivatorMenu.Add("Ignite", new CheckBox("Kullan Tutustur"));
            ActivatorMenu.Add("IgniteHp", new Slider("0 KS otherwise if Hp <=", 0, 0, 100));
            ActivatorMenu.Add("Exhaust", new CheckBox("Kullan bitkinlik"));
            ActivatorMenu.Add("ExhaustHp", new Slider("bitkinlik if dusman Hp <=", 40, 0, 100));
            ActivatorMenu.AddGroupLabel("Smite");
            ActivatorMenu.Add("SmiteActive", new KeyBind("Carp aktif", true, KeyBind.BindTypes.PressToggle));
            ActivatorMenu.Add("SStatus", new CheckBox("Goster Durum"));
            ActivatorMenu.Add("SDamage", new CheckBox("Goster Hasar"));
            ActivatorMenu.Add("SBaron", new CheckBox("Baron"));
            ActivatorMenu.Add("SHerald", new CheckBox("Alamet"));
            ActivatorMenu.Add("SWDragon", new CheckBox("Su Dragon"));
            ActivatorMenu.Add("SFDragon", new CheckBox("Alev Dragon"));
            ActivatorMenu.Add("SEDragon", new CheckBox("Dag Dragon"));
            ActivatorMenu.Add("SADragon", new CheckBox("Bulut Dragon"));
            ActivatorMenu.Add("SElder", new CheckBox("Kadim Dragon"));
            ActivatorMenu.Add("SBlue", new CheckBox("Mavi Buff", false));
            ActivatorMenu.Add("SRed", new CheckBox("Kirmisi Buff", false));
            ActivatorMenu.Add("SRaptor", new CheckBox("Tavuklarin Camp", false));
            ActivatorMenu.Add("SWolf", new CheckBox("Kurt Camp", false));
            ActivatorMenu.Add("SGromp", new CheckBox("Gromp Camp", false));
            ActivatorMenu.Add("SKrug", new CheckBox("Kayacil Camp", false));
            ActivatorMenu.Add("SCrap", new CheckBox("Yengec", false));
            ActivatorMenu.AddGroupLabel("Cleanse");
            ActivatorMenu.Add("Qss", new CheckBox("Kullan Qss"));
            ActivatorMenu.Add("Mecurial", new CheckBox("Kullan Mecurial"));
            ActivatorMenu.Add("Cleanser", new CheckBox("Kullan civali"));
            ActivatorMenu.Add("QssDelay", new Slider("Delay", 100, 0, 2000));
            ActivatorMenu.Add("Blind",
                new CheckBox("Kor Etme", false));
            ActivatorMenu.Add("Charm",
                new CheckBox("Buyu"));
            ActivatorMenu.Add("Fear",
                new CheckBox("Korkutma"));
            ActivatorMenu.Add("Polymorph",
                new CheckBox("Polymorph"));
            ActivatorMenu.Add("Stun",
                new CheckBox("Sersemleme"));
            ActivatorMenu.Add("Snare",
                new CheckBox("Kapan"));
            ActivatorMenu.Add("Silence",
                new CheckBox("Susturma", false));
            ActivatorMenu.Add("Taunt",
                new CheckBox("Taunt"));
            ActivatorMenu.Add("Suppression",
                new CheckBox("Durdurma"));
            ActivatorMenu.Add("Knockback",
                new CheckBox("Geriatma"));
            ActivatorMenu.Add("Knockup",
                new CheckBox("ileriatma"));
        }

        private static void Utilitymenu()
        {
            UtilityMenu = HMenu.AddSubMenu("Utility", "kappa");
            UtilityMenu.AddGroupLabel("Utility");
            UtilityMenu.Add("Skinhack", new CheckBox("Skin secici aktif", false));
            UtilityMenu.Add("SkinID", new Slider("SkinID", 0, 0, 15));
            UtilityMenu.Add("Autolvl", new CheckBox("Oto level aktif"));
            UtilityMenu.Add("AutolvlS", new ComboBox("Level Mode", 0, "Max Q", "Max W", "Max E"));
            UtilityMenu.Add("Autobuy", new CheckBox("Oto Baslangic"));
            UtilityMenu.Add("AutobuyS", new ComboBox("Starter Item", 0, "Dorans Kilici", "Dorans yuzugu", "Dorans kalkani", "Corrupting Potion", "Avci Palasi", "Avci tilsimi", "Ancient Coin", "SpellThiefs", "Relic Shield"));
            UtilityMenu.Add("Autobuyt", new CheckBox("Totem yukseltme"));
            UtilityMenu.Add("AutobuytS", new ComboBox("Upgrade to", 0, "UzakGorus Alteration", "Tarayici Alteration"));
            UtilityMenu.Add("Autolantern", new CheckBox("Otomatik Fener"));
            UtilityMenu.Add("AutolanternHP", new Slider("Otomatik Fener if Hp =>", 40));
        }

        private static void Orbwalkermenu()
        {
            OrbwalkerMenu = HMenu.AddSubMenu("Orbwalk", "asdasf");
            OrbwalkerMenu.AddGroupLabel("Orbwalker Extensions");
            if (ObjectManager.Player.ChampionName == "Draven")
            {
                OrbwalkerMenu.AddGroupLabel("Auto Catch");
                OrbwalkerMenu.Add("Qmode", new ComboBox("Axe Catch Mode", 1, "Combo", "Always", "Never"));
                OrbwalkerMenu.Add("Qrange", new Slider("Catch Axe Range:", 800, 120, 1500));
                OrbwalkerMenu.Add("WforQ", new CheckBox("Use W if axe to far away"));
                OrbwalkerMenu.AddGroupLabel("Safety Options");
                OrbwalkerMenu.Add("Qturret", new CheckBox("Don´t catch under turret"));
                OrbwalkerMenu.Add("Qenemies", new CheckBox("Don´t catch in enemy group"));
                OrbwalkerMenu.Add("Qkill", new CheckBox("Don´t catch if can kill"));
                OrbwalkerMenu.Add("Qmelee", new CheckBox("Don´t catch near melees"));
                OrbwalkerMenu.AddGroupLabel("Drawings");
                OrbwalkerMenu.Add("DrawAxe", new CheckBox("Draw Axe"));
                OrbwalkerMenu.Add("DrawAxeRange", new CheckBox("Draw Axe catch Range"));
            }
            else
            {
                OrbwalkerMenu.AddGroupLabel("Play Draven to have perfect Auto Catch");
                OrbwalkerMenu.AddGroupLabel("Animation Cancel");
                OrbwalkerMenu.AddLabel("Note: Obvious - Works good with Vayne/Riven");
                OrbwalkerMenu.Add("Spell1", new CheckBox("Cancel Q Animation", false));
                OrbwalkerMenu.Add("Spell2", new CheckBox("Cancel W Animation", false));
                OrbwalkerMenu.Add("Spell3", new CheckBox("Cancel E Animation", false));
                OrbwalkerMenu.Add("Spell4", new CheckBox("Cancel R Animation", false));
            }
        }

        private static void Evademenu()
        {
            //Attention: Spaghetti Code inc kappa

            EvadeManager.EvadeSkillshot.Init();
            EvadeManager.EvadeTarget.Init();
            
            EvadeMenu = HMenu.AddSubMenu("Evade", "asddsf");
            {
                EvadeMenu.AddGroupLabel("Evade");
                if (ObjectManager.Player.ChampionName == "Yasuo" || ObjectManager.Player.ChampionName == "Vayne")
                {
                    EvadeMenu.AddGroupLabel("My Spells");
                    foreach (var spell in EvadeSpellDatabase.Spells)
                    {
                        EvadeMenu.AddGroupLabel(spell.Name);
                        {
                            if (spell.Name == "YasuoDashWrapper")
                            {
                                EvadeMenu.Add("ETower", new CheckBox("Under Tower", false));
                            }
                            else if (spell.Name == "YasuoWMovingWall")
                            {
                                EvadeMenu.Add("WDelay", new Slider("Extra Delay", 100, 0, 150));
                            }
                            EvadeMenu.Add(spell.Name + "DangerLevelY", new Slider("If Danger Level >=", 2, 1, 5));
                            EvadeMenu.Add(spell.Name + "EnabledY", new CheckBox("Enabled"));
                        }
                        MyDLVL = EvadeMenu[spell.Name + "DangerLevelY"];
                        MyEnabled = EvadeMenu[spell.Name + "EnabledY"];
                    }
                    EvadeMenu.AddGroupLabel("Enemy Spells");
                    foreach (var spell in
                        SpellDatabase.Spells.Where(i => EntityManager.Heroes.Enemies.Any(a => a.ChampionName == i.ChampionName)))
                    {
                        EvadeMenu.AddGroupLabel(spell.SpellName);
                        {
                            EvadeMenu.Add(spell.SpellName + "DangerLevel", new Slider("Danger Level", spell.DangerValue, 1, 5));
                            EvadeMenu.Add(spell.SpellName + "Enabled", new CheckBox("Enabled", !spell.DisabledByDefault));
                        }
                        EEnabled = EvadeMenu[spell.SpellName + "Enabled"];
                        EDLVL = EvadeMenu[spell.SpellName + "DangerLevel"];
                    }
                    if (ObjectManager.Player.ChampionName == "Yasuo")
                    {
                        EvadeMenu.AddGroupLabel("Evade Target"); {
                            EvadeMenu.Add("W", new CheckBox("Use W")); //                                    evadeSpells.Add("ETower", new CheckBox("Under Tower", false));
                            EvadeMenu.Add("E", new CheckBox("Use E (To Dash Behind WindWall)"));
                            EvadeMenu.Add("ETower", new CheckBox("-> Under Tower", false));
                            EvadeMenu.Add("BAttack", new CheckBox("Basic Attack"));
                            EvadeMenu.Add("BAttackHpU", new Slider("-> If Hp <", 35));
                            EvadeMenu.Add("CAttack", new CheckBox("Crit Attack"));
                            EvadeMenu.Add("CAttackHpU", new Slider("-> If Hp <", 40));
                            EvadeMenu.AddGroupLabel("Evade Point to Click");
                            foreach (
         var spell in EvadeManager.EvadeTarget.Spells.Where(i => EntityManager.Heroes.Enemies.Any(a => a.ChampionName == i.ChampionName)))
                            {
                                EvadeMenu.Add(spell.MissileName, new CheckBox(
                                    spell.MissileName + " (" + spell.Slot + ")",
                                    false));
                            }
                        }
                    }
                }
                else
                {
                    EvadeMenu.AddGroupLabel("Champ not supported yet");
                }
            }
            
        }

        private static void FPSMenu()
        {
            FPS = HMenu.AddSubMenu("FPS Protection", "asd");
            //FPS.AddGroupLabel("This is only working with my addons :/");
            
            FPS.Add("minfps", new Slider("Min Fps", 45, 1, 350));
            FPS.Add("calcps", new Slider("Calculations per Sec", 35, 1, 350));
            FPS.Add("enablefps", new CheckBox("Enable FPS Protection"));
        }

        private static void Humanizer()
        {
            HumanizerMenu = HMenu.AddSubMenu("Humanizer", "hu3");
            HumanizerMenu.AddGroupLabel("Humanize Spells >.<");
            HumanizerMenu.AddLabel("Ask me if you need more options.");
            HumanizerMenu.Add("HQ", new Slider("Humanize Q", 0, 0, 400));
            HumanizerMenu.Add("HW", new Slider("Humanize W", 0, 0, 400));
            HumanizerMenu.Add("HE", new Slider("Humanize E", 0, 0, 400));
            HumanizerMenu.Add("HR", new Slider("Humanize R", 0, 0, 400));
            HumanizerMenu.Add("Humanize", new CheckBox("Enable Humanzier", false));
        }

        #region checkvalues
        #region checkvalues:activator
        //pots
        public static bool HPPot
        {
            get { return (ActivatorMenu["HPPot"].Cast<CheckBox>().CurrentValue); }
        }

        public static int HPPotHp
        {
            get { return (ActivatorMenu["HPPotHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool Biscuit
        {
            get { return (ActivatorMenu["Biscuit"].Cast<CheckBox>().CurrentValue); }
        }

        public static int BiscuitHp
        {
            get { return (ActivatorMenu["BiscuitHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool RefillPot
        {
            get { return (ActivatorMenu["RefillPot"].Cast<CheckBox>().CurrentValue); }
        }

        public static int RefillPotHp
        {
            get { return (ActivatorMenu["RefillPotHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool HunterPot
        {
            get { return (ActivatorMenu["HunterPot"].Cast<CheckBox>().CurrentValue); }
        }

        public static int HunterPotHp
        {
            get { return (ActivatorMenu["HunterPotHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool CorruptPot
        {
            get { return (ActivatorMenu["CorruptPot"].Cast<CheckBox>().CurrentValue); }
        }

        public static int CorruptPotHp
        {
            get { return (ActivatorMenu["CorruptPotHp"].Cast<Slider>().CurrentValue); }
        }
        //Aggressiveive
        public static bool Botrk
        {
            get { return (ActivatorMenu["Botrk"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool Hextech
        {
            get { return (ActivatorMenu["HGB"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool HextechGLP
        {
            get { return (ActivatorMenu["HGLP"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool HextechPB
        {
            get { return (ActivatorMenu["HPB"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool Hydra
        {
            get { return (ActivatorMenu["Hydra"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool Titanic
        {
            get { return (ActivatorMenu["Titanic"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool Queens
        {
            get { return (ActivatorMenu["Queens"].Cast<CheckBox>().CurrentValue); }
        }

        public static int QueensDistance
        {
            get { return (ActivatorMenu["QueensDis"].Cast<Slider>().CurrentValue); }
        }

        public static bool Glory
        {
            get { return (ActivatorMenu["Glory"].Cast<CheckBox>().CurrentValue); }
        }

        public static int GloryDistance
        {
            get { return (ActivatorMenu["GloryDis"].Cast<Slider>().CurrentValue); }
        }

        public static bool Talis
        {
            get { return (ActivatorMenu["Talis"].Cast<CheckBox>().CurrentValue); }
        }

        public static int TalisDistance
        {
            get { return (ActivatorMenu["TalisDis"].Cast<Slider>().CurrentValue); }
        }

        public static bool You
        {
            get { return (ActivatorMenu["You"].Cast<CheckBox>().CurrentValue); }
        }
        //Defensive
        public static bool MountainMe
        {
            get { return (ActivatorMenu["FaceMe"].Cast<CheckBox>().CurrentValue); }
        }

        public static int MountainMeHp
        {
            get { return (ActivatorMenu["FaceMeHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool MountainAlly
        {
            get { return (ActivatorMenu["FaceAlly"].Cast<CheckBox>().CurrentValue); }
        }

        public static int MoutaiAllyHp
        {
            get { return (ActivatorMenu["FaceAllyHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool SolariMe
        {
            get { return (ActivatorMenu["SolariMe"].Cast<CheckBox>().CurrentValue); }
        }

        public static int SolraiMeHp
        {
            get { return (ActivatorMenu["SolariMeHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool SolariAlly
        {
            get { return (ActivatorMenu["SolariAlly"].Cast<CheckBox>().CurrentValue); }
        }

        public static int SolraiAllyHp
        {
            get { return (ActivatorMenu["SolariAllyHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool Omen
        {
            get { return (ActivatorMenu["Omen"].Cast<CheckBox>().CurrentValue); }
        }

        public static int OmenHp
        {
            get { return (ActivatorMenu["OmenHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool Seraphs
        {
            get { return (ActivatorMenu["Seraphs"].Cast<CheckBox>().CurrentValue); }
        }

        public static int SeraphsHp
        {
            get { return (ActivatorMenu["SerahpsHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool TalisDef
        {
            get { return (ActivatorMenu["Talis2"].Cast<CheckBox>().CurrentValue); }
        }

        public static int TalisDefHp
        {
            get { return (ActivatorMenu["Talis2Hp"].Cast<Slider>().CurrentValue); }
        }

        public static bool GloryDef
        {
            get { return (ActivatorMenu["Glory2"].Cast<CheckBox>().CurrentValue); }
        }

        public static int GloryDefHp
        {
            get { return (ActivatorMenu["Glory2Hp"].Cast<Slider>().CurrentValue); }
        }

        public static bool Zhonyas
        {
            get { return (ActivatorMenu["Zhonyas"].Cast<CheckBox>().CurrentValue); }
        }

        public static int ZhonyasHp
        {
            get { return (ActivatorMenu["ZhonyasHp"].Cast<Slider>().CurrentValue); }
        }

        //Summoososos
        public static bool Heal
        {
            get { return (ActivatorMenu["Heal"].Cast<CheckBox>().CurrentValue); }
        }

        public static int HealHp
        {
            get { return (ActivatorMenu["HealHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool Barrier
        {
            get { return (ActivatorMenu["Barrier"].Cast<CheckBox>().CurrentValue); }
        }

        public static int BarrierHp
        {
            get { return (ActivatorMenu["BarrierHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool HealAlly
        {
            get { return (ActivatorMenu["HealAlly"].Cast<CheckBox>().CurrentValue); }
        }

        public static int HealAllyHp
        {
            get { return (ActivatorMenu["HealAllyHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool Ignite
        {
            get { return (ActivatorMenu["Ignite"].Cast<CheckBox>().CurrentValue); }
        }

        public static int IgniteHp
        {
            get { return (ActivatorMenu["IgniteHp"].Cast<Slider>().CurrentValue); }
        }

        public static bool Exhaust
        {
            get { return (ActivatorMenu["Exhaust"].Cast<CheckBox>().CurrentValue); }
        }

        public static int ExhaustHp
        {
            get { return (ActivatorMenu["ExhaustHp"].Cast<Slider>().CurrentValue); }
        }
        //smite
        public static bool SmiteActive
        {
            get { return (ActivatorMenu["SmiteActive"].Cast<KeyBind>().CurrentValue); }
        }
        public static bool SStatus
        {
            get { return (ActivatorMenu["SStatus"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SDamage
        {
            get { return (ActivatorMenu["SDamage"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SBaron
        {
            get { return (ActivatorMenu["SBaron"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SHerald
        {
            get { return (ActivatorMenu["SHerald"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SWDragon
        {
            get { return (ActivatorMenu["SWDragon"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SEDragon
        {
            get { return (ActivatorMenu["SEDragon"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SADragon
        {
            get { return (ActivatorMenu["SADragon"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SFDragon
        {
            get { return (ActivatorMenu["SFDragon"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool Elder
        {
            get { return (ActivatorMenu["SElder"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SBlue
        {
            get { return (ActivatorMenu["SBlue"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SRaptor
        {
            get { return (ActivatorMenu["SRaptor"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SRed
        {
            get { return (ActivatorMenu["SRed"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SWolf
        {
            get { return (ActivatorMenu["SWolf"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SKrug
        {
            get { return (ActivatorMenu["SKrug"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SGromp
        {
            get { return (ActivatorMenu["SGromp"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool SGrap
        {
            get { return (ActivatorMenu["SCrap"].Cast<CheckBox>().CurrentValue); }
        }
        //Qssss
        public static bool Qss
        {
            get { return (ActivatorMenu["Qss"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool Mecurial
        {
            get { return (ActivatorMenu["Mecurial"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool Cleanse
        {
            get { return (ActivatorMenu["Cleanser"].Cast<CheckBox>().CurrentValue); }
        }

        public static int QssDelay
        {
            get { return (ActivatorMenu["QssDelay"].Cast<Slider>().CurrentValue); }
        }

        public static bool QssBlind
        {
            get { return (ActivatorMenu["Blind"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool QssCharm
        {
            get { return (ActivatorMenu["Charm"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool QssFear
        {
            get { return (ActivatorMenu["Fear"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool QssPolymorph
        {
            get { return (ActivatorMenu["Polymorph"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool QssStun
        {
            get { return (ActivatorMenu["Stun"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool QssSnare
        {
            get { return (ActivatorMenu["Snare"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool QssSilence
        {
            get { return (ActivatorMenu["Silence"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool QssTaunt
        {
            get { return (ActivatorMenu["Taunt"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool QssSupression
        {
            get { return (ActivatorMenu["Suppression"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool QssKnockback
        {
            get { return (ActivatorMenu["Knockback"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool QssKnockup
        {
            get { return (ActivatorMenu["Knockup"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool AItems
        {
            get { return (ActivatorMenu["AItems"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool DItems
        {
            get { return (ActivatorMenu["DItems"].Cast<CheckBox>().CurrentValue); }
        }

        public static bool PItems
        {
            get { return (ActivatorMenu["PItems"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #region checkvalues:utility
        public static bool Skinhack
        {
            get { return (UtilityMenu["Skinhack"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool Autolvl
        {
            get { return (UtilityMenu["Autolvl"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool Autobuy
        {
            get { return (UtilityMenu["Autobuy"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool Autobuytrinkets
        {
            get { return (UtilityMenu["Autobuyt"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool Autolantern
        {
            get { return (UtilityMenu["Autolantern"].Cast<CheckBox>().CurrentValue); }
        }
        public static int SkinID
        {
            get { return (UtilityMenu["SkinID"].Cast<Slider>().CurrentValue); }
        }
        public static int AutolvlSlider
        {
            get { return (UtilityMenu["AutolvlS"].Cast<ComboBox>().CurrentValue); }
        }
        public static int AutobuySlider
        {
            get { return (UtilityMenu["AutobuyS"].Cast<ComboBox>().CurrentValue); }
        }
        public static int AutobuytrinketsSlider
        {
            get { return (UtilityMenu["AutobuytS"].Cast<ComboBox>().CurrentValue); }
        }
        public static int AutlanternHp
        {
            get { return (UtilityMenu["AutolanternHP"].Cast<Slider>().CurrentValue); }
        }
        #endregion
        #region checkvalues:orbwalk
        #region autocatch
        #region Axe
        public static int AxeMode
        {
            get { return (OrbwalkerMenu["Qmode"].Cast<ComboBox>().CurrentValue); }
        }
        public static int AxeCatchRange
        {
            get { return (OrbwalkerMenu["Qrange"].Cast<Slider>().CurrentValue); }
        }
        public static bool AxeW
        {
            get { return (OrbwalkerMenu["WforQ"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool DrawAxe
        {
            get { return (OrbwalkerMenu["DrawAxe"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool DrawAxeCatchRange
        {
            get { return (OrbwalkerMenu["DrawAxeRange"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #region safety
        public static bool CatchTower
        {
            get { return (OrbwalkerMenu["Qturret"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool CatchEnemies
        {
            get { return (OrbwalkerMenu["Qenemies"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool CatchKill
        {
            get { return (OrbwalkerMenu["Qkill"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool CatchMelees
        {
            get { return (OrbwalkerMenu["Qmelee"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #endregion
        #region Animationcancel
        public static bool AnimationCancelQ
        {
            get { return (OrbwalkerMenu["Spell1"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool AnimationCancelW
        {
            get { return (OrbwalkerMenu["Spell2"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool AnimationCancelE
        {
            get { return (OrbwalkerMenu["Spell3"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool AnimationCancelR
        {
            get { return (OrbwalkerMenu["Spell4"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #endregion
        #region checkvalues:fps

        public static int MinFps
        {
            get { return (FPS["minfps"].Cast<Slider>().CurrentValue); }
        }

        public static int CalcPerSecond
        {
            get { return (FPS["calcps"].Cast<Slider>().CurrentValue); }
        }

        public static bool EnableFPS
        {
            get { return (FPS["enablefps"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #region checkvalus:evade
        public static bool EvadeETower
        {
            get { return (EvadeMenu["ETower"].Cast<CheckBox>().CurrentValue); }
        }
        public static int EvadeWDelay
        {
            get { return (EvadeMenu["WDelay"].Cast<Slider>().CurrentValue); }
        }
        public static ValueBase MyEnabled;
        public static ValueBase MyDLVL;
        public static ValueBase EEnabled;
        public static ValueBase EDLVL;
        public static ValueBase PointtoClick;
        #endregion
        #region checkvalues:humanizer
        public static int HumanizeQ
        {
            get { return (HumanizerMenu["HQ"].Cast<Slider>().CurrentValue); }
        }
        public static int HumanizeW
        {
            get { return (HumanizerMenu["HW"].Cast<Slider>().CurrentValue); }
        }
        public static int HumanizeE
        {
            get { return (HumanizerMenu["HE"].Cast<Slider>().CurrentValue); }
        }
        public static int HumanizeR
        {
            get { return (HumanizerMenu["HR"].Cast<Slider>().CurrentValue); }
        }
        public static bool EnableH
        {
            get { return (HumanizerMenu["Humanize"].Cast<CheckBox>().CurrentValue); }
        }
        #endregion
        #endregion
    }
}
