using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReWarwick.ReCore.Utility;

namespace ReWarwick.ReCore.Config
{
    public static class CItems
    {
        public static readonly Menu Menu;

        static CItems()
        {
            Menu = Loader.Menu.AddSubMenu("Consumer items");
            Menu.AddGroupLabel("Consumer items settings");
            #region Potions
            Menu.AddGroupLabel("Potions");
            Menu.CreateCheckBox("Kullan iksir", "Items.Consumer.Potions.Status");
            Menu.CreateCheckBox("Kullan can iksiri", "Items.Consumer.HealthPotion.Status");
            Menu.CreateCheckBox("Kullan yenilenebilir iksir", "Items.Consumer.RefillablePotion.Status");
            Menu.CreateCheckBox("Kullan avci iksiri", "Items.Consumer.HunterPotion.Status");
            Menu.CreateCheckBox("Kullan musubet iksiri", "Items.Consumer.CorruptingPotion.Status");
            Menu.CreateSlider("Kullan sadece benim HP <= {0}%", "Items.Consumer.Health");
            Menu.AddSeparator(15);
            #endregion
        }

        public static void Initialize()
        {
        }
    }
}