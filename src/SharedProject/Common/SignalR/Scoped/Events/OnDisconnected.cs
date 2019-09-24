// ReSharper disable CheckNamespace
namespace Common.SignalR.Scoped
{
    public static class OnDisconnectedEventExtensions
    {
        public static string _UpdateScopedConnectionBags = "UpdateScopedConnectionBags";
        public static string _ScopedConnectionsUpdated = "ScopedConnectionsUpdated";

        public static string UpdateScopedConnectionBags(this ScopedConstForServer server)
        {
            return _UpdateScopedConnectionBags;
        }

        public static string ScopedConnectionsUpdated(this ScopedConstForClient client)
        {
            return _ScopedConnectionsUpdated;
        }
    }
}
