using SFML.Graphics;
using SFML.System;

namespace SFML
{
    public class Circle
    {
        public CircleShape circle { get; private set; }
        public Vector2f Direction;

        public void CreateCircle(Color Color, int Radius)
        {
            circle = new CircleShape()
            {
                FillColor = Color,
                Radius = Radius,
                Origin = new Vector2f(Radius, Radius),
            };
        }

        public void SetPosition(Vector2f Position)
        {
            circle.Position = Position;
        }

        public void ChangePosition()
        {
            circle.Position += Direction;
        }

        public void ChangeDirectionIfOutside()
        {
            if (!IsObjectXInside(circle.Position, rightEdge: 0))
                Direction.X *= -1;
            if (!IsObjectYInside(circle.Position))
                Direction.Y *= -1;
        }

        public bool IsObjectXInside(Vector2f position, uint rightEdge = 0, uint leftEdge = Game.Width) =>
            position.X >= rightEdge + circle.Radius && position.X <= leftEdge - circle.Radius;

        public bool IsObjectYInside(Vector2f position) =>
            position.Y >= circle.Radius && position.Y <= Game.Heigh - circle.Radius;

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