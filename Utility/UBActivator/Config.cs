using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Notifications;
using EloBuddy.SDK.Menu.Values;

namespace UBActivator
{
    class Config
    {
        public static Menu Menu { get; private set; }
        public static Menu Potions { get; private set; }
        public static Menu Offensive { get; private set; }
        public static Menu Defensive { get; private set; }
        public static Menu Combat { get; private set; }
        public static Menu Clean { get; private set; }
        public static Menu Spell { get; private set; }
        public static Menu Ward { get; private set; }
        public static Menu Utility { get; private set; }
        public static Menu Level { get; private set; }
        public static int[] SkillOrder;
        public static CheckBox OnTickButton;
        public static CheckBox OnUpdateButton;
        public static CheckBox RandomButton;
        public static CheckBox WardButton;
        public static CheckBox CleanDelay;
        public static Slider SkinSlider;
        public static Slider CleanSlider;
        public static Slider WardSlider;
        public static Slider RandomSlider;
        public static Slider FOTMSlider;
        public static Slider SolariSlider;
        public static Slider SerSlider;
        public static Label SerLabel;
        public static Label FOTMLabel;
        public static Label SolariLabel;

        public static void Dattenosa()
        {
            Menu = MainMenu.AddMenu("UBActivator", "UB Activator");
            Menu.AddGroupLabel("Made by Uzumaki Boruto");
            Menu.AddLabel("Dattenosa");
            Menu.AddLabel("Global Settings");
            Menu.AddLabel("Must F5 to take effect");
            Menu.AddLabel("Uncheck both won't load anything");
            OnTickButton = Menu.Add("Ontick", new CheckBox("Use Game.OnTick (More fps)"));
            OnTickButton.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (args.NewValue)
                {
                    OnUpdateButton.CurrentValue = false;
                    return;
                }
                if (!OnUpdateButton.CurrentValue)
                {
                    OnTickButton.CurrentValue = true;
                }
            };
            OnUpdateButton = Menu.Add("OnUpdate", new CheckBox("Use Game.OnUpdate (Faster rection)", false));
            OnUpdateButton.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (args.NewValue)
                {
                    OnTickButton.CurrentValue = false;
                    return;
                }
                if (!OnTickButton.CurrentValue)
                {
                    OnUpdateButton.CurrentValue = true;
                }
            };

            /*
            Menu.AddLabel("Global Settings");
            Menu.AddLabel("Must F5 to take effect");          
            Menu.Add("Potions", new ComboBox("Potions Menus will be", 1, "Simple", "Details"));
            Menu.Add("Offensive", new ComboBox("Offensive Menus will be", 0, "Simple", "Details"));
            Menu.Add("Defensive", new ComboBox("Defensive Menus will be", 0, "Simple", "Details"));
            Menu.Add("Clean", new ComboBox("Clean Menus will be", 0, "Simple", "Details"));
            Menu.Add("Spell", new ComboBox("Spell Menus will be", 0, "Simple", "Details"));
            Menu.Add("Ward", new ComboBox("Ward Menus will be", 0, "Simple", "Details"));
            Menu.Add("Utility", new ComboBox("Utility Menus will be", 0, "Simple", "Details"));*/


            Potions = Menu.AddSubMenu("Potions Config");
            {
                Potions.Add("ePotions", new CheckBox("Oto iksir aktif"));
                Potions.Add("preHPrecall", new CheckBox("Base atarken iksir kullanmayi onle"));
                Potions.Add("inshopHP", new CheckBox("Basede iksir kullanmayi onle"));
                Potions.Add("predHP", new CheckBox("Tahmin kullan", false));
                Potions.AddSeparator();

                Potions.Add("HP", new CheckBox("Can pozisyonu"));
                Potions.Add("HPH", new Slider("HP az ise {0}% iksir kullan", 65, 0, 100));
                Potions.AddSeparator();

                Potions.Add("Biscuit", new CheckBox("Biskuvi"));
                Potions.Add("BiscuitH", new Slider("HP az ise {0}% iksir kullan", 60, 0, 100));
                Potions.AddSeparator();

                Potions.Add("RP", new CheckBox("Yenilenebilir iksir"));
                Potions.Add("RPH", new Slider("HP az ise {0}% iksir kullan", 65, 0, 100));
                Potions.AddSeparator();

                Potions.Add("CP", new CheckBox("Musubet iksiri"));
                Potions.Add("MPCP", new CheckBox("Iksir kunllanmayi onle geri yukselince MP azami MP"));
                Potions.Add("CPH", new Slider("HP az ise {0}% iksir kullan", 65, 0, 100));
                Potions.AddSeparator();

                Potions.Add("HTP", new CheckBox("Avci iksiri"));
                Potions.Add("MPHTP", new CheckBox("Iksir kunllanmayi onle geri yukselince MP azami MP"));
                Potions.Add("HTPH", new Slider("HP az ise {0}% iksir kullan", 75, 0, 100));
            }

