using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Moon_Walk_Evade.EvadeSpells;
using Moon_Walk_Evade.Evading;
using Moon_Walk_Evade.Skillshots;

namespace Moon_Walk_Evade
{
    internal static class MenuExtension
    {
        public static void AddStringList(this Menu m, string uniqueId, string displayName, string[] values, int defaultValue)
        {
            var mode = m.Add(uniqueId, new Slider(displayName, defaultValue, 0, values.Length - 1));
            mode.DisplayName = displayName + ": " + values[mode.CurrentValue];
            mode.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
            {
                sender.DisplayName = displayName + ": " + values[args.NewValue];
            };
        }
    }
    internal class EvadeMenu
    {
        public static Menu MainMenu { get; private set; }

        public static Menu HumanizerMenu { get; private set; }

        public static Menu SpellBlockerMenu { get; private set; }
        public static Menu SkillshotMenu { get; private set; }
        public static Menu EvadeSpellMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }
        public static Menu HotkeysMenu { get; private set; }
        public static Menu CollisionMenu { get; private set; }

        public static Menu DebugMenu { get; private set; }

        public static readonly Dictionary<string, EvadeSkillshot> MenuSkillshots = new Dictionary<string, EvadeSkillshot>();
        public static readonly List<EvadeSpellData> MenuEvadeSpells = new List<EvadeSpellData>();

