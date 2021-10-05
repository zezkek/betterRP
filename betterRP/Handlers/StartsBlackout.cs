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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "<Pending>")]
    public class StartsBlackout
    {
        private readonly Plugin plugin;

        public StartsBlackout(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnRoundStarted()
        {
            this.plugin.NewCoroutine(this.BlackoutDealy());
        }

        public IEnumerator<float> BlackoutDealy()
        {
            yield return MEC.Timing.WaitForSeconds(this.plugin.Config.BlackoutDelay);
            Map.TurnOffAllLights(5, isHeavyContainmentZoneOnly: false);
            yield break;
        }
    }
}
