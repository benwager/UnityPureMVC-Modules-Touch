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

        Dictionary<LeanFingerTap, List<OnTouchDelegate>> registeredTapCallbacks;


        public void Initialize()
        {
            swipeDeltaCallbacks = new List<OnTouchDelegate>();

            registeredTapCallbacks = new Dictionary<LeanFingerTap, List<OnTouchDelegate>>();

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


            LeanFingerTap tap = collider.gameObject.GetComponent<LeanFingerTap>();

            if(tap == null)
            {
                tap = collider.gameObject.AddComponent<LeanFingerTap>();
            }
            
            LeanSelectable selectable = tap.gameObject.GetComponent<LeanSelectable>();
            if(selectable == null)
            {
                selectable = tap.gameObject.AddComponent<LeanSelectable>();
            }

            // Check if it exists in the dictionary
            if(registeredTapCallbacks.ContainsKey(tap))
            {
                // Check if this particular callback is already registered
                if(registeredTapCallbacks[tap].Contains(callback))
                {
                    return;
                }
            }
            else
            {
                registeredTapCallbacks.Add(tap, new List<OnTouchDelegate>());
            }

            registeredTapCallbacks[tap].Add(callback);

            selectable.DeselectOnUp = true;
            tap.RequiredSelectable = selectable;
            tap.OnFinger.AddListener(OnTap);
        }

        private void RegisterTap(OnTouchDelegate callback)
        {
            RegisterTap(gameObject, callback);
        }

        public void UnRegisterTap(GameObject gameObject, OnTouchDelegate callback)
        {
            LeanFingerTap tap = gameObject.GetComponent<LeanFingerTap>();
            if (tap == null) return;

            if(registeredTapCallbacks.ContainsKey(tap))
            {
                if(registeredTapCallbacks[tap].Contains(callback))
                {
                    registeredTapCallbacks[tap].Remove(callback);
                }
            }
        }

        private void OnTap(LeanFinger leanFinger)
        {
            GameObject go = leanFinger.gameObject;
            LeanFingerTap tap = go.GetComponent<LeanFingerTap>();
            if (tap == null) return;
            if(registeredTapCallbacks.ContainsKey(tap))
            {
                registeredTapCallbacks[tap].ForEach(i => i.Invoke(new TouchCallbackVO
                {
                    gameObject = go,
                    screenPosition = leanFinger.ScreenPosition
                }));
            }
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
                    swipeDelta = LeanGesture.GetScreenDelta(Use.GetFingers()),
                    screenPosition = LeanGesture.GetStartScreenCenter(Use.GetFingers())
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