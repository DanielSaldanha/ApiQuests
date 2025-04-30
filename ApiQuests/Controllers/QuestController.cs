using Microsoft.AspNetCore.Mvc;
using ApiQuests.Model;
using Microsoft.AspNetCore.Authorization;
namespace ApiQuests.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestController : ControllerBase
    {
        private readonly IHttpClientFactory _IHttpClientFactory;

        public QuestController(IHttpClientFactory IHttpClientFactory)
        {
            _IHttpClientFactory = IHttpClientFactory;
        }

        [HttpGet("questions")]
        public async Task<ActionResult> GetQuest(int id)
        {
            var client = _IHttpClientFactory.CreateClient("CurrencyAPI");
            var response = await client.GetAsync($"/api.php?amount=5&category={id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound("response nao está funcionando");
            }
            var QuestJson = await response.Content.ReadFromJsonAsync<QuestModel>();
            if(QuestJson == null)
            {
                return NotFound("valor nulo");
            }
            return Ok(QuestJson);
        }
        [HttpGet("questions2")]
        public async Task<ActionResult> GetQuest2(int id)
        {
            var client = _IHttpClientFactory.CreateClient("CurrencyAPI");
            var response = await client.GetAsync($"/api.php?amount={id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound("response nao está funcionando");
            }
            var QuestJson = await response.Content.ReadFromJsonAsync<QuestModel>();
            if (QuestJson == null)
            {
                return NotFound("valor nulo");
            }
            return Ok(QuestJson);
        }
    }
}
