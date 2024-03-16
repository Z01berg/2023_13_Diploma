namespace UI.Events
{
    public class CardEvent
    {
        public readonly Wrapper card;

        public CardEvent(Wrapper card)
        {
            this.card = card;
        }
    }
}