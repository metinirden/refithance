using Refit;
using Refithance.Generator;

namespace Refithance.Debugger.Apis;

[Refithance]
public interface IUserApi
{
    [Get("/users/{id}")]
    public Task<string> GetUser(int id = 1);
}
