using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
// ReSharper disable CheckNamespace

namespace Common.SignalR
{
    public static class SignalRExtensions
    {
        public static T GetQueryParameterValue<T>(this IQueryCollection httpQuery, string queryParameterName) =>
            httpQuery.TryGetValue(queryParameterName, out var value) && value.Any()
                ? (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T))
                : default(T);
    }
}
