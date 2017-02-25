using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ReGaren.ConfigList
{
    public static class Combo
    {
        private static readonly Menu Menu;
        private static readonly CheckBox _ComboQ;
        private static readonly CheckBox _ComboW;
        private static readonly CheckBox _ComboE;
        private static readonly CheckBox _ComboR;

        public static bool ComboQ
        {
            get { return _ComboQ.CurrentValue; }
        }
        public static bool ComboW
        {
            get { return _ComboW.CurrentValue; }
        }
        public static bool ComboE
        {
            get { return _ComboE.CurrentValue; }
        }
        public static bool ComboR
        {
            get { return _ComboR.CurrentValue; }
        }

        static Combo()
        {
            Menu = Config.Menu.AddSubMenu("Combo");
            Menu.AddGroupLabel("Combo settings");
            _ComboQ = Menu.Add("ComboQ", new CheckBox("Komboda Q kullan"));
            _ComboW = Menu.Add("ComboW", new CheckBox("Komboda W kullan"));
            _ComboE = Menu.Add("ComboE", new CheckBox("Komboda E kullan"));
            _ComboR = Menu.Add("ComboR", new CheckBox("Komboda R kullan"));
        }

        public static void Initialize()
        {
        }
    }
}