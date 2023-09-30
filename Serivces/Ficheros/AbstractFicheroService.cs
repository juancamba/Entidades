namespace Entidades.Services.Ficheros
{
    public abstract class AbstractFicheroService
    {




        public readonly string? SEPARADOR_CSV;
        public readonly string? CABECERA_ESTATICA_MUESTRAS;
        public readonly string? CABECERA_ESTATICA_ENTIDADES;
        //inica en que posición empiezan los nombres de las variables de datos variables en el archivo de muestras
        public readonly int INICIO_DATOS_VARIABLES_MUESTRAS;

        protected AbstractFicheroService(IConfiguration configuration)
        {
            SEPARADOR_CSV = configuration.GetSection("General")["separadorCsv"];
            CABECERA_ESTATICA_MUESTRAS = configuration.GetSection("General")["cabeceraEstaticaMuestras"]; ;
            CABECERA_ESTATICA_ENTIDADES = configuration.GetSection("General")["cabeceraEstaticaEntidades"]; ;
            INICIO_DATOS_VARIABLES_MUESTRAS = Convert.ToInt32(configuration.GetSection("General")["inicioDatosVariablesMuestras"]);
        }




        protected List<string> ObtenerNombresVariables(string cabecera, string parteEstatica)
        {
            // string cabeceraEstatica = "idEntidad;idCampo;idTipoMuestra;fechaMuestra;";
            //elimino la parte estatica y dejo solo los nombres de las varibles en formato array
            string[] nombres = cabecera.Replace(parteEstatica, "").Split(SEPARADOR_CSV);
            List<string> listaNombres = new List<string>(nombres);
            return listaNombres;

        }
    }
}
