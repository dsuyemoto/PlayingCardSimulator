using System.Collections.Generic;

namespace Dealer
{
    public abstract class TableViewBase
    {
        public abstract int TableId { get; }
        public abstract int Seats { get; }
        public abstract decimal Pot { get; }
        public abstract int PlayerTimeout { get; }
        public List<Player> Players { get; }

        public TableViewBase(TableBase tableBase, int playerId)
        {
            Players = new List<Player>();

            foreach (var player in tableBase.Players)
            {
                if (player.Id != playerId)
                    player.Cards.Clear();

                Players.Add(player);
            }
        }


    }
}
