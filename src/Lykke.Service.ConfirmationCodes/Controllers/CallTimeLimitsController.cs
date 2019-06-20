using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.ConfirmationCodes.Client.Models.Request;
using Lykke.Service.ConfirmationCodes.Client.Models.Response;
using Lykke.Service.ConfirmationCodes.Contract;
using Lykke.Service.ConfirmationCodes.Contract.Models;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Services;
using Lykke.Service.ConfirmationCodes.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.ConfirmationCodes.Controllers
{
    [Route("api/[controller]")]
    [ModelStateValidationActionFilter]
    public class CallTimeLimitsController : Controller
    {
        private readonly ICallTimeLimitsService _callTimeLimitsService;
        private readonly IGoogle2FaBlacklistService _blacklistService;
        private readonly ILog _log;

        public CallTimeLimitsController(
            ICallTimeLimitsService callTimeLimitsService,
            IGoogle2FaBlacklistService blacklistService,
            ILogFactory logFactory
            )
        {
            _callTimeLimitsService = callTimeLimitsService;
            _blacklistService = blacklistService;
            _log = logFactory.CreateLog(this);
        }

        [HttpPost("count")]
        [ProducesResponseType(typeof(CallsCountResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetCallsCount([FromBody] CallsCountRequest model)
        {
            try
            {
                int callsCount = await _callTimeLimitsService.GetCallsCountAsync(model.Operation, model.ClientId);

                return Ok(new CallsCountResponse { Count = callsCount });
            }
            catch (Exception exception)
            {
                _log.Error(exception, context: new { model.ClientId });

                throw;
            }
        }

        [HttpPost("clear")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ClearCallsCount([FromBody] CallsCountRequest model)
        {
            try
            {
                var tasks = new List<Task>
                {
                    _callTimeLimitsService.ClearCallsHistoryAsync(model.Operation, model.ClientId)
                };

                if (model.Operation == ConfirmOperations.Google2FaSmsConfirm)
                {
                    tasks.Add(_blacklistService.ResetAsyncAsync(model.ClientId));
                }

                await Task.WhenAll(tasks);

                return Ok();
            }
            catch (Exception exception)
            {
                _log.Error(exception, context: new { model.ClientId });

                throw;
            }
        }

        [HttpPost("checkLimit")]
        [ProducesResponseType(typeof(CallLimitStatus), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> CheckCallsLimit([FromBody] CheckOperationLimitRequest model)
        {
            try
            {
                CallLimitsResult callLimitsResult = await _callTimeLimitsService.ProcessCallLimitsAsync
                    (model.ClientId, model.Operation, model.RepeatCheck, false);

                return Ok(callLimitsResult.Status);
            }
            catch (Exception exception)
            {
                _log.Error(exception, context: model);

                throw;
            }
        }
    }
}
