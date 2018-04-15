namespace Map.Framework
{
    /// <summary>
    /// Документ для доставки
    /// </summary>
    public interface IDoc
    {
        /// <summary>
        /// Географическая широта
        /// </summary>
        double Lat { get; }
        /// <summary>
        /// Географическая долгота
        /// </summary>
        double Lon { get; }
        /// <summary>
        /// Вес товара
        /// </summary>
        double Weight { get; }
    }
}