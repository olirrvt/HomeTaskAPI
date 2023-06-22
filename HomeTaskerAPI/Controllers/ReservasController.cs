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

        [HttpGet]
        [Route("Reserva/{id}")]
        public async Task<IActionResult> getByIdAsyncReserva(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var reserva = await homeTaskerDbContext
                .Contas
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            return reserva == null ? NotFound() : Ok(reserva); 
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


        [HttpDelete]
        [Route("Reserva/{id}/Apagar")]
        public async Task<IActionResult> deleteReservaAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var resv = await homeTaskerDbContext
                .Reservas
                .FirstOrDefaultAsync(re => re.Id == id);

            if (resv == null)
            {
                return BadRequest("Reserva não encontrada!");
            }

            try
            {
                homeTaskerDbContext.Reservas.Remove(resv);
                await homeTaskerDbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("Reserva/{id}/Editar")]
        public async Task<IActionResult> PutAsyncReserva(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Reserva reserva,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model inválida");
            }

            var reserv = await homeTaskerDbContext
                .Reservas
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserv == null)
            {
                return NotFound("Reserva não encontrada");
            }

            try
            {
                reserv.DataHoraReserva = reserva.DataHoraReserva;
                reserv.EspacoComum = reserva.EspacoComum;

                homeTaskerDbContext.Reservas.Update(reserv);
                await homeTaskerDbContext.SaveChangesAsync();
                return Ok(reserv);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }
    }
}
