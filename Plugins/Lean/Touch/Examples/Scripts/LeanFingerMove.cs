using UnityEngine;

namespace Lean.Touch
{
	public class LeanFingerMove : MonoBehaviour
	{
		public delegate void OnDragDelegate(Vector3 delta, Vector3 position);
		public event OnDragDelegate OnDrag;

		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>The camera the translation will be calculated using.\n\nNone = MainCamera.</summary>
		[Tooltip("The camera the translation will be calculated using.\n\nNone = MainCamera.")]
		public Camera Camera;

		public Vector2 startPosition; 

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			Use.AddFinger(finger);
			startPosition = transform.position;
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			Use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			Use.RemoveAllFingers();
		}

		public LeanSelectable RequiredSelectable
		{
			set
			{
				Use.RequiredSelectable = value;
			}
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif
		protected virtual void Awake()
		{
			Use.UpdateRequiredSelectable(gameObject);
			Use.Filter = LeanFingerFilter.FilterType.ManuallyAddedFingers;
		}

		protected virtual void Update()
		{
			// Get the fingers we want to use
			var fingers = Use.GetFingers();

			if (fingers.Count > 0)
			{
				Vector2 swipeDelta = fingers[0].SwipeScreenDelta;
				if (swipeDelta != Vector2.zero)
				{
					OnDrag?.Invoke(swipeDelta, (startPosition + swipeDelta));
				}
			}
		}
	}
}