using System.Net;
using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Client.Models.Request;
using Lykke.Service.ConfirmationCodes.Client.Models.Response;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Services;
using Lykke.Service.ConfirmationCodes.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.ConfirmationCodes.Controllers
{
    [Route("api/[controller]")]
    [ModelStateValidationActionFilter]
    public class EmailConfirmationController : Controller
    {
        private readonly IEmailConfirmationService _emailConfirmationService;

        public EmailConfirmationController(IEmailConfirmationService emailConfirmationService)
        {
            _emailConfirmationService = emailConfirmationService;
        }
        /// <summary>
        /// Generates code and sends it to specified phone number using email. Code will be persisted.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody]SendEmailConfirmationRequest model)
        {
            await _emailConfirmationService.SendConfirmEmail(model.Email, model.PartnerId, model.IsPriority, model.ExpirationInterval, model.CodeLength);

            return Ok();
        }

        /// <summary>
        /// Verifies code for specified email.
        /// </summary>
        /// <returns>
        /// VerificationResult
        /// If code matched to stored one, IsValid flag will be set to "true"
        /// </returns>
        [HttpPost]
        [SwaggerOperation("VerifyCode")]
        [Route("VerifyCode")]
        [ProducesResponseType(typeof(VerificationResult), (int)HttpStatusCode.OK)]        
        public async Task<IActionResult> VerifyCode([FromBody]VerifyEmailConfirmationRequest model)
        {
            var isValid = await _emailConfirmationService.CheckAsync(model.Email, model.PartnerId, model.Code);
            if (isValid) await _emailConfirmationService.DeleteCodes(model.Email, model.PartnerId);

            return Ok(new VerificationResult{IsValid = isValid });
        }

        /// <summary>
        /// Get actual code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("GetEmailPriorityCode")]
        [Route("GetEmailPriorityCode")]
        [ProducesResponseType(typeof(EmailVerificationCode), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetEmailPriorityCode(EmailConfirmationRequest model)
        {
            var result = await _emailConfirmationService.GetPriorityCode(model.Email, model.PartnerId);

            if (result == null)
            {
                return BadRequest("Code not found.");
            }

            var response = new EmailVerificationCode(result.Code, result.CreationDateTime, result.ExpirationDate);

            return Ok(response);
        }

        /// <summary>
        /// Removes codes
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation("Delete")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromBody]EmailConfirmationRequest model)
        {
            await _emailConfirmationService.DeleteCodes(model.Email, model.PartnerId);

            return Ok();
        }
    }
}
