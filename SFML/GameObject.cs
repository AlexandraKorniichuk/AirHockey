using SFML.Graphics;

namespace SFML
{
    public class GameObject
    {
        public CircleShape Circle;

        public CircleShape CreateCircle(int Radius)
        {
            CircleShape circle = new CircleShape();
            circle.FillColor = Color.Green;
            circle.Radius = Radius;
            return circle;
        }
    }
}