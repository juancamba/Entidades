using Entidades.Models;
using Entidades.Models.DTO;
using System.IO;


namespace Entidades.Services.Ficheros
{
    public class FicheroMuestraService : AbstractFicheroService, IFicheroMuestraService
    {


        public FicheroMuestraService(IConfiguration configuration) : base(configuration)
        {

        }

        public ConjuntoMuestra Cargar(IFormFile file)
        {



            if (file == null || file.Length == 0)
            {

                throw new InvalidDataException("Debe enviar un archivo");

            }

            ConjuntoMuestra conjuntoMuestra;

            conjuntoMuestra = new ConjuntoMuestra();
            conjuntoMuestra.Muestras = new List<MuestraDto>();
            List<string> nombresVariables = new List<string>();
            // tiene que existr idcampo, idTipoMuestra

            var reader = new StreamReader(file.OpenReadStream());
            int nlinea = 1;

            string linea;
            while ((linea = reader.ReadLine()) != null)
            //while (!reader.EndOfStream)
            {
                //string linea = reader.ReadLine();
                var values = linea.Split(SEPARADOR_CSV);
                if (nlinea == 1)
                {
                    //obtenemos los nombre de las variables
                    nombresVariables = ObtenerNombresVariables(linea, CABECERA_ESTATICA_MUESTRAS);
                    //asigno al conjunto muestra
                    //conjuntoMuestra.NombresVariablesMuestras = nombresVariables;
                }
                else
                {
                    MuestraDto muestra = ObtenerValoresVariablesMuestra(linea);
                    conjuntoMuestra.Muestras.Add(muestra);
                    NombreVariableTipoMuestra nombreVariable = new NombreVariableTipoMuestra();
                    nombreVariable.IdTipoMuestra = ObtenerIdTipoMuestra(linea);
                    nombreVariable.Nombres = nombresVariables;
                    conjuntoMuestra.NombreVariable = nombreVariable;
                }

                nlinea++;
            }




            return conjuntoMuestra;


        }
        private EntidadDto ObtenerValoresVariablesEntidad(string linea)
        {

            EntidadDto entidad = new EntidadDto();

            return entidad;
        }
        private string ObtenerIdTipoMuestra(string linea)
        {
            return linea.Split(SEPARADOR_CSV)[2];
        }

        private MuestraDto ObtenerValoresVariablesMuestra(string linea)
        {
            string[] nombres = linea.Split(SEPARADOR_CSV);
            MuestraDto muestra = new MuestraDto();
            //esto va a generar el array con los datos variables
            List<string> valoresVariablesMuestras = new List<string>();
            for (int i = INICIO_DATOS_VARIABLES_MUESTRAS; i < nombres.Count(); i++)
            {
                valoresVariablesMuestras.Add(nombres[i]);
            }
            muestra.IdEntidad = nombres[0];
            muestra.IdCampo = nombres[1];
            muestra.IdTipoMuestra = nombres[2];
            muestra.FechaMuestra = nombres[3];


            muestra.ValoresVariablesMuestras = valoresVariablesMuestras;

            /*
             * idEntidad;idCampo;idTipoMuestra;fechaMuestra;tv;irv;erv;rv
32715858a;1;1;20220328;500;2500;1500;1500
            */
            return muestra;
        }


    }
}
