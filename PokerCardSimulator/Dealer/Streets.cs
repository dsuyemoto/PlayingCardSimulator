using System.Collections.Generic;
using static Dealer.TableBase;

namespace Dealer
{
    public class Streets
    {
        private List<StreetBase> _streets = new List<StreetBase>();
        private int _counter = 0;

        public StreetName CurrentStreet 
        {
            get { return _streets[_counter].Name; } 
        }

        public Streets()
        {

        }

        public void Add(StreetBase streetBase)
        {
            _streets.Add(streetBase);
        }

        public bool DealCards()
        {
            if (_counter < _streets.Count)
            {
                _streets[_counter].DealCards();
                _counter++;

                return true;
            }

            return false;
        }
    }
}
