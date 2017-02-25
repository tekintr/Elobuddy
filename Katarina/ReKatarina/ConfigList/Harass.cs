using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ReKatarina.ConfigList
{
    public static class Harass
    {
        private static readonly Menu Menu;
        private static readonly CheckBox _HarassWithQ;
        private static readonly CheckBox _AutoHarassWithQ;
        private static readonly Slider _AutoHarassChance;

        public static bool HarassWithQ
        {
            get { return _HarassWithQ.CurrentValue; }
        }
        public static bool AutoHarassWithQ
        {
            get { return _AutoHarassWithQ.CurrentValue; }
        }
        public static int AutoHarassChance
        {
            get { return _AutoHarassChance.CurrentValue; }
        }

        static Harass()
        {
            Menu = Config.Menu.AddSubMenu("Harass");
            Menu.AddGroupLabel("Harass settings");
            _HarassWithQ = Menu.Add("Harass.UseQ", new CheckBox("Durtme modunda Q aktif."));
            Menu.AddGroupLabel("Auto-harass settings");
            _AutoHarassWithQ = Menu.Add("Harass.Auto.UseQ", new CheckBox("Oto durtmede kullanilsin Q."));
            _AutoHarassChance = Menu.Add("Harass.Auto.Chance", new Slider("Oto durtme sansi.", 50, 1, 100));
        }

        public static void Initialize()
        {
        }
    }
}