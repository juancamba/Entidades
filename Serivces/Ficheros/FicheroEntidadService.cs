using Entidades.Models.DTO;
using Entidades.Services.Ficheros;

namespace Entidades.Serivces.Ficheros
{
    public class FicheroEntidadService : AbstractFicheroService, IFicheroEntidadService
    {
        public FicheroEntidadService(IConfiguration configuration) : base(configuration)
        {
        }

        public ConjuntoEntidad Cargar(IFormFile formFile)
        {
            ConjuntoEntidad conjuntoEntidad = new ConjuntoEntidad();
            conjuntoEntidad.Entidades = new List<EntidadDto>();
            conjuntoEntidad.NombresDatosEstaticos = new List<string>();
            List<string> nombresDatosEstaticos = new List<string>();

            var reader = new StreamReader(formFile.OpenReadStream());
            int nlinea = 1;

            while (!reader.EndOfStream)
            {
                string linea = reader.ReadLine();
                var values = linea.Split(SEPARADOR_CSV);
                if (nlinea == 1)
                {
                    //obtenemos los nombre de las variables
                    //nombresVariables = ObtenerNombresVariables(linea);
                    //asigno al conjunto muestra
                    //conjuntoMuestra.NombresVariablesMuestras = nombresVariables;
                    nombresDatosEstaticos = ObtenerNombresVariables(linea, CABECERA_ESTATICA_ENTIDADES);
                    conjuntoEntidad.NombresDatosEstaticos = nombresDatosEstaticos;
                }
                else
                {
                    EntidadDto entidad = ObtenerValoresVariablesEntidad(linea);
                    conjuntoEntidad.Entidades.Add(entidad);

                }



                nlinea++;
            }
            return conjuntoEntidad;
        }
        private EntidadDto ObtenerValoresVariablesEntidad(string linea)
        {
            string[] lineaSplitted = linea.Split(SEPARADOR_CSV);
            EntidadDto entidad = new EntidadDto();
            //esto va a generar el array con los datos variables
            List<string> valoresDatosEstaticos = new List<string>();
            for (int i = 1; i < lineaSplitted.Count(); i++)
            {
                valoresDatosEstaticos.Add(lineaSplitted[i]);
            }
            entidad.IdEntidad = lineaSplitted[0];

            entidad.ValoresDatosEstaticos = valoresDatosEstaticos;

            /*
             idEntidad;fechaNacimiento;movil;nombres;apellidos
            32715858a;20020329;666666666;alberto;morales
            */
            return entidad;
        }
    }
}
