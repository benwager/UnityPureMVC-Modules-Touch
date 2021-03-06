﻿using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityPureMVC.Modules.Touch.Model.VO;

namespace UnityPureMVC.Modules.Touch.Controller.Commands.Requests
{
    internal class RequestRegisterSwipeDeltaCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            RegisterTouchVO registerTouchVO = notification.Body as RegisterTouchVO;

            TouchProxy touchProxy = Facade.RetrieveProxy(TouchProxy.NAME) as TouchProxy;

            touchProxy.TouchEngineComponent.RegisterSwipeDelta(registerTouchVO.gameObject, registerTouchVO.includeChildren, registerTouchVO.callback);
        }
    }
}
