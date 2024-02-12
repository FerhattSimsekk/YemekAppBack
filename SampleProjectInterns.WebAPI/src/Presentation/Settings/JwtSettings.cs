using  Application.Settings;

namespace SampleProjectInterns.WebAPI.Presentation.Settings;

public class JwtSettings : IValidatableSettings
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Key { get; set; } = null!;
    public int ExpiresInMinutes { get; set; }

    public bool Validate()
    {
        return !string.IsNullOrEmpty(Issuer)
            && !string.IsNullOrEmpty(Audience)
            && !string.IsNullOrEmpty(Key)
            && ExpiresInMinutes > 0;
    }
}