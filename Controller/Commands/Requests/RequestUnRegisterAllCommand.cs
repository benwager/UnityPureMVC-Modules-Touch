using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;
using UnityPureMVC.Modules.Touch.Model.VO;

namespace UnityPureMVC.Modules.Touch.Controller.Commands.Requests
{
    internal class RequestUnRegisterAllCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            TouchProxy touchProxy = Facade.RetrieveProxy(TouchProxy.NAME) as TouchProxy;

            touchProxy.TouchEngineComponent.UnRegisterAll(notification.Body as GameObject);
        }
    }
}
