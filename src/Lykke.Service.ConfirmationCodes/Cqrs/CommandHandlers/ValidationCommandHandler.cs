using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Cqrs;
using Lykke.Service.ConfirmationCodes.Contract.Commands;
using Lykke.Service.ConfirmationCodes.Contract.Events;
using Lykke.Service.ConfirmationCodes.Core.Services;

namespace Lykke.Service.ConfirmationCodes.Cqrs.CommandHandlers
{
    public class ValidationCommandHandler
    {
        private readonly IGoogle2FaService _google2FaService;
        private readonly IGoogle2FaBlacklistService _blacklistService;

        public ValidationCommandHandler(
            IGoogle2FaService google2FaService,
            IGoogle2FaBlacklistService blacklistService)
        {
            _google2FaService = google2FaService;
            _blacklistService = blacklistService;
        }
        
        [UsedImplicitly]
        public async Task<CommandHandlingResult> Handle(ValidateConfirmationCommand command, IEventPublisher publisher)
        {
            if (!await _google2FaService.ClientHasEnabledAsync(command.ClientId))
            {
                publisher.PublishEvent(new ConfirmationValidationFailedEvent
                {
                    Id = command.Id,
                    ClientId = command.ClientId,
                    Reason = ValidationFailReason.SecondFactorNotSetUp
                });
                
                return CommandHandlingResult.Ok();
            }
            
            if (await _blacklistService.IsClientBlockedAsync(command.ClientId))
            {
                publisher.PublishEvent(new ConfirmationValidationFailedEvent
                {
                    Id = command.Id,
                    ClientId = command.ClientId,
                    Reason = ValidationFailReason.InvalidConfirmation
                });
            }
            
            if (await _google2FaService.CheckCodeAsync(command.ClientId, command.Confirmation))
            {
                await _blacklistService.ClientSucceededAsync(command.ClientId);
                
                publisher.PublishEvent(new ConfirmationValidationPassedEvent
                {
                    Id = command.Id,
                    ClientId = command.ClientId
                });
            }
            else
            {
                await _blacklistService.ClientFailedAsync(command.ClientId);
                
                publisher.PublishEvent(new ConfirmationValidationFailedEvent
                {
                    Id = command.Id,
                    ClientId = command.ClientId,
                    Reason = ValidationFailReason.InvalidConfirmation
                });
            }
            
            return CommandHandlingResult.Ok();
        }
    }
}
