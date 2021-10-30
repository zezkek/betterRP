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
        public void OnSpawning(SpawningEventArgs ev)
        {
            Plugin.PluginItem.NewCoroutine(this.ChangeSizeDelay(ev.Player));
        }

        public void OnChangingRoleEventArgs(ChangingRoleEventArgs ev)
        {
            Plugin.PluginItem.NewCoroutine(this.ChangeSizeDelay(ev.Player));
        }

        public void OnDying(DyingEventArgs ev)
        {
            ev.Target.Scale = new Vector3(1, 1, 1);
        }

        private IEnumerator<float> ChangeSizeDelay(Player player)
        {
            yield return MEC.Timing.WaitForSeconds(0.1f);
            if (player == null)
            {
                yield break;
            }

            if (player.IsHuman && Plugin.PluginItem.Config.PlayerNamesEnabled && player.Role != RoleType.None)
            {
                player.Scale = new Vector3(UnityEngine.Random.Range(0.9f, 1.1f), UnityEngine.Random.Range(0.9f, 1.1f), 1);
            }
            else if (player.IsAlive)
            {
                player.Scale = new Vector3(1, 1, 1);
            }
        }
    }
}
