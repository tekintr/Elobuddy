using System;
using EloBuddy;
using EloBuddy.SDK;

namespace TwistedFate
{
    public enum Cards
    {
        Gold,
        Red,
        Blue,
        None,
    }

    public enum SelectCardStatus
    {
        Ready,
        Selecting,
        Selected,
        Cooldown,
    }

    class CardSelector
    {
        private static SelectCardStatus Status { get; set; }
        private static Cards SelectedCard { get; set; }
        private static int LastWTick = 0;

        static CardSelector()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Game.OnUpdate += OnUpdate;
        }

        public static void SelectCard(Obj_AI_Base t, Cards selectedCard)
        {
            if (t == null)
                return;

            switch(SelectedCard)
            {
                case Cards.Gold:
                    StartSelecting(Cards.Gold);
                    break;
                case Cards.Red:
                    StartSelecting(Cards.Red);
                    break;
                case Cards.Blue:
                    StartSelecting(Cards.Blue);
                    break;
            }
        }

        public static void StartSelecting(Cards card)
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name.Equals("PickACard", StringComparison.InvariantCultureIgnoreCase) 
                && Status == SelectCardStatus.Ready)
            {
                SelectedCard = card;
                if (Environment.TickCount - LastWTick > 170)
                {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, false);
                    LastWTick = Environment.TickCount;
                }
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
                return;

            if (args.SData.Name.Equals("PickACard", StringComparison.InvariantCultureIgnoreCase))
                Status = SelectCardStatus.Selecting;

            if (args.SData.Name.Equals("GoldCardLock", StringComparison.InvariantCultureIgnoreCase) 
                || args.SData.Name.Equals("BlueCardLock", StringComparison.InvariantCultureIgnoreCase)
                || args.SData.Name.Equals("RedCardLock", StringComparison.InvariantCultureIgnoreCase))
            {
                Status = SelectCardStatus.Selected;
                SelectedCard = Cards.None;
            }
        }
        private static void OnUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead)
                return;

            string spellName = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name;
            SpellState spellState = ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.W);

            if (spellState == SpellState.Ready && spellName.Equals("PickACard", StringComparison.InvariantCultureIgnoreCase))
                Status = SelectCardStatus.Ready;
            else if (spellState == SpellState.Cooldown && spellName.Equals("PickACard", StringComparison.InvariantCultureIgnoreCase))
            {
                SelectedCard = Cards.None;
                Status = SelectCardStatus.Cooldown;
            }
            else if (spellState == SpellState.Surpressed)
                Status = SelectCardStatus.Selected;

            switch(SelectedCard)
            {
                case Cards.Gold:
                    if (spellName.Equals("GoldCardLock", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, false);
                    }
                    break;
                case Cards.Red:
                    if (spellName.Equals("RedCardLock", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, false);
                    }
                    break;
                case Cards.Blue:
                    if (spellName.Equals("BlueCardLock", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, false);
                    }
                    break;
            }
        }
    }
}
