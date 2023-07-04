using HomeTaskerAPI.Models;
using HomeTaskerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HomeTaskerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpGet]
        [Route("VerificarToken")]
        [Authorize]
        public IActionResult VerificarToken()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("MoradorLogado")]
        [Authorize]
        public async Task<IActionResult> GetMoradorLogadoAsync(
        [FromServices] HomeTaskerDbContext homeTaskerDbContext)
        {
            try
            {
                var moradorId = Convert.ToInt32(User.Identity.Name);

                var morador = await homeTaskerDbContext.Moradores
                    .Include(c => c.Conta)
                    .Include(o => o.Ocorrencia)
                    .Include(p => p.Produtos)
                    .Include(r => r.Reservas)
                    .Include(s => s.Servicos)
                    .Include(v => v.Visitantes)
                    .FirstOrDefaultAsync(m => m.Id == moradorId);

                if (morador == null)
                {
                    return NotFound();
                }

                return Ok(morador);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("Login")]
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

                var token = TokenService.GenerateToken(morador);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
