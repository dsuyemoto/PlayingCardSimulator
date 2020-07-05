using Dealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dealer.TableBase;

namespace PokerCardSimulator
{
    public class TableFactory
    {
        static List<TableBase> _tables = new List<TableBase>();

        public enum TableType
        {
            NoLimitHoldem
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

        public static TexasHoldemBase GetTable(int tableId)
        {
            return (TexasHoldemBase)_tables.Find(t => t.TableId == tableId);
        }
    }
}
