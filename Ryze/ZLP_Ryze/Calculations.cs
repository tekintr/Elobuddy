using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace ZLP_Ryze
{
    public class Calculations
    {
        public static float Damage;

        public static void Execute()
        {
            var mana = Player.Instance.MaxMana - (350f + 50f * Player.Instance.Level);

            foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsHPBarRendered))
            {
                var q = Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Magical,
                        new[] { 0f, 60f, 85f, 110f, 135f, 160f, 185f }[Spells.Q.Level]
                        + 0.45f * Player.Instance.TotalMagicalDamage + 0.03f * mana);
                var w = Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Magical,
                        new[] { 0f, 80f, 100f, 120f, 140f, 160f }[Spells.W.Level]
                        + 0.2f * Player.Instance.TotalMagicalDamage + 0.01f * mana);
                var e = Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Magical,
                        new[] { 0f, 50f, 75f, 100f, 125f, 150f }[Spells.E.Level]
                        + 0.3f * Player.Instance.TotalMagicalDamage + 0.02f * mana);
                var bonus = new[] { 0f, 0.4f, 0.55f, 0.7f, 0.85f, 1f }[Spells.E.Level] * q;

                if (Spells.Q.IsLearned && Spells.W.IsLearned && Spells.E.IsLearned)
                {
                    if (!Spells.Q.IsReady() && Spells.W.IsReady() && Spells.E.IsReady())
                        Damage = q * 2 + w + e + bonus;

                    else if (!Spells.Q.IsReady() && Spells.W.IsReady() && !Spells.E.IsReady())
                        Damage = q + w;

                    else if (!Spells.Q.IsReady() && !Spells.W.IsReady() && Spells.E.IsReady())
                        Damage = q + e + bonus;

                    else if (!Spells.Q.IsReady() && !Spells.W.IsReady() && !Spells.E.IsReady())
                        Damage = 0f;

                    else
                    {
                        if (!enemy.HasBuff("RyzeE"))
                        {
                            if (Spells.Q.IsReady() && Spells.W.IsReady() && Spells.E.IsReady())
                                Damage = q * 3 + w + e + bonus;
                            if (Spells.Q.IsReady() && !Spells.W.IsReady() && Spells.E.IsReady())
                                Damage = q * 2 + e + bonus;
                            if (Spells.Q.IsReady() && Spells.W.IsReady() && !Spells.E.IsReady())
                                Damage = q * 2 + w;
                            if (Spells.Q.IsReady() && !Spells.W.IsReady() && !Spells.E.IsReady())
                                Damage = q;
                        }

                        else
                        {
                            if (Spells.Q.IsReady() && Spells.W.IsReady() && Spells.E.IsReady())
                                Damage = q * 3 + w + e + bonus * 2;
                            if (Spells.Q.IsReady() && !Spells.W.IsReady() && Spells.E.IsReady())
                                Damage = q * 2 + e + bonus * 2;
                            if (Spells.Q.IsReady() && Spells.W.IsReady() && !Spells.E.IsReady())
                                Damage = q * 2 + w + bonus;
                            if (Spells.Q.IsReady() && !Spells.W.IsReady() && !Spells.E.IsReady())
                                Damage = q + bonus;
                        }
                    }
                }

                else if (Spells.Q.IsLearned && Spells.W.IsLearned && !Spells.E.IsLearned)
                {
                    if (Spells.Q.IsReady() && Spells.W.IsReady())
                        Damage = q * 2 + w;
                    else if (Spells.Q.IsReady() && !Spells.W.IsReady())
                        Damage = q;
                    else if (!Spells.Q.IsReady() && Spells.W.IsReady())
                        Damage = q + w;
                    else
                        Damage = 0f;
                }

                else if (Spells.Q.IsLearned && !Spells.W.IsLearned && Spells.E.IsLearned)
                {
                    if (!Spells.Q.IsReady() && Spells.E.IsReady())
                        Damage = q + e + bonus;

                    else if (!Spells.Q.IsReady() && !Spells.E.IsReady())
                        Damage = 0f;

                    else
                    {
                        if (!enemy.HasBuff("RyzeE"))
                        {
                            if (Spells.Q.IsReady() && Spells.E.IsReady())
                                Damage = q * 2 + e + bonus;
                            if (Spells.Q.IsReady() && !Spells.E.IsReady())
                                Damage = q;
                        }

                        else
                        {
                            if (Spells.Q.IsReady() && Spells.E.IsReady())
                                Damage = q * 2 + e + bonus * 2;
                            if (Spells.Q.IsReady() && !Spells.E.IsReady())
                                Damage = q + bonus;
                        }
                    }
                }

                else if (!Spells.Q.IsLearned && Spells.W.IsLearned && Spells.E.IsLearned)
                {
                    if (Spells.W.IsReady() && Spells.E.IsReady())
                        Damage = w + e;
                    else if (Spells.W.IsReady() && !Spells.E.IsReady())
                        Damage = w;
                    else if (!Spells.W.IsReady() && Spells.E.IsReady())
                        Damage = e;
                    else
                        Damage = 0f;
                }

                else if (Spells.Q.IsLearned && !Spells.W.IsLearned && !Spells.E.IsLearned)
                    Damage = Spells.Q.IsReady() ? q : 0f;

                else if (!Spells.Q.IsLearned && Spells.W.IsLearned && !Spells.E.IsLearned)
                    Damage = Spells.W.IsReady() ? w : 0f;

                else if (!Spells.Q.IsLearned && !Spells.W.IsLearned && Spells.E.IsLearned)
                    Damage = Spells.E.IsReady() ? e : 0f;

                else
                    Damage = 0f;

                if (Spells.Ignite != null && Spells.Ignite.IsReady())
                {
                    var ignite = Player.Instance.GetSummonerSpellDamage(enemy, DamageLibrary.SummonerSpells.Ignite);
                    Damage += ignite;
                }
            }
        }
    }
}