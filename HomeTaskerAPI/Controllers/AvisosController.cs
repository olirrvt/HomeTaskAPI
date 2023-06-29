using HomeTaskerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeTaskerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AvisosController : ControllerBase
    {
        [HttpGet]
        [Route("Avisos")]
        public async Task<IActionResult> getAllAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext)
        {
            var avisos = await homeTaskerDbContext
                .Avisos
                .AsNoTracking()
                .ToListAsync();

            return avisos == null ? NotFound() : Ok(avisos);
        }

        [HttpGet]
        [Route("Aviso/{id}")]
        public async Task<IActionResult> getByIdAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var aviso = await homeTaskerDbContext
                .Avisos
                .AsNoTracking()
                .FirstOrDefaultAsync(av => av.Id == id);

            return aviso == null ? NotFound() : Ok(aviso);
        }

        [HttpPost]
        [Route("Aviso")]
        public async Task<IActionResult> PostAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Aviso aviso )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await homeTaskerDbContext.Avisos.AddAsync(aviso);
                await homeTaskerDbContext.SaveChangesAsync();
                return Created($"Avisos/Aviso/{aviso.Id}", aviso);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Aviso/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Aviso aviso,
            [FromRoute] int id )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var av = await homeTaskerDbContext.Avisos
                .FirstOrDefaultAsync(a => a.Id == id);

            if (av == null)
            {
                NotFound("Aviso não encontrado!");
            }

            try
            {
                av.Titulo = aviso.Titulo;
                av.Mensagem = aviso.Mensagem;

                homeTaskerDbContext.Avisos.Update(av);
                await homeTaskerDbContext.SaveChangesAsync();

                return Ok(av);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Aviso/{id}")]
        public async Task<IActionResult> DeleteAsyncAvisos(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var avs = await homeTaskerDbContext.Avisos
                .FirstOrDefaultAsync(a => a.Id == id);

            if (avs == null)
            {
                return NotFound();
            }

            try
            {
                homeTaskerDbContext.Avisos.Remove(avs);
                await homeTaskerDbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
