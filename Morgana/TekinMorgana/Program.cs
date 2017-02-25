using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using UnsignedEvade;





namespace TekinMorgana
{
    class Program
    {
        private static Spell.Skillshot Q;
        private static Spell.Skillshot W;
        private static Spell.Targeted E;
        private static Spell.Active R;
        private static AIHeroClient User = Player.Instance;
        private static Menu MorganaMenu, Saldiri, Cizimler, BuyulereKalkan, TakimaKalkan, Otomatik, Durtme, QAyarlari, KoridorTemizleme, WAyari;
        private static List<Spell.SpellBase> SpellList = new List<Spell.SpellBase>();
        public static List<MissileClient> ProjectileList = new List<MissileClient>();
        public static List<SpellInfo> EnemyProjectileInformation = new List<SpellInfo>();
        public static readonly Random Random = new Random(DateTime.Now.Millisecond);


        static void Main(string[] args)
        {

            Loading.OnLoadingComplete += Loading_OnLoadingComplete;



        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Chat.Print("<font color='#07667F'>TekinMorgana v1 Yuklendi Keyifli Oyunlar Dilerim :).</font> "); 


            if (User.ChampionName != "Morgana")
            {
                return;
            }
            Q = new Spell.Skillshot(spellSlot: SpellSlot.Q, spellRange: 1150, skillShotType: SkillShotType.Linear,
                    castDelay: 250, spellSpeed: 1200, spellWidth: 70)
            { AllowedCollisionCount = 0 };

            W = new Spell.Skillshot(spellSlot: SpellSlot.W, spellRange: 1000, skillShotType: SkillShotType.Circular,
                    castDelay: 250, spellSpeed: 2200, spellWidth: 200)
            { AllowedCollisionCount = -1 };

            E = new Spell.Targeted(SpellSlot.E, 800);

            R = new Spell.Active(SpellSlot.R, 600);
            R.DamageType = DamageType.Magical;

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);


            MorganaMenu = MainMenu.AddMenu("Morgana", "Morgana");
            QAyarlari = MorganaMenu.AddSubMenu("QAyarlari");
            WAyari = MorganaMenu.AddSubMenu("WAyari");
            Saldiri = MorganaMenu.AddSubMenu("Saldiri");
            Durtme = MorganaMenu.AddSubMenu("Durtme");
            KoridorTemizleme = MorganaMenu.AddSubMenu("KoridorTemizleme");
            BuyulereKalkan = MorganaMenu.AddSubMenu("BuyulereKalkan");
            TakimaKalkan = MorganaMenu.AddSubMenu("TakimaKalkan");
            Cizimler = MorganaMenu.AddSubMenu("Cizimler");
            Otomatik = MorganaMenu.AddSubMenu("Otomatik R Ayari");




            Saldiri.Add("Q", new CheckBox("Q Kullan"));
            Saldiri.Add("W", new CheckBox("W Kullan"));
            Saldiri.Add("E", new CheckBox("E Kullan"));
            Saldiri.Add("R", new CheckBox("R Kullan"));
            Saldiri.Add("SpamW", new CheckBox("Hedef hareket etse bile W kullanilsin?"));
            Durtme.Add("Q", new CheckBox("Q Kullan"));
            Durtme.Add("W", new CheckBox("W Kullan"));
            Durtme.Add("SpamW", new CheckBox("Hedef hareket etse bile W kullanilsin?"));
            KoridorTemizleme.Add("W", new CheckBox("Minyonlara W kullan"));
            Otomatik.Add("Rcount", new Slider("R kullanmak icin dusman sayisi ", 3, 1, 5));
            KoridorTemizleme.Add("minionCount", new Slider("W kullanmak icin minyon sayisi ", 3, 1, 5));

            foreach (var Spell in SpellList)
            {
                Cizimler.Add(Spell.Slot.ToString(), new CheckBox("Cizimler " + Spell.Slot));
            }

