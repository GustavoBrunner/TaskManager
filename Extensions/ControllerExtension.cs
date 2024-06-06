using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.ViewModel;

namespace TaskManager.Extensions
{
    public static class ControllerExtensions
    {
        public const string INFO_NAME = "infoMessage";
        public static void ShowInfoMessage(this Controller @this, string message, bool error = false){
            @this.TempData[INFO_NAME] = MessageViewModel.Serialize(message,
                error ? MessageType.error : MessageType.info );
        }
    }
}