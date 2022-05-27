using System;
using SFML.Window;
using SFML.Graphics;

namespace SFML
{
    public class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(Game.Width, Game.Heigh), "Air Hockey");
            window.Closed += WindowClosed;
            Game game = new Game(window);
            game.StartNewGame();
            game.DrawResults();
        }

        static void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}
