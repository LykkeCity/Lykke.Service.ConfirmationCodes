using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.HttpClientGenerator;
using Lykke.Service.ConfirmationCodes.AzureRepositories;
using Lykke.Service.ConfirmationCodes.Client;
using Lykke.Service.ConfirmationCodes.Client.Models.Request;
using Lykke.Service.ConfirmationCodes.Client.Models.Response;
using Lykke.Service.ConfirmationCodes.Core.Exceptions;
using Lykke.Service.ConfirmationCodes.Core.Services;
using Lykke.Service.ConfirmationCodes.Services;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Lykke.Service.ConfirmationCodes.Controllers
{
    [Route("api/[controller]")]
    [ModelStateValidationActionFilter]
    public class Google2FAController : Controller
    {
        private readonly IGoogle2FaService _google2FaService;
        private readonly ILog _log;
        
        public Google2FAController(
            IGoogle2FaService google2FaService,
            ILogFactory log)
        {
            _google2FaService = google2FaService;
            _log = log.CreateLog(this);
        }

        [HttpPost]
        [Route("Setup")]
        [ProducesResponseType(typeof(RequestSetupGoogle2FaResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Setup([FromBody] RequestSetupGoogle2FaRequest model)
        {
            try
            {
                if (await _google2FaService.ClientHasEnabledAsync(model.ClientId))
                {
                    throw new Google2FaAlreadySetException(model.ClientId, "Cannot set up 2FA because it's already set up");
                }

                var manualEntryKey = await _google2FaService.CreateAsync(model.ClientId);

                return Ok(new RequestSetupGoogle2FaResponse {ManualEntryKey = manualEntryKey});
            }
            catch (Exception exception)
            {
                _log.WriteError(nameof(Setup), new { model.ClientId }, exception);
                
                if (exception is Google2FaAlreadySetException)
                    return BadRequest();
                
                throw;
            }
        }

        [HttpPut]
        [Route("Setup")]
        [ProducesResponseType(typeof(VerifySetupGoogle2FaResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifySetup([FromBody] VerifySetupGoogle2FaRequest model)
        {
            try
            {
                if (await _google2FaService.ClientHasEnabledAsync(model.ClientId))
                {
                    throw new Google2FaAlreadySetException(model.ClientId, "Cannot set up 2FA because it's already set up");
                }

                if (!await _google2FaService.ClientHasPendingAsync(model.ClientId))
                {
                    throw new Google2FaNoSetupInProgressException(model.ClientId, "No 2FA setup is in progress");
                }

                if (await _google2FaService.CheckCodeAsync(model.ClientId, model.Code, true))
                {
                    await _google2FaService.ActivateAsync(model.ClientId);

                    return Ok(new VerifySetupGoogle2FaResponse {IsValid = true});
                }
                else
                {
                    return Ok(new VerifySetupGoogle2FaResponse {IsValid = false});
                }
            }
            catch (Exception exception)
            {
                _log.WriteError(nameof(VerifySetup), new { model.ClientId }, exception);

                if (exception is Google2FaAlreadySetException || exception is Google2FaNoSetupInProgressException)
                    return BadRequest();
                
                throw;
            }
        }

        [HttpGet]
        [Route("Setup")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ClientHasSetup([FromQuery] string clientId)
        {
            try
            {
                return Ok(await _google2FaService.ClientHasEnabledAsync(clientId));
            }
            catch (Exception exception)
            {
                _log.WriteError(nameof(ClientHasSetup), new { clientId }, exception);
                
                throw;
            }
        }
    }
}
