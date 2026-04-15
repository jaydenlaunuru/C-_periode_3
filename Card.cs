using System.Collections.Generic;

namespace Balatro1
{
    public class Card : ICard
    {
        public Suit Suit { get; }
        public CardValue Value { get; }
        public virtual bool IsWild => false;

        public Card(Suit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }

        public virtual int CalculateBonus(IEnumerable<Card> hand) => 0;
        public virtual double CalculateMultiplier(IEnumerable<Card> hand) => 1.0;

        public virtual string GetTypePrefix()
        {
            return this.GetType().Name switch
            {
                "BonusCard" => "[B] ",
                "ExtraCard" => "[E] ",
                "GlassCard" => "[G] ",
                "WildCard" => "[W] ",
                "SteelCard" => "[S] ",
                "StickerDecorator" => "[★] ",
                _ => ""
            };
        }

        // Used by the UI to pick a color for the card type. Can be overridden by decorators
        // so the visual color reflects the wrapped card (for example a sticker on a BonusCard
        // should show the bonus color, not the sticker color).
        public virtual string DisplayTypeName() => this.GetType().Name;

        public string GetSuitSymbol()
        {
            return Suit switch
            {
                Suit.Hearts => "♥",
                Suit.Diamonds => "♦",
                Suit.Clubs => "♣",
                Suit.Spades => "♠",
                _ => Suit.ToString()
            };
        }

        public string GetValueString()
        {
            return Value switch
            {
                CardValue.Two => "2",
                CardValue.Three => "3",
                CardValue.Four => "4",
                CardValue.Five => "5",
                CardValue.Six => "6",
                CardValue.Seven => "7",
                CardValue.Eight => "8",
                CardValue.Nine => "9",
                CardValue.Ten => "10",
                _ => Value.ToString()
            };
        }

        public override string ToString()
        {
            return $"{GetTypePrefix()}{GetValueString()}{GetSuitSymbol()}";
        }
    }
}