namespace Dealer
{
    public class PlayerEvent
    {
        public PromptOptions Options { get; set; }

        public PlayerEvent(PromptOptions options)
        {
            Options = options;
        }
    }
}
