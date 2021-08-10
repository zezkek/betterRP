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
        //хуй пойми через конфиг не работает, потом разберусь
        //[Description("Science Status depends on start keycard")]
        //public string[] ScienceStatus { get; } ={ "Стажер", "Д-р ", "Руководитель зоны д-р " , "Ст. научный сотрудник ", "Инженер камер содержания " , "Руководитель комплекса д-р " };
    }
}
