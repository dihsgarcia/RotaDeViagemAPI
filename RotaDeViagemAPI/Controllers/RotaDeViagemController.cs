using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RotaDeViagemAPI.Services.RotaDeViagemService;
using System.Threading.Tasks;

namespace RotaDeViagemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotaDeViagemController : ControllerBase
    {

        private readonly IRotaDeViagemService _rotaDeViagemService;

        public RotaDeViagemController(IRotaDeViagemService rotaDeViagemService)
        {
            _rotaDeViagemService = rotaDeViagemService;
        }

       
        [HttpGet("PesquisarRota/{id}")]
        //[Route("{id}")]
        public async Task<ActionResult<RotaDeViagemResponse>> GetRouteById(int id)
        {
            var result = await _rotaDeViagemService.GetRouteById(id);
            if (result is null)
                return NotFound("Rota não encontrada!");

            return Ok(result);

        }

        [HttpGet("PesquisarMelhorRota/{origem}/{destino}")]
        //[Route("{id}")]
        public ActionResult<RotaDeViagemResponse> GetBestRouteByOrigemDestino(string origem, string destino)
        {
            var result = _rotaDeViagemService.GetBestRouteByOrigemDestino(origem, destino);
            if (result is null)
                return NotFound("Não foi possivel calcular a rota");

            return Ok(result);
        }


        [HttpPost("AdicionarRota")]
        public async Task<ActionResult<RotaDeViagemResponse>> AddRoute([FromBody]RotaDeViagem rota)
        {
            var result = await _rotaDeViagemService.AddRoute(rota);
            if (result is null)
                return NotFound("Não foi possivel adicionar a rota");

            return Ok(result);

        }


        [HttpPut("AtualizarRota/{id}")]
        //[Route("{id}")]
        public async Task<IActionResult> UpdateRoute(int id, RotaDeViagem request)
        {
            var result = await _rotaDeViagemService.UpdateRoute(id, request);
             if (result is null)
                return NotFound("Não foi possivel atualizar a rota!");

            return Ok(result);
       
        }

        [HttpDelete("DeletarRota/{id}")]
        //[Route("{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {    
            var result = await _rotaDeViagemService.DeleteRoute(id);
            if (!result)
                return NotFound("Não foi possivel deletar a rota!");

            return Ok($"Registro com Id: {id}, deletado com sucesso!");

        }

     
    }
}
