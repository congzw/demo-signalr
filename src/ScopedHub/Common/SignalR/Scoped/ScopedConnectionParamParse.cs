using System;
using Microsoft.AspNetCore.SignalR;
// ReSharper disable CheckNamespace

namespace Common.SignalR.Scoped
{
    public interface IScopedConnectionParamParse
    {
        string TryGetParamValue(Hub hub, string paramName);
    }

    public class ScopedConnectionParamParse : IScopedConnectionParamParse
    {
        public string TryGetParamValue(Hub hub, string paramName)
        {
            var httpContext = hub.Context.GetHttpContext();
            if (httpContext == null)
            {
                return string.Empty;
            }
            var query = httpContext.Request.Query;
            var paramValue = query.GetQueryParameterValue<string>(paramName);
            return string.IsNullOrWhiteSpace(paramValue) ? string.Empty : paramValue.Trim();
        }
        
        #region for di extensions

        private static readonly Lazy<IScopedConnectionParamParse> LazyInstance = new Lazy<IScopedConnectionParamParse>(() => new ScopedConnectionParamParse());
        public static Func<IScopedConnectionParamParse> Resolve { get; set; } = () => LazyInstance.Value;

        #endregion
    }
}