        public static void CreateMenu()
        {
            if (MainMenu != null)
            {
                return;
            }

            MainMenu = EloBuddy.SDK.Menu.MainMenu.AddMenu("MoonWalkEvade", "MoonWalkEvade");

            MainMenu.Add("evadeMode", new ComboBox("Kacma modu", new[] {"Puruzsuz", "Hizli"}));
            MainMenu.Add("fowDetection", new CheckBox("Aktif Savas sisi algilama"));
            //MainMenu.Add("serverTimeBuffer", new Slider("Server Time Buffer"));
            MainMenu.AddSeparator(50);
            MainMenu.AddGroupLabel("Misc");
            MainMenu.Add("processSpellDetection", new CheckBox("Hizli spell algilama Ekin"));
            MainMenu.Add("limitDetectionRange", new CheckBox("Spell algilama limit uzakligi"));
            MainMenu.Add("moveToInitialPosition", new CheckBox("Kactiktan sonra istenen yere git", false));
            MainMenu.Add("forceEvade", new CheckBox("Imkansizsa kacmak icin deneyin"));
            MainMenu.AddSeparator(50);
            MainMenu.AddGroupLabel("Recalculation");
            MainMenu.Add("recalculatePosition", new CheckBox("Kacma Pozisyonunun Yeniden Hesaplanmasina Izin Verin"));
            MainMenu.Add("recalculationSpeed", new Slider("Yeniden Hesaplama Gecikmesi", 500, 0, 1000));
            MainMenu.AddLabel("Low Delay is Cpu Intense");
            MainMenu.Add("minRecalculationAngle", new Slider("Yeniden hesaplama icin minimum Aci Degistirme [Derecelerde]", 2, 0, 50));
            MainMenu.AddSeparator(50);
            MainMenu.AddGroupLabel("Buffers");
            MainMenu.Add("minDangerTurretEvade", new Slider("Kule Altinda Kacmak Icin Minimum Tehlike Degeri", 4, 1, 5));
            MainMenu.Add("minComfortDistance", new Slider("Düşmana Minimum Rahatlik Mesafesi", 550, 0, 1000));
            MainMenu.Add("enemyComfortCount", new Slider("Rahatlik Mesafesine Katilmak Icin Dusmanlarin Minimum Miktarı", 3, 1, 5));
            MainMenu.AddSeparator();
            MainMenu.Add("dontEvadeComfort", new CheckBox("Rahatlik Noktasi Bulunmazsa kacma"));
            MainMenu.AddSeparator(0);
            MainMenu.Add("letAAPass", new CheckBox("Atak kacirdiktan Sonradan Kacinilmasi"));
            MainMenu.AddLabel("Otherwise forbid auto attacks if in danger");

            HumanizerMenu = MainMenu.AddSubMenu("Humanizer");
            HumanizerMenu.Add("skillshotActivationDelay", new Slider("Tepki Geciktirme", 0, 0, 400));
            HumanizerMenu.AddSeparator();

            HumanizerMenu.Add("useFastInSmooth", new CheckBox("Pürüzsüz Modda Hizli Kacinma Gerekirse"));
            HumanizerMenu.Add("extraEvadeRange", new Slider("Extra Kacma mesafesi", 0, 0, 300));
            HumanizerMenu.Add("randomizeExtraEvadeRange", new CheckBox("Ekstra Menzil Rastgele Olustur", false));
            HumanizerMenu.AddSeparator();
            HumanizerMenu.Add("stutterDistanceTrigger", new Slider("Stutter Tetikleme Mesafesi", 200, 0, 400));
            HumanizerMenu.AddLabel("Kacinma noktaniz senden 200 birim veya daha uzak oldugunda");
            HumanizerMenu.AddLabel("Eski noktada durmanizi önlemek icin değistirilecek");
            HumanizerMenu.AddSeparator();
            HumanizerMenu.AddStringList("stutterPointFindType", "Anti Stutter Evade Point Search", new []{"Mouse Position", "Same As Player Direction", "Farest Away"}, 0);
            HumanizerMenu.AddLabel("Yeni bir nokta bulmak icin arama yontemi turudur");

            SpellBlockerMenu = MainMenu.AddSubMenu("Spell Blocker");
            SpellBlockerMenu.AddGroupLabel("Spells to block while evading");
            SpellBlockerMenu.Add("blockDangerousDashes", new CheckBox("Tehlikeli hamleleri Engelle"));
            SpellBlockerMenu.AddSeparator(10);
            for (int slot = 0; slot < 4; slot++)
            {
                var currentSlot = (SpellSlot) slot;
                bool block = SpellBlocker.ShouldBlock(currentSlot);
                SpellBlockerMenu.Add("block" + Player.Instance.ChampionName + "/" + currentSlot, new CheckBox("Block " + currentSlot, block));
            }

            var heroes = Program.DeveloperMode ? EntityManager.Heroes.AllHeroes : EntityManager.Heroes.Enemies;
            var heroNames = heroes.Select(obj => obj.ChampionName).ToArray();
            var skillshots =
                SkillshotDatabase.Database.Where(s => heroNames.Contains(s.OwnSpellData.ChampionName)).ToList();
            skillshots.AddRange(
                SkillshotDatabase.Database.Where(
                    s =>
                        s.OwnSpellData.ChampionName == "AllChampions" ||
                        heroes.Any(obj => obj.Spellbook.Spells.Select(c => c.Name).Contains(s.OwnSpellData.SpellName))));
            var evadeSpells =
                EvadeSpellDatabase.Spells.Where(s => Player.Instance.ChampionName.Contains(s.ChampionName)).ToList();
            evadeSpells.AddRange(EvadeSpellDatabase.Spells.Where(s => s.ChampionName == "AllChampions"));


            SkillshotMenu = MainMenu.AddSubMenu("Skillshots");

            foreach (var c in skillshots)
            {
                var skillshotString = c.ToString().ToLower();

                if (MenuSkillshots.ContainsKey(skillshotString))
                    continue;

                MenuSkillshots.Add(skillshotString, c);

                SkillshotMenu.AddGroupLabel(c.DisplayText);
                SkillshotMenu.Add(skillshotString + "/enable", new CheckBox("Kac", c.OwnSpellData.EnabledByDefault));
                SkillshotMenu.Add(skillshotString + "/draw", new CheckBox("Goster"));

                var dangerous = new CheckBox("Dangerous", c.OwnSpellData.IsDangerous);
                dangerous.OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                {
                    GetSkillshot(sender.SerializationId).OwnSpellData.IsDangerous = args.NewValue;
                };
                SkillshotMenu.Add(skillshotString + "/dangerous", dangerous);

                var dangerValue = new Slider("Danger Value", c.OwnSpellData.DangerValue, 1, 5);
                dangerValue.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    GetSkillshot(sender.SerializationId).OwnSpellData.DangerValue = args.NewValue;
                };
                SkillshotMenu.Add(skillshotString + "/dangervalue", dangerValue);

                SkillshotMenu.AddSeparator();
            }

            // Set up spell menu
            EvadeSpellMenu = MainMenu.AddSubMenu("Evading Spells");

