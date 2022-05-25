using SFML.System;

namespace SFML
{
    public class GamePlayer : GameObject
    {
        public int WinsAmount;
        public GamePlayer()
        {
            WinsAmount = 0;
            Direction = new Vector2f(0, 0.5f);
        }
    }
}