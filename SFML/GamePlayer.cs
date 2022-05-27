using SFML.Graphics;
using SFML.System;

namespace SFML
{
    public class GamePlayer 
    {
        public int WinsAmount;
        public Circle circle;

        public GamePlayer()
        {
            WinsAmount = 0;
            circle = new Circle();
        }
    }
}