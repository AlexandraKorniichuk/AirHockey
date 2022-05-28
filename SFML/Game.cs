using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Threading;

namespace SFML
{
    public class Game
    {
        private GamePlayer Player1;
        private GamePlayer Player2;
        private Circle Ball;
        private Circle Coin;

        private RenderWindow Window;
        public const uint Width = 1000, Heigh = 500;
        
        private const int WinsAmountToWin = 7;
        private Text ScoreText;
        private Vector2f LastPlayerPosition;
        private Vector2f PlayerDirection;
        private GamePlayer LastTouchedPlayer;

        private Action OnRoundEnd;

        public Game(RenderWindow window)
        {
            Window = window;

            Player1 = new GamePlayer();
            Player2 = new GamePlayer();
            Ball = new Circle();
            Coin = new Circle();
            PlayerDirection = new Vector2f(0, 0);
            ScoreText = new Text("", new Font("BasicText.ttf"));
        }

        public void StartNewGame()
        {
            CreateObjects();
            SetStartPositions();
            CustomizeText();
            OnRoundEnd += SetStartPositions;

            GameLoop();
        }

        public void GameLoop()
        {
            do
            {
                Window.DispatchEvents();

                Vector2f MousePosition = (Vector2f)Mouse.GetPosition(Window);
                InputPlayerDirection();
                MoveObjects(MousePosition);
                ChangeDirections();
                Ball.DecreaseDirection();
                ChangeBallDirectionIfClashed();
                CheckingForCoin();
                CheckingForGoalScore();
                Draw();
                Wait();

                Window.Display();
                Window.Clear();
            } while (!IsEndGame()) ;
        }

        private void CreateObjects()
        {
            Player1.circle.CreateCircle(Colors.Player, (int)Radiuses.Player);
            Player2.circle.CreateCircle(Colors.Player, (int)Radiuses.Player);
            Ball.CreateCircle(Colors.Ball, (int)Radiuses.Ball);
            Coin.CreateCircle(Colors.Coin, (int)Radiuses.Coin);
        }

        public void SetStartPositions()
        {
            const int distanceFromEdge = 100;
            Vector2f position = new Vector2f(Width / 2, Heigh / 2);
            Ball.SetPosition(position);
            Ball.SetDirection();

            position.X = distanceFromEdge;
            Player1.circle.SetPosition(position);
            Mouse.SetPosition((Vector2i)Player1.circle.Position, Window);

            position.X = Width - distanceFromEdge;
            Player2.circle.SetPosition(position);
            Player2.circle.SetDirection();

            Coin.SetRandomPosition();
            LastTouchedPlayer = null;
        }

        private void CustomizeText()
        {
            const float TextY = 3;
            const uint Size = 40;
            ScoreText.CharacterSize = Size;
            ScoreText.Position = new Vector2f(Width / 2 - Size / 2, TextY);
        }

        private void InputPlayerDirection()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                PlayerDirection.Y = -10;
            else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                PlayerDirection.Y = 10;
            else
                PlayerDirection.Y = 0;
        }

        private void Draw()
        {
            Window.Draw(Coin);
            Window.Draw(Player1.circle);
            Window.Draw(Player2.circle);
            Window.Draw(Ball);
            Window.Draw(GetScoreMessage());
        }

        private void Wait()
        {
            const int timeToWait = 1;
            Thread.Sleep(timeToWait);
        }

        private Drawable GetScoreMessage()
        {
            ScoreText.DisplayedString = $"{Player1.WinsAmount} : {Player2.WinsAmount}";
            return ScoreText;
        }

        private void MoveObjects(Vector2f MousePosition)
        {
            Vector2f PlayerPosition = Player1.circle.Position;
            LastPlayerPosition = PlayerPosition;

            if (IsMouseInside(MousePosition))
                PlayerPosition = MousePosition;

            Player1.circle.SetPosition(PlayerPosition);
            Player2.circle.ChangePositionIfInside();
            Ball.ChangePosition();
        }

        private bool IsMouseInside(Vector2f MousePosition) =>
            Player1.circle.IsObjectXInside(MousePosition, leftEdge: Width / 2 + (int)Radiuses.Player) && 
            Player1.circle.IsObjectYInside(MousePosition);

        private void ChangeDirections()
        {
            Ball.ChangeDirectionIfOutside();
            Player2.circle.Direction = PlayerDirection;
            Player1.circle.Direction = Player1.circle.Position - LastPlayerPosition;
        }

        private void ChangeBallDirectionIfClashed()
        {
            CheckClashWithBall(Player1);
            CheckClashWithBall(Player2);
        }

        private void CheckClashWithBall(GamePlayer ClashingPlayer)
        {
            if (Ball.HaveObjectsClashed(ClashingPlayer.circle))
            {
                Ball.SetBallDirectionAfterClash(ClashingPlayer.circle.Direction);
                LastTouchedPlayer = ClashingPlayer;
            }
        }

        private void CheckingForCoin()
        {
            CheckClashWithCoin(Player1);
            CheckClashWithCoin(Player2);
        }

        private void CheckClashWithCoin(GamePlayer Player)
        {
            if (Coin.HaveObjectsClashed(Ball) && Player == LastTouchedPlayer)
            {
                Player.WinsAmount++;
                Coin.SetRandomPosition();
            }
        }

        private void CheckingForGoalScore()
        {
            if (Ball.HasBallReachedLeftGate())
            {
                Player2.WinsAmount++;
                OnRoundEnd.Invoke();
            }
            else if (Ball.HasBallReachedRightGate())
            {
                Player1.WinsAmount++;
                OnRoundEnd.Invoke();
            }
        }

        private bool IsEndGame() =>
            Player1.WinsAmount == WinsAmountToWin || Player2.WinsAmount == WinsAmountToWin;

        public void DrawResults()
        {
            const uint Size = 40;
            string Winner = Player1.WinsAmount == WinsAmountToWin ? nameof(Player1) : nameof(Player2);
            Text ResultText = new Text
            {
                DisplayedString = $"Congratulations {Winner}",
                CharacterSize = Size,
                Position = new Vector2f(Width / 2 - 5 * Size, Heigh / 2),
                Font = new Font("BasicText.ttf")
            };

            while (!Keyboard.IsKeyPressed(Keyboard.Key.Enter))
            {
                Window.Draw(ResultText);
                Window.Display();
                Window.Clear();
            }
        }
    }
}