using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ReKatarina.ConfigList
{
    public static class Misc
    {
        public static readonly Menu Menu;
        private static readonly CheckBox _KSWithQ;
        private static readonly CheckBox _KSWithE;
        private static readonly Slider _KSMinE;
        private static readonly CheckBox _SkinManagerStatus;
        private static readonly Slider _SkinManager;
        private static readonly CheckBox _EnableHumanizer;
        private static readonly Slider _Delay;
        private static readonly Slider _MaxRandomDelay;

        public static bool KSWithQ
        {
            get { return _KSWithQ.CurrentValue; }
        }
        public static bool KSWithE
        {
            get { return _KSWithE.CurrentValue; }
        }
        public static int KSMinE
        {
            get { return _KSMinE.CurrentValue; }
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
        public static int GetMaxAditDelay
        {
            get { return _MaxRandomDelay.CurrentValue; }
        }
        public static bool GetHumanizerStatus
        {
            get { return _EnableHumanizer.CurrentValue; }
        }

        static Misc()
        {
            Menu = Config.Menu.AddSubMenu("Misc");
            Menu.AddGroupLabel("Misc settings");
            _KSWithQ = Menu.Add("Misc.KillSteal.UseQ", new CheckBox("Oldurmek icin aktif Q."));
            _KSWithE = Menu.Add("Misc.KillSteal.UseE", new CheckBox("Oldurmek icin aktif E."));
            _KSMinE = Menu.Add("Misc.KillSteal.MinHP", new Slider("Canin olmalidir >= {0}% kullanmak icin E / Q+E.", 35, 1, 100));
            Menu.AddGroupLabel("Skin manager");
            _SkinManagerStatus = Menu.Add("Misc.Skin.Status", new CheckBox("Skin secici aktif."));
            _SkinManager = Menu.Add("Misc.Skin.Id", new Slider("Istedigini sec.", 1, 0, 10));
            Menu.AddGroupLabel("Humanizer");
            _EnableHumanizer = Menu.Add("Misc.Humanizer.Status", new CheckBox("Insancil aktif", false));
            _Delay = Menu.Add("Misc.Humanizer.Delay", new Slider("Skill gecikme suresini secin (ms).", 200, 50, 500));
            _MaxRandomDelay = Menu.Add("Misc.Humanizer.RandomDelay", new Slider("Rastgele gecikme (ms).", 75, 50, 100));

            #region Disable / enable skin changer
            switch(_SkinManagerStatus.CurrentValue)
            {
                case true:
                    Player.Instance.SetSkinId(GetSkinManager);
                    break;
                case false:
                    break;
            }
            _SkinManagerStatus.OnValueChange += (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args) =>
            {
                if (args.NewValue == false)
                    Player.Instance.SetSkinId(0);
                else
                    Player.Instance.SetSkinId(GetSkinManager);
            };

            _SkinManager.OnValueChange += (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args) =>
            {
                if (_SkinManagerStatus.CurrentValue == true)
                    Player.Instance.SetSkinId(GetSkinManager);
            };
            #endregion
        }


        public static void Initialize()
        {
        }
    }
}