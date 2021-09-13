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
    public class StartsBlackout
    {
        public StartsBlackout BlackoutHandler;
        private readonly Plugin plugin;
        public StartsBlackout(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnRoundStarted()
        {
            plugin.NewCoroutine(BlackoutDealy());
        }

        public IEnumerator<float> BlackoutDealy()
        {
            yield return MEC.Timing.WaitForSeconds(plugin.Config.BlackoutDelay);
            Map.TurnOffAllLights(5, isHeavyContainmentZoneOnly: false);
            yield break;
        }
    }
}
