using MapCoordinatesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapCoordinatesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoordinatesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCoordinates()
        {
            var coordinates = new[]
            {
                new { lat = -23.561684, lng = -46.655981, label = "Av. Paulista" },
                new { lat = -23.55052, lng = -46.633308, label = "Praça da Sé" }
            };

            return Ok(coordinates);
        }

        private readonly ExcelService _excelService;

        public CoordinatesController()
        {
            _excelService = new ExcelService();
        }

        [HttpGet("from-excel")]
        public async Task<IActionResult> GetCoordinatesFromExcel()
        {
            var filePath = "C:\\Temp\\Endereco\\Imoveis_INSS.xlsx";
            var enderecos = _excelService.LerEnderecosDoExcel(filePath);

            var coordenadas = new List<object>();

            foreach (var endereco in enderecos)
            {
                var resultado = await _excelService.ObterCoordenadas(endereco);
                if (resultado != null)
                {
                    coordenadas.Add(new
                    {
                        Endereco = endereco,
                        Latitude = resultado.Value.Latitude,
                        Longitude = resultado.Value.Longitude
                    });
                }
            }

            return Ok(coordenadas);
        }
    }
}
