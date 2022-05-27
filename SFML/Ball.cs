using SFML.System;

namespace SFML
{
    public class Ball
    {
        public Vector2f MaxDirection;
        public Circle circle;

        public Ball()
        {
            circle = new Circle();
            MaxDirection = new Vector2f(15, 15);
        }

        public void ChangeDirectionIfHigherThanMax()
        {
            if (circle.Direction.X > MaxDirection.X)
                circle.Direction.X = MaxDirection.X;
            if (circle.Direction.Y > MaxDirection.Y)
                circle.Direction.Y = MaxDirection.Y;
        }
    }
}