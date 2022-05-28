using SFML.Graphics;
using SFML.System;
using System;

namespace SFML
{
    public class Circle : CircleShape
    {
        public Vector2f Direction;

        private Vector2f MaxDirection;
        private Random rand;

        public Circle()
        {
            rand = new Random();
            MaxDirection = new Vector2f(15, 15);
        }

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

        public void SetDirection()
        {
            Direction = new Vector2f(0, 0);
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

        public void ChangePositionIfInside()
        {
            if (IsObjectYInside(Position + Direction))
                ChangePosition();
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

        public bool HaveObjectsClashed(CircleShape OtherCircle) =>
            VectorExtencions.CalculateSquaredDistance(Position, OtherCircle.Position)
            <= (Radius + OtherCircle.Radius) * (Radius + OtherCircle.Radius);

        public void SetBallDirectionAfterClash(Vector2f PlayerDirection)
        {
            if (PlayerDirection.Y == 0) PlayerDirection.Y = 1;
            Direction += (int)Radiuses.Player * PlayerDirection / Radius;
            ChangeDirectionIfHigherThanMax();
        }

        private void ChangeDirectionIfHigherThanMax()
        {
            if (Direction.X > MaxDirection.X)
                Direction.X = MaxDirection.X;
            if (Direction.Y > MaxDirection.Y)
                Direction.Y = MaxDirection.Y;
        }

        public void SetRandomPosition()
        {
            int X = rand.Next((int)Radius, (int)Game.Width - (int)Radius);
            int Y = rand.Next((int)Radius, (int)Game.Heigh - (int)Radius);
            Position = new Vector2f(X, Y);
        }

        public bool HasBallReachedLeftGate() =>
            Position.X <= 0;

        public bool HasBallReachedRightGate() =>
             Position.X >= Game.Width - (int)Radiuses.Ball;
    }
}