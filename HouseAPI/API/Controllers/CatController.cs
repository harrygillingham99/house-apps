namespace House.API.Controllers
{
    using HLL.Cat.Interfaces;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("cat")]
    public class CatController : BaseController
    {
        private readonly ICatServiceClient _cat;
        public CatController(ICatServiceClient cat)
        {
            _cat = cat;
        }

        [HttpGet("url")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public Task<IActionResult> RandomCatUrl()
        {
            return ExecuteAndMapToActionResult(() => _cat.GetRandomCatUrl());
        }
    }
}