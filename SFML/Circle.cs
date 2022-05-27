using SFML.Graphics;
using SFML.System;

namespace SFML
{
    public class Circle : CircleShape
    {
        public Vector2f Direction;

        public void CreateCircle(Color color, int radius)
        {
            FillColor = color;
            Radius = radius;
            Origin = new Vector2f(Radius, Radius);
        }

        public void SetPosition(Vector2f position)
        {
            Position = position;
        }

        public void ChangePosition()
        {
            Position += Direction;
        }

        public void ChangeDirectionIfOutside()
        {
            if (!IsObjectXInside(Position, rightEdge: 0))
                Direction.X *= -1;
            if (!IsObjectYInside(Position))
                Direction.Y *= -1;
        }

        public bool IsObjectXInside(Vector2f position, uint rightEdge = 0, uint leftEdge = Game.Width) =>
            position.X >= rightEdge + Radius && position.X <= leftEdge - Radius;

        public bool IsObjectYInside(Vector2f position) =>
            position.Y >= Radius && position.Y <= Game.Heigh - Radius;

        public void DecreaseDirection()
        {
            const float percentage = 0.8f;
            if (Direction.X > 5)
                Direction.X *= percentage;

            if (Direction.Y > 5)
                Direction.Y *= percentage;
        }
    }
}