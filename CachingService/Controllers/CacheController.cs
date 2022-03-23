using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly ILogger<CacheController> _logger;

        public CacheController(ILogger<CacheController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Cache> GetCache()
        {
            throw new NotImplementedException("Not implemented exception");
        }

        [HttpPost]
        public IEnumerable<Cache> AddCache()
        {
            throw new NotImplementedException("Not implemented exception");
        }
    }
}
