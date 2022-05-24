using SFML.System;

namespace SFML
{
    public class GamePlayer : GameObject
    {
        public int WinsAmount;
        public GamePlayer()
        {
            WinsAmount = 0;
            MaxDirection = new Vector2f(-0.3f, 0);
        }
    }
}