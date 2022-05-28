namespace SFML
{
    public class GamePlayer 
    {
        public int WinsAmount;
        public bool IsPlayerHitBall;
        public Circle circle;

        public GamePlayer()
        {
            WinsAmount = 0;
            circle = new Circle();
        }
    }
}