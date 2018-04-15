using System.Collections.Generic;

namespace Map.Framework
{
    /// <summary>
    /// Маршрут автомобиля
    /// </summary>
    public interface IRoute
    {
        /// <summary>
        /// Список документов в маршруте
        /// </summary>
        List<IDoc> Docs { get; set; }
    }
}