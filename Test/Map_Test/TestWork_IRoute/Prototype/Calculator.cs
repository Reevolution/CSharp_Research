using System;
using System.Collections.Generic;
using System.Linq;
using Map.Framework;
using Map.Math;

namespace Map.Prototype
{
    /*
     * Задача: реализовать метод CalcRoutes в классе Calculator.
     * Метод должен распределять документы по машинам с учётом
     * максимальной грузоподъёмности.
     * В маршрут необходимо добавлять точки, наиболее близкие к 
     * последней точке маршрута (по прямой).
     * Исходная точка - StartLat и StartLon автомобиля.
     * Допускается остаток неразвозимых документов (все машины заполены).
     * Порядок выбора автомобилей не важен.
     * Изменять интерфейсы не допускается.
     * Изменения в классе Calculator не ограничены (кроме сигнатуры метода CalcRoutes),
     *   но работать он должен только с указанными интерфейсами, не с их реализацией.
     * Реализация интерфейсов с загрузкой из любых источников приветствуется.
     */

    /// <summary>
    /// Расчётчик маршрутов
    /// </summary>
    public class Calculator
    {
        private class UtilityDoc
        {
            public IDoc Doc { get; }
            public Vector2 Position { get; }

            public UtilityDoc(IDoc doc, Vector2 position)
            {
                Position = position;
                Doc = doc;
            }
        }

        /// <summary>
        /// Рассчитать маршруты
        /// </summary>
        /// <param name="docs">Список документов</param>
        /// <param name="cars">Список доступных машин</param>
        /// <returns>Список сформированных маршрутов</returns>
        public List<IRoute> CalcRoutes(List<IDoc> docs, List<ICar> cars)
        {
            if (docs == null)
            {
                throw new ArgumentNullException($"{nameof(docs)}");
            }

            if (cars == null)
            {
                throw new ArgumentNullException($"{nameof(cars)}");
            }

            var routes = new List<IRoute>();

            foreach (var car in cars)
            {
                var availableDocs = docs.Where(n => n.Weight < car.MaxWeight).ToList();

                if (availableDocs.Count != 0)
                {
                    var startDoc = availableDocs.OrderBy(n => Point.Distance(
                        new Point((float) car.StartLon, (float) car.StartLat),
                        new Point((float) n.Lon, (float) n.Lat))).FirstOrDefault();

                    docs.Remove(startDoc);
                    var route = CreateRoute(car, startDoc, docs);
                    routes.Add(route);
                }
            }

            return routes;
        }

        private IRoute CreateRoute(ICar car, IDoc startDoc, List<IDoc> docs)
        {
            var weight = startDoc.Weight;

            var utilityDocs = new List<UtilityDoc>
            {
                new UtilityDoc(startDoc,
                    Point.PointsToVector2(new Point((float) car.StartLon, (float) car.StartLat),
                        new Point((float) startDoc.Lon, (float) startDoc.Lat)))
            };

            while (docs.Count > 0 && weight <= car.MaxWeight)
            {
                var lastUtilityDoc = utilityDocs.LastOrDefault();

                if (lastUtilityDoc == null)
                {
                    throw new NullReferenceException("Last document is null.");
                }

                // Проверяем пакеты которые при добавлении не превысят грузоподъемность автомобиля.
                var acceptableDocs = docs.Where(n => weight + n.Weight <= car.MaxWeight).ToList();

                if (acceptableDocs.Count == 0)
                {
                    break;
                }

                // Сортируем документы по минимальному углу, и расстоянию, которые мы сверяем с предыдущим документом из маршрута.
                // Так как по прямой может попасться несколько пакетов, но на разном расстоянии.
                var potentialDocs = acceptableDocs.Select(n => new
                {
                    docValue = n,

                    angleValue = Vector2.Angle(lastUtilityDoc.Position, Point.PointsToVector2(
                        new Point((float) lastUtilityDoc.Doc.Lon, (float) lastUtilityDoc.Doc.Lat),
                        new Point((float) n.Lon, (float) n.Lat))),

                    distanceValue = Point.Distance(
                        new Point((float) lastUtilityDoc.Doc.Lon, (float) lastUtilityDoc.Doc.Lat),
                        new Point((float) n.Lon, (float) n.Lat))
                });
                potentialDocs = potentialDocs.OrderBy(n => n.angleValue).ThenBy(n => n.distanceValue).ToList();

                // Добавляем подходящий документ.
                var potentialDoc = potentialDocs.FirstOrDefault();

                if (potentialDoc == null)
                {
                    throw new NullReferenceException("Potential document is null.");
                }

                var potentialDocPosition = Point.PointsToVector2(
                    new Point((float) lastUtilityDoc.Doc.Lon, (float) lastUtilityDoc.Doc.Lat),
                    new Point((float) potentialDoc.docValue.Lon, (float) potentialDoc.docValue.Lat));

                var utilityDoc = new UtilityDoc(potentialDoc.docValue, potentialDocPosition);
                weight += utilityDoc.Doc.Weight;
                utilityDocs.Add(utilityDoc);
                docs.Remove(utilityDoc.Doc);
            }

            // Создаем маршрут.
            var route = car.CreateRoute();
            route.Docs = utilityDocs.ConvertAll(n => n.Doc);

            return route;
        }
    }
}
