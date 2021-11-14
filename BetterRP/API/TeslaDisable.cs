namespace BetterRP.API
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

    /// <summary>
    /// Prevents all teslas to hurt person, which has specific item.
    /// </summary>
    public class TeslaDisable
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeslaDisable"/> class.
        /// </summary>
        /// <param name="plugin">Used to take config params.</param>
        public TeslaDisable(Plugin plugin)
        {
            this.plugin = plugin;
        }

        /// <summary>
        /// Allows or not a tesla to work, if any player has started to trigger it.
        /// </summary>
        /// <param name="ev">TriggeringTeslaEventArgs.</param>
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (!Plugin.Instance.Config.IsCardDisbaleTesla)
            {
                ev.IsTriggerable = true;
            }
            else
            {
                foreach (ItemType iscard in Plugin.Instance.Config.Cards)
                {
                    if (ev.Player.HasItem(iscard))
                    {
                        ev.IsTriggerable = false;
                    }
                }

                if (ev.IsTriggerable)
                {
                    List<Player> players = Player.List.ToList();
                    foreach (Player has_card in players.Where(x => Vector3.Distance(x.Position, ev.Player.Position) <= Plugin.Instance.Config.DistanceToDisable))
                    {
                        foreach (ItemType iscard in Plugin.Instance.Config.Cards)
                        {
                            if (has_card.HasItem(iscard))
                            {
                                ev.IsTriggerable = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
