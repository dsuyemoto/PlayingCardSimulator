using Dealer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerCardSimulator
{
    public class TableManager : IObservable<PlayerObserverEvent>
    {
        static List<TableBase> _tables = new List<TableBase>();
        static List<IObserver<PlayerObserverEvent>> _playerObservers;

        public enum TableType
        {
            NoLimitHoldem
        }
        public TableManager()
        {
            _playerObservers = new List<IObserver<PlayerObserverEvent>>();
        }

        public static TableBase CreateTable(
            TableType tableType,
            int tableId,
            Deck deck,
            double smallBlind,
            double bigBlind,
            int seats,
            int dealerButton)
        {
            var table = _tables.Find(t => t.TableId == tableId);
            if (table != null) return table;
            
            switch (tableType)
            {
                case TableType.NoLimitHoldem:
                    table = new TexasHoldemNoLimit(tableId, deck, smallBlind, bigBlind, seats, dealerButton);
                    _tables.Add(table);
                    return table;
                default:
                    throw new Exception("TableType not implemented");
            }
        }

        public async static Task<TexasHoldemBase> GetTexasHoldemTableAsync(int tableId)
        {
            var table = await Task.Run(() =>
            {
                return (TexasHoldemBase)_tables.Find(t => t.TableId == tableId);
            });

            return table;
        }

        public IDisposable Subscribe(IObserver<PlayerObserverEvent> playerObserver)
        {
            if (!_playerObservers.Contains(playerObserver))
                _playerObservers.Add(playerObserver);

            return new Unsubscriber(_playerObservers, playerObserver);
        }

        public void NotifyObservers(PlayerObserverEvent playerObserverEvent)
        {
            foreach (var playerObserver in _playerObservers)
                playerObserver.OnNext(playerObserverEvent);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<PlayerObserverEvent>> _playerObservers;
            private IObserver<PlayerObserverEvent> _playerObserver;

            public Unsubscriber(
                List<IObserver<PlayerObserverEvent>> playerObservers,
                IObserver<PlayerObserverEvent> playerObserver
                )
            {
                this._playerObservers = playerObservers;
                this._playerObserver = playerObserver;
            }
            public void Dispose()
            {
                if (_playerObserver != null)
                    _playerObservers.Remove(_playerObserver);
            }
        }
    }
}
