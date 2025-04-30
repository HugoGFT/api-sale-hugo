using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCart;

/// <summary>
/// Validator for CreateCartRequest that defines validation rules for Cart creation.
/// </summary>
public class ListCartRequestValidator : AbstractValidator<ListCartRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateCartRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Email: Must be valid format (using EmailValidator)
    /// - Cartname: Required, length between 3 and 50 characters
    /// - Password: Must meet security requirements (using PasswordValidator)
    /// - Phone: Must match international format (+X XXXXXXXXXX)
    /// - Status: Cannot be Unknown
    /// - Role: Cannot be None
    /// </remarks>
    public ListCartRequestValidator()
    {
        //RuleFor(Cart => Cart.Email).SetValidator(new EmailValidator());
        //RuleFor(Cart => Cart.Cartname).NotEmpty().Length(3, 50);
        //RuleFor(Cart => Cart.Password).SetValidator(new PasswordValidator());
        //RuleFor(Cart => Cart.Phone).Matches(@"^\+?[1-9]\d{1,14}$");
        //RuleFor(Cart => Cart.Status).NotEqual(CartStatus.Unknown);
        //RuleFor(Cart => Cart.Role).NotEqual(CartRole.None);
    }
}