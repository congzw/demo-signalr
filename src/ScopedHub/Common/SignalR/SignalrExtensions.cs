using Common.Http;
using Microsoft.AspNetCore.SignalR;

// ReSharper disable CheckNamespace

namespace Common.SignalR
{
    public static class SignalRExtensions
    {
        public static T TryGetQueryParameterValue<T>(this Hub hub, string queryParameterName, T defaultValue = default(T))
        {
            if (hub == null)
            {
                return defaultValue;
            }

            return hub.Context.TryGetQueryParameterValue(queryParameterName, defaultValue);
        }

        public static T TryGetQueryParameterValue<T>(this HubCallerContext hubCallerContext, string queryParameterName, T defaultValue = default(T))
        {
            if (hubCallerContext == null)
            {
                return defaultValue;
            }


            var httpContext = hubCallerContext.GetHttpContext();
            if (httpContext == null)
            {
                return defaultValue;
            }

            return httpContext.TryGetQueryParameterValue(queryParameterName, defaultValue);
        }
    }
}
