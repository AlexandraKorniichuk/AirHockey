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
        private Ball Ball;
        private Coin Coin;

        private RenderWindow Window;
        public const uint Width = 1000, Heigh = 500;
        
        private const int WinsAmountToWin = 7;
        private Text ScoreText;
        private Vector2f LastPlayerPosition;

        private Action OnRoundEnd;

        public Game(RenderWindow window)
        {
            Window = window;

            Player1 = new GamePlayer();
            Player2 = new GamePlayer();
            Ball = new Ball();
            Coin = new Coin();
            ScoreText = new Text("", new Font("BasicText.ttf"));
        }

        public void StartNewGame()
        {
            CreateObjects();
            SetStartPositions();
            CustomizeText();
            Mouse.SetPosition((Vector2i)Player1.circle.Position, Window);
            OnRoundEnd += SetStartPositions;

            GameLoop();
        }

        public void GameLoop()
        {
            do
            {
                Window.DispatchEvents();

                Vector2f MousePosition = (Vector2f)Mouse.GetPosition(Window);
                MoveObjects(MousePosition);
                ChangeDirections();
                Ball.circle.DecreaseDirection();
                ChangeBallDirectionIfClashed();
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
            Ball.circle.CreateCircle(Colors.Ball, (int)Radiuses.Ball);
            Coin.circle.CreateCircle(Colors.Coin, (int)Radiuses.Coin);
        }

        public void SetStartPositions()
        {
            const int distanceFromEdge = 100;
            Vector2f position = new Vector2f(Width / 2, Heigh / 2);
            Ball.circle.SetPosition(position);
            Ball.circle.Direction = new Vector2f(0, 0);

            position.X = distanceFromEdge;
            Player1.circle.SetPosition(position);

            position.X = Width - distanceFromEdge;
            Player2.circle.SetPosition(position);
            Player2.circle.Direction = new Vector2f(0, 15);

            Coin.circle.SetRandomPosition();
        }

        private void CustomizeText()
        {
            const float TextY = 3;
            const uint Size = 40;
            ScoreText.CharacterSize = Size;
            ScoreText.Position = new Vector2f(Width / 2 - Size / 2, TextY);
        }

        private void Draw()
        {
            Window.Draw(Player1.circle);
            Window.Draw(Player2.circle);
            Window.Draw(Ball.circle);
            Window.Draw(Coin.circle);
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
            Player2.circle.ChangePosition();
            Ball.circle.ChangePosition();
        }

        private bool IsMouseInside(Vector2f MousePosition) =>
            Player1.circle.IsObjectXInside(MousePosition, leftEdge: Width / 2 + (int)Radiuses.Player) && 
            Player1.circle.IsObjectYInside(MousePosition);

        private void ChangeDirections()
        {
            Ball.circle.ChangeDirectionIfOutside();
            Player2.circle.ChangeDirectionIfOutside();
            Player1.circle.Direction = Player1.circle.Position - LastPlayerPosition;
        }

        private void ChangeBallDirectionIfClashed()
        {
            CheckClashWithPlayer(Player1);
            CheckClashWithPlayer(Player2);
        }

        private void CheckClashWithPlayer(GamePlayer Player)
        {
            if (Ball.circle.HaveObjectsClashed(Player.circle))
                Ball.circle.SetBallDirectionAfterClash(Player.circle.Direction);
        }

        private void CheckingForGoalScore()
        {
            if (HasBallReachedLeftGate())
            {
                Player2.WinsAmount++;
                OnRoundEnd.Invoke();
            }
            else if (HasBallReachedRightGate())
            {
                Player1.WinsAmount++;
                OnRoundEnd.Invoke();
            }
        }

        private bool HasBallReachedLeftGate() =>
            Ball.circle.Position.X <= 0;

        private bool HasBallReachedRightGate() =>
             Ball.circle.Position.X >= Width - (int)Radiuses.Ball;

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