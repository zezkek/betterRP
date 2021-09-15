using Exiled.API.Features;
using Exiled.Events.EventArgs;
using RolePlayNames.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using PlayerEv = Exiled.Events.Handlers.Player;
using MapEv = Exiled.Events.Handlers.Map;
using SvEv = Exiled.Events.Handlers.Server;

namespace betterRP
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "BetterRP";
        public override string Author { get; } = ".fkn_goose & Mydak";
        public override string Prefix => "BetterRP";
        public override Version Version => new Version(0, 3, 5);
        public override Version Version => new Version(0, 3, 5);

        private readonly List<MEC.CoroutineHandle> coroutines = new List<MEC.CoroutineHandle>();
        public void AddCoroutine(MEC.CoroutineHandle coroutineHandle) => coroutines.Add(coroutineHandle);
        public void NewCoroutine(IEnumerator<float> coroutine, MEC.Segment segment = MEC.Segment.Update) => coroutines.Add(MEC.Timing.RunCoroutine(coroutine, segment));

        //Для передачи объекта класса Plugin и получения с него конфигов для команд
        //public CommandMethods commandMethods;
        public static readonly Lazy<Plugin> LazyInstance = new Lazy<Plugin>(valueFactory: () => new Plugin());
        public static Plugin PluginItem => LazyInstance.Value;
        private PlayerNames PlayerNames;
        private PlayerResize PlayerResize;
        private GrenadesAdditionalEffects GrenadesAdditionalEffects;
        private StartsBlackout startsBlackout;
        private PlayerLeave replaceSCP;
        public override void OnEnabled()
        {
            if (!Config.IsEnabled)
            {
                Log.Info("Better RP is disabled via configs. It will not be loaded.");
                return;
            }
            Log.Info("BetterRP enabled");
            //if (!Config.IsEnabled)
            //{
            //    Log.Info("better RP is disabled via configs. It will not be loaded.");
            //    return;
            //}
            Log.Info("betterRP enabled");
            //commandMethods = new CommandMethods(this);
            PlayerNames = new PlayerNames();
            PlayerResize = new PlayerResize();
            GrenadesAdditionalEffects = new GrenadesAdditionalEffects(this);

            replaceSCP = new PlayerLeave();
            startsBlackout = new StartsBlackout(this);

            PlayerEv.ChangedRole += PlayerNames.OnChangedRole;
            PlayerEv.Dying += PlayerNames.OnDying;
            PlayerEv.ChangedRole += PlayerResize.OnChangedRoleEventArgs;
            PlayerEv.Dying += PlayerResize.OnDying;
            PlayerEv.Left += replaceSCP.OnDisconnect;
            PlayerEv.Hurting += GrenadesAdditionalEffects.OnDamage;
            SvEv.RoundStarted += PlayerNames.OnRoundStarted;
            SvEv.RoundStarted += startsBlackout.OnRoundStarted;
            MapEv.ExplodingGrenade += GrenadesAdditionalEffects.OnFlash;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            //commandMethods = null;
            if (coroutines != null && coroutines.Count > 0) MEC.Timing.KillCoroutines(coroutines.ToArray());

            PlayerEv.ChangedRole -= PlayerNames.OnChangedRole;
            PlayerEv.Dying -= PlayerNames.OnDying;
            PlayerEv.ChangedRole -= PlayerResize.OnChangedRoleEventArgs;
            PlayerEv.Dying -= PlayerResize.OnDying;
            PlayerEv.Left -= replaceSCP.OnDisconnect;
            PlayerEv.Hurting -= GrenadesAdditionalEffects.OnDamage;
            SvEv.RoundStarted -= startsBlackout.OnRoundStarted;
            SvEv.RoundStarted -= PlayerNames.OnRoundStarted;
            MapEv.ExplodingGrenade -= GrenadesAdditionalEffects.OnFlash;

            PlayerNames = null;
            PlayerResize = null;
            GrenadesAdditionalEffects = null;
            startsBlackout = null;
            base.OnDisabled();

            if (Config.IsEnabled)
            {
                Log.Info("BetterRP disabled");
                base.OnDisabled();
            }
        }
    }
}