            Offensive = Menu.AddSubMenu("Offensive Config");
            {
                Offensive.AddGroupLabel("Targeted Item");
                Offensive.Add("cbitem", new CheckBox("Yanlizca komboda aktif"));
                Offensive.Add("BC", new CheckBox("Kullan Bilgewater Palasi"));
                Offensive.Add("Bork", new CheckBox("Kullan Mahvolmus"));
                Offensive.Add("HG", new CheckBox("Kullan Hextech Silahkilic"));
                Offensive.Add("MyHPT", new Slider("My HP", 80));
                Offensive.Add("TargetHPT", new Slider("Hedef HP", 80));
                Offensive.AddSeparator();
                Offensive.Add("HGks", new CheckBox("Hextech Silahkilic ile oldur"));
                Offensive.Add("BCks", new CheckBox("Bilgewater Palasi ile oldur"));
                Offensive.Add("Borkks", new CheckBox("Mahvolmus ile oldur"));
                Offensive.AddGroupLabel("AOE Item");
                Offensive.AddLabel("Versus Champion");
                Offensive.Add("Tiamat", new ComboBox("Kullan Tiamat/Hydra/Titanic", 4, "Yok", "Sadece durt", "Sadece kombo", "Her ikiside", "Oto kullan"));
                Offensive.Add("TiamatSlider", new Slider("{0}% hasarda kullan Tiamat/Hydra", 75));
                Offensive.Add("styletitanic", new ComboBox("Kullan Titanic Hydra:", 0, "Saldiridan sonra", "Saldiridan once"));
                Offensive.Add("TiamatKs", new CheckBox("Kullan Tiamat/Hydra ile oldur"));
                Offensive.AddLabel("Versus Minion");
                Offensive.Add("TiamatLc", new CheckBox("Kullan Tiamat/Hydra/Titanic koridor temizleme"));
                Offensive.Add("TiamatLccount", new Slider("Kullan sadece cevremdeki ≥ {0} minyonlara(s)", 4, 1, 10));
                Offensive.Add("TiamatLh", new CheckBox("Kullan olmicek minyona"));
                Offensive.AddLabel("Versus Monster");
                Offensive.Add("TiamatJc", new CheckBox("Kullan Ormanda"));
                Offensive.Add("TiamatJccount", new Slider("Kullan sadece cevremdeki ≥ {0} monster(s), kullan herhangi birine Baron/Dragon/Herald", 2, 1, 4));
                Offensive.AddGroupLabel("Movement Item");
                Offensive.Add("cbmvitem", new CheckBox("Sadece komboda kullan"));
                Offensive.Add("Youmuu", new CheckBox("Youmuu'nun Hayalet kilici"));
                Offensive.AddSeparator();
                Offensive.Add("Hextech01", new CheckBox("Kullan HT_Protobelt_01"));
                Offensive.Add("Hextech01Ks", new CheckBox("Kullan HT_Protobelt_01 oldurmede"));
                Offensive.Add("Hextech01gap", new ComboBox("Kullan HT_Protobelt_01 on GapCloser", 2, "Yok", "Fareye dogru", "Hedefe dogru"));
                Offensive.Add("Hextech01agap", new CheckBox("Kullan HT_Protobelt_01 on anti GapCloser", false));
                Offensive.AddSeparator();
                Offensive.Add("Hextech800style", new ComboBox("Use HT_GLP_800 On:", 3, "Hic", "Sadece Anti GapCloser", "Sadece oldurmede", " ikiside"));

            }

