using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ReKatarina.ConfigList
{
    public static class Farm
    {
        private static readonly Menu Menu;
        private static readonly CheckBox _FarmQ;
        private static readonly CheckBox _FarmW;
        private static readonly CheckBox _FarmE;
        private static readonly CheckBox _LastHitQ;
        private static readonly Slider _FarmQCount;
        private static readonly CheckBox _IgnoreQCountJng;

        public static bool FarmQ
        {
            get { return _FarmQ.CurrentValue; }
        }
        public static bool FarmW
        {
            get { return _FarmW.CurrentValue; }
        }
        public static bool FarmE
        {
            get { return _FarmE.CurrentValue; }
        }
        public static bool LastHitQ
        {
            get { return _LastHitQ.CurrentValue; }
        }
        public static int FarmQCount
        {
            get { return _FarmQCount.CurrentValue; }
        }
        public static bool FarmQIgnore
        {
            get { return _IgnoreQCountJng.CurrentValue; }
        }

        static Farm()
        {
            Menu = Config.Menu.AddSubMenu("Farm");
            Menu.AddGroupLabel("Farm settings");
            _FarmQ = Menu.Add("Farm.UseQ", new CheckBox("Kullan Q koridor / orman temizleme."));
            _FarmW = Menu.Add("Farm.UseW", new CheckBox("Kullan W koridor / orman temizleme."));
            _FarmE = Menu.Add("Farm.UseE", new CheckBox("Kullan E koridor / orman temizleme."));
            Menu.AddSeparator();
            _IgnoreQCountJng = Menu.Add("Farm.IgnoreCount", new CheckBox("Orman temizlerken Q sayisini yoksay."));
            _FarmQCount = Menu.Add("Farm.Q.Count", new Slider("Su kadar yaratiga Q kullan", 3, 1, 5));
            Menu.AddGroupLabel("Last hit settings");
            _LastHitQ = Menu.Add("Farm.LastHit.UseQ", new CheckBox("Son vurus modunda Q kullan."));
        }

        public static void Initialize()
        {
        }
    }
}