            foreach (AIHeroClient client in EntityManager.Heroes.Enemies)
            {
                QAyarlari.Add(client.ChampionName, new CheckBox("Q Aktif" + client.ChampionName));
                WAyari.Add(client.ChampionName, new CheckBox("W Aktif" + client.ChampionName));
                foreach (SpellInfo info in SpellDatabase.SpellList)
                {
                    if (info.ChampionName == client.ChampionName)
                    {
                        EnemyProjectileInformation.Add(info);

                    }
                }
            }

            foreach (AIHeroClient client in EntityManager.Heroes.Allies)
            {

                TakimaKalkan.Add(client.ChampionName, new CheckBox("Kalkan" + client.ChampionName));


            }

            foreach (SpellInfo spell in EnemyProjectileInformation)
            {
                Console.WriteLine(spell.SpellName);
                BuyulereKalkan.Add(spell.MissileName, new CheckBox("Kalkan" + spell.MissileName));
            }

            Game.OnTick += Game_OnTick;
            Game.OnUpdate += GameOnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDelete += OnDelete;
            Obj_AI_Base.OnUpdatePosition += OnUpdate;

        }


        public static void OnUpdate(GameObject obj, EventArgs args)
        {
            var missile = obj as MissileClient;
            if (missile != null &&
                missile.SpellCaster != null &&
                missile.SpellCaster.IsEnemy &&
                missile.SpellCaster.Type == GameObjectType.AIHeroClient &&
                ProjectileList.Contains(missile))
            {
                ProjectileList.Remove(missile);
                ProjectileList.Add(missile);
            }
        }

        private static void GameOnUpdate(EventArgs args)
        {
            if (User.Position.CountEnemiesInRange(R.Range) >= Otomatik["Rcount"].Cast<Slider>().CurrentValue)
            {
                R.Cast();

            }

            TryToE();

            //   EAllies();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            foreach (var Spell in SpellList.Where(spell => Cizimler[spell.Slot.ToString()].Cast<CheckBox>().CurrentValue))
            {

                Circle.Draw(Spell.IsReady() ? Color.Chartreuse : Color.Aquamarine, Spell.Range, User);
            }
        }





