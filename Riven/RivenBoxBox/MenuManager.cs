using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Linq;

namespace RivenBoxBox
{
    class MenuManager : MenuBase
    {
        public static OrbHelper.Orbwalker Orbwalkerhep { get; set; }
        public static void LoadMenu()
        {
            Main = MainMenu.AddMenu("Riven", "Riven");
            Orbwalkerhep = new OrbHelper.Orbwalker();
            comboMenu = Main.AddSubMenu("Combo");
            comboMenu.Add("combokey", new KeyBind("Combo", false, KeyBind.BindTypes.HoldActive, 32));
            comboMenu.Add("manualCancel", new CheckBox("Animasyon iptali (Yarım)"));
            comboMenu.Add("manualCancelPing", new CheckBox("Animasyonu iptal ederken ping'i hesapla?"));
            comboMenu.AddGroupLabel("Q Ayarlari");
            comboMenu.Add("Q3Wall", new CheckBox("Q3 Duvaların üzerinden", true));
            comboMenu.Add("keepq", new CheckBox("Q'nun süresi bitmeden kullan", true));
            comboMenu.Add("useQgap", new CheckBox("Atılma yapan olursa oto Q", true));
            comboMenu.Add("gaptimeQ", new Slider("Q gecikmesi (ms)", 115, 0, 200));
            comboMenu.Add("safeq", new CheckBox("Birden fazla düşmanı Q ile engelle", true));
            //comboMenu.Add("Qtimer", new CheckBox("Delay Q Manual", false));
            comboMenu.Add("TheshyQ", new CheckBox("The Shy Combo", false));
            //comboMenu.Add("QD", new Slider("First,Second Q Delay", 29, 0, 29));
            //comboMenu.Add("QLD", new Slider("Third Q Delay", 39, 0, 39));

            comboMenu.AddGroupLabel("W Ayarları");
            comboMenu.Add("usecombow", new CheckBox("Komboda W kullan", true));
            comboMenu.Add("ComboWLogic", new CheckBox("W mantığı kullan", true));
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.Team != Player.Instance.Team))
                comboMenu.Add("w" + enemy.ChampionName, new CheckBox("W'yu sadece şu şampiyona kullan : " + enemy.ChampionName));
                       
            comboMenu.AddGroupLabel("E Ayarları");
            comboMenu.Add("ComboEGap", new CheckBox("Atılma yapan olursa oto E", true));
            comboMenu.Add("usecomboe", new CheckBox("Komboda E kullan", true));
            comboMenu.Add("vhealth", new Slider("E kullanmak icin kalan Can% <=", 60));
            comboMenu.Add("safee", new CheckBox("Birden fazla dusman varsa güvenlik icin E", true));

