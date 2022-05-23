using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace SFML
{
    public class Game
    {
        private GamePlayer Player1;
        private GamePlayer Player2;
        private GameObject Ball;
        private const int PlayerRadius = 20, BallRadius = 10;
        private Color PlayerColor = Color.Green, BallColor = Color.Red;

        private RenderWindow Window;
        public const uint Width = 1000, Heigh = 500;

        private const int WinsAmountToWin = 7;

        public Game(RenderWindow window)
        {
            Window = window;

            Player1 = new GamePlayer();
            Player2 = new GamePlayer();
            Ball = new GameObject();
        }

        public void StartNewRound()
        {
            CreateObjects();
            SetStartPositions();
            Mouse.SetPosition((Vector2i)Player1.Circle.Position, Window);

            GameLoop();
        }

        public void GameLoop()
        {
            do
            {
                Window.DispatchEvents();

                Draw();
                Vector2i MousePosition = Mouse.GetPosition(Window);
                MoveObjects(MousePosition);

                Window.Display();
                Window.Clear();
            } while (!IsEndRound()) ;
        }

        private void CreateObjects()
        {
            Player1.CreateCircle(PlayerColor, PlayerRadius);
            Player2.CreateCircle(PlayerColor, PlayerRadius);
            Ball.CreateCircle(BallColor, BallRadius);
        }

        private void SetStartPositions()
        {
            const int distanceFromEdge = 100;
            Vector2f position = new Vector2f(Width / 2, Heigh / 2);
            Ball.SetPosition(position);

            position.X = distanceFromEdge;
            Player1.SetPosition(position);

            position.X = Width - distanceFromEdge;
            Player2.SetPosition(position);

        }

        private void Draw()
        {
            Window.Draw(Player1.Circle);
            Window.Draw(Player2.Circle);
            Window.Draw(Ball.Circle);
        }

        private void MoveObjects(Vector2i MousePosition)
        {
            if (IsMouseInside(MousePosition, Player1.Circle.Radius))
                Player1.SetPosition((Vector2f)MousePosition);
        }

        private bool IsMouseInside(Vector2i mousePosition, float radius) =>
            mousePosition.X >= 0 && mousePosition.X <= Width / 2 - radius * 2 &&
            mousePosition.Y >= 0 && mousePosition.Y <= Heigh - radius * 2;

        private bool IsEndRound() =>
            Player1.WinsAmount == WinsAmountToWin || Player2.WinsAmount == WinsAmountToWin;
    }
}