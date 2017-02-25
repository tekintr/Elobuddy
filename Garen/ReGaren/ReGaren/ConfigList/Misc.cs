using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ReGaren.ConfigList
{
    public static class Misc
    {
        private static readonly Menu Menu;
        private static readonly CheckBox _KSWithR;
        private static readonly CheckBox _FleeWithQ;
        private static readonly CheckBox _SkinManagerStatus;
        private static readonly Slider _SkinManager;
        private static readonly Slider _Delay;

        public static bool KSWithR
        {
            get { return _KSWithR.CurrentValue; }
        }
        public static bool FleeWithQ
        {
            get { return _FleeWithQ.CurrentValue; }
        }
        public static bool GetSkinManagerStatus
        {
            get { return _SkinManagerStatus.CurrentValue; }
        }
        public static int GetSkinManager
        {
            get { return _SkinManager.CurrentValue; }
        }
        public static int GetSpellDelay
        {
            get { return _Delay.CurrentValue; }
        }

        static Misc()
        {
            Menu = Config.Menu.AddSubMenu("Misc");
            Menu.AddGroupLabel("Misc settings");
            _KSWithR = Menu.Add("KSWithR", new CheckBox("R ile oldurucu vurus modu."));
            _FleeWithQ = Menu.Add("FleeWithQ", new CheckBox("Kacis modunda oto Q."));
            Menu.AddGroupLabel("Skin manager");
            _SkinManagerStatus = Menu.Add("SkinManagerStatus", new CheckBox("Skin secici aktif."));
            _SkinManager = Menu.Add("SkinManager", new Slider("Kendine skin sec.", 1, 0, 8));
            Menu.AddGroupLabel("Delayer");
            _Delay = Menu.Add("Delay", new Slider("Select your delay between AA in (ms).", 125, 50, 300));
        }

        public static void Initialize()
        {
        }
    }
}