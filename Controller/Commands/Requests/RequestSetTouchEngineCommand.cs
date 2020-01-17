using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityPureMVC.Modules.Touch.Model.Enums;
using UnityPureMVC.Modules.Touch.Model.VO;
using UnityPureMVC.Modules.Touch.View.Components;
using UnityEngine;

namespace UnityPureMVC.Modules.Touch.Controller.Commands.Requests
{
    internal class RequestSetTouchEngineCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            TouchProxy touchProxy = Facade.RetrieveProxy(TouchProxy.NAME) as TouchProxy;

            TouchEngine touchEngine = (TouchEngine)notification.Body;

            GameObject touchEngineGameObject = touchProxy.TouchEngineGameObject;

            ITouchEngineComponent touchEngineComponent = touchProxy.TouchEngineComponent;

            if (touchEngineComponent != null)
            {
                touchEngineComponent.Destroy();
            }

            if (touchEngineGameObject == null)
            {
                touchEngineGameObject = new GameObject("Touch");
            }

            switch (touchEngine)
            {
                case TouchEngine.LeanTouch:
                    touchEngineComponent = touchEngineGameObject.AddComponent<LeanTouchEngineComponent>();
                    break;

                default:
                    touchEngineComponent = null;
                    break;
            }

            if (touchEngineComponent == null)
            {
                return;
            }

            touchEngineComponent.Initialize();

            touchProxy.TouchEngine = touchEngine;
            touchProxy.TouchEngineGameObject = touchEngineGameObject;
            touchProxy.TouchEngineComponent = touchEngineComponent;
        }
    }
}
