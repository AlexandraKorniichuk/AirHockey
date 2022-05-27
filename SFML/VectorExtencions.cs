using SFML.System;

namespace SFML
{
    public class VectorExtencions
    {
        public static float CalculateSquaredDistance(Vector2f from, Vector2f to)
        {
            float dx = from.X - to.X;
            float dy = from.Y - to.Y;
            return dx * dx + dy * dy;
        }
    }
}