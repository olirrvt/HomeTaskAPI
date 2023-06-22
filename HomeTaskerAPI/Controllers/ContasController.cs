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

            return Created($"Contas/ContaApp/{conta.Id}", conta);
        }
    }
}
