namespace UnityEngine
{
    public static class Vector2IntExtensions
    {
        public static Vector2Int Plus(this Vector2Int vector2, int value)
        {
            return new Vector2Int(vector2.x + value, vector2.y + value);
        }

        public static Vector2Int Minus(this Vector2Int vector2, int value)
        {
            return new Vector2Int(vector2.x + value, vector2.y - value);
        }
    }
}