        private static void Game_OnTick(EventArgs args)
        {

            TryToE();




            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {

                Combo();

            }

            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }

            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }

        }




        private static void Combo()
        {
            var t = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var pred = Q.GetPrediction(t);

            if (t == null)
            {
                return;
            }


            if ((Saldiri["Q"].Cast<CheckBox>().CurrentValue && t.IsValidTarget(Q.Range) && Q.IsReady() && QAyarlari[t.ChampionName].Cast<CheckBox>().CurrentValue && pred.HitChance >= HitChance.High))
            {
                Q.Cast(t);
            }

            if ((Saldiri["W"].Cast<CheckBox>().CurrentValue && t.IsValidTarget(W.Range) && W.IsReady() && WAyari[t.ChampionName].Cast<CheckBox>().CurrentValue && Saldiri["SpamW"].Cast<CheckBox>().CurrentValue))
            {
                W.Cast(t);
            }

            if ((Saldiri["W"].Cast<CheckBox>().CurrentValue && t.IsValidTarget(W.Range) && W.IsReady() && WAyari[t.ChampionName].Cast<CheckBox>().CurrentValue && Saldiri["SpamW"].Cast<CheckBox>().CurrentValue == false && t.HasBuffOfType(BuffType.Snare) || t.HasBuffOfType(BuffType.Stun)))
            {
                W.Cast(t);
            }


       

        }



        private static void LaneClear()
        {
            var farm = EntityManager.MinionsAndMonsters.EnemyMinions.Where(s => s.IsInRange(W.RangeCheckSource ?? User.Position, W.Range));
            var WBestFarmLoc = W.GetBestCircularCastPosition(farm);
            if (WBestFarmLoc.HitNumber >= Otomatik["Rcount"].Cast<Slider>().CurrentValue && KoridorTemizleme["W"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast(WBestFarmLoc.CastPosition);
            }


        }

        private static void Harass()
        {
            var t = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var pred = Q.GetPrediction(t);
            if (t.IsValidTarget(Q.Range) && Q.IsReady() && pred.HitChance >= HitChance.High)
            {
                Q.Cast(t);
            }

            if (t.IsValidTarget(W.Range) && W.IsReady() && Durtme["SpamW"].Cast<CheckBox>().CurrentValue && WAyari[t.ChampionName].Cast<CheckBox>().CurrentValue && pred.HitChance >= HitChance.Medium)
            {
                W.Cast(t);
            }

            if (t.IsValidTarget(W.Range) && W.IsReady() && Durtme["SpamW"].Cast<CheckBox>().CurrentValue == false && t.IsRooted || t.IsStunned && WAyari[t.ChampionName].Cast<CheckBox>().CurrentValue)
            {
                W.Cast(t);
            }
        }


        public static void OnCreate(GameObject obj, EventArgs args)
        {
            var missile = obj as MissileClient;
            if (missile != null &&
                missile.SpellCaster != null &&
                missile.SpellCaster.IsEnemy &&
                missile.SpellCaster.Type == GameObjectType.AIHeroClient)
                ProjectileList.Add(missile);
        }

        public static void OnDelete(GameObject obj, EventArgs args)
        {
            if (obj == null)
                return;

            var missile = obj as MissileClient;
            if (missile != null &&
                missile.SpellCaster != null &&
                missile.SpellCaster.IsEnemy &&
                missile.SpellCaster.Type == GameObjectType.AIHeroClient &&
                ProjectileList.Contains(missile))
            {
                ProjectileList.Remove(missile);
            }
        }

        private static void TryToE()
        //credit to Chaos for this logic if about to be hit!
        {
            if (E.IsReady() && E.IsLearned)
                foreach (MissileClient missile in ProjectileList)
                    foreach (SpellInfo info in EnemyProjectileInformation)
                        foreach (var client in EntityManager.Heroes.Allies)
                        {
                            if (ShouldShield(missile, info, client) && CollisionCheck(missile, info, client))
                            {
                                if (info.ChannelType == SpellDatabase.ChannelType.None && E.IsReady() &&
                                    (BuyulereKalkan[info.MissileName].Cast<CheckBox>().CurrentValue) &&
                                    missile.IsInRange(Player.Instance, Q.Range) && TakimaKalkan[client.ChampionName].Cast<CheckBox>().CurrentValue)
                                    E.Cast(client);
                                else if (info.ChannelType != SpellDatabase.ChannelType.None && E.IsReady() &&
                                         (BuyulereKalkan[info.MissileName].Cast<CheckBox>().CurrentValue) &&
                                         missile.IsInRange(Player.Instance, Q.Range) && TakimaKalkan[client.ChampionName].Cast<CheckBox>().CurrentValue)
                                    E.Cast(client);
                            }
                        }
        }





        public static bool ShouldShield(MissileClient missile, SpellInfo info, AIHeroClient client)
        {


            if (missile.SpellCaster.Name != "Diana")
                if (missile.SData.Name != info.MissileName ||
                    !missile.IsInRange(client, 800))
                    return false;


            if (info.ProjectileType == SpellDatabase.ProjectileType.LockOnProjectile
                && missile.Target != client)
                return false;



            return true;
        }



        public static bool CollisionCheck(MissileClient missile, SpellInfo info, AIHeroClient client)
        {
            bool variable = Prediction.Position.Collision.LinearMissileCollision(
                client, missile.StartPosition.To2D(), missile.StartPosition.To2D().Extend(missile.EndPosition, info.Range),
                info.MissileSpeed, info.Width, info.Delay);
            return variable;
        }




    }
}
