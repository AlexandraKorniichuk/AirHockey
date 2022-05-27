using SFML.System;

namespace SFML
{
    public class Ball
    {
        public Circle circle;

        public Ball()
        {
            circle = new Circle();
            circle.MaxDirection = new Vector2f(15, 15);
        }
    }
}