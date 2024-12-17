using OfficeOpenXml;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MapCoordinatesAPI.Services
{
    public class ExcelService
    {
        private const string GoogleApiKey = "COLOQUE_SUA_CHAVE_AQUI";
        private readonly HttpClient _httpClient;

        public ExcelService()
        {
            _httpClient = new HttpClient();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Licença EPPlus
        }

        public List<string> LerEnderecosDoExcel(string filePath)
        {
            var enderecos = new List<string>();

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];

            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                var enderecoBase = worksheet.Cells[row, 1].Text;    // ENDEREÇO
                var bairro = worksheet.Cells[row, 3].Text;          // BAIRRO
                var municipio = worksheet.Cells[row, 4].Text;       // MUNICÍPIO
                var uf = worksheet.Cells[row, 5].Text;              // UF
                var pais = worksheet.Cells[row, 6].Text;            // PAIS

                // Montar o endereço completo
                var enderecoCompleto = $"{enderecoBase}, {bairro}, {municipio}, {uf}, {pais}";
                enderecos.Add(enderecoCompleto);
            }

            // Removendo tudo a partir de "APTO" e filtrando endereços únicos
            var distinctEnderecos = enderecos
                .Select(e => Regex.Replace(e, @"\s*APTO.*", "").Trim()) // Remove "APTO" e tudo que vem depois
                .Distinct() // Filtra valores únicos
                .ToList();

            
            return distinctEnderecos;
        }


        public async Task<(double Latitude, double Longitude)?> ObterCoordenadas(string endereco)
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(endereco)}&key={GoogleApiKey}";

            var response = await _httpClient.GetStringAsync(url);
            var json = JsonDocument.Parse(response);

            var results = json.RootElement.GetProperty("results");
            if (results.GetArrayLength() > 0)
            {
                var location = results[0].GetProperty("geometry").GetProperty("location");
                var lat = location.GetProperty("lat").GetDouble();
                var lng = location.GetProperty("lng").GetDouble();
                return (lat, lng);
            }

            return null;
        }
    }
}
