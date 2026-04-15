using System;
using System.Collections.Generic;
using System.Linq;

namespace Balatro1
{
    public class Deck
    {
        private List<Card> _cards;
        private readonly Random _random;

        public Deck()
        {
            _random = new Random();
            _cards = new List<Card>();
            Reset();
        }

        public int RemainingCards => _cards.Count;

        public void Reset()
        {
            _cards.Clear();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (CardValue val in Enum.GetValues(typeof(CardValue)))
                {
                    _cards.Add(new Card(suit, val));
                }
            }

            MakeSpecialCards();
            AddStickers();
        }

        private void MakeSpecialCards()
        {
            var used = new HashSet<int>();
            for (int i = 0; i < 5; i++)
            {
                int index = _random.Next(_cards.Count);
                while (used.Contains(index))
                    index = _random.Next(_cards.Count);
                used.Add(index);

                Card old = _cards[index];

                Card special;
                int mod = i % 5;
                if (mod == 0) special = new BonusCard(old.Suit, old.Value);
                else if (mod == 1) special = new ExtraCard(old.Suit, old.Value);
                else if (mod == 2) special = new GlassCard(old.Suit, old.Value);
                else if (mod == 3) special = new WildCard(old.Suit, old.Value);
                else /* mod == 4 */ special = new SteelCard(old.Suit, old.Value);

                _cards[index] = special;
            }
        }

        private void AddStickers()
        {
            var used = new HashSet<int>();
            for (int i = 0; i < 3; i++)
            {
                int index = _random.Next(_cards.Count);
                while (used.Contains(index))
                    index = _random.Next(_cards.Count);
                used.Add(index);

                Card card = _cards[index];

                if (!(card is StickerDecorator))
                {
                    int points = (_random.Next(1, 4)) * 5;
                    _cards[index] = new StickerDecorator(card, points);
                }
            }
        }

        public void Shuffle()
        {
            _cards = _cards.OrderBy(x => _random.Next()).ToList();
        }

        public Card? TakeCard()
        {
            if (_cards.Count == 0) return null;
            var card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }
    }
}
