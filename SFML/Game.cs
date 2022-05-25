using SFML.System;
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
        private const int PlayerRadius = 30, BallRadius = 15;
        private Color PlayerColor = Color.Cyan, BallColor = Color.Magenta;

        private RenderWindow Window;
        public const uint Width = 1000, Heigh = 500;

        private const int WinsAmountToWin = 7;
        private Text ScoreText;
        private Vector2f LastPlayerPosition;

        private Action OnWinsAmountChanged;

        public Game(RenderWindow window)
        {
            Window = window;

            Player1 = new GamePlayer();
            Player2 = new GamePlayer();
            Ball = new GameObject();
            ScoreText = new Text("", new Font("BasicText.ttf"));
        }

        public void StartNewRound()
        {
            CreateObjects();
            SetStartPositions();
            CustomizeText();
            Mouse.SetPosition((Vector2i)Player1.Circle.Position, Window);
            OnWinsAmountChanged += SetStartPositions;

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
                ChangeDirections();
                DecreaseBallDirection();
                ChangeBallDirectionIfClashed();
                AddWin();

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

        public void SetStartPositions()
        {
            const int distanceFromEdge = 100;
            Vector2f position = new Vector2f(Width / 2, Heigh / 2);
            Ball.SetPosition(position);
            Ball.Direction = new Vector2f(0, 0);

            position.X = distanceFromEdge;
            Player1.SetPosition(position);

            position.X = Width - distanceFromEdge;
            Player2.SetPosition(position);
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
            Window.Draw(Player1.Circle);
            Window.Draw(Player2.Circle);
            Window.Draw(Ball.Circle);
            Window.Draw(GetScoreMessage());
        }

        private Drawable GetScoreMessage()
        {
            ScoreText.DisplayedString = $"{Player1.WinsAmount} : {Player2.WinsAmount}";
            return ScoreText;
        }

        private void MoveObjects(Vector2f MousePosition)
        {
            Vector2f PlayerPosition = Player1.Circle.Position;
            LastPlayerPosition = PlayerPosition;

            if (IsMouseInside(MousePosition, PlayerRadius))
                PlayerPosition = MousePosition;

            Player1.SetPosition(PlayerPosition);
            Player2.ChangePosition();
            Ball.ChangePosition();
        }

        private bool IsMouseInside(Vector2f MousePosition, float Radius) =>
            IsObjectXInside(MousePosition, Radius, leftEdge: Width / 2 + PlayerRadius) && 
            IsObjectYInside(MousePosition, Radius);

        private bool IsObjectXInside(Vector2f position, float radius, uint rightEdge = 0, uint leftEdge = Width) =>
            position.X >= rightEdge && position.X <= leftEdge - radius * 2;

        private bool IsObjectYInside(Vector2f position, float radius) =>
            position.Y >= 0 && position.Y <= Heigh - radius * 2;

        private void ChangeDirections()
        {
            ChangeDirectionIfObjectOutside(Ball);
            ChangeDirectionIfObjectOutside(Player2, rightEndge: Width / 2);
            Player1.Direction = Player1.Circle.Position - LastPlayerPosition;
        }

        private void ChangeDirectionIfObjectOutside(GameObject gameObject, uint rightEndge = 0)
        {
            if (!IsObjectXInside(gameObject.Circle.Position, gameObject.Circle.Radius, rightEdge: rightEndge))
                gameObject.Direction.X *= -1;
            if (!IsObjectYInside(gameObject.Circle.Position, gameObject.Circle.Radius))
                gameObject.Direction.Y *= -1;
        }

        private void DecreaseBallDirection()
        {
            const float percentage = 0.8f;
            if (Ball.Direction.X > 0.3f)
                Ball.Direction.X *= percentage;

            if (Ball.Direction.Y > 0.3f)
                Ball.Direction.Y *= percentage;
        }

        private void ChangeBallDirectionIfClashed()
        {
            CheckClashWithPlayer(Player1);
            CheckClashWithPlayer(Player2);
        }

        private void CheckClashWithPlayer(GameObject Player)
        {
            if (HaveObjectsClashed(Player.Circle.Position, Ball.Circle.Position))
                SetBallDirectionAfterClash(Player.Direction);
        }

        private bool HaveObjectsClashed(Vector2f position1, Vector2f position2) =>
            CalculateDistance(position1.X, position1.Y, position2.X, position2.Y)
            <= (PlayerRadius + 2 * BallRadius) * (PlayerRadius + 2 * BallRadius);

        private float CalculateDistance(float x1, float y1, float x2, float y2) =>
            (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);

        private void SetBallDirectionAfterClash(Vector2f PlayerDirection)
        {
            Ball.Direction += PlayerRadius * PlayerDirection / BallRadius;
            Ball.ChangeDirectionIfHigherThanMax();
        }

        private void AddWin()
        {
            if (HasBallReachedLeftGate())
            {
                Player2.WinsAmount++;
                OnWinsAmountChanged.Invoke();
            }
            else if (HasBallReachedRightGate())
            {
                Player1.WinsAmount++;
                OnWinsAmountChanged.Invoke();
            }
        }

        private bool HasBallReachedLeftGate() =>
            Ball.Circle.Position.X <= 0;

        private bool HasBallReachedRightGate() =>
             Ball.Circle.Position.X >= Width - 2 * BallRadius;

        private bool IsEndRound() =>
            Player1.WinsAmount == WinsAmountToWin || Player2.WinsAmount == WinsAmountToWin;
    }
}