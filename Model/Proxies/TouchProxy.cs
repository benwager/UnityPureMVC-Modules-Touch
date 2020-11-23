using PureMVC.Patterns.Proxy;
using UnityEngine;
using UnityPureMVC.Core.Libraries.UnityLib.Utilities.Logging;
using UnityPureMVC.Modules.Touch.Model.Enums;
using UnityPureMVC.Modules.Touch.View.Components;

namespace UnityPureMVC.Modules.Touch.Model.VO
{
    internal class TouchProxy : Proxy
    {
        new internal const string NAME = "TouchProxy";

        internal TouchVO TouchVO { get { return Data as TouchVO; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:UnityPureMVC.Modules.Touch.Model.VO.TouchProxy"/> class.
        /// </summary>
        internal TouchProxy() : base(NAME)
        {
            DebugLogger.Log("{0}::__Contstruct", NAME);
            Data = new TouchVO();
        }

        internal TouchEngine TouchEngine
        {
            get
            {
                return TouchVO.touchEngine;
            }
            set
            {
                TouchVO.touchEngine = value;
            }
        }

        internal GameObject TouchEngineGameObject
        {
            get
            {
                return TouchVO.touchEngineGameObject;
            }
            set
            {
                TouchVO.touchEngineGameObject = value;
            }

        }

        internal ITouchEngineComponent TouchEngineComponent
        {
            get
            {
                return TouchVO.touchEngineComponent;
            }
            set
            {
                TouchVO.touchEngineComponent = value;
            }
        }
    }
}