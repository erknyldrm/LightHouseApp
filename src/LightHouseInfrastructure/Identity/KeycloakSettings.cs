using System;

namespace LightHouseInfrastructure.Identity;

public class KeycloakSettings
{
    public string Audience { get; set; } = null!;
    public string Authority { get; set; } = null!;
    public string Realm { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public int ClockSkew { get; set; } = 5;
    public bool RequireHttpsMetadata { get; set; } = true;
    public bool ValidateAudience { get; set; } = true;
    public bool ValidateIssuer { get; set; } = true;
    public bool ValidateLifetime { get; set; } = true;
    public bool ValidateTokenSignature { get; set; } = true;
}
