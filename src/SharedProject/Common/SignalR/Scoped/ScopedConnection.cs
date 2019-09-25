using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

// ReSharper disable CheckNamespace

namespace Common.SignalR.Scoped
{
    public class ScopedConnection
    {
        public ScopedConnection()
        {
            ScopeGroupId = ScopedConst.ForOther.DefaultScopeGroup();
            var now = DateHelper.Instance.GetDateNow();
            CreateAt = now;
            LastUpdateAt = now;
            Bags = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public string ScopeGroupId { get; set; }
        public string ClientId { get; set; }
        //只有在Hub内触发的事件，才应该被赋值
        public string ConnectionId { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
        public IDictionary<string, object> Bags { get; set; }


        public T GetBagValue<T>(string key, T defaultValue = default(T))
        {
            var tryGetValue = Bags.TryGetValue(key, out object value);
            if (tryGetValue)
            {
                return (T)value;
            }
            return defaultValue;
        }
        public ScopedConnection SetBagValue<T>(string key, T value)
        {
            Bags[key] = value;
            return this;
        }

        public static ScopedConnection Create(string scopeGroupId, string clientId, string connectionId)
        {
            var context = new ScopedConnection();
            context.ClientId = clientId ?? string.Empty;
            context.ScopeGroupId = scopeGroupId ?? string.Empty;
            context.ConnectionId = connectionId ?? string.Empty;
            return context;
        }

        #region for debug only

        public string Desc { get; set; }
        public string CreateDesc()
        {
            return this.AsIniString(new string[] { nameof(Desc), nameof(Bags) });
        }
        public ScopedConnection UpdateDesc()
        {
            this.Desc = CreateDesc();
            var bagIni = this.Bags.AsIniString(null);
            if (!string.IsNullOrWhiteSpace(bagIni))
            {
                this.Desc += ";";
                this.Desc += bagIni;
            }
            return this;
        }

        #endregion
    }
}
