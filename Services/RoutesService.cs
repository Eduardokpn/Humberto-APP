using Google.Api.Gax.Grpc;
using Google.Apis.Auth.OAuth2;
using Google.Maps.Routing.V2;
using Google.Type;
using HumbertoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumbertoMVC.Services
{
    public class RoutesService : Controller
    {

        public RoutesService() 
        {
         


        }
        [HttpPost("Rotas")]
        public async Task<OnibusRotaModel> Rotas(LocationModel.Coordenadas origemCord , LocationModel.Coordenadas destCord)
        {
            var GOOGLE_APPLICATION_CREDENTIALS = "C:/Users/eduar/Documents/ETEC/TESTE API GOOGLE/TESTE API GOOGLE/Controllers/humberto-transit-api-38ef852b5f99.json";

            HttpClient httpClient = new HttpClient();
            GoogleCredential credential = GoogleCredential.GetApplicationDefault();
            RoutesClient client = RoutesClient.Create();
            CallSettings callSettings = CallSettings.FromHeader("X-Goog-FieldMask","routes.legs.steps.transitDetails,routes.duration,routes.polyline.encodedPolyline");

            ComputeRoutesRequest request = new ComputeRoutesRequest
            {
                Origin = new Waypoint
                {
                    Location = new Location { LatLng = new LatLng { Latitude = origemCord.Latitude, Longitude = origemCord.Longitude} } //TODO: Current location

                },
                Destination = new Waypoint
                {
                    Location = new Location { LatLng = new LatLng { Latitude = destCord.Latitude, Longitude = destCord.Longitude} }
                },
                TravelMode = RouteTravelMode.Transit,
                TransitPreferences = new TransitPreferences
                {
                    AllowedTravelModes = { TransitPreferences.Types.TransitTravelMode.Bus },

                },
                //TransitPreferences = TransitPreferences.Types.TransitTravelMode.Bus,
                RoutingPreference = RoutingPreference.Unspecified,
                ComputeAlternativeRoutes = false,

            };

            ComputeRoutesResponse response = client.ComputeRoutes(request, callSettings);

            
           // var content = new StringContent(JsonConvert.SerializeObject(request));
           var content = JsonConvert.SerializeObject(response);
           
           // Deserializando para OnibusRotaModel
           var responseDeserialized = JsonConvert.DeserializeObject<OnibusRotaModel>(content);
            
            // Retornando o objeto deserializado
            return responseDeserialized;
        }
        



        public IActionResult Index()
        {
            return View();
        }
    }
}



//TODO: Verificar se currentLocation atualiza em tempo real no celular.
//TODO: DESTINO EM ENDEREÇO;