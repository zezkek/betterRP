namespace BetterRP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BetterRP;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Mirror;
    using UnityEngine;

    public class GrenadesAdditionalEffects
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrenadesAdditionalEffects"/> class.
        /// </summary>
        /// <param name="plugin">Used to take config params.</param>
        public GrenadesAdditionalEffects(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnFlash(ExplodingGrenadeEventArgs ev)
        {
            if (ev.IsFrag)
            {
                return;
            }

            if (this.plugin.Config.Debug)
            {
                Log.Debug("Event OnFlash has been taken");
#pragma warning disable CS0618 // Type or member is obsolete
                Log.Debug($"Count of targets: {ev.TargetsToAffect.Count}");
#pragma warning restore CS0618 // Type or member is obsolete
            }

            foreach (Player player in Player.List)
            {
                if (this.plugin.Config.Debug)
                {
                    Log.Debug($"Distance Difference between grenade {ev.Grenade.transform.position}" +
                        $" and possible target {player.Nickname}:" +
                        $" {Vector3.Distance(ev.Grenade.transform.position, player.Position)}");
                }

                if (Vector3.Distance(ev.Grenade.transform.position, player.Position) <= 10)
                {
                    switch (player.Role)
                    {
                        case RoleType.None:
                            break;
                        case RoleType.Scp173:
                            break;
                        case RoleType.Spectator:
                            break;
                        case RoleType.Scp106:
                            break;
                        case RoleType.Scp079:
                            break;
                        case RoleType.Scp096:
                            break;
                        case RoleType.Tutorial:
                            break;
                        case RoleType.Scp93953:
                        case RoleType.Scp93989:
                            if (this.plugin.Config.Debug)
                            {
                                Log.Debug("Giving flash effects for 939. Distance less than 15");
                            }

                            if (!ev.IsFrag)
                            {
                                player.EnableEffect(EffectType.Flashed, duration: 6);
                            }

                            player.EnableEffect(EffectType.Ensnared, duration: 6);
                            break;
                        default:
                            if (this.plugin.Config.Debug)
                            {
                                Log.Debug("Giving flash effects for others. Distance less than 15");
                            }

                            if (!ev.IsFrag)
                            {
                                player.EnableEffect(EffectType.Flashed, duration: 6);
                            }

                            player.EnableEffect(EffectType.Deafened, duration: 12);
                            player.EnableEffect(EffectType.Concussed, duration: 30);
                            break;
                    }
                }
                else if (Vector3.Distance(ev.Grenade.transform.position, player.Position) <= 25)
                {
                    switch (player.Role)
                    {
                        case RoleType.None:
                            break;
                        case RoleType.Scp173:
                            break;
                        case RoleType.Spectator:
                            break;
                        case RoleType.Scp106:
                            break;
                        case RoleType.Scp079:
                            break;
                        case RoleType.Scp096:
                            break;
                        case RoleType.Tutorial:
                            break;
                        case RoleType.Scp93953:
                        case RoleType.Scp93989:
                            if (this.plugin.Config.Debug)
                            {
                                Log.Debug("Giving flash effects for 939. Distance less than 25");
                            }

                            if (!ev.IsFrag)
                            {
                                player.EnableEffect(EffectType.Flashed, duration: 3);
                            }

                            player.EnableEffect(EffectType.Ensnared, duration: 3);
                            break;
                        default:
                            if (this.plugin.Config.Debug)
                            {
                                Log.Debug("Giving flash effects for others. Distance less than 25");
                            }

                            player.EnableEffect(EffectType.Deafened, duration: 6);
                            player.EnableEffect(EffectType.Concussed, duration: 15);
                            break;
                    }
                }
            }
        }
    }
}
