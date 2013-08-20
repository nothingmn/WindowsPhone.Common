using Exercisr.Contracts.Geo;
using System.Device.Location;

namespace Exercisr.Domain.Geo
{
    public class Coordinate : GeoCoordinate , ICoordinate
    {
        public int Id { get; set; }
        public int HistoryItemId { get; set; }
    }
}
