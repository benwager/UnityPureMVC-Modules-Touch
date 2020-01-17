using UnityEngine;

namespace UnityPureMVC.Modules.Touch.Model.VO
{
    [System.Serializable]
    public class TouchCallbackVO
    {
        internal GameObject gameObject;
        internal Vector2 swipeDelta;
        internal Vector2 screenPosition;
    }
}