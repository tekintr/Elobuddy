using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ReGaren.ConfigList
{
    public static class Farm
    {
        private static readonly Menu Menu;
        private static readonly CheckBox _FarmQ;
        private static readonly CheckBox _FarmQLastHit;
        private static readonly CheckBox _FarmE;
        private static readonly Slider _FarmECount;
        private static readonly CheckBox _IgnoreECountJng;

        public static bool FarmQ
        {
            get { return _FarmQ.CurrentValue; }
        }
        public static bool FarmQLastHit
        {
            get { return _FarmQLastHit.CurrentValue; }
        }
        public static bool FarmE
        {
            get { return _FarmE.CurrentValue; }
        }
        public static int FarmECount
        {
            get { return _FarmECount.CurrentValue; }
        }
        public static bool FarmEIgnore
        {
            get { return _IgnoreECountJng.CurrentValue; }
        }

        static Farm()
        {
            Menu = Config.Menu.AddSubMenu("Farm");
            Menu.AddGroupLabel("Farm settings");
            _FarmQ = Menu.Add("FarmQ", new CheckBox("Koridor ve ormanda Q kullan."));
            _FarmE = Menu.Add("FarmE", new CheckBox("Koridor ve ormanda E kullan."));
            Menu.AddSeparator();
            _IgnoreECountJng = Menu.Add("IgniteECountJng", new CheckBox("E icin canavar sayisini onemseme."));
            _FarmECount = Menu.Add("FarmECount", new Slider("Use E if creeps count", 3, 1, 5));
            Menu.AddGroupLabel("Last hit");
            _FarmQLastHit = Menu.Add("FarmQLastHit", new CheckBox("Q ile sadece son vurus."));
        }

        public static void Initialize()
        {
        }
    }
}