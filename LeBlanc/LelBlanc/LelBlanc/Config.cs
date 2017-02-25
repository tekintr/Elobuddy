using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace LelBlanc
{
    internal class Config
    {
        /// <summary>
        /// Contains all the Menu's
        /// </summary>
        public static Menu ConfigMenu,
            ComboMenu,
            HarassMenu,
            LaneClearMenu,
            JungleClearMenu,
            KillStealMenu,
            DrawingMenu,
            MiscMenu;

        /// <summary>
        /// Contains Different Modes
        /// </summary>
        private static readonly string[] LogicModes = {"Burst Logic", "Two Chain(TM) Logic", "Chase Burst"};

        /// <summary>
        /// Creates the Menu
        /// </summary>
        public static void Initialize()
        {
            ConfigMenu = MainMenu.AddMenu("LelBlanc", "LelBlanc");
            ConfigMenu.AddGroupLabel("Bu eklenti KarmaPanda tarafından yapıldı ve TekinTR tarafindan Turkce yapildi.");
            ConfigMenu.AddGroupLabel("Yetkisi olmayan izinsiz yeniden dağıtım, ciddi sonuçlar doğuracaktır.");
            ConfigMenu.AddGroupLabel("Bu eklentiyi kullandığınız ve eğlendiğiniz için tesekkür ederiz!");

            ComboMenu = ConfigMenu.AddSubMenu("Combo Menu", "cMenu");
            ComboMenu.AddLabel("Spell Settings");
            ComboMenu.Add("useQ", new CheckBox("Kullan Q"));
            ComboMenu.Add("useW", new CheckBox("Kullan W"));
            ComboMenu.Add("useReturn", new CheckBox("Kullan W Dönüs"));
            ComboMenu.Add("useE", new CheckBox("Kullan E"));
            ComboMenu.AddLabel("R Settings");
            ComboMenu.Add("useQR", new CheckBox("Kullan QR"));
            ComboMenu.Add("useWR", new CheckBox("Kullan WR", false));
            ComboMenu.Add("useReturn2", new CheckBox("Kullan WR Dönüs", false));
            ComboMenu.Add("useER", new CheckBox("Kullan ER", false));
            ComboMenu.AddLabel("Extra Settings");
            ComboMenu.Add("mode", new ComboBox("Kombo Modu", 2, LogicModes));
            ComboMenu.AddLabel("Burst Logic Settings");
            ComboMenu.Add("minRange", new CheckBox("Kullan Q -> R sadece W menzilinde", false));

            HarassMenu = ConfigMenu.AddSubMenu("Harass Menu", "hMenu");
            HarassMenu.AddLabel("Spell Settings");
            HarassMenu.Add("useQ", new CheckBox("Kullan Q"));
            HarassMenu.Add("useW", new CheckBox("Kullan W"));
            HarassMenu.Add("useReturn", new CheckBox("Kullan W Dönüs"));
            HarassMenu.Add("useE", new CheckBox("Kullan E"));
            HarassMenu.AddLabel("R Settings");
            HarassMenu.Add("useQR", new CheckBox("Kullan QR"));
            HarassMenu.Add("useWR", new CheckBox("Kullan WR", false));
            HarassMenu.Add("useReturn2", new CheckBox("Kullan WR Dönüs"));
            HarassMenu.Add("useER", new CheckBox("Kullan ER", false));
            HarassMenu.AddLabel("Extra Settings");
            HarassMenu.Add("mode", new ComboBox("Dürtme Modu", 1, LogicModes));
            HarassMenu.AddLabel("Burst Logic Settings");
            HarassMenu.Add("minRange", new CheckBox("Kullan Q -> R sadece W menzilinde", false));

            LaneClearMenu = ConfigMenu.AddSubMenu("Laneclear Menu", "lcMenu");
            LaneClearMenu.AddLabel("Spell Settings");
            LaneClearMenu.Add("useQ", new CheckBox("Kullan Q"));
            LaneClearMenu.Add("useW", new CheckBox("Kullan W"));
            LaneClearMenu.Add("sliderQ", new Slider("Kullan Q ölücekse {0} Minyon", 3, 1, 5));
            LaneClearMenu.Add("sliderW", new Slider("Kullan W ölücekse {0} Minyon", 3, 1, 5));

            /*JungleClearMenu = ConfigMenu.AddSubMenu("Jungleclear Menu", "jcMenu");
            JungleClearMenu.AddLabel("Spell Settings");
            JungleClearMenu.Add("useQ", new CheckBox("Use Q"));
            JungleClearMenu.Add("useW", new CheckBox("Use W"));
            JungleClearMenu.Add("useE", new CheckBox("Use E"));
            JungleClearMenu.Add("sliderW", new Slider("Use W if Hit {0} Minions", 3, 1, 5));
            JungleClearMenu.AddLabel("R Settings");
            JungleClearMenu.Add("useQR", new CheckBox("Use QR"));
            JungleClearMenu.Add("useWR", new CheckBox("Use WR"));
            JungleClearMenu.Add("useER", new CheckBox("Use ER"));
            JungleClearMenu.Add("sliderWR", new Slider("Use WR if Hit {0} Minions", 5, 1, 5));*/

            KillStealMenu = ConfigMenu.AddSubMenu("Killsteal Menu", "ksMenu");
            KillStealMenu.AddLabel("Spell Settings");
            KillStealMenu.Add("useQ", new CheckBox("Kullan Q"));
            KillStealMenu.Add("useW", new CheckBox("Kullan W"));
            KillStealMenu.Add("useReturn", new CheckBox("Kullan W Dönüs"));
            KillStealMenu.Add("useE", new CheckBox("Kullan E"));
            KillStealMenu.AddLabel("R Settings");
            KillStealMenu.Add("useQR", new CheckBox("Kullan QR"));
            KillStealMenu.Add("useWR", new CheckBox("Kullan WR"));
            KillStealMenu.Add("useReturn2", new CheckBox("Kullan WR Dönüs"));
            KillStealMenu.Add("useER", new CheckBox("Kullan ER"));
            KillStealMenu.AddLabel("Misc Settings");
            KillStealMenu.Add("useIgnite", new CheckBox("Kullan Tutustur"));
            KillStealMenu.Add("usePrediction", new CheckBox("Kullan Can tahmini", false));
            KillStealMenu.Add("toggle", new CheckBox("Öldürme sekli aktif"));

            DrawingMenu = ConfigMenu.AddSubMenu("Drawing Menu", "dMenu");
            DrawingMenu.AddLabel("Range Drawings");
            DrawingMenu.Add("drawQ", new CheckBox("Goster Q Menzili", false));
            DrawingMenu.Add("drawW", new CheckBox("Goster W Menzili", false));
            DrawingMenu.Add("drawE", new CheckBox("Goster E Menzili", false));
            DrawingMenu.AddLabel("DamageIndicator");
            DrawingMenu.Add("draw.Damage", new CheckBox("Goster Hasarimi"));
            DrawingMenu.Add("draw.Q", new CheckBox("Hesapla Q Hasarimi"));
            DrawingMenu.Add("draw.W", new CheckBox("Hesapla W Hasarimi"));
            DrawingMenu.Add("draw.E", new CheckBox("Hesapla E Hasarimi"));
            DrawingMenu.Add("draw.R", new CheckBox("Hesapla R Hasarimi"));
            DrawingMenu.Add("draw.Ignite", new CheckBox("Hesapla Tutustur Hasarimi"));

            MiscMenu = ConfigMenu.AddSubMenu("Misc Menu", "mMenu");
            MiscMenu.AddLabel("Miscellaneous");
            MiscMenu.Add("pet", new CheckBox("Klonu otomatik yürüt."));
        }
    }
}