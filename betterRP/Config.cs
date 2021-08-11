using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace betterRP
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        [Description("Enable or disable name change")]
        public bool PlayerNamesEnabled { get; set; } = true;
        [Description("Enable or disable player resize")]
        public bool PlayerResizeEnabled { get; set; } = true;
        [Description("New name hint message time")]
        public float HintDisplayTime { get; set; } = 10;
    }
}
