namespace Exercisr.Contracts.Geo
{
    public interface ICoordinate
    {
        int HistoryItemId { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        double Altitude { get; set; }
        double HorizontalAccuracy { get; set; }
        double VerticalAccuracy { get; set; }
        double Speed { get; set; }
        double Course { get; set; }

    }
}
