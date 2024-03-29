﻿#define TOC
namespace BetterRP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BetterRP.API;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using UnityEngine;
    using MapEv = Exiled.Events.Handlers.Map;
    using PlayerEv = Exiled.Events.Handlers.Player;
    using SvEv = Exiled.Events.Handlers.Server;
    using WarheadEv = Exiled.Events.Handlers.Warhead;

    public class Plugin : Plugin<Config>
    {
        private static readonly Plugin InstanceValue = new Plugin();
        private readonly List<MEC.CoroutineHandle> coroutines = new List<MEC.CoroutineHandle>();
        private PlayerNames playerNames;
        private PlayerResize playerResize;
        private GrenadesAdditionalEffects grenadesAdditionalEffects;
        private StartsBlackout startsBlackout;
        private TeslaDisable teslaDisable;
        private UserInterface.UserInterface userInterface;
        private CommandMethods commandMethods;

        private Plugin()
        {
        }

        public static List<string> SurnameBase { get; set; } = new List<string> { };

        public static List<string> CallSignsBase { get; set; } = new List<string> { };

        public static List<string> SponsorsNamesBase { get; set; } = new List<string> { };

        public static Plugin Instance => InstanceValue;

        public override string Name { get; } = "BetterRP";

        public override string Author { get; } = ".fkn_goose & Mydak";

        public override string Prefix => "BetterRP";

        public override Version Version => new Version(1, 3, 1);

        public void AddCoroutine(MEC.CoroutineHandle coroutineHandle) => this.coroutines.Add(coroutineHandle);

        public void NewCoroutine(IEnumerator<float> coroutine, MEC.Segment segment = MEC.Segment.Update) => this.coroutines.Add(MEC.Timing.RunCoroutine(coroutine, segment));

#if PlayerLeavle
        private PlayerLeave replaceSCP;
#endif

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            if (!this.Config.IsEnabled)
            {
                Log.Debug("Better RP is disabled via configs. It will not be loaded.");
                return;
            }

            this.commandMethods = new CommandMethods(this);
            this.playerNames = new PlayerNames();
            this.playerResize = new PlayerResize();
            this.grenadesAdditionalEffects = new GrenadesAdditionalEffects(this);
            this.teslaDisable = new TeslaDisable(this);
            this.userInterface = new UserInterface.UserInterface();
#if PlayerLeave
            this.replaceSCP = new PlayerLeave();
#endif
            this.startsBlackout = new StartsBlackout(this);

            PlayerEv.ChangingRole += this.playerNames.OnChangingRole;
            PlayerEv.ChangingRole += this.playerResize.OnChangingRoleEventArgs;
            PlayerEv.Dying += this.playerResize.OnDying;
#if PlayerLeave
            PlayerEv.Left += this.replaceSCP.OnDisconnect;
#endif
            SvEv.WaitingForPlayers += this.playerNames.OnWaiting;
            SvEv.RoundStarted += this.startsBlackout.OnRoundStarted;
            SvEv.RoundStarted += this.userInterface.OnRoundStarted;
            MapEv.ExplodingGrenade += this.grenadesAdditionalEffects.OnExplodingGrenade;
            PlayerEv.Hurting += this.grenadesAdditionalEffects.OnHurting;
            PlayerEv.TriggeringTesla += this.teslaDisable.OnTriggeringTesla;
#if TOC
            SvEv.RoundStarted += BetterRP.Commands.TOC.TOC.OnRoundStarted;
            SvEv.RespawningTeam += BetterRP.Commands.TOC.TOC.OnTeamSpawn;
            WarheadEv.Detonated += BetterRP.Commands.TOC.TOC.OnWarheadDetonated;
#endif

            this.LoadNames();
            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            if (this.coroutines != null && this.coroutines.Count > 0)
            {
                MEC.Timing.KillCoroutines(this.coroutines.ToArray());
            }

            PlayerEv.ChangingRole -= this.playerNames.OnChangingRole;
            PlayerEv.ChangingRole -= this.playerResize.OnChangingRoleEventArgs;
            PlayerEv.Dying -= this.playerResize.OnDying;
#if PlayerLeave
            PlayerEv.Left -= this.replaceSCP.OnDisconnect;
#endif
            SvEv.RoundStarted -= this.startsBlackout.OnRoundStarted;
            SvEv.RoundStarted -= this.userInterface.OnRoundStarted;

            SvEv.WaitingForPlayers -= this.playerNames.OnWaiting;
            MapEv.ExplodingGrenade -= this.grenadesAdditionalEffects.OnExplodingGrenade;
            PlayerEv.Hurting -= this.grenadesAdditionalEffects.OnHurting;
            PlayerEv.TriggeringTesla -= this.teslaDisable.OnTriggeringTesla;

#if TOC
            SvEv.RoundStarted -= BetterRP.Commands.TOC.TOC.OnRoundStarted;
            SvEv.RespawningTeam -= BetterRP.Commands.TOC.TOC.OnTeamSpawn;
            WarheadEv.Detonated -= BetterRP.Commands.TOC.TOC.OnWarheadDetonated;
#endif

            this.playerNames = null;
            this.playerResize = null;
            this.grenadesAdditionalEffects = null;
            this.startsBlackout = null;
            this.teslaDisable = null;
            this.userInterface = null;
            this.commandMethods = null;
            SurnameBase = new List<string> { };
            CallSignsBase = new List<string> { };
            SponsorsNamesBase = new List<string> { };
            base.OnDisabled();

            if (this.Config.IsEnabled)
            {
                Log.Debug("BetterRP disabled");
                base.OnDisabled();
            }
        }

        private void LoadNames()
        {
            try
            {
                if (!Directory.Exists(Path.Combine(Instance.Config.ItemConfigFolder, "Names/")))
                {
                    Log.Warn("Folder with names files not found. Creating...");
                    Directory.CreateDirectory(Instance.Config.ItemConfigFolder);
                }

                Log.Info("Creating path for names folder...");
                string surnameFilePath = Path.Combine(Instance.Config.ItemConfigFolder, "Names/Surnames.txt");
                string callSignsFilePath = Path.Combine(Instance.Config.ItemConfigFolder, "Names/CallSigns.txt");
                string sponsorsNamesFilePath = Path.Combine(Instance.Config.ItemConfigFolder, "Names/SponsorsNames.txt");

                Log.Debug($"{surnameFilePath}");
                if (!File.Exists(surnameFilePath))
                {
                    Log.Warn("File with surnames not found. Creating...");
                    File.WriteAllText(surnameFilePath, string.Empty);
                }
                else
                {
                    Log.Info("File with surnames has been found. Reading...");
                    SurnameBase = File.ReadAllLines(surnameFilePath).ToList();
                    Log.Info($"Total surnames count: {SurnameBase.Count()}");
                }

                Log.Debug($"{callSignsFilePath}");
                if (!File.Exists(callSignsFilePath))
                {
                    Log.Warn("File with callsigns found. Creating...");
                    File.WriteAllText(callSignsFilePath, string.Empty);
                }
                else
                {
                    Log.Info("File with callsigns has been found. Reading...");
                    CallSignsBase = File.ReadAllLines(callSignsFilePath).ToList();
                    Log.Info($"Total callsigns count: {CallSignsBase.Count()}");
                }

                Log.Debug($"{sponsorsNamesFilePath}");
                if (!File.Exists(sponsorsNamesFilePath))
                {
                    Log.Warn("File with callsigns found. Creating...");
                    File.WriteAllText(sponsorsNamesFilePath, string.Empty);
                }
                else
                {
                    Log.Info("File with callsigns has been found. Reading...");
                    SponsorsNamesBase = File.ReadAllLines(sponsorsNamesFilePath).ToList();
                    Log.Info($"Total callsigns count: {SponsorsNamesBase.Count()}");
                }
            }
            catch (Exception e)
            {
                Log.Error($"An error occured while parsing names:\n{e}");
                throw;
            }
        }
    }
}
