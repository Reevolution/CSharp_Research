namespace Map.Framework
{
    /// <summary>
    /// Автомобиль для доставки
    /// </summary>
    public interface ICar
    {
        /// <summary>
        /// Начальная широта
        /// </summary>
        double StartLat { get; }
        /// <summary>
        /// Начальная долгота
        /// </summary>
        double StartLon { get; }
        /// <summary>
        /// Максимальный вес
        /// </summary>
        double MaxWeight { get; }
        /// <summary>
        /// Создать маршрут с этим автомобилем
        /// </summary>
        /// <returns></returns>
        IRoute CreateRoute();
    }
}