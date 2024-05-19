using Refit;
using Refithance.Generator;

namespace Refithance.Debugger.Apis;

[Refithance]
public interface ITodoApi
{
    [Get("/todos/{id}")]
    public Task<string> GetTask(int id = 1);
}
