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
        private ReplaceSCP ReplaceSCP;
        private GrenadesAdditionalEffects GrenadesAdditionalEffects;
        public override void OnEnabled()
        {
            PlayerNames = new PlayerNames();
            PlayerResize = new PlayerResize();
            //ReplaceSCP = new ReplaceSCP();
            PlayerEv.ChangedRole += PlayerNames.OnChangedRole;
            PlayerEv.ChangedRole += PlayerResize.OnChangedRoleEventArgs;
            //PlayerEv.SyncingData += PlayerNames.OnSyncingData;
            //PlayerEv.Left += ReplaceSCP.OnDisconnect;
            PlayerEv.Hurting += GrenadesAdditionalEffects.OnDamage;
            MapEv.ExplodingGrenade += GrenadesAdditionalEffects.OnFlash;
            PlayerEv.Hurting += GrenadesAdditionalEffects.OnDamage;
            MapEv.ExplodingGrenade += GrenadesAdditionalEffects.OnFlash;
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
            PlayerEv.Hurting -= GrenadesAdditionalEffects.OnDamage;
            MapEv.ExplodingGrenade -= GrenadesAdditionalEffects.OnFlash;
            PlayerNames = null;
            PlayerResize = null;
            GrenadesAdditionalEffects = null;
            base.OnDisabled();
        }
    }
}
