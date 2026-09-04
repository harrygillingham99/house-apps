using House.Objects.Objects;

namespace House.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using HLL.ServerStats;
    using Microsoft.AspNetCore.Mvc;

    [Route("status")]
    public class StatusController : BaseController
    {
        private readonly IMemoryStatusProvider _memoryStatusProvider;
        private readonly IProcessInfo _processInfo;
        public StatusController(IMemoryStatusProvider memoryStatusProvider, IProcessInfo processInfo)
        {
            _memoryStatusProvider = memoryStatusProvider;
            _processInfo = processInfo;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<Status>), (int)HttpStatusCode.OK)]
        public IActionResult GetStats()
        {
            return ExecuteAndMapToActionResultSync(() =>
            {
                var result = _memoryStatusProvider
                    .GetMemoryInfo()
                    .ToList();
                return result;
            });
        }

        [HttpGet("ProcessInfo")]
        [ProducesResponseType(typeof(List<ProcessInfoResult>), (int)HttpStatusCode.OK)]
        public IActionResult GetProcessInfo()
        {
            return ExecuteAndMapToActionResultSync(() =>
            {
                var result = _processInfo
                    .GetProcessInfo()
                    .OrderByDescending(proc => proc.MemoryMbUsed)
                    .ToList();
                return result;
            });
        }
    }
}