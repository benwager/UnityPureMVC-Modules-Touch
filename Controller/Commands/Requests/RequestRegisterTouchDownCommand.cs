﻿using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityPureMVC.Modules.Touch.Model.VO;

namespace UnityPureMVC.Modules.Touch.Controller.Commands.Requests
{
    internal class RequestRegisterTouchDownCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            RegisterTouchVO registerTouchVO = notification.Body as RegisterTouchVO;

            TouchProxy touchProxy = Facade.RetrieveProxy(TouchProxy.NAME) as TouchProxy;

            touchProxy.TouchEngineComponent.RegisterTouchDown(registerTouchVO.gameObject, registerTouchVO.includeChildren, registerTouchVO.callback);
        }
    }
}
