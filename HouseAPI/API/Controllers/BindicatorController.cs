namespace House.API.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using HLL.Dashboard.Bindicator.Models;
    using HLL.Dashboard.Bindicator.Interfaces;
    using System.Net;

    [ApiController]
    [Route("[controller]")]
    public class BindicatorController : BaseController
    {
        private readonly IBindicatorProvider _bindicatorProvider;

        public BindicatorController(IBindicatorProvider bindicatorProvider)
        {
            _bindicatorProvider = bindicatorProvider;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(BinLookup), (int)HttpStatusCode.OK)]
        public Task<IActionResult> Get()
        {
            return ExecuteAndMapToActionResult(() => _bindicatorProvider.Get());
        }
    }
}
