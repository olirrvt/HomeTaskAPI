using HomeTaskerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeTaskerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicosController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("Servicos")]
        public async Task<IActionResult> getAllAsyncServices(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext)
        {
            var servicos = await homeTaskerDbContext
                .Servicos
                .AsNoTracking()
                .ToListAsync();

            return servicos == null ? NotFound() : Ok(servicos);
        }

        [Authorize]
        [HttpGet]
        [Route("Servico/{id}")]
        public async Task<IActionResult> getByIdAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id)
        {
            var servico = await homeTaskerDbContext
                .Servicos
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return servico == null ? NotFound() : Ok(servico);
        }

        [Authorize]
        [HttpPost]
        [Route("Servico/{id}")]
        public async Task<IActionResult> registerServicesAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Servico servico,
            [FromRoute] int id )
        {
            var morador = await homeTaskerDbContext
            .Moradores.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

            if (morador == null)
            {
                return NotFound();
            }

            servico.Morador = morador;

            try
            {
                homeTaskerDbContext.Set<Servico>().Add(servico);
                homeTaskerDbContext.Entry(servico.Morador).State = EntityState.Unchanged;
                await homeTaskerDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"Servicos/Servico/{morador.Id}", servico);
        }

        [Authorize]
        [HttpPut]
        [Route("Servico/{id}/Editar")]
        public async Task<IActionResult> PutAsyncServices(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Servico servico,
            [FromRoute] int id )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model inválida");
            }

            var serv = await homeTaskerDbContext
                .Servicos
                .FirstOrDefaultAsync(s => s.Id == id);

            if (serv == null)
            {
                NotFound("Serviço não encontrado!");
            }

            try
            {
                serv.Descricao = servico.Descricao;
                serv.TipoServico = servico.TipoServico;

                homeTaskerDbContext.Servicos.Update(serv);
                await homeTaskerDbContext.SaveChangesAsync();
                return Ok(serv);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Servico/{id}/StatusDoServico")]
        public async Task<IActionResult> StatusPutAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var serv = await homeTaskerDbContext
                .Servicos
                .FirstOrDefaultAsync(s => s.Id == id);

            if (serv == null )
            {
                NotFound("Serviço não encontrado!");
            }

            try
            {
                serv.StatusDoServico = "Concluído";

                await homeTaskerDbContext.SaveChangesAsync(); 
                return Ok(serv);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }   

        }

        [Authorize]
        [HttpDelete]
        [Route("Servico/{id}/Apagar")]
        public async Task<IActionResult> deleteServiceAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var service = await homeTaskerDbContext
                .Servicos
                .FirstOrDefaultAsync(se => se.Id == id);

            if (service == null)
            {
                return NotFound("Serviço não encontrado!");
            }

            try
            {
                homeTaskerDbContext.Servicos.Remove(service);
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
