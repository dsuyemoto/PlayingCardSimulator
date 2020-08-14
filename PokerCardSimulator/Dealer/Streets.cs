using System.Collections.Generic;
using static Dealer.TableBase;

namespace Dealer
{
    public class Streets
    {
        private List<StreetBase> _streets = new List<StreetBase>();
        private int _counter = 0;

        public StreetName CurrentStreet {
            get {
                if (_counter < _streets.Count)
                    return _streets[_counter].Name;
                else
                    return StreetName.Ended;
            }
        }

        public Streets()
        {

        }

        public void Add(StreetBase streetBase)
        {
            _streets.Add(streetBase);
        }

        public void DealCards()
        {
            _streets[_counter].DealCards();
        }

        public void StartBettingRound(int startingSeatNumber)
        {
            _streets[_counter].StartBettingRound(startingSeatNumber);
        }

        public void CollectBets()
        {
            _streets[_counter].CollectBets();
        }

        public void PayWinner()
        {
            _streets[_counter].PayWinner();
        }

        public bool Next()
        {
            _counter++;
            if (CurrentStreet == StreetName.Ended)
                return false;
            
            return true;
        }
    }
}
