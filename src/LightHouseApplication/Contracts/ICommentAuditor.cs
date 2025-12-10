using System;
using LightHouseApplication.Common;

namespace LightHouseApplication.Contracts;

public interface ICommentAuditor
{
    Task<Result<bool>> IsTextAppropriateAsync(string text);

}
