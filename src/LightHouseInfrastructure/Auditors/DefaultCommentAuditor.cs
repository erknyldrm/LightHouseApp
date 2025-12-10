using System;
using LightHouseApplication.Common;
using LightHouseApplication.Contracts;


namespace LightHouseInfrastructure.Auditors;

public class DefaultCommentAuditor : ICommentAuditor
{
    private static readonly string[] _bannedWords =
    [
        "spam",
        "racist",
        "sexist",
    ];
    public Task<Result<bool>> IsTextAppropriateAsync(string text)
    {
        var containsBannedWord = _bannedWords.Any(word => text.Contains(word, StringComparison.OrdinalIgnoreCase)); 

        if (containsBannedWord)
        {
            return Task.FromResult(Result<bool>.Ok(false));
        }
        else
        {
            return Task.FromResult(Result<bool>.Ok(true));
        }
    }
}
