namespace BookStoreManager.Contracts.Authentication;

public record AuthenticationResult
(
        Guid Id,
        string Firstname,
        string Lastname,
        string Email,
        string Token
);