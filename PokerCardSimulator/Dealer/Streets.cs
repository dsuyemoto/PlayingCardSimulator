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

        public bool DealCards()
        {
            if (CurrentStreet != StreetName.Ended)
            {
                _streets[_counter].DealCards();
                _counter++;

                return true;
            }

            return false;
        }
    }
}
