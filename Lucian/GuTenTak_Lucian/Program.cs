using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using GuTenTak.Lucian;
using SharpDX;
using EloBuddy.SDK.Constants;

namespace GuTenTak.Lucian
{
    internal class Program
    {
        public const string ChampionName = "Lucian";
        public static Menu Menu, ModesMenu1, ModesMenu2, ModesMenu3, DrawMenu;
        public static int SkinBase;
        public static Item Youmuu = new Item(ItemId.Youmuus_Ghostblade);
        public static Item Botrk = new Item(ItemId.Blade_of_the_Ruined_King);
        public static Item Cutlass = new Item(ItemId.Bilgewater_Cutlass);
        public static Item Tear = new Item(ItemId.Tear_of_the_Goddess);
        public static Item Qss = new Item(ItemId.Quicksilver_Sash);
        public static Item Simitar = new Item(ItemId.Mercurial_Scimitar);
        public static Item hextech = new Item(ItemId.Hextech_Gunblade, 700);
        public static AIHeroClient lastTarget;
        public static float lastSeen = Game.Time;
        public static float RCast = 0;
        public static Vector3 predictedPos;
        public static AIHeroClient RTarget = null;
        public static Vector3 RCastToPosition = new Vector3();
        public static Vector3 MyRCastPosition = new Vector3();
        public static bool disableMovement = false;
        public static bool PassiveUp;


        public static AIHeroClient PlayerInstance
        {
            get { return Player.Instance; }
        }
        private static float HealthPercent()
        {
            return (PlayerInstance.Health / PlayerInstance.MaxHealth) * 100;
        }

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static bool AutoQ { get; protected set; }
        public static float Manaah { get; protected set; }
        public static object GameEvent { get; private set; }

        public static Spell.Targeted Q;
        public static Spell.Skillshot Q1;
        public static Spell.Skillshot W;
        public static Spell.Skillshot W1;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }


        static void Game_OnStart(EventArgs args)
        {
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Game_OnDraw;
            Gapcloser.OnGapcloser += Common.Gapcloser_OnGapCloser;
            Obj_AI_Base.OnBuffGain += Common.OnBuffGain;
            Game.OnTick += OnTick;
            Orbwalker.OnPostAttack += Common.aaCombo;
            Orbwalker.OnPostAttack += Common.LJClear;
            Player.OnBasicAttack += Player_OnBasicAttack;
            SkinBase = Player.Instance.SkinId;
            // Item
            try
            {
                if (ChampionName != PlayerInstance.BaseSkinName)
                {
                    return;
                }

                Q = new Spell.Targeted(SpellSlot.Q, 675);
                Q1 = new Spell.Skillshot(SpellSlot.Q, 940, SkillShotType.Linear, 350, int.MaxValue, 75);
                W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Linear, 250, 1600, 100);
                W1 = new Spell.Skillshot(SpellSlot.W, 500, SkillShotType.Linear, 250, 1600, 100);
                E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear);
                R = new Spell.Skillshot(SpellSlot.R, 1400, SkillShotType.Linear, 500, 2800, 110);



                Bootstrap.Init(null);
                Chat.Print("GuTenTak Addon Loading Success", Color.Green);


                Menu = MainMenu.AddMenu("GuTenTak Lucian", "Lucian");
                Menu.AddSeparator();
                Menu.AddLabel("GuTenTak Lucian Addon");

                var Enemies = EntityManager.Heroes.Enemies.Where(a => !a.IsMe).OrderBy(a => a.BaseSkinName);
                ModesMenu1 = Menu.AddSubMenu("Menu", "Modes1Lucian");
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Combo Configs");
                ModesMenu1.Add("CWeaving", new CheckBox("Komboda pasif kullan", true));
                ModesMenu1.Add("ComboQ", new CheckBox("Komboda Q kullan", true));
                ModesMenu1.Add("ComboW", new CheckBox("Komboda W kullan", true));
                ModesMenu1.Add("ComboE", new CheckBox("Komboda E kullan", true));
                ModesMenu1.Add("ManaCW", new Slider("W için gereken mana %", 30));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Logic Configs");
                ModesMenu1.Add("LogicAA", new ComboBox(" Kombo türü ", 1, "Hızlı", "AA menzilinde Full Hasar"));
                ModesMenu1.Add("LogicW", new ComboBox(" Basit W mantığı", 1, "AAMenzilinde", "Sürekli"));
                ModesMenu1.Add("WColision", new ComboBox(" W Capışma ", 1, "Capışma", "Capışma yok"));
                ModesMenu1.Add("LogicE", new ComboBox(" E mantığı ", 0, "E fareye doğru(Güvenli Pozis.)", "Yana", "Fareye dogru"));

