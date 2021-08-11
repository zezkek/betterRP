﻿using Exiled.API.Features;
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
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            Vector3 resize = new Vector3(1, 1, 1);
            if (Plugin.PluginItem.Config.PlayerNamesEnabled && ev.Player != null && ev.Player.Role != RoleType.None && ev.Player.IsHuman)
            {
                if (ev.Player.Role == RoleType.Spectator)
                    ev.Player.Scale = resize;
                else
                {
                    XScale = UnityEngine.Random.Range(0.9f, 1.1f);
                    YScale = UnityEngine.Random.Range(0.9f, 1.1f);
                    resize = new Vector3(XScale, YScale);
                    ev.Player.Scale = resize;
                }
            }
        }
    }
}