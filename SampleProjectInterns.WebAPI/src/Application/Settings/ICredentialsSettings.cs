namespace Application.Settings;

public interface ICredentialsSettings
{
    string ClientId { get; }
    string ClientSecret { get; }
    string? RedirectUri { get; }
}