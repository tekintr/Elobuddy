using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReKatarina.ReCore.Utility;

namespace ReKatarina.ReCore.ConfigList
{
    public static class Cleansers
    {
        private static readonly Menu Menu;

        static Cleansers()
        {
            Menu = Loader.Menu.AddSubMenu("Cleaners");
            Menu.AddGroupLabel("Cleaners settings");
        }

        public static void Initialize()
        {
        }
    }
}