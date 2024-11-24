namespace HumbertoMVC.Models;

public class Adress
{
    public class GeoResponse
    {
        public List<Result> Results { get; set; }
        public string Status { get; set; }
    }

    public class Result
    {
        public List<AddressComponent> AddressComponents { get; set; }
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
        public string PlaceId { get; set; }
        public PlusCode PlusCode { get; set; }
        public List<string> Types { get; set; }
    }

    public class AddressComponent
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public List<string> Types { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
        public string LocationType { get; set; }
        public Viewport Viewport { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Viewport
    {
        public Coordinates Northeast { get; set; }
        public Coordinates Southwest { get; set; }
    }

    public class Coordinates
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class PlusCode
    {
        public string CompoundCode { get; set; }
        public string GlobalCode { get; set; }
    }
}