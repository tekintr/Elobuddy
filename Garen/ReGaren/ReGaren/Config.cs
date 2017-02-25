using EloBuddy.SDK.Menu;
using ReGaren.ConfigList;

namespace ReGaren
{
    public static class Config
    {
        public static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu("ReGaren", "ReGaren");
            Menu.AddGroupLabel("ReGaren Hosgeldiniz.Ceviri TekinTR!");
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            static Modes()
            {
                // Menu
                Combo.Initialize();
                Drawing.Initialize();
                Farm.Initialize();
                Misc.Initialize();
            }

            public static void Initialize()
            {
            }
        }
    }
}