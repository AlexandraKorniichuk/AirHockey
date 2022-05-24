using SFML.Graphics;
using SFML.System;

namespace SFML
{
    public class GameObject
    {
        public CircleShape Circle { get; private set; }
        public Vector2f Direction;
        public Vector2f MaxDirection;

        public GameObject()
        {
            Direction = new Vector2f();
            MaxDirection = new Vector2f(2, 2);
        }

        public void CreateCircle(Color Color, int Radius)
        {
            Circle = new CircleShape();
            Circle.FillColor = Color;
            Circle.Radius = Radius;
        }

        public void SetPosition(Vector2f Position)
        {
            Circle.Position = Position;
        }

        public void ChangePosition()
        {
            Circle.Position += Direction;
        }

        public void CheckMaxDirection()
        {
            if (Direction.X > MaxDirection.X)
                Direction.X = MaxDirection.X;
            if (Direction.Y > MaxDirection.Y)
                Direction.Y = MaxDirection.Y;
        }
    }
}