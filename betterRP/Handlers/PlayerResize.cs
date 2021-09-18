using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;
using betterRP;

namespace RolePlayNames.Handlers
{
    public class PlayerResize
    {
        float XScale, YScale;
        public PlayerResize ResizeHandler;
        public void OnChangedRoleEventArgs(ChangedRoleEventArgs ev)
        {
            if (ev.Player.IsHuman && Plugin.PluginItem.Config.PlayerNamesEnabled && ev.Player != null && ev.Player.Role != RoleType.None)
            {
                Vector3 resize = new Vector3(1, 1, 1);
                XScale = UnityEngine.Random.Range(0.9f, 1.1f);
                YScale = UnityEngine.Random.Range(0.9f, 1.1f);
                ev.Player.Scale = new Vector3(XScale, YScale, 1);
            }
            else
                ev.Player.Scale = new Vector3(1, 1, 1);
        }
        public void OnDying(DyingEventArgs ev)
        {
            ev.Target.Scale = new Vector3(1, 1, 1);
        }
    }
}
