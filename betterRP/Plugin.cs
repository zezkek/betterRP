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
        public override string Name { get; } = "betterRP";
        public override string Author { get; } = ".fkn_goose & Mydak";
        public override Version Version => new Version(0, 3, 4);

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
        public override void OnEnabled()
        {
            if (!Config.IsEnabled)
            {
                Log.Info("better RP is disabled via configs. It will not be loaded.");
                return;
            }
            Log.Info("betterRP enabled");
            //commandMethods = new CommandMethods(this);
            PlayerNames = new PlayerNames();
            PlayerResize = new PlayerResize();
            GrenadesAdditionalEffects = new GrenadesAdditionalEffects(this);

            startsBlackout = new StartsBlackout(this);

            PlayerEv.ChangedRole += PlayerNames.OnChangedRole;
            PlayerEv.ChangedRole += PlayerResize.OnChangedRoleEventArgs;
            PlayerEv.Hurting += GrenadesAdditionalEffects.OnDamage;
            MapEv.ExplodingGrenade += GrenadesAdditionalEffects.OnFlash;

            SvEv.RoundStarted += startsBlackout.OnRoundStarted;

            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            //commandMethods = null;
            if (coroutines != null && coroutines.Count > 0) MEC.Timing.KillCoroutines(coroutines.ToArray());

            PlayerEv.ChangedRole -= PlayerNames.OnChangedRole;
            PlayerEv.ChangedRole -= PlayerResize.OnChangedRoleEventArgs;
            PlayerEv.Hurting -= GrenadesAdditionalEffects.OnDamage;
            MapEv.ExplodingGrenade -= GrenadesAdditionalEffects.OnFlash;

            SvEv.RoundStarted -= startsBlackout.OnRoundStarted;

            PlayerNames = null;
            PlayerResize = null;
            GrenadesAdditionalEffects = null;
            startsBlackout = null;
            base.OnDisabled();

            if (Config.IsEnabled)
            {
                Log.Info("betterRP disabled");
                base.OnDisabled();
            }
        }
    }
}
