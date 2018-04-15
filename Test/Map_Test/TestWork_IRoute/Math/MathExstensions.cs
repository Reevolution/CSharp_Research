namespace Map.Math
{
    /// <summary>
    /// Provides Math exstensions.
    /// </summary>
    public static class MathExstensions
    {
        /// <summary>
        /// Converts degree angle to radian.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double DegreeToRadian(this double angle)
        {
            return System.Math.PI * angle / 180.0;
        }

        /// <summary>
        /// Converts radian angle to degree.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double RadianToDegree(this double angle)
        {
            return angle * (180.0 / System.Math.PI);
        }
    }
}
