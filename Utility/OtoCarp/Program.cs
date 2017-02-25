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
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += delegate
            {
                var Smite = Player.Instance.Spellbook.Spells.FirstOrDefault(x => x.Name.ToLower().Contains("smite"));

                if (Smite != null)
                {
                    Settings.Load();
                    Extensions.Smite = new Spell.Targeted(Smite.Slot, 570);
                    Game.OnTick += OtoCarp.Load;
                    Drawing.OnEndScene += OtoCarp.Draw;
                }
            };
        }
    }
}