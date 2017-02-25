using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using EloBuddy.SDK.Rendering;

namespace OtoCarp
{
    class Settings
    {
        public static Menu Principal;

        public static void Load()
        {
            Principal = MainMenu.AddMenu("OtoCarp", "OtoCarp");
            Principal.Add("Enable", new KeyBind("Carp Aktif?", false, KeyBind.BindTypes.PressToggle, 'H'));
            Principal.Add("Draw", new CheckBox("Cizimler Aktif?"));
            Principal.AddSeparator(2);

            if (Extensions.Map == GameMapId.SummonersRift)
            {
                Principal.AddGroupLabel("Dragons");
                Principal.Add("SRU_Dragon_Air", new CheckBox("Bulut Ejderi?"));
                Principal.Add("SRU_Dragon_Fire", new CheckBox("Alev Ejderi?"));
                Principal.Add("SRU_Dragon_Earth", new CheckBox("Torpak Ejderi?"));
                Principal.Add("SRU_Dragon_Water", new CheckBox("Su Ejderi?"));
                Principal.Add("SRU_Dragon_Elder", new CheckBox("Kadim Ejderi?"));
                Principal.AddSeparator(2);
                Principal.AddGroupLabel("Big Mobs");
                Principal.Add("SRU_Baron", new CheckBox("Baron?"));
                Principal.Add("SRU_Blue", new CheckBox("Mavi?"));
                Principal.Add("SRU_Red", new CheckBox("Kirmisi?"));
                Principal.Add("SRU_RiftHerald", new CheckBox("Rift Herald?"));
                Principal.AddSeparator(2);
                Principal.AddGroupLabel("Small Mobs");
                Principal.Add("SRU_Gromp", new CheckBox("Gromp?"));
                Principal.Add("SRU_Murkwolf", new CheckBox("Kurtlar?"));
                Principal.Add("SRU_Krug", new CheckBox("Kayacil?"));
                Principal.Add("SRU_Razorbeak", new CheckBox("Tavuk?"));
                Principal.Add("SRU_Crab", new CheckBox("Yengec?"));
            }
        }
    }
}