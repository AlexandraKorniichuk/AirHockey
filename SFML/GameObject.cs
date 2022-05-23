using SFML.Graphics;
using SFML.System;

namespace SFML
{
    public class GameObject
    {
        public CircleShape Circle { get; private set; }
        public Vector2f Direction;

        public GameObject()
        {
            Direction = new Vector2f();
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
    }
}