            Defensive = Menu.AddSubMenu("Defensive Config");
            {
                if (!Extensions.ChampNoMana)
                {
                    Defensive.AddGroupLabel("Seraph's Embrace Setting");
                    Defensive.Add("Ser", new CheckBox("Kullan Seraph'ın Sevkati"));
                    Defensive.Add("Seratt", new CheckBox("Engelle basit atak & buyuler"));
                    SerSlider = Defensive.Add("Sershield", new Slider("Kullan engellemede {0}% kalkan orani", 60, 0, 100));
                    SerSlider.OnValueChange += SerSlider_OnValueChange;
                    SerLabel = Defensive.Add("SerLabel", new Label("Bu, temel saldiri veya buyunun Seraph kullanmasi demek. ≥ " + Math.Round((150 + 0.2 * Player.Instance.Mana) * 60 / 100) + " hasar"));
                    Defensive.AddSeparator();
                }
                Defensive.AddGroupLabel("Face of The Moutain Setting");
                Defensive.Add("Face", new CheckBox("Kullan Dagin sureti"));
                Defensive.Add("Faceatt", new CheckBox("Engelle basit atak & buyuler"));
                FOTMSlider = Defensive.Add("Faceshield", new Slider("Kullan engellemede {0}% kalkan orani", 60, 0, 100));
                FOTMSlider.OnValueChange += FOTMSlider_OnValueChange;
                FOTMLabel = Defensive.Add("FOTMLabel", new Label("Bu, temel saldiri veya buyunun Dagin sureti kullanmasi demek ≥ " + Math.Round(Player.Instance.MaxHealth * 0.1 * 60 / 100) + " hasar"));
                foreach (var Ally in EntityManager.Heroes.Allies)
                {
                    Defensive.Add("Face" + Ally.ChampionName, new CheckBox("Kullan Dagin sureti" + Ally.ChampionName));
                }
                Defensive.AddSeparator();
                Defensive.AddGroupLabel("Solari Setting");
                Defensive.Add("Solari", new CheckBox("Kullan Solari"));
                SolariSlider = Defensive.Add("Solarishield", new Slider("Kullan engellemede {0}% kalkan orani", 60, 0, 100));
                SolariSlider.OnValueChange += SolariSlider_OnValueChange;
                SolariLabel = Defensive.Add("SolariLabel", new Label("Bu, temel saldiri veya buyunun demir solari kullanmasi demek ≥ " + (75 + 15 * Player.Instance.Level) * 60 / 100 + " hasar"));
                foreach (var Ally in EntityManager.Heroes.Allies)
                {
                    Defensive.Add("Solari" + Ally.ChampionName, new CheckBox("Kullan Solari" + Ally.ChampionName));
                }
                Defensive.AddSeparator();
                Defensive.AddGroupLabel("Zhonya Customize");
                Defensive.Add("predtimebool", new CheckBox("Using Prediction to use Zhonya"));
                Defensive.Add("predtime", new Slider("Prediction in {0}0 ms", 70, 1));
                Defensive.Add("percentusagebool", new CheckBox("Percent Health Usage"));
                Defensive.Add("percentusage", new Slider("Use Zhonya if my HP below {0}%", 15));
                Defensive.AddGroupLabel("Block Danger Spells by Zhonya");
                Defensive.Add("enableblock", new CheckBox("Enable deny spell in spells list below", false));
                Defensive.AddSeparator();
            }

            Combat = Menu.AddSubMenu("Combat Item");
            {
                Combat.Add("Randuin", new CheckBox("Kullan Randuin"));
                Combat.Add("RanduinCb", new CheckBox("Sadece komboda"));
                Combat.Add("Randuincount", new Slider("Kullan Randuin varsa {0} dusman", 2, 1, 5));
                Combat.AddSeparator();
                Combat.Add("Glory", new CheckBox("Oto kullan gorkemli zafer"));
                Combat.Add("GloryCb", new CheckBox("Sadece komboda"));
                Combat.Add("Glorycountally", new Slider("Yakalarken kullan {0} takim", 3, 1, 4));

            }

