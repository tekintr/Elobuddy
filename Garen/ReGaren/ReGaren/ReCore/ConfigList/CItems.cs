using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ReGaren.ReCore.Utility;

namespace ReGaren.ReCore.ConfigList
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
            Menu.CreateCheckBox("Use Potions", "Items.Consumer.Potions.Status");
            Menu.CreateCheckBox("Use Health Potion", "Items.Consumer.HealthPotion.Status");
            Menu.CreateCheckBox("Use Refillable Potion", "Items.Consumer.RefillablePotion.Status");
            Menu.CreateCheckBox("Use Hunter's Potion", "Items.Consumer.HunterPotion.Status");
            Menu.CreateCheckBox("Use Corrupting Potion", "Items.Consumer.CorruptingPotion.Status");
            Menu.CreateSlider("Use only if my HP <= {0}%", "Items.Consumer.Health");
            Menu.AddSeparator(15);
            #endregion
        }

        public static void Initialize()
        {
        }
    }
}