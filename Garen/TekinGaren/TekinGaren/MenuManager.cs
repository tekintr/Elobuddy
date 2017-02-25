using System;
using EloBuddy.SDK.Menu;

namespace TekinGaren
{
    class MenuManager
    {
        public static Menu mainMenu, Combo, LaneClear, LastHit, JungleClear, KillSteal, Rendering, Settings;
        public static int percentHealth;

        public static void Initialize()
        {
            mainMenu = MainMenu.AddMenu("TekinGaren", "mainMenu");
            Combo = mainMenu.AddSubMenu("Combo", "ComboMenu");
            LaneClear = mainMenu.AddSubMenu("LaneClear", "LaneClearMenu");
            LastHit = mainMenu.AddSubMenu("LastHit", "LastHitMenu");
            JungleClear = mainMenu.AddSubMenu("JungleClear", "JungleClearMenu");
            KillSteal = mainMenu.AddSubMenu("KillSteal", "KillStealMenu");
            Rendering = mainMenu.AddSubMenu("Rendering", "RenderingMenu");
            Settings = mainMenu.AddSubMenu("Settings", "SettingsMenu");

            mainMenu.AddGroupLabel("Created by TekinTR");
            mainMenu.AddLabel("Bu addon Garen icin tasarlandi.");
            mainMenu.AddLabel("Bug yada hatami buldun? Benimle iletisime gec.");
            
            Combo.AddGroupLabel("Combo Features");
            Combo.AddCheckBox("comboQ", "Kullan Q");
            Combo.AddCheckBox("comboE", "Kullan E");
            
            LaneClear.AddGroupLabel("LaneClear Features");
            LaneClear.AddCheckBox("laneQ", "Use Q", false);
            LaneClear.AddCheckBox("laneE", "Use E");
            
            LastHit.AddGroupLabel("LastHit Features");
            LastHit.AddCheckBox("lasthitQ", "Kullan Q");
            
            JungleClear.AddGroupLabel("JungleClear Features");
            JungleClear.AddCheckBox("jungleQ", "Kullan Q");
            JungleClear.AddCheckBox("jungleE", "Kullan E");
            
            KillSteal.AddGroupLabel("KillSteal Features");
            KillSteal.AddCheckBox("ksAA", "Oto atak ile KS");
            KillSteal.AddCheckBox("ksQ", "Q ile KS");
            KillSteal.AddCheckBox("ksR", "R ile KS");
            
            Rendering.AddGroupLabel("Rendering Features");
            //Rendering.AddCheckBox("renderP", "Render Player HP Bar");
            //Rendering.AddCheckBox("renderA", "Render Ally HP Bar");
            Rendering.AddCheckBox("renderE", "Dusman HP bar goster");
            Rendering.AddCheckBox("killable", "Goster 'Olucek' yazisi");
            Rendering.AddLabel("Rendering Configurations");
            Rendering.AddCheckBox("renderS_dmg", "Goster Skill hasari");
            Rendering.AddCheckBox("renderI_dmg", "Goster Item hasari");
            Rendering.AddSlider("renderAA", "Goster Basit Atak hasari", 2, 0, 5);
            //Rendering.AddCheckBox("renderS_heal", "Render Spell healing");
            //Rendering.AddCheckBox("renderI_heal", "Render Item healing");

            Settings.AddGroupLabel("Settings Features");
            Settings.AddCheckBox("cleanseQ", "Yavaslatma temizle Q", false);
            Settings.AddCheckBox("destroy", "Q ile yapilari yok et");
            Settings.AddSlider("percentQ", "Kaydet Q ks icin can yüzdesi iken >= ", 35, 0, 100);

            Settings.GetSliderObject("percentQ").OnValueChange += MenuManager_OnValueChange; ;

            Console.WriteLine("MenuManager initialized.");
        }

        static void MenuManager_OnValueChange(EloBuddy.SDK.Menu.Values.ValueBase<int> sender,
            EloBuddy.SDK.Menu.Values.ValueBase<int>.ValueChangeArgs args)
        {
            percentHealth = args.NewValue;
        }
    }
}
