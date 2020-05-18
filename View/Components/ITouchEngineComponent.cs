using UnityEngine;
using static UnityPureMVC.Modules.Touch.View.Components.Delegates;

namespace UnityPureMVC.Modules.Touch.View.Components
{
    public interface ITouchEngineComponent
    {
        void Initialize();

        void RegisterTap(GameObject gameObject, bool includeChildren, OnTouchDelegate callback);

        void UnRegisterTap(GameObject gameObject, OnTouchDelegate callback);

        void RegisterTouchDown(GameObject gameObject, bool includeChildren, OnTouchDelegate callback);

        void RegisterTouchUp(GameObject gameObject, bool includeChildren, OnTouchDelegate callback);

        void RegisterSwipeDelta(GameObject gameObject, bool includeChildren, OnTouchDelegate callback);

        void RegisterDraggableUI(GameObject gameObject, bool includeChildren, OnTouchDelegate callback);

        void Destroy();
    }
}