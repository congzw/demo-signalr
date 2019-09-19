using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.SignalR.Scoped;
using Microsoft.AspNetCore.SignalR.Client;

namespace ScopedHubMonitor.Scoped
{
    public class ScopedConnectionFormCtrl
    {
        public HubConnection HubConn { get; set; }

        public async Task UpdateBags(IDictionary<string, object> bags, Action<string> logMessage)
        {
            if (HubConn == null || HubConn.State == HubConnectionState.Disconnected)
            {
                logMessage?.Invoke("Connection Not Start...");
                return;
            }

            await HubConn.InvokeAsync(ScopedConnection.UpdateScopedConnectionBags, bags);
            logMessage?.Invoke(ScopedConnection.UpdateScopedConnectionBags + "Invoked...");
        }

        public async Task TryStartConnection(string hubUri, Action<string> logMessage)
        {
            void ScopedConnectionsUpdated(IList<ScopedConnection> connections)
            {
                logMessage("----Changed----");
                var sb = new StringBuilder();
                foreach (var item in connections)
                {
                    //item.UpdateDesc();
                    //sb.AppendFormat("ClientId={0};CreatedAt={1};ConnId={2}{3}", item.ClientId, item.CreateAt.AsFormat(), item.ConnectionId, Environment.NewLine);
                    sb.AppendFormat("{0}{1}", item.Desc, Environment.NewLine);
                }
                logMessage(sb.ToString());
            }

            var hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUri)
                .Build();

            hubConnection.On<IList<ScopedConnection>>(ScopedConnection.CallBackUpdateScopedConnections, ScopedConnectionsUpdated);

            logMessage?.Invoke("Starting connection...");
            try
            {
                await hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                logMessage?.Invoke(ex.ToString());
                await hubConnection.DisposeAsync();
                return;
            }
            logMessage?.Invoke("Connection established.");
            HubConn = hubConnection;
        }

        public async Task TryStopConnection(Action<string> logMessage)
        {
            if (HubConn == null || HubConn.State == HubConnectionState.Disconnected)
            {
                logMessage?.Invoke("No Need Stop connection...");
                return;
            }

            logMessage?.Invoke("Stopping connection...");
            try
            {
                await HubConn.StopAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logMessage?.Invoke(ex.ToString());
                return;
            }

            logMessage?.Invoke("Connection terminated.");
            HubConn = null;
        }
    }
}
