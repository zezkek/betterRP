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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "<Pending>")]
    public class StartsBlackout
    {
        private readonly Plugin plugin;

        private Dictionary<Room, float> defaultBright = new Dictionary<Room, float> { };

        public StartsBlackout(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnRoundStarted()
        {
            this.defaultBright.Clear();
            this.plugin.NewCoroutine(this.BlackoutDealy());
        }

        public IEnumerator<float> BlackoutDealy()
        {
            yield return MEC.Timing.WaitForSeconds(this.plugin.Config.BlackoutDelay);
            Map.TurnOffAllLights(5);
            foreach (var item in Map.Rooms)
            {
                this.defaultBright.Add(item, item.LightIntensity);
                item.LightIntensity = UnityEngine.Random.Range(
                    this.defaultBright[item] * (Plugin.Instance.Config.LightIntensityMin / 100),
                    this.defaultBright[item] * (Plugin.Instance.Config.LightIntensityMax / 100));
            }

            while (Round.IsStarted)
            {
                List<Room> rooms = Map.Rooms.ToList();
                rooms.ShuffleList();
                float time = UnityEngine.Random.Range(1f, 7f);
                for (int i = 0; i < 5; i++)
                {
                    rooms[i].TurnOffLights(time);
                }

                yield return MEC.Timing.WaitForSeconds(time);
                for (int i = 0; i < 5; i++)
                {
                    rooms[i].LightIntensity = UnityEngine.Random.Range(
                        this.defaultBright[rooms[i]] * (Plugin.Instance.Config.LightIntensityMin / 100),
                        this.defaultBright[rooms[i]] * (Plugin.Instance.Config.LightIntensityMax / 100));
                }

                yield return MEC.Timing.WaitForSeconds(UnityEngine.Random.Range(30f, 90f));
            }

            yield break;
        }
    }
}
