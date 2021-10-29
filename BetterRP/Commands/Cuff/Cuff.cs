namespace BetterRP.Commands.Cuff
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using UnityEngine;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]

    public class Cuff : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cuff"/> class.
        /// </summary>
        public Cuff() => this.LoadGeneratedCommands();

        /// <inheritdoc/>
        public override string Command { get; } = "cuff";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = new string[] { };

        /// <inheritdoc/>
        public override string Description { get; } = "Связывание человека или объекта";

        /// <inheritdoc/>
        public override void LoadGeneratedCommands()
        {
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerRequester = Player.Get((sender as CommandSender)?.SenderId);

            CommandMethods.LogCommandUsed((CommandSender)sender, CommandMethods.FormatArguments(arguments, 0));
            if (!((CommandSender)sender).CheckPermission("betterRP.cuff"))
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: В ДОСТУПЕ ОТКАЗАНО</color>";
                return false;
            }

            if (!playerRequester.IsAlive || playerRequester.Team == Team.SCP)
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ДОСТУП ОГРАНИЧЕН ДЛЯ НАБЛЮДАТЕЛЕЙ</color>";
                return false;
            }

            if (!Physics.Raycast(playerRequester.CameraTransform.position, playerRequester.CameraTransform.forward, out RaycastHit raycastthit, 3f))
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ОБЪЕКТ НЕ НАЙДЕН В ЗОНЕ ДОСЯГАЕМОСТИ</color>";
                return false;
            }
            else if (Player.Get(raycastthit.transform.gameObject) == null)
            {
                response = $"\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ОБЪЕКТ {raycastthit.transform.gameObject.name} {raycastthit.transform.gameObject.tag} {raycastthit.transform.gameObject.GetType()} НЕ РАСПОЗНАН</color>";
                return false;
            }
            else
            {
                var target = Player.Get(raycastthit.transform.gameObject);
                if (!target.IsAlive)
                {
                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ МЕРТВА</color>";
                    return false;
                }
                else if (target.Team == Team.SCP)
                {
                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ НЕВОЗМОЖНО ОБЕЗВРЕДИТЬ</color>";
                    return false;
                }
                else if (target.CurrentItem.Base.Category == ItemCategory.Firearm || target.CurrentItem.Base.Category == ItemCategory.Grenade || target.CurrentItem.Base.Category == ItemCategory.MicroHID)
                {
                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ ВООРУЖЕНА</color>";
                    return false;
                }
                else if (playerRequester.CurrentItem.Base.Category == ItemCategory.Firearm || playerRequester.CurrentItem.Base.Category == ItemCategory.MicroHID)
                {
                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ВЫ НЕ ВООРУЖЕНЫ</color>";
                    return false;
                }

                target.Cuffer = playerRequester;

                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#6aa84f>УСПЕШНО</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ ОБЕЗВРЕЖЕНА</color>";
                return true;
            }
        }
    }
}
