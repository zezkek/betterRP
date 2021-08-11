using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolePlayNames
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public float HintDisplayTime { get; set; } = 10;
    }
}
