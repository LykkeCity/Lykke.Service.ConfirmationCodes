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

        public ValidationCommandHandler(
            IGoogle2FaService google2FaService)
        {
            _google2FaService = google2FaService;
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

            if (await _google2FaService.CheckCodeAsync(command.ClientId, command.Confirmation))
            {
                publisher.PublishEvent(new ConfirmationValidationPassedEvent
                {
                    Id = command.Id,
                    ClientId = command.ClientId
                });
            }
            else
            {
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
