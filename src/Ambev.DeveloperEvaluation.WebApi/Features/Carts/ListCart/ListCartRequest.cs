namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCart;

/// <summary>
/// Represents a request to create a new Cart in the system.
/// </summary>
public class ListCartRequest
{
    /// <summary>
    /// Gets or sets the Cartname. Must be unique and contain only valid characters.
    /// </summary>
    public string Cartname { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password. Must meet security requirements.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number in format (XX) XXXXX-XXXX.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address. Must be a valid email format.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}