                ModesMenu1.AddSeparator();
                //ModesMenu1.AddLabel("AutoHarass Configs");
                //ModesMenu1.Add("AutoHarass", new CheckBox("Use Q on AutoHarass", false));
               // ModesMenu1.Add("ManaAuto", new Slider("Mana %", 80));

                ModesMenu1.AddLabel("Harass Configs");
                ModesMenu1.Add("HWeaving", new CheckBox("Dürtmede Pasif kullan", true));
                ModesMenu1.Add("HarassMana", new Slider("Dürtmek icin gereken mana %", 60));
                ModesMenu1.Add("HarassQ", new CheckBox("Dürterken Q kullan", true));
                ModesMenu1.Add("HarassQext", new CheckBox("Durterken Q geniş kullan", true));
                ModesMenu1.Add("HarassW", new CheckBox("Dürterken W kullan", true));
                ModesMenu1.Add("ManaHW", new Slider("W kullanmak icin mana %", 60));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Kill Steal Configs");
                ModesMenu1.Add("KS", new CheckBox("Kullan öldürme", true));
                ModesMenu1.Add("KQ", new CheckBox("Öldürürken Q kullan", true));
                ModesMenu1.Add("KW", new CheckBox("Öldürürken W kullan", true));
                ModesMenu1.Add("KR", new CheckBox("Öldürürken R kullan", false));

                ModesMenu2 = Menu.AddSubMenu("Farm", "Modes2Lucian");
                ModesMenu2.AddLabel("Lane Clear Config");
                ModesMenu1.AddSeparator();
                ModesMenu2.Add("FarmQ", new CheckBox("Lane temizlerken Q kullan", true));
                ModesMenu2.Add("ManaLQ", new Slider("Mana %", 40));
                ModesMenu2.Add("FarmW", new CheckBox("Lane temizlerken W kullan", true));
                ModesMenu2.Add("ManaLW", new Slider("Mana %", 40));
                ModesMenu2.AddLabel("Jungle Clear Config");
                ModesMenu2.Add("JungleQ", new CheckBox("Orman temizlerken Q kullan", true));
                ModesMenu2.Add("ManaJQ", new Slider("Mana %", 40));
                ModesMenu2.Add("JungleW", new CheckBox("Orman temizlerken W kullan", true));
                ModesMenu2.Add("ManaJW", new Slider("Mana %", 40));

                ModesMenu3 = Menu.AddSubMenu("Misc", "Modes3Lucian");
                ModesMenu3.Add("AntiGap", new CheckBox("Atılma yapana E kullan", true));
                ModesMenu3.AddLabel("Flee Configs");
                ModesMenu3.Add("FleeE", new CheckBox("Kaçarken E kullan", true));

                ModesMenu3.AddLabel("Item Usage on Combo");
                ModesMenu3.Add("useYoumuu", new CheckBox("Kullan Youmuu", true));
                ModesMenu3.Add("usehextech", new CheckBox("Kullan Hextech", true));
                ModesMenu3.Add("useBotrk", new CheckBox("Kullan Botrk & Cutlass", true));
                ModesMenu3.Add("useQss", new CheckBox("Kullan Civalı", true));
                ModesMenu3.Add("minHPBotrk", new Slider("Altındaysa can Botrk %", 80));
                ModesMenu3.Add("enemyMinHPBotrk", new Slider("Altındaysa düşman canı Botrk %", 80));

                ModesMenu3.AddLabel("QSS Configs");
                ModesMenu3.Add("Qssmode", new ComboBox(" ", 0, "Auto", "Combo"));
                ModesMenu3.Add("Stun", new CheckBox("Stun", true));
                ModesMenu3.Add("Blind", new CheckBox("kör", true));
                ModesMenu3.Add("Charm", new CheckBox("Büyü", true));
                ModesMenu3.Add("Suppression", new CheckBox("Durdurma", true));
                ModesMenu3.Add("Polymorph", new CheckBox("Polymorph", true));
                ModesMenu3.Add("Fear", new CheckBox("Korkutma", true));
                ModesMenu3.Add("Taunt", new CheckBox("Taunt", true));
                ModesMenu3.Add("Silence", new CheckBox("Susturma", false));
                ModesMenu3.Add("QssDelay", new Slider("Kullan QSS Gecikme(ms)", 250, 0, 1000));

