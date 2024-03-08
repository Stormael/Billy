using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy.cards
{
    public class CardsSystem
    {
        private int[] cardNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, };
        private string[] cardSuits = { "Clubs", "Spades", "Diamonds", "Hearts" };

        public int selectedNumber { get; set; }
        public string selectedCard { get; set; }

        public CardsSystem() 
        {
            var random = new Random();
            int numberIndex = random.Next(0, cardNumbers.Length - 1);
            int suitIndex = random.Next(0, cardSuits.Length - 1);

            this.selectedNumber = cardNumbers[numberIndex];
            this.selectedCard = $"{cardNumbers[numberIndex]} of {cardSuits[suitIndex]}";
        }
    }
}
