namespace BetterRP.API
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

    /// <summary>
    /// Sets realistic human scale.
    /// </summary>
    public class PlayerResize
    {
        /// <summary>
        /// Needed to call ChangeSizeDelay.
        /// </summary>
        /// <param name="ev">ChangingRoleEventArgs.</param>
        public void OnChangingRoleEventArgs(ChangingRoleEventArgs ev)
        {
            Plugin.Instance.NewCoroutine(this.ChangeSize(ev.Player));
        }

        /// <summary>
        /// Needed to call ChangeSizeDelay.
        /// </summary>
        /// <param name="ev">ChangingRoleEventArgs.</param>
        public void OnDying(DyingEventArgs ev)
        {
            ev.Target.Scale = new Vector3(1, 1, 1);
        }

        private IEnumerator<float> ChangeSize(Player player)
        {
            yield return MEC.Timing.WaitForSeconds(0.1f);
            if (player == null)
            {
                yield break;
            }

            if (player.IsHuman && Plugin.Instance.Config.PlayerNamesEnabled && player.Role != RoleType.None)
            {
                float hight = UnityEngine.Random.Range(0.95f, 1.05f);
                if (hight > 1)
                {
                    player.Scale = new Vector3(UnityEngine.Random.Range(1f, 1.05f), hight, 1);
                }
                else
                {
                    player.Scale = new Vector3(UnityEngine.Random.Range(0.95f, 1.05f), hight, 1);
                }
            }
            else if (player.IsAlive)
            {
                player.Scale = new Vector3(1, 1, 1);
            }
        }
    }
}
