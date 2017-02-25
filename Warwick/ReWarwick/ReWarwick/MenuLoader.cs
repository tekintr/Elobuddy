using EloBuddy.SDK.Menu;
using ReWarwick.Config;

namespace ReWarwick
{
    public static class MenuLoader
    {
        public static readonly Menu Menu;

        static MenuLoader()
        {
            Menu = MainMenu.AddMenu("ReWarwick", "ReWarwick");
            Menu.AddGroupLabel("Hosgeldiniz ReWarwick!");
            Utility.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Utility
        {
            static Utility()
            {
                // Menu
                Combo.Initialize();
                Drawing.Initialize();
                Farm.Initialize();
                Harass.Initialize();
                Misc.Initialize();
            }

            public static void Initialize()
            {
            }
        }
    }
}