using System.Collections.Generic;

namespace Dealer
{
    public interface ITexasHoldemTable
    {
        bool AutoStartEnabled { get; set; }
        decimal BigBlind { get; set; }
        int BigBlindSeatNumber { get; set; }
        List<Card> Community { get; set; }
        int DealerButtonSeatNumber { get; set; }
        Deck Deck { get; set; }
        decimal LastBet { get; set; }
        List<Player> Players { get; }
        double PlayerTimeoutMilliseconds { get; set; }
        decimal Pot { get; set; }
        int Seats { get; set; }
        decimal SmallBlind { get; set; }
        int SmallBlindSeatNumber { get; set; }
        int StartDealingSeatNumber { get; set; }
        Streets Streets { get; set; }
        int TableId { get; set; }

        void InitializeStreets();
        void DealCommunityCards(StreetBase street);
        void DealHand();
        void FixDealerButton();
        Player GetBlindPlayer(Player.BlindName blindName);
        void SetBlinds();
        void SitIn(int seatNumber);
        void StartBettingRound(int startingSeatNumber);
    }
}