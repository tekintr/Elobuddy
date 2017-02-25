using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using GuTenTak.Ezreal;
using SharpDX;

namespace GuTenTak.Ezreal
{
    internal class Program
    {
        public const string ChampionName = "Ezreal";
        public static Menu Menu, ModesMenu1, ModesMenu2, ModesMenu3, DrawMenu;
        //public static int SkinBase;
        public static Item Youmuu = new Item(ItemId.Youmuus_Ghostblade);
        public static Item Botrk = new Item(ItemId.Blade_of_the_Ruined_King);
        public static Item Cutlass = new Item(ItemId.Bilgewater_Cutlass);
        public static Item Tear = new Item(ItemId.Tear_of_the_Goddess);
        public static Item Manamune = new Item(ItemId.Manamune);
        public static Item Archangel = new Item(ItemId.Archangels_Staff);
        public static Item Qss = new Item(ItemId.Quicksilver_Sash);
        public static Item Simitar = new Item(ItemId.Mercurial_Scimitar);
        public static Item hextech = new Item(ItemId.Hextech_Gunblade, 700);
        public static string[] herogapcloser =
        {
            "Braum", "Ekko", "Elise", "Fiora", "Kindred", "Lucian", "Yi", "Nidalee", "Quinn", "Riven", "Shaco", "Sion", "Vayne", "Yasuo", "Graves", "Azir", "Gnar", "Irelia", "Kalista"
        };
        /*public static AIHeroClient PlayerInstance
        {
            get { return Player.Instance; }
        }
        private static float HealthPercent()
        {
            return (Player.Instance.Health / Player.Instance.MaxHealth) * 100;
        }

        public static AIHeroClient _Player
        {
            get { return Player.Instance; }
        }

        public static bool AutoQ { get; protected set; }
        public static float Manaah { get; protected set; }
        public static object GameEvent { get; private set; }
        public static int LastTick = 0;
        public static IOrderedEnumerable<Obj_AI_Turret> enemyTurret=null;*/
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }


