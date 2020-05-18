using UnityEngine;

namespace UnityPureMVC.Modules.Touch.Model.VO
{
    [System.Serializable]
    public class TouchCallbackVO
    {
        internal GameObject gameObject;
        internal Vector2 delta;
        internal Vector2 screenPosition;
        internal Vector2 objectPosition;
        internal Vector2 normalisedScreenPosition;
        internal Vector2 normalisedObjectPosition;
    }
}