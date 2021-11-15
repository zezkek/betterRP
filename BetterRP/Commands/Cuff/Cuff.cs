#define Cuff
namespace BetterRP.Commands.Cuff
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using CommandSystem;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using UnityEngine;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Cuff : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cuff"/> class.
        /// </summary>
        public Cuff() => this.LoadGeneratedCommands();

        /// <inheritdoc/>
        public override string Command { get; } = "handcuff";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = new string[] { };

        /// <inheritdoc/>
        public override string Description { get; } = "Связывает объект/человека, стоящего перед Вами";

        /// <inheritdoc/>
        public override void LoadGeneratedCommands()
        {
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            Player playerRequester = Player.Get((sender as CommandSender)?.SenderId);
            Log.Debug($"Player {playerRequester.Nickname} tries execute command .cuff", Plugin.Instance.Config.Debug);
            Player target;
            try
            {
                float distance = 3f;
                Log.Debug($"Starting check parameters.\nCuff distance: {distance}. Logging command", Plugin.Instance.Config.Debug);
                CommandMethods.LogCommandUsed((CommandSender)sender, CommandMethods.FormatArguments(arguments, 0));
                //if (!((CommandSender)sender).CheckPermission("betterRP."))
                //{
                //    Log.Debug($"Error. Missed required permissions.", Plugin.Instance.Config.Debug);
                //    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: В ДОСТУПЕ ОТКАЗАНО</color>";
                //    return false;
                //}
                Log.Debug($"Command has been logged.\nChecking is sender alive or not", Plugin.Instance.Config.Debug);
                if (!playerRequester.IsAlive || playerRequester.Team == Team.SCP)
                {
                    Log.Debug($"Error. Wrong role.", Plugin.Instance.Config.Debug);
                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ДОСТУП ОГРАНИЧЕН ДЛЯ ДАННОЙ РОЛИ</color>";
                    return false;
                }

                Log.Debug($"Sender is alive.\nChecking is raycast hit something or not", Plugin.Instance.Config.Debug);
                if (!Physics.Raycast(playerRequester.CameraTransform.position, playerRequester.CameraTransform.forward, out RaycastHit raycastthit, distance))
                {
                    Log.Debug($"Error. Nothing found.", Plugin.Instance.Config.Debug);
                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ОБЪЕКТ НЕ НАЙДЕН В ЗОНЕ ДОСЯГАЕМОСТИ</color>";
                    return false;
                }

                Log.Debug($"Catched some object.\nTrying to get player.Potential target: {Player.Get(raycastthit.transform.gameObject)?.Nickname}.", Plugin.Instance.Config.Debug);
                target = Player.Get(raycastthit.transform.gameObject);
                Log.Debug($"New target: {target?.Nickname}. Checking is target null or not", Plugin.Instance.Config.Debug);
                if (target == null)
                {
                    Log.Debug($"Error. Target in null.", Plugin.Instance.Config.Debug);
                    response = string.Join(
                        "\n",
                        $"\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>",
                        $"<color=#C1B5B5>ВЫВОД: ОБЪЕКТ НЕ РАСПОЗНАН</color>");

                    return false;
                }
                else
                {
                    Log.Debug($"Object is not null.\nChecking is target alive or not", Plugin.Instance.Config.Debug);
                    if (!target.IsAlive)
                    {
                        Log.Debug($"Error. Target is dead.", Plugin.Instance.Config.Debug);
                        response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ МЕРТВА</color>";
                        return false;
                    }

                    //else if (target.Team == Team.SCP)
                    //{
                    //    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ НЕВОЗМОЖНО ОБЕЗВРЕДИТЬ</color>";
                    //    return false;
                    //}
                    //Log.Debug($"Target is alive.\nChecking has target a firearm or not", Plugin.Instance.Config.Debug);
                    //if (target.CurrentItem.Base.Category == ItemCategory.Firearm || target.CurrentItem.Base.Category == ItemCategory.Grenade || target.CurrentItem.Base.Category == ItemCategory.MicroHID)
                    //{
                    //    Log.Debug($"Error. Target has firearm.", Plugin.Instance.Config.Debug);
                    //    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ ВООРУЖЕНА</color>";
                    //    return false;
                    //}

                    //if (playerRequester.CurrentItem.Base.Category == ItemCategory.Firearm || playerRequester.CurrentItem.Base.Category == ItemCategory.MicroHID)
                    //{
                    //    Log.Debug($"Sender hasn't firearm.", Plugin.Instance.Config.Debug);
                    //    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ВЫ НЕ ВООРУЖЕНЫ</color>";
                    //    return false;
                    //}

                    Log.Debug($"Trying to handcuff player.", Plugin.Instance.Config.Debug);
                    target.Handcuff(playerRequester);
                    Log.Debug($"Player has been handcuffed. Cuffer: {target.Cuffer}. Trying to set cuffer: {playerRequester}", Plugin.Instance.Config.Debug);

                    target.Cuffer = playerRequester;
                    Log.Debug($"Second cuff attempt. Cuffer: {target.Cuffer}.", Plugin.Instance.Config.Debug);

                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#6aa84f>УСПЕШНО</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ ОБЕЗВРЕЖЕНА</color>";
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error($"An error occured while trying to cuff player: {e}");
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>КРИТИЧЕСКАЯ ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ОБНАРУЖЕНА КРИТИЧЕСКАЯ ОШИБКА. СВЯЖИТЕСЬ С ПРЕДСТАВИТЕЛЕМ ИНЖЕНЕРНО-ТЕХНИЧЕСКОЙ СЛУЖБЫ</color>";
                return false;
            }
        }

    }
}
