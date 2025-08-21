using System;

namespace LightHouseDomain.Interfaces;

public interface ICommentAuditor
{
    Task<bool> IsTextAppropriateAsync(string text);

}
