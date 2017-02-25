using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ReGaren.ReCore.ConfigList
{
    public static class Settings
    {
        private static readonly Menu Menu;

        static Settings()
        {
            Menu = Loader.Menu.AddSubMenu("Settings");
            Menu.AddGroupLabel("Settings");
        }

        public static void Initialize()
        {
        }
    }
}