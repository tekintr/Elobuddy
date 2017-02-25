using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace UBLucian
{
    class Config
    {
        public static Menu Menu;
        public static Menu ESettings;
        public static Menu ComboMenu;
        public static Menu HarassMenu;
        public static Menu LaneClear;
        public static Menu JungleClear;
        public static Menu LasthitMenu;
        public static Menu FleeMenu;
        public static Menu MiscMenu;
        public static Menu DrawMenu;

        public static ComboBox WStyle;
        public static Slider EQHit, ERange;
        public static CheckBox ESafe, EWall, ECorrect, EQ, EPath, EKite, EGap, EGrass;

        public static void Dattenosa()
        {
            Menu = MainMenu.AddMenu("UB Lucian", "UBLucian");
            Menu.AddGroupLabel("Made by Uzumaki Boruto");
            Menu.AddLabel("Dattenosa");

            ESettings = Menu.AddSubMenu("E");
            {               
                ESettings.AddGroupLabel("E General Settings");
                ESafe = ESettings.Add("Esafe", new CheckBox("Guvenli pozisyona E"));
                EWall = ESettings.Add("Ewall", new CheckBox("Duvari kontrol et"));
                ECorrect = ESettings.Add("Ecorrect", new CheckBox("Duzeltmesine izin ver"));
                EQ = ESettings.Add("EQ", new CheckBox("E Q dene", false));
                EQHit = ESettings.Add("EQhit", new Slider("E den sonra Q isabet {0} sampiyona", 4, 1, 5));
                ERange = ESettings.Add("Erange", new Slider("Menzil arttir {0}", 35, 0, 75));
                ESettings.Add("label", new Label("Daha fazla deger, sonraki a.a için daha kolay, daha tehlikeli, önerilir 20~40"));
                EPath = ESettings.Add("Epath", new CheckBox("Dusman karsiti yol"));
                EKite = ESettings.Add("Ekite", new CheckBox("Sampiyona kite dene"));
                EGap = ESettings.Add("Etogap", new CheckBox("Atilma yapana E kullan"));
                EGrass = ESettings.Add("Egrass", new CheckBox("Ota E dene"));
                ESettings.AddSeparator();
                ESettings.AddGroupLabel("E Style Settings");
                var EStyle = ESettings.Add("E", new ComboBox("E style", 1, "E Kullanma", "Nisanci", "FareKonum", "Fareye (Akilli)"));
                EStyle.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    switch (args.NewValue)
                    {
                        case 1:
                            {
                                ESafe.IsVisible = true;
                                ESafe.CurrentValue = true;
                                EWall.IsVisible = true;
                                EWall.CurrentValue = true;
                                ECorrect.IsVisible = true;
                                EQ.IsVisible = true;
                                EQHit.IsVisible = true;
                                ERange.IsVisible = true;
                                ESettings["label"].IsVisible = true;
                                EPath.CurrentValue = true;
                                EPath.IsVisible = true;
                                EKite.CurrentValue = true;
                                EKite.IsVisible = true;
                                EGap.IsVisible = true;
                                EGrass.IsVisible = false;

                            }
                            break;
                        case 2:
                            {
                                ESafe.IsVisible = false;
                                EWall.IsVisible = false;
                                ECorrect.IsVisible = false;
                                EQ.IsVisible = false;
                                EQHit.IsVisible = false;
                                ERange.IsVisible = false;
                                ESettings["label"].IsVisible = false;
                                EPath.IsVisible = false;
                                EKite.IsVisible = false;
                                EGap.IsVisible = false;
                                EGrass.IsVisible = false;
                            }
                            break;
                        case 3:
                            {
                                ESafe.CurrentValue = true;
                                EWall.CurrentValue = false;
                                ECorrect.IsVisible = true;
                                ECorrect.CurrentValue = true;
                                EQ.IsVisible = true;
                                EQHit.IsVisible = true;
                                ERange.IsVisible = false;
                                ESettings["label"].IsVisible = false;
                                EPath.IsVisible = false;
                                EKite.IsVisible = true;
                                EGap.IsVisible = true;
                                EGrass.IsVisible = true;
                            }
                            break;
                    }
                };
            }

            ComboMenu = Menu.AddSubMenu("Combo");
            {
                ComboMenu.AddGroupLabel("Combo Settings");
                ComboMenu.Add("passive", new CheckBox("Passive Combo"));
                ComboMenu.Add("Q", new ComboBox("Q style", 3, "Q Kullanma", "Q Sadece", "Q Sadece Uzun", "Ikiside"));
                ComboMenu.Add("W", new ComboBox("W style", 2, "W Kullanma", "A.A Menzilinde", "Hedefe carpicaksa"));
                ComboMenu.Add("E", new CheckBox("Kullan E"));
                ComboMenu.Add("R", new Slider("Sadece R dusmana isabet ederse {0}% hasar"));
                ComboMenu.AddLabel("Slide to 0 and not use R");
            }

            HarassMenu = Menu.AddSubMenu("Harass");
            {
                HarassMenu.AddGroupLabel("Harass Settings");
                HarassMenu.Add("passive", new CheckBox("Pasifle durtme"));
                HarassMenu.Add("Q", new ComboBox("Q style", 3, "Q Kullanma", "Q Sadece", "Q Sadece Uzun", "Ikiside"));
                HarassMenu.Add("W", new CheckBox("Kullan W"));
                HarassMenu.Add("E", new CheckBox("Kullan E"));
                HarassMenu.Add("hr", new Slider("Mana daha dusukse {0}% durtmeyi durdur", 50));
                HarassMenu.Add("keyharass", new KeyBind("Auto Harass", false, KeyBind.BindTypes.HoldActive, 'Z'));
                HarassMenu.Add("autohrmng", new Slider("Mana daha dusukse {0}% durtmede skll kullanma", 85));
            }

            LaneClear = Menu.AddSubMenu("LaneClear");
            {
                LaneClear.AddGroupLabel("Laneclear Settings");
                LaneClear.Add("passive", new CheckBox("Passive LaneClear"));
                LaneClear.Add("Q", new CheckBox("Kullan Q koridor", false));
                LaneClear.Add("Qhit", new Slider("Sadece carpicaksa Q {0} minyona kullan(s)", 6, 1, 15));
                LaneClear.Add("W", new CheckBox("Kullan W koridor", false));
                LaneClear.Add("E", new CheckBox("Kullan E koridor", false));
                LaneClear.Add("lc", new Slider("Mana daha dusukse {0}% koridor skll kullanma", 50));
            }

            JungleClear = Menu.AddSubMenu("JungleClear");
            {
                JungleClear.AddGroupLabel("Jungleclear Settings");
                JungleClear.Add("passive", new CheckBox("Passive jungleclear"));
                JungleClear.Add("Q", new CheckBox("Kullan Q orman"));
                JungleClear.Add("W", new CheckBox("Kullan Q orman"));
                JungleClear.Add("E", new CheckBox("Kullan Q orman"));
                JungleClear.Add("jc", new Slider("Mana daha dusukse {0}% ormanda skll kullanma", 50));
            }

            LasthitMenu = Menu.AddSubMenu("Lasthit");
            {
                LasthitMenu.AddGroupLabel("Unkillable Minion Settings");
                LasthitMenu.Add("Q", new CheckBox("Kullan Q son vurus"));
                LasthitMenu.Add("W", new CheckBox("Kullan W son vurus"));
                LasthitMenu.Add("E", new CheckBox("Kullan E son vurus", false));
                LasthitMenu.Add("lh", new Slider("Mana daha dusukse {0}% son vurusta skll kullanma", 50));
            }

            FleeMenu = Menu.AddSubMenu("Flee Setting");
            {
                var switchkey = FleeMenu.Add("switchkey", new KeyBind("Switch Key", false, KeyBind.BindTypes.HoldActive, 'S'));
                switchkey.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                {
                    if (args.NewValue)
                    {
                        var value = FleeMenu.GetValue("jumptype", false);
                        if (value == 2)
                        {
                            value = 0;
                        }
                        else
                        {
                            value++;
                        }
                    }
                };
                FleeMenu.Add("jumptype", new ComboBox("Cizgi tipi", 0, "Kacarken E kullanma", "Kacis aktif", "Oto ziplama"));
                FleeMenu.Add("jumpclickrange", new Slider("Tiklama araligi", 50, 20, 100));
            }

            DrawMenu = Menu.AddSubMenu("Drawings");
            {
                DrawMenu.Add("draw", new CheckBox("Gostergeler aktif"));
                DrawMenu.Add("notif", new CheckBox("Bildirimler aktif"));
                DrawMenu.AddSeparator();
                DrawMenu.Add("Qdr", new CheckBox("Goster Q"));
                DrawMenu.Add("Q2dr", new CheckBox("Goster Q uzun"));
                DrawMenu.Add("Wdr", new CheckBox("Goster W"));
                DrawMenu.Add("Edr", new CheckBox("Goster E"));
                DrawMenu.Add("Eposdr", new CheckBox("Draw best postion E"));
                DrawMenu.Add("Rdr", new CheckBox("Goster R"));
                DrawMenu.Add("dmg", new CheckBox("Verilecek hasari goster"));
                var ColorPick = DrawMenu.Add("color", new ColorPicker("Damage Indicator Color", SaveColor.Load("dmg")));
                ColorPick.OnLeftMouseUp += delegate(Control sender, System.EventArgs args)
                {
                    SaveColor.Save(ColorPick.CurrentValue, "dmg");
                };
                DrawMenu.Add("spot", new CheckBox("Cizgi noktasi ciz"));
                var SpotColor = DrawMenu.Add("spotcolor", new ColorPicker("Dash spot color", SaveColor.Load("spot")));
                SpotColor.OnLeftMouseUp += delegate(Control sender, System.EventArgs args)
                {
                    SaveColor.Save(SpotColor.CurrentValue, "spot");
                };
            }

            MiscMenu = Menu.AddSubMenu("MiscMenu");
            {
                MiscMenu.AddGroupLabel("KillSteal Settings");
                MiscMenu.Add("Qks", new CheckBox("Oldururken Q kullan"));
                MiscMenu.Add("Wks", new CheckBox("Oldururken W kullan"));
                MiscMenu.Add("Eks", new CheckBox("Oldururken E kullan", false));
                MiscMenu.Add("Rks", new ComboBox("Oldururken R kullan", 2, "Kapali", "Surekli", "Menzil disindakilere"));
                MiscMenu.Add("Rkstick", new Slider("R atis sayisi kill", 5, 1, 30));
                MiscMenu.AddGroupLabel("Orb cancel amination");
                MiscMenu.Add("Qcancel", new CheckBox("Q animasyonu iptal et[BETA]"));
                MiscMenu.AddGroupLabel("GapCloser");
                MiscMenu.Add("Egap", new CheckBox("Atilma yapana E kullan"));             

            }
            //Need Improve
            //SemiPlayer = Menu.AddSubMenu("Semi Player");
            //{
            //    SemiPlayer.AddGroupLabel("Semi Player Setting");
            //    SemiPlayer.Add("semi", new KeyBind("Enable Semi Player", false, KeyBind.BindTypes.PressToggle, 'G'));
            //    SemiPlayer.Add("passive", new CheckBox("Passive Semi"));
            //    SemiPlayer.Add("Q", new ComboBox("Q style", 3, "Don't use Q", "Q only", "Q Extend only", "Both"));
            //    SemiPlayer.Add("W", new ComboBox("W style", 2, "Don't use W", "A.A Range", "If hit target"));
            //    SemiPlayer.Add("E", new CheckBox("Use E"));
            //}
        }
        public static void CorrectTheMenu()
        {
            switch (ESettings.GetValue("E", false))
            {
                case 1:
                    {
                        ESafe.IsVisible = true;
                        ESafe.CurrentValue = true;
                        EWall.IsVisible = true;
                        EWall.CurrentValue = true;
                        ECorrect.IsVisible = true;
                        EQ.IsVisible = true;
                        EQHit.IsVisible = true;
                        ERange.IsVisible = true;
                        ESettings["label"].IsVisible = true;
                        EPath.CurrentValue = true;
                        EPath.IsVisible = true;
                        EKite.CurrentValue = true;
                        EKite.IsVisible = true;
                        EGap.IsVisible = true;
                        EGrass.IsVisible = false;

                    }
                    break;
                case 0: case 2:
                    {
                        ESafe.IsVisible = false;
                        EWall.IsVisible = false;
                        ECorrect.IsVisible = false;
                        EQ.IsVisible = false;
                        EQHit.IsVisible = false;
                        ERange.IsVisible = false;
                        ESettings["label"].IsVisible = false;
                        EPath.IsVisible = false;
                        EKite.IsVisible = false;
                        EGap.IsVisible = false;
                        EGrass.IsVisible = false;
                    }
                    break;
                case 3:
                    {
                        ESafe.CurrentValue = true;
                        EWall.CurrentValue = false;
                        ECorrect.IsVisible = true;
                        ECorrect.CurrentValue = true;
                        EQ.CurrentValue = false;
                        EQ.IsVisible = true;
                        EQHit.IsVisible = true;
                        ERange.IsVisible = false;
                        ESettings["label"].IsVisible = false;
                        EPath.IsVisible = false;
                        EKite.IsVisible = true;
                        EGap.IsVisible = true;
                        EGrass.IsVisible = true;
                    }
                    break;
            }
        }
    }
}
