using System.Collections.Generic;

namespace Dealer
{
    public abstract class TableViewBase
    {
        public abstract int TableId { get; }
        public abstract int Seats { get; }
        public abstract decimal Pot { get; }
        public abstract double PlayerTimeoutMilliseconds { get; }
        public List<Player> Players { get; }

        public TableViewBase(TableBase tableBase, int playerId)
        {
            Players = new List<Player>();

            foreach (var player in tableBase.Players)
            {
                if (player.Id != playerId)
                {
                    var cards = new List<Card>();

                    foreach (var card in player.Cards)
                        if (!card.IsHidden)
                            cards.Add(card);

                    player.Cards = cards;
                }

                Players.Add(player);
            }
        }


    }
}
