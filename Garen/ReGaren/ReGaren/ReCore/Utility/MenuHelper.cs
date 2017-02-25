using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReGaren.ReCore.Utility
{
    public static class MenuHelper
    {
        public static void CreateCheckBox(this Menu menu, string displayName, string uniqueIdentifer, bool defaultValue = true)
        {
            try
            {
                menu.Add(uniqueIdentifer, new CheckBox(displayName, defaultValue));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error creating CheckBox uniqueIdentifer = {0}", uniqueIdentifer);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} Exception caught.", e);
                Console.ResetColor();
            }
        }
        public static void CreateSlider(this Menu menu, string displayName, string uniqueIdentifer, int defaultValue = 0, int minValue = 0, int maxValue = 100)
        {
            try
            {
                menu.Add(uniqueIdentifer, new Slider(displayName, defaultValue, minValue, maxValue));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error creating Slider uniqueIdentifer = {0}", uniqueIdentifer);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} Exception caught.", e);
                Console.ResetColor();
            }
        }
        public static void CreateComboBox(this Menu menu, string displayName, string uniqueIdentifer, List<string> textValues, int defaultIndex = 0)
        {
            try
            {
                menu.Add(uniqueIdentifer, new ComboBox(displayName, textValues, defaultIndex));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error creating ComboBox uniqueIdentifer = {0}", uniqueIdentifer);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} Exception caught.", e);
                Console.ResetColor();
            }
        }
        public static void CreateKeyBind(this Menu menu, string displayName, string uniqueIdentifer, uint defaultKey1 = 'U', uint defaultKey2 = 'I', KeyBind.BindTypes bindType = KeyBind.BindTypes.PressToggle, bool defaultValue = true)
        {
            try
            {
                menu.Add(uniqueIdentifer, new KeyBind(displayName, defaultValue, bindType, defaultKey1, defaultKey2));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error creating KeyBind uniqueIdentifer = {0}", uniqueIdentifer);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} Exception caught.", e);
                Console.ResetColor();
            }
        }

        public static bool GetCheckBoxValue(this Menu menu, string uniqueIdentifer)
        {
            try
            {
                return menu.Get<CheckBox>(uniqueIdentifer).CurrentValue;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error getting CheckBox uniqueIdentifer = {0}", uniqueIdentifer);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} Exception caught.", e);
                Console.ResetColor();
            }
            return false;
        }
        public static int GetSliderValue(this Menu menu, string uniqueIdentifer)
        {
            try
            {
                return menu.Get<Slider>(uniqueIdentifer).CurrentValue;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error getting Slider uniqueIdentifer = {0}", uniqueIdentifer);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} Exception caught.", e);
                Console.ResetColor();
            }
            return -1;
        }
        public static int GetComboBoxValue(this Menu menu, string uniqueIdentifer)
        {
            try
            {
                return menu.Get<ComboBox>(uniqueIdentifer).CurrentValue;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error getting ComboBox uniqueIdentifer = {0}", uniqueIdentifer);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} Exception caught.", e);
                Console.ResetColor();
            }
            return -1;
        }
        public static bool GetKeyBindValue(this Menu menu, string uniqueIdentifer)
        {
            try
            {
                return menu.Get<KeyBind>(uniqueIdentifer).CurrentValue;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error getting KeyBind uniqueIdentifer = {0}", uniqueIdentifer);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} Exception caught.", e);
                Console.ResetColor();
            }
            return false;
        }
    }
}
