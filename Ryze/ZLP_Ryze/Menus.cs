using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ZLP_Ryze
{
    public class Menus
    {
        public static Menu Main, Combo, Clear, Misc, Draw;
        public static Slider MinDelay, MaxDelay, ComboHealth, HarassMana, LaneMana, JungleMana, LastMana, Auto;

        public static void Initialize()
        {
            Main = MainMenu.AddMenu("ZLP Ryze", "ZLP_Ryze");
            Main.AddLabel("Made by Zaloopa");
            Main.AddSeparator();
            Main.AddGroupLabel("Humanizer");
            Main.Add("human", new CheckBox("Kullan insancil buyu"));
            MinDelay = Main.Add("min", new Slider("Min. Gecikme (ms)", 1, 0, 1000));
            MaxDelay = Main.Add("max", new Slider("Max. Gecikme (ms)", 249, 0, 1000));
            Main.AddGroupLabel("Q Hit Chance");
            Main.Add("hit", new ComboBox("Hit Chance", 1, "Low", "Medium", "High"));

            Combo = Main.AddSubMenu("Combo & Harass");
            {
                Combo.AddGroupLabel("Combo");
                Combo.Add("Qc", new CheckBox("Kullan Q"));
                Combo.Add("Wc", new CheckBox("Kullan W"));
                Combo.Add("Ec", new CheckBox("Kullan E"));
                ComboHealth = Combo.Add("MinHP", new Slider("daha az ise HP defansif kombo {0}% (0 = disable)", 30));
                Combo.AddGroupLabel("Harass");
                Combo.Add("Qh", new CheckBox("Kullan Q"));
                Combo.Add("Eh", new CheckBox("Kullan E"));
                HarassMana = Combo.Add("HarassMP", new Slider("Mana az ise buyu kullanma {0}%", 40));
                Combo.Add("auto", new CheckBox("Oto durtme (Q)", false));
            }

            Clear = Main.AddSubMenu("Lane & Jungle Clear");
            {
                Clear.AddGroupLabel("Lane Clear");
                Clear.Add("Qlc", new CheckBox("Kullan Q"));
                Clear.Add("Wlc", new CheckBox("Kullan W"));
                Clear.Add("Elc", new CheckBox("Kullan E"));
                LaneMana = Clear.Add("lcmp", new Slider("Mana az ise buyu kullanma {0}%", 40));
                Clear.AddGroupLabel("Jungle Clear");
                Clear.Add("Qjc", new CheckBox("Kullan Q"));
                Clear.Add("Wjc", new CheckBox("Kullan W"));
                Clear.Add("Ejc", new CheckBox("Kullan E"));
                JungleMana = Clear.Add("jcmp", new Slider("Mana az ise buyu kullanma {0}%", 40));
                Clear.AddGroupLabel("Last Hit");
                Clear.Add("Qlh", new CheckBox("Kullan Q"));
                Clear.Add("Wlh", new CheckBox("Kullan W"));
                Clear.Add("Elh", new CheckBox("Kullan E"));
                LastMana = Clear.Add("lhmp", new Slider("Mana az ise buyu kullanma {0}%", 25));
            }

            Misc = Main.AddSubMenu("Misc Settings");
            {
                Misc.AddGroupLabel("Kill Steal");
                Misc.Add("Q", new CheckBox("Kullan Q"));
                Misc.Add("W", new CheckBox("Kullan W"));
                Misc.Add("E", new CheckBox("Kullan E"));
                Misc.Add("ignite", new CheckBox("Kullan Tutustur"));
                Misc.AddGroupLabel("Escape");
                Misc.Add("esc", new KeyBind("Kacis R + Zhonya's", false, KeyBind.BindTypes.HoldActive, "A".ToCharArray()[0]));
                Auto = Misc.Add("auto", new Slider("Oto R + Zhonya's icin HP {0}% (0 = disable)", 20));
                Misc.AddGroupLabel("Other Options");
                Misc.Add("gap", new CheckBox("Oto W atilma yapana", false));
                Misc.Add("stack", new CheckBox("Oto yuk kasma"));
            }

            Draw = Main.AddSubMenu("Drawings");
            {
                Draw.Add("draw", new CheckBox("Cizimler aktif"));
                Draw.AddSeparator();
                Draw.Add("Q", new CheckBox("Goster Q"));
                Draw.Add("WE", new CheckBox("Goster W/E"));
                Draw.Add("R", new CheckBox("Goster R"));
                Draw.Add("ignite", new CheckBox("Goster tutustur", false));
                Draw.AddSeparator();
                Draw.Add("damage", new CheckBox("Verilicek hasar"));
            }
        }
    }
}