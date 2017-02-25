﻿using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Pitufo.Addon
{
    internal class PiMenu
    {
        public static bool ComboQ => cQ.CurrentValue;
        public static bool ComboW => cW.CurrentValue;
        public static bool ComboE => cE.CurrentValue;
        public static bool DisableAA => daa.CurrentValue;
        public static int ComboMode => combocm.CurrentValue;
        public static int QHitchance => qHit.CurrentValue;
        public static bool SafeUlt => ult.CurrentValue;
        public static bool LaneQ => lQ.CurrentValue;
        public static bool LaneW => lW.CurrentValue;
        public static bool LaneE => lE.CurrentValue;
        public static bool LaneMana => mLc.CurrentValue;
        public static int LaneSliMana => mnLc.CurrentValue;
        public static int LaneMode => lanecm.CurrentValue;
        public static bool JungQ => qJc.CurrentValue;
        public static bool JungW => wJc.CurrentValue;
        public static bool JungE => eJc.CurrentValue;
        public static bool KsQ => qKs.CurrentValue;
        public static bool KsW => wKs.CurrentValue;
        public static bool KsE => eKs.CurrentValue;
        public static bool KsIg => iKs.CurrentValue;
        public static bool seraph => itemseraph.CurrentValue;
        public static int seraphSli => seraphSlie.CurrentValue;
        public static int seraphRange => seraphrc.CurrentValue;
        public static bool DrawQ => dQ.CurrentValue;
        public static bool DrawWE => dW.CurrentValue;
        public static bool DrawIg => dI.CurrentValue;
        private static CheckBox cQ;
        private static CheckBox cW;
        private static CheckBox cE;
        private static CheckBox daa;
        private static Slider qHit;
        private static KeyBind ult;
        private static CheckBox lQ;
        private static CheckBox lW;
        private static CheckBox lE;
        private static CheckBox mLc;
        private static Slider mnLc;
        private static ComboBox lanecm;
        private static ComboBox combocm;
        private static CheckBox qJc;
        private static CheckBox wJc;
        private static CheckBox eJc;
        private static CheckBox qKs;
        private static CheckBox wKs;
        private static CheckBox eKs;
        private static CheckBox iKs;
        private static CheckBox dQ;
        private static CheckBox dW;
        private static CheckBox dI;
        private static CheckBox itemseraph;
        private static Slider seraphSlie;
        private static Slider seraphrc;
        private static Menu menu;

        public static void Load()
        {
            menu = MainMenu.AddMenu("Ryze", "pitufo");
            menu.AddLabel("Sur's Series: Ryze", 24);
            menu.AddLabel("TekinTR Tarafindan Turkce Yapildi.", 16);
            menu.AddSeparator();
            menu.AddGroupLabel("Combo");
            combocm = menu.Add("combomode", new ComboBox("Combo Algorithm", 0, "Default", "No Collision"));
            cQ =menu.Add("qcom", new CheckBox("Kullan Q"));
            cW=menu.Add("wcom", new CheckBox("Kullan W"));
            cE=menu.Add("ecom", new CheckBox("Kullan E"));
            daa=menu.Add("disaa", new CheckBox("AA Devre disi"));
            menu.AddSeparator();
            menu.AddGroupLabel("Laneclear");
            lanecm = menu.Add("clearmode", new ComboBox("Laneclear Algorithm", 0, "Smart", "E Logic"));
            lQ =menu.Add("qlan", new CheckBox("Kullan Q"));
            lW=menu.Add("wlan", new CheckBox("Kullan W"));
            lE=menu.Add("elan", new CheckBox("Kullan E"));
            mLc=menu.Add("manalan", new CheckBox("Aktif mana yardimcisi"));
            mnLc=menu.Add("manaslider", new Slider("Stop at {0}% mana", 85, 1));
            menu.AddSeparator();
            menu.AddGroupLabel("Jungleclear");
            qJc=menu.Add("qjun", new CheckBox("Kullan Q"));
            wJc=menu.Add("wjun", new CheckBox("Kullan W"));
            eJc=menu.Add("ejun", new CheckBox("Kullan E"));
            menu.AddSeparator();
            menu.AddGroupLabel("Kill Steal");
            qKs=menu.Add("qks", new CheckBox("Kullan Q"));
            wKs=menu.Add("wks", new CheckBox("Kullan W"));
            eKs=menu.Add("eks", new CheckBox("Kullan E"));
            iKs=menu.Add("iks", new CheckBox("Kullan Tutustur"));
            menu.AddSeparator();
            menu.AddGroupLabel("Extra");
            menu.AddLabel("HitChance: 0 = Low, 1 = Medium, 2 = High", 18);
            qHit=menu.Add("hitchance_q", new Slider("Hitchance Q", 2, 0, 2));
            itemseraph= menu.Add("usesera", new CheckBox("Kullan Seraph's Yakalaninca"));
            seraphSlie=menu.Add("serapsslia", new Slider("Execute at {0}% HP", 29, 1));
            seraphrc=menu.Add("serapsslaaa", new Slider("Detect enemies in {0}Range", 700, 100, 1000));
            ult=menu.Add("safeulllt",
                new KeyBind("Ultimate OnSafePosition (Turrent)", false, KeyBind.BindTypes.HoldActive, 'T'));
            menu.AddSeparator();
            menu.AddGroupLabel("Drawings");
            dQ=menu.Add("qdraw", new CheckBox("Goster Q"));
            dW=menu.Add("wedraw", new CheckBox("Goster W/E"));
            dI=menu.Add("idraw", new CheckBox("Goster Tutustur"));
        }
    }
}
