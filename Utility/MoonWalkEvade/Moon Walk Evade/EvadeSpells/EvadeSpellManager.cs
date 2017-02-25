using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using Moon_Walk_Evade.Evading;
using Moon_Walk_Evade.Utils;
using SharpDX;

namespace Moon_Walk_Evade.EvadeSpells
{
    public static class EvadeSpellManager
    {
        public static SpellSlot GetFlashSpellSlot()
        {
            return Player.Instance.GetSpellSlotFromName("summonerflash");
        }

        public static bool TryEvadeSpell(int TimeAvailable, MoonWalkEvade evadeInstance, out Vector2 evadePointOut)
        {
            evadePointOut = Vector2.Zero;

            IEnumerable<EvadeSpellData> evadeSpells = EvadeMenu.MenuEvadeSpells.Where(evadeSpell =>
            {
                var item = EvadeMenu.EvadeSpellMenu[evadeSpell.SpellName + "/enable"];
                // ReSharper disable once SimplifyConditionalTernaryExpression
                // ReSharper disable once MergeConditionalExpression
                return item != null ? item.Cast<CheckBox>().CurrentValue : false;
            });
            foreach (EvadeSpellData _evadeSpell in evadeSpells)
            {
                var evadeSpell = _evadeSpell;
                int dangerValue = EvadeMenu.MenuEvadeSpells.First(x => x.SpellName == evadeSpell.SpellName).DangerValue;
                if (evadeInstance.GetDangerValue() < dangerValue)
                    continue;

                bool isReady = !evadeSpell.isItem ? Player.Instance.Spellbook.GetSpell(evadeSpell.Slot).IsReady &&
                           Player.Instance.Spellbook.GetSpell(evadeSpell.Slot).IsLearned &&
                           Player.Instance.Spellbook.GetSpell(evadeSpell.Slot).SData.Mana <= Player.Instance.Mana : Item.CanUseItem(evadeSpell.itemID);

                if (!isReady)
                    continue;

                if (evadeSpell.EvadeType == EvadeType.Dash || evadeSpell.EvadeType == EvadeType.Blink)
                {
                    /*check if ekko E2*/
                    if (Player.Instance.ChampionName == "Ekko" &&
                        Player.Instance.Spellbook.GetSpell(evadeSpell.Slot).Name.Contains("Two"))//todo: find out ekko E2 name
                    {
                        evadeSpell = EvadeSpellDatabase.Spells.First(spell => spell.SpellName == "EkkoEAttack");
                        goto jump;
                    }

                    float castTime = evadeSpell.Delay;
                    if (TimeAvailable > castTime)
                    {
                        var evadePoints = evadeInstance.GetEvadePoints(null, evadeSpell.Speed, evadeSpell.Delay);
                        if (evadePoints.Any())
                        {
                            evadePointOut = evadePoints.OrderBy(p => p.Distance(Game.CursorPos)).First();
                            CastEvadeSpell(evadeSpell, evadePointOut);
                            return true;
                        }
                    }
                }
                jump:

                if (evadeSpell.Slot == SpellSlot.Unknown && evadeSpell.isItem)
                {
                    var inventoryItem = Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == evadeSpell.itemID);
                    if (inventoryItem != null)
                    {
                        evadeSpell.Slot = inventoryItem.SpellSlot;
                    }
                }

                //speed buff (spell or item)
                if (evadeSpell.EvadeType == EvadeType.MovementSpeedBuff)
                {
                    float speed = Player.Instance.MoveSpeed;
                    float speedArrayValue = !evadeSpell.isItem
                        ? evadeSpell.speedArray[Player.Instance.Spellbook.GetSpell(evadeSpell.Slot).Level - 1]
                        : evadeSpell.speedArray[0];
                    speed += speed * speedArrayValue / 100;

                    var evadePoints = evadeInstance.GetEvadePoints(null, speed, evadeSpell.Delay);

                    var evadePoint = evadePoints.OrderBy(x => !x.IsUnderTurret()).ThenBy(p => p.Distance(Game.CursorPos)).FirstOrDefault();
                    if (evadeSpell.Delay < TimeAvailable && evadePoint != default(Vector2))
                    {
                        evadePointOut = evadePoint;
                        CastEvadeSpell(evadeSpell, evadeSpell.isItem ? Vector2.Zero : evadePoint);
                        return true;
                    }
                }

                /*shield spell or item*/
                if (evadeSpell.EvadeType == EvadeType.SpellShield && evadeSpell.Delay < TimeAvailable)
                {
                    evadePointOut = evadeInstance.LastIssueOrderPos;
                    CastEvadeSpell(evadeSpell, Vector2.Zero);
                    return true;
                }

                if (evadeSpell.CastType == CastType.Target)
                {
                    
                }

            }

            return false;
        }

        private static void CastEvadeSpell(EvadeSpellData evadeSpell, Vector2 evadePos)
        {
            bool isItem = evadePos.IsZero;

            if (isItem)
            {
                Item.UseItem(evadeSpell.itemID);
                return;
            }

            switch (evadeSpell.CastType)
            {
                case CastType.Position:
                    if (!evadeSpell.isReversed)
                        Player.Instance.Spellbook.CastSpell(evadeSpell.Slot, evadePos.To3D());
                    else
                        Player.Instance.Spellbook.CastSpell(evadeSpell.Slot,
                            evadePos.Extend(Player.Instance, evadePos.Distance(Player.Instance) + evadeSpell.Range).To3D());
                    break;
                case CastType.Self:
                    if (!evadeSpell.isItem)
                        Player.Instance.Spellbook.CastSpell(evadeSpell.Slot, Player.Instance);
                    else
                        Player.Instance.InventoryItems.First(item => item.Id == evadeSpell.itemID).Cast();
                break;
            }
        }

        /// <summary>
        /// returns if a dash or a blink is safe
        /// </summary>
        /// <returns></returns>
        public static bool IsDashSafe(EvadeSpellData evadeSpellData, Vector2 endPos, MoonWalkEvade evadeInstance)
        {
            if (evadeSpellData.Speed >= short.MaxValue && evadeInstance.IsPointSafe(endPos))
                return true;

            var evadeSpell =
                EvadeSpellDatabase.Spells.FirstOrDefault(
                    x => x.ChampionName == Player.Instance.ChampionName && x.Slot == evadeSpellData.Slot);
            if (evadeSpell == null)
                return false;

            return evadeInstance.IsPathSafeEx(Player.Instance.GetPath(endPos.To3D()).ToVector2(), (int) evadeSpell.Speed,
                (int) evadeSpell.Delay);
        }


        public static bool IsDashSpell(SpellSlot slot) =>
            EvadeSpellDatabase.Spells.Any(x => x.ChampionName == Player.Instance.ChampionName && x.Slot == slot && 
             (x.EvadeType == EvadeType.Dash || x.EvadeType == EvadeType.Blink));
    }
}