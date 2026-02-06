using FinancialBox.Application.DomainEvents;
using FinancialBox.Domain.Features.Users.Events;

namespace FinancialBox.Application.Features.Auth.Commands.Register.Events;

public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredEvent>
{
    //private readonly IEmailService _emailService;

    //public UserRegisteredEventHandler(IEmailService emailService)
    //{
    //    _emailService = emailService;
    //}

    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        var subject = "Welcome to FinancialBox!";
        var body = $"Hello,\n\nYour account with email {notification.Email} was successfully registered.\n\nEnjoy our services!";

        //await _emailService.SendAsync(notification.Email, subject, body, cancellationToken);
        Console.WriteLine(subject);

        await Task.CompletedTask;
    }
}
