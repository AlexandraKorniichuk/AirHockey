﻿using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System;

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
        private Vector2f LastPlayerPosition;

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
            SetDirections();

            GameLoop();
        }

        public void GameLoop()
        {
            do
            {
                Window.DispatchEvents();

                Draw();
                Vector2f MousePosition = (Vector2f)Mouse.GetPosition(Window);
                MoveObjects(MousePosition);
                CheckClashes();

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

        private void SetDirections()
        {
            Vector2f BotDirection = new Vector2f(0, 0.5f);
            Vector2f StartDirection = new Vector2f(0.3f, 0.3f);

            Player1.Direction = new Vector2f();
            Player2.Direction = BotDirection;
            Ball.Direction = StartDirection;
        }

        private void Draw()
        {
            Window.Draw(Player1.Circle);
            Window.Draw(Player2.Circle);
            Window.Draw(Ball.Circle);
        }

        private void MoveObjects(Vector2f MousePosition)
        {
            Vector2f PlayerPosition = Player1.Circle.Position;
            LastPlayerPosition = PlayerPosition;

            if (IsMouseInside(MousePosition, PlayerRadius))
                PlayerPosition = MousePosition;

            if (!IsObjectYInside(Player2.Circle.Position, PlayerRadius))
                Player2.Direction.Y *= -1;

            if (!IsObjectXInside(Ball.Circle.Position, BallRadius))
                Ball.Direction.X *= -1;
            if (!IsObjectYInside(Ball.Circle.Position, BallRadius))
                Ball.Direction.Y *= -1;

            Player1.SetPosition(PlayerPosition);
            Player2.ChangePosition();
            Ball.ChangePosition();

            Player1.Direction = PlayerPosition - LastPlayerPosition;
        }

        private bool IsMouseInside(Vector2f MousePosition, float Radius) =>
            IsObjectXInside(MousePosition, Radius, Width / 2) && IsObjectYInside(MousePosition, Radius);

        private bool IsObjectXInside(Vector2f position, float radius, uint width = Width) =>
            position.X >= 0 && position.X <= width - radius * 2;

        private bool IsObjectYInside(Vector2f position, float radius) =>
            position.Y >= 0 && position.Y <= Heigh - radius * 2;

        private void CheckClashes()
        {
            CheckClashWithPlayer(Player1);
            CheckClashWithPlayer(Player2);
        }

        private void CheckClashWithPlayer(GameObject Player)
        {
            if (HaveObjectsClashed(Player.Circle.Position, Ball.Circle.Position, PlayerRadius, BallRadius))
                SetBallDirectionAfterClash(Player.Direction);
        }

        private bool HaveObjectsClashed(Vector2f position1, Vector2f position2, float radius1, float radius2) =>
            CalculateDistance(position1.X, position1.Y, position2.X, position2.Y) 
            <= (radius1 + radius2) * (radius1 + radius2);

        private float CalculateDistance(float x1, float y1, float x2, float y2) =>
            (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);

        private void SetBallDirectionAfterClash(Vector2f PlayerDirection)
        {
            Ball.Direction += PlayerRadius * PlayerDirection / BallRadius;
        }

        private bool HasBallReachedGate() =>
            Ball.Circle.Position.X <= 0 || Ball.Circle.Position.X >= Width;

        private bool IsEndRound() =>
            Player1.WinsAmount == WinsAmountToWin || Player2.WinsAmount == WinsAmountToWin;
    }
}