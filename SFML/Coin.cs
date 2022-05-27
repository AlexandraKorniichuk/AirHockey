using System;

namespace SFML
{
    public class Coin
    {
        public Circle circle;

        public Coin()
        {
            circle = new Circle();
            circle.rand = new Random();
        }
    }
}