namespace HumbertoMVC.Models;


public class OnibusRotaModel
{
    
    
    public List<RouteModel> Routes { get; set; }
    

    public class RouteModel
    {
        public int Cr { get; set; }
        public List<LegModel> Legs { get; set; }
    }

    public class LegModel
    {
        public List<StepModel> Steps { get; set; }
    }

    public class StepModel
    {
        public TransitDetailsModel TransitDetails { get; set; }
        public int TravelMode { get; set; }
    }

    public class TransitDetailsModel
    {
        public StopDetailModel StopDetails { get; set; }
        public LocalizedValuesModel LocalizedValues { get; set; }
        public string Headsign { get; set; }
        public TransitLineModel TransitLine { get; set; }
        public int StopCount { get; set; }
    }

    public class StopDetailModel
    {
        public StopModel ArrivalStop { get; set; }
        public StopModel DepartureStop { get; set; }
    }

    public class StopModel
    {
        public string Name { get; set; }
        public LocationModel Location { get; set; }
    }

    public class LocationModel
    {
        public LatLngModel LatLng { get; set; }
    }

    public class LatLngModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class LocalizedValuesModel
    {
        public TimeInfoModel ArrivalTime { get; set; }
        public TimeInfoModel DepartureTime { get; set; }
    }

    public class TimeInfoModel
    {
        public TimeModel Time { get; set; }
        public string TimeZone { get; set; }
    }

    public class TimeModel
    {
        public string Text { get; set; }
        public string LanguageCode { get; set; }
    }

    public class TransitLineModel
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public string Color { get; set; }
        public string NameShort { get; set; }
        public VehicleModel Vehicle { get; set; }
    }

    public class VehicleModel
    {
        public TextModel Name { get; set; }
        public int Type { get; set; }
        public string IconUri { get; set; }
    }

    public class TextModel
    {
        public string Text { get; set; }
        public string LanguageCode { get; set; }
    }

}