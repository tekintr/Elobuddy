using EloBuddy.SDK.Menu;

namespace Looksharp.Utilities
{
    internal class Load
    {
        public static Menu UtilityMenu;

        public static void Init()
        {
            UtilityMenu = MainMenu.AddMenu("Utility", "Utility");
            UtilityMenu.AddGroupLabel("Information");
            UtilityMenu.AddLabel("Made by Lookaside");

            Structure.Init();
        }
    }
}
