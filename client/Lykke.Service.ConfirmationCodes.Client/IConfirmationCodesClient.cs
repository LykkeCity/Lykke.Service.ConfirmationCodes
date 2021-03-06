﻿using System;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.ConfirmationCodes.Client.Models.Request;
using Lykke.Service.ConfirmationCodes.Client.Models.Response;
using Lykke.Service.ConfirmationCodes.Contract.Models;
using Refit;

namespace Lykke.Service.ConfirmationCodes.Client
{
    /// <summary>
    /// Interface for calling service via HTTP
    /// </summary>
    public interface IConfirmationCodesClient
    {
        /// <summary>
        /// Sends confirmation code to email
        /// </summary>
        [Post("/api/EmailConfirmation")]
        Task SendEmailConfirmationAsync([Body] SendEmailConfirmationRequest model);

        /// <summary>
        /// Checks if code is valid
        /// </summary>
        [Post("/api/EmailConfirmation/VerifyCode")]
        Task<VerificationResult> VerifyEmailCodeAsync([Body] VerifyEmailConfirmationRequest model);

        /// <summary>
        /// Returns last code
        /// </summary>
        /// <returns></returns>
        [Get("/api/EmailConfirmation/GetEmailPriorityCode")]
        Task<EmailVerificationCode> GetEmailPriorityCodeAsync([Query] EmailConfirmationRequest model);

        /// <summary>
        /// Removes codes
        /// </summary>
        /// <returns></returns>
        [Delete("/api/EmailConfirmation")]
        Task DeleteEmailCodeAsync([Body] EmailConfirmationRequest model);

        /// <summary>
        /// Sends confirmation code via sms
        /// </summary>
        [Post("/api/SmsConfirmation")]
        [Obsolete("Use SendSmsConfirmCodeAsync instead")]
        Task SendSmsConfirmationAsync([Body] SendSmsConfirmationRequest model);

        /// <summary>
        /// Sends confirmation code via sms
        /// </summary>
        [Post("/api/SmsConfirmation/SmsConfirmCode")]
        Task<SmsConfirmationResponse> SendSmsConfirmCodeAsync([Body] SmsConfirmCodeRequest model);

        /// <summary>
        /// Checks if code is valid
        /// </summary>
        [Post("/api/SmsConfirmation/VerifyCode")]
        Task<VerificationResult> VerifySmsCodeAsync([Body] VerifySmsConfirmationRequest model);

        /// <summary>
        /// Checks if code is valid with operation lockout
        /// </summary>
        [Post("/api/SmsConfirmation/VerifySmsCode")]
        Task<VerificationResult> VerifySmsCodeWithLockoutAsync([Body] CheckSmsCodeRequest model);

        /// <summary>
        /// Returns last code
        /// </summary>
        [Get("/api/SmsConfirmation/GetSmsPriorityCode")]
        Task<SmsVerificationCode> GetSmsPriorityCodeAsync([Query] SmsConfirmationRequest model);

        /// <summary>
        /// Removes codes
        /// </summary>
        [Delete("/api/SmsConfirmation")]
        Task DeleteSmsCodeAsync([Body] SmsConfirmationRequest model);

        /// <summary>
        /// Requests Google 2FA setup for the client
        /// </summary>
        [Post("/api/Google2FA/Setup")]
        Task<RequestSetupGoogle2FaResponse> Google2FaRequestSetupAsync([Body] RequestSetupGoogle2FaRequest model);

        /// <summary>
        /// Verifies that Google 2FA setup was successful by checking the sms code and GA code
        /// </summary>
        [Put("/api/Google2FA/Setup")]
        Task<VerifySetupGoogle2FaResponse> Google2FaVerifySetupAsync([Body] VerifySetupGoogle2FaRequest model);

        /// <summary>
        /// Checks if user has Google 2FA set up
        /// </summary>
        [Get("/api/Google2FA/Setup")]
        Task<bool> Google2FaClientHasSetupAsync([Query] string clientId);

        /// <summary>
        /// Checks code entered by user
        /// </summary>
        [Get("/api/Google2FA/CheckCode")]
        Task<bool> Google2FaCheckCodeAsync([Query] string clientId, [Query] string code);

        /// <summary>
        /// Check if user is blacklisted
        /// </summary>
        [Get("/api/Google2FA/Blacklist")]
        Task<Google2FaBlacklistCheckResponse> Google2FaIsClientBlacklistedAsync([Query] string clientId);

        /// <summary>
        /// Resets Google 2FA set up
        /// </summary>
        [Delete("/api/Google2FA/Reset")]
        Task<Google2FaBlacklistCheckResponse> Google2FaResetAsync([Query] string clientId);

        /// <summary>
        /// Liveness probe
        /// </summary>
        [Get("/api/IsAlive")]
        Task<IsAliveResponse> IsAlive([Query] SmsConfirmationRequest model);

        /// <summary>
        /// Gets calls count for specified operation
        /// </summary>
        [Post("/api/CallTimeLimits/count")]
        Task<CallsCountResponse> GetCallsCountAsync([Body] CallsCountRequest model);

        /// <summary>
        /// Gets calls count for specified operation
        /// </summary>
        [Post("/api/CallTimeLimits/clear")]
        Task ClearCallsCountAsync([Body] ClearCallsCountRequest model);

        /// <summary>
        /// Checks call limits for the operation
        /// </summary>
        [Post("/api/CallTimeLimits/checkLimit")]
        Task<CallLimitStatus> CheckCallsLimitAsync([Body] CheckOperationLimitRequest model);
    }
}
