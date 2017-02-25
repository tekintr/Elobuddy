using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace PandaTeemoReborn
{
    internal class Config
    {        
        public static Menu PandaTeemoReborn,
            ComboMenu,
            HarassMenu,
            LaneClearMenu,
            JungleClearMenu,
            KillStealMenu,
            FleeMenu,
            DrawingMenu,
            MiscMenu,
            AutoShroomMenu;

        static Config()
        {
            PandaTeemoReborn = MainMenu.AddMenu("PandaTeemoReborn", "PTR");
            PandaTeemoReborn.AddGroupLabel("Yapimci KarmaPanda.");
            PandaTeemoReborn.AddGroupLabel("Turkce ceviri TekinTR.");
            PandaTeemoReborn.AddGroupLabel("Herhangi bir sorunla karsilasirsan TekinTR ile iletisime gec!");
            
            ComboMenu = PandaTeemoReborn.AddSubMenu("Combo", "Combo");
            ComboMenu.AddLabel("Spell Settings");
            ComboMenu.Add("useQ", new CheckBox("Kullan Q Komboda"));
            ComboMenu.Add("useW", new CheckBox("Kullan W Komboda"));
            ComboMenu.Add("useR", new CheckBox("Kullan R Komboda"));
            ComboMenu.AddLabel("ManaManager");
            ComboMenu.Add("manaQ", new Slider("Mana kullanmadan once Q"));
            ComboMenu.Add("manaW", new Slider("Mana kullanmadan once W"));
            ComboMenu.Add("manaR", new Slider("Mana kullanmadan once R"));
            ComboMenu.AddLabel("Q Settings");
            ComboMenu.Add("checkAA", new Slider("Menzilden cikarma araligi Q: {0}", 0, 0, 180));
            ComboMenu.AddLabel("R Settings");
            ComboMenu.Add("doubleShroom", new CheckBox("Kullan cift mantar mantigi"));
            ComboMenu.Add("rPoison", new CheckBox("Kullan R sadece hedef zehirlenmemisse"));
            ComboMenu.Add("rCharge", new Slider("Sayisi R kullanmadan once R: {0}", 2, 1, 3));
            //ComboMenu.Add("rDelay", new Slider("Delay for R Casting in ms: {0}", 1000, 0, 5000));
            ComboMenu.AddLabel("Misc Settings");
            ComboMenu.Add("adc", new CheckBox("Kullanilsin Q sadece ADC icin", false));
            ComboMenu.Add("wRange", new CheckBox("Kullan W sadece dusman menzilde ise"));

            HarassMenu = PandaTeemoReborn.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Spell Settings");
            HarassMenu.Add("useQ", new CheckBox("Kullan Q durterken"));
            HarassMenu.Add("useW", new CheckBox("Kullan W durterken", false));
            HarassMenu.AddLabel("ManaManager");
            HarassMenu.Add("manaQ", new Slider("Mana kullanmadan once Q"));
            HarassMenu.Add("manaW", new Slider("Mana kullanmadan once W"));
            HarassMenu.AddLabel("Q Settings");
            HarassMenu.Add("checkAA", new Slider("Menzilden cikarma araligi Q: {0}", 0, 0, 180));
            HarassMenu.AddLabel("Misc Settings");
            HarassMenu.Add("adc", new CheckBox("Kullanilsin Q sadece ADC icin", false));
            HarassMenu.Add("wRange", new CheckBox("Kullan W sadece dusman menzilde ise"));

            LaneClearMenu = PandaTeemoReborn.AddSubMenu("LaneClear", "LaneClear");
            LaneClearMenu.AddLabel("Spell Settings");
            LaneClearMenu.Add("useQ", new CheckBox("Koridor temizlemede Q"));
            LaneClearMenu.Add("useR", new CheckBox("Koridor temizlemede R"));
            LaneClearMenu.AddLabel("ManaManager");
            LaneClearMenu.Add("manaQ", new Slider("Q Mana yardimcisi", 50));
            LaneClearMenu.Add("manaR", new Slider("R Mana yardimcisi", 50));
            LaneClearMenu.AddLabel("Q Settings");
            LaneClearMenu.Add("harass", new CheckBox("Kullan durtme mantigi"));
            LaneClearMenu.Add("disableLC", new CheckBox("Koridor temizleme mantigini kapat"));
            LaneClearMenu.AddLabel("R Settings");
            LaneClearMenu.Add("rKillable", new CheckBox("Sadece kullan R minyonlar(s) olucekse"));
            LaneClearMenu.Add("rPoison", new CheckBox("Kullan R sadece minyonlar zehirlenmesse"));
            LaneClearMenu.Add("rCharge", new Slider("Sayisi R kullanmadan once R: {0}", 2, 1, 3));
            //LaneClearMenu.Add("rDelay", new Slider("Delay for R Casting in ms: {0}", 1000, 0, 5000));
            LaneClearMenu.Add("minionR", new Slider("Kac minyon icin kullanilsin R: {0}", 3, 1, 4));

            JungleClearMenu = PandaTeemoReborn.AddSubMenu("JungleClear", "JungleClear");
            JungleClearMenu.AddGroupLabel("Spell Settings");
            JungleClearMenu.Add("useQ", new CheckBox("Ormanda Q"));
            JungleClearMenu.Add("useR", new CheckBox("Ormanda R"));
            JungleClearMenu.AddLabel("ManaManager");
            JungleClearMenu.Add("manaQ", new Slider("Q Mana yardimcisi", 25));
            JungleClearMenu.Add("manaR", new Slider("R Mana yardimcisi", 25));
            JungleClearMenu.AddLabel("R Settings");
            JungleClearMenu.Add("rKillable", new CheckBox("Sadece kullan R yaratik(s) oldurulebilirse", false));
            JungleClearMenu.Add("rPoison", new CheckBox("Kullan R sadece mob zehirlenmesse"));
            JungleClearMenu.Add("rCharge", new Slider("Sayisi R kullanmadan once R: {0}", 2, 1, 3));
            //JungleClearMenu.Add("rDelay", new Slider("Delay for R Casting in ms: {0}", 1000, 0, 5000));
            JungleClearMenu.Add("mobR", new Slider("Yaratik sayisi kullanmadan once R: {0}", 1, 1, 4));
            JungleClearMenu.AddLabel("Misc Settings");
            JungleClearMenu.Add("bMob", new CheckBox("Kucuk yaratiklarda skill kullanimini onle"));

            KillStealMenu = PandaTeemoReborn.AddSubMenu("Kill Steal", "Kill Steal");
            KillStealMenu.AddGroupLabel("Spell Settings");
            KillStealMenu.Add("useQ", new CheckBox("Kullanarak oldur Q"));
            KillStealMenu.Add("useR", new CheckBox("Kullanarak oldur R", false));
            KillStealMenu.AddLabel("ManaManager");
            KillStealMenu.Add("manaQ", new Slider("Q Mana yardimcisi", 25));
            KillStealMenu.Add("manaR", new Slider("R Mana yardimcisi", 25));
            KillStealMenu.AddLabel("R Settings");
            //KillStealMenu.Add("rDelay", new Slider("Delay for R Casting in ms: {0}", 1000, 0, 5000));
            KillStealMenu.Add("doubleShroom", new CheckBox("Kullan mantar sektirme"));

            FleeMenu = PandaTeemoReborn.AddSubMenu("Flee Menu", "Flee");
            FleeMenu.AddGroupLabel("Flee Settings");
            FleeMenu.Add("useW", new CheckBox("Kacarken W kullan"));
            FleeMenu.Add("useR", new CheckBox("Kacarken R kullan"));
            FleeMenu.AddLabel("R Settings");
            //FleeMenu.Add("rDelay", new Slider("Delay for R Casting in ms: {0}", 1000, 0, 5000));
            FleeMenu.Add("rCharge", new Slider("Sayisi R kullanmadan once R: {0}", 2, 1, 3));

            AutoShroomMenu = PandaTeemoReborn.AddSubMenu("Auto Shroom", "Auto Shroom");
            AutoShroomMenu.AddGroupLabel("Auto Shroom Settings");
            AutoShroomMenu.Add("useR", new CheckBox("Otomatik R"));
            AutoShroomMenu.Add("manaR", new Slider("R mana yardimcisi", 25));
            AutoShroomMenu.Add("rCharge", new Slider("Sayisi R kullanmadan once R: {0}", 2, 1, 3));
            AutoShroomMenu.Add("enableShroom", new CheckBox("Yukle otomatik mantar (Basmalisin F5)"));
            AutoShroomMenu.Add("enableDefaultLocations", new CheckBox("Varsayilan konumlari kullan (Basmalisin F5)"));
            AutoShroomMenu.AddLabel("Debug Mode");
            var enable = AutoShroomMenu.Add("enableDebug", new CheckBox("Enable Debug Mode", false));
            enable.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (!args.NewValue)
                {
                    Chat.Print("PandaTeemo | Debug Mode Disabled", System.Drawing.Color.LawnGreen);
                }
                else
                {
                    Chat.Print("PandaTeemo | Debug Mode Enabled", System.Drawing.Color.Red);
                }
            };
            var save = AutoShroomMenu.Add("saveButton", new KeyBind("Save Configuration", false, KeyBind.BindTypes.HoldActive, 'K'));
            save.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (!args.NewValue)
                {
                    return;
                }

                if (Extensions.MenuValues.AutoShroom.DebugMode)
                {
                    save.CurrentValue = false;
                    AutoShroom.SavePositions();
                }
            };
            AutoShroomMenu.AddLabel("Shroom Location Adder");
            AutoShroomMenu.Add("posMode", new ComboBox("Position Mode", 0, "Save Mouse", "Save Player Position"));
            var add = AutoShroomMenu.Add("newposButton", new KeyBind("Save Position", false, KeyBind.BindTypes.HoldActive, 'L'));
            add.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (!args.NewValue)
                {
                    return;
                }

                if (Extensions.MenuValues.AutoShroom.DebugMode)
                {
                    add.CurrentValue = false;

                    Vector3 newPosition = Vector3.Zero;

                    switch (Extensions.MenuValues.AutoShroom.PositionMode.CurrentValue)
                    {
                        case 0:
                            newPosition = Game.CursorPos;
                            break;
                        case 1:
                            newPosition = Player.Instance.Position;
                            break;
                    }

                    if (newPosition != Vector3.Zero && !AutoShroom.ShroomPosition.Contains(newPosition))
                    {
                        AutoShroom.AddShroomLocation(newPosition);
                        AutoShroom.SavePositions();
                    }
                }
            };
            var remove = AutoShroomMenu.Add("delposButton", new KeyBind("Delete Position", false, KeyBind.BindTypes.HoldActive, 'U'));
            remove.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (!args.NewValue)
                {
                    return;
                }

                if (Extensions.MenuValues.AutoShroom.DebugMode)
                {
                    remove.CurrentValue = false;
                }

                Vector3 newPosition = Vector3.Zero;

                switch (Extensions.MenuValues.AutoShroom.PositionMode.CurrentValue)
                {
                    case 0:
                        newPosition = Game.CursorPos;
                        break;
                    case 1:
                        newPosition = Player.Instance.Position;
                        break;
                }

                if (newPosition == Vector3.Zero) return;

                var nearbyShrooms = AutoShroom.PlayerAssignedShroomPosition.Where(pos => pos.IsInRange(newPosition, SpellManager.R.Radius)).ToList();

                if (!nearbyShrooms.Any())
                {
                    return;
                }

                AutoShroom.RemoveShroomLocations(nearbyShrooms);
                AutoShroom.SavePositions();
            };

            DrawingMenu = PandaTeemoReborn.AddSubMenu("Drawing", "Drawing");
            DrawingMenu.AddGroupLabel("Drawing Settings");
            DrawingMenu.Add("drawQ", new CheckBox("Goster Q Menzili"));
            DrawingMenu.Add("drawR", new CheckBox("Goster R Menzili"));
            DrawingMenu.Add("drawautoR", new CheckBox("Goster oto mantar pozisyonlari"));
            DrawingMenu.Add("drawdoubleR", new CheckBox("Goster cift mantar tahmini", false));
            
            MiscMenu = PandaTeemoReborn.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Spell Settings");
            MiscMenu.Add("autoQ", new CheckBox("Otomatik Q", false));
            MiscMenu.Add("autoW", new CheckBox("Otomatik W", false));
            MiscMenu.Add("intq", new CheckBox("Kesici Q"));
            MiscMenu.Add("gapR", new CheckBox("Atilma yapanlara R"));
        }

        public static void Initialize()
        {
        }
    }
}