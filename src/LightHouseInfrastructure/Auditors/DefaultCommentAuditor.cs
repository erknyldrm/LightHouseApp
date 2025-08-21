using System;
using LightHouseDomain.Interfaces;

namespace LightHouseInfrastructure.Auditors;

public class DefaultCommentAuditor : ICommentAuditor
{
    private static readonly string[] _bannedWords =
    [
        "spam",
        "racist",
        "sexist",
    ];
    public Task<bool> IsTextAppropriateAsync(string text)
    {
        return Task.FromResult(!_bannedWords.Any(word => text.Contains(word, StringComparison.OrdinalIgnoreCase)));
    }
}
