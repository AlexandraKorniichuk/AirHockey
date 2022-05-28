using SFML.Window;
using SFML.System;
using SFML.Graphics;

namespace SFML
{
    public class InputController
    {
        public static float InputPlayerDirection()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                return -10;
            else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                return 10;
            return 0;
        }

        public static Vector2f GetMousePosition(RenderWindow Window) =>
            (Vector2f)Mouse.GetPosition(Window);

        public static bool IsEnterPressed() =>
            Keyboard.IsKeyPressed(Keyboard.Key.Enter);
    }
}