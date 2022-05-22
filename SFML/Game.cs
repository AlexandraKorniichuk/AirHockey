using System;
using SFML.Graphics;

namespace SFML
{
    public class Game
    {
        private GamePlayer Player1;
        private GamePlayer Player2;
        private GameObject Ball;

        private RenderWindow Window;
        public const uint Width = 1000, Heigh = 500;
        private const int PlayerRadius = 20, BallRadius = 10;
        public Game(RenderWindow window)
        {
            Window = window;

            Player1 = new GamePlayer();
            Player2 = new GamePlayer();
            Ball = new GameObject();
        }

        public void StartNewRound()
        {
            Player1.Circle = Player1.CreateCircle(Radius: PlayerRadius);
            Player2.Circle = Player2.CreateCircle(Radius: PlayerRadius);
            Ball.Circle = Ball.CreateCircle(Radius: BallRadius);
        }

        public void GameLoop()
        {
            do
            {
                Window.DispatchEvents();

                Draw();
                Input();
                MoveObjects();

                Window.Display();
                Window.Clear();
            } while (!IsEndRound()) ;
        }

        private void Draw()
        {

        }

        private void Input()
        {

        }

        private void MoveObjects()
        {

        }

        private bool IsEndRound()
        {
            return false;
        }
    }
}