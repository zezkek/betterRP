using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlayerEv = Exiled.Events.Handlers.Player;

namespace RolePlayNames
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "RP Names";
        public override string Author { get; } = ".fkn_goose";
        public override Version Version => new Version(0, 2, 0);
        public static readonly Lazy<Plugin> LazyInstance = new Lazy<Plugin>(valueFactory: () => new Plugin());
        public static Plugin PluginItem => LazyInstance.Value;
        private Handlers handler;
        public override void OnEnabled()
        {
            handler = new Handlers();
            PlayerEv.ChangedRole += handler.OnChangedRole;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            PlayerEv.ChangedRole -= handler.OnChangedRole;
            handler = null;
            base.OnDisabled();
        }
    }
}
