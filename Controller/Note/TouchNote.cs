namespace UnityPureMVC.Modules.Touch.Controller.Notes
{
    internal class TouchNote
    {
        internal const string START = "Touch/start";

        /// <summary>
        /// Pass a TouchEngine enum item as the notification body
        /// </summary>
        internal const string REQUEST_SET_TOUCH_ENGINE = "Touch/requestSetTouchEngine";
        
        /// <summary>
        /// Pass a TouchSettingsVO as the notification body
        /// </summary>
        internal const string REQUEST_UPDATE_SETTINGS = "Touch/requestUpdateSettings";

        /// <summary>
        /// Pass a RegisterTouchVO object as the notification body
        /// </summary>
        internal const string REQUEST_REGISTER_TAP = "Touch/requestRegisterTap";

        /// <summary>
        /// Pass a RegisterTouchVO object as the notification body
        /// </summary>
        internal const string REQUEST_UNREGISTER_TAP = "Touch/requestUnRegisterTap";

        /// <summary>
        /// Pass a RegisterTouchVO object as the notification body
        /// </summary>
        internal const string REQUEST_REGISTER_SWIPE_DELTA = "Touch/requestRegisterSwipeDelta";

        /// <summary>
        /// Pass a RegisterTouchVO object as the notification body
        /// </summary>
        internal const string REQUEST_REGISTER_TOUCH_DOWN = "Touch/requestRegisterTouchDown";

        /// <summary>
        /// Pass a RegisterTouchVO object as the notification body
        /// </summary>
        internal const string REQUEST_REGISTER_TOUCH_UP = "Touch/requestRegisterTouchUp";

        /// <summary>
        /// Pass a RegisterTouchVO object as the notification body
        /// </summary>
        internal const string REQUEST_REGISTER_DRAGGABLE_UI = "Touch/requestRegisterDraggableUI";
    }
}
