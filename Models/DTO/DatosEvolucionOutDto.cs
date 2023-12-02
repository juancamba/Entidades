using Newtonsoft.Json;

namespace Entidades.Models.DTO
{
    public class DatosEvolucionOutDto
    {


        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; } = new();
        
        public Dictionary<string, object> ValoresReferencia = new();
        public InformacionFechas InformacionFechas { get; set; }
    }
    public class InformacionFechas
    {

        public string FechaMinima { get; set; }

        public string FechaMaxima { get; set; }
        public int Distancia { get; set; }
        public List<string> FechasCompletas { get; set; }
    }
}
