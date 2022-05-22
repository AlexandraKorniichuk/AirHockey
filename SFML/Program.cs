using System;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace SFML
{
    public class Program
    {
        static Vector2f Direction = new Vector2f(0.3f, 0.3f);
        const uint w = 1000, h = 500;
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(w, h), "Circle");
            window.Closed += WindowClosed;
            CircleShape Circle = CreateCircle();
            Vector2f CirclePosition = new Vector2f(1, 1);
            while (window.IsOpen)
            {
                window.DispatchEvents();
                Circle.Draw(window, RenderStates.Default);
                Circle.Position = CirclePosition;
                CirclePosition = ChangePosition(CirclePosition, Circle);
                Circle = DecreaseCircle(Circle);
                window.Display();
                window.Clear();
            }
        }


        static CircleShape CreateCircle()
        {
            CircleShape circle = new CircleShape();
            circle.FillColor = Color.Green;
            circle.Radius = 20;
            return circle;
        }

        private static Vector2f ChangePosition(Vector2f circlePosition, CircleShape Circle)
        {
            if (circlePosition.X + Circle.Radius * 2 > w || circlePosition.X < 0)
                Direction.X *= -1;
            if (circlePosition.Y + Circle.Radius * 2 > h || circlePosition.Y < 0)
                Direction.Y *= -1;
            circlePosition.X += Direction.X;
            circlePosition.Y += Direction.Y;
            return circlePosition;
        }

        private static CircleShape DecreaseCircle(CircleShape circle)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                circle.Radius *= 0.9999f;
            return circle;
        }

        static void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}
