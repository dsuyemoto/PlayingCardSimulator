using System;

namespace Dealer
{
    public class ActionPromptedEventArgs : EventArgs
    {
        public PlayerOptions PlayerOptions { get; set; }
    }
}