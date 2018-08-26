using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Cqrs;
using Lykke.Service.ConfirmationCodes.Conrtact.Commands;
using Lykke.Service.ConfirmationCodes.Conrtact.Events;
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
        public async Task<CommandHandlingResult> Handle(ValidateGoogle2FaCodeCommand command, IEventPublisher publisher)
        {
            if (!await _google2FaService.ClientHasEnabledAsync(command.ClientId))
            {
                publisher.PublishEvent(new Google2FaCodeValidatedEvent
                {
                    Id = command.Id,
                    ClientId = command.ClientId,
                    ValidationResult = Google2FaValidationResult.NotSetUp
                });
                
                return CommandHandlingResult.Ok();
            }

            publisher.PublishEvent(new Google2FaCodeValidatedEvent
            {
                Id = command.Id,
                ClientId = command.ClientId,
                ValidationResult =
                    await _google2FaService.CheckCodeAsync(command.ClientId, command.Code)
                        ? Google2FaValidationResult.Ok
                        : Google2FaValidationResult.Fail
            });
            
            return CommandHandlingResult.Ok();
        }
    }
}
