namespace UnityPureMVC.Modules.Touch
{
    using PureMVC.Interfaces;
    using PureMVC.Patterns.Facade;
    using UnityPureMVC.Modules.Touch.Controller.Commands;
    using UnityPureMVC.Modules.Touch.Controller.Notes;
    using System;
    using UnityEngine;

    internal class TouchModule : MonoBehaviour
    {
        /// <summary>
        /// The core facade.
        /// </summary>
        private IFacade facade;

        /// <summary>
        /// Start this instance.
        /// </summary>
        protected virtual void Awake()
        {
            try
            {
                facade = Facade.GetInstance("Touch");
                facade.RegisterCommand(TouchNote.START, typeof(TouchStartCommand));
                facade.SendNotification(TouchNote.START, this, null, "Touch");
            }
            catch (Exception exception)
            {
                throw new UnityException("Unable to initiate Facade", exception);
            }
        }

        /// <summary>
        /// On destroy.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (facade != null)
            {
                facade.Dispose();
                facade = null;
            }
        }
    }
}