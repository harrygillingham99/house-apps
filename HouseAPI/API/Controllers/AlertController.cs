namespace House.API.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using DAL.Alert.DataTransferObjects;
    using HLL.Alert.Interfaces;
    using HLL.Alert.Models;
    using HLL.News.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    [ApiController]
    public class AlertController : BaseController
    {
        private readonly IAlertProvider _alertProvider;
        private readonly IAutoNewsConsumer _autoNewsConsumer;

        public AlertController(IAlertProvider alertProvider, IAutoNewsConsumer autoNewsConsumer)
        {
            _alertProvider = alertProvider;
            _autoNewsConsumer = autoNewsConsumer;
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<Alert>), (int)HttpStatusCode.OK)]
        public Task<IActionResult> Get()
        {
            return ExecuteAndMapToActionResult(() => _alertProvider.Get());
            
        }

        [HttpGet("Latest")]
        [ProducesResponseType(typeof(IEnumerable<Alert>), (int)HttpStatusCode.OK)]
        public Task<IActionResult> GetLatestAlerts()
        {
            return ExecuteAndMapToActionResult(() => _alertProvider.GetLatestAlerts());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<Alert>), (int)HttpStatusCode.OK)]
        public Task<IActionResult> Get(int id)
        {
            return ExecuteAndMapToActionResult(() => _alertProvider.Get(id));
        }

        [HttpPost("GetMultiple")]
        [ProducesResponseType(typeof(IEnumerable<Alert>), (int)HttpStatusCode.OK)]
        public Task<IActionResult> Get([FromBody] IEnumerable<int> ids)
        {
            return ExecuteAndMapToActionResult(() => _alertProvider.Get(ids));
        }

        [HttpPost("NewAlert")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Post([FromBody] NewAlert newAlert)
        {
            return ExecuteAction(() => _alertProvider.Post(newAlert));
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Put(int id, [FromBody] NewAlert newAlert)
        {
            return ExecuteAction(() => _alertProvider.Put(id, newAlert));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Delete(int id)
        {
            return ExecuteAction(() => _alertProvider.Delete(id));
        }
    }
}
