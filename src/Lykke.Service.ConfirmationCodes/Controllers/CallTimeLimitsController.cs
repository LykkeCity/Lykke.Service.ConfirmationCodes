using System;
using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.ConfirmationCodes.Client.Models.Request;
using Lykke.Service.ConfirmationCodes.Client.Models.Response;
using Lykke.Service.ConfirmationCodes.Core.Repositories;
using Lykke.Service.ConfirmationCodes.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.ConfirmationCodes.Controllers
{
    [Route("api/[controller]")]
    [ModelStateValidationActionFilter]
    public class CallTimeLimitsController : Controller
    {
        private readonly ICallTimeLimitsRepository _callTimeLimitsRepository;
        private readonly ILog _log;

        public CallTimeLimitsController(
            ICallTimeLimitsRepository callTimeLimitsRepository,
            ILogFactory logFactory
            )
        {
            _callTimeLimitsRepository = callTimeLimitsRepository;
            _log = logFactory.CreateLog(this);
        }
        
        [HttpPost("count")]
        [ProducesResponseType(typeof(CallsCountResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetCallsCount([FromBody] CallsCountRequest model)
        {
            try
            {
                int callsCount = await _callTimeLimitsRepository.GetCallsCount(model.Operation, model.ClientId);

                return Ok(new CallsCountResponse { Count = callsCount });
            }
            catch (Exception exception)
            {
                _log.WriteError(nameof(GetCallsCount), new { model.ClientId }, exception);
                
                throw;
            }
        }
        
        [HttpPost("clear")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ClearCallsCount([FromBody] CallsCountRequest model)
        {
            try
            {
                await _callTimeLimitsRepository.ClearCallsHistory(model.Operation, model.ClientId);

                return Ok();
            }
            catch (Exception exception)
            {
                _log.WriteError(nameof(ClearCallsCount), new { model.ClientId }, exception);
                
                throw;
            }
        }
    }
}
