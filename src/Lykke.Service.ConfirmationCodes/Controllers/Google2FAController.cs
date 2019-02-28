using System;
using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.ConfirmationCodes.Client.Models.Request;
using Lykke.Service.ConfirmationCodes.Client.Models.Response;
using Lykke.Service.ConfirmationCodes.Contract;
using Lykke.Service.ConfirmationCodes.Contract.Models;
using Lykke.Service.ConfirmationCodes.Core.Exceptions;
using Lykke.Service.ConfirmationCodes.Core.Services;
using Lykke.Service.ConfirmationCodes.Services;
using Lykke.Service.ConfirmationCodes.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.ConfirmationCodes.Controllers
{
    [Route("api/[controller]")]
    [ModelStateValidationActionFilter]
    public class Google2FAController : Controller
    {
        private readonly IGoogle2FaService _google2FaService;
        private readonly IGoogle2FaBlacklistService _blacklistService;
        private readonly IConfirmationCodesService _confirmationCodesService;
        private readonly ILog _log;
        private readonly ConfirmationCodesServiceSettings _confirmationCodesServiceSettings;

        public Google2FAController(
            IGoogle2FaService google2FaService,
            IGoogle2FaBlacklistService blacklistService,
            IConfirmationCodesService confirmationCodesService,
            ILogFactory log,
            ConfirmationCodesServiceSettings confirmationCodesServiceSettings
            )
        {
            _confirmationCodesServiceSettings = confirmationCodesServiceSettings;
            _google2FaService = google2FaService;
            _blacklistService = blacklistService;
            _confirmationCodesService = confirmationCodesService;
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
                    throw new Google2FaAlreadySetException(model.ClientId, "Cannot set up 2FA because it's already set up");
                
                if (_confirmationCodesServiceSettings.Google2FaSetupDisabled)
                    throw new Exception("Google 2FA setup is disabled");

                var manualEntryKey = await _google2FaService.CreateAsync(model.ClientId);

                return Ok(new RequestSetupGoogle2FaResponse {ManualEntryKey = manualEntryKey});
            }
            catch (Exception exception)
            {
                _log.WriteError(nameof(Setup), new { model.ClientId }, exception);
                
                switch (exception)
                {
                    case Google2FaAlreadySetException _:
                        return BadRequest();
                }
                
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
                
                if (await _blacklistService.IsClientBlockedAsync(model.ClientId))
                {
                    throw new Google2FaTooManyAttemptsException(model.ClientId, "Client has exceeded maximum consecutive failed 2FA code verification attempts");
                }

                var checkResult = await _confirmationCodesService.CheckSmsAsync(model.ClientId, model.Phone,
                    model.SmsCode, ConfirmOperations.Google2FaSmsConfirm);
                
                if (checkResult.Status == CallLimitStatus.LimitExceed)
                    throw new Google2FaTooManyAttemptsException(model.ClientId, "Client has exceeded maximum consecutive failed sms verification attempts");
                
                if (checkResult.Status == CallLimitStatus.Allowed && checkResult.Result)
                {
                    var codeIsValid = await _google2FaService.CheckCodeAsync(model.ClientId, model.GaCode, true);

                    if (codeIsValid)
                    {
                        await _blacklistService.ClientSucceededAsync(model.ClientId);
                    }
                    else
                    {
                        await _blacklistService.ClientFailedAsync(model.ClientId);
                    }

                    if (checkResult.Result && codeIsValid)
                    {
                        await _google2FaService.ActivateAsync(model.ClientId);

                        return Ok(new VerifySetupGoogle2FaResponse {IsValid = true});
                    }
                }

                return Ok(new VerifySetupGoogle2FaResponse {IsValid = false});
            }
            catch (Exception exception)
            {
                _log.WriteError(nameof(VerifySetup), new { model.ClientId }, exception);

                switch (exception)
                {
                    case Google2FaAlreadySetException alreadyEx:
                        return BadRequest(alreadyEx.Message);
                    case Google2FaNoSetupInProgressException inProgressEx:
                        return BadRequest(inProgressEx.Message);
                    case Google2FaTooManyAttemptsException _:
                        return StatusCode(403);
                    default:
                        throw;
                }
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

        [HttpGet]
        [Route("CheckCode")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Check2FaCode([FromQuery] string clientId, [FromQuery] string code)
        {
            try
            {
                if (!await _google2FaService.ClientHasEnabledAsync(clientId))
                {
                    throw new Google2FaNotSetUpException(clientId, "Cannot check code because client doesn't have 2FA set up");
                }

                if (await _blacklistService.IsClientBlockedAsync(clientId))
                {
                    throw new Google2FaTooManyAttemptsException(clientId, "Client has exceeded maximum consecutive failed verification attempts");
                }
                
                var codeIsValid = await _google2FaService.CheckCodeAsync(clientId, code);

                if (codeIsValid)
                {
                    await _blacklistService.ClientSucceededAsync(clientId);
                }
                else
                {
                    await _blacklistService.ClientFailedAsync(clientId);
                }

                return Ok(codeIsValid);
            }
            catch (Exception exception)
            {
                _log.WriteError(nameof(Check2FaCode), new { clientId }, exception);
                
                switch (exception)
                {
                    case Google2FaNotSetUpException _:
                        return BadRequest();
                    case Google2FaTooManyAttemptsException _:
                        return StatusCode(403);
                }

                throw;
            }
        }

        [HttpGet]
        [Route("Blacklist")]
        [ProducesResponseType(typeof(Google2FaBlacklistCheckResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> IsClientBlacklisted([FromQuery] string clientId)
        {
            try
            {
                if (!await _google2FaService.ClientHasEnabledAsync(clientId))
                {
                    throw new Google2FaNotSetUpException(clientId, "Cannot check blacklist because client doesn't have 2FA set up");
                }

                return Ok(new Google2FaBlacklistCheckResponse { IsClientBlacklisted = await _blacklistService.IsClientBlockedAsync(clientId) });
            }
            catch (Exception exception)
            {
                _log.WriteError(nameof(IsClientBlacklisted), new { clientId }, exception);

                switch (exception)
                {
                    case Google2FaNotSetUpException _:
                        return BadRequest();
                }

                throw;
            }
        }
    }
}
