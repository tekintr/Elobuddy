using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ReGaren.ReCore.ConfigList
{
    public static class DItems
    {
        private static readonly Menu Menu;

        static DItems()
        {
            Menu = Loader.Menu.AddSubMenu("Deffensive items");
            Menu.AddGroupLabel("Deffensive items settings");
        }

        public static void Initialize()
        {
        }
    }
}