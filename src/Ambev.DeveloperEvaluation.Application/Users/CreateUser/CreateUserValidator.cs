using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

/// <summary>
/// Validator for CreateUserCommand that defines validation rules for user creation command.
/// </summary>
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(user => user.Email)
            .SetValidator(new EmailValidator())
            .WithMessage("Invalid email format.");

        RuleFor(user => user.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .Length(3, 50)
            .WithMessage("Username must be between 3 and 50 characters.");

        RuleFor(user => user.Password)
            .SetValidator(new PasswordValidator())
            .WithMessage("Password does not meet security requirements.");

        RuleFor(user => user.Phone)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must match the international format (+X XXXXXXXXXX).");

        RuleFor(user => user.Status)
            .NotEqual(UserStatus.Unknown)
            .WithMessage("User status cannot be 'Unknown'.");

        RuleFor(user => user.Role)
            .NotEqual(UserRole.None)
            .WithMessage("User role cannot be 'None'.");

        RuleFor(user => user.Name.Firstname)
            .NotEmpty()
            .WithMessage("Firstname is required.");

        RuleFor(user => user.Name.Lastname)
            .NotEmpty()
            .WithMessage("Lastname is required.");

        RuleFor(user => user.Address.Street)
            .NotEmpty()
            .WithMessage("Street is required.");

        RuleFor(user => user.Address.City)
            .NotEmpty()
            .WithMessage("City is required.");

        RuleFor(user => user.Address.Number)
            .NotNull()
            .WithMessage("Number is required.");

        RuleFor(user => user.Address.ZipCode)
            .NotEmpty()
            .WithMessage("ZipCode is required.");
    }
}