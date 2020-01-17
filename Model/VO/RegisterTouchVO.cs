using UnityEngine;
using static UnityPureMVC.Modules.Touch.View.Components.Delegates;

namespace UnityPureMVC.Modules.Touch.Model.VO
{
    [System.Serializable]
    internal class RegisterTouchVO
    {
        internal GameObject gameObject;
        internal OnTouchDelegate callback;
    }
}