using HomeTaskerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HomeTaskerAPI.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> getByIdAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id)
        {
            var morador = await homeTaskerDbContext
                .Moradores
                .Include(m => m.Conta).Include(r => r.Reservas).Include(oc => oc.Ocorrencia)
                .Include(s => s.Servicos).Include(p => p.Produtos).Include(v => v.Visitantes)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);  // Pega o Primeiro registro que tenha o id solicitado

            return morador == null ? NotFound() : Ok(morador);
        }

        [HttpPost]
        [Route("Morador/Create")]
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
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding
                        .UTF8.GetBytes(moradore.Senha));
                    var hashedPassword = BitConverter
                        .ToString(hashedBytes).Replace("-", "").ToLower();
                    moradore.Senha = hashedPassword;
                }

                await homeTaskerDbContext.Moradores.AddAsync(moradore);
                await homeTaskerDbContext.SaveChangesAsync();
                return Created($"Morador/Morador/{moradore.Id}", moradore);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("Morador/Login")]
        public async Task<IActionResult> LoginAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                // Verificar o login e autenticar o usuário
                var morador = await homeTaskerDbContext.Moradores
                    .FirstOrDefaultAsync(m => m.Email == loginModel.Email);

                if (morador == null)
                {
                    return Unauthorized("Credenciais inválidas");
                }

                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(loginModel.Senha));
                    var hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                    if (hashedPassword != morador.Senha)
                    {
                        return Unauthorized("Credenciais inválidas");
                    }
                }

                return Ok(morador);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut]
        [Route("{id}")]
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
                m.Apartamento = moradore.Apartamento;
                m.Senha = moradore.Senha;
                m.Email = moradore.Email;

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
        [Route("{id}")]
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
