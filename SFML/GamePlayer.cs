namespace SFML
{
    public class GamePlayer 
    {
        public int WinsAmount { get; private set; }
        public Circle circle { get; private set; }

        public GamePlayer()
        {
            WinsAmount = 0;
            circle = new Circle();
        }

        public void IncreaseWins() =>
            WinsAmount++;
    }
}