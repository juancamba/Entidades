using CsvHelper.Configuration;
using CsvHelper;
using Entidades.Models.DTO;
using System.Globalization;

namespace Entidades.Serivces.Ficheros
{
    public class FicheroValoresReferenciaService : IFicheroValoresReferenciaService
    {



        public List<FicheroValorReferenciaDto> Cargar(IFormFile file)
        {
            List<FicheroValorReferenciaDto> registros;

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";" // Especificar el separador utilizado en tu archivo CSV
            }))
            {

                registros = csv.GetRecords<FicheroValorReferenciaDto>().ToList();

            }
            return registros;

        }
    }
}