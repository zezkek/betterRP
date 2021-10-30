namespace BetterRP
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Enable or disable debug mode")]
        public bool Debug { get; set; } = false;

        [Description("Enable or disable name change")]
        public bool PlayerNamesEnabled { get; set; } = true;

        [Description("Enable or disable player resize")]
        public bool PlayerResizeEnabled { get; set; } = true;

        public string ItemConfigFolder { get; set; } = Path.Combine(Paths.Configs, "BetterRP");

        public float BlackoutDelay { get; set; } = 15f;
    }
}
