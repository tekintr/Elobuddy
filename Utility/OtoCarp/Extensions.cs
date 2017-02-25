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
    public class Extensions
    {
        public static Spell.Targeted Smite;
        public static float Damage(Obj_AI_Base T) => Player.Instance.GetSummonerSpellDamage(T, DamageLibrary.SummonerSpells.Smite);

        public static GameMapId Map = Game.MapId;

        public static string[] MNames = 
        {
            "SRU_Krug", "Sru_Crab", "SRU_Baron", "SRU_Blue", "SRU_Gromp",
            "SRU_Murkwolf", "SRU_Razorbeak", "SRU_Red",
            "SRU_RiftHerald"
        };

        public static string[] DNames =
        {
            "SRU_Dragon_Air", "SRU_Dragon_Earth",
            "SRU_Dragon_Fire", "SRU_Dragon_Water", "SRU_Dragon_Elder"
        };
             
        public static bool CheckBox(Menu m, string s)
        {
            return m[s].Cast<CheckBox>().CurrentValue;
        }

        public static int Slider(Menu m, string s)
        {
            return m[s].Cast<Slider>().CurrentValue;
        }

        public static bool Keybind(Menu m, string s)
        {
            return m[s].Cast<KeyBind>().CurrentValue;
        }

        public static int ComboBox(Menu m, string s)
        {
            return m[s].Cast<ComboBox>().SelectedIndex;
        }
    }
}
