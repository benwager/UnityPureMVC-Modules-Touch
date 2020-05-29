using Lean.Touch;
using UnityPureMVC.Core.Libraries.UnityLib.Utilities.Logging;
using UnityPureMVC.Modules.Touch.Model.VO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityPureMVC.Modules.Touch.View.Components.Delegates;
using UnityPureMVC.Core;
using System;
using System.Diagnostics.Eventing.Reader;

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
        Dictionary<GameObject, List<OnTouchDelegate>> registeredSwipeDeltaCallbacks;
        Dictionary<LeanFingerTap, List<OnTouchDelegate>> registeredTapCallbacks;

        public void Initialize()
        {
            registeredSwipeDeltaCallbacks = new Dictionary<GameObject, List<OnTouchDelegate>>();
            registeredTapCallbacks = new Dictionary<LeanFingerTap, List<OnTouchDelegate>>();

            leanTouch = gameObject.AddComponent<LeanTouch>();
            leanSelect = gameObject.AddComponent<LeanSelect>();
            leanFingerTap = gameObject.AddComponent<LeanFingerTap>();
            leanFingerDown = gameObject.AddComponent<LeanFingerDown>();
            leanFingerUp = gameObject.AddComponent<LeanFingerUp>();

            leanFingerTap.OnFinger.AddListener(leanSelect.SelectScreenPosition);
            leanFingerDown.OnFinger.AddListener(leanSelect.SelectScreenPosition);

            leanSelect.LayerMask &= ~(1 << LayerMask.NameToLayer("PostProcessing"));
            leanSelect.SelectUsing = LeanSelect.SelectType.CanvasUI;
            leanSelect.SelectUsingAlt = LeanSelect.SelectType.Raycast3D;

            leanFingerDown.IgnoreStartedOverGui = false;
            leanFingerUp.IgnoreStartedOverGui = false;
            leanFingerTap.IgnoreStartedOverGui = false;
        }

        private LeanSelectable AddSelectableComponent(GameObject gameObject)
        {
            LeanSelectable selectable = gameObject.GetComponent<LeanSelectable>();
            if (selectable == null)
            {
                selectable = gameObject.AddComponent<LeanSelectable>();
            }
            return selectable;
        }

        /// <summary>
        /// Registers a Tap event on a specific object
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="includeChildren"></param>
        /// <param name="callback"></param>
        public void RegisterTap(GameObject gameObject, bool includeChildren, OnTouchDelegate callback)
        {
            if (gameObject == null)
            {
                // Register a global tap event
                RegisterTap(callback);
                return;
            }

            GameObject target = gameObject;
            if(gameObject.transform is RectTransform)
            {

            }
            else
            {
                Collider collider = gameObject.GetComponentInChildren<Collider>();

                if (collider == null)
                {
                    DebugLogger.LogWarning("Touch : Could not register tap. No Collider found on {0}", gameObject.name);
                    return;
                }
                target = collider.gameObject;
            }


            LeanFingerTap tap = target.GetComponent<LeanFingerTap>();

            if(tap == null)
            {
                tap = target.AddComponent<LeanFingerTap>();
            }

            tap.IgnoreStartedOverGui = false;

            LeanSelectable selectable = AddSelectableComponent(target);

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


        /// <summary>
        /// Registers a global Tap event
        /// </summary>
        /// <param name="callback"></param>
        private void RegisterTap(OnTouchDelegate callback)
        {
            RegisterTap(gameObject, false, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="callback"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public void UnRegisterTap(OnTouchDelegate callback)
        {
            UnRegisterTap(gameObject, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leanFinger"></param>
        private void OnTap(LeanFinger leanFinger)
        {
            GameObject go = leanFinger.gameObject;
            LeanFingerTap tap = go.GetComponent<LeanFingerTap>();
            if (tap == null) return;


            float nsx = leanFinger.ScreenPosition.x / Screen.width;
            float nsy = leanFinger.ScreenPosition.y / Screen.height;
            Vector2 normalisedScreenPosition = new Vector2(
                nsx,
                nsy
                );


            Vector2 objectPosition = new Vector2();
            Vector2 normalisedObjectPosition = new Vector2();
            RectTransform rt = go.transform as RectTransform;
            if (rt != null)
            {
                // Flip the y axis
                Vector2 rtPos = rt.anchoredPosition;
                rtPos.y = -rtPos.y;
                Vector2 flippedScreenPos = leanFinger.ScreenPosition;
                flippedScreenPos.y = Screen.height - leanFinger.ScreenPosition.y;

                objectPosition = flippedScreenPos - rtPos;
                normalisedObjectPosition.x = objectPosition.x / rt.sizeDelta.x;
                normalisedObjectPosition.y = objectPosition.y / rt.sizeDelta.y;

            }


            if (registeredTapCallbacks.ContainsKey(tap))
            {
                registeredTapCallbacks[tap].ForEach(i => i.Invoke(new TouchCallbackVO
                {
                    gameObject = go,
                    screenPosition = leanFinger.ScreenPosition,
                    normalisedScreenPosition = normalisedScreenPosition,
                    objectPosition = objectPosition,
                    normalisedObjectPosition = normalisedObjectPosition
                }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="includeChildren"></param>
        /// <param name="callback"></param>
        public void RegisterTouchDown(GameObject gameObject, bool includeChildren, OnTouchDelegate callback)
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
            LeanSelectable selectable = AddSelectableComponent(gameObject);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="includeChildren"></param>
        /// <param name="callback"></param>
        public void RegisterTouchUp(GameObject gameObject, bool includeChildren, OnTouchDelegate callback)
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
            LeanSelectable selectable = AddSelectableComponent(gameObject);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="includeChildren"></param>
        /// <param name="callback"></param>
        public void RegisterSwipeDelta(GameObject gameObject, bool includeChildren, OnTouchDelegate callback)
        {
            if (gameObject == null)
            {
                // Register a global swipe delta event
                RegisterSwipeDelta(callback);
                return;
            }

            LeanFingerMove move = gameObject.AddComponent<LeanFingerMove>();
            LeanSelectable selectable = AddSelectableComponent(gameObject);
            selectable.DeselectOnUp = true;
            move.RequiredSelectable = selectable;
            move.Use.IgnoreStartedOverGui = false;

            selectable.OnSelect.AddListener((LeanFinger finger) =>
            {
                move.AddFinger(finger);
            });
            selectable.OnDeselect.AddListener(() =>
            {
                move.RemoveAllFingers();
            });

            move.OnDrag += (Vector3 delta, Vector3 position) =>
            {
                callback?.Invoke(new TouchCallbackVO()
                {
                    gameObject = gameObject,
                    delta = delta,
                    objectPosition = position
                });
            };

            // Check if it exists in the dictionary
            if (registeredSwipeDeltaCallbacks.ContainsKey(gameObject))
            {
                // Check if this particular callback is already registered
                if (registeredSwipeDeltaCallbacks[gameObject].Contains(callback))
                {
                    return;
                }
            }
            else
            {
                registeredSwipeDeltaCallbacks.Add(gameObject, new List<OnTouchDelegate>());
            }

            registeredSwipeDeltaCallbacks[gameObject].Add(callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        private void RegisterSwipeDelta(OnTouchDelegate callback)
        {
            RegisterSwipeDelta(gameObject, false, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="includeChildren"></param>
        /// <param name="callback"></param>
        public void RegisterDraggableUI(GameObject gameObject, bool includeChildren, OnTouchDelegate callback)
        {
            LeanDragTranslate drag = gameObject.AddComponent<LeanDragTranslate>();
            LeanSelectable selectable = AddSelectableComponent(gameObject);

            if (includeChildren)
            {
                HandleIncludeChildren(gameObject, selectable);
            }
            selectable.OnSelect.AddListener((LeanFinger finger) =>
            {
                drag.AddFinger(finger);
            });
            selectable.OnDeselect.AddListener(() =>
            {
                drag.RemoveAllFingers();
            });


            drag.Inertia = .2f;
            drag.Dampening = 10.0f;
            drag.RequiredSelectable = selectable;
            drag.Use.IgnoreStartedOverGui = false;
            selectable.DeselectOnUp = true;
            drag.OnDrag += (Vector3 delta) => 
            {
                callback?.Invoke(new TouchCallbackVO()
                {
                    gameObject = gameObject,
                    delta = delta,
                    screenPosition = gameObject.transform.position
                });
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="selectable"></param>
        private void HandleIncludeChildren(GameObject gameObject, LeanSelectable selectable)
        {
            foreach (Transform child in gameObject.transform)
            {
                LeanSelectable s = AddSelectableComponent(child.gameObject);
                s.DeselectOnUp = true;
                s.OnSelect.AddListener((LeanFinger finger) =>
                {
                    selectable.Select(finger);
                });
                s.OnDeselect.AddListener(() =>
                {
                    selectable.Deselect();
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if(!registeredSwipeDeltaCallbacks.ContainsKey(gameObject))
            {
                return;
            }

            if (Use.GetFingers().Count > 0)
            {
                TouchCallbackVO touchCallbackVO = new TouchCallbackVO
                {
                    delta = LeanGesture.GetScreenDelta(Use.GetFingers()),
                    screenPosition = LeanGesture.GetStartScreenCenter(Use.GetFingers())
                };
                registeredSwipeDeltaCallbacks[gameObject].ForEach(i => i?.Invoke(touchCallbackVO));
            }
        }

        /// <summary>
        /// Clean up
        /// </summary>
        public void Destroy()
        {
            registeredSwipeDeltaCallbacks.Clear();
            registeredTapCallbacks.Clear();

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