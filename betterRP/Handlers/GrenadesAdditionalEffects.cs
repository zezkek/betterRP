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
        //ToFix: Улетает в замкнутый цикл
        public void OnFlash(ReceivingEffectEventArgs ev)
        {
            if(ev.Player.GetEffect(EffectType.Blinded).Enabled)
            {
                switch (ev.Player.Role)
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
                        ev.Player.EnableEffect(EffectType.Blinded, duration: 6);
                        ev.Player.EnableEffect(EffectType.Ensnared, duration: 6);
                        break;
                    case RoleType.Scp93989:
                        ev.Player.EnableEffect(EffectType.Blinded, duration: 6);
                        ev.Player.EnableEffect(EffectType.Ensnared, duration: 6);
                        break;
                    default:
                        ev.Player.EnableEffect(EffectType.Blinded, duration: 6);
                        ev.Player.EnableEffect(EffectType.Deafened, duration: 15);
                        ev.Player.EnableEffect(EffectType.Concussed, duration: 30);
                        break;
                }
            }
        }
        public void OnDamage(HurtingEventArgs ev)
        {
            Log.Debug("Event has been taken");
            if (ev.DamageType != DamageTypes.Grenade) return;
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
                    Log.Debug("Giving effects for 939.53");
                    ev.Target.EnableEffect(EffectType.Blinded, duration: 6);
                    ev.Target.EnableEffect(EffectType.Ensnared, duration: 6);
                    break;
                case RoleType.Scp93989:
                    Log.Debug("Giving effects for 939.89");
                    ev.Target.EnableEffect(EffectType.Blinded, duration: 6);
                    ev.Target.EnableEffect(EffectType.Ensnared, duration: 6);
                    break;
                default:
                    Log.Debug("Giving effects for others");
                    ev.Target.EnableEffect(EffectType.Deafened, duration: 15);
                    ev.Target.EnableEffect(EffectType.Concussed, duration: 30);
                    break;
            }
        }
    }
}
