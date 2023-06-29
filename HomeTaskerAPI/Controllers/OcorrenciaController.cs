using HomeTaskerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeTaskerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OcorrenciaController : ControllerBase
    {
        [HttpGet]
        [Route("Ocorrencias")]
        public async Task<IActionResult> getAllAsyncOcorrencias(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext)
        {
            var ocorrencias = await homeTaskerDbContext
                .Ocorrencias
                .AsNoTracking()
                .ToListAsync();

            return ocorrencias == null ? NotFound() : Ok(ocorrencias);
        }

        [HttpGet]
        [Route("Ocorrencia/{id}")]
        public async Task<IActionResult> getByIdAsyncOcorrencia(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id)
        {
            var ocorrencia = await homeTaskerDbContext
                .Ocorrencias
                .AsNoTracking()
                .FirstOrDefaultAsync(oc => oc.Id == id);

            return ocorrencia == null ? NotFound() : Ok(ocorrencia);
        }

        [HttpPost]
        [Route("Ocorrencia/{id}")]
        public async Task<IActionResult> registerOcorrenciaAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Ocorrencia ocorrencia,
            [FromRoute] int id)
        {
            var morador = await homeTaskerDbContext
                .Moradores.AsNoTracking()
                .FirstOrDefaultAsync(mo => mo.Id == id);

            if (morador == null)
            {
                return NotFound("Morador não encontrado");
            }

            ocorrencia.Morador = morador;

            try
            {
                homeTaskerDbContext.Set<Ocorrencia>().Add(ocorrencia);
                homeTaskerDbContext.Entry(ocorrencia.Morador).State = EntityState.Unchanged;
                await homeTaskerDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"Ocorrencia/Ocorrencia/{morador.Id}", ocorrencia);
        }

        [HttpPut]
        [Route("Ocorrencia/{id}/Editar")]
        public async Task<IActionResult> PutAsyncOcorrencias(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Ocorrencia ocorrencia,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Inválida");
            }

            var ocorr = await homeTaskerDbContext
                .Ocorrencias
                .FirstOrDefaultAsync(ocr => ocr.Id == id);

            if (ocorrencia == null)
            {
                return NotFound("Ocorrencia não encontrada!");
            }

            try
            {
                ocorr.Descricao = ocorrencia.Descricao;

                homeTaskerDbContext.Ocorrencias.Update(ocorr);
                await homeTaskerDbContext.SaveChangesAsync();
                return Ok(ocorr);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Ocorrencia/{id}/Apagar")]
        public async Task<IActionResult> deleteOcorrenciaAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id)
        {
            var ocorren = await homeTaskerDbContext
                .Ocorrencias
                .FirstOrDefaultAsync(ocr => ocr.Id == id);

            if (ocorren == null)
            {
                return NotFound("Ocorrencia não encontrada!");
            }

            try
            {
                homeTaskerDbContext.Ocorrencias.Remove(ocorren);
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
