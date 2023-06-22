using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeTaskerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MoradorController : ControllerBase
    {
        [HttpGet]
        [Route("Moradores")]
        public async Task<IActionResult> getAllAsync(
            // Usando injeção de dependencia
            [FromServices] HomeTaskerDbContext homeTaskerDbContext) 
        {
            var moradores = await homeTaskerDbContext
                .Moradores
                .AsNoTracking() // Objetos não precisam ser rastreados para a atualização
                .ToListAsync(); // Recebe uma lista de moradores de forma assícrona

            return moradores == null ? NotFound() : Ok(moradores); // Retorno do método
        }
    }
}
