using Laboratorio_7_OOP_201902.Cards;
using Laboratorio_7_OOP_201902.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Laboratorio_7_OOP_201902.Interfaces;


namespace Laboratorio_7_OOP_201902
{
    public class Deck : ICharacteristics
    {

        private List<Card> cards;
        public List<string> characteristics;

        public Deck()
        {
        
        }

        public List<Card> Cards { get => cards; set => cards = value; }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
        public void DestroyCard(int cardId)
        {
            cards.RemoveAt(cardId);
        }

        

        public void Shuffle()
        {
            Random random = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        
        public List<string> GetCharacteristics()
        {
            List<string> caracs = new List<string>();

            int totalCards = 0;
            List<Card> numQuery1 = (from card in cards
                                    select card).ToList();
            foreach (Card card in numQuery1)
                totalCards++;
            Console.WriteLine(totalCards);
            caracs.Add(($"Total cards of Player:{totalCards}"));

            int totalMeleeCards = 0;
            List<Card> numQuery2 = (from card in cards
                                    where card.Type == EnumType.melee
                                    select card).ToList();
            foreach (Card card in numQuery2)
                totalMeleeCards++;
            Console.WriteLine(totalMeleeCards);
            caracs.Add(($"Total melee cards of Player:{totalMeleeCards}"));

            int totalRangeCards = 0;
            List<Card> numQuery3 = (from card in cards
                                    where card.Type == EnumType.range
                                    select card).ToList();
            foreach (Card card in numQuery3)
                totalRangeCards++;
            Console.WriteLine(totalRangeCards);
            caracs.Add(($"Total range cards of Player:{totalRangeCards}"));

            int totalLongRangeCards = 0;
            List<Card> numQuery4 = (from card in cards
                                    where card.Type == EnumType.longRange
                                    select card).ToList();
            foreach (Card card in numQuery4)
                totalLongRangeCards++;
            Console.WriteLine(totalLongRangeCards);
            caracs.Add(($"Total long range cards of Player:{totalLongRangeCards}"));


            int totalbuffCards = 0;
            List<Card> numQuery5 = (from card in cards
                                    where card.Type == EnumType.buff
                                    select card).ToList();
            foreach (Card card in numQuery5)
                totalbuffCards++;
            Console.WriteLine(totalbuffCards);
            caracs.Add(($"Total buff cards of Player:{totalbuffCards}"));


            int totalWeatherCards = 0;
            List<Card> numQuery6 = (from card in cards
                                    where card.Type == EnumType.weather
                                    select card).ToList();
            foreach (Card card in numQuery6)
                totalWeatherCards++;
            Console.WriteLine(totalWeatherCards);
            caracs.Add(($"Total weather cards of Player:{totalWeatherCards}"));
            // para obtener los puntos de ataques para cada tipo se usaron los linqs hechos de antes para poder acceder a sus AttackPoints

            int totalMeleeAttackPoints = 0;
            foreach (CombatCard card in numQuery2)
                totalMeleeAttackPoints += card.AttackPoints;
            Console.WriteLine(totalMeleeAttackPoints);
            caracs.Add(($"Total melee attackPoints of Player:{totalMeleeAttackPoints}"));

            int totalRangeAttackPoints = 0;
            foreach (CombatCard card in numQuery3)
                totalRangeAttackPoints += card.AttackPoints;
            Console.WriteLine(totalRangeAttackPoints);
            caracs.Add(($"Total range attackPoints of Player:{totalRangeAttackPoints}"));

            int totalLongRangeAttackPoints = 0;
            foreach (CombatCard card in numQuery4)
                totalLongRangeAttackPoints += card.AttackPoints;
            Console.WriteLine(totalLongRangeAttackPoints);
            caracs.Add(($"Total long range attackPoints of Player:{totalLongRangeAttackPoints}"));

            int totalAttackPoints = totalMeleeAttackPoints + totalRangeAttackPoints + totalLongRangeAttackPoints;
            Console.WriteLine(totalAttackPoints);
            caracs.Add(($"Total attackPoints of Player:{totalAttackPoints}"));

            return caracs;
        }
    }
}
