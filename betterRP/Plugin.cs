using betterRP.Handlers;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Events.Handlers;
using RolePlayNames.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlayerEv = Exiled.Events.Handlers.Player;

namespace betterRP
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "betterRP";
        public override string Author { get; } = ".fkn_goose";
        public override Version Version => new Version(0, 5, 0);
        public static readonly Lazy<Plugin> LazyInstance = new Lazy<Plugin>(valueFactory: () => new Plugin());
        public static Plugin PluginItem => LazyInstance.Value;
        private PlayerNames PlayerNames;
        private PlayerResize PlayerResize;
        private ReplaceSCP ReplaceSCP;
        public override void OnEnabled()
        {
            PlayerNames = new PlayerNames();
            PlayerResize = new PlayerResize();
            //ReplaceSCP = new ReplaceSCP();
            PlayerEv.ChangedRole += PlayerNames.OnChangedRole;
            PlayerEv.ChangedRole += PlayerResize.OnChangedRoleEventArgs;
            //PlayerEv.SyncingData += PlayerNames.OnSyncingData;
            //PlayerEv.Left += ReplaceSCP.OnDisconnect;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            PlayerEv.ChangedRole -= PlayerNames.OnChangedRole;
            PlayerEv.ChangedRole -= PlayerResize.OnChangedRoleEventArgs;
            //PlayerEv.SyncingData -= PlayerNames.OnSyncingData;
            //PlayerEv.Left -= ReplaceSCP.OnDisconnect;
            PlayerNames = null;
            PlayerResize = null;
            //ReplaceSCP = null;
            base.OnDisabled();
        }
    }
}
