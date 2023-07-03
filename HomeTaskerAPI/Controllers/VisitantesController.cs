using HomeTaskerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeTaskerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitantesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("Visitantes")]
        public async Task<IActionResult> getAllAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext)
        {
            var visitantes = await homeTaskerDbContext
                .Visitantes
                .AsNoTracking()
                .ToListAsync();

            return visitantes == null ? NotFound() : Ok(visitantes);
        }

        [Authorize]
        [HttpGet]
        [Route("Visitante/{id}")]
        public async Task<IActionResult> getByIdAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var visitantes = await homeTaskerDbContext
                .Visitantes
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);

            return visitantes == null ? NotFound() : Ok(visitantes);
        }

        [Authorize]
        [HttpGet]
        [Route("Morador/{idMorador}/Visitantes")]
        public async Task<IActionResult> getVisitantesPorMoradorAsync(
        [FromServices] HomeTaskerDbContext homeTaskerDbContext,
        [FromRoute] int idMorador)
        {
            var visitantes = await homeTaskerDbContext
                .Visitantes
                .AsNoTracking()
                .Where(v => v.MoradorId == idMorador)
                .ToListAsync();

            return visitantes == null ? NotFound() : Ok(visitantes);
        }

        [Authorize]
        [HttpPost]
        [Route("Visitantes/{id}/Cadastrar")]
        public async Task<IActionResult> registerVisitAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Visitante visitante,
            [FromRoute] int id )
        {
            var morador = await homeTaskerDbContext
            .Moradores.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

            if (morador == null)
            {
                return NotFound();
            }

            visitante.Morador = morador;

            try
            {
                homeTaskerDbContext.Set<Visitante>().Add(visitante);
                homeTaskerDbContext.Entry(visitante.Morador).State = EntityState.Unchanged;
                await homeTaskerDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"Visitantes/Visitantes/{morador.Id}", visitante);
        }

        [Authorize]
        [HttpPut]
        [Route("Visitantes/{id}/Editar")]
        public async Task<IActionResult> putAsyncVisitantes(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Visitante visitante,
            [FromRoute] int id )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model inválida");
            }

            var visit = await homeTaskerDbContext
                .Visitantes
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visit == null)
            {
                return NotFound("Visitante não encontrado!");
            }

            try
            {
                visit.NomeVisitante = visitante.NomeVisitante;
                visit.DataHoraSaida = visitante.DataHoraSaida;

                homeTaskerDbContext.Visitantes.Update(visit);
                await homeTaskerDbContext.SaveChangesAsync();
                return Ok(visit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("Visitante/{id}/Apagar")]
        public async Task<IActionResult> deleteVisitanteAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var visita = await homeTaskerDbContext
                .Visitantes
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visita == null)
            {
                return NotFound();
            }

            try
            {
                homeTaskerDbContext.Visitantes.Remove(visita);
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
