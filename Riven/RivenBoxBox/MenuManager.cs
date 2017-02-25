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
            comboMenu = Main.AddSubMenu("Kombo");
            comboMenu.Add("combokey", new KeyBind("Combo", false, KeyBind.BindTypes.HoldActive, 32));
            comboMenu.Add("manualCancel", new CheckBox("Yari iptal animasyon"));
            comboMenu.Add("manualCancelPing", new CheckBox("Animasyonu iptal et Ping'i hesapla?"));
            comboMenu.AddGroupLabel("Q Ayari");
            comboMenu.Add("Q3Wall", new CheckBox("Q3 Duvarin uzerinden", true));
            comboMenu.Add("keepq", new CheckBox("Kullan Q suresi bitmeden", true));
            comboMenu.Add("useQgap", new CheckBox("Atilma yapana Q", true));
            comboMenu.Add("gaptimeQ", new Slider("Atilma yapana Q Gecikmesi (ms)", 115, 0, 200));
            comboMenu.Add("safeq", new CheckBox("Birden fazla dusmani Q ile engelle", true));
            //comboMenu.Add("Qtimer", new CheckBox("Delay Q Manual", false));
            comboMenu.Add("TheshyQ", new CheckBox("The Shy Combo", false));
            //comboMenu.Add("QD", new Slider("First,Second Q Delay", 29, 0, 29));
            //comboMenu.Add("QLD", new Slider("Third Q Delay", 39, 0, 39));

            comboMenu.AddGroupLabel("W Ayari");
            comboMenu.Add("usecombow", new CheckBox("Kullan W komboda", true));
            comboMenu.Add("ComboWLogic", new CheckBox("Kullan W mantigi", true));
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.Team != Player.Instance.Team))
                comboMenu.Add("w" + enemy.ChampionName, new CheckBox("isabet edicekse W sadece : " + enemy.ChampionName));
                       
            comboMenu.AddGroupLabel("E Ayari");
            comboMenu.Add("ComboEGap", new CheckBox("Kullan E Atilma yapana", true));
            comboMenu.Add("usecomboe", new CheckBox("Kullan E komboda", true));
            comboMenu.Add("vhealth", new Slider("Kullan E eger HP% <=", 60));
            comboMenu.Add("safee", new CheckBox("Birden fazla dusman var ise E ile don", true));

            comboMenu.AddGroupLabel("R1 Ayari");
            comboMenu.Add("useignote", new CheckBox("Komboda Tutustur", true));
            comboMenu.Add("user", new KeyBind("Kullan R1 komboda", false, KeyBind.BindTypes.PressToggle, 'H'));
            comboMenu.Add("ultwhen", new ComboBox("Kullan R1 ne zaman", new[] { "Normal Oldur", "Sert oldur", "Surekli" }, 2));
            comboMenu.Add("overk", new Slider("Kullanma R1 eger hedefin HP % <=", 25, 1, 99));
            comboMenu.Add("userq", new Slider("Kullanilicak Q sayisi <=", 2, 1, 3));
            comboMenu.Add("multib", new ComboBox("Atil ne zaman", new[] { "Hasar yeterli", "Surekli" }, 1));
            comboMenu.Add("flashb", new CheckBox("-> Sicra ile atilma", true));
            comboMenu.AddGroupLabel("R2 Ayari");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.Team != Player.Instance.Team))
                comboMenu.Add("r" + enemy.ChampionName, new CheckBox("Sadece R2 isabet edicekse : " + enemy.ChampionName));


            comboMenu.Add("usews", new CheckBox("Kullan R2 komboda", true));
            comboMenu.Add("rhitc", new ComboBox("-> iSabet Sansi", new[] { "Orta", "Yuksek", "CokYuksek" }, 2));
            comboMenu.Add("logicR", new CheckBox("Mantik R2", false));
            comboMenu.Add("wsmode", new ComboBox("Kullan R2 nezaman", new[] {"Sadece olucekse", "Max Hasar" }, 1));
            comboMenu.Add("keepr", new CheckBox("Use R2 Before Expiry"));


            harassMenu = Main.AddSubMenu("Durtme");
            harassMenu.Add("useharassw", new CheckBox("Kullan W durterken"));
            harassMenu.Add("usegaph", new CheckBox("Kullan E durterken"));
            harassMenu.Add("qtoo", new ComboBox("Kullan Kacis/Kacma:", new[] { "Hedeften uzaga", "Takim kulesine", "Fare Konumuna" }, 1));
            harassMenu.Add("useitemh", new CheckBox("Kullan Tiamat/Hydra", true));
            harassMenu.Add("semiq", new CheckBox("Oto Q Durtme/Orman", true));


            miscMenu = Main.AddSubMenu("Karisik");
            miscMenu.Add("shycombo", new KeyBind("Atilama Kombosu", false, KeyBind.BindTypes.HoldActive, 'T'));
            miscMenu.Add("qint", new CheckBox("Engelle Q ile", true));
            miscMenu.Add("wint", new CheckBox("Kullan engelleyici", true));
            miscMenu.Add("wgap", new CheckBox("Kullan atilma engelleyici", true));
            miscMenu.Add("dodge", new CheckBox("Skillden kac E ile", true));
            miscMenu.Add("WallFlee", new CheckBox("Duvardan Atla Kacarken", true));
            miscMenu.Add("skinHack", new CheckBox("Kostum Sec"));
            miscMenu.Add("SkinID", new Slider("Skin", 0, 0, 8));


            farmMenu = Main.AddSubMenu("Orman");
            farmMenu.AddGroupLabel("Jung Clear");
            farmMenu.Add("usejungleq", new CheckBox("Kullan Q Ormanda", true));
            farmMenu.Add("usejunglew", new CheckBox("Kullan W Ormanda", true));
            farmMenu.Add("usejunglee", new CheckBox("Kullan E Ormanda", true));
           
            farmMenu.AddGroupLabel("KoridorTemizleme");
            farmMenu.Add("clearnearenemy", new CheckBox("Dusman yakinda degil ise minyon temizleme", true));
            farmMenu.Add("uselaneq", new CheckBox("Kullan Q minyon dalgasi temizle", true));
            farmMenu.Add("uselanew", new CheckBox("Kullan W minyon dalgasi temizle", true));
            farmMenu.Add("usewlaneaa", new CheckBox("Oldurulemicek minyona W kullan", true));
            farmMenu.Add("wminion", new Slider("Kullan W minyon dalgasi temizle minyon sayisi >=", 3, 0, 5));
            farmMenu.Add("uselanee", new CheckBox("Kullan E minyon dalgasi temizle", true));

            drawMenu = Main.AddSubMenu("Cizimler");
            drawMenu.Add("drawAlwaysR", new CheckBox("Her zaman R durumunu goster", false));
            drawMenu.Add("drawTimer1", new CheckBox("Goster Q kalan sure", false));
            drawMenu.Add("drawTimer2", new CheckBox("Goster R kalan sure", true));
            drawMenu.Add("drawengage", new CheckBox("Goster Yakalama menzili", false));
            drawMenu.Add("drawr2", new CheckBox("Goster R2 menzili", false));
            drawMenu.Add("drawburst", new CheckBox("Goster Atilma menzili", false));
            drawMenu.Add("drawf", new CheckBox("Goster Hedef", true));
            drawMenu.Add("draGetWDamage", new CheckBox("Goster Kombo hasarini", true));
            drawMenu.Add("fleeSpot", new CheckBox("Goster kacis noktalarini", true));
        }
    }
}
