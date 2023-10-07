namespace Entidades.Models.DTO
{
    public class MuestrasYValoresDto
    {
        public IEnumerable<Muestra> Muestras { get; set; }

        public int NumVariables { get; set; } = 0;

        public IEnumerable<string> NombresVariables { get; set; }


    }
}
