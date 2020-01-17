using UnityEngine;
using static UnityPureMVC.Modules.Touch.View.Components.Delegates;

namespace UnityPureMVC.Modules.Touch.View.Components
{
    public interface ITouchEngineComponent
    {
        void Initialize();

        void RegisterTap(GameObject gameObject, OnTouchDelegate callback);

        void RegisterTouchDown(GameObject gameObject, OnTouchDelegate callback);

        void RegisterTouchUp(GameObject gameObject, OnTouchDelegate callback);

        void RegisterSwipeDelta(GameObject gameObject, OnTouchDelegate callback);

        void Destroy();
    }
}