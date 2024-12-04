using Newtonsoft.Json;

namespace HumbertoMVC.Models;

public class OnibusRotaModel
{
    public List<RouteModel> Routes { get; set; }

    public class RouteModel
    {
        public int Cr { get; set; }
        public List<LegModel> Legs { get; set; }
        public double DistanceMeters { get; set; }
        public DurationModel Duration { get; set; }
        public DurationModel StaticDuration { get; set; }
        public PolylineModel Polyline { get; set; }
    }

    public class LegModel
    {
        public double DistanceMeters { get; set; }
        public DurationModel Duration { get; set; }
        public DurationModel StaticDuration { get; set; }
        public PolylineModel Polyline { get; set; }
        public LocationModel StartLocation { get; set; }
        public LocationModel EndLocation { get; set; }
        public List<StepModel> Steps { get; set; }
        public TravelAdvisoryModel TravelAdvisory { get; set; }
        public LocalizedValuesModel LocalizedValues { get; set; }
    }

    public class StepModel
    {
        public double DistanceMeters { get; set; }
        public DurationModel StaticDuration { get; set; }
        public PolylineModel Polyline { get; set; }
        public LocationModel StartLocation { get; set; }
        public LocationModel EndLocation { get; set; }
        public NavigationInstructionModel NavigationInstruction { get; set; }
        public TravelAdvisoryModel TravelAdvisory { get; set; }
        public LocalizedValuesModel LocalizedValues { get; set; }
        public TransitDetailsModel TransitDetails { get; set; }
        public int TravelMode { get; set; }
    }

    public class NavigationInstructionModel
    {
        public maneuverEnum Maneuver { get; set; }
        public string Instructions { get; set; }
    }

    public enum maneuverEnum
    {
        ManeuverUnspecified,         // Não utilizado.
        TurnSlightLeft,              // Vire um pouco para a esquerda.
        TurnSharpLeft,               // Vire bruscamente para a esquerda.
        UTurnLeft,                   // Faça um retorno à esquerda.
        TurnLeft,                    // Vire à esquerda.
        TurnSlightRight,             // Vire um pouco para a direita.
        TurnSharpRight,              // Vire bruscamente para a direita.
        UTurnRight,                  // Faça um retorno à direita.
        TurnRight,                   // Vire à direita.
        Straight,                    // Siga em frente.
        RampLeft,                    // Pegue a rampa à esquerda.
        RampRight,                   // Pegue a rampa à direita.
        Merge,                       // Entrar no tráfego.
        ForkLeft,                    // Pegue a bifurcação à esquerda.
        ForkRight,                   // Pegue a bifurcação à direita.
        Ferry,                       // Pegue a balsa.
        FerryTrain,                  // Pegue o trem que leva à balsa.
        RoundaboutLeft,              // Vire à esquerda na rotatória.
        RoundaboutRight,             // Vire à direita na rotatória.
        Depart,                      // Manobra inicial.
        NameChange                   // Usado para indicar uma mudança no nome de uma rua.
    }
    
    public class DurationModel
    {
        public int Seconds { get; set; }
        public int Nanos { get; set; }
    }

    public class PolylineModel
    {
        public string EncodedPolyline { get; set; }
        public bool HasEncodedPolyline { get; set; }
    }

    public class LocationModel
    {
        public LatLngModel LatLng { get; set; }
    }

    public class LatLngModel
    {
	[JsonProperty("Latitude")]
        public double Lat{ get; set; }

	[JsonProperty("Longitude")] 
        public double Lng{ get; set; }
    }

    public class LocalizedValuesModel
    {
        public DistanceModel Distance { get; set; }
        public TimeModel StaticDuration { get; set; }
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

    public class DistanceModel
    {
        public string Text { get; set; }
        public string LanguageCode { get; set; }
    }

    public class TravelAdvisoryModel
    {
        // Adicionar propriedades caso necessário
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
