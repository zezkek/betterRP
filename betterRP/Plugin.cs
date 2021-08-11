using Exiled.API.Features;
using Exiled.Events.EventArgs;
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
        public override string Name { get; } = "RP Names";
        public override string Author { get; } = ".fkn_goose";
        public override Version Version => new Version(0, 3, 1);
        public static readonly Lazy<Plugin> LazyInstance = new Lazy<Plugin>(valueFactory: () => new Plugin());
        public static Plugin PluginItem => LazyInstance.Value;
        private PlayerNames PlayerNames;
        private PlayerResize PlayerResize;
        public override void OnEnabled()
        {
            PlayerNames = new PlayerNames();
            PlayerResize = new PlayerResize();
            PlayerEv.ChangedRole += PlayerNames.OnChangedRole;
            PlayerEv.ChangingRole += PlayerResize.OnChangingRole;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            PlayerEv.ChangedRole -= PlayerNames.OnChangedRole;
            PlayerEv.ChangingRole -= PlayerResize.OnChangingRole;
            PlayerNames = null;
            PlayerResize = null;
            base.OnDisabled();
        }
    }
}
