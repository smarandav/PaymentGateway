using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PaymentGatewayApi.Controllers
{
    public class StemsController
    {
        private readonly IWordCompletionService _wordCompletionService;
        private readonly ILogger _logger;

        public StemsController(IWordCompletionService wordCompletionService, ILogger _logger)
        {
            _wordCompletionService = wordCompletionService;
            this._logger = _logger;
        }
        
        public async Task<IActionResult> Get([FromQuery(Name = "stem")] string stem)
        {
            _logger.LogInformation(stem);
            var searchResult = _wordCompletionService.Find(stem);

            var response = new StemsCompletionResponse()
            {
                Data = searchResult
            };

            if (!searchResult.Any())
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(response);

        }
    }

    public class StemsCompletionResponse
    {
        public IEnumerable<string> Data { get; set; }
    }
}