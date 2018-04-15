namespace Map.Math
{
    /// <summary>
    /// Representation of 2D vectors.
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// X component of the <see cref="Vector2"/>.
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Y component of the <see cref="Vector2"/>.
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Constructs a new instance of the <see cref="Vector2"/> struct with given a x, y components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Sum two 2D Vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Substract one 2d Vector from another.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Returns the lenght of this Vector.
        /// </summary>
        /// <returns></returns>
        public float Magnitude()
        {
            return (float) System.Math.Sqrt(this.X * this.X + this.Y * this.Y);
        }

        /// <summary>
        /// Returns Dot product(Scalar product) of a and b 2D vectors. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        /// <summary>
        /// Returns the distance between a and b 2d Vectors.
        /// </summary>
        /// <returns></returns>
        public static float Distance(Vector2 a, Vector2 b)
        {
            return (a - b).Magnitude();
        }

        /// <summary>
        /// Return the angle in degrees between two 2D vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Angle(Vector2 a, Vector2 b)
        {
            var scalarProduct = Dot(a, b);
            var aLenght = a.Magnitude();
            var bLenght = b.Magnitude();

            var formula = scalarProduct / (aLenght * bLenght);

            // Для того что бы получить угол надо сделать обратное преобразование через Acos
            // Acos выводить значение в радианах поэтому преобразуем радианы в градусы.

            return (float)System.Math.Acos(formula).RadianToDegree();
        }
    }
}