            Clean = Menu.AddSubMenu("Cleanse");
            {
                Clean.Add("enableQSS", new CheckBox("kullan QSS/Mercurial"));
                Clean.Add("enableCleanse", new CheckBox("kullan Temizlemede (Buyu)"));
                Clean.Add("enableMikael", new CheckBox("kullan Mikael takima"));
                foreach (var ally in EntityManager.Heroes.Allies)
                {
                    if (ally.ChampionName != Player.Instance.ChampionName)
                        Clean.Add("mikael" + ally.ChampionName, new CheckBox("Use on " + ally.ChampionName));
                }
                Clean.AddGroupLabel("Auto QSS if :");
                Clean.Add("Airbone", new CheckBox("Havalanma"));
                Clean.Add("Blind", new CheckBox("Kor etme", false));
                Clean.Add("Charm", new CheckBox("Cazibe"));
                Clean.Add("Fear", new CheckBox("Korku"));
                Clean.Add("Nearsight", new CheckBox("Gorus", false));
                Clean.Add("Polymorph", new CheckBox("Donusturme"));
                Clean.Add("Taunt", new CheckBox("Taunt"));
                Clean.Add("Slow", new CheckBox("Yavaslama", false));
                Clean.Add("Stun", new CheckBox("Sabitlenme"));
                Clean.Add("Snare", new CheckBox("Kok"));
                Clean.Add("Suppression", new CheckBox("Durdurma"));
                Clean.Add("Silence", new CheckBox("Susturma", false));
                Clean.AddSeparator();
                CleanDelay = Clean.Add("random", new CheckBox("Rasgele gecikme degerini kullan"));
                CleanDelay.OnValueChange += CleanDelay_OnValueChange;
                CleanSlider = Clean.Add("CCDelay", new Slider("gecikme", 250, 0, 1000));
                Clean.Add("EnemyManager", new CheckBox("Kullan QSS etrafta dusman yoksa bile", false));
            }

