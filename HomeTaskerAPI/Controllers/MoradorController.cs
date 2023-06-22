using HomeTaskerAPI.Models;
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
                .Include(m => m.Conta)
                .AsNoTracking() // Objetos não precisam ser rastreados para a atualização
                .ToListAsync(); // Recebe uma lista de moradores de forma assícrona

            return moradores == null ? NotFound() : Ok(moradores); // Retorno do método
        }

        [HttpGet]
        [Route("Morador/{id}")]
        public async Task<IActionResult> getByIdAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id)
        {
            var morador = await homeTaskerDbContext
                .Moradores
                .Include(m => m.Conta)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);  // Pega o Primeiro registro que tenha o id solicitado

            return morador == null ? NotFound() : Ok(morador);
        }

        [HttpPost]
        [Route("Morador")]
        public async Task<IActionResult> PostAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Moradore moradore
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await homeTaskerDbContext.Moradores.AddAsync(moradore);
                await homeTaskerDbContext.SaveChangesAsync();
                return Created($"Morador/Morador/{moradore.Id}", moradore);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Morador/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Moradore moradore,
            [FromRoute] int id )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model inválida");
            }

            var m = await homeTaskerDbContext.Moradores
                .FirstOrDefaultAsync(m => m.Id == id);

            if (m == null)
            {
                return NotFound("Morador não encontrado!");
            }

            try
            {
                m.Nome = moradore.Nome;

                homeTaskerDbContext.Moradores.Update(m);
                await homeTaskerDbContext.SaveChangesAsync();
                return Ok(m);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Morador/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id)
        {
            var mor = await homeTaskerDbContext.Moradores
                .FirstOrDefaultAsync(mor => mor.Id == id);

            if (mor == null)
            {
                return BadRequest("Morador não encontrado!");
            }

            try
            {
                homeTaskerDbContext.Moradores.Remove(mor);
                await homeTaskerDbContext.SaveChangesAsync();

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
