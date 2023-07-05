using HomeTaskerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeTaskerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("Produtos")]
        public async Task<IActionResult> getAllAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext)
        {
            var produtos = await homeTaskerDbContext
                .Produtos
                .AsNoTracking()
                .ToListAsync();

            return produtos == null ? NotFound() : Ok(produtos);
        }

        [Authorize]
        [HttpGet]
        [Route("Produto/{id}")]
        public async Task<IActionResult> getByIdAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var produto = await homeTaskerDbContext
                .Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return produto == null ? NotFound() : Ok(produto);   
        }

        [Authorize]
        [HttpPost]
        [Route("Produto/{id}")]
        public async Task<IActionResult> registerProductAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Produto produto,
            [FromRoute] int id )
        {
            var morador = await homeTaskerDbContext
            .Moradores.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

            if (morador == null)
            {
                return NotFound();
            }

            produto.Morador = morador;

            try
            {
                homeTaskerDbContext.Set<Produto>().Add(produto);
                homeTaskerDbContext.Entry(produto.Morador).State = EntityState.Unchanged;
                await homeTaskerDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created($"Produto/Produto/{morador.Id}", produto);
        }

        [Authorize]
        [HttpPut]
        [Route("Produto/{id}/Alterar")]
        public async Task<IActionResult> PutAsyncProducts(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromBody] Produto produto,
            [FromRoute] int id )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model inválida");
            }

            var prod = await homeTaskerDbContext
                .Produtos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prod == null)
            {
                NotFound("Produto não encontrado!");
            }

            try
            {

                if (!string.IsNullOrEmpty(produto.NomeProduto))
                {
                    prod.NomeProduto = produto.NomeProduto;
                }
                else
                {
                    BadRequest("O nome do produto é obrigatório!");
                }

                if (!string.IsNullOrEmpty(produto.DescricaoProduto))
                {
                    prod.DescricaoProduto = produto.DescricaoProduto;
                }

                if (produto.Preco != 0)
                {
                    prod.Preco = produto.Preco;
                }

                homeTaskerDbContext.Produtos.Update(prod);
                await homeTaskerDbContext.SaveChangesAsync();
                return Ok(prod);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("Produto/{id}/Apagar")]
        public async Task<IActionResult> deleteProductAsync(
            [FromServices] HomeTaskerDbContext homeTaskerDbContext,
            [FromRoute] int id )
        {
            var prd = await homeTaskerDbContext
                .Produtos
                .FirstOrDefaultAsync (p => p.Id == id);

            if (prd == null)
            {
                return NotFound("Produto não encontrado!");
            }

            try
            {
                homeTaskerDbContext.Produtos.Remove(prd);
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
