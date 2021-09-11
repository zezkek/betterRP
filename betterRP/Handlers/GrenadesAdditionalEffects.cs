using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using CustomPlayerEffects;
using UnityEngine;
using Mirror;
using betterRP;

namespace betterRP
{
    public class GrenadesAdditionalEffects
    {
        public GrenadesAdditionalEffects GrenadeHandler;
        private readonly Plugin plugin;
        public GrenadesAdditionalEffects(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnFlash(ExplodingGrenadeEventArgs ev)
        {
            if (ev.IsFrag) return;
            if(plugin.Config.Debug)
                Log.Debug("Event OnFlash has been taken");
            foreach (Player player in Player.List)
            {
                if (plugin.Config.Debug)
                    Log.Debug($"Distance Difference between grenade {ev.Grenade.transform.position}" +
                        $" and possible target {player.Nickname}:" +
                        $" {Vector3.Distance(ev.Grenade.transform.position, player.Position)}");
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
                            if (plugin.Config.Debug)
                                Log.Debug("Giving flash effects for 939.53");
                            player.EnableEffect(EffectType.Flashed, duration: 6);
                            player.EnableEffect(EffectType.Ensnared, duration: 6);
                            break;
                        case RoleType.Scp93989:
                            if (plugin.Config.Debug)
                                Log.Debug("Giving flash effects for 939.89");
                            player.EnableEffect(EffectType.Flashed, duration: 6);
                            player.EnableEffect(EffectType.Ensnared, duration: 6);
                            break;
                        default:
                            if (plugin.Config.Debug)
                                Log.Debug("Giving flash effects for others");
                            player.EnableEffect(EffectType.Blinded, duration: 12);
                            player.EnableEffect(EffectType.Deafened, duration: 12);
                            player.EnableEffect(EffectType.Concussed, duration: 30);
                            break;
                    }
                }
            }
        }

        public void OnDamage(HurtingEventArgs ev)
        {
            if (plugin.Config.Debug)
                Log.Debug("Event OnDamage has been taken");
            if (ev.DamageType != DamageTypes.Grenade) return;
            if (plugin.Config.Debug)
                Log.Debug("First check passed");
            switch (ev.Target.Role)
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
                    if (plugin.Config.Debug)
                        Log.Debug("Giving frag effects for 939.53");
                    ev.Target.EnableEffect(EffectType.Blinded, duration: 6);
                    ev.Target.EnableEffect(EffectType.Ensnared, duration: 6);
                    break;
                case RoleType.Scp93989:
                    if (plugin.Config.Debug)
                        Log.Debug("Giving frag effects for 939.89");
                    ev.Target.EnableEffect(EffectType.Blinded, duration: 6);
                    ev.Target.EnableEffect(EffectType.Ensnared, duration: 6);
                    break;
                default:
                    if (plugin.Config.Debug)
                        Log.Debug("Giving frag effects for others");
                    ev.Target.EnableEffect(EffectType.Deafened, duration: 12);
                    ev.Target.EnableEffect(EffectType.Concussed, duration: 30);
                    break;
            }
        }
    }
}
