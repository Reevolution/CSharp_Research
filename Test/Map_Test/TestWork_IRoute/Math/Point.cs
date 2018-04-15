namespace Map.Math
{
    /// <summary>
    /// Represent of 2D Points.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// X component of the <see cref="Point"/>
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Y component of the <see cref="Point"/>
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Constructs a new instance of the <see cref="Point"/> struct with given a x, y components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Converts two points to Vector2.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 PointsToVector2(Point a, Point b)
        {
            return new Vector2(b.X - a.X, b.Y - a.Y);
        }

        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Distance(Point a, Point b)
        {
            return (float) System.Math.Sqrt(System.Math.Pow(b.X - a.X, 2) + System.Math.Pow(b.Y - a.Y, 2));
        }
    }
}
