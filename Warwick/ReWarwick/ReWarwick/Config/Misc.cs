using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReWarwick.Utils;

namespace ReWarwick.Config
{
    public static class Misc
    {
        public static readonly Menu Menu;

        static Misc()
        {
            Menu = MenuLoader.Menu.AddSubMenu("Misc");
            Menu.AddGroupLabel("Misc settings");

            Menu.AddGroupLabel("Kill Steal");
            Menu.CreateCheckBox("Oldururken Q", "Config.Misc.KillSteal.Q");

            Menu.AddGroupLabel("Skin manager");
            Menu.CreateCheckBox("Skin secici aktif", "Misc.Skin.Status");
            Menu.CreateSlider("Skinini sec", "Misc.Skin.Id", 0, 0, 8);

            Menu.AddGroupLabel("Humanizer");
            Menu.CreateCheckBox("Insancil Aktif", "Misc.Humanizer.Status", false);
            Menu.CreateSlider("Skiller arasindaki gecis gecikmesini sec (ms).", "Misc.Humanizer.Delay", 200, 50, 500);
            Menu.CreateSlider("Ek rastgele gecikme (ms).", "Misc.Humanizer.RandomDelay", 75, 50, 100);

            Menu.AddGroupLabel("Another settings");
            Menu.CreateCheckBox("Atilma onleyici aktif", "Config.Misc.Another.Gapcloser");
            Menu.CreateCheckBox("Engelleyici aktif", "Config.Misc.Another.Interrupter");
            Menu.CreateSlider("Atilma onleyici / Engelleyici", "Config.Misc.Another.Delay", 0, 0, 500);

            #region Disable / enable skin changer
            if (Menu.Get<CheckBox>("Misc.Skin.Status").CurrentValue)
                Player.Instance.SetSkinId(Menu.GetSliderValue("Misc.Skin.Id"));

            Menu.Get<CheckBox>("Misc.Skin.Status").OnValueChange += (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args) =>
            {
                if (args.NewValue == false) Player.Instance.SetSkinId(0);
                else Player.Instance.SetSkinId(Menu.GetSliderValue("Misc.Skin.Id"));
            };

            Menu.Get<Slider>("Misc.Skin.Id").OnValueChange += (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args) =>
            {
                if (Menu.GetCheckBoxValue("Misc.Skin.Status")) Player.Instance.SetSkinId(Menu.GetSliderValue("Misc.Skin.Id"));
            };
            #endregion
        }


        public static void Initialize()
        {
        }
    }
}