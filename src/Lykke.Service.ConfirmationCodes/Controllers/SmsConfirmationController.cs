using System.Net;
using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Client.Models.Request;
using Lykke.Service.ConfirmationCodes.Client.Models.Response;
using Lykke.Service.ConfirmationCodes.Core.Services;
using Lykke.Service.ConfirmationCodes.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.ConfirmationCodes.Controllers
{
    [Route("api/[controller]")]
    [ModelStateValidationActionFilter]
    public class SmsConfirmationController : Controller
    {
        private readonly IConfirmationCodesService _confirmationCodesService;

        public SmsConfirmationController(IConfirmationCodesService confirmationCodesService)
        {
            _confirmationCodesService = confirmationCodesService;
        }

        /// <summary>
        /// Generates code and sends it to specified phone number using SMS. Code will be persisted.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation("Post")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody]SendSmsConfirmationRequest model)
        {
            await _confirmationCodesService.RequestSmsCode(model.PartnerId, model.Phone, model.IsPriority);

            return Ok();
        }

        /// <summary>
        /// Verifies code for specified phone number.
        /// </summary>
        /// <returns>
        /// VerificationResult
        /// </returns>
        [HttpPost]
        [SwaggerOperation("VerifyCode")]
        [Route("VerifyCode")]
        [ProducesResponseType(typeof(VerificationResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> VerifyCode([FromBody]VerifySmsConfirmationRequest model)
        {
            var isValid = await _confirmationCodesService.CheckAsync(model.PartnerId, model.Phone, model.Code);

            return Ok(new VerificationResult{IsValid = isValid });
        }

        /// <summary>
        /// Get actual code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("GetSmsPriorityCode")]
        [Route("GetSmsPriorityCode")]
        [ProducesResponseType(typeof(SmsVerificationCode), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSmsPriorityCode(SmsConfirmationRequest model)
        {
            var result = await _confirmationCodesService.GetPriorityCode(model.PartnerId, model.Phone);

            if (result == null)
            {
                return BadRequest("Code not found.");
            }

            var responseModel = new SmsVerificationCode(result.Code, result.CreationDateTime, result.ExpirationDate);

            return Ok(responseModel);
        }

        /// <summary>
        /// Removes codes
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation("Delete")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromBody]SmsConfirmationRequest model)
        {
            await _confirmationCodesService.DeleteCodes(model.PartnerId, model.Phone);

            return Ok();
        }
    }
}
