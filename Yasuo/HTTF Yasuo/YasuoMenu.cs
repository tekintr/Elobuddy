using System.Reflection;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Drawing;

    namespace HTTF_Yasuo
    {
        class YasuoMenu
        {
            public static Menu Principal, Combo;

            public static void Load()
            {
                Principal = MainMenu.AddMenu("HTTF Yasuo ", "Yasuo");
                Principal.AddLabel("Yasuo Riven v" + Assembly.GetExecutingAssembly().GetName().Version);


                Combo = Principal.AddSubMenu("Combo", "Combo");
                Combo.AddSeparator(3);
                Combo.AddLabel("• Kombo Ayarlari");
                Combo.Add("UseQCombo", new CheckBox("Kullan Q"));
                Combo.Add("UseWCombo", new CheckBox("Kullan W"));
                Combo.Add("UseECombo", new CheckBox("Kullan E"));
                Combo.Add("UseRCombo", new CheckBox("Kullan R"));









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
