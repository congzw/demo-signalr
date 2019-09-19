using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Common;
using Microsoft.AspNetCore.SignalR.Client;

namespace ScopedHubMonitor.Scoped
{
    public partial class ScopedConnectionForm : Form
    {
        public ScopedConnectionForm()
        {
            InitializeComponent();
            MyInit();
        }

        public ScopedConnectionFormCtrl Ctrl { get; set; }

        private void MyInit()
        {
            Ctrl = new ScopedConnectionFormCtrl();
            this.txtMessage.ScrollBars = ScrollBars.Vertical;
        }

        private void ScopedConnectionForm_Load(object sender, EventArgs e)
        {
            var simpleIniFile = SimpleIni.ResolveFile();
            var items = simpleIniFile.TryLoadIniFileItems("config.ini");
            var scopedConfig = new ScopedConfig();
            simpleIniFile.SetProperties(items, scopedConfig, null);
            var hubUri = string.Format("{0}?ScopeGroupId={1}&ClientId={2}", scopedConfig.HubUri, scopedConfig.ScopeGroupId, scopedConfig.ClientId);
            this.txtHubUri.Text = hubUri;
        }
        
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            UpdateState(connected: false);
            await Ctrl.TryStartConnection(this.txtHubUri.Text.Trim(), Log);
            UpdateState(connected: true);
        }

        private async void btnDisConnect_Click(object sender, EventArgs e)
        {
            await Ctrl.TryStopConnection(Log);
            UpdateState(connected: false);
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var bags = new Dictionary<string, object>();
                bags["ticks"] = "[" + DateHelper.Instance.GetDateNow().Ticks + "]";
                await Ctrl.UpdateBags(bags, Log);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtMessage.Clear();
        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            if (Ctrl.HubConn == null || Ctrl.HubConn.State == HubConnectionState.Disconnected)
            {
                return;
            }

            await Ctrl.TryStopConnection(Log);
            UpdateState(connected: false);
            base.OnClosing(e);
        }
        
        private void UpdateState(bool connected)
        {
            btnDisConnect.Enabled = connected;
            btnConnect.Enabled = !connected;
            txtHubUri.Enabled = !connected;

            //txtMessage.Enabled = connected;
            btnUpdate.Enabled = connected;
        }

        private void Log(string message)
        {
            void Callback()
            {
                this.txtMessage.AppendText(message + Environment.NewLine);
            }

            Invoke((Action)Callback);
        }
    }
}
