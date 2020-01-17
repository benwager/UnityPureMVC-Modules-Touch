using Lean.Touch;
using UnityPureMVC.Core.Libraries.UnityLib.Utilities.Logging;
using UnityPureMVC.Modules.Touch.Model.VO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityPureMVC.Modules.Touch.View.Components.Delegates;

namespace UnityPureMVC.Modules.Touch.View.Components
{
    public class LeanTouchEngineComponent : MonoBehaviour, ITouchEngineComponent
    {
        public LeanFingerFilter Use = new LeanFingerFilter(true);
        LeanTouch leanTouch;
        LeanSelect leanSelect;
        LeanFingerTap leanFingerTap;
        LeanFingerDown leanFingerDown;
        LeanFingerUp leanFingerUp;

        List<OnTouchDelegate> swipeDeltaCallbacks;

        public void Initialize()
        {
            swipeDeltaCallbacks = new List<OnTouchDelegate>();

            leanTouch = gameObject.AddComponent<LeanTouch>();
            leanSelect = gameObject.AddComponent<LeanSelect>();
            leanFingerTap = gameObject.AddComponent<LeanFingerTap>();
            leanFingerDown = gameObject.AddComponent<LeanFingerDown>();
            leanFingerUp = gameObject.AddComponent<LeanFingerUp>();

            leanFingerTap.OnFinger.AddListener(leanSelect.SelectScreenPosition);
            leanFingerDown.OnFinger.AddListener(leanSelect.SelectScreenPosition);

            leanSelect.LayerMask &= ~(1 << LayerMask.NameToLayer("PostProcessing"));
        }

        public void RegisterTap(GameObject gameObject, OnTouchDelegate callback)
        {
            if (gameObject == null)
            {
                // Register a global tap event
                RegisterTap(callback);
                return;
            }

            Collider collider = gameObject.GetComponentInChildren<Collider>();

            if (collider == null)
            {
                DebugLogger.LogWarning("Touch : Could not register tap. No Collider found on {0}", gameObject.name);
                return;
            }

            LeanFingerTap tap = collider.gameObject.AddComponent<LeanFingerTap>();
            LeanSelectable selectable = tap.gameObject.AddComponent<LeanSelectable>();
            selectable.DeselectOnUp = true;
            tap.RequiredSelectable = selectable;
            tap.OnFinger.AddListener((LeanFinger leanFinger) =>
            {
                callback?.Invoke(new TouchCallbackVO
                {
                    gameObject = gameObject
                });
            });
        }

        private void RegisterTap(OnTouchDelegate callback)
        {
            leanFingerTap.OnFinger.AddListener((LeanFinger leanFinger) =>
            {
                callback?.Invoke(new TouchCallbackVO
                {
                    screenPosition = leanFinger.ScreenPosition
                });
            });
        }

        public void RegisterTouchDown(GameObject gameObject, OnTouchDelegate callback)
        {
            if (gameObject == null)
            {
                RegisterTouchDown(callback);
                return;
            }

            Collider collider = gameObject.GetComponentInChildren<Collider>();

            if (collider == null)
            {
                DebugLogger.LogWarning("Touch : Could not register tap. No Collider found on {0}", gameObject.name);
                return;
            }

            LeanFingerDown down = collider.gameObject.AddComponent<LeanFingerDown>();
            LeanSelectable selectable = down.gameObject.AddComponent<LeanSelectable>();
            selectable.DeselectOnUp = true;
            down.RequiredSelectable = selectable;
            down.OnFinger.AddListener((LeanFinger leanFinger) =>
            {
                callback?.Invoke(new TouchCallbackVO
                {
                    gameObject = gameObject
                });
            });
        }

        private void RegisterTouchDown(OnTouchDelegate callback)
        {
            leanFingerDown.OnFinger.AddListener((LeanFinger leanFinger) =>
            {
                callback?.Invoke(new TouchCallbackVO
                {
                    screenPosition = leanFinger.ScreenPosition
                });
            });
        }

        public void RegisterTouchUp(GameObject gameObject, OnTouchDelegate callback)
        {
            if (gameObject == null)
            {
                RegisterTouchUp(callback);
                return;
            }

            Collider collider = gameObject.GetComponentInChildren<Collider>();

            if (collider == null)
            {
                DebugLogger.LogWarning("Touch : Could not register tap. No Collider found on {0}", gameObject.name);
                return;
            }

            LeanFingerUp up = collider.gameObject.AddComponent<LeanFingerUp>();
            LeanSelectable selectable = up.gameObject.AddComponent<LeanSelectable>();
            selectable.DeselectOnUp = true;
            up.RequiredSelectable = selectable;
            up.OnFinger.AddListener((LeanFinger leanFinger) =>
            {
                callback?.Invoke(new TouchCallbackVO
                {
                    gameObject = gameObject
                });
            });
        }

        private void RegisterTouchUp(OnTouchDelegate callback)
        {
            leanFingerUp.OnFinger.AddListener((LeanFinger leanFinger) =>
            {
                callback?.Invoke(new TouchCallbackVO
                {
                    screenPosition = leanFinger.ScreenPosition
                });
            });
        }

        public void RegisterSwipeDelta(GameObject gameObject, OnTouchDelegate callback)
        {
            if (gameObject == null)
            {
                RegisterSwipeDelta(callback);
                return;
            }

            Debug.LogError("NEED TO IMPLEMENT OBJECT SPECIFIC SWIPE DELTA!");
        }

        private void RegisterSwipeDelta(OnTouchDelegate callback)
        {
            swipeDeltaCallbacks.Add(callback);
        }

        private void Update()
        {
            if (Use.GetFingers().Count > 0)
            {
                TouchCallbackVO touchCallbackVO = new TouchCallbackVO
                {
                    swipeDelta = LeanGesture.GetScreenDelta(Use.GetFingers())
                };
                swipeDeltaCallbacks.ForEach(i => i?.Invoke(touchCallbackVO));
            }
        }

        public void Destroy()
        {
            swipeDeltaCallbacks.Clear();

            FindObjectsOfType<LeanFingerTap>().ToList().ForEach(i => GameObject.Destroy(i));
            FindObjectsOfType<LeanFingerDown>().ToList().ForEach(i => GameObject.Destroy(i));
            FindObjectsOfType<LeanFingerUp>().ToList().ForEach(i => GameObject.Destroy(i));
            FindObjectsOfType<LeanSelectable>().ToList().ForEach(i => GameObject.Destroy(i));

            GameObject.Destroy(leanTouch);
            GameObject.Destroy(leanSelect);
            GameObject.Destroy(leanFingerTap);
            GameObject.Destroy(leanFingerDown);
            GameObject.Destroy(leanFingerUp);
        }
    }
}