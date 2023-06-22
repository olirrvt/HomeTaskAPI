using HomeTaskerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeTaskerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {

        [HttpGet]
        [Route("Reservas")]
        public async Task<IActionResult> getAllAsyncReservas(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext)
        {
            var reservas = await homeTaskerDbContext
                .Reservas
                .AsNoTracking()
                .ToListAsync();

            return reservas == null ? NotFound() : Ok(reservas);
        }


        [HttpPost]
        [Route("Reserva/{id}")]
        public async Task<IActionResult> registReservaAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Reserva reserva,
            [FromRoute] int id )
        {
            var morador = await homeTaskerDbContext
                .Moradores.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (morador == null)
            {
                return NotFound();
            }

            reserva.Morador = morador;

            try
            {
                homeTaskerDbContext.Set<Reserva>().Add(reserva);
                homeTaskerDbContext.Entry(reserva.Morador).State = EntityState.Unchanged;
                await homeTaskerDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"Reserva/Reserva/{morador.Id}", reserva);
        }
    }
}
