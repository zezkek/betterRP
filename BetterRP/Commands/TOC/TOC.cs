// #define ShowHints
namespace BetterRP.Commands.TOC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using Exiled.Permissions.Extensions;
    using MEC;
    using UnityEngine;

    /// <summary>
    /// Tactical Operations Center.
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class TOC : ParentCommand
    {
#pragma warning disable SA1401 // Fields should be private
        public static Vector3 MtfPoint;
        public static Vector3 ChiPoint;
        public static float OnEvacuateCooldownMTF;
        public static float OnSupportCooldownMTF;
        public static float OnEvacuateCooldownCHI;
        public static float OnSupportCooldownCHI;
        public static float OnTheWayMTF;
        public static float OnTheWayCHI;
#pragma warning restore SA1401 // Fields should be private

        private static readonly int[,] EvacQueryMTF = new int[,]
{
            { (int)RoleType.Scp173, 0 },
            { (int)RoleType.Scp049, 0 },
            { (int)RoleType.Scp096, 0 },
            { (int)RoleType.Scp93989, 0 },
            { (int)RoleType.Scp93953, 0 },
            { (int)RoleType.Scientist, 0 },
            { (int)RoleType.NtfCaptain, 0 },
            { (int)RoleType.NtfSpecialist, 0 },
            { (int)RoleType.NtfSergeant, 0 },
            { (int)RoleType.NtfPrivate, 0 },
            { (int)RoleType.Scientist, 1 },
            { (int)RoleType.NtfCaptain, 1 },
            { (int)RoleType.NtfSpecialist, 1 },
            { (int)RoleType.NtfSergeant, 1 },
            { (int)RoleType.NtfPrivate, 1 },
            { (int)RoleType.ChaosMarauder, 1 },
            { (int)RoleType.ChaosRepressor, 1 },
            { (int)RoleType.ChaosRifleman, 1 },
            { (int)RoleType.ChaosConscript, 1 },
            { (int)RoleType.ClassD, 1 },
            { (int)RoleType.Scp0492, 0 },
};

        private static readonly int[,] EvacQueryCHI = new int[,]
        {
            { (int)RoleType.Scp173, 0 },
            { (int)RoleType.Scp049, 0 },
            { (int)RoleType.Scp096, 0 },
            { (int)RoleType.Scp93989, 0 },
            { (int)RoleType.Scp93953, 0 },
            { (int)RoleType.ChaosMarauder, 0 },
            { (int)RoleType.ChaosRepressor, 0 },
            { (int)RoleType.ChaosRifleman, 0 },
            { (int)RoleType.ChaosConscript, 0 },
            { (int)RoleType.ChaosMarauder, 1 },
            { (int)RoleType.ChaosRepressor, 1 },
            { (int)RoleType.ChaosRifleman, 1 },
            { (int)RoleType.ChaosConscript, 1 },
            { (int)RoleType.ClassD, 1 },
            { (int)RoleType.Scientist, 1 },
            { (int)RoleType.NtfCaptain, 1 },
            { (int)RoleType.NtfSpecialist, 1 },
            { (int)RoleType.NtfSergeant, 1 },
            { (int)RoleType.NtfPrivate, 1 },
            { (int)RoleType.ClassD, 0 },
            { (int)RoleType.Scp0492, 0 },
        };

        private static readonly Dictionary<RoleType, sbyte> PlayerWeight = new Dictionary<RoleType, sbyte>
        {
            { RoleType.Scp173, 3 },
            { RoleType.Scp049, 3 },
            { RoleType.Scp0492, 1 },
            { RoleType.Scp096, 3 },
            { RoleType.Scp93953, 3 },
            { RoleType.Scp93989, 3 },
            { RoleType.Scientist, 1 },
            { RoleType.NtfCaptain, 1 },
            { RoleType.NtfSpecialist, 1 },
            { RoleType.NtfSergeant, 1 },
            { RoleType.NtfPrivate, 1 },
            { RoleType.ChaosMarauder, 1 },
            { RoleType.ChaosRepressor, 1 },
            { RoleType.ChaosRifleman, 1 },
            { RoleType.ChaosConscript, 1 },
            { RoleType.ClassD, 1 },
        };

        private static bool supportCalled;

        /// <summary>
        /// Initializes a new instance of the <see cref="TOC"/> class.
        /// </summary>
        public TOC() => this.LoadGeneratedCommands();

        /// <summary>
        /// Variants of result for TOC commands.
        /// </summary>
        internal enum Subcommand : byte
        {
#pragma warning disable SA1602 // Enumeration items should be documented
            Evacuate = 1,
            Support = 2,
            Error = 0,
#pragma warning restore SA1602 // Enumeration items should be documented
        }

        /// <inheritdoc/>
        public override string Command { get; } = "toc";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = { };

        /// <inheritdoc/>
        public override string Description { get; } = "Центр Тактических Операций. Отвечает за снабжение подкреплений и эвакуации по запросу";

        /// <summary>
        /// Adds delay to call ability, when round starts.
        /// </summary>
        public static void OnRoundStarted()
        {
            MtfPoint = new Vector3(178, 992.5f, -60);
            ChiPoint = new Vector3(5, 988.5f, -58);
            OnEvacuateCooldownMTF = Time.time + 60;
            OnSupportCooldownMTF = Time.time + 60;
            OnTheWayMTF = Time.time;
            OnEvacuateCooldownCHI = Time.time + 60;
            OnSupportCooldownCHI = Time.time + 60;
            OnTheWayCHI = Time.time;
        }

        /// <summary>
        /// Adds delay to call ability, when new command spawns.
        /// </summary>
        /// <param name="ev">RespawningTeamEventArgs.</param>
        public static void OnTeamSpawn(RespawningTeamEventArgs ev)
        {
            if (ev.IsAllowed)
            {
                if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
                {
                    Log.Debug($"{ev.NextKnownTeam} has been spawned without sound.\nPrevious Evacuation/Support cooldown of {ev.NextKnownTeam}: {OnEvacuateCooldownCHI}/{OnSupportCooldownCHI}.", Plugin.Instance.Config.Debug);
                    OnEvacuateCooldownCHI = Time.time + Plugin.Instance.Config.CooldownEv;
                    OnSupportCooldownCHI = Time.time + Plugin.Instance.Config.CooldownSup;
                    Log.Debug($"New Evacuation/Support cooldown of {ev.NextKnownTeam}: {OnEvacuateCooldownCHI}/{OnSupportCooldownCHI}. Calling Evacuate coroutine with force command.", Plugin.Instance.Config.Debug);
                    Plugin.Instance.NewCoroutine(Evacuate(Team.CHI, true));
                }
                else if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox)
                {
                    Log.Debug($"{ev.NextKnownTeam} has been spawned without sound.\nPrevious Evacuation/Support cooldown of {ev.NextKnownTeam}: {OnEvacuateCooldownMTF}/{OnSupportCooldownMTF}.", Plugin.Instance.Config.Debug);
                    OnEvacuateCooldownMTF = Time.time + Plugin.Instance.Config.CooldownEv;
                    OnSupportCooldownMTF = Time.time + Plugin.Instance.Config.CooldownSup;
                    Log.Debug($"New Evacuation/Support cooldown of {ev.NextKnownTeam}: {OnEvacuateCooldownMTF}/{OnSupportCooldownMTF}. Calling Evacuate coroutine with force command.", Plugin.Instance.Config.Debug);
                    Plugin.Instance.NewCoroutine(Evacuate(Team.MTF, true));
                    if (supportCalled)
                    {
                        supportCalled = false;
                        List<RoleType> orderOfRolesToChange = new List<RoleType> { RoleType.NtfPrivate, RoleType.NtfSergeant };
                        short countToChange = 1;
                        MEC.Timing.CallDelayed(Timing.WaitForOneFrame, () =>
                        {
                            foreach (var role in orderOfRolesToChange)
                            {
                                if (countToChange <= 0)
                                {
                                    break;
                                }

                                foreach (var player in ev.Players)
                                {
                                    if (countToChange <= 0)
                                    {
                                        break;
                                    }

                                    if (role == player.Role)
                                    {
                                        player.SetRole(RoleType.NtfSpecialist);
                                        countToChange--;
                                    }
                                }
                            }
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Kills everyone at the Surface, if GodMod isn't enabled.
        /// </summary>
        public static void OnWarheadDetonated()
        {
            foreach (var player in Player.List.Where(p => p.Role != RoleType.Spectator && p.Role != RoleType.Tutorial && p.Role != RoleType.None && !p.IsGodModeEnabled))
            {
                player.Kill(DamageTypes.Nuke);
            }
        }

        public static void OnEndingRound(EndingRoundEventArgs ev)
        {
            if (Warhead.IsInProgress && (ev.ClassList.mtf_and_guards > 0 || ev.ClassList.scientists > 0))
            {
                ev.IsRoundEnded = false;
            }
            else if (Warhead.IsDetonated)
            {
                ev.LeadingTeam = LeadingTeam.ChaosInsurgency;
                ev.IsRoundEnded = true;
            }
            else if (ev.ClassList.scps_except_zombies > 0 && ev.ClassList.chaos_insurgents > 0)
            {
                ev.IsRoundEnded = false;
            }
        }

        /// <inheritdoc/>
        public override void LoadGeneratedCommands()
        {
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerRequester = Player.Get((sender as CommandSender)?.SenderId);
            Log.Debug($"Player {playerRequester.Nickname} tries execute command TOC", Plugin.Instance.Config.Debug);
            CommandMethods.LogCommandUsed((CommandSender)sender, CommandMethods.FormatArguments(arguments, 0));
            if (!((CommandSender)sender).CheckPermission("betterRP.toc"))
            {
                Log.Debug($"Error. Missed required permissions.", Plugin.Instance.Config.Debug);
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: В ДОСТУПЕ ОТКАЗАНО</color>";
                return false;
            }

            if (!Round.IsStarted)
            {
                Log.Debug($"Error. Round hasn't started/", Plugin.Instance.Config.Debug);
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: СЕССИЯ НЕ НАЧАЛАСЬ</color>";
                return false;
            }

            if (arguments.Count != 1)
            {
                Log.Debug($"Error. Count of args({arguments.Count}) too low.", Plugin.Instance.Config.Debug);
                response = string.Join(
                    "\n",
                    "\n<color=#C1B5B5>СТАТУС: ИНФО</color>",
                    "<color=#C1B5B5>ИСПОЛЬЗОВАНИЕ КОМАНДЫ:</color><color=#990000>toc [ЗАДАЧА]</color>",
                    "<color=#C1B5B5>ЗАДАЧИ:</color>",
                    "<color=#990000>evacuate</color> <color=#FFD966>(СИНОНИМЫ: evac)</color> - Запрос эвакуационного транспорта.",
                    "<color=#990000>support</color> <color=#FFD966>(СИНОНИМЫ: sup)</color> - Запрос группы поддержки.");
                return false;
            }

            if (playerRequester.Team != Team.MTF && playerRequester.Team != Team.CHI)
            {
                Log.Debug($"Error. {playerRequester.Team} can't execute toc command", Plugin.Instance.Config.Debug);
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ДОСТУП ОГРАНИЧЕН ДЛЯ ТЕКУЩЕЙ РОЛИ</color>";
                return true;
            }

            if (playerRequester.CurrentRoom.Type != RoomType.Surface)
            {
                Log.Debug($"Error. Sender isn't at the Surface", Plugin.Instance.Config.Debug);
                response = string.Join(
                    "\n",
                    "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>",
                    "<color=#C1B5B5>ВЫВОД: НЕТ СИГНАЛА</color>");
                return true;
            }

            var args = arguments.Array;
            {
                Subcommand subcommand = Subcommand.Error;
                Log.Debug($"Parsing subcoomand. Second arg: {args[1]}", Plugin.Instance.Config.Debug);
                if (args[1].ToLower().Equals("evac") || args[1].ToLower().Equals("evacuate"))
                {
                    subcommand = Subcommand.Evacuate;
                    Log.Debug($"Evacuation subcommand has been found.", Plugin.Instance.Config.Debug);
                }

                if (args[1].ToLower().Equals("sup") || args[1].ToLower().Equals("support"))
                {
                    subcommand = Subcommand.Support;
                }

                Log.Debug($"Last entered MTF/CHI time: {OnTheWayMTF}/{OnTheWayCHI}. Current time: {Time.time}", Plugin.Instance.Config.Debug);
                if ((OnTheWayMTF > Time.time && playerRequester.Team == Team.MTF) ||
                    (OnTheWayCHI > Time.time && playerRequester.Team == Team.CHI))
                {
                    Log.Debug($"Skipping. Command for {playerRequester.Team} is at work", Plugin.Instance.Config.Debug);
                    response = "\"Говорит TOC. Транспорт уже в пути. Конец связи.\"";
                    return true;
                }

                Log.Debug($"Last reload MTF sup/evac time:{OnSupportCooldownMTF}/{OnEvacuateCooldownMTF}. CHI sup/evc time: {OnSupportCooldownCHI}/{OnEvacuateCooldownCHI}. Current time: {Time.time}", Plugin.Instance.Config.Debug);
                if (((OnEvacuateCooldownMTF > Time.time || OnSupportCooldownMTF > Time.time) && playerRequester.Team == Team.MTF) ||
                    ((OnEvacuateCooldownCHI > Time.time || OnSupportCooldownCHI > Time.time) && playerRequester.Team == Team.CHI))
                {
                    Log.Debug($"Skipping. Command for {playerRequester.Team} is in reload", Plugin.Instance.Config.Debug);
                    response = "\"Говорит TOC. Транспорт ещё не готов. Ждите. Конец связи.\"";
                    return true;
                }

                Log.Debug($"Warhead.IsInProgress: {Warhead.IsInProgress}. Warhead.DetonationTimer: {Warhead.DetonationTimer} MTF EST time: {(Plugin.Instance.Config.TimeInTheWay / 2) + 36f}. Current time: {Time.time}", Plugin.Instance.Config.Debug);
                if (playerRequester.Team == Team.MTF && Warhead.IsInProgress && Warhead.DetonationTimer < (Plugin.Instance.Config.TimeInTheWay / 2) + 36f)
                {
                    Log.Debug(string.Join(
                        string.Empty,
                        $"Skipping. Too close to warhead detonation: {Warhead.DetonationTimer}. ",
                        $"Required time for entering: {(Plugin.Instance.Config.TimeInTheWay / 2) + 36f}",
                        Plugin.Instance.Config.Debug));

                    response = "\"Говорит TOC. Эвакуация невозможна, вы в зоне поражения Альфа-боеголовки. Конец связи.\"";
                    return true;
                }

                Log.Debug($"Calling {subcommand} for {playerRequester.Team}", Plugin.Instance.Config.Debug);
                switch (subcommand)
                {
                    case Subcommand.Evacuate:
                        if (playerRequester.Team == Team.CHI)
                        {
                            Plugin.Instance.NewCoroutine(Evacuate(Team.CHI));
                        }
                        else
                        {
                            Plugin.Instance.NewCoroutine(Evacuate(Team.MTF));
                        }

                        break;
                    case Subcommand.Support:
                        if (playerRequester.Team == Team.CHI)
                        {
                            Plugin.Instance.NewCoroutine(this.WaitForSupport(Team.CHI));
                        }
                        else
                        {
                            Plugin.Instance.NewCoroutine(this.WaitForSupport(Team.MTF));
                        }

                        break;
                    case Subcommand.Error:
                    default:
                        response = string.Join(
                            "\n",
                            "\n<color=#C1B5B5>СТАТУС:</color> <color=#990000>ОШИБКА</color>",
                            "<color=#C1B5B5>ИСПОЛЬЗОВАНИЕ КОМАНДЫ:</color><color=#990000>toc [ЗАДАЧА]</color>",
                            "<color=#C1B5B5>ЗАДАЧИ:</color>",
                            "<color=#990000>evacuate</color> <color=#FFD966>(СИНОНИМЫ: evac)</color> - Запрос эвакуационного транспорта.",
                            "<color=#990000>support</color> <color=#FFD966>(СИНОНИМЫ: sup)</color> - Запрос группы поддержки.",
                            "<color=#C1B5B5>ПРИМЕЧАНИЕ: На обратном пути, транспорт поддержки заберёт ожидающих эвакуации.</color>");
                        return false;
                }

                if (playerRequester.Team == Team.CHI)
                {
                    Log.Debug($"Executing for CHI command. Defined TimeInTheWay: {(subcommand == Subcommand.Evacuate ? Plugin.Instance.Config.TimeInTheWay / 2 : Plugin.Instance.Config.TimeInTheWay)}", Plugin.Instance.Config.Debug);
                    OnTheWayCHI = Time.time + (subcommand == Subcommand.Evacuate ? Plugin.Instance.Config.TimeInTheWay / 2 : Plugin.Instance.Config.TimeInTheWay) + 14f;
                    Log.Debug($"New entering time for CHI team: {OnTheWayCHI}. Calling Evacuate coroutine", Plugin.Instance.Config.Debug);
                }
                else
                {
                    Log.Debug($"Executing for MTF command. Defined TimeInTheWay: {(subcommand == Subcommand.Evacuate ? Plugin.Instance.Config.TimeInTheWay / 2 : Plugin.Instance.Config.TimeInTheWay)}", Plugin.Instance.Config.Debug);
                    OnTheWayMTF = Time.time + (subcommand == Subcommand.Evacuate ? Plugin.Instance.Config.TimeInTheWay / 2 : Plugin.Instance.Config.TimeInTheWay) + 19f;
                    Log.Debug($"New entering time for MTF team: {OnTheWayMTF}. Calling Evacuate coroutine", Plugin.Instance.Config.Debug);
                }

                response = $"\"Говорит TOC. Запрос получен. Конец связи.\"";
                return true;
            }
        }

        private static IEnumerator<float> Evacuate(Team team, bool force = false)
        {
            Log.Debug($"Evacuation coroutine has started", Plugin.Instance.Config.Debug);
            List<Player> readyToEvac = new List<Player> { };
            Vector3 point = team == Team.MTF ? TOC.MtfPoint : TOC.ChiPoint;
            Log.Debug($"Current evacuation point of team {team}: {point}", Plugin.Instance.Config.Debug);
            if (!force)
            {
                Log.Debug($"Usual call deteced. Waiting for {Plugin.Instance.Config.TimeInTheWay / 2}", Plugin.Instance.Config.Debug);
                yield return Timing.WaitForSeconds(Plugin.Instance.Config.TimeInTheWay / 2);
                Log.Debug($"Usual call deteced. Waiting for end of {team} transport animation: {(team == Team.MTF ? 18 : 13)}", Plugin.Instance.Config.Debug);
                Respawn.PlayEffect(team == Team.MTF ? RespawnEffectType.SummonNtfChopper : RespawnEffectType.SummonChaosInsurgencyVan);
                yield return Timing.WaitForSeconds(team == Team.MTF ? 18 : 13);
            }
            else
            {
                Log.Debug($"Forced call detected", Plugin.Instance.Config.Debug);
            }

            if (!Warhead.IsInProgress)
            {
                readyToEvac = Player.List.Where(x => x.Team != team || (x.Team == team && x.IsCuffed)).ToList();
                Log.Debug($"Warhead hasn't been activated. Count of targets: {readyToEvac.Count}", Plugin.Instance.Config.Debug);
            }
            else
            {
                readyToEvac = Player.List.Where(x => Vector3.Distance(x.Position, point) <= Plugin.Instance.Config.EvacuationDistance).ToList();
                Log.Debug($"Warhead has been activated. Count of targets: {readyToEvac.Count}", Plugin.Instance.Config.Debug);
            }

            int[,] evacQuery = team == Team.MTF ? EvacQueryMTF : EvacQueryCHI;

            Log.Debug($"Taken evacuation queary for {team}.", Plugin.Instance.Config.Debug);
            sbyte capacity = 10;
            Log.Debug($"Current vehicle capacity: {capacity}.", Plugin.Instance.Config.Debug);
            List<Pickup> itemsToEvac = Map.Pickups.ToList().Where(
                x => Vector3.Distance(x.Position, point) <= Plugin.Instance.Config.EvacuationDistance &&
                (team == Team.MTF ? Plugin.Instance.Config.MTFEvacItems : Plugin.Instance.Config.CHIEvacItems).Contains(x.Type)).ToList();
            Log.Debug($"Trying to evacuating items of {team} team.", Plugin.Instance.Config.Debug);
            foreach (var item in itemsToEvac)
            {
                Log.Debug($"{item.Type} has been evacuated (destroyed).", Plugin.Instance.Config.Debug);
                item.Destroy();
            }

            for (int role = 0; role < evacQuery.GetLength(0); role++)
            {
                foreach (Player ply in readyToEvac.Where(
                    x => x.Role == (RoleType)evacQuery[role, 0] &&
                    x.IsCuffed == Convert.ToBoolean(evacQuery[role, 1]) &&
                    Vector3.Distance(point, x.Position) <= Plugin.Instance.Config.EvacuationDistance))
                {
                    Log.Debug($"Player {ply.Nickname} with {ply.Role} has evac. distance: {Vector3.Distance(ply.Position, point)}. Config distance: {Plugin.Instance.Config.EvacuationDistance}", Plugin.Instance.Config.Debug);
                    if (PlayerWeight[ply.Role] > capacity)
                    {
                        Log.Debug($"Player weight bigger than current trasport capacity. Skipping.", Plugin.Instance.Config.Debug);
                        continue;
                    }

                    capacity -= PlayerWeight[ply.Role];
                    Log.Debug($"Player will be evacuated. New transport capacity: {capacity}.", Plugin.Instance.Config.Debug);
                    if (ply.Team == Team.SCP)
                    {
                        switch (ply.Role)
                        {
                            case RoleType.Scp049:
                                Cassie.Message(Plugin.Instance.Config.ScpEvacuate.Replace("{scpnumber}", "scp 0 4 9"));
                                break;
                            case RoleType.Scp096:
                                Cassie.Message(Plugin.Instance.Config.ScpEvacuate.Replace("{scpnumber}", "scp 0 9 6"));
                                break;
                            case RoleType.Scp173:
                                Cassie.Message(Plugin.Instance.Config.ScpEvacuate.Replace("{scpnumber}", "scp 1 7 3"));
                                break;
                            case RoleType.Scp93953:
                                Cassie.Message(Plugin.Instance.Config.ScpEvacuate.Replace("{scpnumber}", "scp 9 3 9"));
                                break;
                            case RoleType.Scp93989:
                                Cassie.Message(Plugin.Instance.Config.ScpEvacuate.Replace("{scpnumber}", "scp 9 3 9"));
                                break;
                        }
                    }

                    ply.ClearInventory();
                    ply.Ammo.Clear();
                    Log.Debug($"Player inventory has been cleared.", Plugin.Instance.Config.Debug);
                    ply.SetRole(RoleType.Spectator);
                    Log.Debug($"Player has been moved into spectators.", Plugin.Instance.Config.Debug);
                }
            }

            if (!force)
            {
                if (team == Team.CHI)
                {
                    Log.Debug($"Previous Evacuation/Support cooldown of {team}: {OnEvacuateCooldownCHI}/{OnSupportCooldownCHI}.", Plugin.Instance.Config.Debug);
                    OnEvacuateCooldownCHI = Time.time + Plugin.Instance.Config.CooldownEv;
                    OnSupportCooldownCHI = Time.time + Plugin.Instance.Config.CooldownSup;
                    Log.Debug($"New Evacuation/Support cooldown of {team}: {OnEvacuateCooldownCHI}/{OnSupportCooldownCHI}.", Plugin.Instance.Config.Debug);
                }
                else
                {
                    Log.Debug($"Previous Evacuation/Support cooldown of {team}: {OnEvacuateCooldownMTF}/{OnSupportCooldownMTF}.", Plugin.Instance.Config.Debug);
                    OnEvacuateCooldownMTF = Time.time + Plugin.Instance.Config.CooldownEv;
                    OnSupportCooldownMTF = Time.time + Plugin.Instance.Config.CooldownSup;
                    Log.Debug($"New Evacuation/Support cooldown of {team}: {OnEvacuateCooldownMTF}/{OnSupportCooldownMTF}.", Plugin.Instance.Config.Debug);
                }
            }
            else
            {
                Log.Debug($"Evacuation has been forced. Reload time skipped.", Plugin.Instance.Config.Debug);
            }
        }

        private IEnumerator<float> WaitForSupport(Team team)
        {
            Log.Debug($"Support coroutine has started. Current team: {team}. TimeInTheWay(Delay){Plugin.Instance.Config.TimeInTheWay}", Plugin.Instance.Config.Debug);
            yield return Timing.WaitForSeconds(Plugin.Instance.Config.TimeInTheWay);
            if (team == Team.MTF)
            {
                Respawn.PlayEffect(RespawnEffectType.SummonNtfChopper);
                Log.Debug($"Chopper has been called. Waiting for {18f} sec.", Plugin.Instance.Config.Debug);
                yield return Timing.WaitForSeconds(18f);
                supportCalled = true;
                Respawn.ForceWave(Respawning.SpawnableTeamType.NineTailedFox, false);
                Log.Debug($"{team} has been spawned without sound.\nPrevious Evacuation/Support cooldown of {team}: {OnEvacuateCooldownMTF}/{OnSupportCooldownMTF}.", Plugin.Instance.Config.Debug);
                OnEvacuateCooldownMTF = Time.time + Plugin.Instance.Config.CooldownEv;
                OnSupportCooldownMTF = Time.time + Plugin.Instance.Config.CooldownSup;
                Log.Debug($"New Evacuation/Support cooldown of {team}: {OnEvacuateCooldownMTF}/{OnSupportCooldownMTF}. Calling Evacuate coroutine with force command.", Plugin.Instance.Config.Debug);
                Plugin.Instance.NewCoroutine(Evacuate(Team.MTF, true));
            }
            else if (team == Team.CHI)
            {
                Respawn.PlayEffect(RespawnEffectType.SummonChaosInsurgencyVan);
                Log.Debug($"Car has been called. Waiting for {13f} sec.", Plugin.Instance.Config.Debug);
                yield return Timing.WaitForSeconds(13f);
                Respawn.ForceWave(Respawning.SpawnableTeamType.ChaosInsurgency, false);
                Log.Debug($"{team} has been spawned without sound.\nPrevious Evacuation/Support cooldown of {team}: {OnEvacuateCooldownCHI}/{OnSupportCooldownCHI}.", Plugin.Instance.Config.Debug);
                OnEvacuateCooldownCHI = Time.time + Plugin.Instance.Config.CooldownEv;
                OnSupportCooldownCHI = Time.time + Plugin.Instance.Config.CooldownSup;
                Log.Debug($"New Evacuation/Support cooldown of {team}: {OnEvacuateCooldownCHI}/{OnSupportCooldownCHI}. Calling Evacuate coroutine with force command.", Plugin.Instance.Config.Debug);
                Plugin.Instance.NewCoroutine(Evacuate(Team.CHI, true));
            }
        }
    }
}
