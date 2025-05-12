using Microsoft.AspNetCore.Mvc;
using ApiQuests.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace ApiQuests.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestController : ControllerBase
    {
        private readonly IHttpClientFactory _IHttpClientFactory;
        private readonly IMemoryCache _cache;

        public QuestController(IMemoryCache cache ,IHttpClientFactory IHttpClientFactory)
        {
            _IHttpClientFactory = IHttpClientFactory;
            _cache = cache;
        }

        [HttpGet("questions")]
        public async Task<ActionResult> GetQuest(int id)
        {
            string key = $"valor_{id}";
            if(_cache.TryGetValue(key, out QuestModel cacheQuast))
            {
                return Ok(cacheQuast);
            }

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
            var cacheoptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };
            _cache.Set(key,QuestJson,cacheoptions);
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
