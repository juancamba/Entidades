using Newtonsoft.Json;

namespace Entidades.Models.DTO
{
    public class DatosEvolucionOutDto
    {
        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; }
        [JsonProperty("valoresReferencia")]
        public Dictionary<string, object> ValoresReferencia;
    }
}
