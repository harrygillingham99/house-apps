namespace House.API.Controllers
{
    using HLL.News.Interfaces;
    using HLL.News.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    [Route("news")]
    public class NewsController : BaseController
    {
        private readonly IAutoNewsConsumer _newsConsumer;

        public NewsController(IAutoNewsConsumer newsConsumer)
        {
            _newsConsumer = newsConsumer;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<NewsMessage>), (int)HttpStatusCode.OK)]
        public Task<IActionResult> GetNews()
        {
            return ExecuteAndMapToActionResult(() => _newsConsumer.CurrentNews());
        }
    }
}