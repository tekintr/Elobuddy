using System.Reflection;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Drawing;
using System;
using EloBuddy;

namespace HTTF_Riven_v2
{
    class RivenMenu
    {
        public static Menu Principal, Combo, Burst, Shield, Items, Laneclear, Jungleclear, Flee, Misc, Draw, Killsteal, AnimationCancle, ComboLogic, M_NVer;

        public static void Load()
        {
            Chat.Print("<font color = '#20b2aa'>Hoşgeldiniz </font><font color = '#ffffff'>[ HTTF ] " + "Riven" + "</font><font color = '#20b2aa'>. Türkçe Çeviri TekinTR.</font>");
            CheckVersion.CheckUpdate();

            Principal = MainMenu.AddMenu("HTTF Riven v2", "Riven");


            Combo = Principal.AddSubMenu("Combo", "Combo");
            Combo.AddSeparator(3);
            Combo.AddLabel("• Kombo Ayarları");
            Combo.Add("UseQCombo", new CheckBox("Q kullan?"));
            Combo.Add("UseWCombo", new CheckBox("W kullan?"));
            Combo.Add("UseECombo", new CheckBox("E kullan"));
            Combo.Add("UseRCombo", new CheckBox("R kullan?"));
            Combo.Add("UseR2Combo", new CheckBox("R2 kullan?"));
            Combo.Add("BrokenAnimations", new CheckBox("Animasyon iptali ?",false));
            Combo.Add("logic1x1", new CheckBox("1x1 mantığını kullan"));
            Combo.Add("UseHT", new CheckBox("Komboda Hydra-Tiamat kullan?"));

            Combo.AddSeparator(3);


            Combo.AddLabel("• R Ayarları");
            Combo.Add("UseRType", new ComboBox("R kullanma durumu", 1, "Hedefin canı az ise 40 % HP", "Verilicek hasar 100 % ise", "Her zaman", "Tuşa basıldığında"));
            Combo.Add("ForceR", new KeyBind("R On Keypress Key", false, KeyBind.BindTypes.PressToggle, 'U'));
            Combo.Add("DontR1", new Slider("Dont R if Target HP {0}% <=", 25, 10, 50));
            Combo.AddSeparator(3);
            Combo.AddLabel("• R2 Ayarları");
            Combo.Add("UseR2Type", new ComboBox("R2 kullanma durumu", 0, "Sadece öldür", "Verilebilicek max hasardan sonra kalacak can 25 %"));
            Combo.AddLabel(" Kaçış");
            Combo.Add("UseQFlee", new CheckBox("Q Kullan"));
            Combo.Add("UseEFlee", new CheckBox("E Kullan"));

            Shield = Principal.AddSubMenu("Kalkan", "Shield");
            Shield.AddLabel("• Kalkan E");
            foreach (var Enemy in EntityManager.Heroes.Enemies)
            {
                Shield.AddLabel(Enemy.ChampionName);
                Shield.Add("E/" + Enemy.BaseSkinName + "/Q", new CheckBox(Enemy.ChampionName + " (Q)", false));
                Shield.Add("E/" + Enemy.BaseSkinName + "/W", new CheckBox(Enemy.ChampionName + " (W)", false));
                Shield.Add("E/" + Enemy.BaseSkinName + "/E", new CheckBox(Enemy.ChampionName + " (E)", false));
                Shield.Add("E/" + Enemy.BaseSkinName + "/R", new CheckBox(Enemy.ChampionName + " (R)", false));
                Shield.AddSeparator(1);
            }




            Laneclear = Principal.AddSubMenu("Koridor", "Laneclear");
            Laneclear.AddLabel("• Koridor");
            Laneclear.Add("UseQLane", new CheckBox("Q kullan"));
            Laneclear.Add("UseWLane", new CheckBox("W kullan"));
            Laneclear.Add("UseWLaneMin", new Slider("W'nun isabet ediceği minyon sayısı {0}", 3, 0, 10));
            Laneclear.AddLabel("• Orman");
            Laneclear.Add("UseQJG", new CheckBox("Q kullan"));
            Laneclear.Add("UseWJG", new CheckBox("W kullan"));
            Laneclear.Add("UseEJG", new CheckBox("E kullan"));



            Misc = Principal.AddSubMenu("Çeşitli", "Misc");
            Misc.Add("Skin", new CheckBox("Kostüm Seç ?", false));
            Misc.Add("SkinID", new Slider("Skin ID: {0}", 4, 0, 11));
            Misc.Add("Interrupter", new CheckBox("Engelleyici ?"));
            Misc.Add("InterrupterW", new CheckBox("W ile engelle ?"));
            Misc.Add("Gapcloser", new CheckBox("Atılma önleyici ?"));
            Misc.Add("GapcloserW", new CheckBox("W ile atılma yapanlari engelle ?"));
            Misc.Add("AliveQ", new CheckBox("Q bitmeden kullan ?"));
            Misc.AddLabel("• Eşya Mantığı");
            Misc.AddLabel("• Hydra Mantığı");
            Misc.Add("Hydra", new CheckBox("Hydra Kullanılsınmı?"));
            Misc.Add("HydraReset", new CheckBox("Düz vuruş sıfırlarmada hydra kullan"));
            Misc.AddSeparator(3);
            Misc.AddLabel("• Tiamat Logic");
            Misc.Add("Tiamat", new CheckBox("Tiamat Kullanılsınmı?"));
            Misc.Add("TiamatReset", new CheckBox("Düz vuruş sıfırlarmada tiamat kullan"));
            Misc.AddSeparator(3);
            Misc.AddLabel("• Civalı Kuşak Mantığı");
            Misc.Add("Qss", new CheckBox("Civalı kullan?"));
            Misc.Add("QssCharm", new CheckBox("Ayartılınca Civalı kullan"));
            Misc.Add("QssFear", new CheckBox("Korkutulunca Civalı kullan"));
            Misc.Add("QssTaunt", new CheckBox("Kışkırtılınca Civalı kullan"));
            Misc.Add("QssSuppression", new CheckBox("Engellenince Civalı kullan"));
            Misc.Add("QssSnare", new CheckBox("Yerine sabitlenince Civalı kullan"));
            Misc.AddSeparator(3);
            Misc.AddLabel("• Youmu Mantığı");
            Misc.Add("Youmu", new CheckBox("Youmu Kullanılsınmı?"));
            Misc.AddLabel("• Önerilen uzaklık 250•");
            Misc.Add("YoumuRange", new Slider("Youmu Kullanma mesafesi", 1, 1, 325));


            Draw = Principal.AddSubMenu("Çizimler", "Drawing");
            Draw.Add("DrawDamage", new CheckBox("Hasarımı göster"));
            Draw.Add("DrawOFF", new CheckBox("Çizimleri kapat", false));
            Draw.Add("drawjump", new CheckBox("Atlanabilicek duvarları göster (beta)", false));


            AnimationCancle = Principal.AddSubMenu("Animasyonİptali", "CanslAnimatio");
            AnimationCancle.Add("4", new CheckBox("Q"));
            AnimationCancle.Add("Spell2", new CheckBox("W"));
            AnimationCancle.Add("Spell3", new CheckBox("E"));
            AnimationCancle.Add("Spell4", new CheckBox("R"));


            ComboLogic = Principal.AddSubMenu("KomboMantığı", "ComboLogics");
            ComboLogic.Add("BrokenAnimon", new CheckBox("Özellikleri kullan?"));
            ComboLogic.Add("moveback", new CheckBox("Komboda HTTF mantığını kullan?", false));

            ComboLogic.AddLabel("Q1,Q2,Q3");
            ComboLogic.Add("Q1Hydra", new CheckBox("Q>Hydra"));
            ComboLogic.Add("HydraQ", new CheckBox("Hydra>Q"));
            ComboLogic.Add("QW", new CheckBox("Q>W"));

            

            ComboLogic.AddLabel("W");
            ComboLogic.Add("HydraW", new CheckBox("Hydra>W"));



            ComboLogic.AddLabel("E");
            ComboLogic.Add("EQall", new CheckBox("E>Q"));
            ComboLogic.Add("EW", new CheckBox("E>W"));
            ComboLogic.Add("EH", new CheckBox("E>Hydra yada Tiamat"));
            ComboLogic.Add("ER1", new CheckBox("E>R1"));
            ComboLogic.Add("ER2", new CheckBox("E>R2"));


            ComboLogic.AddLabel("R1");
            ComboLogic.Add("R1W", new CheckBox("R1>W"));
            ComboLogic.Add("R1Q", new CheckBox("R1>Q"));
            ComboLogic.Add("R1Hydra", new CheckBox("R1>Hydra yada Tiamat"));


            ComboLogic.AddLabel("R2");
            ComboLogic.Add("R2W", new CheckBox("R2>W"));
            ComboLogic.Add("R2Q", new CheckBox("R2>Q"));
            ComboLogic.Add("R2E", new CheckBox("R2>E"));


            ComboLogic.AddLabel("Combo Mantığı V2 Yakında");












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

        internal static bool getKeyBindItem(Action combo, string v)
        {
            throw new NotImplementedException();
        }

        public static bool Keybind(Menu m, string s)
        {
            return m[s].Cast<KeyBind>().CurrentValue;

        }

        public static bool getKeyBindItem(Menu m, string item)
        {
            return m[item].Cast<KeyBind>().CurrentValue;
        }



        public static int ComboBox(Menu m, string s)
        {
            return m[s].Cast<ComboBox>().SelectedIndex;
        }
    }
}