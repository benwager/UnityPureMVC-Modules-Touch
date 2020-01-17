using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityPureMVC.Core.Libraries.UnityLib.Utilities.Logging;
using UnityPureMVC.Modules.Touch.Controller.Commands.Requests;
using UnityPureMVC.Modules.Touch.Controller.Notes;
using UnityPureMVC.Modules.Touch.Model.Enums;
using UnityPureMVC.Modules.Touch.Model.VO;

namespace UnityPureMVC.Modules.Touch.Controller.Commands
{
    class TouchStartCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            DebugLogger.Log("TouchStartCommand::Execute");

            // Register Proxies
            Facade.RegisterProxy(new TouchProxy());

            // Register commands
            Facade.RegisterCommand(TouchNote.REQUEST_REGISTER_TAP, typeof(RequestRegisterTapCommand));
            Facade.RegisterCommand(TouchNote.REQUEST_SET_TOUCH_ENGINE, typeof(RequestSetTouchEngineCommand));
            Facade.RegisterCommand(TouchNote.REQUEST_REGISTER_SWIPE_DELTA, typeof(RequestRegisterSwipeDeltaCommand));
            Facade.RegisterCommand(TouchNote.REQUEST_REGISTER_TOUCH_DOWN, typeof(RequestRegisterTouchDownCommand));
            Facade.RegisterCommand(TouchNote.REQUEST_REGISTER_TOUCH_UP, typeof(RequestRegisterTouchUpCommand));

            SendNotification(TouchNote.REQUEST_SET_TOUCH_ENGINE, TouchEngine.LeanTouch, null, "Touch");

            Facade.RemoveCommand(TouchNote.START);
        }
    }
}