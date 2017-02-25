using System.Reflection;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Drawing;
namespace HTTF_Riven_v2
{
    class RivenMenu
    {
        public static Menu Principal, Combo, Burst, Shield, Items, Laneclear, Jungleclear, Flee, Misc, Draw, Killsteal, AnimationCancle, ComboLogic;

        public static void Load()
        {
            Principal = MainMenu.AddMenu("HTTF Riven v2", "Riven");
            Principal.AddLabel("HTTF Riven v" + Assembly.GetExecutingAssembly().GetName().Version);


            Combo = Principal.AddSubMenu("Combo", "Combo");
            Combo.AddSeparator(3);
            Combo.AddLabel("• Spells Combo");
            Combo.Add("UseQCombo", new CheckBox("Kullan Q?"));
            Combo.Add("UseWCombo", new CheckBox("Kullan W?"));
            Combo.Add("UseECombo", new CheckBox("Kullan E"));
            Combo.Add("UseRCombo", new CheckBox("Kullan R?"));
            Combo.Add("UseR2Combo", new CheckBox("Kullan R2?"));
            Combo.Add("BrokenAnimations", new CheckBox("Animasyon Kirma ?",false));
            Combo.Add("moveback", new CheckBox("Combo'da geri donun ?", false));
            Combo.AddSeparator(3);
            Combo.AddLabel("• Spell R");
            Combo.Add("UseRType", new ComboBox("Kullan R ne zaman", 1, "Hedeften az 40 % HP", "Zarar Gostergesi daha buyuk 100 %", "Her zaman", "Tusa basildiginda"));
            Combo.Add("ForceR", new KeyBind("R Tusa basildiginda", false, KeyBind.BindTypes.PressToggle, 'U'));
            Combo.Add("DontR1", new Slider("Kullanma R if Hedef HP {0}% <=", 25, 10, 50));
            Combo.AddSeparator(3);
            Combo.AddLabel("• Spell R2");
            Combo.Add("UseR2Type", new ComboBox("Kullan R2 ne zaman", 0, "Sadece Oldur", "Hedef az olduğunda maksimum hasar 25 %"));
            Combo.AddLabel(" FLEE");
            Combo.Add("UseQFlee", new CheckBox("Kullan Q"));
            Combo.Add("UseEFlee", new CheckBox("Kullan E"));

            Shield = Principal.AddSubMenu("Shield", "Shield");
            Shield.AddLabel("• Spell E");
            foreach (var Enemy in EntityManager.Heroes.Enemies)
            {
                Shield.AddLabel(Enemy.ChampionName);
                Shield.Add("E/" + Enemy.BaseSkinName + "/Q", new CheckBox(Enemy.ChampionName + " (Q)", false));
                Shield.Add("E/" + Enemy.BaseSkinName + "/W", new CheckBox(Enemy.ChampionName + " (W)", false));
                Shield.Add("E/" + Enemy.BaseSkinName + "/E", new CheckBox(Enemy.ChampionName + " (E)", false));
                Shield.Add("E/" + Enemy.BaseSkinName + "/R", new CheckBox(Enemy.ChampionName + " (R)", false));
                Shield.AddSeparator(1);
            }




            Laneclear = Principal.AddSubMenu("Laneclear", "Laneclear");
            Laneclear.AddLabel("• WaweClean");
            Laneclear.Add("UseQLane", new CheckBox("Kullan Q"));
            Laneclear.Add("UseWLane", new CheckBox("Kullan W"));
            Laneclear.Add("UseWLaneMin", new Slider("Kullan W carpicaksa {0} minyona", 3, 0, 10));
            Laneclear.AddLabel("• JunglClean");
            Laneclear.Add("UseQJG", new CheckBox("Kullan Q"));
            Laneclear.Add("UseWJG", new CheckBox("Kullan W"));
            Laneclear.Add("UseEJG", new CheckBox("Kullan E"));



            Misc = Principal.AddSubMenu("Misc", "Misc");
            Misc.Add("Skin", new CheckBox("SkinHilesi ?", false));
            Misc.Add("SkinID", new Slider("Skin ID: {0}", 4, 0, 11));
            Misc.Add("Interrupter", new CheckBox("Kesici ?"));
            Misc.Add("InterrupterW", new CheckBox("Kesici ile W ?"));
            Misc.Add("Gapcloser", new CheckBox("Atilma Onleyicisi ?"));
            Misc.Add("GapcloserW", new CheckBox("Kullan W ile atilma engelleme ?"));
            Misc.Add("AliveQ", new CheckBox("Kullan Q Hayatta Kalma ?"));
            Misc.AddLabel("• ItemLogic");
            Misc.AddLabel("• Hydra Logic");
            Misc.Add("Hydra", new CheckBox("Kullan Hydra?"));
            Misc.Add("HydraReset", new CheckBox("Kullan hydra AA sifirlamada"));
            Misc.AddSeparator(3);
            Misc.AddLabel("• Tiamat Logic");
            Misc.Add("Tiamat", new CheckBox("Kullan Tiamat?"));
            Misc.Add("TiamatReset", new CheckBox("Kullan Tiamat AA sifirlamada"));
            Misc.AddSeparator(3);
            Misc.AddLabel("• Qss / Mercurial Logic");
            Misc.Add("Qss", new CheckBox("Use Qss?"));
            Misc.Add("QssCharm", new CheckBox("Kullan Qss buyu yuzunden"));
            Misc.Add("QssFear", new CheckBox("Kullan Qss korkutma"));
            Misc.Add("QssTaunt", new CheckBox("Kullan Qss because of taunt"));
            Misc.Add("QssSuppression", new CheckBox("Kullan Qss durdurma tutma"));
            Misc.Add("QssSnare", new CheckBox("Kullan Qss yakalanma"));
            Misc.AddSeparator(3);
            Misc.AddLabel("• Youmu Logic");
            Misc.Add("Youmu", new CheckBox("Kullan Youmu?"));
            Misc.AddLabel("• Recommend Use 250•");
            Misc.Add("YoumuRange", new Slider("Youmu icin uzaklik", 1, 1, 325));


            Draw = Principal.AddSubMenu("Drawing", "Drawing");
            Draw.Add("DrawDamage", new CheckBox("Hasarimi Goster"));
            Draw.Add("DrawOFF", new CheckBox("Gostergeler Kapali", false));
            Draw.Add("drawjump", new CheckBox("Goster Ziplamayi(beta)", false));


            AnimationCancle = Principal.AddSubMenu("AnimationCancle", "CanslAnimatio");
            AnimationCancle.Add("4", new CheckBox("Q"));
            AnimationCancle.Add("Spell2", new CheckBox("W"));
            AnimationCancle.Add("Spell3", new CheckBox("E"));
            AnimationCancle.Add("Spell4", new CheckBox("R"));


            ComboLogic = Principal.AddSubMenu("ComboLogic", "ComboLogics");
            ComboLogic.Add("BrokenAnimon", new CheckBox("Use features?"));

            ComboLogic.AddLabel("Q1,Q2,Q3");
            ComboLogic.Add("Q1Hydra", new CheckBox("Q>Hydra"));
            ComboLogic.Add("HydraQ", new CheckBox("Hydra>Q"));
            ComboLogic.Add("QW", new CheckBox("Q>W"));

            

            ComboLogic.AddLabel("W");
            ComboLogic.Add("HydraW", new CheckBox("Hydra>W"));



            ComboLogic.AddLabel("E");
            ComboLogic.Add("EQall", new CheckBox("E>Q"));
            ComboLogic.Add("EW", new CheckBox("E>W"));
            ComboLogic.Add("EH", new CheckBox("E>Hydra or Tiamat"));
            ComboLogic.Add("ER1", new CheckBox("E>R1"));
            ComboLogic.Add("ER2", new CheckBox("E>R2"));


            ComboLogic.AddLabel("R1");
            ComboLogic.Add("R1W", new CheckBox("R1>W"));
            ComboLogic.Add("R1Q", new CheckBox("R1>Q"));
            ComboLogic.Add("R1Hydra", new CheckBox("R1>Hydra or Tiamat"));


            ComboLogic.AddLabel("R2");
            ComboLogic.Add("R2W", new CheckBox("R2>W"));
            ComboLogic.Add("R2Q", new CheckBox("R2>Q"));
            ComboLogic.Add("R2E", new CheckBox("R2>E"));


            ComboLogic.AddLabel("Combo Logic V2 SOON");












        }
        //Cancler
        public static bool AnimationCancelQ
        {
            get { return (AnimationCancle["Spell1"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool AnimationCancelW
        {
            get { return (AnimationCancle["Spell2"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool AnimationCancelE
        {
            get { return (AnimationCancle["Spell3"].Cast<CheckBox>().CurrentValue); }
        }
        public static bool AnimationCancelR
        {
            get { return (AnimationCancle["Spell4"].Cast<CheckBox>().CurrentValue); }
        }



        public static bool CheckBox(Menu m, string s)
        {
            return m[s].Cast<CheckBox>().CurrentValue;
        }

        public static int Slider(Menu m, string s)
        {
            return m[s].Cast<Slider>().CurrentValue;
        }

        public static bool Keybind(Menu m, string s)
        {
            return m[s].Cast<KeyBind>().CurrentValue;

        }



        public static int ComboBox(Menu m, string s)
        {
            return m[s].Cast<ComboBox>().SelectedIndex;
        }
    }
}