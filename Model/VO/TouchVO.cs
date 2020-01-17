using UnityPureMVC.Modules.Touch.Model.Enums;
using UnityPureMVC.Modules.Touch.View.Components;
using UnityEngine;

namespace UnityPureMVC.Modules.Touch.Model.VO
{
    [System.Serializable]
    internal class TouchVO
    {
        internal TouchEngine touchEngine;
        internal GameObject touchEngineGameObject;
        internal ITouchEngineComponent touchEngineComponent;
    }
}