namespace BetterRP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using RolePlayNames.Handlers;
    using UnityEngine;
    using MapEv = Exiled.Events.Handlers.Map;
    using PlayerEv = Exiled.Events.Handlers.Player;
    using SvEv = Exiled.Events.Handlers.Server;

    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "BetterRP";

        public override string Author { get; } = ".fkn_goose & Mydak";

        public override string Prefix => "BetterRP";

        public override Version Version => new Version(0, 3, 7);

#pragma warning disable SA1201 // Elements should appear in the correct order
        private readonly List<MEC.CoroutineHandle> coroutines = new List<MEC.CoroutineHandle>();
#pragma warning restore SA1201 // Elements should appear in the correct order

        public void AddCoroutine(MEC.CoroutineHandle coroutineHandle) => this.coroutines.Add(coroutineHandle);

        public void NewCoroutine(IEnumerator<float> coroutine, MEC.Segment segment = MEC.Segment.Update) => this.coroutines.Add(MEC.Timing.RunCoroutine(coroutine, segment));

#pragma warning disable SA1201 // Elements should appear in the correct order
        public static Plugin PluginItem => new Lazy<Plugin>(valueFactory: () => new Plugin()).Value;
#pragma warning restore SA1201 // Elements should appear in the correct order

#pragma warning disable SA1201 // Elements should appear in the correct order
        private PlayerNames playerNames;
#pragma warning restore SA1201 // Elements should appear in the correct order

        private PlayerResize playerResize;

        private GrenadesAdditionalEffects grenadesAdditionalEffects;

        private StartsBlackout startsBlackout;

        private PlayerLeave replaceSCP;

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            if (!this.Config.IsEnabled)
            {
                Log.Info("Better RP is disabled via configs. It will not be loaded.");
                return;
            }

            // commandMethods = new CommandMethods(this);
            this.playerNames = new PlayerNames();
            this.playerResize = new PlayerResize();
            this.grenadesAdditionalEffects = new GrenadesAdditionalEffects(this);

            this.replaceSCP = new PlayerLeave();
            this.startsBlackout = new StartsBlackout(this);

            PlayerEv.ChangedRole += this.playerNames.OnChangedRole;
            PlayerEv.Dying += this.playerNames.OnDying;
            PlayerEv.ChangedRole += this.playerResize.OnChangedRoleEventArgs;
            PlayerEv.Dying += this.playerResize.OnDying;
            PlayerEv.Left += this.replaceSCP.OnDisconnect;
            PlayerEv.Hurting += this.grenadesAdditionalEffects.OnDamage;
            SvEv.RoundStarted += this.playerNames.OnRoundStarted;
            SvEv.RoundStarted += this.startsBlackout.OnRoundStarted;
            MapEv.ExplodingGrenade += this.grenadesAdditionalEffects.OnFlash;

            this.NamesFileCheck();

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            // commandMethods = null;
            if (this.coroutines != null && this.coroutines.Count > 0)
            {
                MEC.Timing.KillCoroutines(this.coroutines.ToArray());
            }

            PlayerEv.ChangedRole -= this.playerNames.OnChangedRole;
            PlayerEv.Dying -= this.playerNames.OnDying;
            PlayerEv.ChangedRole -= this.playerResize.OnChangedRoleEventArgs;
            PlayerEv.Dying -= this.playerResize.OnDying;
            PlayerEv.Left -= this.replaceSCP.OnDisconnect;
            PlayerEv.Hurting -= this.grenadesAdditionalEffects.OnDamage;
            SvEv.RoundStarted -= this.startsBlackout.OnRoundStarted;
            SvEv.RoundStarted -= this.playerNames.OnRoundStarted;
            MapEv.ExplodingGrenade -= this.grenadesAdditionalEffects.OnFlash;

            this.playerNames = null;
            this.playerResize = null;
            this.grenadesAdditionalEffects = null;
            this.startsBlackout = null;
            base.OnDisabled();

            if (this.Config.IsEnabled)
            {
                Log.Info("BetterRP disabled");
                base.OnDisabled();
            }
        }

        private void NamesFileCheck()
        {
            if (!Directory.Exists(PluginItem.Config.ItemConfigFolder))
            {
                Directory.CreateDirectory(PluginItem.Config.ItemConfigFolder);
            }

            string sSfilePath = Path.Combine(PluginItem.Config.ItemConfigFolder, "Names/ScientistAndSecuritySurname.txt");
            string nickfilePath = Path.Combine(PluginItem.Config.ItemConfigFolder, "Names/CallSigns.txt");

            Log.Info($"{sSfilePath}");
            if (!File.Exists(sSfilePath))
            {
                File.WriteAllText(sSfilePath, string.Empty);
            }

            Log.Info($"{nickfilePath}");
            if (!File.Exists(nickfilePath))
            {
                File.WriteAllText(nickfilePath, string.Empty);
            }
        }
    }
}