                ModesMenu3.AddLabel("QSS Ult Configs");
                ModesMenu3.Add("ZedUlt", new CheckBox("Zed R", true));
                ModesMenu3.Add("VladUlt", new CheckBox("Vladimir R", true));
                ModesMenu3.Add("FizzUlt", new CheckBox("Fizz R", true));
                ModesMenu3.Add("MordUlt", new CheckBox("Mordekaiser R", true));
                ModesMenu3.Add("PoppyUlt", new CheckBox("Poppy R", true));
                ModesMenu3.Add("QssUltDelay", new Slider("Kullan QSS Gecikme(ms) Ultiler icin", 250, 0, 1000));

                ModesMenu3.AddLabel("Skin Hack");
                ModesMenu3.Add("skinhack", new CheckBox("Skin hilesi aktif", false));
                ModesMenu3.Add("skinId", new ComboBox("Skin Mode", 0, "Default", "1", "2", "3", "4", "5", "6", "7", "8"));

                DrawMenu = Menu.AddSubMenu("Draws", "DrawLucian");
                DrawMenu.Add("drawA", new CheckBox(" Göster gercek AA", true));
                DrawMenu.Add("drawQ", new CheckBox(" Göster Q", true));
                DrawMenu.Add("drawQext", new CheckBox(" Göster Q uzun", true));
                DrawMenu.Add("drawW", new CheckBox(" Göster W", true));
                DrawMenu.Add("drawE", new CheckBox(" Göster E", true));
                DrawMenu.Add("drawR", new CheckBox(" Göster R", false));
                
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
            }

            catch (Exception e)
            {

            }

        }
        private static void Game_OnDraw(EventArgs args)
        {

            try
            {
                if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady() && Q.IsLearned)
                    {
                        Circle.Draw(Color.White, Q.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawQext"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady() && Q.IsLearned)
                    {
                        Circle.Draw(Color.White, Q1.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue)
                {
                    if (W.IsReady() && W.IsLearned)
                    {
                        Circle.Draw(Color.White, W.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue)
                {
                    if (E.IsReady() && E.IsLearned)
                    {
                        Circle.Draw(Color.White, E.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
                {
                    if (R.IsReady() && R.IsLearned)
                    {
                        Circle.Draw(Color.White, R.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawA"].Cast<CheckBox>().CurrentValue)
                {
                    Circle.Draw(Color.LightGreen, 560, Player.Instance.Position);
                }
            }
            catch (Exception e)
            {

            }
        }
        static void Game_OnUpdate(EventArgs args)
        {
            try
            {
                //var AutoHarass = ModesMenu1["AutoHarass"].Cast<CheckBox>().CurrentValue;
                //var ManaAuto = ModesMenu1["ManaAuto"].Cast<Slider>().CurrentValue;
                Common.KillSteal();

                /*
                if (AutoHarass && ManaAuto <= _Player.ManaPercent)
                    {
                        Common.AutoQ();
                    }*/
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    Common.Combo();
                    Common.ItemUsage();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    Common.Harass();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {

                    //Common.LaneClear();

                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {

                    //Common.JungleClear();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                {
                    //Common.LastHit();

                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                {
                    Common.Flee();

                }
            }
            catch (Exception e)
            {

            }
        }

        public static void OnTick(EventArgs args)
        {
            //Common.Skinhack();
            if (lastTarget != null)
            {
                if (lastTarget.IsVisible)
                {
                    predictedPos = Prediction.Position.PredictUnitPosition(lastTarget, 300).To3D();
                    lastSeen = Game.Time;
                }
                if (lastTarget.Distance(Player.Instance) > 700)
                {
                    lastTarget = null;
                }
            }
        }

        private static void Player_OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender != Player.Instance)
                return;
            if (args.Target is AIHeroClient)
                lastTarget = (AIHeroClient)args.Target;
            else
                lastTarget = null;
        }

        public static void OnCastSpell(GameObject sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsDead || !sender.IsMe) return;
            if (args.SData.IsAutoAttack())
            {
                PassiveUp = false;
            }
            switch (args.Slot)
            {
                case SpellSlot.Q:
                case SpellSlot.W:
                    Orbwalker.ResetAutoAttack();
                    break;
            }
        }

        public static void OnProcessSpellCast(GameObject sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsDead || !sender.IsMe) return;
            {
                switch (args.Slot)
                {
                    case SpellSlot.Q:
                    case SpellSlot.W:
                    case SpellSlot.R:
                        PassiveUp = true;
                        break;
                    case SpellSlot.E:
                        PassiveUp = true;
                        Orbwalker.ResetAutoAttack();
                        break;
                }
            }
        }

    }
}
