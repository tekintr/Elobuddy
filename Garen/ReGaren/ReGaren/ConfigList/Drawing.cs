using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ReGaren.ConfigList
{
    public static class Drawing
    {
        private static readonly Menu Menu;
        private static readonly CheckBox _drawQ;
        private static readonly CheckBox _drawE;
        private static readonly CheckBox _drawR;
        private static readonly CheckBox _drawDI;

        public static bool DrawQ
        {
            get { return _drawQ.CurrentValue; }
        }

        public static bool DrawE
        {
            get { return _drawE.CurrentValue; }
        }

        public static bool DrawR
        {
            get { return _drawR.CurrentValue; }
        }

        public static bool DrawDI
        {
            get { return _drawDI.CurrentValue; }
        }

        static Drawing()
        {
            Menu = Config.Menu.AddSubMenu("Drawing");
            Menu.AddGroupLabel("Drawing settings");
            _drawQ = Menu.Add("DrawQ", new CheckBox("Q mesafesini goster"));
            _drawE = Menu.Add("DrawE", new CheckBox("E mesafesini goster"));
            _drawR = Menu.Add("DrawR", new CheckBox("R mesafesini goster"));
            Menu.AddGroupLabel("Damage indicator");
            _drawDI = Menu.Add("DrawDI", new CheckBox("Verilebilicek hasari goster"));
        }

        public static void Initialize()
        {
        }
    }
}