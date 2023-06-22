using HomeTaskerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HomeTaskerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContasController : ControllerBase
    {

        [HttpGet]
        [Route("Contas")]
        public async Task<IActionResult> getAllAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext)
        {
            var contas = await homeTaskerDbContext
                .Contas
                .AsNoTracking()
                .ToListAsync();

            return contas == null ? NotFound() : Ok(contas);
        }

        [HttpGet]
        [Route("Conta/{id}")]
        public async Task<IActionResult> getByIdAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id)
        {
            var conta = await homeTaskerDbContext
                .Contas
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return conta == null ? NotFound() : Ok(conta);
        }

        [HttpPost]
        [Route("ContaApp/{id}")]
        public async Task<IActionResult> regContasAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Conta conta,
            [FromRoute] int id )
        {
            var morador = await homeTaskerDbContext
                .Moradores.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if ( morador == null )
            {
                return NotFound();
            }

            conta.Morador = morador;

            try
            {
                homeTaskerDbContext.Set<Conta>().Add(conta);
                homeTaskerDbContext.Entry(conta.Morador).State = EntityState.Unchanged;
                await homeTaskerDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"Contas/ContaApp/{morador.Id}", conta);
        }

        [HttpPut]
        [Route("ContaApp/{id}/Pagar")]
        public async Task<IActionResult> ContaPagaAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id)
        {
            var conta = await homeTaskerDbContext
                .Contas
                .FirstOrDefaultAsync(c => c.Id == id);

            if (conta == null)
            {
                return NotFound("Pessoa não encontrada!");
            }

            conta.Status = "Pago";

            try
            {
                await homeTaskerDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("ContaApp/{id}/Apagar")]
        public async Task<IActionResult> deleteContaAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var cont = await homeTaskerDbContext
                .Contas
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cont == null)
            {
                return BadRequest("Conta não encontrada!");
            }

            try
            {
                homeTaskerDbContext.Contas.Remove(cont);
                await homeTaskerDbContext.SaveChangesAsync();

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("ContaApp/{id}/Editar")]
        public async Task<IActionResult> PutAsyncContas(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Conta conta,
            [FromRoute] int id )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model inválida");
            }

            var co = await homeTaskerDbContext
                .Contas
                .FirstOrDefaultAsync(c => c.Id == id);

            if (co == null)
            {
                return NotFound("Conta não encontrada!");
            }

            try
            {
                co.Valor = conta.Valor;
                co.DataVencimento = conta.DataVencimento;

                homeTaskerDbContext.Contas.Update(co);
                await homeTaskerDbContext.SaveChangesAsync();
                return Ok(co);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
