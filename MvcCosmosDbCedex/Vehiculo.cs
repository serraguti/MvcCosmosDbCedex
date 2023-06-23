using MvcCosmosDbCedex.Models;
using Newtonsoft.Json;

namespace MvcCosmosDbCedex
{
    public class Vehiculo
    {
        //DENTRO DE COSMOS, DEBEMOS INDICAR QUE PROPIEDAD
        //SERA EL ID Y SE REALIZA PONIENDO ID COMO NOMBRE DE
        //PROPERTY
        [JsonProperty(PropertyName ="id")]
        public string IdVehiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Imagen { get; set; }
        public Motor Motor { get; set; }
    }
}
