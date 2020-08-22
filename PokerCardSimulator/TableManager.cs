using Dealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerCardSimulator
{
    public class TableManager
    {
        static List<TexasHoldemBase> _tables = new List<TexasHoldemBase>();

        public enum TableType
        {
            NoLimitHoldem
        }

        public static TableBase CreateTable(
            TableType tableType,
            int tableId,
            Deck deck,
            decimal smallBlind,
            decimal bigBlind,
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

        public static async Task<TexasHoldemView> GetTexasHoldemView(int tableId, int playerId)
        {
            var table = await Task.Run(() => _tables.Single(t => t.TableId == tableId));

            return new TexasHoldemView(table, playerId);
        }

        public static TexasHoldemBase GetTable(int tableId)
        {
            if (_tables.Exists(t => t.TableId == tableId))
                return _tables.Single(t => t.TableId == tableId);

            return null;
        }
    }
}