            Spell = Menu.AddSubMenu("Spells");
            {
                if (Spells.Smite != null)
                {
                    var notif = new SimpleNotification("UBActivator Notification", "Detected Smite as a summoner spell.");
                    Notifications.Show(notif, 5000);
                    Spell.AddGroupLabel("Smite");
                    Spell.Add("esmite3r", new CheckBox("Kullan carp ormanda"));
                    Spell.AddLabel("Sadece uzerinde Baron/ Dragon/ Alamet");
                    Spell.Add("esmitered", new CheckBox("Kirmizida carp", false));
                    Spell.Add("esmiteblue", new CheckBox("Mavide carp", false));
                    Spell.Add("esmiteKs", new CheckBox("Kullan Carp oldururken"));
                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        Spell.Add("Smite" + enemy.ChampionName, new CheckBox("Kullan Carp " + enemy.ChampionName));
                    }
                }
                if (Spells.Heal != null)
                {
                    var notif = new SimpleNotification("UBActivator Notification", "Detected Heal as a summoner spell.");
                    Notifications.Show(notif, 5000);
                    Spell.AddGroupLabel("Heal");
                    Spell.Add("eHeal", new CheckBox("Sifa kullan"));
                    Spell.Add("myHPHeal", new Slider("Canim sundan dusukse {0}%", 30));
                    Spell.Add("eHealAlly", new CheckBox("Takim icin sifa kullan"));
                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        if (ally.ChampionName != Player.Instance.ChampionName)
                            Spell.Add("heal" + ally.ChampionName, new CheckBox("Heal " + ally.ChampionName));
                    }
                    Spell.Add("allyHPHeal", new Slider("takim cani sundan dusukse {0}% sifa kullan", 15));
                }
                if (Spells.Ignite != null)
                {
                    var notif = new SimpleNotification("UBActivator Notification", "Detected Ignite as a summoner spell.");
                    Notifications.Show(notif, 5000);

                    Spell.AddGroupLabel("Ignite");
                    Spell.Add("eIg", new CheckBox("Kullan oldururken tutustur"));
                    Spell.Add("Igstyle", new ComboBox("Hasar hesaplayici:", 0, "Tam hasar", "Tek hasar"));
                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        Spell.Add("Ig" + enemy.ChampionName, new CheckBox("Uzerinde Tutustur kullan " + enemy.ChampionName));
                    }
                }
            }

            Ward = Menu.AddSubMenu("Auto Reveal");
            {
                Ward.Add("enableward", new CheckBox("Aciklama aktif"));
                Ward.Add("enablebrush", new CheckBox("Anti Bush"));                
                WardButton = Ward.Add("wardhuman", new CheckBox("Rasgele deger kullan", false));
                WardButton.OnValueChange += WardButton_OnValueChange;
                WardSlider = Ward.Add("warddelay", new Slider("Gecikme", 500, 0, 2000));

            }

            Utility = Menu.AddSubMenu("Other Settings");
            {
                if (EntityManager.Heroes.Allies.Any(x => x.Hero == Champion.Thresh && !x.IsMe))
                {
                    var notif = new SimpleNotification("UBActivator Notification", "Detected Thresh in team.");
                    Notifications.Show(notif, 5000);
                    Utility.Add("lantern", new CheckBox("Otomatik fener"));
                }
                Utility.AddGroupLabel("Remind Trinkets");
                Utility.Add("remind", new ComboBox("Bana totem degistirmeyi hatirlat", 2, "Pasif", "Chate Yaz", "Bildir"));
                Utility.AddGroupLabel("Auto Tear");
                Utility.Add("etear", new CheckBox("Oto gozyasi kas"));
                Utility.AddGroupLabel("Mod Skin");
                Utility.Add("eskin", new CheckBox("Skin modu aktif", false));
                SkinSlider = Utility.Add("skin", new Slider("Skin sec", 0, 0, 15));
                SkinSlider.OnValueChange += SkinSlider_OnValueChange;
            }

            Level = Menu.AddSubMenu("Auto Level");
            {
                Level.AddGroupLabel("Auto Level Up");
                Level.Add("lvl", new CheckBox("Oto level yukseltme aktif"));
                RandomButton = Level.Add("lvlrandom", new CheckBox("Rastgele kullan", false));
                var button = Level.Add("reset", new CheckBox("Sifirlamak icin tikla", false));
                button.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                {
                    switch (args.NewValue)
                    {
                        case true:
                            {
                                for (var i = 1; i <= 18; i++)
                                {
                                    Level[i.ToString() + Player.Instance.ChampionName].Cast<ComboBox>().CurrentValue = SkillOrder[i - 1];
                                }
                                button.DisplayName = "Reseted";
                                button.CurrentValue = false;
                            }
                            break;
                        case false:
                            {
                            }
                            break;
                    }
                };
                RandomButton.OnValueChange += RandomButton_OnValueChange;
                RandomSlider = Level.Add("lvldelay", new Slider("Delay", 500, 0, 2000));

                #region Champion Skill Order
                switch (Player.Instance.ChampionName)
                {
                    case "Aatrox":
                        SkillOrder = new[] { 2, 1, 3, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                        break;

                    case "Ahri":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Akali":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Alistar":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Amumu":
                        SkillOrder = new[] { 3, 2, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        break;

                    case "Anivia":
                        SkillOrder = new[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Annie":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Ashe":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "AurelionSol":
                        SkillOrder = new[] { 1, 2, 2, 3, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        break;

                    case "Azir":
                        SkillOrder = new[] { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Bard":
                        SkillOrder = new[] { 1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Blitzcrank":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Brand":
                        SkillOrder = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        break;

                    case "Braum":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Caitlyn":
                        SkillOrder = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Cassiopeia":
                        SkillOrder = new[] { 1, 3, 3, 2, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Chogath":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Corki":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Darius":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Diana":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "DrMundo":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Draven":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Elise":
                        Extensions.ROff = -1;
                        SkillOrder = new[] { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Evelynn":
                        SkillOrder = new[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Ezreal":
                        SkillOrder = new[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Ekko":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "FiddleSticks":
                        SkillOrder = new[] { 2, 3, 1, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        break;

                    case "Fiora":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Fizz":
                        SkillOrder = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Galio":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Gangplank":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Garen":
                        SkillOrder = new[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Gnar":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Gragas":
                        SkillOrder = new[] { 1, 3, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Graves":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Hecarim":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Heimerdinger":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Illaoi":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Ivern":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Irelia":
                        SkillOrder = new[] { 1, 3, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        break;

                    case "Janna":
                        SkillOrder = new[] { 3, 1, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                        break;

                    case "JarvanIV":
                        SkillOrder = new[] { 3, 1, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Jax":
                        SkillOrder = new[] { 3, 2, 1, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        break;

                    case "Jayce":
                        Extensions.ROff = -1;
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 2, 1, 3, 1, 3, 1, 3, 3, 2, 2, 3, 2, 2 };
                        break;

                    case "Jhin":
                        SkillOrder = new[] { 1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Jinx":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Kled":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Karma":
                        Extensions.ROff = -1;
                        SkillOrder = new[] { 1, 3, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Karthus":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Kassadin":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Katarina":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Kalista":
                        SkillOrder = new[] { 3, 1, 2, 3, 3, 4, 1, 3, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Kayle":
                        SkillOrder = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Kennen":
                        SkillOrder = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        break;

                    case "Khazix":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Kindred":
                        SkillOrder = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "KogMaw":
                        SkillOrder = new[] { 2, 3, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        break;

                    case "Leblanc":
                        SkillOrder = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        break;

                    case "LeeSin":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Leona":
                        SkillOrder = new[] { 3, 1, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        break;

                    case "Lissandra":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Lucian":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Lulu":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 1, 4, 2, 2 };
                        break;

                    case "Lux":
                        SkillOrder = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Malphite":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Malzahar":
                        SkillOrder = new[] { 2, 3, 1, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Maokai":
                        SkillOrder = new[] { 3, 1, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "MasterYi":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "MissFortune":
                        SkillOrder = new[] { 1, 2, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        break;

                    case "Mordekaiser":
                        SkillOrder = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Morgana":
                        SkillOrder = new[] { 1, 2, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        break;
                    case "Nami":
                        SkillOrder = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Nasus":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Nautilus":
                        SkillOrder = new[] { 3, 2, 1, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                        break;

                    case "Nidalee":
                        Extensions.ROff = -1;
                        SkillOrder = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Nocturne":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 2, 3, 2, 4, 2, 2 };
                        break;

                    case "Nunu":
                        SkillOrder = new[] { 1, 3, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                        break;

                    case "Olaf":
                        SkillOrder = new[] { 2, 1, 3, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Orianna":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Pantheon":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Poppy":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Quinn":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Rammus":
                        SkillOrder = new[] { 2, 1, 3, 2, 3, 4, 2, 3, 3, 3, 4, 2, 2, 1, 1, 4, 1, 1 };
                        break;

                    case "Renekton":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Rengar":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Riven":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Rumble":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "RekSai":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Ryze":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 1, 3, 3, 2, 2, 2, 2 };
                        break;

                    case "Sejuani":
                        SkillOrder = new[] { 2, 3, 1, 2, 2, 4, 2, 1, 2, 3, 4, 3, 3, 3, 1, 4, 1, 1 };
                        break;

                    case "Shaco":
                        SkillOrder = new[] { 2, 1, 3, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Shen":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Shyvana":
                        SkillOrder = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        break;

                    case "Singed":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Sion":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Sivir":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Skarner":
                        SkillOrder = new[] { 1, 2, 3, 3, 3, 4, 3, 3, 1, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Sona":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Soraka":
                        SkillOrder = new[] { 1, 2, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        break;

                    case "Swain":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Syndra":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Taliyah":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Talon":
                        SkillOrder = new[] { 2, 3, 1, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        break;

                    case "Taric":
                        SkillOrder = new[] { 3, 2, 1, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "TahmKench":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Teemo":
                        SkillOrder = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Thresh":
                        SkillOrder = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Tristana":
                        SkillOrder = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Trundle":
                        SkillOrder = new[] { 1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Tryndamere":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "TwistedFate":
                        SkillOrder = new[] { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 4, 3, 3, 4, 3, 3 };
                        break;

                    case "Twitch":
                        SkillOrder = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Udyr":
                        SkillOrder = new[] { 4, 2, 3, 4, 4, 1, 4, 2, 4, 2, 2, 2, 3, 3, 3, 3, 1, 1 };
                        break;

                    case "Urgot":
                        SkillOrder = new[] { 3, 1, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Varus":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Vayne":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Veigar":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Velkoz":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Vi":
                        SkillOrder = new[] { 3, 1, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Viktor":
                        SkillOrder = new[] { 1, 3, 3, 2, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Vladimir":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Volibear":
                        SkillOrder = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        break;

                    case "Warwick":
                        SkillOrder = new[] { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "MonkeyKing":
                        SkillOrder = new[] { 3, 1, 2, 1, 1, 4, 3, 1, 3, 1, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Xerath":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "XinZhao":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        break;

                    case "Yasuo":
                        SkillOrder = new[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Yorick":
                        SkillOrder = new[] { 2, 3, 1, 3, 3, 4, 3, 2, 3, 1, 4, 2, 1, 2, 1, 4, 2, 1 };
                        break;

                    case "Zac":
                        SkillOrder = new[] { 2, 1, 3, 3, 1, 4, 3, 1, 3, 1, 4, 3, 1, 2, 2, 4, 2, 2 };
                        break;

                    case "Zed":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 3, 1, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Ziggs":
                        SkillOrder = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;

                    case "Zilean":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;
                    case "Zyra":
                        SkillOrder = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        break;
                    default:
                        SkillOrder = new[] { 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4, 0, 0 };
                        break;
                }
                #endregion

                for (var i = 1; i <= 18; i++)
                {
                    Level.Add(i.ToString() + Player.Instance.ChampionName, new ComboBox("Level " + i, SkillOrder[i - 1], "None", "Q", "W", "E", "R"));
                }

            }
        }

        #region Value Change
        static void SkinSlider_OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            if (ObjectManager.Player.SkinId != args.NewValue)
            {
                if (Config.Utility["eskin"].Cast<CheckBox>().CurrentValue)
                {
                    Player.Instance.SetSkinId(args.NewValue);
                }
            }

        }
        static void CleanDelay_OnValueChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            switch (args.NewValue)
            {
                case true:
                    {
                        CleanSlider.DisplayName = "Max Value for Random Delay";
                    }
                    break;
                case false:
                    {
                        CleanSlider.DisplayName = "Delay for auto Cleanse";
                    }
                    break;
            }
        }
        static void SerSlider_OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            SerLabel.DisplayName = "This mean Use Solari if the basic attack or spell ≥ " + Math.Round((150 + 0.2 * Player.Instance.Mana) * args.NewValue / 100) + " damage";
        }
        static void FOTMSlider_OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            FOTMLabel.DisplayName = "This mean Use FOTM if the basic attack or spell ≥ " + Math.Round(Player.Instance.MaxHealth * 0.1 * args.NewValue / 100) + " damage";
        }
        static void SolariSlider_OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            SolariLabel.DisplayName = "This mean Use Solari if the basic attack or spell ≥ " + (75 + 15 * Player.Instance.Level) * args.NewValue / 100 + " damage";
        }
        static void WardButton_OnValueChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            switch (args.NewValue)
            {
                case true:
                    {
                        WardSlider.DisplayName = "Max Value for Random Delay";
                    }
                    break;
                case false:
                    {
                        WardSlider.DisplayName = "Delay for auto reveal";
                    }
                    break;
            }
        }
        private static void RandomButton_OnValueChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            switch (args.NewValue)
            {
                case true:
                    {
                        RandomSlider.DisplayName = "Max Value for Random Delay";
                    }
                    break;
                case false:
                    {
                        RandomSlider.DisplayName = "Delay for auto skill up";
                    }
                    break;
            }
        }
        #endregion
    }
}
