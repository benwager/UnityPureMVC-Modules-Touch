using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityPureMVC.Modules.Touch.Model.VO;

namespace UnityPureMVC.Modules.Touch.Controller.Commands.Requests
{
    internal class RequestUpdateSettingsCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            TouchSettingsVO touchSettingsVO = notification.Body as TouchSettingsVO;

            TouchProxy touchProxy = Facade.RetrieveProxy(TouchProxy.NAME) as TouchProxy;

            if(touchSettingsVO.tapThreshold > 0)
            {
                touchProxy.TouchEngineComponent.SetTapThreshold(touchSettingsVO.tapThreshold);
            }
        }
    }
}
