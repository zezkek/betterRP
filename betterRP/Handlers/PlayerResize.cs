namespace RolePlayNames.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BetterRP;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Mirror;
    using UnityEngine;

    public class PlayerResize
    {
        private float xScale;
        private float yScale;

        public void OnChangingRoleEventArgs(ChangingRoleEventArgs ev)
        {
            if (ev.Player.IsHuman && Plugin.PluginItem.Config.PlayerNamesEnabled && ev.Player != null && ev.NewRole != RoleType.None)
            {
                this.xScale = UnityEngine.Random.Range(0.9f, 1.1f);
                this.yScale = UnityEngine.Random.Range(0.9f, 1.1f);
                ev.Player.Scale = new Vector3(this.xScale, this.yScale, 1);
            }
            else
            {
                ev.Player.Scale = new Vector3(1, 1, 1);
            }
        }

        public void OnDying(DyingEventArgs ev)
        {
            ev.Target.Scale = new Vector3(1, 1, 1);
        }
    }
}
