using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Support_Mode
{
    public static class Config
    {
        private static Menu _disableAa;
        public static Menu Main, Harass, LaneClear, LastHit, Draw;
        public static bool GlobalToggler
        {
            get { return _disableAa["globalToggle"].Cast<KeyBind>().CurrentValue; }
        }

        public static void CallMenu()
        {
            _disableAa = MainMenu.AddMenu("Support Mode", "SupportMode");
            _disableAa.AddGroupLabel("Support Mode");

            //Main = _disableAa.AddSubMenu("Main", "Main");
            _disableAa.AddGroupLabel("Global options");
            _disableAa.Add(
                "globalToggle", new KeyBind("Genel Aktif/Pasif butonu", true, KeyBind.BindTypes.PressToggle));

            Harass = _disableAa.AddSubMenu("Harass", "Harass");
            Harass.AddGroupLabel("Options for Harass");
            Harass.Add("disableAAIH", new CheckBox("Minyona AA devre disi durtme modunda", true));
            Harass.Add("stacksIH", new CheckBox("Kalkan yukleri olsa bile minyona AA devre disi birak", false));
            Harass.Add("allyRangeH", new Slider("Harass Modunda AA devre disi birakmak icin x araliklarındaki müttefikler", 1400, 0, 5000));

            LaneClear = _disableAa.AddSubMenu("LaneClear", "LaneClear");
            LaneClear.AddGroupLabel("Options for LaneClear");
            LaneClear.Add("disableAAILC", new CheckBox("Oto atak devre disi birak minyon temizlemede", true));
            LaneClear.Add("stacksILC", new CheckBox("Kalkan yukleri olsa bile AA devre disi birak", false));
            LaneClear.Add("allyRangeLC", new Slider("LaneClear Modunda AA devre disi birakmak icin X mesafesindeki müttefikler", 1400, 0, 5000));
            LaneClear.Add("pushNoCS", new CheckBox("AA minyona, ama CS almayin", false));

            LastHit = _disableAa.AddSubMenu("LastHit", "LastHit");
            LastHit.AddGroupLabel("Options for LastHit");
            LastHit.Add("disableAAILH", new CheckBox("Oto atak devre disi birak minyona son vurusda", true));
            LastHit.Add("stacksILH", new CheckBox("Kalkan yukleri olsa bile AA devre disi birak", false));
            LastHit.Add("allyRangeLH", new Slider("LastHit Modunda AA devre disi birakmak için X mesafesindeki muttefikler", 1400, 0, 5000));

            Draw = _disableAa.AddSubMenu("Draw", "Draw");
            Draw.AddGroupLabel("Options for draw stuff");
            Draw.AddGroupLabel("Status Text");
            Draw.Add("globalDraw", new CheckBox("Durumu Goster", true));
            Draw.Add("globaldrawX", new Slider("Durum Metninin X Pozisyonu", 35, -200, 200));
            Draw.Add("globaldrawY", new Slider("Durum Metninin Y Pozisyonu", -30, -200, 200));
        }

        public static bool IsChecked(Menu obj, string value)
        {
            return obj[value].Cast<CheckBox>().CurrentValue;
        }

        public static int GetSliderValue(Menu obj, string value)
        {
            return obj[value].Cast<Slider>().CurrentValue;
        }
    }
}
