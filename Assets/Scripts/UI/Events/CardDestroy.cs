namespace UI.Events
{
    public class CardDestroy : CardEvent
    {
        public CardDestroy(Wrapper card) : base(card)
        {
        }
    }
}