            foreach (var e in evadeSpells)
            {
                var evadeSpellString = e.SpellName;

                if (MenuEvadeSpells.Any(x => x.SpellName == evadeSpellString))
                    continue;

                MenuEvadeSpells.Add(e);

                EvadeSpellMenu.AddGroupLabel(evadeSpellString);
                EvadeSpellMenu.Add(evadeSpellString + "/enable", new CheckBox("Kullan " + (!e.isItem ? e.Slot.ToString() : "")));

                var dangerValueSlider = new Slider("Tehlike Duzeyi", e.DangerValue, 1, 5);
                dangerValueSlider.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    MenuEvadeSpells.First(x =>
                        x.SpellName.Contains(sender.SerializationId.Split('/')[0])).DangerValue = args.NewValue;
                };
                EvadeSpellMenu.Add(evadeSpellString + "/dangervalue", dangerValueSlider);

                EvadeSpellMenu.AddSeparator();
            }


            DrawMenu = MainMenu.AddSubMenu("Drawings");
            DrawMenu.Add("disableAllDrawings", new CheckBox("Tum cizimleri kapat", false));
            DrawMenu.Add("drawEvadePoint", new CheckBox("Kacma noktasini goster", false));
            DrawMenu.Add("drawEvadeStatus", new CheckBox("Kacma durumunu goster"));
            DrawMenu.Add("drawSkillshots", new CheckBox("Goster gelen beceriler"));
            DrawMenu.AddStringList("drawType", "Drawing Type", new [] { "Fancy", "Fast" }, 1);


            HotkeysMenu = MainMenu.AddSubMenu("KeyBinds");
            HotkeysMenu.Add("enableEvade", new KeyBind("Kacma Aktif", true, KeyBind.BindTypes.PressToggle, 'M'));
            HotkeysMenu.Add("dodgeOnlyDangerousH", new KeyBind("Yanliz Tehlikeden Kac (Dur)", false, KeyBind.BindTypes.HoldActive));
            HotkeysMenu.Add("dodgeOnlyDangerousT", new KeyBind("Yanliz Tehlikeden Kac (Degistir)", false, KeyBind.BindTypes.PressToggle));

            CollisionMenu = MainMenu.AddSubMenu("Collision");
            CollisionMenu.Add("minion", new CheckBox("Minyonlara dikkat et"));
            CollisionMenu.Add("yasuoWall", new CheckBox("Dikkat Yasuo Duvari"));

            DebugMenu = MainMenu.AddSubMenu("Testings");
            DebugMenu.Add("debugMode", new KeyBind("Debug Mode", false, KeyBind.BindTypes.PressToggle));
            DebugMenu.Add("debugModeIntervall", new Slider("Debug Skillshot Creation Intervall", 1000, 0, 12000));
            DebugMenu.AddStringList("debugMissile", "Selected Skillshot",
                SkillshotDatabase.Database.Select(x => x.OwnSpellData.SpellName).ToArray(), 0);
            DebugMenu.Add("isProjectile", new CheckBox("Is Projectile?"));
            DebugMenu.Add("manageMovementDelay", new CheckBox("Orbwalker hareket gecikmesini yonet", false));
        }

        private static EvadeSkillshot GetSkillshot(string s)
        {
            return MenuSkillshots[s.ToLower().Split('/')[0]];
        }

        public static bool IsSkillshotEnabled(EvadeSkillshot skillshot)
        {
            var valueBase = SkillshotMenu[skillshot + "/enable"];
            return (valueBase != null && valueBase.Cast<CheckBox>().CurrentValue) ||
                DebugMenu["debugMode"].Cast<KeyBind>().CurrentValue;
        }

        public static bool IsSkillshotDrawingEnabled(EvadeSkillshot skillshot)
        {
            var valueBase = SkillshotMenu[skillshot + "/draw"];
            return (valueBase != null && valueBase.Cast<CheckBox>().CurrentValue) ||
                DebugMenu["debugMode"].Cast<KeyBind>().CurrentValue;
        }

        public static bool IsEvadeSkillhotEnabled(EvadeSpellData spell)
        {
            if (spell == null)
                return false;

            var valueBase = EvadeSpellMenu[spell.SpellName + "/enable"];
            return valueBase != null && valueBase.Cast<CheckBox>().CurrentValue;
        }
    }
}