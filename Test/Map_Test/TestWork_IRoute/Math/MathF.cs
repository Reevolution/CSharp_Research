namespace Map.Math
{
    /// <summary>
    /// Provides additional math operations.
    /// </summary>
    public static class MathF
    {
        /// <summary>
        /// Calculates the shortest difference between two given angles given in degrees.
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        public static float DeltaAngle(float alpha, float beta)
        {
            float phi = System.Math.Abs(beta - alpha) % 360; // This is either the distance or 360 - distance
            float distance = phi > 180 ? 360 - phi : phi;
            return distance;
        }
    }
}
