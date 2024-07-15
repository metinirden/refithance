using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Refithance.Configuration;

namespace Refithance.Web;

public class HeaderPassThroughDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RefithanceOptions _optionsSnapshot;

    public HeaderPassThroughDelegatingHandler(
        IHttpContextAccessor httpContextAccessor,
        IOptionsSnapshot<RefithanceOptions> optionsSnapshot
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _optionsSnapshot = optionsSnapshot.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext is null || _optionsSnapshot.GlobalHeaderPassThrough.Count is 0)
        {
            return base.SendAsync(request, cancellationToken);
        }

        var headerDictionary = _httpContextAccessor.HttpContext.Request.Headers;
        foreach (var passThroughKey in _optionsSnapshot.GlobalHeaderPassThrough)
        {
            if (headerDictionary.TryGetValue(passThroughKey, out var headerValue))
            {
                request.Headers.Add(passThroughKey, (IEnumerable<string>)headerValue);
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}