        static void Game_OnStart(EventArgs args)
        {
            if (ChampionName != Player.Instance.BaseSkinName)
            {
                return;
            }
                
            Game.OnUpdate += Game_OnUpdate;
            /*Drawing.OnDraw += Game_OnDraw;
            Obj_AI_Base.OnBuffGain += Common.OnBuffGain;
            Gapcloser.OnGapcloser += Common.Gapcloser_OnGapCloser;
            Game.OnTick += Common.ItemUsage;
            SkinBase = Player.Instance.SkinId;*/
            try
            {
                Q = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, 2000, 60);
                Q.AllowedCollisionCount = 0;
                W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Linear, 250, 1600, 80);
                W.AllowedCollisionCount = int.MaxValue;
                E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear);
                E.AllowedCollisionCount = int.MaxValue;
                R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 1000, 2000, 160);
                R.AllowedCollisionCount = int.MaxValue;
                
                Bootstrap.Init(null);
                Chat.Print("GuTenTak Ezrael yuklendi.Keyifli oyunlar.Ceviri TekinTR", Color.Green);
                
                Menu = MainMenu.AddMenu("GuTenTak Ezreal", "Ezreal");
                Menu.AddSeparator();
                Menu.AddLabel("GuTenTak Ezreal Addon");

                //var Enemies = EntityManager.Heroes.Enemies.Where(a => !a.IsMe).OrderBy(a => a.BaseSkinName);
                ModesMenu1 = Menu.AddSubMenu("Menu", "Modes1Ezreal");
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Combo Configs");
                ModesMenu1.Add("ComboQ", new CheckBox("Komboda Q kullan", true));
                ModesMenu1.Add("ComboA", new CheckBox("Kullan AA => Q Kombosu", false));
                ModesMenu1.Add("ComboW", new CheckBox("Komboda W kullan", true));
                ModesMenu1.Add("ComboR", new CheckBox("Komboda R kullan", true));
                ModesMenu1.Add("ManaCW", new Slider("W icin mana %", 30));
                ModesMenu1.Add("RCount", new Slider("R kac dusmana carpsin >=", 3, 2, 5));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("AutoHarass Configs");
                ModesMenu1.Add("AutoHarass", new CheckBox("Oto durtmede Q kullan", false));

                ModesMenu1.Add("ManaAuto", new Slider("Mana %", 80));
                ModesMenu1.AddLabel("Harass Configs");
                ModesMenu1.Add("HarassQ", new CheckBox("Durterken Q kullan", true));
                ModesMenu1.Add("ManaHQ", new Slider("Mana %", 40));
                ModesMenu1.Add("HarassW", new CheckBox("Durterken W kullan", true));
                ModesMenu1.Add("ManaHW", new Slider("Mana %", 60));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Kill Steal Configs");
                ModesMenu1.Add("KS", new CheckBox("Kullan KillSteal", true));
                ModesMenu1.Add("KQ", new CheckBox("Q ile oldur", true));
                ModesMenu1.Add("KW", new CheckBox("W ile oldur", true));
                ModesMenu1.Add("KR", new CheckBox("R ile oldur", true));

                ModesMenu2 = Menu.AddSubMenu("Farm", "Modes2Ezreal");
                ModesMenu2.AddLabel("LastHit Configs");
                ModesMenu2.Add("ManaF", new Slider("Mana %", 60));
                ModesMenu2.Add("LastQ", new CheckBox("Minyona son vurus Q ile", true));
                ModesMenu2.AddLabel("Lane Clear Config");
                ModesMenu2.Add("ManaL", new Slider("Mana %", 40));
                ModesMenu2.Add("FarmQ", new CheckBox("Q ile koridor ittir", true));
                ModesMenu2.AddLabel("Jungle Clear Config");
                ModesMenu2.Add("ManaJ", new Slider("Mana %", 40));
                ModesMenu2.Add("JungleQ", new CheckBox("Q ile orman temizle", true));

                ModesMenu3 = Menu.AddSubMenu("Misc", "Modes3Ezreal");
                ModesMenu3.AddLabel("Misc Configs");
                ModesMenu3.Add("AntiGap", new CheckBox("Atilma yapanlara E kullan", true));
                ModesMenu3.Add("StackTear", new CheckBox("Otomatik gozyasi kas", true));
                ModesMenu3.AddLabel("Flee Configs");
                ModesMenu3.Add("FleeQ", new CheckBox("Kacarken Q kullan", true));
                ModesMenu3.Add("FleeOQ", new CheckBox("Kacarken Q yu sadece sampiyonlara", true));
                ModesMenu3.Add("FleeE", new CheckBox("Kacarkern E kullan", true));
                //ModesMenu3.Add("BlockE", new CheckBox("Block EnemyUnderTurret", false));
                ModesMenu3.Add("ManaFlQ", new Slider("Q Mana %", 35));

                ModesMenu3.AddLabel("Item Usage on Combo");
                ModesMenu3.Add("useItems", new CheckBox("Item Kullan", true));
                ModesMenu3.AddSeparator(1);
                ModesMenu3.Add("useYoumuu", new CheckBox("Kullan Youmuu", true));
                ModesMenu3.Add("usehextech", new CheckBox("Kullan Hextech", true));
                ModesMenu3.Add("useBotrk", new CheckBox("Kullan Botrk & Cutlass", true));
                ModesMenu3.Add("minHPBotrk", new Slider("Canim sundan dusukse kullan Botrk %", 80));
                ModesMenu3.Add("enemyMinHPBotrk", new Slider("Dusmanin cani sundan dusukse kullan Botrk %", 80));

                ModesMenu3.AddLabel("QSS Configs");
                ModesMenu3.Add("useQss", new CheckBox("Kullan Civali", true));
                ModesMenu3.AddSeparator(1);
                ModesMenu3.Add("Qssmode", new ComboBox(" ", 0, "Auto", "Combo"));
                ModesMenu3.Add("Stun", new CheckBox("Stan", true));
                ModesMenu3.Add("Blind", new CheckBox("Kör", true));
                ModesMenu3.Add("Charm", new CheckBox("Ayartma", true));
                ModesMenu3.Add("Suppression", new CheckBox("Durdurma", true));
                ModesMenu3.Add("Polymorph", new CheckBox("Polymorph", true));
                ModesMenu3.Add("Fear", new CheckBox("Korku", true));
                ModesMenu3.Add("Taunt", new CheckBox("Taunt", true));
                ModesMenu3.Add("Silence", new CheckBox("Susturma", false));
                ModesMenu3.Add("QssDelay", new Slider("Use QSS Delay(ms)", 250, 0, 1000));

                ModesMenu3.AddLabel("QSS Ult Configs");
                ModesMenu3.Add("ZedUlt", new CheckBox("Zed R", true));
                ModesMenu3.Add("VladUlt", new CheckBox("Vladimir R", true));
                ModesMenu3.Add("FizzUlt", new CheckBox("Fizz R", true));
                ModesMenu3.Add("MordUlt", new CheckBox("Mordekaiser R", true));
                ModesMenu3.Add("PoppyUlt", new CheckBox("Poppy R", true));
                ModesMenu3.Add("QssUltDelay", new Slider("Use QSS Delay(ms) for Ult", 250, 0, 1000));

                ModesMenu3.AddLabel("Skin Hack");
                ModesMenu3.Add("skinhack", new CheckBox("Skin secme aktif", false));
                ModesMenu3.Add("skinId", new ComboBox("Skin Mode", 0, "Default", "1", "2", "3", "4", "5", "6", "7", "8"));

                DrawMenu = Menu.AddSubMenu("Draws", "DrawEzreal");
                DrawMenu.Add("usedraw", new CheckBox("Gostergeler Aktif", true));
                DrawMenu.AddSeparator(1);
                DrawMenu.Add("drawQ", new CheckBox(" Goster Q", true));
                DrawMenu.Add("drawW", new CheckBox(" Goster W", true));
                DrawMenu.Add("drawR", new CheckBox(" Goster R", false));
                DrawMenu.Add("drawXR", new CheckBox(" R icin gosterge kullanma", true));
                DrawMenu.Add("drawXFleeQ", new CheckBox(" Q ile kacarken gosterge kullanma", false));

                if (ModesMenu3["useQss"].Cast<CheckBox>().CurrentValue)
                    Obj_AI_Base.OnBuffGain += Common.OnBuffGain;
                ModesMenu3["useQss"].Cast<CheckBox>().OnValueChange += (sender, vargs) =>
                {
                    if (vargs.NewValue)
                        Obj_AI_Base.OnBuffGain += Common.OnBuffGain;
                    else
                        Obj_AI_Base.OnBuffGain -= Common.OnBuffGain;
                };

                if (ModesMenu3["useItems"].Cast<CheckBox>().CurrentValue)
                    Game.OnTick += Common.ItemUsage;
                ModesMenu3["useItems"].Cast<CheckBox>().OnValueChange += (sender, vargs) =>
                {
                    if (vargs.NewValue)
                        Game.OnTick += Common.ItemUsage;
                    else
                        Game.OnTick -= Common.ItemUsage;
                };

                if (DrawMenu["usedraw"].Cast<CheckBox>().CurrentValue)
                    Drawing.OnDraw += Game_OnDraw;
                DrawMenu["usedraw"].Cast<CheckBox>().OnValueChange += (sender, vargs) =>
                {
                    if (vargs.NewValue)
                        Drawing.OnDraw += Game_OnDraw;
                    else
                        Drawing.OnDraw -= Game_OnDraw;
                };

                if (ModesMenu3["AntiGap"].Cast<CheckBox>().CurrentValue)
                    Gapcloser.OnGapcloser += Common.Gapcloser_OnGapCloser;
                ModesMenu3["AntiGap"].Cast<CheckBox>().OnValueChange += (sender, vargs) =>
                {
                    if (vargs.NewValue)
                        Gapcloser.OnGapcloser += Common.Gapcloser_OnGapCloser;
                    else
                        Gapcloser.OnGapcloser -= Common.Gapcloser_OnGapCloser;
                };

                if (ModesMenu1["KS"].Cast<CheckBox>().CurrentValue)
                    Game.OnTick += Common.KillSteal;
                ModesMenu1["KS"].Cast<CheckBox>().OnValueChange += (sender, vargs) =>
                {
                    if (vargs.NewValue)
                        Game.OnTick += Common.KillSteal;
                    else
                        Game.OnTick -= Common.KillSteal;
                };

                if (ModesMenu1["ComboA"].Cast<CheckBox>().CurrentValue)
                    Orbwalker.OnPostAttack += Common.Orbwalker_OnPostAttack;
                ModesMenu1["ComboA"].Cast<CheckBox>().OnValueChange += (sender, vargs) =>
                {
                    if (vargs.NewValue)
                        Orbwalker.OnPostAttack += Common.Orbwalker_OnPostAttack;
                    else
                        Orbwalker.OnPostAttack -= Common.Orbwalker_OnPostAttack;
                };

                if (ModesMenu3["StackTear"].Cast<CheckBox>().CurrentValue)
                    Game.OnTick += Common.StackTear;
                ModesMenu3["StackTear"].Cast<CheckBox>().OnValueChange += (sender, vargs) =>
                {
                    if (vargs.NewValue)
                        Game.OnTick += Common.StackTear;
                    else
                        Game.OnTick -= Common.StackTear;
                };

                if (ModesMenu3["skinhack"].Cast<CheckBox>().CurrentValue)
                    Player.SetSkinId(ModesMenu3["skinId"].Cast<ComboBox>().CurrentValue);
                ModesMenu3["skinId"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
                {
                    if (ModesMenu3["skinhack"].Cast<CheckBox>().CurrentValue)
                        Player.SetSkinId(vargs.NewValue);
                };
                ModesMenu3["skinhack"].Cast<CheckBox>().OnValueChange += (sender, vargs) =>
                {
                    if (vargs.NewValue)
                        Player.SetSkinId(ModesMenu3["skinId"].Cast<ComboBox>().CurrentValue);
                    else
                        Player.SetSkinId(0);
                };

                if (ModesMenu1["AutoHarass"].Cast<CheckBox>().CurrentValue)
                    Game.OnTick += Common.AutoQ;
                ModesMenu1["AutoHarass"].Cast<CheckBox>().OnValueChange += (sender, vargs) =>
                {
                    if (vargs.NewValue)
                        Game.OnTick += Common.AutoQ;
                    else
                        Game.OnTick -= Common.AutoQ;
                };
            }

            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }
        private static void Game_OnDraw(EventArgs args)
        {
            try
            {
                if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady())
                    {
                        Circle.Draw(Color.White, Q.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue)
                {
                    if (W.IsReady())
                    {
                        Circle.Draw(Color.White, W.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
                {
                    if (R.IsReady())
                    {
                        Circle.Draw(Color.White, R.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawXR"].Cast<CheckBox>().CurrentValue)
                {
                    if (R.IsReady())
                    {
                        Circle.Draw(Color.Red, 700, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawXFleeQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady())
                    {
                        Circle.Draw(Color.Red, 400, Player.Instance.Position);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        static void Game_OnUpdate(EventArgs args)
        {
            try
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    Common.Combo();
                    //Common.ItemUsage();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    Common.Harass();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                    Common.LaneClear();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    Common.JungleClear();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                {
                    Common.LastHit();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                {
                    Common.Flee();
                }
            }
            catch (Exception)
            {
                //Game.LuaDoString("");
            }
        }
        /*public static void OnTick(EventArgs args)
        {
            if (ModesMenu1["AutoHarass"].Cast<CheckBox>().CurrentValue)
            {
                Common.AutoQ();
            }
            if (ModesMenu1["ComboA"].Cast<CheckBox>().CurrentValue)
            {
                Orbwalker.OnPostAttack += Common.Orbwalker_OnPostAttack;
            }
            else
            {
                Orbwalker.OnPostAttack -= Common.Orbwalker_OnPostAttack;
            }
            Common.KillSteal();
            Common.StackTear();
            Common.Skinhack();
            if (ModesMenu3["BlockE"].Cast<CheckBox>().CurrentValue && Environment.TickCount - LastTick > 1500)
            {
                enemyTurret = ObjectManager.Get<Obj_AI_Turret>().Where(tur => tur.IsEnemy && tur.Health > 0)
                .OrderBy(tur => tur.Distance(Player.Instance.Position));
                LastTick = Environment.TickCount;
            }
        }*/
    }
}