            comboMenu.AddGroupLabel("R1 Ayarları");
            comboMenu.Add("useignote", new CheckBox("Komboda Tutuştur kullan", true));
            comboMenu.Add("user", new KeyBind("Komboda R1 kullan", false, KeyBind.BindTypes.PressToggle, 'H'));
            comboMenu.Add("ultwhen", new ComboBox("R1'i kullanma şekli", new[] { "Normal", "Zor öldürme", "Sürekli" }, 2));
            comboMenu.Add("overk", new Slider("hedefin canı şu kadarsa R1 kullanma % <=", 25, 1, 99));
            comboMenu.Add("userq", new Slider("Kullanılıcak Q sayısı <=", 2, 1, 3));
            comboMenu.Add("multib", new ComboBox("Atılma durumu", new[] { "Hasar yetiyorsa", "Sürekli" }, 1));
            comboMenu.Add("flashb", new CheckBox("-> Sıçra ile atıl", true));
            comboMenu.AddGroupLabel("R2 Ayarları");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.Team != Player.Instance.Team))
                comboMenu.Add("r" + enemy.ChampionName, new CheckBox("R2 isabet edicekse kullan : " + enemy.ChampionName));


            comboMenu.Add("usews", new CheckBox("Komboda R2 kullan", true));
            comboMenu.Add("rhitc", new ComboBox("-> İsabet oranı", new[] { "Orta", "Yüksek", "Çok Yüksek" }, 2));
            comboMenu.Add("logicR", new CheckBox("R2 mantığı", false));
            comboMenu.Add("wsmode", new ComboBox("R2 durumları", new[] {"Sadece öldürmede", "En fazla hasar" }, 1));
            comboMenu.Add("keepr", new CheckBox("Süresi bitmeden önce R2 kullan"));


            harassMenu = Main.AddSubMenu("Dürtme");
            harassMenu.Add("useharassw", new CheckBox("Dürtmede W kullan"));
            harassMenu.Add("usegaph", new CheckBox("Dürtmede E kullan"));
            harassMenu.Add("qtoo", new ComboBox("Kaçarken Q kullan:", new[] { "Hedeften uzağa", "Takım kulesine", "Fare konumuna" }, 1));
            harassMenu.Add("useitemh", new CheckBox("Use Tiamat/Hydra", true));
            harassMenu.Add("semiq", new CheckBox("Auto Q Harass/Jungle", true));


            miscMenu = Main.AddSubMenu("Çeşitli");
            miscMenu.Add("shycombo", new KeyBind("Atılma kombosu", false, KeyBind.BindTypes.HoldActive, 'T'));
            miscMenu.Add("qint", new CheckBox("Q3 ile engelle", true));
            miscMenu.Add("wint", new CheckBox("Engelleyici kullan", true));
            miscMenu.Add("wgap", new CheckBox("Atılma önleyici kullan", true));
            miscMenu.Add("dodge", new CheckBox("Dodge E", true));
            miscMenu.Add("WallFlee", new CheckBox("Kaçarken duvarları kullan", true));
            miscMenu.Add("skinHack", new CheckBox("Kostüm Seç"));
            miscMenu.Add("SkinID", new Slider("Skin", 0, 0, 8));


            farmMenu = Main.AddSubMenu("Farm");
            farmMenu.AddGroupLabel("Orman");
            farmMenu.Add("usejungleq", new CheckBox("Q kullan", true));
            farmMenu.Add("usejunglew", new CheckBox("W kullan", true));
            farmMenu.Add("usejunglee", new CheckBox("E kullan", true));
           
            farmMenu.AddGroupLabel("Koridor");
            farmMenu.Add("clearnearenemy", new CheckBox("Dont Clear Near Enemy", true));
            farmMenu.Add("uselaneq", new CheckBox("Use Q in WaveClear", true));
            farmMenu.Add("uselanew", new CheckBox("Use W in WaveClear", true));
            farmMenu.Add("usewlaneaa", new CheckBox("Use W on Unkillable Minion", true));
            farmMenu.Add("wminion", new Slider("Use W in WaveClear Minions >=", 3, 0, 5));
            farmMenu.Add("uselanee", new CheckBox("Use E in WaveClear", true));

            drawMenu = Main.AddSubMenu("Cizimler");
            drawMenu.Add("drawAlwaysR", new CheckBox("Her zaman R durumunu göster", false));
            drawMenu.Add("drawTimer1", new CheckBox("Q'nun bitiş süresi", false));
            drawMenu.Add("drawTimer2", new CheckBox("R'nin bitiş süresi", true));
            drawMenu.Add("drawengage", new CheckBox("Yakalama mesafesi göster", false));
            drawMenu.Add("drawr2", new CheckBox("R2 menzilini göster", false));
            drawMenu.Add("drawburst", new CheckBox("Atılma menzilini göster", false));
            drawMenu.Add("drawf", new CheckBox("Hedefimi göster", true));
            drawMenu.Add("draGetWDamage", new CheckBox("Hasarımı göster", true));
            drawMenu.Add("fleeSpot", new CheckBox("Kaçış noktalarını göster", true));
        }
    }
}
